using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Account;
using CleanArchitecture.Account.Data;
using CleanArchitecture.Account.Data.Identity;
using CleanArchitecture.Account.Data.IdentityProvider;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("oidc.json", optional: true, reloadOnChange: true);

// Add services to the container.

if (builder.Configuration.GetValue<bool>("UseInMemoryDatabase"))
{
    builder.Services.AddDbContext<IdentityDbContext>(options => {
        options.UseInMemoryDatabase("Identity");
    });

    builder.Services.AddDbContext<IdentityProviderDbContext>(options => {
        options.UseInMemoryDatabase("IdentityProvider");
        options.UseOpenIddict();
    });
}
else
{
    builder.Services.AddDbContext<IdentityDbContext>(options => {
        options.UseSqlServer(builder.Configuration.GetConnectionString("Identity"),
            builder => builder.MigrationsAssembly(typeof(IdentityDbContext).Assembly.FullName));
    });

    builder.Services.AddDbContext<IdentityProviderDbContext>(options => {
        options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityProvider"),
            builder => builder.MigrationsAssembly(typeof(IdentityProviderDbContext).Assembly.FullName));
        options.UseOpenIddict();
    });
}

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<IdentityDbContext>();

builder.Services.AddScoped<IDataInitialiser, IdentityDbContextInitialiser>();
builder.Services.AddScoped<IDataInitialiser, IdentityProviderDbContextInitialiser>();

builder.Services.AddOpenIddictServer();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();

    // Initialise and seed database
    using (var scope = app.Services.CreateScope())
    {
        var initialisers = scope.ServiceProvider.GetServices<IDataInitialiser>();

        foreach (var initialiser in initialisers)
        {
            await initialiser.InitialiseAsync();
            await initialiser.SeedAsync();
        }
    }
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

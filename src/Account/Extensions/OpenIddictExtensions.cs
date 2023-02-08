using CleanArchitecture.Account.Data.IdentityProvider;

using static OpenIddict.Abstractions.OpenIddictConstants;

namespace CleanArchitecture.Account;

public static class OpenIddictExtensions
{
    public static IServiceCollection AddOpenIddictServer(this IServiceCollection services)
    {
        services.AddOpenIddict()
            .AddCore(options =>
            {
                options.UseEntityFrameworkCore()
                       .UseDbContext<IdentityProviderDbContext>();

            })
            .AddServer(options =>
            {
                options.DisableAccessTokenEncryption();

                options.SetAuthorizationEndpointUris("connect/authorize")
                       .SetDeviceEndpointUris("connect/device")
                       .SetIntrospectionEndpointUris("connect/introspect")
                       .SetLogoutEndpointUris("connect/logout")
                       .SetTokenEndpointUris("connect/token")
                       .SetUserinfoEndpointUris("connect/userinfo")
                       .SetVerificationEndpointUris("connect/verify");

                options.AllowAuthorizationCodeFlow()
                       .AllowDeviceCodeFlow()
                       .AllowPasswordFlow()
                       .AllowRefreshTokenFlow();

                options.RegisterScopes(Scopes.Email, Scopes.Profile, Scopes.Roles);

                options.AddDevelopmentEncryptionCertificate()
                       .AddDevelopmentSigningCertificate();

                options.RequireProofKeyForCodeExchange();

                options.UseAspNetCore()
                       .EnableStatusCodePagesIntegration()
                       .EnableAuthorizationEndpointPassthrough()
                       .EnableLogoutEndpointPassthrough()
                       .EnableTokenEndpointPassthrough()
                       .EnableUserinfoEndpointPassthrough()
                       .EnableVerificationEndpointPassthrough();
            })
            .AddValidation(options =>
            {
                options.AddAudiences("CleanArchitecture_API");

                options.UseLocalServer();

                options.UseAspNetCore();
            });

        services.AddCors(options =>
            options.AddDefaultPolicy(policy =>
                policy
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin()));

        return services;
    }
}

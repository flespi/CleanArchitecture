using System.Security.Claims;
using Balea.EntityFrameworkCore.Store.Entities;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Infrastructure.Persistence.Authorization;
using Duende.IdentityServer.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Services;

public class PrincipalService : IPrincipalService
{
    private readonly AuthorizationDbContext _context;

    public PrincipalService(AuthorizationDbContext context)
    {
        _context = context;
    }

    public async Task CreateOrUpdateAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default)
    {
        var name = principal.GetDisplayName();
        var sub = principal.GetSubjectId();

        var subject = await _context.Subjects.FirstOrDefaultAsync(x => x.Sub == sub, cancellationToken);

        if (subject is null)
        {
            subject = new SubjectEntity(name, sub);
            _context.Subjects.Add(subject);
        }
        else
        {
            subject.Name = name;
            _context.Subjects.Update(subject);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}

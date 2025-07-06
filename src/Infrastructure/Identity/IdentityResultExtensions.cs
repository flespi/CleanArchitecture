using CleanArchitecture.Application.Common.Models;
using Orca;

namespace CleanArchitecture.Infrastructure.Orca;

public static class IdentityResultExtensions
{
    public static Result ToApplicationResult(this AccessManagementResult result)
    {
        return result.Succeeded
            ? Result.Success()
            : Result.Failure(result.Errors.Select(e => e.Description));
    }
}

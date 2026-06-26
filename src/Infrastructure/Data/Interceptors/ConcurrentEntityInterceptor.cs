using CleanArchitecture.Application.Common.Exceptions;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CleanArchitecture.Infrastructure.Data.Interceptors;

public class ConcurrentEntityInterceptor : SaveChangesInterceptor
{
    public override void SaveChangesFailed(DbContextErrorEventData eventData)
    {
        if (eventData.Exception.InnerException is ConcurrencyException concurrencyException)
        {
            throw concurrencyException;
        }
    }

    public override Task SaveChangesFailedAsync(DbContextErrorEventData eventData, CancellationToken cancellationToken = default)
    {
        if (eventData.Exception.InnerException is ConcurrencyException concurrencyException)
        {
            throw concurrencyException;
        }

        return Task.CompletedTask;
    }

    public override InterceptionResult ThrowingConcurrencyException(ConcurrencyExceptionEventData eventData, InterceptionResult result)
    {
        if (result.IsSuppressed) return result;

        throw new ConcurrencyException(eventData.Exception);
    }

    public override ValueTask<InterceptionResult> ThrowingConcurrencyExceptionAsync(ConcurrencyExceptionEventData eventData, InterceptionResult result, CancellationToken cancellationToken = default)
    {
        if (result.IsSuppressed) return ValueTask.FromResult(result);

        throw new ConcurrencyException(eventData.Exception);
    }
}

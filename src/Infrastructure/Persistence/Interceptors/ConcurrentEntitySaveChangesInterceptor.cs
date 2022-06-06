using CleanArchitecture.Application.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CleanArchitecture.Infrastructure.Persistence.Interceptors;

public class ConcurrentEntitySaveChangesInterceptor : SaveChangesInterceptor
{
    public override void SaveChangesFailed(DbContextErrorEventData eventData)
    {
        HandleException(eventData.Exception);

        base.SaveChangesFailed(eventData);
    }

    public override Task SaveChangesFailedAsync(DbContextErrorEventData eventData, CancellationToken cancellationToken = default)
    {
        HandleException(eventData.Exception);

        return base.SaveChangesFailedAsync(eventData, cancellationToken);
    }

    private static void HandleException(Exception exception)
    {
        if (exception is not DbUpdateConcurrencyException concurrencyException) return;

        throw new ConcurrencyException(concurrencyException);
    }
}

using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Domain.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CleanArchitecture.Infrastructure.Persistence.Interceptors;

public class IdempotentEntitySaveChangesInterceptor : SaveChangesInterceptor
{
    public override void SaveChangesFailed(DbContextErrorEventData eventData)
    {
        HandleException(eventData.Exception);

        base.SaveChangesFailed(eventData);
    }

    public override Task SaveChangesFailedAsync(DbContextErrorEventData eventData, CancellationToken cancellationToken = default)
    {
        HandleException(eventData.Exception);

        return base.SaveChangesFailedAsync(eventData);
    }

    private static void HandleException(Exception exception)
    {
        const int sqlErrorCode = 2601;

        if (exception is not DbUpdateException updateException) return;
        if (updateException.InnerException is not SqlException sqlException) return;
        
        if (sqlException.Number == sqlErrorCode && sqlException.Message.Contains(nameof(IIdempotentEntity.IdempotencyKey)))
        {
            throw new IdempotencyException(updateException);
        }
    }
}

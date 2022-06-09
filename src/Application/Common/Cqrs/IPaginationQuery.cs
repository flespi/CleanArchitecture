namespace CleanArchitecture.Application.Common.Cqrs;

public interface IPaginationQuery
{
    int PageNumber { get; init; }

    int PageSize { get; init; }
}

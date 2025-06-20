namespace CleanArchitecture.Application.Common.Models;

public class UserData<TData>
{
    public required string? UserId { get; init; }

    public required TData Data { get; init; }
}

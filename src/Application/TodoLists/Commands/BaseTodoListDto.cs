namespace CleanArchitecture.Application.TodoLists.Commands;

public record BaseTodoListDto
{
    public string? Title { get; init; }
}

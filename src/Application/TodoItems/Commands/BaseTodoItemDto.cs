namespace CleanArchitecture.Application.TodoItems.Commands;

public record BaseTodoItemDto
{
    public string? Title { get; init; }
}

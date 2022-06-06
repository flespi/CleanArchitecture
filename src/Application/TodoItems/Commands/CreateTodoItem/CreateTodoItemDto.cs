namespace CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;

public record CreateTodoItemDto : BaseTodoItemDto
{
    public int ListId { get; init; }
}

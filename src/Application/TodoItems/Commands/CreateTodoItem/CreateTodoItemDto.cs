namespace CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;

public record CreateTodoItemDto : BaseTodoItemDto
{
    public Guid ListId { get; init; }
}

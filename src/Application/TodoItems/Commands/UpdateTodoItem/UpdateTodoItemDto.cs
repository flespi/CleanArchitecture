namespace CleanArchitecture.Application.TodoItems.Commands.UpdateTodoItem;

public record UpdateTodoItemDto : BaseTodoItemDto
{
    public bool Done { get; init; }
}

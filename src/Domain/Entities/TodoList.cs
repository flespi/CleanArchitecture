namespace CleanArchitecture.Domain.Entities;

public class TodoList : BaseAuditableEntity, IIdempotentEntity
{
    public string? Title { get; set; }

    public Colour Colour { get; set; } = Colour.White;

    public IList<TodoItem> Items { get; private set; } = new List<TodoItem>();

    public Guid IdempotencyKey { get; set; } = Guid.NewGuid();
}

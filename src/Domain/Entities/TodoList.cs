namespace CleanArchitecture.Domain.Entities;

public class TodoList : BaseEntity, IAuditableEntity
{
    public string? Title { get; set; }

    public Colour Colour { get; set; } = Colour.White;

    public IList<TodoItem> Items { get; private set; } = new List<TodoItem>();

    public Auditability Audit { get; set; } = null!;
}

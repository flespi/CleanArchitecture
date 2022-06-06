namespace CleanArchitecture.Domain.Entities;

public class TodoList : BaseAuditableEntity, IConcurrentEntity
{
    public string? Title { get; set; }

    public Colour Colour { get; set; } = Colour.White;

    public IList<TodoItem> Items { get; private set; } = new List<TodoItem>();

    public byte[]? ConcurrencyToken { get; set; }
}

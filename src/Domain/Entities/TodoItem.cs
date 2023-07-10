﻿namespace CleanArchitecture.Domain.Entities;

public class TodoItem : BaseEntity, IAuditableEntity, IDeletableEntity, IConcurrentEntity
{
    public Guid ListId { get; set; }

    public string? Title { get; set; }

    public string? Note { get; set; }

    public PriorityLevel Priority { get; set; }

    public DateTime? Reminder { get; set; }

    private bool _done;
    public bool Done
    {
        get => _done;
        set
        {
            if (value && !_done)
            {
                AddDomainEvent(new TodoItemCompletedEvent(this));
            }

            _done = value;
        }
    }

    public TodoList List { get; set; } = null!;

    public Auditability Audit { get; set; } = null!;

    public byte[]? ConcurrencyToken { get; set; }

    public bool IsDeleted { get; set; }
}

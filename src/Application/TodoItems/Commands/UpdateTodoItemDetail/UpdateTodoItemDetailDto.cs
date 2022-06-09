﻿using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Application.TodoItems.Commands.UpdateTodoItemDetail;

public record UpdateTodoItemDetailDto
{
    public Guid ListId { get; init; }

    public PriorityLevel Priority { get; init; }

    public string? Note { get; init; }
}

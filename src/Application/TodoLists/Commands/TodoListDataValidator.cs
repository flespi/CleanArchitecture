﻿using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Validations;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.TodoLists.Commands;

public class TodoListDataValidator : DataValidator<TodoListData>
{
    private readonly IApplicationDbContext _context;

    public TodoListDataValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.")
            .MustAsync(BeUniqueTitle).WithMessage("The specified title already exists.");
    }

    public async Task<bool> BeUniqueTitle(TodoListData model, string title, CancellationToken cancellationToken)
    {
        return await _context.TodoLists
            .Where(l => l.Id != Options.Id)
            .AllAsync(l => l.Title != title, cancellationToken);
    }
}

using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Validations;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Application.TodoLists.Commands;

public class BaseTodoListDtoValidator : DataValidator<BaseTodoListDto>
{
    private readonly IApplicationDbContext _context;

    public BaseTodoListDtoValidator(
        IApplicationDbContext context,
        IStringLocalizer<ValidationMessages> localizer)
    {
        _context = context;

        RuleFor(v => v.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.")
            .MustAsync(BeUniqueTitle).WithMessage(localizer["The field must be unique."]);
    }

    public async Task<bool> BeUniqueTitle(BaseTodoListDto model, string title, CancellationToken cancellationToken)
    {
        return await _context.TodoLists
            .Where(l => l.Id != Options.Id)
            .AllAsync(l => l.Title != title, cancellationToken);
    }
}

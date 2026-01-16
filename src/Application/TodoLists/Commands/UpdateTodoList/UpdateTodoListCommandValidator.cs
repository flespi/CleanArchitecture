using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Resources;

namespace CleanArchitecture.Application.TodoLists.Commands.UpdateTodoList;

public class UpdateTodoListCommandValidator : AbstractValidator<UpdateTodoListCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateTodoListCommandValidator(
        IApplicationDbContext context,
        IStringLocalizer<ValidationMessages> localizer)
    {
        _context = context;

        RuleFor(v => v.Title)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(BeUniqueTitle)
                .WithMessage(localizer["Unique_Field"])
                .WithErrorCode("Unique");
    }

    public async Task<bool> BeUniqueTitle(UpdateTodoListCommand model, string title, CancellationToken cancellationToken)
    {
        return !await _context.TodoLists
            .Where(l => l.Id != model.Id)
            .AnyAsync(l => l.Title == title, cancellationToken);
    }
}

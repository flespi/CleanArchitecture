using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Validations;
namespace CleanArchitecture.Application.TodoLists.Commands;

public class BaseTodoListDtoValidator : DataValidator<BaseTodoListDto>
{
    private readonly IApplicationDbContext _context;

    public BaseTodoListDtoValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Title)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(BeUniqueTitle)
                .WithMessage("'{PropertyName}' must be unique.")
                .WithErrorCode("Unique");
    }

    public async Task<bool> BeUniqueTitle(BaseTodoListDto model, string title, CancellationToken cancellationToken)
    {
        return !await _context.TodoLists
            .Where(l => l.Id != Options.Id)
            .AllAsync(l => l.Title == title, cancellationToken);
    }
}

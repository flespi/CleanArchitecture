using CleanArchitecture.Application.Common.Validations;
using FluentValidation;

namespace CleanArchitecture.Application.TodoItems.Commands;

public class BaseTodoItemDtoValidator : DataValidator<BaseTodoItemDto>
{
    public BaseTodoItemDtoValidator()
    {
        RuleFor(v => v.Title)
            .MaximumLength(200)
            .NotEmpty();
    }
}

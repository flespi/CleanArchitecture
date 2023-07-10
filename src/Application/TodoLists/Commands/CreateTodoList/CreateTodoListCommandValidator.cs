using CleanArchitecture.Application.Common.Validations;
using FluentValidation;

namespace CleanArchitecture.Application.TodoLists.Commands.CreateTodoList;

public class CreateTodoListCommandValidator : AbstractValidator<CreateTodoListCommand>
{
    public CreateTodoListCommandValidator(IValidatorFactory validatorFactory)
    {
        RuleFor(x => x.Data)
            .NotNull()
            .SetValidator(x => validatorFactory.GetValidator<BaseTodoListDto>());
    }
}

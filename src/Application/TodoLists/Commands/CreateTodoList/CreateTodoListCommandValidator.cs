using CleanArchitecture.Application.Common.Validations;

namespace CleanArchitecture.Application.TodoLists.Commands.CreateTodoList;

public class CreateTodoListCommandValidator : AbstractValidator<CreateTodoListCommand>
{
    public CreateTodoListCommandValidator(Common.Validations.IValidatorFactory validatorFactory)
    {
        RuleFor(x => x.Data!)
            .NotNull()
            .SetValidator(x => validatorFactory.GetValidator<BaseTodoListDto>());
    }
}

using CleanArchitecture.Application.Common.Validations;
using FluentValidation;

namespace CleanArchitecture.Application.TodoLists.Commands.UpdateTodoList;

public class UpdateTodoListCommandValidator : AbstractValidator<UpdateTodoListCommand>
{
    public UpdateTodoListCommandValidator(IValidatorFactory validatorFactory)
    {
        RuleFor(x => x.Data!)
            .NotNull()
            .SetValidator(x => validatorFactory.GetValidator<BaseTodoListDto>().ForEntity(x.Id));
    }
}

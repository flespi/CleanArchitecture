using CleanArchitecture.Application.Common.Validations;

namespace CleanArchitecture.Application.TodoLists.Commands.UpdateTodoList;

public class UpdateTodoListCommandValidator : AbstractValidator<UpdateTodoListCommand>
{
    public UpdateTodoListCommandValidator(Common.Validations.IValidatorFactory validatorFactory)
    {
        RuleFor(x => x.Data!)
            .NotNull()
            .SetValidator(x => validatorFactory.GetValidator<BaseTodoListDto>().UseEntity(x.Id));
    }
}

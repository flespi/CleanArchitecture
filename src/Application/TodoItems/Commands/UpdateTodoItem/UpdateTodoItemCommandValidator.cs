namespace CleanArchitecture.Application.TodoItems.Commands.UpdateTodoItem;

public class UpdateTodoItemCommandValidator : AbstractValidator<UpdateTodoItemCommand>
{
    public UpdateTodoItemCommandValidator(Common.Validations.IValidatorFactory validatorFactory)
    {
        RuleFor(x => x.Data!)
            .NotNull()
            .SetValidator(x => validatorFactory.GetValidator<BaseTodoItemDto>());
    }
}

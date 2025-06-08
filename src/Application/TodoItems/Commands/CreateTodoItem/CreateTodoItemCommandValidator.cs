namespace CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;

public class CreateTodoItemCommandValidator : AbstractValidator<CreateTodoItemCommand>
{
    public CreateTodoItemCommandValidator(Common.Validations.IValidatorFactory validatorFactory)
    {
        RuleFor(x => x.Data!)
            .NotNull()
            .SetValidator(x => validatorFactory.GetValidator<BaseTodoItemDto>());
    }
}

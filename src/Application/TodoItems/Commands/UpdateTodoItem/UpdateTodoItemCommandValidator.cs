using FluentValidation;

namespace CleanArchitecture.Application.TodoItems.Commands.UpdateTodoItem;

public class UpdateTodoItemCommandValidator : AbstractValidator<UpdateTodoItemCommand>
{
    public UpdateTodoItemCommandValidator(IValidatorFactory validatorFactory)
    {
        RuleFor(x => x.Data)
            .NotNull()
            .SetValidator(x => validatorFactory.GetValidator<BaseTodoItemDto>());
    }
}

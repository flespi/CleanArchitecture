using FluentValidation;

namespace CleanArchitecture.Application.Common.Validations;

public static class ValidatorExtensions
{
    public static IValidator<T> ForEntity<T>(this IValidator<T> validator, int id)
    {
        if (validator is DataValidator<T> dataValidator)
        {
            dataValidator.Options.Id = id;
        }

        return validator;
    }
}

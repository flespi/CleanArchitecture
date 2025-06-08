using FluentValidation;

namespace CleanArchitecture.Application.Common.Validations;

public static class ValidatorExtensions
{
    public static IValidator<T> UseEntity<T>(this IValidator<T> validator, int id)
    {
        if (validator is DataValidator<T> dataValidator)
        {
            dataValidator.Options.Id = id;
        }

        return validator;
    }
}

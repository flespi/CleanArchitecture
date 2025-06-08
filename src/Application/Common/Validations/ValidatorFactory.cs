using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Application.Common.Validations;

/// <summary>
/// Factory for creating validators
/// </summary>
public class ValidatorFactory : IValidatorFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ValidatorFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Gets the validator for the specified type.
    /// </summary>
    public IValidator<T> GetValidator<T>()
    {
        return _serviceProvider.GetRequiredService<IValidator<T>>();
    }

    /// <summary>
    /// Gets the validator for the specified type.
    /// </summary>
    public IValidator GetValidator(Type type)
    {
        var genericType = typeof(IValidator<>).MakeGenericType(type);
        return (IValidator)_serviceProvider.GetRequiredService(genericType);
    }
}

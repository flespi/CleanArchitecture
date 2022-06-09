using FluentValidation;

namespace CleanArchitecture.Application.Common.Validations;

public abstract class DataValidator<T> : AbstractValidator<T>
{
    public DataValidatorOptions Options { get; } = new DataValidatorOptions();
}

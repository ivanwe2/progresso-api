using FluentValidation;

namespace Prime.Progreso.Domain.Abstractions.Factories
{
    public interface IValidatorFactory
    {
        IValidator<T> GetValidator<T>();
    }
}

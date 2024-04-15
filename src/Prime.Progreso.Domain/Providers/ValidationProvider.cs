using FluentValidation;
using FluentValidation.Results;
using Prime.Progreso.Domain.Abstractions.Factories;
using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Exceptions;
using Prime.Progreso.Domain.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IValidatorFactory = Prime.Progreso.Domain.Abstractions.Factories.IValidatorFactory;
using ValidationException = Prime.Progreso.Domain.Exceptions.ValidationException;

namespace Prime.Progreso.Domain.Providers
{
    public class ValidationProvider : IValidationProvider
    {
        private IValidatorFactory _validatorFactory;

        public ValidationProvider(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
        }

        public void TryValidate<TDto>(TDto dto)
        {
            var result = TryGetValidator<TDto>().Validate(dto);

            ThrowIfInvalid(result);
        }

        public async Task TryValidateAsync<TDto>(TDto dto)
        {
            var result = await TryGetValidator<TDto>().ValidateAsync(dto);

            ThrowIfInvalid(result);
        }

        private void ThrowIfInvalid(ValidationResult result)
        {
            if (!result.IsValid)
            {
                var message = string.Join(";  ", result.Errors.Select(x => x.ErrorMessage));
                throw new ValidationException(message);
            }
        }

        private IValidator<TDto> TryGetValidator<TDto>()
        {
            var dtoTypeValidator = _validatorFactory.GetValidator<TDto>();
            if (dtoTypeValidator is null)
            {
                throw new NoExistingValidatorForGivenTypeException(typeof(TDto));
            }
            return dtoTypeValidator;
        }
    }
}

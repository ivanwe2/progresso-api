using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prime.Progreso.Domain.Exceptions
{
    public sealed class NoExistingValidatorForGivenTypeException : Exception
    {
        public NoExistingValidatorForGivenTypeException(Type dtoType)
            : base($"Requested validator for type: {dtoType}, was not found!")
        {
        }
    }
}

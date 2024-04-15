using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prime.Progreso.Domain.Exceptions
{
    public class InvalidHeaderException : Exception
    {
        public InvalidHeaderException(string message) 
            :base(message)
        { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prime.Progreso.Domain.Exceptions
{
    public class InvalidFileNameException : Exception
    {
        public InvalidFileNameException() : base("File name is invalid or already exists!") { }
    }
}

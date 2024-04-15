using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prime.Progreso.Domain.Exceptions
{
    public class FileIsNullException : Exception
    {
        public FileIsNullException() : base("File given was null!")
        { }    
    }
}

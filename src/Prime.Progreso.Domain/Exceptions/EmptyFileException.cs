using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prime.Progreso.Domain.Exceptions
{
    public class EmptyFileException : Exception
    {
        public EmptyFileException()
            :base("File passed cannot be empty or null!")
        { 

        }
    }
}

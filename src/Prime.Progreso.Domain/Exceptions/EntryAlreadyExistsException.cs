using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prime.Progreso.Domain.Exceptions
{
    public class EntryAlreadyExistsException : Exception
    {
        public EntryAlreadyExistsException(string message) 
            : base(message)
        {          
        }
    }
}

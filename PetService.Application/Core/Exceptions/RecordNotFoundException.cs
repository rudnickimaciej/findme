using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetService.Application.Core.Exceptions
{
    public sealed class RecordNotFoundException : Exception
    {
        public RecordNotFoundException(string msg) : base("Record not found")
        {
        }
    }
}

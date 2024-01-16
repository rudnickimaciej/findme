using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetService.Application.Core.Exceptions
{
    public sealed class ServerErrorException : Exception
    {
        public ServerErrorException(string msg) : base("Server error.")
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiAnimes.Infra.Exceptions
{
    public class DataAcessExceptions : Exception
    {
        public DataAcessExceptions(string message, Exception innerException) : base(message, innerException)
        {
        }
        public DataAcessExceptions(string message) : base(message)
        {
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace SpyStore.DAL.Exceptions
{
    public class InvalidQuantityException:Exception
    {
        public InvalidQuantityException() : base() { }
        public InvalidQuantityException(string message) : base(message) { }
        public InvalidQuantityException(string message, Exception innerException) : base(message, innerException) { }
    }
}

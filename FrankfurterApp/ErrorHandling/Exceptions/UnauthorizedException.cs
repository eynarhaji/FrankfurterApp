using System;

namespace FrankfurterApp.ErrorHandling.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public Exception InternalException;
        
        public UnauthorizedException(Exception internalException)
        {
            InternalException = internalException;
        }
    }
}
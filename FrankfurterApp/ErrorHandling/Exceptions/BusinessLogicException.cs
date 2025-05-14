using System;

namespace FrankfurterApp.ErrorHandling.Exceptions
{
    public class BusinessLogicException : Exception
    {
        public readonly object Argument;
        public readonly string Key;

        public BusinessLogicException(string message, object argument = null, string key = "Error") : base(message)
        {
            Argument = argument;
            Key = key;
        }
    }
}
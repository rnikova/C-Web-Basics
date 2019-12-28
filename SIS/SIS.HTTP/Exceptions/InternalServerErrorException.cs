using System;

namespace SIS.HTTP.Exceptions
{
    public class InternalServerErrorException : Exception
    {
        private const string InternalServerExceptionDefaultMessage = "The Server has encountered an error.";

        public InternalServerErrorException()
            : this(InternalServerExceptionDefaultMessage)
        {
        }

        public InternalServerErrorException(string name)
            : base(name)
        {
        }
    }
}

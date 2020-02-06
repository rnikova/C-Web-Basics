using System;

namespace SIS.SandBox
{
    public abstract class ValidationSisAttribute : Attribute
    {
        protected ValidationSisAttribute(string errorMessage)
        {
            this.ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; }

        public abstract bool IsValid(object value);
    }
}

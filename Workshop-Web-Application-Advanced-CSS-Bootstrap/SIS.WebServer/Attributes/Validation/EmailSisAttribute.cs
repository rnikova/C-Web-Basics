﻿using System;
using System.Text.RegularExpressions;

namespace SIS.MvcFramework.Attributes.Validation
{
    public class EmailSisAttribute : ValidationSisAttribute
    {
        public EmailSisAttribute()
        {
        }

        public EmailSisAttribute(string errorMessage)
            : base(errorMessage)
        {
        }

        public override bool IsValid(object value)
        {
            string valueAsString = (string)Convert.ChangeType(value, typeof(string));

            return Regex
                .IsMatch(valueAsString, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
        }
    }
}

﻿using SIS.HTTP.Enums;
using System;

namespace SIS.MvcFramework.Attributes
{
    public class HttpGetAttribute : BaseHttpAttribute
    {
        public override HttpRequestMethod Method => HttpRequestMethod.Get;
    }
}

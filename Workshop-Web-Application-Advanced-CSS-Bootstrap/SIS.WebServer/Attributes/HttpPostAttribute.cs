using SIS.HTTP.Enums;
using System;

namespace SIS.MvcFramework.Attributes
{
    public class HttpPostAttribute : BaseHttpAttribute
    {
        public override HttpRequestMethod Method => HttpRequestMethod.Post;
    }
}

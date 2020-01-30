using SIS.HTTP.Cookies;
using SIS.HTTP.Enums;
using SIS.HTTP.Headers;
using SIS.HTTP.Responses;

namespace SIS.MvcFramework.Result
{
    public abstract class ActionResult : HttpResponse
    {
        protected ActionResult(HttpResponseStatusCode responseStatusCode)
            : base(responseStatusCode)
        {

        }
    }
}

using SIS.HTTP.Enums;
using SIS.HTTP.Headers;

namespace SIS.MvcFramework.Result
{
    public class FileResult : ActionResult
    {
        public FileResult(byte[] fileContent, HttpResponseStatusCode responseStatusCode = HttpResponseStatusCode.Ok) 
            : base(responseStatusCode)
        {
            this.Headers.AddHeader(new HttpHeader(HttpHeader.ContentLength, fileContent.Length.ToString()));
            this.Headers.AddHeader(new HttpHeader(HttpHeader.ContentDisposition, "attachment"));
            this.Content = fileContent;
        }
    }
}

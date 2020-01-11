using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;

namespace Demo.App.Controllers
{
    public class HomeController : BaseController
    {
        public IHttpResponse Home(IHttpRequest httpRequest)
        {
            this.HttpRequest = httpRequest;

            return this.View();
        }

        public IHttpResponse Login(IHttpRequest httpRequest)
        {
            httpRequest.Session.AddParameter("username", "Pesho");

            return this.Redirect("/");
        }
    }
}

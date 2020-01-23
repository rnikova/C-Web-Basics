using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.MvcFramework;
using SIS.MvcFramework.Result;

namespace IRunes.App.Controllers
{
    public class InfoController : Controller
    {
        public ActionResult Json(IHttpRequest httpRequest)
        {
            return Json(new 
            {
                Name = "Pesho",
                Age = 25,
                Ocupation = "bezraboten",
                Maried = false
            });
        }

        public IHttpResponse About(IHttpRequest request)
        {
            return this.View();
        }
    }
}

using SIS.HTTP.Enums;
using SIS.HTTP.Requests;
using SIS.MvcFramework.Extencions;
using SIS.MvcFramework.Identity;
using SIS.MvcFramework.Result;
using SIS.WebServer.Result;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SIS.MvcFramework
{
    public abstract class Controller
    {
        protected Dictionary<string, object> ViewData;

        protected Controller()
        {
            this.ViewData = new Dictionary<string, object>();
        }

        public Principal User => (Principal) this.Request.Session.GetParameter("principal");

        public IHttpRequest Request { get; set; }

        protected bool IsLoggedIn()
        {
            return this.Request.Session.ContainsParameter("principal");
        }

        protected void SignIn(string id, string username, string email)
        {
            this.Request.Session.AddParameter("pricipal", new Principal
            {
                Id = id,
                Username = username,
                Email = email
            });
        }
        
        protected void SingOut()
        {
            this.Request.Session.ClearParameters();
        }

        protected string ParseTemplate(string viewContent)
        {
            foreach (var param in this.ViewData)
            {
                viewContent = viewContent.Replace($"@Model.{param.Key}", param.Value.ToString());
            }

            return viewContent;
        }

        protected ActionResult View([CallerMemberName] string view = null)
        {
            string controllerName = this.GetType().Name.Replace("Controller", string.Empty);
            string viewName = view;

            string viewContent = System.IO.File.ReadAllText("Views/" + controllerName + "/" + viewName + ".html");

            viewContent = this.ParseTemplate(viewContent);

            HtmlResult htmlResult = new HtmlResult(viewContent, HttpResponseStatusCode.Ok);

            return htmlResult;
        }

        protected ActionResult Redirect(string url)
        {
            return new RedirectResult(url);
        }

        protected ActionResult Xml(object obj)
        {
            return new XmlResult(obj.ToXml());
        }
        
        protected ActionResult Json(object obj)
        {
            return new JsonResult(obj.ToJson());
        }
        
        protected ActionResult File(byte[] fileContent)
        {
            return new FileResult(fileContent);
        }
    }
}

﻿using SIS.MvcFramework;
using SIS.MvcFramework.Attributes.Http;
using SIS.MvcFramework.Result;

namespace IRunes.App.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet(Url = "/")]
        public ActionResult IndexSlash()
        {
            return Index();
        }

        public ActionResult Index()
        {
            if (this.IsLoggedIn())
            {
                this.ViewData["username"] = this.User.Username;

                return this.View("Home");
            }

            return this.View();
        }
    }
}

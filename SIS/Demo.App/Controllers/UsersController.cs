using Demo.Data;
using Demo.Models;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using System.Linq;

namespace Demo.App.Controllers
{
    public class UsersController : BaseController
    {
        public IHttpResponse Login(IHttpRequest httpRequest)
        {
            return this.View();
        }

        public IHttpResponse LoginConfirm(IHttpRequest httpRequest)
        {
            using (var context = new DemoDbContext())
            {
                var username = httpRequest.FormData["username"].ToString();
                var password = httpRequest.FormData["password"].ToString();

                User user = context.Users
                    .SingleOrDefault(u => u.Username == username && u.Password == password);

                if (user == null)
                {
                    return this.Redirect("/login");
                }

                httpRequest.Session.AddParameter(username, user.Username);
            }

            return this.Redirect("/home");
        }

        public IHttpResponse Register(IHttpRequest httpRequest)
        {
            return this.View();
        }

        public IHttpResponse RegisterConfirm(IHttpRequest httpRequest)
        {
            using (var context = new DemoDbContext())
            {
                var username = httpRequest.FormData["username"].ToString();
                var password = httpRequest.FormData["password"].ToString();
                var confirmPassword = httpRequest.FormData["confirmPassword"].ToString();

                if (password != confirmPassword)
                {
                    return this.Redirect("/register");
                }

                User user = new User()
                {
                    Username = username,
                    Password = password
                };

                context.Users.Add(user);
                context.SaveChanges();
            }

            return this.Redirect("/login");
        }

        public IHttpResponse Logout(IHttpRequest httpRequest)
        {
            httpRequest.Session.ClearParameters();

            return this.Redirect("/");
        }
    }
}

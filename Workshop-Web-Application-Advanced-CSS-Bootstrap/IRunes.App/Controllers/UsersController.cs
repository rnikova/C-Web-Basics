using System.Linq;
using System.Text;
using IRunes.Data;
using IRunes.Models;
using SIS.MvcFramework;
using SIS.MvcFramework.Result;
using System.Collections.Generic;
using System.Security.Cryptography;
using SIS.MvcFramework.Attributes.Http;
using SIS.MvcFramework.Attributes.Action;

namespace IRunes.App.Controllers
{
    public class UsersController : Controller
    {
        [NonAction]
        private string HashPassword(string password)
        {
            using (SHA256 sha256hash = SHA256.Create())
            {
                return Encoding.UTF8.GetString(sha256hash.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }
        }

        public ActionResult Login()
        {
            return this.View();
        }

        [HttpPost(ActionName = "Login")]
        public ActionResult LoginConfirm()
        {
            using (var context = new RunesDbContext())
            {
                string username = ((ISet<string>)this.Request.FormData["username"]).FirstOrDefault();
                string password = ((ISet<string>)this.Request.FormData["password"]).FirstOrDefault();

                User userFromDb = context.Users
                    .FirstOrDefault(user =>
                    (user.Username == username
                    || user.Email == username)
                    && user.Password == this.HashPassword(password));

                if (userFromDb == null)
                {
                    return this.Redirect("/Users/Login");
                }

                this.SignIn(userFromDb.Id, userFromDb.Username, userFromDb.Email);
            }

            return this.Redirect("/");
        }

        public ActionResult Register()
        {
            return this.View();
        }

        [HttpPost(ActionName = "Register")]
        public ActionResult RegisterConfirm()
        {
            using (var context = new RunesDbContext())
            {
                string username = ((ISet<string>)this.Request.FormData["username"]).FirstOrDefault();
                string password = ((ISet<string>)this.Request.FormData["password"]).FirstOrDefault();
                string confirmPassword = ((ISet<string>)this.Request.FormData["confirmPassword"]).FirstOrDefault();
                string email = ((ISet<string>)this.Request.FormData["email"]).FirstOrDefault();

                if (password != confirmPassword)
                {
                    return this.Redirect("/Users/Register");
                }

                User user = new User
                {
                    Username = username,
                    Password = this.HashPassword(password),
                    Email = email
                };

                context.Users.Add(user);
                context.SaveChanges();
            }

            return this.Redirect("/Users/Login");
        }

        public ActionResult Logout()
        {
            this.SingOut();

            return this.Redirect("/");
        }
    }
}
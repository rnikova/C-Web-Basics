using IRunes.Data;
using IRunes.Models;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace IRunes.App.Controllers
{
    public class UsersController : BaseController
    {
        private string HashPassword(string password)
        {
            using(SHA256 sHA256hash = SHA256.Create())
            {
                return Encoding.UTF8.GetString(sHA256hash.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }
        }

        public IHttpResponse Login(IHttpRequest request)
        {
            return this.View();
        }
        
        public IHttpResponse LoginConfirm(IHttpRequest request)
        {
            using(var context = new RunesDbContext())
            {
                string username = ((ISet<string>)request.FormData["username"]).FirstOrDefault();
                string password = ((ISet<string>)request.FormData["password"]).FirstOrDefault();

                User userFromDb = context.Users
                    .FirstOrDefault(user => 
                    (user.Username == username 
                    || user.Email == username) 
                    && user.Password ==  this.HashPassword(password));

                if (userFromDb == null)
                {
                    return this.Redirect("/Users/Login");
                }

                this.SingIn(request, userFromDb);
            }

            return this.Redirect("/");
        }
        
        public IHttpResponse Register(IHttpRequest request)
        {
            return this.View();
        }
        
        public IHttpResponse RegisterConfirm(IHttpRequest request)
        {
            using (var context = new RunesDbContext())
            {
                string username = ((ISet<string>)request.FormData["username"]).FirstOrDefault();
                string password = ((ISet<string>)request.FormData["password"]).FirstOrDefault();
                string confirmPassword = ((ISet<string>)request.FormData["confirmPassword"]).FirstOrDefault();
                string email = ((ISet<string>)request.FormData["email"]).FirstOrDefault();

                if (password != confirmPassword)
                {
                    return this.Redirect("/Users/Register");
                }

                User user = new User()
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
        
        public IHttpResponse Logout(IHttpRequest request)
        {
            //this.SingOut();

            return this.Redirect("/");
        }
    }
}
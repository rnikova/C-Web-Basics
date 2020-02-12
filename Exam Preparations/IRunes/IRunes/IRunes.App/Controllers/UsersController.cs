using IRunes.App.ViewModels.Users;
using IRunes.Services;
using SIS.HTTP;
using SIS.MvcFramework;

namespace IRunes.App.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public HttpResponse Login()
        {
            return this.View();
        }

        [HttpPost]
        public HttpResponse Login(LoginInputModel inputModel)
        {
            var userId = this.usersService.GetUserId(inputModel.Username, inputModel.Password);

            if (userId == null)
            {
                return this.Redirect("/Users/Login");
            }

            this.SignIn(userId);
            return this.Redirect("/");
        }

        public HttpResponse Register()
        {
            return this.View();
        }

        [HttpPost]
        public HttpResponse Register(RegisterInputModel inputModel)
        {
            if (string.IsNullOrWhiteSpace(inputModel.Email))
            {
                return this.Error("Email cannot be empty!");
            }

            if (inputModel.Password.Length < 6 || inputModel.Password.Length > 20)
            {
                return this.Error("Password must be at least 6 characters and at most 20");
            }

            if (inputModel.Username.Length < 4 || inputModel.Username.Length > 10)
            {
                return this.Error("Username must be at least 4 characters and at most 10");
            }

            if (inputModel.Password != inputModel.ConfirmPassword)
            {
                return this.Error("Password should match.");
            }

            if (this.usersService.EmailExists(inputModel.Email))
            {
                return this.Error("Email already in use.");
            }

            if (this.usersService.UsernameExists(inputModel.Username))
            {
                return this.Error("Username already in use.");
            }

            this.usersService.Register(inputModel.Username, inputModel.Email, inputModel.Password);
            return this.Redirect("/Users/Login");
        }

        public HttpResponse Logout()
        {
            this.SignOut();
            return this.Redirect("/");
        }
    }
}

using SIS.MvcFramework.Attributes.Validation;

namespace SULS.App.ViewModels.Users
{
    public class LoginInputModel
    {
        private const string ErrorMessage = "Invalid username or password!";

        [RequiredSis]
        public string Username { get; set; }
        
        [RequiredSis]
        public string Password { get; set; }
    }
}

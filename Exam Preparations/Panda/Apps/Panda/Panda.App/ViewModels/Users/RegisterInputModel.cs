using SIS.MvcFramework.Attributes.Validation;

namespace Panda.App.ViewModels.Users
{
    public class RegisterInputModel
    {
        private const string InvalidUsername = "Username must be between 5 and 20 characters";
        private const string InvalidPassword = "Password must be between 5 and 20 characters";
        private const string InvalidEmail = "Email must be between 5 and 20 characters";

        [RequiredSis]
        [StringLengthSis(5, 20, InvalidUsername)]
        public string Username { get; set; }

        [RequiredSis]
        [StringLengthSis(5, 20, InvalidEmail)]
        public string Email { get; set; }

        [RequiredSis]
        public string Password { get; set; }

        [RequiredSis]
        public string ConfirmPassword { get; set; }
    }
}

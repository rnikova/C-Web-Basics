using SIS.MvcFramework.Attributes.Validation;

namespace SULS.App.ViewModels.Users
{
    public class RegisterInputModel
    {
        private const string InvalidUsername = "Username should be between 5 and 20 characters";
        private const string InvalidPassword = "Password should be between 6 and 20 characters";

        [RequiredSis]
        [StringLengthSis(5, 20, InvalidUsername)]
        public string Username { get; set; }

        [RequiredSis]
        [EmailSis]
        public string Email { get; set; }

        [RequiredSis]
        [StringLengthSis(6, 20, InvalidPassword)]
        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }
}

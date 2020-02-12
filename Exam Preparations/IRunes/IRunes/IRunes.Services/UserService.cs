using IRunes.Data;
using IRunes.Models;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace IRunes.Services
{
    public class UserService : IUsersService
    {
        private readonly RunesDbContext context;

        public UserService(RunesDbContext context)
        {
            this.context = context;
        }

        public void Register(string username, string email, string password)
        {
            var user = new User
            {
                Username = username,
                Email = email,
                Password = this.Hash(password)
            };

            this.context.Users.Add(user);
            this.context.SaveChanges();
        }
     
        public string GetUserId(string username, string password)
        {
            var user = this.context.Users.FirstOrDefault(x => x.Username == username && x.Password == this.Hash(password));

            if (user == null)
            {
                return null;
            }

            return user.Id;
        }

        public string GetUsername(string id)
        {
            var username = this.context.Users
                .Where(x => x.Id == id)
                .Select(x => x.Username)
                .FirstOrDefault();

            return username;
        }

        public bool UsernameExists(string username)
        {
            return this.context.Users.Any(x => x.Username == username);
        }

        public bool EmailExists(string email)
        {
            return this.context.Users.Any(x => x.Email == email);
        }

        private string Hash(string input)
        {
            if (input == null)
            {
                return null;
            }

            var crypt = new SHA256Managed();
            var hash = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(input));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }

            return hash.ToString();
        }
    }
}

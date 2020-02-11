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

        public void CreateUser(string username, string email, string password)
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

        public string GetUsername(string id)
        {
            var username = this.context.Users
                .Where(x => x.Id == id)
                .Select(x => x.Username)
                .FirstOrDefault();

            return username;
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

using Panda.Data;
using Panda.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Panda.Services
{
    public class UsersService : IUsersService
    {
        private readonly PandaDbContext context;

        public UsersService(PandaDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<string> GetAllUsernames()
        {
            var users = this.context.Users.Select(x => x.Username).ToList();

            return users;
        }

        public void Create(string username, string email, string password)
        {
            var hashPassword = this.HashPassword(password);

            var user = new User
            {
                Username = username,
                Email = email,
                Password = hashPassword
            };

            this.context.Users.Add(user);
            this.context.SaveChanges();
        }

        public User GetUser(string username, string password)
        {
            var hashPassword = this.HashPassword(password);
            var user = this.context.Users.FirstOrDefault(x => x.Username == username && x.Password == hashPassword);

            return user;
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                return Encoding.UTF8.GetString(sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }
        }
    }
}

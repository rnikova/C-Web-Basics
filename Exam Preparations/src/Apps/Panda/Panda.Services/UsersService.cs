using Panda.Data;
using Panda.Data.Models;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Panda.Services
{
    public class UsersService : IUsersService
    {
        private readonly PandaDbContext dbContext;

        public UsersService(PandaDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public string CreateUser(string username, string email, string password)
        {
            var user = new User()
            {
                Username = username,
                Password = this.HashPassword(password),
                Email = email
            };

            this.dbContext.Users.Add(user);
            this.dbContext.SaveChanges();

            return user.Id;
        }

        public User GetUserOrNull(string username, string password)
        {
            var hashPassword = this.HashPassword(password);

            var user =  this.dbContext.Users.FirstOrDefault(x => x.Username == username && x.Password == hashPassword);

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

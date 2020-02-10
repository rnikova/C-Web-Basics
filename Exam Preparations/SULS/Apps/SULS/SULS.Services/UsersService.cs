using SULS.Data;
using SULS.Models;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SULS.Services
{
    public class UsersService : IUsersService
    {
        private readonly SULSDbContext dbContext;

        public UsersService(SULSDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public string CreateUser(string username, string email, string password)
        {
            var user = new User
            {
                Username = username,
                Email = email,
                Password = this.HashPassword(password),
            };

            this.dbContext.Users.Add(user);
            this.dbContext.SaveChanges();

            return user.Id;
        }

        public User GetUserOrNull(string username, string password)
        {
            var passwordHash = this.HashPassword(password);
            var user = this.dbContext.Users.FirstOrDefault(x => x.Username == username && x.Password == passwordHash);

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

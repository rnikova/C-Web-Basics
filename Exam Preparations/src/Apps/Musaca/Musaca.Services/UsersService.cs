using Musaca.Data;
using Musaca.Models;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Musaca.Services
{
    public class UsersService : IUsersService
    {
        private readonly MusacaDbContext context;

        public UsersService(MusacaDbContext dbContext)
        {
            this.context = dbContext;
        }

        public User CreateUser(User user)
        {
            user = this.context.Users.Add(user).Entity;
            this.context.SaveChanges();

            return user;
        }

        public User GetUserByUsernameAndPassword(string username, string password)
        {
            var hashPassword = this.HashPassword(password);

            return this.context.Users
                .SingleOrDefault(user => (user.Username == username) && user.Password == hashPassword);
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

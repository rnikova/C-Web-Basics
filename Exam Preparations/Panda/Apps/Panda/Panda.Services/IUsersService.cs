using Panda.Models;

namespace Panda.Services
{
    public interface IUsersService
    {
        void Create(string username, string email, string password);

        User GetUser(string username, string password);
    }
}

using Musaca.Models;

namespace Musaca.Services
{
    public interface IUsersService
    {
        User CreateUser(User user);

        User GetUserByUsernameAndPassword(string username, string password);
    }
}

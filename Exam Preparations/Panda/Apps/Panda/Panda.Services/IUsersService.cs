using Panda.Models;
using System.Collections.Generic;

namespace Panda.Services
{
    public interface IUsersService
    {
        void Create(string username, string email, string password);

        User GetUser(string username, string password);

        IEnumerable<string> GetAllUsernames();
    }
}

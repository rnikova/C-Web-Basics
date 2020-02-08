﻿using Panda.Data.Models;
using System.Collections.Generic;

namespace Panda.Services
{
    public interface IUsersService
    {
        string CreateUser(string username, string email, string password);

        User GetUserOrNull(string username, string password);

        IEnumerable<string> GetUsernames();
    }
}

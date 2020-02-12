namespace IRunes.Services
{
    public interface IUsersService
    {
        void Register(string username, string email, string password);

        bool UsernameExists(string username);

        bool EmailExists(string email);

        string GetUserId(string username, string password);

        string GetUsername(string id);
    }
}

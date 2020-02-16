namespace SharedTrip.Services.Users
{
    public interface IUsersService
    {
        bool EmailExists(string email);

        bool UsernameExists(string username);

        string GetUserId(string username, string password);

        string GetUsername(string id);

        void Register(string username, string email, string password);
    }
}

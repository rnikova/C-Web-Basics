namespace IRunes.Services
{
    public interface IUsersService
    {
        void CreateUser(string username, string email, string password);

        string GetUsername(string id);
    }
}

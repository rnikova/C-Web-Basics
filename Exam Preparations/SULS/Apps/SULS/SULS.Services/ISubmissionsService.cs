namespace SULS.Services
{
    public interface ISubmissionsService
    {
        void Create(string code, string problemId, string userId);

        void Delete(string id);
    }
}

using SULS.Models;
using System.Linq;

namespace SULS.Services
{
    public interface IProblemsService
    {
        void CreateProblem(string name, int points, string userId);

        IQueryable<Problem> GetAll();

        Problem GetProblemById(string id);
    }
}

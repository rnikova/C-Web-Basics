using Microsoft.EntityFrameworkCore;
using SULS.Data;
using SULS.Models;
using System.Linq;

namespace SULS.Services
{
    public class ProblemsService : IProblemsService
    {
        private readonly SulsDbContext dbContext;

        public ProblemsService(SulsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void CreateProblem(string name, int points, string userId)
        {
            var problem = new Problem
            {
                Name = name,
                Points = points,
                UserId = userId
            };

            dbContext.Problems.Add(problem);
            dbContext.SaveChanges();
        }

        public IQueryable<Problem> GetAll()
        {
            return this.dbContext.Problems;
        }

        public Problem GetProblemById(string id)
        {
            return this.dbContext.Problems.Include(p => p.Submissions).Include(p => p.User).SingleOrDefault(p => p.Id == id);
        }
    }
}

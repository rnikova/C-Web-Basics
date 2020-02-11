using SULS.Data;
using SULS.Models;
using System;

namespace SULS.Services
{
    public class SubmissionsService : ISubmissionsService
    {
        private readonly SulsDbContext dbContext;
        private readonly IProblemsService problemsService;
        private readonly Random random;

        public SubmissionsService(SulsDbContext dbContext, IProblemsService problemsService)
        {
            this.dbContext = dbContext;
            this.problemsService = problemsService;
        }

        public void Create(string code, string problemId, string userId)
        {
            var problem = this.problemsService.GetProblemById(problemId);

            var submission = new Submission
            {
                Code = code,
                AchievedResult = this.random.Next(0, problem.Points),
                CreatedOn = DateTime.UtcNow,
                ProblemId = problemId,
                UserId = userId
            };

            this.dbContext.Submissions.Add(submission);
            this.dbContext.SaveChanges();
        }

        public void Delete(string id)
        {
            var submission = this.dbContext.Submissions.Find(id);
            this.dbContext.Submissions.Remove(submission);
            this.dbContext.SaveChanges();
        }
    }
}

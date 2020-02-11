using SIS.MvcFramework;
using SIS.MvcFramework.Attributes;
using SIS.MvcFramework.Attributes.Security;
using SIS.MvcFramework.Result;
using SULS.App.ViewModels.Problems;
using SULS.App.ViewModels.Submissions;
using SULS.Services;
using System.Linq;

namespace SULS.App.Controllers
{
    public class ProblemsController : Controller
    {
        private readonly IProblemsService problemsService;

        public ProblemsController(IProblemsService problemsService)
        {
            this.problemsService = problemsService;
        }

        [Authorize]
        public IActionResult Create()
        {
            return this.View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(ProblemInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.Redirect("/Problems/Create");
            }

            this.problemsService.CreateProblem(model.Name, model.TotalPoints, model.UserId);

            return this.Redirect("/");
        }

        [Authorize]
        public IActionResult Details(string id)
        {
            var problem = this.problemsService.GetProblemById(id);

            var viewModel = new ProblemDetailsViewModel
            {
                Name = problem.Name,
                Submissions = problem.Submissions.Select(x => new SubmissionDetailsViewModel
                {
                    SubmissionId = x.Id,
                    Username = x.User.Username,
                    AchievedResult = x.AchievedResult,
                    MaxPoints = x.Problem.Points,
                    CreatedOn = x.CreatedOn,
                    ProblemId = x.ProblemId
                }).ToList()
            };

            return this.View(viewModel);
        }
    }
}

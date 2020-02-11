using SIS.MvcFramework;
using SIS.MvcFramework.Attributes;
using SIS.MvcFramework.Attributes.Security;
using SIS.MvcFramework.Result;
using SULS.App.ViewModels.Submissions;
using SULS.Data;
using SULS.Models;
using SULS.Services;
using System;

namespace SULS.App.Controllers
{
    public class SubmissionsController : Controller
    {
        private readonly ISubmissionsService submissionsService;
        private readonly IProblemsService problemsService;

        public SubmissionsController(ISubmissionsService submissionsService, IProblemsService problemsService)
        {
            this.submissionsService = submissionsService;
            this.problemsService = problemsService;
        }

        [Authorize]
        public IActionResult Create(string id)
        {
            var problem = this.problemsService.GetProblemById(id);
            var viewModel = new ProblemSubmissionViewModel
            {
                ProblemId = problem.Id,
                Name = problem.Name
            };

            return this.View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(SubmissionInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Redirect($"/Submissions/Create?id={inputModel.ProblemId}");
            }

            this.submissionsService.Create(inputModel.Code, inputModel.ProblemId, inputModel.UserId);

            return this.Redirect("/");
        }

        [Authorize]
        public IActionResult Delete(string id)
        {
            this.submissionsService.Delete(id);

            return this.Redirect("/");
        }
    }
}
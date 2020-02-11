using SIS.MvcFramework;
using SIS.MvcFramework.Attributes;
using SIS.MvcFramework.Result;
using SULS.App.ViewModels.Problems;
using SULS.Services;
using System.Linq;

namespace SULS.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProblemsService problemsService;

        public HomeController(ProblemsService problemsService)
        {
            this.problemsService = problemsService;
        }

        [HttpGet(Url = "/")]
        public IActionResult IndexSlash()
        {
            return this.Index();
        }

        public IActionResult Index()
        {
            if (this.IsLoggedIn())
            {
                var viewModel = this.problemsService.GetAll().Select(
                x => new ProblemViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Count = x.Submissions.Count
                }).ToList();

                return this.View(viewModel, "IndexLoggedIn");
            }

            return this.View();
        }
    }
}
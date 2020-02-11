using SULS.App.ViewModels.Submissions;
using System.Collections.Generic;

namespace SULS.App.ViewModels.Problems
{
    public class ProblemDetailsViewModel
    {
        public string Name { get; set; }

        public IEnumerable<SubmissionDetailsViewModel> Submissions { get; set; }
    }
}

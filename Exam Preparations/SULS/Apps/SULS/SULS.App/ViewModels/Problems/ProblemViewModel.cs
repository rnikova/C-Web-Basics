using SIS.MvcFramework.Attributes.Validation;

namespace SULS.App.ViewModels.Problems
{
    public class ProblemViewModel
    {
        private const string InvalidName = "Name should be between 5 and 20 characters";

        public string Id { get; set; }

        [RequiredSis]
        [StringLengthSis(5, 20, InvalidName)]
        public string Name { get; set; }

        public int Count { get; set; }
    }
}

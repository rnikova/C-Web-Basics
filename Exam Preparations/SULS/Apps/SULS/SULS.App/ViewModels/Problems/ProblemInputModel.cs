using SIS.MvcFramework.Attributes.Validation;

namespace SULS.App.ViewModels.Problems
{
    public class ProblemInputModel
    {
        private const string InvalidName = "Name should be between 5 and 20 characters";
        private const string InvalidPoints = "The points should be between 5 and 300";

        [RequiredSis]
        [StringLengthSis(5, 20, InvalidName)]
        public string Name { get; set; }

        [RangeSis(5, 300, InvalidPoints)]
        public int TotalPoints { get; set; }

        [RequiredSis]
        public string UserId { get; set; }
    }
}

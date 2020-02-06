using SIS.MvcFramework.Attributes.Validation;

namespace IRunes.App.ViewModels.Albums
{
    public class AlbumCreateInputModel
    {
        private const string NameErrorMessage = "Invalid length! Name must be between 3 and 30 symbols!";

        private const string CoverErrorMessage = "Invalid length! Cover must be between 5 and 255 symbols!";

        [RequiredSis]
        [StringLengthSis(3, 30, NameErrorMessage)]
        public string Name { get; set; }

        [RequiredSis]
        [StringLengthSis(5, 255, CoverErrorMessage)]
        public string Cover { get; set; }
    }
}

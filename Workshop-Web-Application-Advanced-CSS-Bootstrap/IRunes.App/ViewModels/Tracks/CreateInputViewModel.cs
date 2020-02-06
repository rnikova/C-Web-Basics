using SIS.MvcFramework.Attributes.Validation;

namespace IRunes.App.ViewModels.Tracks
{
    public class CreateInputViewModel
    {
        public string AlbumId { get; set; }

        [StringLengthSis(3, 20, "Error Message")]
        public string Name { get; set; }

        public string Link { get; set; }

        public decimal Price { get; set; }
    }
}

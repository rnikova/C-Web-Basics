using IRunes.App.ViewModels.Tracks;
using IRunes.Services;
using SIS.HTTP;
using SIS.MvcFramework;

namespace IRunes.App.Controllers
{
    public class TracksController : Controller
    {
        private readonly ITracksService tracksService;

        public TracksController(ITracksService tracksService)
        {
            this.tracksService = tracksService;
        }

        public HttpResponse Create()
        {
            return this.View();
        }

        [HttpPost]
        public HttpResponse Create(TrackCreateInputModel inputModel)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (inputModel.Name.Length < 4 || inputModel.Name.Length > 20)
            {
                return this.Error("Track name should be between 4 and 20 characters.");
            }

            if (!inputModel.Link.StartsWith("http"))
            {
                return this.Error("Invalid link.");
            }

            if (inputModel.Price < 0)
            {
                return this.Error("Price should be a positive number.");
            }

            this.tracksService.Create(inputModel.AlbumId, inputModel.Name, inputModel.Link, inputModel.Price);

            return this.Redirect("/Albums/Details?id=" + inputModel.AlbumId);
        }

        public HttpResponse Details(string trackId)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var track = this.tracksService.GetDetails(trackId);
            var viewModel = new TrackDetailsViewModel
            {
                Name = track.Name,
                Link = track.Link,
                AlbumId = track.AlbumId,
                Price = track.Price,
            };

            if (viewModel == null)
            {
                return this.Error("Track not found.");
            }

            return this.View(viewModel);
        }
    }
}

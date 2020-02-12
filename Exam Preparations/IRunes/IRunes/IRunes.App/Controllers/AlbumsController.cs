using IRunes.App.ViewModels.Albums;
using IRunes.App.ViewModels.Tracks;
using IRunes.Services;
using SIS.HTTP;
using SIS.MvcFramework;
using System.Linq;

namespace IRunes.App.Controllers
{
    public class AlbumsController : Controller
    {
        private readonly IAlbumsService albumsService;

        public AlbumsController(IAlbumsService albumsService)
        {
            this.albumsService = albumsService;
        }

        public HttpResponse Create()
        {
            return this.View();
        }

        [HttpPost]
        public HttpResponse Create(AlbumCreateInputModel inputModel)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (string.IsNullOrWhiteSpace(inputModel.Cover))
            {
                return this.Error("Cover is required!");
            }

            if (inputModel.Name.Length < 6 || inputModel.Name.Length > 20)
            {
                return this.Error("Name must be at least 6 characters and at most 20");
            }

            this.albumsService.Create(inputModel.Name, inputModel.Cover);

            return this.Redirect("/Albums/All");
        }

        public HttpResponse All()
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var viewModel = new AlbumAllViewModel
            {
                Albums = this.albumsService.GetAllAlbums(x => new AlbumInfoViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
            };

            return this.View(viewModel);
        }

        public HttpResponse Details(string id)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var album = this.albumsService.GetAlbumDetails(id);
            var viewModel = new AlbumDetailsViewModel
            {
                Id = album.Id,
                Name = album.Name,
                Cover = album.Cover,
                Price = album.Price,
                Tracks = album.Tracks.Select(x => new TrackInfoViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
            };

            return this.View(viewModel);
        }
    }
}

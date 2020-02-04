using System.Linq;
using IRunes.Models;
using IRunes.Services;
using SIS.MvcFramework;
using IRunes.App.ViewModels;
using SIS.MvcFramework.Result;
using SIS.MvcFramework.Mapping;
using System.Collections.Generic;
using SIS.MvcFramework.Attributes.Http;
using SIS.MvcFramework.Attributes.Security;

namespace IRunes.App.Controllers
{
    public class AlbumsController : Controller
    {
        private readonly IAlbumService albumService;

        public AlbumsController(IAlbumService albumService)
        {
            this.albumService = albumService;
        }

        [Authorize("user")]
        public ActionResult All()
        {
            ICollection<Album> allAlbums = this.albumService.GetAllAlbums();

            if (allAlbums.Count != 0)
            {
                return this.View(allAlbums.Select(album => ModelMapper.ProjectTo<AlbumAllViewModel>(album)).ToList());
            }

            return this.View();
        }

        [Authorize]
        public ActionResult Create()
        {
            return this.View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(string name, string cover)
        {
            Album album = new Album
            {
                Name = name,
                Cover = cover,
                Price = 0M
            };

            this.albumService.CreateAlbum(album);

            return this.Redirect("/Albums/All");
        }

        [Authorize]
        public ActionResult Details(string id)
        {
            Album albumFromDb = this.albumService.GetAlbumById(id);

            AlbumDetailsViewModel albumViewModel = ModelMapper.ProjectTo<AlbumDetailsViewModel>(albumFromDb);

            if (albumFromDb == null)
            {
                return this.Redirect("/Albums/All");
            }

            return this.View(new List<AlbumAllViewModel>());

        }
    }
}
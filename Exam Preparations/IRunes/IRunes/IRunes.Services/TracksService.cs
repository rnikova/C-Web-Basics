using IRunes.Data;
using IRunes.Models;
using System.Linq;

namespace IRunes.Services
{
    public class TracksService : ITracksService
    {
        private readonly RunesDbContext context;

        public TracksService(RunesDbContext context)
        {
            this.context = context;
        }

        public void Create(string albumId, string name, string link, decimal price)
        {
            var track = new Track
            {
                AlbumId = albumId,
                Name = name,
                Link = link,
                Price = price
            };

            this.context.Tracks.Add(track);

            var allTrackPricesSum = this.context.Tracks
                .Where(x => x.AlbumId == albumId)
                .Sum(x => x.Price) + price;

            var album = this.context.Albums.Find(albumId);

            album.Price = allTrackPricesSum * 0.87M;

            this.context.SaveChanges();
        }

        public Track GetDetails(string trackId)
        {
            var track = this.context.Tracks.Where(x => x.Id == trackId).FirstOrDefault();

            return track;
        }
    }
}

using IRunes.Data;
using IRunes.Models;
using System.Linq;

namespace IRunes.Services
{
    public class TrackService : ITrackService
    {
        private readonly RunesDbContext context;

        public TrackService()
        {
            this.context = new RunesDbContext();
        }

        public Track GetTrackById(string trackId)
        {
            return this.context.Tracks
                .SingleOrDefault(track => track.Id == trackId);
        }
    }
}
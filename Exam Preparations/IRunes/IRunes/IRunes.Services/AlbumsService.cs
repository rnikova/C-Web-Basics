using IRunes.Data;
using IRunes.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IRunes.Services
{
    public class AlbumsService : IAlbumsService
    {
        private readonly RunesDbContext context;

        public AlbumsService(RunesDbContext context)
        {
            this.context = context;
        }

        public void Create(string name, string cover)
        {
            var album = new Album
            {
                Name = name,
                Cover = cover,
                Price = 0.0M
            };

            this.context.Albums.Add(album);
            this.context.SaveChanges();
        }

        public Album GetAlbumDetails(string id)
        {
            var album = this.context.Albums.Where(x => x.Id == id).FirstOrDefault();

            return album;
        }

        public IEnumerable<T> GetAllAlbums<T>(Func<Album, T> selectFunc)
        {
            var allAlbums = this.context.Albums.Select(selectFunc).ToList();

            return allAlbums;
        }
    }
}

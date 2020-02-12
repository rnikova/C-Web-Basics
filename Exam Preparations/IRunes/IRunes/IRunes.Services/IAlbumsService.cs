using IRunes.Models;
using System;
using System.Collections.Generic;

namespace IRunes.Services
{
    public interface IAlbumsService
    {
        void Create(string name, string cover);

        IEnumerable<T> GetAllAlbums<T>(Func<Album, T> selectFunc);

        Album GetAlbumDetails(string id);
    }
}

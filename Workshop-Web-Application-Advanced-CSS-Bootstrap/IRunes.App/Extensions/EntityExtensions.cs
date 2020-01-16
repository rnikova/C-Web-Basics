using IRunes.Models;
using System.Linq;

namespace IRunes.App.Extensions
{
    public static class EntityExtensions
    {
        public static string ToHtmlAll(this Album album)
        {
            return $"<a href=\"/Albums/Details?id={album.Id}\">{album.Name}<a/>";
        }

        public static string ToHtmlDetails(this Album album)
        {
            return "< div class=\"album-details\">" +
                "   <div class=\"album-data\" >" +
                $"       <img-src>\"{ album.Cover}\"</img-src>" +
                $"      <h1>Album Name: {album.Name}</h1>" +
                $"      <h1>Album Price: {album.Price:F2}</h1>" +
                "       <br />" +
                "   </div>" +
                "   <div class=\"album-tracks\" > " +
                "       <h1>Trakcs</h1>" +
                "       <hr style = \"height: 2px\" />" +
                "       < a href=\" /Tracks/Create\" > Create Tracks</a>" +
                "       <hr style = \"height: 2px\" />" +
                "       < ul class=\"tracks-list\" > " +
                $"           {album.Tracks.Select((track, index) => (index + 1) + " " + track.ToHtmlAll())}" +
                "       </ul>" +
                "   </div>" +
                "</div>";
        }
        
        public static string ToHtmlAll(this Track track)
        {
            return null;
        }

        public static string ToHtmlDetails(this Track track)
        {
            return null;
        }
    }
}

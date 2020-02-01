using IRunes.Models;
using System.Linq;
using System.Net;

namespace IRunes.App.Extensions
{
    public static class EntityExtensions
    {
        public static string ToHtmlAll(this Album album)
        {
            return $"";
        }
        
        public static string ToHtmlAll(this Track track, string albumId, int index)
        {
            return $"<li><strong>{index}</strong>. <a href=\"/Tracks/Details?albumId={albumId}&trackId={track.Id}\"><i>{WebUtility.UrlDecode(track.Name)}</i></a></li>";
        }

        public static string ToHtmlDetails(this Track track, string albumId)
        {
            return "<div class=\"track-details\">" +
                   $"    <h4 class=\"text-center\">Track Name: {WebUtility.UrlDecode(track.Name)}</h4>" +
                   $"    <h4 class=\"text-center\">Track Price: ${track.Price:F2}</h4>" +
                   "    <hr class=\"bg-success w-50\" style=\"height: 2px\" />" +
                   "    <div class=\"d-flex justify-content-center\">" +
                   $"        <iframe src=\"{WebUtility.UrlDecode(track.Link)}\" class=\"w-50\" height=\"480\"></iframe>" +
                   "    </div>" +
                   "    <hr class=\"bg-success w-50\" style=\"height: 2px\" />" +
                   "    <div class=\"d-flex justify-content-center\">" +
                   $"        <a href=\"/Albums/Details?id={albumId}\" class=\"btn bg-success text-white\">Back To Album</a>" +
                   "    </div>" +
                   "</div>";
        }
    }
}

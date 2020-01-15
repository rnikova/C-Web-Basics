using System.Collections.Generic;

namespace IRunes.Models
{
    public class Album
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Cover { get; set; }

        public decimal Price { get; set; }

        public ICollection<Track> Traks { get; set; } = new HashSet<Track>();
    }
}

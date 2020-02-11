using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SULS.Models
{
    public class Problem
    {
        public string Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        [Required]
        public int Points { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public ICollection<Submission> Submissions { get; set; } = new HashSet<Submission>();
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace SULS.Models
{
    public class Submission
    {
        public string Id { get; set; }

        [Required]
        [MaxLength(800)]
        public string HasCode { get; set; }

        [Required]
        [Range(0, 300)]
        public int AchievedResult  { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public Problem HasProblem { get; set; }

        public User HasUser { get; set; }
    }
}

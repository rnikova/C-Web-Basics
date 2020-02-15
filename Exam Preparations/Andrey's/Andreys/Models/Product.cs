using System.ComponentModel.DataAnnotations;

namespace Andreys.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public decimal Price { get; set; }

        [Required]
        public Category Category { get; set; }

        [Required]
        public Gender Gender { get; set; }
    }
}

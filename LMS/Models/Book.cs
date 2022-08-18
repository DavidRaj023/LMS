using System.ComponentModel.DataAnnotations;

namespace LMS.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime PublicationDate { get; set; }
        [Required]
        public string Language { get; set; }
        [Required]
        public int Edition { get; set; }
        [Required]
        public bool IsAvailable { get; set; }
        public int RackId { get; set; }
        [Required]
        public int NumberOfCopies { get; set; }
        [Required]
        public int ReturnThreshold { get; set; }
        [Required]
        public Category Category { get; set; }
        [Required]
        public Author Author { get; set; }
        public int MyProperty { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}

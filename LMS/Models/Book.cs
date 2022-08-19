using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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
        [Display(Name = "Publication Date ")]
        public DateTime PublicationDate { get; set; }
        
        [Required]
        public string Language { get; set; }
        
        [Required]
        public int Edition { get; set; }
        
        [Required]
        public bool IsAvailable { get; set; } = false;

        [Display(Name = "Rack No ")]
        public int RackId { get; set; }
        
        [Required]
        [Display(Name = "Number of Copies ")]
        public int NumberOfCopies { get; set; }
        
        [Required]
        [Display(Name = "Return Threshold")]
        public int ReturnThreshold { get; set; }
        
        [Required]
        public Category Category { get; set; }
        public int CategoryId { get; set; }

        [Required]
        public Author Author { get; set; }
        public int AuthorId { get; set; }

        [ValidateNever]
        public string? ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime? UpdatedAt { get; set; }
    }
}

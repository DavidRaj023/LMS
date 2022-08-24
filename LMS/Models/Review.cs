using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [ValidateNever]
        [ForeignKey("UserId")]
        public User? User { get; set; }
        public int UserId { get; set; }

        [ValidateNever]
        [ForeignKey("BookId")]
        public Book? Book { get; set; }
        public int BookId { get; set; }

        [Required]
        [Display(Name = "Star Value")]
        public int StarValue { get; set; }
        [Required]
        public string Description { get; set; }
    }
}

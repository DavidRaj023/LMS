using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Models
{
    public class Rental
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

        public bool IsReturned { get; set; } = false;

        [Display(Name = "Penalty Amount")]
        public double? PenaltyAmount { get; set; }

        [Display(Name = "Rented Date")]
        public DateTime DateRented { get; set; } = DateTime.Now;
        [Display(Name = "Return Date")]
        public DateTime? DateReturn { get; set; }

    }
}

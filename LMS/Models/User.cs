using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Email { get; set; }
        public int Phone { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }

        [ForeignKey("RoleId")]
        [ValidateNever]
        public Role? Role { get; set; }
        public int RoleId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}

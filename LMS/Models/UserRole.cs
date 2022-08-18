using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Models
{
    [Keyless]
    public class UserRole
    {
        [ForeignKey("UserId")]
        [ValidateNever]
        public User User { get; set; }

        [ForeignKey("RoleId")]
        [ValidateNever]
        public Role Role { get; set; }
    }
}

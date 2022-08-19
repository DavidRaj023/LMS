using LMS.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LMS.ViewModel
{
    public class BookViewModel
    {
        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> AuthorList { get; set; }
        public Book Book { get; set; }

    }
}

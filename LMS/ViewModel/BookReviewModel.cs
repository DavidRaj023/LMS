using LMS.Models;

namespace LMS.ViewModel
{
    public class BookReviewModel
    {
        public Book Book { get; set; }
        public IEnumerable<Review> Review { get; set; }
    }
}

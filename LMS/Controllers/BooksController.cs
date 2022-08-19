using LMS.Data;
using LMS.Models;
using LMS.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LMS.Controllers
{
    public class BooksController : Controller
    {
        private ApplicationDbContext _context;

        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        //Category
        public IActionResult Category()
        {
            var categories = _context.Categories.ToList();
            return View("Category", categories);
        }
        public IActionResult NewCategory()
        {
            return View("CategoryForm");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveCategory(Category model)
        {
            _context.Categories.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Category", "Books");
        }

        //Author
        public IActionResult Authors()
        {
            var authors = _context.Authors.ToList();
            return View("Author", authors);
        }
        public IActionResult NewAuthor()
        {
            return View("AuthorForm");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveAuthor(Author model)
        {
            _context.Authors.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Authors", "Books");
        }

        //Books
        public IActionResult Index()
        {
            var books = _context.Books.ToList();
            return View("List", books);
        }
        
        public IActionResult New()
        {
            var categoryList = _context.Categories.Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.Name
            }).ToList();
            var authorList = _context.Authors.Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.Name
            }).ToList();

            var viewModel = new BookViewModel
            {
                CategoryList = categoryList,
                AuthorList = authorList
            };
            return View("BookForm", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveBooks(BookViewModel model)
        {
            _context.Books.Add(model.Book);
            _context.SaveChanges();
            return RedirectToAction("index", "Books");
        }

    }
}

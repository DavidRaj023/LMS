using LMS.Data;
using LMS.Models;
using LMS.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LMS.Controllers
{
    public class BooksController : Controller
    {
        private ApplicationDbContext _context;

        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
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
        public IActionResult Index(int? id)
        {
            if (id == null || id == 0)
            {
                var books = _context.Books
                    .Include(b => b.Category)
                    .Include(b => b.Author).ToList();
                return View(books);
            }
            var book = _context.Books
                    .Include(b => b.Category)
                    .Include(b => b.Author)
                    .FirstOrDefault(b => b.Id == id);
            return View("book", book);
        }

        //Books/filter?category=3
        //Books/filter?author=3
        public IActionResult Filter(int? category, int? author)
        {
            if(category != null || author != null)
            {
                if (category.HasValue)
                {
                    var books = _context.Books.Where(b => b.Category.Id == category);
                    return View("index", books);
                }
                if (author.HasValue)
                {
                    var books = _context.Books.Where(b =>b.Author.Id == author).ToList();
                    return View("index", books);
                }
            }
            return RedirectToAction("index", "Books");
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

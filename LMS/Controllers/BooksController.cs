using LMS.Data;
using LMS.Models;
using LMS.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LMS.Controllers
{
    public class BooksController : Controller
    {
        private ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        
        public BooksController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
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

        [Authorize(Roles ="Admin")]
        public IActionResult NewCategory()
        {
            return View("CategoryForm");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        public IActionResult NewAuthor()
        {
            return View("AuthorForm");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles ="Admin")]
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
        [Authorize(Roles = "Admin")]
        public IActionResult SaveBooks(BookViewModel model, IFormFile? file)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwwRootPath, @"Images\books");
                var extension = Path.GetExtension(file.FileName);

                if (model.Book.ImageUrl != null)
                {
                    var oldImagePath = Path.Combine(wwwRootPath, model.Book.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    file.CopyTo(fileStreams);
                }
                model.Book.ImageUrl = @"\Images\books\" + fileName + extension;

                _context.Books.Add(model.Book);
            }
            _context.SaveChanges();
            return RedirectToAction("index", "Books");
        }

    }
}

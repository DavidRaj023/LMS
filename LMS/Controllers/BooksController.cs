using AspNetCoreHero.ToastNotification.Abstractions;
using LMS.Data;
using LMS.Models;
using LMS.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LMS.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public INotyfService _notifyService { get; }

        public BooksController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment, INotyfService notifyService)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _notifyService = notifyService;
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

        [Authorize(Roles = "Admin")]
        public IActionResult NewCategoryModel()
        {
            return PartialView("CategoryPartialView");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult SaveCategory(Category model)
        {
            _context.Categories.Add(model);
            _context.SaveChanges();
            _notifyService.Success("New Category Added");
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
            _notifyService.Success("New Author Added");
            return RedirectToAction("Authors", "Books");
        }

        //Books
        public IActionResult Index(int? id)
        {
            if (id == null || id == 0)
            {
                var books = _context.Books
                    .Include(b => b.Category)
                    .Include(b => b.Author)
                    .Where(b => b.IsAvailable == true)
                    .ToList();
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
            var excitingBooks = _context.Books.FirstOrDefault(b => b.Title == model.Book.Title);
            if(excitingBooks != null)
            {
                _notifyService.Error("This Book is already exists");
                return RedirectToAction("index");
            }
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
            _notifyService.Success("New Book Added");
            return RedirectToAction("index", "Books");
        }

        //Edit
        [Authorize(Roles = "Admin")]
        public IActionResult EditBook(int id)
        {
            var bookDetails = _context.Books.FirstOrDefault(b => b.Id == id);
            if (bookDetails == null)
            {
                _notifyService.Error("Can't find the book");
                return RedirectToAction("index");
            }

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
                AuthorList = authorList,
                Book = bookDetails
            };
            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult UpdateBook(BookViewModel model, IFormFile? file)
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
            }

            var existingBook = _context.Books.FirstOrDefault(b => b.Id == model.Book.Id);
            if (existingBook == null)
            {
                _notifyService.Error("Somthing went wrong please try again");
                return RedirectToAction("index");
            }
            existingBook.Title = model.Book.Title;
            existingBook.Description = model.Book.Description;
            existingBook.PublicationDate = model.Book.PublicationDate;
            existingBook.Language = model.Book.Language;
            existingBook.Edition = model.Book.Edition;
            existingBook.NumberOfCopies = model.Book.NumberOfCopies;
            existingBook.RackId = model.Book.RackId;
            existingBook.ReturnThreshold = model.Book.ReturnThreshold;
            existingBook.Category = model.Book.Category;
            existingBook.CategoryId = model.Book.CategoryId;
            existingBook.Author = model.Book.Author;
            existingBook.AuthorId = model.Book.AuthorId;
            if(file != null)
            {
                existingBook.ImageUrl = model.Book.ImageUrl;
            }
            _context.SaveChanges();
            _notifyService.Success("Book Updated");
            return RedirectToAction("index", "Books");
        }

        //Delete Books
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteBook(int id)
        {
            var bookDetails = _context.Books.FirstOrDefault(b => b.Id == id);
            if (bookDetails == null)
            {
                _notifyService.Error("Not able to find the book");
                return View("index");
            }
            bookDetails.IsAvailable = false;
            _context.SaveChanges();
            _notifyService.Success("Book Removed");
            return RedirectToAction("index");
        }

        //AddReading
        public IActionResult AddToReadings(int bookId)
        {
            var user = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            /*var myBooks = _context.Rentals
                .Where(r => r.UserId == user.Id && r.BookId == bookId).ToList();*/
            var myBooks = _context.Rentals
                .Where(r => r.UserId == user.Id && r.BookId == bookId && r.IsReturned ==false).ToList();

            if (myBooks.Count > 0)
            {
                _notifyService.Error("This book is already in your readings list");
                return RedirectToAction("Index", bookId);
            }

            var bookInDb = _context.Books.Where(b => b.NumberOfCopies >0 && b.Id == bookId).FirstOrDefault();

            if(bookInDb == null)
            {
                _notifyService.Error("This book is not available");
                return RedirectToAction("Index", bookId);
            }

            var rendal = new Rental
            {
                BookId = bookId,
                UserId = user.Id
            };

            bookInDb.NumberOfCopies--;
            
            _context.Rentals.Add(rendal);
            _context.SaveChanges();
            
            _notifyService.Success("Added to your Readings");
            return RedirectToAction("Index", bookId);
        }

        public IActionResult MyReadings()
        {
            var user = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            
            var rentals = _context.Rentals.Include(r => r.Book).Where(r => r.UserId == user.Id && r.IsReturned ==false).ToList();

            foreach(var rental in rentals)
            {
                if(DateTime.Now > rental.DateRented.AddDays(rental.Book.ReturnThreshold))
                {
                    rental.PenaltyAmount = 10 * Math.Truncate((DateTime.Now - rental.DateRented.AddDays(rental.Book.ReturnThreshold)).TotalDays);
                }
            }
            _context.SaveChanges();
            return View(rentals);
        }

        public IActionResult MyReview(int rentId)
        {
            var rentDetails = _context.Rentals.FirstOrDefault(r => r.Id == rentId);
            var model = new ReviewViewModel
            {
                Rental = rentDetails
            };
            return View(model);
        }

        public IActionResult SubmitReview(ReviewViewModel model, int rentalId)
        {
            var rentalDetials = _context.Rentals.FirstOrDefault(r => r.Id == rentalId);
            var bookDetials = _context.Books.FirstOrDefault(b => b.Id == rentalDetials.BookId);
            if(rentalDetials == null)
            {
                _notifyService.Error("Something went wrong, Please try again");
                return RedirectToAction("MyReadings", "Books");
            }

            var myReview = new Review
            {
                UserId = rentalDetials.UserId,
                BookId = rentalDetials.BookId,
                StarValue = model.Review.StarValue,
                Description = model.Review.Description
            };

            _context.Reviews.Add(myReview);
            //Change 
            rentalDetials.IsReturned = true;
            rentalDetials.DateReturn = DateTime.Now;
            bookDetials.NumberOfCopies++;
            _context.SaveChanges();
            _notifyService.Success("Book Returned");
            return RedirectToAction("MyReadings", "Books");
        }

        public IActionResult Rentals()
        {
            var rentalDetials = _context.Rentals
                .Include(r => r.User)
                .Include(r => r.Book).ToList();
            return View(rentalDetials);
        }

        public IActionResult Back(string id)
        {
            return RedirectToAction(id);
        }
    }
}

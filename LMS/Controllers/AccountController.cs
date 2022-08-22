using LMS.Data;
using LMS.Models;
using LMS.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LMS.Controllers
{
    
    public class AccountController : Controller
    {
        private ApplicationDbContext _context;
        private readonly ITokenService _tokenService;
        private string generatedToken = null;
        private readonly IConfiguration _config;

        public AccountController(ApplicationDbContext context, ITokenService tokenService, IConfiguration config)
        {
            _context = context;
            _tokenService = tokenService;
            _config = config;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SignUp()
        {
            return View();
        }

        public IActionResult AdminSignUp()
        {
            return View();
        }

        public IActionResult New(User user)
        {
            var userData = new User
            {
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                UserName = user.UserName,
                Password = user.Password,
                RoleId = 2
            };
            _context.Users.Add(userData);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult NewAdmin(User user)
        {
            var userData = new User
            {
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                UserName = user.UserName,
                Password = user.Password,
                RoleId = 1
            };
            _context.Users.Add(userData);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public async Task<IActionResult> Login(User userModel)
        {
            IActionResult response = Unauthorized();
            if (string.IsNullOrEmpty(userModel.UserName) || string.IsNullOrEmpty(userModel.Password))
            {
                ViewBag.Message = "Please enter the valid input";
                return View("index");
            }

            var user = _context.Users.Include(u =>u.Role).FirstOrDefault(x => x.UserName == userModel.UserName);
            if(user == null)
            {
                ViewBag.Message = "Invalid User";
                return View("index");
            }

            if (userModel.Password != user.Password)
            {
                ViewBag.Message = "Invalid User";
                return View("index");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role.Name)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(20)
            };
            await HttpContext.SignInAsync(
              CookieAuthenticationDefaults.AuthenticationScheme,
              new ClaimsPrincipal(claimsIdentity),
              authProperties
            );
            return RedirectToAction("Index", "Home");

            /*var t = HttpContext.User.Identity.IsAuthenticated;*/

        }

        public async Task<IActionResult> LogOff()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        
    }

}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using PremiumHousing.Data;
using PremiumHousing.Helper;
using PremiumHousing.Model;
using PremiumHousing.Model.Enums;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace PremiumHousing.Controllers
{
    [Route("[controller]/[action]")]
    public class UsreController(ApplicationDbContext context, IConfiguration configuration, TokenHelper tokenHelper) : Controller
    {
        private readonly ApplicationDbContext _context = context;//read about it 
        private readonly JwtTokenGenerator _jwtTokenGenerator = new(configuration);//read about it 
        private readonly TokenHelper _tokenHelper = tokenHelper;//read about it 

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(UserDtoModle Model)
        {
            if (Model is null)
            {
                NotificationHelper.Alert(TempData, false, "Error Occured");
                return View();
                
            }
            if (ModelState.IsValid is false)
            {
                return View(Model);
            }

            bool UsernameChack = _context.User.Any(x => x.Username == Model.Username);

            if (UsernameChack is true)
            {
                NotificationHelper.Alert(TempData, false, "The name is already in use");
                return View(Model);
            }
            User user = new();
            if (Model.Type is false)
            {

                user = new()
                {
                    FullName = Model.FullName,
                    Username = Model.Username,
                    Email = Model.Email,
                    Password = HashHelper.HashPassword(Model.Password),
                    PhoneNumber = Model.PhoneNumber,
                    Gender = Model.Gender,
                    Type = UserType.Client,
                    CreatedDate = DateTime.Now,
                };
            }
            else
            {

                user = new()
                {
                    FullName = Model.FullName,
                    Username = Model.Username,
                    Email = Model.Email,
                    Password = HashHelper.HashPassword(Model.Password),
                    PhoneNumber = Model.PhoneNumber,
                    Gender = Model.Gender,
                    Type = UserType.Admin,
                    CreatedDate = DateTime.Now,
                    BusinessPhoneNumper = Model.BusinessPhoneNumper,
                    BusinessLicenseNumper = Model.BusinessLicenseNumper,
                    BusinessType = Model.BusinessType,
                    HasBusinessAddress = Model.BusinessAddress,
                    TaxNumber = Model.TaxNumber,
                    VerificationDate = DateTime.Now.AddDays(1)

                };
            }
            _context.User.Add((user));
            _context.SaveChanges();
            NotificationHelper.Alert(TempData, true, "Add Successfully");

            return RedirectToAction("Index", "Home");

        }
        [HttpGet]
        public IActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserViewModel Model)
        {
            if (Model is null)
            {
                NotificationHelper.Alert(TempData, false, "Error Occured");
                return View();
            }

            var password = HashHelper.HashPassword(Model.Password);

            var User = await _context.User
                .Where(x => x.Username.Equals(Model.Username) && x.Password.Equals(password))
                .FirstOrDefaultAsync();

            if (User is null)
            {
                NotificationHelper.Alert(TempData, false, "The user name or password is incorrect");
                return View();
            }
            
            //Handle JWT Token
            string Token = _jwtTokenGenerator.GenerateToken(User);

            // Set the token in a cookie
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true, // Prevents JavaScript access to the cookie
                Secure = true,   // Use Secure if using HTTPS
                SameSite = SameSiteMode.Strict, // Helps prevent CSRF
                Expires = DateTimeOffset.UtcNow.AddDays(7) // Set expiration as needed
            };

            Response.Cookies.Append("Token", Token, cookieOptions);

            return User.Type switch
            {
                UserType.system => RedirectToAction("Dashboard", "System"),
                UserType.Admin => RedirectToAction("Dashboard", "Admin"),
                _ => RedirectToAction("Index", "Home"),
            };
        }
        [HttpGet]
        [Authorize]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("Token");

            return RedirectToAction("Index", "Home");
        }


    }
}

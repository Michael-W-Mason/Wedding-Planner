using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Wedding_Planner.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Wedding_Planner.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    public Context _context;

    public HomeController(ILogger<HomeController> logger, Context context)
    {
        _logger = logger;
        _context = context;
    }


    // 
    // User Routes
    // 
    [HttpGet("")]
    public IActionResult Home()
    {
        return View("Home");
    }

    public IActionResult Login(LoginUser user)
    {
        if (ModelState.IsValid)
        {
            var userInDb = _context.Users.FirstOrDefault(u => u.Email == user.LoginEmail);
            if (userInDb == null)
            {
                ModelState.AddModelError("LoginEmail", "Invalid Email/Password");
                return View("Home");
            }
            var hasher = new PasswordHasher<LoginUser>();
            var result = hasher.VerifyHashedPassword(user, userInDb.Password, user.LoginPassword);
            if (result == 0)
            {
                ModelState.AddModelError("LoginEmail", "Invalid Email/Password");
                return View("Home");
            }
            HttpContext.Session.SetInt32("UserId", userInDb.UserId);
            return RedirectToAction("Dashboard");
        }
        return View("Home");
    }

    public IActionResult Register(User newUser)
    {
        if (ModelState.IsValid)
        {
            if (_context.Users.Any(user => user.Email == newUser.Email))
            {
                ModelState.AddModelError("Email", "Email Already In Use");
                return View("Home");
            }
            else
            {
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
                _context.Add(newUser);
                _context.SaveChanges();
                var userInDb = _context.Users.FirstOrDefault(u => u.Email == newUser.Email);
                HttpContext.Session.SetInt32("UserId", userInDb.UserId);
                return RedirectToAction("Dashboard");
            }
        }
        return View("Home");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Home");
    }

    // 
    // Dashboard
    // 
    public IActionResult DashBoard()
    {
        int? UserId = HttpContext.Session.GetInt32("UserId");
        if (UserId == null)
        {
            return RedirectToAction("Home");
        }
        ViewBag.UserId = UserId;
        List<Wedding> Weddings = _context.Weddings
        .Include(wed => wed.Guests)
        .ThenInclude(guest => guest.User)
        .ToList();
        return View("Dashboard", Weddings);
    }

    // 
    // Weddings
    // 
    public IActionResult NewWedding()
    {
        int? UserId = HttpContext.Session.GetInt32("UserId");
        if (UserId == null)
        {
            return RedirectToAction("Home");
        }
        return View("NewWedding");
    }

    public IActionResult CreateWedding(Wedding newWedding)
    {
        if (ModelState.IsValid)
        {
            newWedding.Creator = HttpContext.Session.GetInt32("UserId");
            _context.Add(newWedding);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        return View("NewWedding");
    }

    public IActionResult DeleteWedding(int WeddingId)
    {
        Wedding wedDelete = _context.Weddings.FirstOrDefault(wed => wed.WeddingId == WeddingId);
        _context.Remove(wedDelete);
        _context.SaveChanges();
        return RedirectToAction("Dashboard");
    }

    public IActionResult RSVP(int WeddingId)
    {
        int? UserId = HttpContext.Session.GetInt32("UserId");
        GuestLog newRSVP = new GuestLog();
        newRSVP.WeddingId = WeddingId;
        newRSVP.UserId = UserId;
        _context.GuestLogs.Add(newRSVP);
        _context.SaveChanges();
        return RedirectToAction("Dashboard");
    }

    public IActionResult UnRSVP(int WeddingId)
    {
        int? UserId = HttpContext.Session.GetInt32("UserId");
        GuestLog removeRSVP = _context.GuestLogs
        .Where(wed => wed.WeddingId == WeddingId)
        .FirstOrDefault(guest => guest.UserId == UserId);
        _context.Remove(removeRSVP);
        _context.SaveChanges();
        return RedirectToAction("Dashboard");
    }

    public IActionResult OneWedding(int WeddingId)
    {
        int? UserId = HttpContext.Session.GetInt32("UserId");
        if (UserId == null)
        {
            return RedirectToAction("Home");
        }
        Wedding oneWedding = _context.Weddings
        .Include(wed => wed.Guests)
        .ThenInclude(guest => guest.User)
        .FirstOrDefault(wed => wed.WeddingId == WeddingId);
        return View("OneWedding", oneWedding);
    }
}

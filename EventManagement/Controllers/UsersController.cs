using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventManagement.Database;
using EventManagement.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace EventManagement.Controllers
{
    public class UsersController : Controller
    {
        private readonly EventDbContext _context;

        public UsersController(EventDbContext context)
        {
            _context = context;
        }

        // GET: Users
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            var events = await _context.Events.ToListAsync();
            var eventIds = events.Select(e => e.Id).ToList();
            var tickets = _context.Tickets.ToList();
            var types = tickets.Select(t => t.Type).ToList();
            var regis = await _context.Registrations.Where(r => r.UserID == id).ToListAsync();
            var regisId = regis.Select(r => r.TicketID).ToList();
            var rTickets = await _context.Tickets.Where(t => regisId.Contains(t.Id)).ToListAsync();

            if (user == null)
            {
                return NotFound();
            }
            var dashboard = new Dashboard()
            {
                User = user,
                Events = events,
                Tickets = tickets,
                Tickets2 = rTickets,
                Registrations = regis,
                Registration = new Registration()
            };
            return View(dashboard);
        }
        public async Task<IActionResult> OrganizerDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            var events = await _context.Events.Where(e => e.OrganizerId == id).ToListAsync();
            var eventIds = events.Select(e => e.Id).ToList();
            var tickets = await _context.Tickets.Where(t => eventIds.Contains(t.EventID)).ToListAsync();
            var registration = await _context.Registrations.Where(r => eventIds.Contains(r.EventID)).ToListAsync();
            if (user == null)
            {
                return NotFound();
            }
            var dashboard = new Dashboard()
            {
                User = user,
                Events = events,
                Tickets = tickets,
                Registrations = registration
            };

            return View(dashboard);
        }
        public async Task<IActionResult> AdminDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            var users = await _context.Users.ToListAsync();
            var events = await _context.Events.ToListAsync();
            var tickets = await _context.Tickets.ToListAsync();
            if (user == null)
            {
                return NotFound();
            }
            var dashboard = new Dashboard()
            {
                User = user,
                Users = users,
                Events = events,
                Tickets = tickets
            };

            return View(dashboard);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Password,Role")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Password,Role")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login userLogin)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == userLogin.Email && x.Password == userLogin.Password && x.Role == userLogin.Role);
                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim("Name", user.Name),
                        new Claim(ClaimTypes.Role, user.Role),
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                    if(userLogin.Role == "Admin")
                    {
                        return RedirectToAction("AdminDetails", "Users", new { area = "", id = user.Id });
                    }else 
                        if(userLogin.Role == "Organizer")
                    {
                        return RedirectToAction("OrganizerDetails", "Users", new { area = "", id = user.Id });
                    }else
                        if(userLogin.Role == "User")
                    {
                        return RedirectToAction("Details", "Users", new { area = "", id = user.Id });
                    }
                    
                }
                else
                {
                    ModelState.AddModelError("", "Login Information is incorrect.");
                    ViewBag.Message = $"Login Information is incorrect.";
                }
            }
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}

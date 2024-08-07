using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventManagement.Database;
using EventManagement.Models;
using System.Security.Claims;

namespace EventManagement.Controllers
{
    public class EventsController : Controller
    {
        private readonly EventDbContext _context;

        public EventsController(EventDbContext context)
        {
            _context = context;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            return View(await _context.Events.ToListAsync());
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EventName,EventDescription,EventType,DateTime,OrganizerId,Location")] Event @event)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }
            return View(@event);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EventName,EventDescription,EventType,DateTime,OrganizerId,Location")] Event @event)
        {
            if (id != @event.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingEvent = await _context.Events.FindAsync(@event.Id);
                if (existingEvent == null)
                {
                    return NotFound();
                }

                var newEvent = new Event()
                {
                    Id = @event.Id,
                    EventName = @event.EventName,
                    EventDescription = @event.EventDescription,
                    EventType = @event.EventType,
                    DateTime = @event.DateTime,
                    OrganizerId = @event.OrganizerId,
                    Location = @event.Location,
                };
                try
                {
                    existingEvent.EventName = newEvent.EventName;
                    existingEvent.EventDescription = newEvent.EventDescription;
                    existingEvent.EventType = newEvent.EventType;
                    existingEvent.DateTime = newEvent.DateTime;
                    existingEvent.OrganizerId = newEvent.OrganizerId;
                    existingEvent.Location = newEvent.Location;

                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                var userid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var role = User.FindFirst(ClaimTypes.Role)?.Value;
                if (role == "Admin")
                {
                    return RedirectToAction("AdminDetails", "Users", new { area = "", id = userid });
                }
                else
                    if (role == "Organizer")
                {
                    return RedirectToAction("OrganizerDetails", "Users", new { area = "", id = userid });
                }
                else
                    if (role == "User")
                {
                    return RedirectToAction("Details", "Users", new { area = "", id = userid });
                }
            }
            return View(@event);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event != null)
            {
                _context.Events.Remove(@event);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}

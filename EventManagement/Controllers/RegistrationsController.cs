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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace EventManagement.Controllers
{
    public class RegistrationsController : Controller
    {
        private readonly EventDbContext _context;

        public RegistrationsController(EventDbContext context)
        {
            _context = context;
        }

        // GET: Registrations
        public async Task<IActionResult> Index()
        {
            return View(await _context.Registrations.ToListAsync());
        }

        // GET: Registrations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registration = await _context.Registrations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (registration == null)
            {
                return NotFound();
            }

            return View(registration);
        }

        // GET: Registrations/Create
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EventID,UserID,DateTime,TicketID,TicketQty,TicketPrice,Total")] Registration registration)
        {
            if (ModelState.IsValid)
            {
                var ticketP = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == registration.TicketID);
                var existingRegistration = await _context.Registrations.FirstOrDefaultAsync(i => registration.TicketID == i.TicketID && registration.UserID == i.UserID);
                var ticket = await _context.Tickets.FirstOrDefaultAsync(t => registration.TicketID == t.Id);

                if(existingRegistration != null)
                {
                    existingRegistration.EventID = registration.EventID;
                    existingRegistration.DateTime = registration.DateTime;
                    existingRegistration.TicketQty = existingRegistration.TicketQty + registration.TicketQty;
                    existingRegistration.TicketPrice = ticketP.Price;
                    existingRegistration.Total = existingRegistration.Total + (ticketP.Price * registration.TicketQty);

                    ticket.Quantity = ticket.Quantity - existingRegistration.TicketQty;

                    if (ticket.Quantity == 0)
                    {
                        ViewBag.msg = "Sold Out";
                    }
                    else 
                        if (ticket.Quantity < registration.TicketQty)
                    {
                        ViewBag.msg = "Tickets Not Available";
                    }
                    else
                    {
                        try
                        {
                            _context.Tickets.Update(ticket);
                            _context.Registrations.Update(existingRegistration);
                            await _context.SaveChangesAsync();
                        }
                        catch
                        {
                            if (!RegistrationExists(existingRegistration.Id))
                            {
                                return NotFound();
                            }
                            else
                            {
                                throw;
                            }
                        }
                    }

                    
                }
                else
                {
                    var newRegistration = new Registration()
                    {
                        EventID = registration.EventID,
                        UserID = registration.UserID,
                        DateTime = registration.DateTime,
                        TicketID = registration.TicketID,
                        TicketQty = registration.TicketQty,
                        TicketPrice = ticketP.Price,
                        Total = ticketP.Price * registration.TicketQty
                    };

                    _context.Add(newRegistration);
                    await _context.SaveChangesAsync();
                }
                var user = registration.UserID;
                var role = User.FindFirst(ClaimTypes.Role)?.Value;
                if (role == "Admin")
                {
                    return RedirectToAction("AdminDetails", "Users", new { area = "", id = user });
                }
                else
                    if (role == "Organizer")
                {
                    return RedirectToAction("OrganizerDetails", "Users", new { area = "", id = user });
                }
                else
                    if (role == "User")
                {
                    return RedirectToAction("Details", "Users", new { area = "", id = user });
                }
                
            }
            return View(registration);
        }

        // GET: Registrations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registration = await _context.Registrations.FindAsync(id);
            if (registration == null)
            {
                return NotFound();
            }
            return View(registration);
        }

        // POST: Registrations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventID,UserID,DateTime,TicketID,TicketQty,TicketPrice,Total")] Registration registration)
        {
            if (id != registration.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registration);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegistrationExists(registration.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                var role = User.FindFirst(ClaimTypes.Role)?.Value;
                var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (role == "Admin")
                {
                    return RedirectToAction("AdminDetails", "Users", new { area = "", id = user });
                }
                else
                    if (role == "Organizer")
                {
                    return RedirectToAction("OrganizerDetails", "Users", new { area = "", id = user });
                }
                else
                    if (role == "User")
                {
                    return RedirectToAction("Details", "Users", new { area = "", id = user });
                }
            }
            return View(registration);
        }

        // GET: Registrations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registration = await _context.Registrations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (registration == null)
            {
                return NotFound();
            }

            return View(registration);
        }

        // POST: Registrations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var registration = await _context.Registrations.FindAsync(id);
            if (registration != null)
            {
                _context.Registrations.Remove(registration);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegistrationExists(int id)
        {
            return _context.Registrations.Any(e => e.Id == id);
        }
        private bool TicketExists(int id)
        {
            return _context.Registrations.Any(e => e.TicketID == id);
        }
    }
}

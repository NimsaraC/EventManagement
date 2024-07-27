using EventManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Database
{
    public class EventDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public EventDbContext(DbContextOptions<EventDbContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = @"Server=localhost;Database=EventDB;Trusted_Connection=True;Connect Timeout=30;Encrypt=True;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

}

namespace EventManagement.Models
{
    public class Dashboard
    {
        public User User { get; set; }
        public IEnumerable<User> Users { get; set; }
        public IEnumerable<Event> Events { get; set; }
        public IEnumerable<Ticket> Tickets { get; set; }
        public IEnumerable<Registration> Registrations { get; set; }
        public Event Event { get; set; }
        public Ticket Ticket { get; set; }
        public Registration Registration { get; set; }
    }
}

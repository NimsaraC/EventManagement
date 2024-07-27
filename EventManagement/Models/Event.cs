namespace EventManagement.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; } = string.Empty;
        public string EventType { get; set; }
        public DateTime DateTime { get; set; }
        public int OrganizerId { get; set; }
        public string Location { get; set; }
    }
}

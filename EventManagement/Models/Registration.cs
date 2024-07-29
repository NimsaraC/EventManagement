﻿namespace EventManagement.Models
{
    public class Registration
    {
        public int Id { get; set; }
        public int EventID { get; set; }
        public int UserID { get; set; }
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
        public int TicketID { get; set; }
    }
}

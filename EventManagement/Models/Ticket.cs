namespace EventManagement.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public int EventID { get; set; }
        public string Type { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}

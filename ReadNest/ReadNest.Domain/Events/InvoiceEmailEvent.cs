namespace ReadNest.Domain.Events
{
    public class InvoiceEmailEvent
    {
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}

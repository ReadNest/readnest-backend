namespace ReadNest.Application.Models.Requests.Event
{
    public class UpdateEventRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
    }
}

namespace ReadNest.Application.Models.Requests.Event
{
    public class CreateEventRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }

        // Additional properties can be added as needed
        // For example, you might want to include properties for rewards, leaderboards, etc.
    }
}

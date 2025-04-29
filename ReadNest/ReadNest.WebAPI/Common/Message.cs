namespace ReadNest.WebAPI.Common
{
    public partial class Message
    {
        // Success messages
        public const string I0000 = "The operation was successful!"; // I0000
        public const string I0001 = "The item was added successfully."; // I0001
        public const string I0002 = "The item was updated successfully."; // I0002

        // Warning messages
        public const string W0000 = "Do you want to proceed with this action?"; // W0000
        public const string W0001 = "This action may have unintended consequences."; // W0001
        public const string W0002 = "You are about to overwrite existing data. Are you sure?"; // W0002

        // Error messages
        public const string E0000 = "The operation wasn't successful!"; // E0000
        public const string E0001 = "The item could not be found."; // E0001
        public const string E0002 = "An unexpected error occurred. Please try again later."; // E0002

        // Mapping ID to message
        private static readonly Dictionary<string, string> _messages = new()
        {
            { nameof(I0000), I0000 },
            { nameof(I0001), I0001 },
            { nameof(I0002), I0002 },
            { nameof(W0000), W0000 },
            { nameof(W0001), W0001 },
            { nameof(W0002), W0002 },
            { nameof(E0000), E0000 },
            { nameof(E0001), E0001 },
            { nameof(E0002), E0002 }
        };

        // Method to get message content by ID
        public static string GetMessageById(string id)
        {
            return _messages.TryGetValue(id, out var message) ? message : "Unknown message.";
        }
    }

}

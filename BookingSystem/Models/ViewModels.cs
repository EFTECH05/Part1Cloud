namespace BookingSystem.Models
{
    // ----- For creating events -----
    public class RegisterEventViewModel
    {
        public string EventName { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public int VenueId { get; set; }
    }

    // ----- For creating bookings -----
    public class RegisterBookingViewModel
    {
        public int EventId { get; set; }
        public int VenueId { get; set; }
        public DateTime BookingDate { get; set; }
    }

    // ----- For creating venues -----
    public class CreateVenueViewModel
    {
        public string VenueName { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public int Capacity { get; set; }   // ✅ CHANGE TO INT
        public string ImageUrl { get; set; } = string.Empty;
    }

}

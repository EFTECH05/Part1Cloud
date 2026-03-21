using Microsoft.Data.SqlClient;

namespace BookingSystem
{
    public static class DbInitializer
    {
        public static void Initialize(string connectionString)
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();

            // ----- tVenue table -----
            var venueCmd = @"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'tVenue')
            BEGIN
                CREATE TABLE tVenue (
                    VenueId INT IDENTITY(1,1) PRIMARY KEY,
                    VenueName NVARCHAR(100) NOT NULL,
                    Location NVARCHAR(100) NOT NULL,
                    Capacity INT NOT NULL,
                    ImageUrl NVARCHAR(255) NOT NULL,
                    IsDeleted BIT DEFAULT 0
                );
            END";
            using var cmdVenue = new SqlCommand(venueCmd, conn);
            cmdVenue.ExecuteNonQuery();

            // ----- tEvent table -----
            var eventCmd = @"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'tEvent')
            BEGIN
                CREATE TABLE tEvent (
                    EventId INT IDENTITY(1,1) PRIMARY KEY,
                    EventName NVARCHAR(100) NOT NULL,
                    EventDate DATETIME NOT NULL,
                    Description NVARCHAR(500),
                    VenueId INT NOT NULL,
                    IsDeleted BIT DEFAULT 0,
                    CONSTRAINT FK_tEvent_tVenue FOREIGN KEY (VenueId) REFERENCES tVenue(VenueId)
                );
            END";
            using var cmdEvent = new SqlCommand(eventCmd, conn);
            cmdEvent.ExecuteNonQuery();

            // ----- tBooking table -----
            var bookingCmd = @"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'tBooking')
            BEGIN
                CREATE TABLE tBooking (
                    BookingId INT IDENTITY(1,1) PRIMARY KEY,
                    EventId INT NOT NULL,
                    VenueId INT NOT NULL,
                    BookingDate DATETIME NOT NULL,
                    IsDeleted BIT DEFAULT 0,
                    CONSTRAINT FK_tBooking_tEvent FOREIGN KEY (EventId) REFERENCES tEvent(EventId),
                    CONSTRAINT FK_tBooking_tVenue FOREIGN KEY (VenueId) REFERENCES tVenue(VenueId)
                );
            END";
            using var cmdBooking = new SqlCommand(bookingCmd, conn);
            cmdBooking.ExecuteNonQuery();
        }
    }
}

using BookingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;

namespace BookingSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly string connection = "Server=tcp:evenease-sqlserver.database.windows.net,1433;Initial Catalog=EvenEaseDB;Persist Security Info=False;User ID=eveneaseadmin;Password=Sephor@5;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public IActionResult Index() => View();

        // ----- CREATE VIEWS -----
        public IActionResult CreateVenue() => View();

        public IActionResult CreateEvent()
        {
            ViewBag.VenueList = LoadVenues() ?? new List<SelectListItem>();
            return View();
        }

        public IActionResult Booking()
        {
            ViewBag.VenueList = LoadVenues() ?? new List<SelectListItem>();
            ViewBag.EventList = LoadEvents() ?? new List<SelectListItem>();
            return View();
        }

        // ----- VIEW VENUES -----
        public IActionResult ViewVenue()
        {
            var venues = new List<Dictionary<string, object>>();
            using var conn = new SqlConnection(connection);
            conn.Open();
            using var cmd = new SqlCommand("SELECT * FROM tVenue WHERE IsDeleted = 0", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var row = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                    row[reader.GetName(i)] = reader.GetValue(i);
                venues.Add(row);
            }
            ViewBag.Venues = venues;
            return View();
        }

        // ----- VIEW EVENTS -----
        public IActionResult ViewEvent()
        {
            var events = new List<Dictionary<string, object>>();
            using var conn = new SqlConnection(connection);
            conn.Open();
            string sql = @"
                SELECT e.EventId, e.EventName, e.EventDate, e.Description, e.VenueId, v.VenueName
                FROM tEvent e
                LEFT JOIN tVenue v ON e.VenueId = v.VenueId
                WHERE e.IsDeleted = 0";
            using var cmd = new SqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var row = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                    row[reader.GetName(i)] = reader.GetValue(i);
                events.Add(row);
            }
            ViewBag.Events = events;
            return View();
        }

        // ----- VIEW BOOKINGS -----
        public IActionResult ViewBooking()
        {
            var bookings = new List<Dictionary<string, object>>();
            using var conn = new SqlConnection(connection);
            conn.Open();
            string sql = @"
                SELECT b.BookingId, e.EventName, v.VenueName, ISNULL(v.ImageUrl,'/Images/default.png') AS ImageUrl
                FROM tBooking b
                JOIN tEvent e ON b.EventId = e.EventId
                JOIN tVenue v ON b.VenueId = v.VenueId
                WHERE b.IsDeleted = 0";
            using var cmd = new SqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var row = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                    row[reader.GetName(i)] = reader.GetValue(i);
                bookings.Add(row);
            }
            ViewBag.tBooking = bookings;
            return View();
        }

        // ----- LOAD VENUES & EVENTS -----
        private List<SelectListItem> LoadVenues()
        {
            var list = new List<SelectListItem>();
            using var conn = new SqlConnection(connection);
            conn.Open();
            using var cmd = new SqlCommand("SELECT VenueId, VenueName FROM tVenue WHERE IsDeleted = 0", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
                list.Add(new SelectListItem
                {
                    Value = reader["VenueId"].ToString(),
                    Text = reader["VenueName"].ToString()
                });
            return list;
        }

        private List<SelectListItem> LoadEvents()
        {
            var list = new List<SelectListItem>();
            using var conn = new SqlConnection(connection);
            conn.Open();
            using var cmd = new SqlCommand("SELECT EventId, EventName FROM tEvent WHERE IsDeleted = 0", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
                list.Add(new SelectListItem
                {
                    Value = reader["EventId"].ToString(),
                    Text = reader["EventName"].ToString()
                });
            return list;
        }

        // ----- CREATE POST METHODS -----
        [HttpPost]
        public async Task<IActionResult> RegisterVenue(CreateVenueViewModel model, IFormFile? ImageFile)
        {
            if (!ModelState.IsValid) return RedirectToAction("CreateVenue");

            if (ImageFile != null)
            {
                string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images");
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                string filePath = Path.Combine(folder, ImageFile.FileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await ImageFile.CopyToAsync(stream);
                model.ImageUrl = "/Images/" + ImageFile.FileName;
            }
            else
            {
                model.ImageUrl = "/Images/default.png";
            }

            using var conn = new SqlConnection(connection);
            conn.Open();
            using var cmd = new SqlCommand(
                "INSERT INTO tVenue (VenueName, Location, Capacity, ImageUrl) VALUES (@VenueName,@Location,@Capacity,@ImageUrl)",
                conn);
            cmd.Parameters.AddWithValue("@VenueName", model.VenueName);
            cmd.Parameters.AddWithValue("@Location", model.Location);
            cmd.Parameters.AddWithValue("@Capacity", model.Capacity);
            cmd.Parameters.AddWithValue("@ImageUrl", model.ImageUrl);
            cmd.ExecuteNonQuery();

            return RedirectToAction("ViewVenue");
        }

        [HttpPost]
        public IActionResult RegisterEvent(RegisterEventViewModel model)
        {
            if (!ModelState.IsValid) return RedirectToAction("CreateEvent");

            using var conn = new SqlConnection(connection);
            conn.Open();
            using var cmd = new SqlCommand(
                "INSERT INTO tEvent (EventName, EventDate, Description, VenueId) VALUES (@EventName,@EventDate,@Description,@VenueId)",
                conn);
            cmd.Parameters.AddWithValue("@EventName", model.EventName);
            cmd.Parameters.AddWithValue("@EventDate", model.EventDate);
            cmd.Parameters.AddWithValue("@Description", model.Description);
            cmd.Parameters.AddWithValue("@VenueId", model.VenueId);
            cmd.ExecuteNonQuery();

            return RedirectToAction("ViewEvent");
        }

        [HttpPost]
        public IActionResult RegisterBooking(RegisterBookingViewModel model)
        {
            if (!ModelState.IsValid) return RedirectToAction("Booking");

            using var conn = new SqlConnection(connection);
            conn.Open();
            using var cmd = new SqlCommand(
                "INSERT INTO tBooking (EventId, VenueId, BookingDate) VALUES (@EventId,@VenueId,@BookingDate)",
                conn);
            cmd.Parameters.AddWithValue("@EventId", model.EventId);
            cmd.Parameters.AddWithValue("@VenueId", model.VenueId);
            cmd.Parameters.AddWithValue("@BookingDate", model.BookingDate);
            cmd.ExecuteNonQuery();

            return RedirectToAction("ViewBooking");
        }

        // ----- UPDATE VENUE -----
      

        // ----- UPDATE EVENT -----
        public IActionResult UpdateEvent(int id)
        {
            using var conn = new SqlConnection(connection);
            conn.Open();
            string sql = "SELECT * FROM tEvent WHERE EventId=@id AND IsDeleted=0";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                var model = new RegisterEventViewModel
                {
                    EventName = reader["EventName"].ToString(),
                    EventDate = Convert.ToDateTime(reader["EventDate"]),
                    Description = reader["Description"].ToString(),
                    VenueId = Convert.ToInt32(reader["VenueId"])
                };
                ViewBag.EventId = id.ToString();
                ViewBag.VenueList = LoadVenues();
                return View(model);
            }
            return RedirectToAction("ViewEvent");
        }

        [HttpPost]
        public IActionResult UpdateEvent(int id, RegisterEventViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.EventId = id.ToString();
                ViewBag.VenueList = LoadVenues();
                return View(model);
            }

            using var conn = new SqlConnection(connection);
            conn.Open();
            string sql = @"UPDATE tEvent 
                           SET EventName=@EventName, EventDate=@EventDate, Description=@Description, VenueId=@VenueId
                           WHERE EventId=@id";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@EventName", model.EventName);
            cmd.Parameters.AddWithValue("@EventDate", model.EventDate);
            cmd.Parameters.AddWithValue("@Description", model.Description);
            cmd.Parameters.AddWithValue("@VenueId", model.VenueId);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();

            return RedirectToAction("ViewEvent");
        }

        // ----- UPDATE BOOKING -----
        public IActionResult UpdateBooking(int id)
        {
            using var conn = new SqlConnection(connection);
            conn.Open();
            string sql = @"SELECT BookingId, EventId, VenueId, BookingDate 
                           FROM tBooking WHERE BookingId=@id AND IsDeleted=0";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                var model = new RegisterBookingViewModel
                {
                    EventId = Convert.ToInt32(reader["EventId"]),
                    VenueId = Convert.ToInt32(reader["VenueId"]),
                    BookingDate = Convert.ToDateTime(reader["BookingDate"])
                };
                ViewBag.BookingId = id.ToString();
                ViewBag.VenueList = LoadVenues();
                ViewBag.EventList = LoadEvents();
                return View(model);
            }
            return RedirectToAction("ViewBooking");
        }

        [HttpPost]
        public IActionResult UpdateBooking(int id, RegisterBookingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.BookingId = id.ToString();
                ViewBag.VenueList = LoadVenues();
                ViewBag.EventList = LoadEvents();
                return View(model);
            }

            using var conn = new SqlConnection(connection);
            conn.Open();
            string sql = @"UPDATE tBooking 
                           SET EventId=@EventId, VenueId=@VenueId, BookingDate=@BookingDate
                           WHERE BookingId=@id";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@EventId", model.EventId);
            cmd.Parameters.AddWithValue("@VenueId", model.VenueId);
            cmd.Parameters.AddWithValue("@BookingDate", model.BookingDate);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();

            return RedirectToAction("ViewBooking");
        }

        // ----- DELETE METHODS -----
        public IActionResult DeleteVenue(int id)
        {
            using var conn = new SqlConnection(connection);
            conn.Open();
            using var cmd = new SqlCommand("UPDATE tVenue SET IsDeleted=1 WHERE VenueId=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            return RedirectToAction("ViewVenue");
        }

        public IActionResult DeleteEvent(int id)
        {
            using var conn = new SqlConnection(connection);
            conn.Open();
            using var cmd = new SqlCommand("UPDATE tEvent SET IsDeleted=1 WHERE EventId=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            return RedirectToAction("ViewEvent");
        }


        public IActionResult DeleteBooking(int id)
        {
            using var conn = new SqlConnection(connection);
            conn.Open();
            using var cmd = new SqlCommand("UPDATE tBooking SET IsDeleted=1 WHERE BookingId=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            return RedirectToAction("ViewBooking");
        }
    }
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// ----- Build app first -----
var app = builder.Build();

// ----- Initialize database tables -----
try
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    BookingSystem.DbInitializer.Initialize(connectionString);
}
catch (Exception ex)
{
    // Log the error or handle it appropriately
    Console.WriteLine($"Database initialization failed: {ex.Message}");
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

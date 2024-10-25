using Microsoft.EntityFrameworkCore;
using GamesDataAccess; // Adjust this according to your namespace

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure Entity Framework with SQLite
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("VideogamesDatabase")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();  // Enables HTTP Strict Transport Security (HSTS) for production.
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Initialize the database with seed data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<DatabaseContext>();
    DbInitializer.Initialize(context);
}

// Set the default route to point to GamesController's Index action
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Games}/{action=Index}/{id?}");

app.Run();

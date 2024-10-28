using Microsoft.EntityFrameworkCore;
using GamesDataAccess;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Configura il Database Context
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("VideogamesDatabase")));

// Configura il servizio di sessione
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true; // Protezione del cookie della sessione
    options.Cookie.IsEssential = true; // Necessario per il GDPR
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); // Aggiungi il middleware della sessione qui

app.UseAuthorization();

// Inizializza il database alla partenza dell'app
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<DatabaseContext>();
    DbInitializer.Initialize(context);
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();

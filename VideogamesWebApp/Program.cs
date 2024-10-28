using Microsoft.EntityFrameworkCore;
using GamesDataAccess;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("VideogamesDatabase")));

builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true; 
    options.Cookie.IsEssential = true; 
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

app.UseSession(); 

app.UseAuthorization();

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

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CRUD_Management_System.Data;
using CRUD_Management_System.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql
    (builder.Configuration.GetConnectionString("DefaultConnection"),
     new MySqlServerVersion(new Version(8, 0, 36))));  // Ensure to use the correct MySQL version

builder.Services.AddAntiforgery(options => {
    options.HeaderName = "RequestVerificationToken"; // Match met fetch header
});

// Dependency Injection
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<AliasService>();

var app = builder.Build();

#region [Migrate csv data]
/*
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<AppDbContext>();

    var csvService = new CsvToDatabaseService(dbContext);
    await csvService.ImportUserDetailsFromCsvAsync("data_users.csv");
}
*/
#endregion [Migrate csv data]

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();

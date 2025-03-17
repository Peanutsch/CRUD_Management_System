using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CRUD_Management_System.Data;
using CRUD_Management_System.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;
using Microsoft.Extensions.Logging;

#region [BUILDERS]
var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Voeg Serilog toe voor bestandslogging
Log.Logger = new LoggerConfiguration()
    .WriteTo.File("Logs/CreatedAccounts/create_logs.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();  // Serilog wordt nu als logger gebruikt

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql
    (builder.Configuration.GetConnectionString("DefaultConnection"),
     new MySqlServerVersion(new Version(8, 0, 36))));  // Ensure to use the correct MySQL version

builder.Services.AddAntiforgery(options => {
    options.HeaderName = "RequestVerificationToken"; // Match met fetch header
});

builder.Logging.ClearProviders();
builder.Logging.AddConsole(options =>
{
    options.LogToStandardErrorThreshold = LogLevel.Warning; // Ignore Entity Framework Core SQL-query logs
});


// Dependency Injection
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<AliasService>();
builder.Services.AddScoped<PasswordUpdateService>(); // Zorg ervoor dat de PasswordUpdateService wordt geregistreerd

#region [JWT Authentication Config]
// JWT Authentication configuration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],  // Set your Issuer from appsettings.json
            ValidAudience = builder.Configuration["Jwt:Audience"],  // Set your Audience from appsettings.json
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!))  // Secret key from appsettings.json
        };
    });
#endregion [JWT Authentication Config]

var app = builder.Build();
#endregion [BUILDERS]

#region [Encrypt Passwords with BCrypt]
/*
// Update de wachtwoorden naar gehashte wachtwoorden
using (var scope = app.Services.CreateScope())
{
    var passwordUpdateService = scope.ServiceProvider.GetRequiredService<PasswordUpdateService>();
    passwordUpdateService.UpdatePasswordsToHash(); // Update alle wachtwoorden in de database
}
*/
#endregion [Encrypt Passwords with BCrypt]

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
    pattern: "{controller=Login}/{action=Index}/{id?}") // Login/Index is start page
    .WithStaticAssets();


app.Run();

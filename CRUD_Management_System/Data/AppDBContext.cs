using Microsoft.EntityFrameworkCore;
using CRUD_Management_System.Models;

namespace CRUD_Management_System.Data;

public class AppDbContext : DbContext 
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<UserLoginModel> Users { get; set; }
}


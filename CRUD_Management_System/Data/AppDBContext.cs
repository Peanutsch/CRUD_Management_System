using Microsoft.EntityFrameworkCore;
using CRUD_Management_System.Models;

public class AppDbContext : DbContext 
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<UserDetails> Users { get; set; }
}


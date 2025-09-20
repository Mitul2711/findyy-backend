using Microsoft.EntityFrameworkCore;
using findyy.Model.Auth;
using findyy.Model.BusinessRegister;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Business> Business { get; set; }
    public DbSet<BusinessLocation> BusinessLocation { get; set; }
    public DbSet<BusinessHour> BusinessHour { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("AppUsers");
    }
}

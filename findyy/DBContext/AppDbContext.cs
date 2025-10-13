using Microsoft.EntityFrameworkCore;
using findyy.Model.Auth;
using findyy.Model.BusinessRegister;
using findyy.DTO.BusinessCategoryDTO;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Business> Business { get; set; }
    public DbSet<BusinessLocation> BusinessLocation { get; set; }
    public DbSet<BusinessHour> BusinessHour { get; set; }
    public DbSet<BusinessCategory> BusinessCategory { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("AppUsers");
    }
}

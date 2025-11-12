using findyy.DTO.BusinessCategoryDTO;
using findyy.Model.Auth;
using findyy.Model.BusinessRegister;
using findyy.Model.BusinessReview;
using findyy.Model;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Business> Business { get; set; }
    public DbSet<BusinessLocation> BusinessLocation { get; set; }
    public DbSet<BusinessHour> BusinessHour { get; set; }
    public DbSet<BusinessCategory> BusinessCategory { get; set; }
    public DbSet<BusinessReview> BusinessReview { get; set; }
    public DbSet<ChatMessageModel> ChatMessages { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("AppUsers");
    }
}

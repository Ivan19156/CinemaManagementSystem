using CinemaManagementSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;

public class CinemaDbContext : DbContext
{
    public CinemaDbContext(DbContextOptions<CinemaDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Film> Films { get; set; }
    public DbSet<Hall> Halls { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Sale> Sales { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<UserDiscount> UserDiscounts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Визначення ключа для UserDiscount
        modelBuilder.Entity<UserDiscount>()
            .HasKey(ud => new { ud.UserId, ud.DiscountId });

        modelBuilder.Entity<UserDiscount>()
            .HasOne(ud => ud.User)
            .WithMany(u => u.UserDiscounts)
            .HasForeignKey(ud => ud.UserId);

        modelBuilder.Entity<UserDiscount>()
            .HasOne(ud => ud.Discount)
            .WithMany(d => d.UserDiscounts)
            .HasForeignKey(ud => ud.DiscountId);

        // Визначення точності та масштабу для decimal
        modelBuilder.Entity<Discount>()
            .Property(d => d.Percentage)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Sale>()
            .Property(s => s.TotalPrice)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Session>()
            .Property(s => s.Price)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Ticket>()
            .Property(t => t.Price)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<User>()
            .Property(u => u.Bonus)
            .HasColumnType("decimal(18,2)");

        // Конвертація enum в string для User.Role
        modelBuilder.Entity<User>()
            .Property(u => u.Role)
            .HasConversion<string>();

        // Конвертація enum в string для Hall.Type
        modelBuilder.Entity<Hall>()
            .Property(h => h.Type)
            .HasConversion<string>();

        // Конвертація enum в string для Session.Status
        modelBuilder.Entity<Session>()
            .Property(s => s.Status)
            .HasConversion<string>();

        // Конвертація enum в string для Ticket.Status
        modelBuilder.Entity<Ticket>()
            .Property(t => t.Status)
            .HasConversion<string>();
    }
}



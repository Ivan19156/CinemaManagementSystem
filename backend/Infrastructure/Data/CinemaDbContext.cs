using Core.Entities;
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

    // 👇 Додай цей блок:
    modelBuilder.Entity<Session>()
        .HasOne(s => s.Hall)
        .WithMany(h => h.Sessions)
        .HasForeignKey(s => s.HallId)
        .OnDelete(DeleteBehavior.Cascade); // або Restrict, якщо не хочеш автоматичного видалення

    // 🔁 інші налаштування залишаються:

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

    modelBuilder.Entity<User>()
        .Property(u => u.Role)
        .HasConversion<string>();

    modelBuilder.Entity<Hall>()
        .Property(h => h.Type)
        .HasConversion<string>();

    modelBuilder.Entity<Session>()
        .Property(s => s.Status)
        .HasConversion<string>();

    modelBuilder.Entity<Ticket>()
        .Property(t => t.Status)
        .HasConversion<string>();
}

}



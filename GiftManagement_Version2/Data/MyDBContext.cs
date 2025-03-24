using Microsoft.EntityFrameworkCore;

namespace GiftManagement_Version2.Data
{
    public class MyDBContext : DbContext
    {
        public MyDBContext(DbContextOptions<MyDBContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Gift> Gifts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Ranking> Rankings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Bảng user
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Username)
                      .IsRequired()
                      .HasMaxLength(50)
                      .IsUnicode(false);

                entity.HasIndex(e => e.Username)
                      .IsUnique();

                entity.Property(e => e.Email)
                      .IsRequired()
                      .HasMaxLength(255)
                      .IsUnicode(false);
                entity.HasIndex(e => e.Email)
                      .IsUnique();

                entity.Property(e => e.Password)
                      .IsRequired()
                      .HasMaxLength(255)
                      .IsUnicode(false);

                entity.Property(e => e.Phone)
                      .HasMaxLength(10)
                      .IsUnicode(false);
                entity.HasIndex(e => e.Phone)
                      .IsUnique();

                entity.Property(e => e.RoleId)
                      .HasDefaultValue(2);

                entity.Property(e => e.Point)
                      .HasDefaultValue(0);

                entity.Property(e => e.AccumulatedPoint)
                      .HasDefaultValue(0);

                entity.HasOne(e => e.Role)
                      .WithMany(r => r.Users)
                      .HasForeignKey(e => e.RoleId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            //Bảng Permission
            modelBuilder.Entity<Permission>(entityBuilder =>
            {
                entityBuilder.HasKey(x => x.Code);
                entityBuilder.Property(x => x.Code).HasColumnType("char(25)");
                entityBuilder.Property(x => x.Name).HasColumnType("varchar(50)");
            });

            //Bảng RolePermission
            modelBuilder.Entity<RolePermission>()
                        .HasKey(rp => new { rp.RoleId, rp.PermissionCode });

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionCode)
                .OnDelete(DeleteBehavior.Cascade);

            //Bảng Cart
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasOne(e => e.User)
                      .WithMany(u => u.Carts)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Gift)
                      .WithMany(g => g.Carts)
                      .HasForeignKey(e => e.GiftId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.UserId, e.GiftId })
                      .IsUnique();

                //Status: 0: isActive, 1: notActive
                entity.Property(e => e.Status)
                      .HasDefaultValue(0);

            });

            //Bảng Gift
            modelBuilder.Entity<Gift>(entity =>
            {
                entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
                entity.Property(e => e.StartDate).HasColumnType("date");
                entity.Property(e => e.EndDate).HasColumnType("date");
                entity.Property(e => e.Image).HasMaxLength(255);
            });

            //Bảng Order
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasOne(e => e.User)
                      .WithMany(u => u.Orders)
                      .HasForeignKey(e => e.UserId);
                entity.HasOne(e => e.Gift)
                      .WithMany(g => g.Orders)
                      .HasForeignKey(e => e.GiftId);
                entity.HasOne(e => e.MainGift)
                      .WithMany()
                      .HasForeignKey(e => e.MainGiftId)
                      .OnDelete(DeleteBehavior.Restrict);

                //Cho phép trường MainGiftId có giá trị null
                entity.Property(e => e.MainGiftId)
                     .IsRequired(false);

            });


            //Bảng Promotion
            modelBuilder.Entity<Promotion>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();
                entity.HasOne(e => e.MainGift)
                      .WithMany(g => g.Promotions)
                      .HasForeignKey(e => e.MainGiftId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.BonusGift)
                      .WithMany()
                      .HasForeignKey(e => e.BonusGiftId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasIndex(e => new { e.MainGiftId, e.BonusGiftId }).IsUnique();
            });

            //Bảng Ranking
            modelBuilder.Entity<Ranking>(entity =>
            {
                entity.HasOne(e => e.User)
                      .WithMany(u => u.Rankings)
                      .HasForeignKey(e => e.UserId);
            });

            //DB Seed
            DbInitializer.Seed(modelBuilder);
        }
    }
}

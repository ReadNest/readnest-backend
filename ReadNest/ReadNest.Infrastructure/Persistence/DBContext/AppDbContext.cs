using Microsoft.EntityFrameworkCore;
using ReadNest.Domain.Base;
using ReadNest.Domain.Entities;
using ReadNest.Shared.Enums;

namespace ReadNest.Infrastructure.Persistence.DBContext
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _ = modelBuilder.Entity<User>()
                .ToTable("users")
                .HasKey(u => u.Id);

            _ = modelBuilder.Entity<User>()
                .Property(u => u.UserName)
                .HasColumnName("user_name")
                .HasMaxLength(255)
                .HasDefaultValue(string.Empty);

            _ = modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .HasColumnName("email")
                .HasMaxLength(255)
                .HasDefaultValue(string.Empty);

            _ = modelBuilder.Entity<User>()
                .Property(u => u.HashPassword)
                .HasColumnName("hash_password")
                .HasMaxLength(255)
                .HasDefaultValue(string.Empty);

            _ = modelBuilder.Entity<User>()
                .Property(u => u.Address)
                .HasColumnName("address")
                .HasMaxLength(500)
                .HasDefaultValue(string.Empty);

            _ = modelBuilder.Entity<User>()
                .Property(u => u.DateOfBirth)
                .HasColumnName("date_of_birth")
                .HasColumnType("date")
                .HasDefaultValue(DateTime.MinValue);

            _ = modelBuilder.Entity<Role>()
                .ToTable("roles")
                .HasKey(r => r.Id);

            _ = modelBuilder.Entity<Role>()
                .Property(r => r.RoleName)
                .HasColumnName("role_name")
                .HasMaxLength(255)
                .HasDefaultValue(string.Empty);

            _ = modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .HasConstraintName("fk_users_roles");

            _ = modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    Id = Guid.NewGuid(),
                    RoleName = RoleEnum.Admin.ToString(),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                },
                new Role
                {
                    Id = Guid.NewGuid(),
                    RoleName = RoleEnum.User.ToString(),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                }
            );

            _ = modelBuilder.Entity<RefreshToken>()
                .ToTable("refresh_tokens")
                .HasKey(rt => rt.Id);

            _ = modelBuilder.Entity<RefreshToken>()
                .Property(rt => rt.Token)
                .HasMaxLength(255)
                .IsRequired();

            _ = modelBuilder.Entity<RefreshToken>()
                .Property(rt => rt.ExpiryDate)
                .IsRequired();

            _ = modelBuilder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
                .WithMany()
                .HasForeignKey(rt => rt.UserId)
                .HasConstraintName("fk_refresh_tokens_users");


            base.OnModelCreating(modelBuilder);
        }


        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is IBaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var entity = (BaseEntity<Guid>)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                    entity.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property(nameof(BaseEntity<Guid>.CreatedAt)).IsModified = false;

                    entity.UpdatedAt = DateTime.UtcNow;
                }
            }
        }

    }
}

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
        public DbSet<Book> Books { get; set; }
        public DbSet<FavoriteBook> FavoriteBooks { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<AffiliateLink> AffiliateLinks { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _ = modelBuilder.Entity<User>()
                .ToTable("users")
                .HasKey(u => u.Id);

            _ = modelBuilder.Entity<User>()
                .Property(u => u.FullName)
                .HasColumnName("full_name")
                .HasMaxLength(100)
                .HasDefaultValue(string.Empty);

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

            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("books");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .HasColumnName("id")
                      .IsRequired();

                entity.Property(e => e.Title)
                      .HasColumnName("title")
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(e => e.Author)
                      .HasColumnName("author")
                      .IsRequired()
                      .HasMaxLength(150);

                entity.Property(e => e.ImageUrl)
                      .HasColumnName("image_url")
                      .HasMaxLength(300);

                entity.Property(e => e.AvarageRating)
                      .HasColumnName("average_rating");

                entity.Property(e => e.Description)
                      .HasColumnName("description")
                      .HasColumnType("text");
            });

            modelBuilder.Entity<FavoriteBook>(entity =>
            {
                entity.ToTable("favorite_books");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .HasColumnName("id")
                      .IsRequired();

                entity.Property(e => e.UserId)
                      .HasColumnName("user_id")
                      .IsRequired();

                entity.Property(e => e.BookId)
                      .HasColumnName("book_id")
                      .IsRequired();

                entity.HasOne(e => e.User)
                      .WithMany(u => u.FavoriteBooks)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Book)
                      .WithMany(b => b.FavoriteBooks)
                      .HasForeignKey(e => e.BookId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("categories");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id").IsRequired();
                entity.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasColumnName("description").HasColumnType("text");
            });

            modelBuilder.Entity<Book>()
                .HasMany(b => b.Categories)
                .WithMany(c => c.Books)
                .UsingEntity<Dictionary<string, object>>(
                    "book_categories",
                    j => j
                        .HasOne<Category>()
                        .WithMany()
                        .HasForeignKey("category_id")
                        .HasConstraintName("fk_book_categories_category_id")
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j
                        .HasOne<Book>()
                        .WithMany()
                        .HasForeignKey("book_id")
                        .HasConstraintName("fk_book_categories_book_id")
                        .OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.HasKey("book_id", "category_id");
                        j.ToTable("book_categories");
                });

            modelBuilder.Entity<AffiliateLink>(entity =>
            {
                entity.ToTable("affiliate_links");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .HasColumnName("id")
                      .IsRequired();

                entity.Property(e => e.Link)
                      .HasColumnName("link")
                      .IsRequired()
                      .HasMaxLength(500);

                entity.Property(e => e.PartnerName)
                      .HasColumnName("partner_name")
                      .IsRequired()
                      .HasMaxLength(150);

                entity.Property(e => e.BookId)
                      .HasColumnName("book_id")
                      .IsRequired();

                entity.HasOne(e => e.Book)
                      .WithMany(b => b.AffiliateLinks)
                      .HasForeignKey(e => e.BookId)
                      .HasConstraintName("fk_affiliate_links_book_id")
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("comments");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .HasColumnName("id")
                      .IsRequired();

                entity.Property(e => e.Content)
                      .HasColumnName("content")
                      .IsRequired()
                      .HasColumnType("text");

                entity.Property(e => e.BookId)
                      .HasColumnName("book_id")
                      .IsRequired();

                entity.Property(e => e.UserId)
                      .HasColumnName("user_id")
                      .IsRequired();

                entity.HasOne(e => e.Book)
                      .WithMany(b => b.Comments)
                      .HasForeignKey(e => e.BookId)
                      .HasConstraintName("fk_comments_book_id")
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Creator)
                      .WithMany(u => u.Comments)
                      .HasForeignKey(e => e.UserId)
                      .HasConstraintName("fk_comments_user_id")
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Comment>()
                .HasMany(c => c.Likes)
                .WithMany(u => u.LikedComments)
                .UsingEntity<Dictionary<string, object>>(
                    "comment_likes",
                    j => j
                        .HasOne<User>()
                        .WithMany()
                        .HasForeignKey("user_id")
                        .HasConstraintName("fk_comment_likes_user_id")
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j
                        .HasOne<Comment>()
                        .WithMany()
                        .HasForeignKey("comment_id")
                        .HasConstraintName("fk_comment_likes_comment_id")
                        .OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.HasKey("comment_id", "user_id");
                        j.ToTable("comment_likes");
                    });

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

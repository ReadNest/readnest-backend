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
        public DbSet<BookImage> BookImages { get; set; }
        public DbSet<CommentReport> CommentReports { get; set; }

        public DbSet<Post> Posts { get; set; }
        public DbSet<UserBadge> UserBadges { get; set; }
        public DbSet<Badge> Badges { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<TradingPost> TradingPosts { get; set; }
        public DbSet<TradingRequest> TradingRequests { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventReward> EventRewards { get; set; }
        public DbSet<Leaderboard> Leaderboards { get; set; }
        public DbSet<TradingPostImage> TradingPostImages { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<PackageFeature> PackageFeatures { get; set; }
        public DbSet<UserSubscription> UserSubscriptions { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

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
                .Property(u => u.AvatarUrl)
                .HasColumnName("avatar_url")
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

            _ = modelBuilder.Entity<User>()
                .Property(u => u.Bio)
                .HasColumnName("bio")
                .HasColumnType("text")
                .HasDefaultValue(string.Empty);

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

            _ = modelBuilder.Entity<Book>(entity =>
            {
                _ = entity.ToTable("books");

                _ = entity.HasKey(e => e.Id);

                _ = entity.Property(e => e.Id)
                      .HasColumnName("id")
                      .IsRequired();

                _ = entity.Property(e => e.Title)
                      .HasColumnName("title")
                      .IsRequired()
                      .HasMaxLength(200);

                _ = entity.Property(e => e.Author)
                      .HasColumnName("author")
                      .IsRequired()
                      .HasMaxLength(150);

                _ = entity.Property(e => e.TitleNormalized)
                      .HasColumnName("title_normalized")
                      .IsRequired()
                      .HasMaxLength(200);

                _ = entity.Property(e => e.AuthorNormalized)
                      .HasColumnName("author_normalized")
                      .IsRequired()
                      .HasMaxLength(150);

                _ = entity.Property(e => e.ImageUrl)
                      .HasColumnName("image_url")
                      .HasMaxLength(300);

                _ = entity.Property(e => e.AvarageRating)
                      .HasColumnName("average_rating");

                _ = entity.Property(e => e.Description)
                      .HasColumnName("description")
                      .HasColumnType("text");

                _ = entity.Property(e => e.DescriptionNormalized)
                      .HasColumnName("description_normalized")
                      .HasColumnType("text");

                _ = entity.Property(e => e.ISBN)
                      .HasColumnName("isbn")
                      .HasDefaultValue(string.Empty)
                      .HasMaxLength(255);

                _ = entity.Property(e => e.Language)
                      .HasColumnName("language")
                      .HasDefaultValue(string.Empty)
                      .HasMaxLength(50);
            });

            _ = modelBuilder.Entity<FavoriteBook>(entity =>
            {
                _ = entity.ToTable("favorite_books");

                _ = entity.HasKey(e => e.Id);

                _ = entity.Property(e => e.Id)
                      .HasColumnName("id")
                      .IsRequired();

                _ = entity.Property(e => e.UserId)
                      .HasColumnName("user_id")
                      .IsRequired();

                _ = entity.Property(e => e.BookId)
                      .HasColumnName("book_id")
                      .IsRequired();

                _ = entity.HasOne(e => e.User)
                      .WithMany(u => u.FavoriteBooks)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                _ = entity.HasOne(e => e.Book)
                      .WithMany(b => b.FavoriteBooks)
                      .HasForeignKey(e => e.BookId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            _ = modelBuilder.Entity<Category>(entity =>
            {
                _ = entity.ToTable("categories");

                _ = entity.HasKey(e => e.Id);

                _ = entity.Property(e => e.Id).HasColumnName("id").IsRequired();
                _ = entity.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(100);
                _ = entity.Property(e => e.Description).HasColumnName("description").HasColumnType("text");
            });

            _ = modelBuilder.Entity<Book>()
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
                        _ = j.HasKey("book_id", "category_id");
                        _ = j.ToTable("book_categories");
                    });

            _ = modelBuilder.Entity<AffiliateLink>(entity =>
            {
                _ = entity.ToTable("affiliate_links");

                _ = entity.HasKey(e => e.Id);

                _ = entity.Property(e => e.Id)
                      .HasColumnName("id")
                      .IsRequired();

                _ = entity.Property(e => e.Link)
                      .HasColumnName("link")
                      .IsRequired()
                      .HasMaxLength(500);

                _ = entity.Property(e => e.PartnerName)
                      .HasColumnName("partner_name")
                      .IsRequired()
                      .HasMaxLength(150);

                _ = entity.Property(e => e.BookId)
                      .HasColumnName("book_id")
                      .IsRequired();

                _ = entity.HasOne(e => e.Book)
                      .WithMany(b => b.AffiliateLinks)
                      .HasForeignKey(e => e.BookId)
                      .HasConstraintName("fk_affiliate_links_book_id")
                      .OnDelete(DeleteBehavior.Cascade);
            });

            _ = modelBuilder.Entity<Comment>(entity =>
            {
                _ = entity.ToTable("comments");

                _ = entity.HasKey(e => e.Id);

                _ = entity.Property(e => e.Id)
                      .HasColumnName("id")
                      .IsRequired();

                _ = entity.Property(e => e.Content)
                      .HasColumnName("content")
                      .IsRequired()
                      .HasColumnType("text");
                _ = entity.Property(e => e.Status)
                      .HasColumnName("status")
                      .IsRequired()
                      .HasColumnType("text");
                _ = entity.Property(e => e.BookId)
                      .HasColumnName("book_id")
                      .IsRequired();

                _ = entity.Property(e => e.UserId)
                      .HasColumnName("user_id")
                      .IsRequired();

                _ = entity.HasOne(e => e.Book)
                      .WithMany(b => b.Comments)
                      .HasForeignKey(e => e.BookId)
                      .HasConstraintName("fk_comments_book_id")
                      .OnDelete(DeleteBehavior.Cascade);

                _ = entity.HasOne(e => e.Creator)
                      .WithMany(u => u.Comments)
                      .HasForeignKey(e => e.UserId)
                      .HasConstraintName("fk_comments_user_id")
                      .OnDelete(DeleteBehavior.Cascade);
            });

            _ = modelBuilder.Entity<Comment>()
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
                        _ = j.HasKey("comment_id", "user_id");
                        _ = j.ToTable("comment_likes");
                    });

            _ = modelBuilder.Entity<BookImage>(entity =>
            {
                _ = entity.ToTable("book_images");

                _ = entity.HasKey(e => e.Id);

                _ = entity.Property(e => e.Id)
                      .HasColumnName("id")
                      .IsRequired();

                _ = entity.Property(e => e.BookId)
                      .HasColumnName("book_id")
                      .IsRequired();

                _ = entity.Property(e => e.ImageUrl)
                      .HasColumnName("image_url")
                      .IsRequired()
                      .HasMaxLength(500);

                _ = entity.Property(e => e.Order)
                      .HasColumnName("order")
                      .IsRequired();

                _ = entity.HasOne(e => e.Book)
                      .WithMany(b => b.BookImages)
                      .HasForeignKey(e => e.BookId)
                      .HasConstraintName("fk_book_images_book_id")
                      .OnDelete(DeleteBehavior.Cascade);
            });
            _ = modelBuilder.Entity<Post>(entity =>
            {
                _ = entity.ToTable("posts");

                _ = entity.HasKey(e => e.Id);

                _ = entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .IsRequired();

                _ = entity.Property(p => p.Title)
                    .HasColumnName("title")
                    .IsRequired()
                    .HasMaxLength(200); _ = entity.Property(e => e.Title);
                _ = entity.Property(e => e.Content)
                      .HasColumnName("content")
                      .IsRequired()
                      .HasColumnType("text");
                _ = entity.Property(e => e.BookId)
                      .HasColumnName("book_id")
                      .IsRequired();

                _ = entity.Property(e => e.UserId)
                      .HasColumnName("user_id")
                      .IsRequired();
                _ = entity.Property(p => p.Views)
                    .HasColumnName("views")
                    .HasDefaultValue(0);

                _ = entity.HasOne(e => e.Book)
                      .WithMany(b => b.Posts)
                      .HasForeignKey(e => e.BookId)
                      .HasConstraintName("fk_posts_book_id")
                      .OnDelete(DeleteBehavior.Cascade);

                _ = entity.HasOne(e => e.Creator)
                      .WithMany(u => u.Posts)
                      .HasForeignKey(e => e.UserId)
                      .HasConstraintName("fk_posts_user_id")
                      .OnDelete(DeleteBehavior.Cascade);
            });

            _ = modelBuilder.Entity<Post>()
                .HasMany(p => p.Likes)
                .WithMany(u => u.LikedPosts)
                .UsingEntity<Dictionary<string, object>>(
                    "post_likes",
                    j => j
                        .HasOne<User>()
                        .WithMany()
                        .HasForeignKey("user_id")
                        .HasConstraintName("fk_post_likes_user_id")
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j
                        .HasOne<Post>()
                        .WithMany()
                        .HasForeignKey("post_id")
                        .HasConstraintName("fk_post_likes_comment_id")
                        .OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        _ = j.HasKey("post_id", "user_id");
                        _ = j.ToTable("post_likes");
                    });
            _ = modelBuilder.Entity<CommentReport>(entity =>
            {
                _ = entity.ToTable("comment_reports");
                _ = entity.HasKey(e => e.Id);

                _ = entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .IsRequired();

                _ = entity.Property(e => e.ReporterId)
                    .HasColumnName("reporter_id")
                    .IsRequired();

                _ = entity.Property(e => e.CommentId)
                    .HasColumnName("comment_id")
                    .IsRequired();

                _ = entity.Property(e => e.Reason)
                    .HasColumnName("reason")
                    .IsRequired()
                    .HasColumnType("text");

                _ = entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .IsRequired()
                    .HasMaxLength(20); // Pending, NotViolated, Violated

                _ = entity.HasOne(e => e.Reporter)
                    .WithMany(u => u.Reports)
                    .HasForeignKey(e => e.ReporterId)
                    .HasConstraintName("fk_comment_reports_reporter_id")
                    .OnDelete(DeleteBehavior.Cascade);

                _ = entity.HasOne(e => e.Comment)
                    .WithMany(c => c.Reports)
                    .HasForeignKey(e => e.CommentId)
                    .HasConstraintName("fk_comment_reports_comment_id")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            _ = modelBuilder.Entity<UserBadge>(entity =>
            {
                _ = entity.ToTable("user_badges");
                _ = entity.HasKey(e => e.Id);
                _ = entity.Property(e => e.Id)
                      .HasColumnName("id")
                      .IsRequired();
                _ = entity.Property(e => e.UserId)
                      .HasColumnName("user_id")
                      .IsRequired();
                _ = entity.Property(e => e.BadgeId)
                      .HasColumnName("badge_id")
                      .IsRequired();
                _ = entity.HasOne(e => e.User)
                      .WithMany(u => u.UserBadges)
                      .HasForeignKey(e => e.UserId)
                      .HasConstraintName("fk_user_badges_user_id")
                      .OnDelete(DeleteBehavior.Cascade);
                _ = entity.HasOne(e => e.Badge)
                      .WithMany(b => b.UserBadges)
                      .HasForeignKey(e => e.BadgeId)
                      .HasConstraintName("fk_user_badges_badge_id")
                      .OnDelete(DeleteBehavior.Cascade);
            });

            _ = modelBuilder.Entity<Badge>(entity =>
            {
                _ = entity.ToTable("badges");
                _ = entity.HasKey(e => e.Id);
                _ = entity.Property(e => e.Id)
                      .HasColumnName("id")
                      .IsRequired();
                _ = entity.Property(e => e.Code)
                        .HasColumnName("code")
                        .IsRequired()
                        .HasMaxLength(50);
                _ = entity.HasIndex(e => e.Code)
                      .IsUnique();
                _ = entity.Property(e => e.Name)
                      .HasColumnName("name")
                      .IsRequired()
                      .HasMaxLength(100);
                _ = entity.Property(e => e.Description)
                      .HasColumnName("description")
                      .HasColumnType("text");
            });

            _ = modelBuilder.Entity<ChatMessage>(entity =>
            {
                _ = entity.ToTable("chat_messages");
                _ = entity.HasKey(e => e.Id);
                _ = entity.Property(e => e.Id)
                      .HasColumnName("id")
                      .IsRequired();
                _ = entity.Property(e => e.SenderId)
                      .HasColumnName("sender_id")
                      .IsRequired();
                _ = entity.Property(e => e.ReceiverId)
                      .HasColumnName("receiver_id")
                      .IsRequired();
                _ = entity.Property(e => e.Message)
                      .HasColumnName("message")
                      .IsRequired()
                      .HasColumnType("text");
                _ = entity.HasOne(e => e.Sender)
                      .WithMany(u => u.SentMessages)
                      .HasForeignKey(e => e.SenderId)
                      .HasConstraintName("fk_chat_messages_sender_id")
                      .OnDelete(DeleteBehavior.Cascade);
                _ = entity.HasOne(e => e.Receiver)
                      .WithMany(u => u.ReceivedMessages)
                      .HasForeignKey(e => e.ReceiverId)
                      .HasConstraintName("fk_chat_messages_receiver_id")
                      .OnDelete(DeleteBehavior.Cascade);
                _ = entity.Property(e => e.SentAt)
                        .HasColumnName("sent_at")
                        .IsRequired();
                _ = entity.Property(e => e.IsRead)
                      .HasColumnName("is_read")
                      .IsRequired()
                      .HasDefaultValue(false);
            });

            _ = modelBuilder.Entity<TradingPost>(entity =>
            {
                _ = entity.ToTable("trading_posts");
                _ = entity.HasKey(e => e.Id);
                _ = entity.Property(e => e.Id)
                      .HasColumnName("id")
                      .IsRequired();
                _ = entity.Property(e => e.Title)
                      .HasColumnName("title")
                      .IsRequired()
                      .HasMaxLength(200);
                _ = entity.Property(e => e.Status)
                      .HasColumnName("status")
                      .IsRequired()
                      .HasMaxLength(200);
                _ = entity.Property(e => e.Condition)
                      .HasColumnName("condition")
                      .IsRequired()
                      .HasMaxLength(200);
                _ = entity.Property(e => e.ShortDesc)
                      .HasColumnName("description")
                      .IsRequired()
                      .HasColumnType("text");
                _ = entity.Property(e => e.OwnerId)
                      .HasColumnName("owner_id")
                      .IsRequired();
                _ = entity.Property(e => e.ExternalBookUrl)
                      .HasColumnName("external_book_url")
                      .HasMaxLength(500);

                _ = entity.Property(e => e.Message)
                      .HasColumnName("message")
                      .HasColumnType("text");

                _ = entity.HasOne(e => e.Owner)
                      .WithMany(u => u.TradingPosts)
                      .HasForeignKey(e => e.OwnerId)
                      .HasConstraintName("fk_trading_posts_user_id")
                      .OnDelete(DeleteBehavior.Cascade);

                _ = entity.Property(e => e.MessageToRequester)
                      .HasColumnName("message_to_requester")
                      .HasMaxLength(500);
            });

            _ = modelBuilder.Entity<TradingRequest>(entity =>
            {
                _ = entity.ToTable("trading_requests");
                _ = entity.HasKey(e => e.Id);
                _ = entity.Property(e => e.Id)
                      .HasColumnName("id")
                      .IsRequired();
                _ = entity.Property(e => e.TradingPostId)
                      .HasColumnName("trading_post_id")
                      .IsRequired();
                _ = entity.Property(e => e.RequesterId)
                      .HasColumnName("requester_id")
                      .IsRequired();
                _ = entity.Property(e => e.Status)
                      .HasColumnName("status")
                      .IsRequired()
                      .HasMaxLength(20);
                _ = entity.HasOne(e => e.TradingPost)
                      .WithMany(tp => tp.TradingRequests)
                      .HasForeignKey(e => e.TradingPostId)
                      .HasConstraintName("fk_trading_requests_trading_post_id")
                      .OnDelete(DeleteBehavior.Cascade);
                _ = entity.HasOne(e => e.Requester)
                      .WithMany(u => u.TradingRequests)
                      .HasForeignKey(e => e.RequesterId)
                      .HasConstraintName("fk_trading_requests_requester_id")
                      .OnDelete(DeleteBehavior.Cascade);
            });

            _ = modelBuilder.Entity<Event>(entity =>
            {
                _ = entity.ToTable("events");
                _ = entity.HasKey(e => e.Id);
                _ = entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .IsRequired();
                _ = entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .IsRequired()
                    .HasMaxLength(100);
                _ = entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasColumnType("text");
                _ = entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .IsRequired();
                _ = entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .IsRequired();
                _ = entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .IsRequired()
                    .HasMaxLength(50);
                _ = entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .IsRequired()
                    .HasMaxLength(50);
            });

            _ = modelBuilder.Entity<EventReward>(entity =>
            {
                _ = entity.ToTable("event_rewards");
                _ = entity.HasKey(e => e.Id);
                _ = entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .IsRequired();
                _ = entity.Property(e => e.ConditionType)
                    .HasColumnName("condition_type")
                    .IsRequired()
                    .HasMaxLength(50);
                _ = entity.Property(e => e.Threshold)
                    .HasColumnName("threshold")
                    .IsRequired();
                _ = entity.Property(e => e.BadgeId)
                    .HasColumnName("badge_id")
                    .IsRequired();
                _ = entity.Property(e => e.EventId)
                    .HasColumnName("event_id")
                    .IsRequired();
                _ = entity.HasOne(e => e.Badge)
                    .WithMany(b => b.EventRewards)
                    .HasForeignKey(e => e.BadgeId)
                    .HasConstraintName("fk_event_rewards_badge_id")
                    .OnDelete(DeleteBehavior.Cascade);
                _ = entity.HasOne(e => e.Event)
                    .WithMany(ev => ev.Rewards)
                    .HasForeignKey(e => e.EventId)
                    .HasConstraintName("fk_event_rewards_event_id")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            _ = modelBuilder.Entity<Leaderboard>(entity =>
            {
                _ = entity.ToTable("leaderboards");
                _ = entity.HasKey(e => e.Id);
                _ = entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .IsRequired();
                _ = entity.Property(e => e.EventId)
                    .HasColumnName("event_id")
                    .IsRequired();
                _ = entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .IsRequired();
                _ = entity.Property(e => e.Rank)
                    .HasColumnName("rank")
                    .IsRequired();
                _ = entity.Property(e => e.Score)
                    .HasColumnName("score")
                    .IsRequired();
                _ = entity.HasOne(e => e.Event)
                    .WithMany(ev => ev.Leaderboards)
                    .HasForeignKey(e => e.EventId)
                    .HasConstraintName("fk_leaderboards_event_id")
                    .OnDelete(DeleteBehavior.Cascade);
                _ = entity.HasOne(e => e.User)
                    .WithMany(u => u.Leaderboards)
                    .HasForeignKey(e => e.UserId)
                    .HasConstraintName("fk_leaderboards_user_id")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            _ = modelBuilder.Entity<TradingPostImage>(entity =>
            {
                _ = entity.ToTable("trading_post_images");
                _ = entity.HasKey(e => e.Id);

                _ = entity.Property(e => e.Id)
                          .HasColumnName("id")
                          .IsRequired();

                _ = entity.Property(e => e.TradingPostId)
                          .HasColumnName("trading_post_id")
                          .IsRequired();

                _ = entity.Property(e => e.ImageUrl)
                          .HasColumnName("image_url")
                          .IsRequired()
                          .HasMaxLength(1000);

                _ = entity.Property(e => e.Order)
                          .HasColumnName("order")
                          .IsRequired();

                _ = entity.HasOne(e => e.TradingPost)
                          .WithMany(p => p.Images)
                          .HasForeignKey(e => e.TradingPostId)
                          .HasConstraintName("fk_trading_post_images_post_id")
                          .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Package>().ToTable("packages");
            modelBuilder.Entity<Feature>().ToTable("features");
            modelBuilder.Entity<UserSubscription>().ToTable("user_subscriptions");
            modelBuilder.Entity<Transaction>().ToTable("transactions");
            modelBuilder.Entity<PackageFeature>().ToTable("package_features");

            // UserSubscription
            modelBuilder.Entity<UserSubscription>()
                .Property(us => us.UserId).HasColumnName("user_id");
            modelBuilder.Entity<UserSubscription>()
                .Property(us => us.PackageId).HasColumnName("package_id");
            modelBuilder.Entity<UserSubscription>()
                .Property(us => us.StartDate).HasColumnName("start_date");
            modelBuilder.Entity<UserSubscription>()
                .Property(us => us.EndDate).HasColumnName("end_date");
            modelBuilder.Entity<UserSubscription>()
                .Property(us => us.Status).HasColumnName("status");

            // Package
            modelBuilder.Entity<Package>()
                .Property(p => p.Name).HasColumnName("name");
            modelBuilder.Entity<Package>()
                .Property(p => p.Price).HasColumnName("price");
            modelBuilder.Entity<Package>()
                .Property(p => p.DurationMonths).HasColumnName("duration_months");
            modelBuilder.Entity<Package>()
                .Property(p => p.Features).HasColumnName("features");

            // Feature
            modelBuilder.Entity<Feature>()
                .Property(f => f.Name).HasColumnName("name");
            modelBuilder.Entity<Feature>()
                .Property(f => f.Description).HasColumnName("description");

            // Transaction
            modelBuilder.Entity<Transaction>()
                .Property(t => t.UserId).HasColumnName("user_id");
            modelBuilder.Entity<Transaction>()
                .Property(t => t.PackageId).HasColumnName("package_id");
            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount).HasColumnName("amount");
            modelBuilder.Entity<Transaction>()
                .Property(t => t.PaymentMethod).HasColumnName("payment_method");
            modelBuilder.Entity<Transaction>()
                .Property(t => t.TransactionStatus).HasColumnName("transaction_status");

            modelBuilder.Entity<PackageFeature>()
                .HasKey(pf => new { pf.PackageId, pf.FeatureId });

            modelBuilder.Entity<PackageFeature>()
                .Property(pf => pf.PackageId).HasColumnName("package_id");
            modelBuilder.Entity<PackageFeature>()
                .Property(pf => pf.FeatureId).HasColumnName("feature_id");

            // Config relationships
            modelBuilder.Entity<UserSubscription>()
                .HasOne(us => us.User)
                .WithMany(u => u.UserSubscriptions)
                .HasForeignKey(us => us.UserId);

            modelBuilder.Entity<UserSubscription>()
                .HasOne(us => us.Package)
                .WithMany(p => p.UserSubscriptions)
                .HasForeignKey(us => us.PackageId);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.User)
                .WithMany(u => u.Transactions)
                .HasForeignKey(t => t.UserId);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Package)
                .WithMany(p => p.Transactions)
                .HasForeignKey(t => t.PackageId);

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

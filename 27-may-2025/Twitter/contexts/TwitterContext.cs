using Twitter.Models;
using Microsoft.EntityFrameworkCore;
namespace Twitter.Contexts
{
    public class TwitterContext : DbContext
    {
        public TwitterContext(DbContextOptions<TwitterContext> options) : base(options)
        {
        }

        public DbSet<User> users { get; set; }
        public DbSet<Post> posts { get; set; }
        public DbSet<Likes> likes { get; set; }
        public DbSet<Follow> follows { get; set; }
        public DbSet<Hashtag> hashtags { get; set; }
        public DbSet<HashtagPost> hashtagPosts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HashtagPost>()
                .HasKey(hp => new { hp.PostId, hp.HashtagId });
            base.OnModelCreating(modelBuilder);
        }
    }
}
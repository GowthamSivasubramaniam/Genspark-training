

using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Context
{
    public class VideoContext : DbContext
    {
        public VideoContext(DbContextOptions<VideoContext> options) : base(options)
        {
        }
        public DbSet<Video> Videos { get; set; }

    }
}
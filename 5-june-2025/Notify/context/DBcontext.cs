
using Notify.Models;
using Microsoft.EntityFrameworkCore;

namespace Notify.Contexts
{
    public class NotifyContext : DbContext
    {
        public NotifyContext(DbContextOptions<NotifyContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<Files>()
      .HasOne(p => p.user)
      .WithMany(u => u.Files)
      .HasForeignKey(p => p.Umail) // matches FK property name
      .HasPrincipalKey(u => u.Mail)   // matches PK in User
      .HasConstraintName("FK_User_File")
      .OnDelete(DeleteBehavior.Restrict);



        }



        public DbSet<Files> files { get; set; }

        public DbSet<User> Users { get; set; }

    }
}

using BankingApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Contexts
{
    public class BankContext : DbContext
    {
        public BankContext(DbContextOptions<BankContext> options) : base(options) {}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Transaction>().HasOne(app => app.FromAccount)
                                              .WithMany(p => p.SentTransactions)
                                              .HasForeignKey(t => t.FromAccountNo)
                                              .HasConstraintName("FK_Transactions_FromACNo")
                                              .OnDelete(DeleteBehavior.Restrict);
                                              

              modelBuilder.Entity<Transaction>().HasOne(app => app.ToAccount)
                                              .WithMany(p => p.ReceivedTransactions)
                                              .HasForeignKey(app => app.ToAccountNo)
                                              .HasConstraintName("FK_Transactions_ToACN")
                                              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Account>().HasKey(a => a.AccountNo);

            modelBuilder.Entity<Account>().HasOne(ds => ds.user)
                                                   .WithMany(d => d.Accounts)
                                                   .HasForeignKey(ds => ds.UserId)
                                                   .HasConstraintName("FK_Account_UID")
                                                   .OnDelete(DeleteBehavior.Restrict);

        }

        public DbSet<Account> accounts { get; set; }
        public DbSet<Transaction> transactions { get; set; }
        public DbSet<User> users { get; set; }
     
    }
}

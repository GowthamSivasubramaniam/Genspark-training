using VSM.Models;
using Microsoft.EntityFrameworkCore;

namespace VSM.Contexts
{
    public class VSMContext : DbContext
    {
        public VSMContext(DbContextOptions<VSMContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Customer>().HasOne(c => c.User)
                                            .WithOne(u => u.Customer)
                                            .HasForeignKey<Customer>(c => c.Email)
                                            .HasConstraintName("FK_Customer_User")
                                            .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Mechanic>().HasOne(c => c.User)
                                            .WithOne(u => u.Mechanic)
                                            .HasForeignKey<Mechanic>(c => c.Email)
                                            .HasConstraintName("FK_Mechanic_User")
                                            .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ServiceCategory>().HasMany(s => s.Services)
                                                  .WithMany(se => se.ServiceCategories);

            modelBuilder.Entity<ServiceRecord>().HasOne(s => s.Customer)
                                                .WithMany(c => c.ServiceRecords)
                                                .HasForeignKey(s => s.CustomerID)
                                                .HasConstraintName("FK_ServiceRecord_Customer")
                                                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ServiceRecord>().HasOne(s => s.Mechanic)
                                                .WithMany(c => c.ServiceRecords)
                                                .HasForeignKey(s => s.MechanicId)
                                                .HasConstraintName("FK_ServiceRecord_Mechanic")
                                                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ServiceRecord>().HasOne(s => s.Booking)
                                                .WithOne(c => c.ServiceRecord)
                                                .HasForeignKey<ServiceRecord>(s => s.BookingID)
                                                .HasConstraintName("FK_ServiceRecord_Booking")
                                                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ServiceRecord>().HasOne(s => s.Service)
                                                .WithOne(c => c.ServiceRecord)
                                                .HasForeignKey<ServiceRecord>(s => s.ServiceID)
                                                .HasConstraintName("FK_ServiceRecord_Service")
                                                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Bill>().HasOne(s => s.ServiceRecord)
                                                .WithOne(c => c.Bill)
                                                .HasForeignKey<Bill>(s => s.ServiceRecordID)
                                                .HasConstraintName("FK_Bill_ServiceRecord")
                                                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Service>().HasOne(s => s.Vehicle)
                                                .WithMany(c => c.Services)
                                                .HasForeignKey(s => s.VehicleID)
                                                .HasConstraintName("FK_Service_Vehicle")
                                                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Booking>().HasOne(s => s.Customer)
                                                .WithMany(c => c.Bookings)
                                                .HasForeignKey(s => s.CustomerID)
                                                .HasConstraintName("FK_Booking_Customer")
                                                .OnDelete(DeleteBehavior.Restrict);
            
            
        
                
            
            

            
        }
        
         public DbSet<User> Users { get; set; }
         public DbSet<Bill> Bills { get; set; }
         public DbSet<Booking> Bookings { get; set; }
         public DbSet<Customer> Customers { get; set; }
         public DbSet<Mechanic> Mechanics { get; set; }
         public DbSet<Service> Services { get; set; }
         public DbSet<ServiceCategory> ServiceCategories { get; set; }
         public DbSet<ServiceRecord> serviceRecords { get; set; }
         public DbSet<Vehicle> Vehicles { get; set; }
    }
}
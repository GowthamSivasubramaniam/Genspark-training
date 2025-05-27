
using DocApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DocApp.Contexts
{
    public class ClinicContext : DbContext
    {
        public ClinicContext(DbContextOptions<ClinicContext> options) : base(options) {}
        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     modelBuilder.Entity<DoctorSpeciality>()
        //         .HasKey(ds => new { ds.DoctorId, ds.SpecialityId });
        // }

        public DbSet<Patient> patients { get; set; }
        public DbSet<Appointmnet> appointments { get; set; }
        public DbSet<Doctor> doctors { get; set; }
        public DbSet<Speciality> specialities { get; set; }
        public DbSet<DoctorSpeciality> doctorSpecialities { get; set; }
    }
}

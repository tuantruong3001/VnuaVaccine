using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DAL.EF
{
    public partial class VaccineDbContext : DbContext
    {
        public VaccineDbContext()
            : base("name=VaccineDbContext")
        {
        }

        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<MedicalStaff> MedicalStaffs { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<VaccinationManagement> VaccinationManagements { get; set; }
        public virtual DbSet<VaccinationSchedule> VaccinationSchedules { get; set; }
        public virtual DbSet<Vaccine> Vaccines { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MedicalStaff>()
                .HasMany(e => e.VaccinationManagements)
                .WithOptional(e => e.MedicalStaff)
                .HasForeignKey(e => e.IdStaff);

            modelBuilder.Entity<Patient>()
                .HasMany(e => e.Appointments)
                .WithOptional(e => e.Patient)
                .HasForeignKey(e => e.IdPatient);

            modelBuilder.Entity<Patient>()
                .HasMany(e => e.VaccinationSchedules)
                .WithOptional(e => e.Patient)
                .HasForeignKey(e => e.IdPatient);

            modelBuilder.Entity<Role>()
                .HasMany(e => e.Users)
                .WithOptional(e => e.Role1)
                .HasForeignKey(e => e.Role);

            modelBuilder.Entity<User>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Email)
                .IsUnicode(false);
         
            modelBuilder.Entity<User>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.MedicalStaffs)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.IdUserName);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Patients)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.IdUserName);

            modelBuilder.Entity<VaccinationSchedule>()
                .HasMany(e => e.Appointments)
                .WithOptional(e => e.VaccinationSchedule)
                .HasForeignKey(e => e.IdSchedule);

            modelBuilder.Entity<VaccinationSchedule>()
                .HasMany(e => e.VaccinationManagements)
                .WithOptional(e => e.VaccinationSchedule)
                .HasForeignKey(e => e.IdSchedule);

            modelBuilder.Entity<Vaccine>()
                .HasMany(e => e.VaccinationSchedules)
                .WithOptional(e => e.Vaccine)
                .HasForeignKey(e => e.IdVaccine);
        }
    }
}

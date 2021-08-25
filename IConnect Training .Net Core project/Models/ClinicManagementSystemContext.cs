using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace IConnect_Training_.Net_Core_project.Models
{
    public partial class ClinicManagementSystemContext : DbContext
    {
        public ClinicManagementSystemContext()
        {
        }

        public ClinicManagementSystemContext(DbContextOptions<ClinicManagementSystemContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<AppointmentType> AppointmentTypes { get; set; }
        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<MedicalHistory> MedicalHistories { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Specialization> Specializations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=Clinic Management System;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasOne(d => d.AppointmentNavigation)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.AppointmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Appointments_AppointmentTypes");

                entity.HasOne(d => d.Doctor)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.DoctorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Appointments_Doctors");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.PatientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Appointments_Patients");
            });

            modelBuilder.Entity<AppointmentType>(entity =>
            {
                entity.Property(e => e.Type).HasMaxLength(50);
            });

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.Property(e => e.Address).HasMaxLength(100);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.Iban).HasColumnName("IBAN");

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.MonthlySalary).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Notes).HasColumnType("text");

                entity.HasOne(d => d.Specialization)
                    .WithMany(p => p.Doctors)
                    .HasForeignKey(d => d.SpecializationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Doctors_Specializations");
            });

            modelBuilder.Entity<MedicalHistory>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(50);

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.MedicalHistories)
                    .HasForeignKey(d => d.PatientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MedicalHistories_Patients");
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.Property(e => e.Address).HasMaxLength(100);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.Ssn).HasColumnName("SSN");
            });

            modelBuilder.Entity<Specialization>(entity =>
            {
                entity.Property(e => e.SpecializationName).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

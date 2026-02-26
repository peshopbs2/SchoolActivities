using Microsoft.EntityFrameworkCore;
using SchoolActivities.Models.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolActivities.Data.Persistance
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<Student> Students => Set<Student>();
        public DbSet<Activity> Activities => Set<Activity>();
        public DbSet<Enrollment> Enrollments => Set<Enrollment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.HasKey(x => new { x.StudentId, x.ActivityId });

                entity.HasOne(x => x.Student)
                    .WithMany(s => s.Enrollments)
                    .HasForeignKey(x => x.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.Activity)
                    .WithMany(a => a.Enrollments)
                    .HasForeignKey(x => x.ActivityId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(x => x.EnrolledAtUtc)
                    .IsRequired();
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(x => x.FirstName).HasMaxLength(50).IsRequired();
                entity.Property(x => x.LastName).HasMaxLength(50).IsRequired();
            });

            modelBuilder.Entity<Activity>(entity =>
            {
                entity.Property(x => x.Name).HasMaxLength(120).IsRequired();
                entity.Property(x => x.Description).HasMaxLength(500);
            });
        }
    }
}

using Lexicon_LMS_G1.Entities.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Lexicon_LMS_G1.Data.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityType> ActivitiesTypes { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<UserFinishedActivity> UserFinishedActivity { get; set; }
        public DbSet<CourseDocument> CourseDocuments { get; set; }
        public DbSet<ModuleDocument> ModuleDocuments { get; set; }
        public DbSet<ActivityDocument> ActivityDocuments { get; set; }
        public DbSet<StudentDocument> StudentDocuments { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserFinishedActivity>().HasKey(ufa => new {ufa.ApplicationUserId, ufa.ActivityId});


            builder.Entity<Course>()
                .HasMany(e => e.Documents)
                .WithOne(s => s.Course)
                .HasForeignKey(s => s.CourseId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Module>()
                .HasMany(e => e.Documents)
                .WithOne(s => s.Module)
                .HasForeignKey(s => s.ModuleId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ModuleDocument>()
                .HasOne(e => e.Module)
                .WithMany(s => s.Documents)
                .HasForeignKey(s => s.ModuleId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Course>()
                .HasMany(c => c.AttendingStudents)
                .WithOne(s => s.Course)
                .HasForeignKey(s => s.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserFinishedActivity>()
                .HasOne(u => u.Activity)
                .WithMany(a => a.FinishedActivities)
                .HasForeignKey(a => a.ActivityId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<UserFinishedActivity>()
                .HasOne(u => u.ApplicationUser)
                .WithMany(a => a.FinishedActivities)
                .HasForeignKey(a => a.ApplicationUserId)
                .OnDelete(DeleteBehavior.NoAction);

        }

    }
}
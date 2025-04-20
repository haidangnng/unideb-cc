using Microsoft.EntityFrameworkCore;
using Shared.Models;
using Shared.Models.Courses;
using Shared.Models.Users;
namespace QuestionLair.API.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<TeacherProfile> TeacherProfiles => Set<TeacherProfile>();
    public DbSet<StudentProfile> StudentProfiles => Set<StudentProfile>();

    public DbSet<Course> Courses { get; set; } = default!;
    public DbSet<Material> Materials { get; set; } = default!;
    public DbSet<StudentCourse> StudentCourses { get; set; } = default!;
    public DbSet<TeacherCourse> TeacherCourses { get; set; } = default!;
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // User has one profile per role (1:1 relationships)
        modelBuilder.Entity<User>()
            .HasOne(u => u.TeacherProfile)
            .WithOne(p => p.User)
            .HasForeignKey<TeacherProfile>(p => p.Id);

        modelBuilder.Entity<User>()
            .HasOne(u => u.StudentProfile)
            .WithOne(p => p.User)
            .HasForeignKey<StudentProfile>(p => p.Id);

        modelBuilder.Entity<User>()
            .HasOne(u => u.AdminProfile)
            .WithOne(p => p.User)
            .HasForeignKey<AdminProfile>(p => p.UserId);

        // Explicitly define primary keys for the profiles
        modelBuilder.Entity<TeacherProfile>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<StudentProfile>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<AdminProfile>()
            .HasKey(p => p.UserId);

        modelBuilder.Entity<StudentCourse>()
            .HasKey(sc => new { sc.StudentProfileId, sc.CourseId });

        modelBuilder.Entity<StudentCourse>()
            .HasOne(sc => sc.StudentProfile)
            .WithMany(sp => sp.Courses)
            .HasForeignKey(sc => sc.StudentProfileId);

        modelBuilder.Entity<StudentCourse>()
            .HasOne(sc => sc.Course)
            .WithMany(c => c.StudentCourses)
            .HasForeignKey(sc => sc.CourseId);

        // TeacherCourse relationship
        modelBuilder.Entity<TeacherCourse>()
            .HasKey(tc => new { tc.TeacherProfileId, tc.CourseId });

        modelBuilder.Entity<TeacherCourse>()
            .HasOne(tc => tc.TeacherProfile)
            .WithMany(tp => tp.Courses)
            .HasForeignKey(tc => tc.TeacherProfileId);

        modelBuilder.Entity<TeacherCourse>()
            .HasOne(tc => tc.Course)
            .WithMany(c => c.TeacherCourses)
            .HasForeignKey(tc => tc.CourseId);
        base.OnModelCreating(modelBuilder);
    }
}
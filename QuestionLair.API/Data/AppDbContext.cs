using Microsoft.EntityFrameworkCore;
using Shared.Models;
using Shared.Models.Courses;
using Shared.Models.Users;
using Shared.Models.Tests;

namespace QuestionLair.API.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<TeacherProfile> TeacherProfiles => Set<TeacherProfile>();
    public DbSet<StudentProfile> StudentProfiles => Set<StudentProfile>();
    public DbSet<AdminProfile> AdminProfiles => Set<AdminProfile>();

    public DbSet<Course> Courses { get; set; } = default!;
    public DbSet<Material> Materials { get; set; } = default!;
    public DbSet<StudentCourse> StudentCourses { get; set; } = default!;
    public DbSet<TeacherCourse> TeacherCourses { get; set; } = default!;

    public DbSet<Test> Tests { get; set; } = default!;
    public DbSet<TestMaterial> TestMaterials { get; set; } = default!;
    public DbSet<TestQuestion> TestQuestions { get; set; } = default!;

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

        modelBuilder.Entity<TeacherProfile>().HasKey(p => p.Id);
        modelBuilder.Entity<StudentProfile>().HasKey(p => p.Id);
        modelBuilder.Entity<AdminProfile>().HasKey(p => p.UserId);

        // StudentCourse relationship
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

        // TestMaterial relationship (Test <-> Material Many to Many)
        modelBuilder.Entity<TestMaterial>()
            .HasKey(tm => new { tm.TestId, tm.MaterialId });

        modelBuilder.Entity<TestMaterial>()
            .HasOne(tm => tm.Test)
            .WithMany(t => t.TestMaterials)
            .HasForeignKey(tm => tm.TestId);

        modelBuilder.Entity<TestMaterial>()
            .HasOne(tm => tm.Material)
            .WithMany() // (or .WithMany(m => m.TestMaterials) if you want back-reference)
            .HasForeignKey(tm => tm.MaterialId);

        modelBuilder.Entity<TestQuestion>()
            .HasOne(q => q.Test)
            .WithMany(t => t.Questions)
            .HasForeignKey(q => q.TestId)
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }
}

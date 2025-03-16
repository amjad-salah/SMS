using Microsoft.EntityFrameworkCore;
using SMS.Models.Entities;

namespace SMS.Backend.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        var entries = ChangeTracker.Entries<BaseEntity>()
            .Where(e =>
                e.State is EntityState.Added or EntityState.Modified);

        foreach (var entry in entries)
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.Now;
                entry.Entity.UpdatedAt = DateTime.Now;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.Now;
            }

        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = new())
    {
        var entries = ChangeTracker.Entries<BaseEntity>()
            .Where(e =>
                e.State is EntityState.Added or EntityState.Modified);

        foreach (var entry in entries)
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }

        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    #region Entities

    public DbSet<Institution> Institutions { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Grade> Grades { get; set; }
    public DbSet<AcademicYear> AcademicYears { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<StudentRecord> StudentRecords { get; set; }
    public DbSet<Application> Applications { get; set; }
    public DbSet<Class> Classes { get; set; }
    public DbSet<Announcement> Announcements { get; set; }
    public DbSet<Assignment> Assignments { get; set; }
    public DbSet<Attendance> Attendances { get; set; }
    public DbSet<Exam> Exams { get; set; }
    public DbSet<ExamResult> ExamResults { get; set; }
    public DbSet<ExamType> ExamTypes { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<Parent> Parents { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Fee> Fees { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<InvoiceItem> InvoiceItems { get; set; }
    public DbSet<Payment> Payments { get; set; }

    #endregion
}

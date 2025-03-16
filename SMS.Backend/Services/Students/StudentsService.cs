using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SMS.Backend.Data;
using SMS.Models.DTOs.Students;
using SMS.Models.Entities;

namespace SMS.Backend.Services.Students;

public class StudentsService(
    AppDbContext context,
    IValidator<UpsertStudentDto> validator) : IStudentsService
{
    public async Task<StudentsResponseDto> GetStudentsAsync()
    {
        var students = await context.Students.AsNoTracking()
            .OrderByDescending(s => s.CreatedAt)
            .Include(s => s.AcademicYear)
            .Include(s => s.Grade)
            .Include(s => s.Class)
            .ProjectToType<StudentDto>()
            .ToListAsync();

        return new StudentsResponseDto() { Success = true, Students = students };
    }

    public async Task<StudentsResponseDto> GetStudentByIdAsync(int id, int userId)
    {
        var user = await context.Users.AsNoTracking()
            .Include(u => u.Student)
            .Include(u => u.Parent)
            .FirstOrDefaultAsync(u => u.Id == userId);

        var student = await context.Students.AsNoTracking()
            .Include(s => s.AcademicYear)
            .Include(s => s.Grade)
            .Include(s => s.Class)
            .Include(s => s.Institution)
            .Include(s => s.Records)
            .Include(s => s.Attendances)
            .Include(s => s.Invoices)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (student == null)
            return new StudentsResponseDto() { Success = false, Message = "Student not found" };

        if (user is { Role: UserRole.Student or UserRole.Parent })
        {
            if (user.Student != null && user.Student.Id != student.Id)
                return new StudentsResponseDto()
                {
                    Success = false,
                    Message = "You don't have permission to view this student"
                };

            if (user.Parent != null && user.Parent.Id != student.ParentId)
                return new StudentsResponseDto()
                {
                    Success = false,
                    Message = "You don't have permission to view this student"
                };
        }

        return new StudentsResponseDto() { Success = true, Student = student.Adapt<StudentDetailsDto>() };
    }

    public async Task<StudentsResponseDto> GetActiveStudentsAsync()
    {
        var students = await context.Students.AsNoTracking()
            .Where(s => s.Status == StudentStatus.Active)
            .OrderByDescending(s => s.CreatedAt)
            .Include(s => s.AcademicYear)
            .Include(s => s.Grade)
            .Include(s => s.Class)
            .ProjectToType<StudentDto>()
            .ToListAsync();

        return new StudentsResponseDto() { Success = true, Students = students };
    }

    public async Task<StudentsResponseDto> AddStudentAsync(UpsertStudentDto student)
    {
        var validationResult = await validator.ValidateAsync(student);

        if (!validationResult.IsValid)
        {
            var error = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));

            Log.Error(error);

            return new StudentsResponseDto() { Success = false, Message = error };
        }

        var studentNumber = context.Students.Any() ? context.Students.Max(s => s.Id) + 1 : 1;

        var newStudent = student.Adapt<Student>();

        newStudent.StudentNo = $"STO{studentNumber}";

        await context.Students.AddAsync(newStudent);
        await context.SaveChangesAsync();

        return new StudentsResponseDto()
        {
            Success = true,
            Message = "Student added successfully!",
            Student = newStudent.Adapt<StudentDetailsDto>()
        };
    }

    public async Task<StudentsResponseDto> UpdateStudentAsync(int id, UpsertStudentDto student)
    {
        var existingStudent = await context.Students.FindAsync(id);

        if (existingStudent == null)
            return new StudentsResponseDto() { Success = false, Message = "Student not found" };

        var validationResult = await validator.ValidateAsync(student);

        if (!validationResult.IsValid)
        {
            var error = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));

            Log.Error(error);

            return new StudentsResponseDto() { Success = false, Message = error };
        }

        existingStudent.ClassId = student.ClassId;
        existingStudent.GradeId = student.GradeId;
        existingStudent.AcademicYearId = student.AcademicYearId;
        existingStudent.Gender = student.Gender;
        existingStudent.InstitutionId = student.InstitutionId;
        existingStudent.ParentId = student.ParentId;
        existingStudent.FullName = student.FullName;
        existingStudent.Status = student.Status;
        existingStudent.BirthDate = student.BirthDate;
        existingStudent.MedicalInfo = student.MedicalInfo;
        existingStudent.AdmissionDate = student.AdmissionDate;

        await context.SaveChangesAsync();

        return new StudentsResponseDto() { Success = true, Message = "Student updated successfully!" };
    }

    public async Task<StudentsResponseDto> DeleteStudentAsync(int id)
    {
        var existingStudent = await context.Students.FindAsync(id);

        if (existingStudent == null)
            return new StudentsResponseDto() { Success = false, Message = "Student not found" };

        context.Students.Remove(existingStudent);
        await context.SaveChangesAsync();

        return new StudentsResponseDto() { Success = true, Message = "Student deleted successfully!" };
    }

    public async Task<StudentsResponseDto> TransferStudentAsync(int studentId, int classId, int userId)
    {
        var existingStudent = await context.Students.FindAsync(studentId);

        if (existingStudent == null)
            return new StudentsResponseDto() { Success = false, Message = "Student not found" };

        var existingClass = await context.Classes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == classId);

        if (existingClass == null)
            return new StudentsResponseDto() { Success = false, Message = "Class not found" };

        var currentYear = await context.AcademicYears.AsNoTracking()
            .FirstOrDefaultAsync(a => a.IsCurrent);

        if (currentYear == null)
            return new StudentsResponseDto()
            {
                Success = false,
                Message = "No current academic year available"
            };

        var user = await context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);

        // ReSharper disable once NullableWarningSuppressionIsUsed
        if (user!.Role == UserRole.Teacher)
        {
            var teacher = await context.Teachers.AsNoTracking().FirstOrDefaultAsync(t =>
                t.UserId == userId);

            var stClass = await context.Classes.AsNoTracking().FirstOrDefaultAsync(c =>
                c.Id == existingStudent.ClassId);

            // ReSharper disable once NullableWarningSuppressionIsUsed
            if (teacher!.Id != stClass!.TeacherId)
            {
                Log.Error("Teacher is not class's teacher");
                return new StudentsResponseDto() { Success = false, Message = "Teacher is not class's teacher" };
            }
        }

        existingStudent.AcademicYearId = currentYear.Id;
        existingStudent.ClassId = existingClass.Id;
        existingStudent.GradeId = existingClass.GradeId;

        return new StudentsResponseDto() { Success = true, Message = "Student transferred successfully!" };
    }

    public async Task<StudentsResponseDto> TransferStudentsAsync(List<int> studentIds, int classId, int userId)
    {
        var user = await context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);

        var existingClass = await context.Classes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == classId);

        if (existingClass == null)
            return new StudentsResponseDto() { Success = false, Message = "Class not found" };

        var currentYear = await context.AcademicYears.AsNoTracking()
            .FirstOrDefaultAsync(a => a.IsCurrent);

        if (currentYear == null)
            return new StudentsResponseDto()
            {
                Success = false,
                Message = "No current academic year available"
            };

        foreach (var id in studentIds)
        {
            var existingStudent = await context.Students.FindAsync(id);

            if (existingStudent == null)
                return new StudentsResponseDto() { Success = false, Message = "Student not found" };

            // ReSharper disable once NullableWarningSuppressionIsUsed
            if (user!.Role == UserRole.Teacher)
            {
                var teacher = await context.Teachers.AsNoTracking().FirstOrDefaultAsync(t =>
                    t.UserId == userId);

                var stClass = await context.Classes.AsNoTracking().FirstOrDefaultAsync(c =>
                    c.Id == existingStudent.ClassId);

                // ReSharper disable once NullableWarningSuppressionIsUsed
                if (teacher!.Id != stClass!.TeacherId)
                {
                    Log.Error("Teacher is not class's teacher");
                    return new StudentsResponseDto() { Success = false, Message = "Teacher is not class's teacher" };
                }
            }

            existingStudent.AcademicYearId = currentYear.Id;
            existingStudent.ClassId = existingClass.Id;
            existingStudent.GradeId = existingClass.GradeId;

            await context.SaveChangesAsync();
        }

        return new StudentsResponseDto() { Success = true, Message = "Students transferred successfully!" };
    }
}
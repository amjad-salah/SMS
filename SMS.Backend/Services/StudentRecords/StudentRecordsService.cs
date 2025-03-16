using System.Globalization;
using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SMS.Backend.Data;
using SMS.Models.DTOs.StudentRecords;
using SMS.Models.Entities;

namespace SMS.Backend.Services.StudentRecords;

public class StudentRecordsService(
    AppDbContext context,
    IValidator<UpsertStudentRecordDto> validator) : IStudentRecordsService
{
    public async Task<StudentRecordsResponseDto> GetRecordsAsync()
    {
        var records = await context.StudentRecords.AsNoTracking()
            .OrderByDescending(r => r.CreatedAt)
            .Include(r => r.Student)
            .Include(r => r.AcademicYear)
            .ProjectToType<StudentRecordDto>()
            .ToListAsync();

        return new StudentRecordsResponseDto() { Success = true, Records = records };
    }

    public async Task<StudentRecordsResponseDto> GetRecordsByStudentAsync(int studentId, int userId)
    {
        var user = await context.Users.AsNoTracking()
            .Include(u => u.Student)
            .Include(u => u.Parent)
            .FirstOrDefaultAsync(u => u.Id == userId);

        var student = await context.Students.AsNoTracking().FirstOrDefaultAsync(s => s.Id == studentId);

        if (student == null)
            return new StudentRecordsResponseDto() { Success = false, Message = "Student not found!" };

        if (user is { Role: UserRole.Student or UserRole.Parent })
        {
            if (user.Student != null && user.Student.Id != student.Id)
                return new StudentRecordsResponseDto()
                {
                    Success = false,
                    Message = "You don't have permission to view these records"
                };

            if (user.Parent != null && user.Parent.Id != student.ParentId)
                return new StudentRecordsResponseDto()
                {
                    Success = false,
                    Message = "You don't have permission to view these records"
                };
        }

        var records = await context.StudentRecords.AsNoTracking()
            .Where(r => r.StudentId == studentId)
            .OrderByDescending(r => r.CreatedAt)
            .Include(r => r.Student)
            .Include(r => r.AcademicYear)
            .ProjectToType<StudentRecordDto>()
            .ToListAsync();

        return new StudentRecordsResponseDto() { Success = true, Records = records };
    }

    public async Task<StudentRecordsResponseDto> GetRecordsByCurrentYearAsync()
    {
        var records = await context.StudentRecords.AsNoTracking()
            .OrderByDescending(r => r.CreatedAt)
            .Include(r => r.Student)
            .Include(r => r.AcademicYear)
            .Where(r => r.AcademicYear != null && r.AcademicYear.IsCurrent)
            .ProjectToType<StudentRecordDto>()
            .ToListAsync();

        return new StudentRecordsResponseDto() { Success = true, Records = records };
    }

    public async Task<StudentRecordsResponseDto> AddRecordsAsync(UpsertStudentRecordDto record)
    {
        var validationResult = await validator.ValidateAsync(record);

        if (!validationResult.IsValid)
        {
            var error = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));

            Log.Error(error);

            return new StudentRecordsResponseDto() { Success = false, Message = error };
        }

        var newRecord = record.Adapt<StudentRecord>();

        await context.StudentRecords.AddAsync(newRecord);
        await context.SaveChangesAsync();

        return new StudentRecordsResponseDto()
        {
            Success = true,
            Message = "Record added successfully!",
            Record = newRecord.Adapt<StudentRecordDto>()
        };
    }

    public async Task<StudentRecordsResponseDto> DeleteRecordsAsync(int id)
    {
        var record = await context.StudentRecords.FindAsync(id);

        if (record == null)
            return new StudentRecordsResponseDto() { Success = false, Message = "Record not found!" };

        context.StudentRecords.Remove(record);
        await context.SaveChangesAsync();

        return new StudentRecordsResponseDto() { Success = true, Message = "Record deleted successfully!" };
    }

    public async Task<StudentRecordsResponseDto> AddStudentsRecordsAsync(List<int> studentIds, int examTypeId)
    {
        var examType = await context.ExamTypes.AsNoTracking().FirstOrDefaultAsync(t => t.Id == examTypeId);

        if (examType == null)
            return new StudentRecordsResponseDto() { Success = false, Message = "Exam type not found" };

        foreach (var studentId in studentIds)
        {
            var student = await context.Students.AsNoTracking().FirstOrDefaultAsync(s => s.Id == studentId);

            if (student == null)
                return new StudentRecordsResponseDto() { Success = false, Message = "Student not found" };

            var examsMaxSum = await context.Exams.AsNoTracking()
                .Include(e => e.AcademicYear)
                .Where(e => e.GradeId == student.GradeId)
                .Where(e => e.AcademicYear != null && e.AcademicYear.IsCurrent)
                .Where(e => e.ExamTypeId == examType.Id)
                .SumAsync(e => e.MaxMark);

            var examsMinSum = await context.Exams.AsNoTracking()
                .Include(e => e.AcademicYear)
                .Where(e => e.GradeId == student.GradeId)
                .Where(e => e.AcademicYear != null && e.AcademicYear.IsCurrent)
                .Where(e => e.ExamTypeId == examType.Id)
                .SumAsync(e => e.MinMark);

            var studentResultSum = await context.ExamResults.AsNoTracking()
                .Include(r => r.Exam)
                .ThenInclude(e => e!.AcademicYear)
                .Where(r => r.StudentId == studentId)
                .Where(r => r.Approved)
                .Where(r => r.Exam != null && r.Exam.AcademicYear != null && r.Exam.AcademicYear.IsCurrent)
                .Where(r => r.Exam != null && r.Exam.ExamTypeId == examType.Id)
                .SumAsync(r => r.Score);

            var record = new UpsertStudentRecordDto()
            {
                StudentId = studentId,
                GradeId = student.GradeId,
                AcademicYearId = student.AcademicYearId,
                Remarks = studentResultSum.ToString(CultureInfo.InvariantCulture),
                TotalRemark = examsMaxSum.ToString(CultureInfo.InvariantCulture),
                Percentage = (studentResultSum / examsMaxSum * 100).ToString(CultureInfo.InvariantCulture),
                Result = studentResultSum >= examsMinSum ? Result.Success : Result.Failed
            };

            var res = await AddRecordsAsync(record);

            if (!res.Success)
                return new StudentRecordsResponseDto() { Success = false, Message = res.Message };
        }

        return new StudentRecordsResponseDto() { Success = true, Message = "Students records added successfully!" };
    }
}
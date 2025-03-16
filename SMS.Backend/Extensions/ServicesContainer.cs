using System;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SMS.Backend.Data;
using SMS.Backend.Validations.AcademicYears;
using SMS.Backend.Validations.Announcements;
using SMS.Backend.Validations.Applications;
using SMS.Backend.Validations.Assignments;
using SMS.Backend.Validations.Attendances;
using SMS.Backend.Validations.Classes;
using SMS.Backend.Validations.ExamResults;
using SMS.Backend.Validations.Exams;
using SMS.Backend.Validations.ExamTypes;
using SMS.Backend.Validations.Fees;
using SMS.Backend.Validations.Grades;
using SMS.Backend.Validations.Institutions;
using SMS.Backend.Validations.InvoiceItems;
using SMS.Backend.Validations.Invoices;
using SMS.Backend.Validations.Lessons;
using SMS.Backend.Validations.Parents;
using SMS.Backend.Validations.Payments;
using SMS.Backend.Validations.StudentRecords;
using SMS.Backend.Validations.Students;
using SMS.Backend.Validations.Subjects;
using SMS.Backend.Validations.Teachers;
using SMS.Backend.Validations.Users;
using SMS.Models.DTOs.AcademicYears;
using SMS.Models.DTOs.Announcements;
using SMS.Models.DTOs.Applications;
using SMS.Models.DTOs.Assignments;
using SMS.Models.DTOs.Attendances;
using SMS.Models.DTOs.Classes;
using SMS.Models.DTOs.ExamResults;
using SMS.Models.DTOs.Exams;
using SMS.Models.DTOs.ExamTypes;
using SMS.Models.DTOs.Fees;
using SMS.Models.DTOs.Grades;
using SMS.Models.DTOs.Institutions;
using SMS.Models.DTOs.InvoiceItems;
using SMS.Models.DTOs.Invoices;
using SMS.Models.DTOs.Lessons;
using SMS.Models.DTOs.Parents;
using SMS.Models.DTOs.Payments;
using SMS.Models.DTOs.StudentRecords;
using SMS.Models.DTOs.Students;
using SMS.Models.DTOs.Subjects;
using SMS.Models.DTOs.Teachers;
using SMS.Models.DTOs.Users;

namespace SMS.Backend.Extensions;

public static class ServicesContainer
{
    public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Default"));
        });

        #region Validations

        services.AddScoped<IValidator<AddUserDto>, AddUserValidation>();
        services.AddScoped<IValidator<UpdateUserDto>, UpdateUserValidation>();
        services.AddScoped<IValidator<LoginRequestDto>, LoginRequestValidation>();
        services.AddScoped<IValidator<UpsertInstitutionDto>, UpsertInstitutionValidation>();
        services.AddScoped<IValidator<UpsertAcademicYearDto>, UpsertAcademicYearValidation>();
        services.AddScoped<IValidator<UpsertAnnouncementDto>, UpsertAnnouncementValidation>();
        services.AddScoped<IValidator<UpsertApplicationDto>, UpsertApplicationValidation>();
        services.AddScoped<IValidator<ApplicationApproveDto>, ApplicationApproveValidation>();
        services.AddScoped<IValidator<UpsertAssignmentDto>, UpsertAssignmentValidation>();
        services.AddScoped<IValidator<UpsertAttendanceDto>, UpsertAttendanceValidation>();
        services.AddScoped<IValidator<AddClassAttendancesDto>, AddClassAttendancesValidation>();
        services.AddScoped<IValidator<UpsertClassDto>, UpsertClassValidation>();
        services.AddScoped<IValidator<UpsertExamResultDto>, UpsertExamResultsValidation>();
        services.AddScoped<IValidator<UpsertExamDto>, UpsertExamValidation>();
        services.AddScoped<IValidator<UpsertExamTypeDto>, UpsertExamTypeValidation>();
        services.AddScoped<IValidator<UpsertFeeDto>, UpsertFeeValidation>();
        services.AddScoped<IValidator<UpsertGradeDto>, UpsertGradeValidation>();
        services.AddScoped<IValidator<UpsertInvoiceItemDto>, UpsertInvoiceItemValidation>();
        services.AddScoped<IValidator<UpsertInvoiceDto>, UpsertInvoiceValidation>();
        services.AddScoped<IValidator<UpsertLessonDto>, UpsertLessonValidation>();
        services.AddScoped<IValidator<UpsertParentDto>, UpsertParentValidation>();
        services.AddScoped<IValidator<UpsertPaymentDto>, UpsertPaymentValidation>();
        services.AddScoped<IValidator<UpsertStudentRecordDto>, UpsertStudentRecordValidation>();
        services.AddScoped<IValidator<UpsertStudentDto>, UpsertStudentValidation>();
        services.AddScoped<IValidator<UpsertSubjectDto>, UpsertSubjectValidation>();
        services.AddScoped<IValidator<UpsertTeacherDto>, UpsertTeacherValidation>();

        #endregion

        return services;
    }
}

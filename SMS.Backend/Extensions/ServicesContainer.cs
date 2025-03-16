using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SMS.Backend.Data;
using SMS.Backend.Services.AcademicYears;
using SMS.Backend.Services.Announcements;
using SMS.Backend.Services.Applications;
using SMS.Backend.Services.Assignments;
using SMS.Backend.Services.Attendances;
using SMS.Backend.Services.Classes;
using SMS.Backend.Services.Dashboard;
using SMS.Backend.Services.ExamResults;
using SMS.Backend.Services.Exams;
using SMS.Backend.Services.ExamTypes;
using SMS.Backend.Services.Fees;
using SMS.Backend.Services.Grades;
using SMS.Backend.Services.Institutions;
using SMS.Backend.Services.Invoices;
using SMS.Backend.Services.Lessons;
using SMS.Backend.Services.Parents;
using SMS.Backend.Services.StudentRecords;
using SMS.Backend.Services.Students;
using SMS.Backend.Services.Subjects;
using SMS.Backend.Services.Teachers;
using SMS.Backend.Services.Users;
using SMS.Backend.Utils;
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

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(cfg =>
        {
            cfg.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidAudience = configuration["Jwt:Audience"],
                ValidIssuer = configuration["Jwt:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
            };
        });

        services.AddAuthorization();

        services.AddExceptionHandler<AppExceptionHandler>();
        services.AddSingleton<AuthUtils>();

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

        #region Services

        services.AddScoped<IUsersService, UsersService>();
        services.AddScoped<IInstitutionService, InstitutionService>();
        services.AddScoped<IAcademicYearsService, AcademicYearsService>();
        services.AddScoped<IAnnouncementsService, AnnouncementsService>();
        services.AddScoped<IApplicationsService, ApplicationsService>();
        services.AddScoped<IAssignmentsService, AssignmentsService>();
        services.AddScoped<IAttendancesService, AttendancesService>();
        services.AddScoped<IClassesService, ClassesService>();
        services.AddScoped<IExamResultsService, ExamResultsService>();
        services.AddScoped<IExamsService, ExamsService>();
        services.AddScoped<IExamTypesService, ExamTypesService>();
        services.AddScoped<IFeesService, FeesService>();
        services.AddScoped<IGradesService, GradesService>();
        services.AddScoped<IInvoicesService, InvoicesService>();
        services.AddScoped<ILessonsService, LessonsService>();
        services.AddScoped<IParentsService, ParentsService>();
        services.AddScoped<IStudentRecordsService, StudentRecordsService>();
        services.AddScoped<IStudentsService, StudentsService>();
        services.AddScoped<ISubjectsService, SubjectsService>();
        services.AddScoped<ITeachersService, TeachersService>();
        services.AddScoped<IDashboardService, DashboardService>();

        #endregion

        return services;
    }
}

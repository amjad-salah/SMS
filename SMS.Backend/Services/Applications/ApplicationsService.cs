using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SMS.Backend.Data;
using SMS.Backend.Services.Invoices;
using SMS.Backend.Services.Parents;
using SMS.Backend.Services.Students;
using SMS.Models.DTOs.Applications;
using SMS.Models.DTOs.InvoiceItems;
using SMS.Models.DTOs.Invoices;
using SMS.Models.DTOs.Parents;
using SMS.Models.DTOs.Students;
using SMS.Models.Entities;

namespace SMS.Backend.Services.Applications;

public class ApplicationsService(
    AppDbContext context,
    IParentsService parentsService,
    IStudentsService studentsService,
    IInvoicesService invoicesService,
    IValidator<UpsertApplicationDto> validator,
    IValidator<ApplicationApproveDto> approveValidator) : IApplicationsService
{
    public async Task<ApplicationsResponseDto> GetApplicationsAsync()
    {
        var applications = await context.Applications.AsNoTracking()
            .Include(a => a.Institution)
            .OrderByDescending(a => a.CreatedAt)
            .ProjectToType<ApplicationDto>().ToListAsync();


        return new ApplicationsResponseDto() { Success = true, Applications = applications };
    }

    public async Task<ApplicationsResponseDto> GetApplicationByIdAsync(int id)
    {
        var application = await context.Applications.AsNoTracking()
            .Include(a => a.Grade)
            .Include(a => a.AcademicYear)
            .Include(a => a.Institution)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (application == null)
            return new ApplicationsResponseDto() { Success = false, Message = "Application not found" };

        return new ApplicationsResponseDto()
        {
            Success = true,
            Application = application.Adapt<ApplicationDetailsDto>()
        };
    }

    public async Task<ApplicationsResponseDto> GetApplicationsByCurrentYearAsync()
    {
        var applications = await context.Applications.AsNoTracking()
            .Include(a => a.AcademicYear)
            .Include(a => a.Institution)
            .Where(a => a.AcademicYear != null && a.AcademicYear.IsCurrent)
            .OrderByDescending(a => a.CreatedAt)
            .ProjectToType<ApplicationDto>()
            .ToListAsync();

        return new ApplicationsResponseDto() { Success = true, Applications = applications };
    }

    public async Task<ApplicationsResponseDto> AddApplicationAsync(UpsertApplicationDto application)
    {
        var validationResult = await validator.ValidateAsync(application);

        if (!validationResult.IsValid)
        {
            var error = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));

            Log.Error(error);

            return new ApplicationsResponseDto() { Success = false, Message = error };
        }

        var newApplication = application.Adapt<Application>();

        var lastId = context.Applications.Any() ? context.Applications.Max(a => a.Id) : 0;

        newApplication.ApplicationNo = $"AP0{lastId + 1}";

        await context.Applications.AddAsync(newApplication);
        await context.SaveChangesAsync();

        return new ApplicationsResponseDto()
        {
            Success = true,
            Message = "Application added successfully!",
            Application = newApplication.Adapt<ApplicationDetailsDto>()
        };
    }

    public async Task<ApplicationsResponseDto> UpdateApplicationAsync(int id, UpsertApplicationDto application)
    {
        var existingApplication = await context.Applications.FindAsync(id);

        if (existingApplication == null)
        {
            Log.Error("Application with {Id} not found", id);

            return new ApplicationsResponseDto() { Success = false, Message = "Application not found" };
        }

        var validationResult = await validator.ValidateAsync(application);

        if (!validationResult.IsValid)
        {
            var error = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));

            Log.Error(error);

            return new ApplicationsResponseDto() { Success = false, Message = error };
        }

        existingApplication.Gender = application.Gender;
        existingApplication.StudentName = application.StudentName;
        existingApplication.GuardianName = application.GuardianName;
        existingApplication.GuardianEmail = application.GuardianEmail;
        existingApplication.GradeId = application.GradeId;
        existingApplication.Status = application.Status;
        existingApplication.AcademicYearId = application.AcademicYearId;
        existingApplication.GuardianAddress = application.GuardianAddress;
        existingApplication.BirthDate = application.BirthDate;
        existingApplication.GuardianPhone = application.GuardianPhone;

        await context.SaveChangesAsync();

        return new ApplicationsResponseDto() { Success = true, Message = "Application updated successfully!" };
    }

    public async Task<ApplicationsResponseDto> DeleteApplicationAsync(int id)
    {
        var existingApplication = await context.Applications.FindAsync(id);

        if (existingApplication == null)
        {
            Log.Error("Application with {Id} not found", id);

            return new ApplicationsResponseDto() { Success = false, Message = "Application not found" };
        }

        context.Applications.Remove(existingApplication);
        await context.SaveChangesAsync();

        return new ApplicationsResponseDto() { Success = true, Message = "Application deleted successfully!" };
    }

    public async Task<ApplicationApproverResponseDto> ApproveApplicationsAsync(int id, ApplicationApproveDto approveDto)
    {
        var existingApplication = await context.Applications.FindAsync(id);

        if (existingApplication == null)
        {
            Log.Error("Application with {Id} not found", id);

            return new ApplicationApproverResponseDto() { Success = false, Message = "Application not found" };
        }

        var validationResult = await approveValidator.ValidateAsync(approveDto);

        if (!validationResult.IsValid)
        {
            var error = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));

            Log.Error(error);

            return new ApplicationApproverResponseDto() { Success = false, Message = error };
        }

        var existClass = context.Classes.AsNoTracking().FirstOrDefault(c => c.Id == approveDto.ClassId);
        if (existClass == null)
            return new ApplicationApproverResponseDto() { Success = false, Message = "Class not found" };

        var newParent = new UpsertParentDto()
        {
            FullName = existingApplication.GuardianName,
            Email = existingApplication!.GuardianEmail!,
            Phone = existingApplication.GuardianPhone,
            Address = existingApplication.GuardianAddress
        };

        var newParentResponse = await parentsService.AddParentAsync(newParent);

        var newStudent = new UpsertStudentDto()
        {
            FullName = existingApplication.StudentName,
            GradeId = existingApplication.GradeId,
            AcademicYearId = existingApplication.AcademicYearId,
            Gender = existingApplication.Gender,
            ClassId = existClass.Id,
            BirthDate = existingApplication.BirthDate,
            ParentId = newParentResponse.Parent!.Id,
            AdmissionDate = DateTime.UtcNow.Date,
            InstitutionId = existingApplication.InstitutionId
        };

        var newStudentResponse = await studentsService.AddStudentAsync(newStudent);

        var newInvoice = new UpsertInvoiceDto()
        {
            Date = DateTime.UtcNow.Date,
            Tax = approveDto.Tax,
            Discount = approveDto.Discount,
            StudentId = newStudentResponse.Student!.Id
        };

        var newInvoiceResponse = await invoicesService.AddInvoiceAsync(newInvoice);

        foreach (var fee in approveDto.Fees!)
            await invoicesService.AddInvoiceItemAsync(new UpsertInvoiceItemDto()
            {
                Description = fee.Name,
                Quantity = 1,
                UnitPrice = fee.Amount,
                InvoiceId = newInvoiceResponse.Invoice!.Id
            });

        existingApplication.Status = ApplicationStatus.Approved;
        await context.SaveChangesAsync();

        return new ApplicationApproverResponseDto()
        {
            Success = true,
            Message = "Application approved successfully!",
            InvoiceNo = newInvoiceResponse.Invoice!.InvoiceNumber
        };
    }
}
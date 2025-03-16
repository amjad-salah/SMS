namespace SMS.Models.DTOs.Invoices;

public class InvoicesResponseDto : BaseResponseDto
{
    public List<InvoiceDto>? Invoices { get; set; }
    public InvoiceDetailsDto? Invoice { get; set; }
}
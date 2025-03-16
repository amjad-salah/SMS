namespace SMS.Models.DTOs.InvoiceItems;

public class InvoiceItemsResponseDto : BaseResponseDto
{
    public List<InvoiceItemDto>? Items { get; set; }
    public InvoiceItemDto? Item { get; set; }
}
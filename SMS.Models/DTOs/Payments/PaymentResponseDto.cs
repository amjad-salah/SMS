namespace SMS.Models.DTOs.Payments;

public class PaymentResponseDto : BaseResponseDto
{
    public List<PaymentDto>? Payments { get; set; }
    public PaymentDto? Payment { get; set; }
}
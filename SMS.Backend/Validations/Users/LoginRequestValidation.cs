using FluentValidation;
using SMS.Models.DTOs.Users;

namespace SMS.Backend.Validations.Users;

public class LoginRequestValidation : AbstractValidator<LoginRequestDto>
{
    public LoginRequestValidation()
    {
        RuleFor(r => r.Email).NotEmpty().WithMessage("Email is required");
        RuleFor(r => r.Password).NotEmpty().WithMessage("Password is required");
    }
}
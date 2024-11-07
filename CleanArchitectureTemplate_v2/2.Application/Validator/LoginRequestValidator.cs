using _2.Application.DTOs.Auth;
using FluentValidation;

namespace _2.Application.Validator
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(user => user.Username).NotEmpty().WithMessage("Username is required.");
            RuleFor(user => user.Password).NotEmpty().WithMessage("Password is required");
        }
    }
}

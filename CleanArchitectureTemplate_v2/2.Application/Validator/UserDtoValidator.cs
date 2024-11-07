using _2.Application.DTOs.User;
using FluentValidation;

namespace _2.Application.Validator
{
    public class UserDtoValidator : AbstractValidator<UserDto>
    {
        public UserDtoValidator()
        {
            RuleFor(user => user.Username).NotEmpty().WithMessage("Username is required.");
            RuleFor(user => user.Email).EmailAddress().WithMessage("Invalid email format.");
        }
    }
}

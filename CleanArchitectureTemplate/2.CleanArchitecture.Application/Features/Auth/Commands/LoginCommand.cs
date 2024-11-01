using _1.CleanArchitecture.Domain.Common;
using _2.CleanArchitecture.Application.Common.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace _2.CleanArchitecture.Application.Features.Auth.Commands
{
    public record LoginCommand : IRequest<Result<string>>
    {
        public string Username { get; init; }
        public string Password { get; init; }
    }

    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(v => v.Username)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(v => v.Password)
                .NotEmpty()
                .MinimumLength(6);
        }
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<string>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ITokenService _tokenService;

        public LoginCommandHandler(IApplicationDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == request.Username, cancellationToken);

            if (user == null || !VerifyPasswordHash(request.Password, user.PasswordHash))
            {
                return Result<string>.Failure(new[] { "Invalid credentials" });
            }

            var token = _tokenService.GenerateJwtToken(user);
            return Result<string>.Success(token);
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }
    }
}

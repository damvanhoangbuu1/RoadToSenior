using _1.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace _3.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId
        {
            get
            {
                string Id = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
                if (Guid.TryParse(Id, out Guid guid))
                {
                    return guid;
                }

                return Guid.Empty;
            }
        }
    }
}

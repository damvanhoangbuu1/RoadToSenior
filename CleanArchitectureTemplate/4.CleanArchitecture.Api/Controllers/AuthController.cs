using _1.CleanArchitecture.Domain.Common;
using _2.CleanArchitecture.Application.DTOs.Auth;
using _2.CleanArchitecture.Application.Features.IServices;
using _4.CleanArchitecture.Api.Filters;
using Microsoft.AspNetCore.Mvc;

namespace _4.CleanArchitecture.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<Result<string>>> Login([FromBody] LoginRequest loginRequest)
        {
            var response = await _authService.Login(loginRequest.Username, loginRequest.Password);
            return Ok(response);
        }

        [Authorize("User", "Admin")]
        [HttpGet("Test")]
        public ActionResult Test()
        {
            return Ok("Hello");
        }

        [Authorize("Admin")]
        [HttpGet("Test2")]
        public ActionResult Test2()
        {
            return Ok("Goodbye");
        }
    }
}
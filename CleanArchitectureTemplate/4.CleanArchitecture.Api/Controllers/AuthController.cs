using _1.CleanArchitecture.Domain.Common;
using _2.CleanArchitecture.Application.DTOs.Auth;
using _2.CleanArchitecture.Application.Features.Services;
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

        [Authorize]
        [HttpPost("update-infor")]
        public async Task<ActionResult<Result<string>>> Update([FromBody] UpdateInforRequest request)
        {
            var response = await _authService.UpdateInfor(request);
            return Ok(response);
        }

        [Authorize("User", "Admin")]
        [HttpGet("Test")]
        public ActionResult Test()
        {
            return Ok("User + Admin");
        }

        [Authorize("Admin")]
        [HttpGet("Test2")]
        public ActionResult Test2()
        {
            return Ok("Admin");
        }

        [Authorize("User")]
        [HttpGet("Test3")]
        public ActionResult Test3()
        {
            return Ok("User");
        }

        [AllowAnonymous]
        [HttpGet("Test4")]
        public ActionResult Test4()
        {
            return Ok("AllowAnonymous");
        }
    }
}
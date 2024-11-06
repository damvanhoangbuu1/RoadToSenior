using _2.Application.Common;
using _2.Application.Interfaces;
using _2.Application.Services;
using _4.WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _4.WebAPI.Controllers
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

        [HttpPost("Login")]
        public async Task<ActionResult<Result<string>>> Login([FromBody] AuthModel auth)
        {
            var response = await _authService.Login(auth.Username, auth.Password);
            return Ok(response);
        }
    }
}

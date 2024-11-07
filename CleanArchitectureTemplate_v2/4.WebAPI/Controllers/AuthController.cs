using _2.Application.Common;
using _2.Application.DTOs.Auth;
using _2.Application.DTOs.User;
using _2.Application.Interfaces;
using _4.WebAPI.Attributes;
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
        public ActionResult<Result<string>> Login([FromBody] LoginRequest request)
        {
            var response = _authService.Login(request.Username, request.Password);
            return Ok(response);
        }

        [Authorize]
        [HttpGet("account-infor")]
        public ActionResult<Result<UserDto>> GetAccountInfo()
        {
            var response = _authService.GetAccountInfor();
            return Ok(response);
        }

        [Authorize]
        [HttpPost("account-infor")]
        public async Task<ActionResult<Result<UserDto>>> UpdateAccountInfor([FromBody] UserDto userDto)
        {
            var response = await _authService.UpdateAccountInfor(userDto);
            return Ok(response);
        }
    }
}

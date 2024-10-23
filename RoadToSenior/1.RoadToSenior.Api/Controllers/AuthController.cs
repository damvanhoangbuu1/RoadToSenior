using _1.RoadToSenior.Api.Helper;
using _1.RoadToSenior.Api.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace _1.RoadToSenior.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtTokenHelper _jwtTokenHelper;

        public AuthController(JwtTokenHelper jwtTokenHelper)
        {
            _jwtTokenHelper = jwtTokenHelper;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            var userMath = Seed.Users.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);
            if (userMath != null)
            {
                var token = _jwtTokenHelper.GenerateToken(userMath);
                return Ok(new { token });
            }

            return Unauthorized();
        }
    }
}

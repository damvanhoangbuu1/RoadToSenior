using _1.CleanArchitecture.Domain.Common;
using _2.CleanArchitecture.Application.Features.Auth.Commands;
using Microsoft.AspNetCore.Mvc;

namespace _4.CleanArchitecture.Api.Controllers
{
    public class AuthController : ApiControllerBase
    {
        [HttpPost("login")]
        public async Task<ActionResult<Result<string>>> Login(LoginCommand command)
        {
            var result = await Mediator.Send(command);

            if (result.Succeeded)
                return Ok(result);

            return BadRequest(result);
        }
    }
}

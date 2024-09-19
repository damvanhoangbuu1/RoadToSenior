using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RoadToSenior.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            return Ok($"Sản phẩm có ID: {id}");
        }
    }
}

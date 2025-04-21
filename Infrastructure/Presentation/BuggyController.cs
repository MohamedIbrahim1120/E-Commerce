using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuggyController : ControllerBase
    {
        [HttpGet("notfound")]
        public IActionResult GetNotFoundRequest()
        {
            return NotFound(); // 404
        }

        [HttpGet("servererror")]
        public IActionResult GetServerErrorRequest()
        {
            throw new Exception(); // 500

            return Ok();
        }

        [HttpGet("badrequest")]
        public IActionResult GetBadRequest()
        {
            return BadRequest(); // 400
        }

        [HttpGet("notfound/{id}")]
        public IActionResult GetBadRequest(int id) // Validation Error
        {
            // Code
            return NotFound(); // 404 
        }

        [HttpGet("unauthorize")]
        public IActionResult GetunauthorizeRequest()
        {
            // Code
            return Unauthorized(); // 404
        }

    }
}

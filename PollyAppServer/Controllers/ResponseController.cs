using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollyAppServer.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ResponseController:ControllerBase
    {
        [Route("{id:int}")]
        [HttpGet]
        public ActionResult GetResponse(int id)
        {
            Random rnd = new Random();
            var rndInteger = rnd.Next(1, 101);

            if (rndInteger >= id) 
            {
                Console.WriteLine("--> Failure - Generate a HTTP 500");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            Console.WriteLine("--> Success - Generate a HTTP 200");
            return Ok();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using PollyClientApp.Policies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollyClientApp.Controllers
{
    //[ApiController]
    [Route("api/[Controller]")]
    public class RequestController:ControllerBase
    {

        private readonly ClientPolicy _clientPolicy;
        private readonly IHttpClientFactory _clientFactory;

        private readonly string serverURL = "http://localhost:5084/api/Response/50";

        public RequestController(ClientPolicy clientPolicy, IHttpClientFactory  clientFactory)
        {            
            this._clientPolicy = clientPolicy;
            this._clientFactory = clientFactory;
        }
        // GET api/request1
        /// <summary>
        ///  original request
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult> MakeRequest1()
        {
            var client = _clientFactory.CreateClient();

            //orignal method 
            var response = await client.GetAsync(serverURL);   


            if (response.IsSuccessStatusCode) 
            {
                Console.WriteLine("--> ResponseService return SUCCESS");
                return Ok();
            }

            Console.WriteLine("--> ResponseService return FAILURE");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// ImmdiateHttpRetry.retry 5 times if failure
        /// </summary>
        /// <returns></returns>
        //GET api/request2
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult> MakeRequest2()
        {
            var client = _clientFactory.CreateClient();

            // with polly ImmdiateHttpRetry policy
            var response = await _clientPolicy.ImmdiateHttpRetry
            .ExecuteAsync(() => client.GetAsync(serverURL));

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> ResponseService return SUCCESS");
                return Ok();
            }

            Console.WriteLine("--> ResponseService return FAILURE");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// LinearHttpRetry.retry 5 times, interval is 3s if failure
        /// </summary>
        /// <returns></returns>
        //GET api/request3
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult> MakeRequest3()
        {
            var client = _clientFactory.CreateClient();

            // with polly LinearHttpRetry policy
            var response = await _clientPolicy.LinearHttpRetry
            .ExecuteAsync(() => client.GetAsync(serverURL));

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> ResponseService return SUCCESS");
                return Ok();
            }

            Console.WriteLine("--> ResponseService return FAILURE");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }


        //GET api/request4
        /// <summary>
        /// ExponentialHttpRetry.retry 5 times, interval is pow(2,retryAttempt) if failure
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult> MakeRequest4()
        {
            var client = _clientFactory.CreateClient();

            // with polly ExponentialHttpRetry policy
            var response = await _clientPolicy.ExponentialHttpRetry
            .ExecuteAsync(() => client.GetAsync(serverURL));

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> ResponseService return SUCCESS");
                return Ok();
            }

            Console.WriteLine("--> ResponseService return FAILURE");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }


        //GET api/request5
        /// <summary>
        /// named httpclient was used with ImmdiateHttpRetry.retry 5 times 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult> MakeRequest5()
        {
            var client = _clientFactory.CreateClient("Test");

            var response = await client.GetAsync(serverURL);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> ResponseService return SUCCESS");
                return Ok();
            }

            Console.WriteLine("--> ResponseService return FAILURE");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}

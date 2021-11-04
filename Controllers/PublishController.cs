using ElsaPlayground.Messages;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElsaPlayground.Controllers
{
    [Route("publish")]
    [ApiController]
    public class PublishController : ControllerBase
    {
        private readonly IBus _bus;

        public PublishController(IBus bus)
        {
            _bus = bus;
        }

        [HttpGet("messages")]
        public async Task<IActionResult> PublishMessage(string message)
        {
            await _bus.Publish(new TestMessage { Message = message });
            return Ok();
        }
    }
}

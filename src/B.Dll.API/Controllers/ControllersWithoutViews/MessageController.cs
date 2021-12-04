using System.Net;
using System.Threading.Tasks;
using Application.Services;
using Application.Wrappers.Attributes;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController: ControllerBase
    {
        private readonly MassTransitRabbitMqService _messageService;

        public MessageController(MassTransitRabbitMqService messageService)
        {
            this._messageService = messageService;
        }

        [HttpPost]
        [Route("[action]")]
        [CustomResponse((int)HttpStatusCode.OK)]
        [CustomResponse((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Send([FromBody] Message<string> msg)
        {
            var result = await _messageService.SendAsync(msg, "myChannel");
            if(result) return Ok("Message Sent");
            return BadRequest("Message not sent!");
        }
    }
}
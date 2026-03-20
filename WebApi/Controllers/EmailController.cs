using Application.DTOs;
using Application.Features.Email.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmailController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Send")]
        public async Task<IActionResult> SendEmail([FromBody] SendEmailRequestDto dto)
        {
            var command = new SendEmailCommand
            {
                To = dto.To,
                Subject = dto.Subject,
                Body = dto.Body
            };
            var emailId = await _mediator.Send(command);
            return Ok(new
            {
                message = "Email Queued Successfully",
                EmailId = emailId
            });
        }
    }
}

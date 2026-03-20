using MediatR;

namespace Application.Features.Email.Commands
{
    public class SendEmailCommand : IRequest<Guid>
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}

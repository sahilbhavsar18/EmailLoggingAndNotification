using Application.Features.Email.Commands;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Email.Handlers
{
    public class SendEmailHandler : IRequestHandler<SendEmailCommand, Guid>
    {
        private readonly IEmailService _emailService;

        public SendEmailHandler(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public Task<Guid> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            return _emailService.QueueEmailAsync(request.To,request.Subject,request.Body);
        }
    }
}

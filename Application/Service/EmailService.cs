using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Hangfire;
using Infrastructure.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service
{
    public class EmailService : IEmailService
    {
        private readonly IEmailRepository _emailRepo;
        private readonly IRabbitMqPublisher _publisher;
        private readonly  IEmailTemplateService _emailTemplateService;

        public EmailService(IEmailRepository emailRepo, IRabbitMqPublisher publisher, IEmailTemplateService emailTemplateService)
        {
            _emailRepo = emailRepo;
            _emailTemplateService = emailTemplateService;
            _publisher = publisher;
        }

        public async Task<Guid> QueueEmailAsync(string to, string subject, string body)
        {
            var renderedBody = await _emailTemplateService.RenderAsync(
                    "Welcome",
                    new
                    {
                        Name="Sahil Bhavsar"
                    }
                );
            var email = new EmailLog()
            {
                Id = Guid.NewGuid(),
                ToEmail = to,
                Subject = subject,
                Body = renderedBody,
                Status = Common.Enums.EmailStatus.Pending,
                RetryCount = 0,
                CreatedAt = DateTime.UtcNow
            };
            await _emailRepo.AddAsync(email);
            var message = new EmailMessage
            {
                EmailId = email.Id,
                To = email.ToEmail,
                Subject = email.Subject,
                Body = email.Body
            };
            _publisher.Publish(message);
            return email.Id;
        }
    }
}

using Application.Interfaces;
using Domain.Entities;
using Hangfire;
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
        private readonly  IBackgroundJobService _backgroundJobService;
        private readonly  IEmailTemplateService _emailTemplateService;

        public EmailService(IEmailRepository emailRepo, IBackgroundJobService backgroundJobService, IEmailTemplateService emailTemplateService)
        {
            _emailRepo = emailRepo;
            _backgroundJobService = backgroundJobService;
            _emailTemplateService = emailTemplateService;
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
            _backgroundJobService.EnqueueEmailJob(email.Id);
            return email.Id;
        }
    }
}

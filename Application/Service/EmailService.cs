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

        public EmailService(IEmailRepository emailRepo, IBackgroundJobService backgroundJobService)
        {
            _emailRepo = emailRepo;
            _backgroundJobService = backgroundJobService;
        }

        public async Task<Guid> QueueEmailAsync(string to, string subject, string body)
        {
            var email = new EmailLog()
            {
                Id = Guid.NewGuid(),
                ToEmail = to,
                Subject = subject,
                Body = body,
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

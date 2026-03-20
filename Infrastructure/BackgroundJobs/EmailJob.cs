using Application.Common.Enums;
using Application.Interfaces;
using Infrastructure.Persistance;
using Microsoft.Extensions.Logging;
using Polly;

namespace Infrastructure.BackgroundJobs
{
    public class EmailJob
    {
        private readonly IEmailRepository _emailRepo;
        private readonly IEmailSender _sender;
        private readonly AppDbContext _context;
        private readonly ILogger<EmailJob> _logger;

        public EmailJob(IEmailRepository emailRepo, IEmailSender sender, AppDbContext context, ILogger<EmailJob> logger)
        {
            _emailRepo = emailRepo;
            _sender = sender;
            _context = context;
            _logger = logger;
        }

        public async Task ProdessMail(Guid emailId)
        {
            var email = await _emailRepo.GetByIdAsync(emailId);
            if(email == null)
            {
                return;
            }
            var retryPolicy = Policy.Handle<Exception>().WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
            try
            {
                await retryPolicy.ExecuteAsync(async () => { 
                    await _sender.SendAsync(email.ToEmail,email.Subject,email.Body);
                });
                email.Status = EmailStatus.Sent;
                email.SentAt = DateTime.UtcNow;
            }
            catch (Exception Ex)
            {
                _logger.LogError(Ex,"Email Failer For {Email}",email.ToEmail);
                email.RetryCount++;
                email.Status = email.RetryCount >= 3 ? EmailStatus.Failed : EmailStatus.Pending;
                email.ErrorMessage = GetFriendlyMessage(Ex);
                email.ErrorDetails = Ex.ToString();
            }
            await _context.SaveChangesAsync();
        }
        private string GetFriendlyMessage(Exception ex)
        {
            var message = ex.Message.ToLower();

            if (message.Contains("authentication"))
            {
                return "Email authentication failed. Please check credentials.";
            }
            if (message.Contains("secure connection"))
            {
                return "SMTP requires a secure connection (SSL).";
            }
            if (message.Contains("timeout"))
            {
                return "Email server timeout. Please try again later.";
            }
            if (message.Contains("invalid"))
            {
                return "Invalid email address.";
            }
            return "Failed to send email due to an unexpected error.";
        }
    }
}

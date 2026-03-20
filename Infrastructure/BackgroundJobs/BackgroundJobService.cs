using Application.Interfaces;
using Hangfire;

namespace Infrastructure.BackgroundJobs
{
    public class BackgroundJobService:IBackgroundJobService
    {
        private readonly IBackgroundJobClient _backgroundJobClient;

        public BackgroundJobService(IBackgroundJobClient backgroundJobClient)
        {
            _backgroundJobClient = backgroundJobClient;
        }

        public void EnqueueEmailJob(Guid emailId)
        {
            _backgroundJobClient.Enqueue<EmailJob>(
                job => job.ProdessMail(emailId)
                );
        }
    }
}

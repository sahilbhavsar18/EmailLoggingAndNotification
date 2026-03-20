using Application.DTOs;
using Application.Interfaces;
using Infrastructure.Persistance;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Messaging
{
    public class RabbitMqConsumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        public RabbitMqConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory()
            {
                HostName="localhost"
            }; 
            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            channel.QueueDeclareAsync("email-queue",true,false,false);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (sender, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = JsonSerializer.Deserialize<EmailMessage>(Encoding.UTF8.GetString(body));
                using var scope = _serviceProvider.CreateScope();

                var senderService = scope.ServiceProvider.GetRequiredService<IEmailSender>();
                var repo = scope.ServiceProvider.GetRequiredService<IEmailRepository>();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();


                var email = await repo.GetByIdAsync(message.EmailId);


                try
                {
                    await senderService.SendAsync(message.To,message.Subject,message.Body);

                    email.Status = Application.Common.Enums.EmailStatus.Sent;
                    email.SentAt = DateTime.UtcNow;
                }
                catch (Exception ex)
                {
                    email.Status = Application.Common.Enums.EmailStatus.Failed;
                    email.ErrorMessage = ex.Message;
                    email.ErrorDetails = ex.ToString();
                }
                await db.SaveChangesAsync();
            };
            await channel.BasicConsumeAsync(queue:"email-queue",autoAck:true,consumer:consumer);
        }
    }
}

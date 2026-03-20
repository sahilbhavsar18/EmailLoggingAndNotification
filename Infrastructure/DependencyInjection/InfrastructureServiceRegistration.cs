using Application.Interfaces;
using Application.Service;
using Infrastructure.BackgroundJobs;
using Infrastructure.Email;
using Infrastructure.Messaging;
using Infrastructure.Persistance;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructure(
                this IServiceCollection services,string connectionString
                )
        {
            services.AddDbContext<AppDbContext>(op => op.UseSqlServer(connectionString));
            services.AddScoped<IEmailRepository, EmailRepository>();
            services.AddScoped<IEmailSender, SmtpEmailSender>();
            services.AddScoped<EmailJob>();
            services.AddScoped<IEmailService,EmailService>();
            services.AddScoped<IBackgroundJobService, BackgroundJobService>();
            services.AddScoped<IEmailTemplateService, EmailTemplateService>();
            services.AddScoped<IRabbitMqPublisher, RabbitMqPublisher>();
            services.AddHostedService<RabbitMqConsumer>();
            return services;
        }
    }
}

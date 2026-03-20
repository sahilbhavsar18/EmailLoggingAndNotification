using Application.DTOs;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Messaging
{
    public class RabbitMqPublisher : IRabbitMqPublisher
    {
        private IConnection _connection;
        private IChannel _channel;
        public RabbitMqPublisher()
        {
            Initialize().GetAwaiter().GetResult();
        }

        private async Task Initialize()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };
            _connection =  await factory.CreateConnectionAsync();
            _channel =  await _connection.CreateChannelAsync();
            _channel.QueueDeclareAsync(
                    queue:"email-queue",
                    durable:true,
                    exclusive:false,
                    autoDelete:false
                );
        }

        public async Task Publish(EmailMessage message)
        {
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
            await _channel.BasicPublishAsync(
                    exchange:"",
                    routingKey:"email-queue",
                    body:body
                );
        }
    }
}

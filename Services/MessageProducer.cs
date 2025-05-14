using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using UserProfileAPI.Interfaces;

namespace UserProfileAPI.Services
{
    public class MessageProducer: IMessageProducer
    {
        public async void SendingMessage<T>(T message)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
                VirtualHost = "/",
            };
           
            using var conn = await factory.CreateConnectionAsync();

            using var channel = await conn.CreateChannelAsync();

            await channel.QueueDeclareAsync("user_profile", durable: true, exclusive: false, autoDelete: false);
             
            var jsonString = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(jsonString);

            await channel.BasicPublishAsync("","user_profile", body: body);
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using Report.Application.Services.Abstract;
using System.Text;
using Newtonsoft.Json;
using Report.Application.Dtos;

namespace Report.Application.Message.Consumer
{
    public class GetHotelByLocationConsumer : BackgroundService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public GetHotelByLocationConsumer(
            IHttpClientFactory httpClientFactory,
            IServiceScopeFactory serviceScopeFactory)
        {
            _httpClientFactory = httpClientFactory;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {

            using var scope = _serviceScopeFactory.CreateScope();
            var rabbitMqService = scope.ServiceProvider.GetRequiredService<IRabbitMqService>();
            var reportService = scope.ServiceProvider.GetRequiredService<IReportService>();

            const string queueName = "report";
            const string exchangeName = "create_report";
            const string routingKey = "report.location";

            var factory = new ConnectionFactory { HostName = "rabbitmq" };

            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            await channel.ExchangeDeclareAsync(exchange: exchangeName, type: ExchangeType.Direct, durable: true);
            await channel.QueueBindAsync(queue: queueName, exchange: exchangeName, routingKey: routingKey);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var location = JsonConvert.DeserializeObject<string>(message);

                    var reportCreate = new CreateReportDto
                    {
                        RequestedBody = location,
                        ReportType = Domain.Enums.ReportType.GetHotelsByLocation
                    };

                    await reportService.CreateReportAsync(reportCreate);                   

                }
                catch (Exception ex)
                {
                    //Log
                }
            };

            await channel.BasicConsumeAsync(queue: queueName, autoAck: true, consumer: consumer);

            await Task.Delay(Timeout.Infinite, cancellationToken);
        }


    }
}

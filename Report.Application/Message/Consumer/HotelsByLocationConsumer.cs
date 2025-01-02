using Microsoft.Extensions.Hosting;
using Report.Application.Services.Abstract;

namespace Report.Application.Message.Consumer
{
    public class HotelsByLocationConsumer : BackgroundService
    {
        private readonly IRabbitMqService rabbitMqService;
        private readonly IReportService reportService;

        public HotelsByLocationConsumer(IRabbitMqService rabbitMqService, IReportService reportService)
        {
            this.rabbitMqService = rabbitMqService;
            this.reportService = reportService;
        }



        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var data = await rabbitMqService.ConsumeAsync(queueName: "location", exchangeName: "hotels_by_location", routingKey: "report.create");
            reportService.CreateReportAsync()

        }
    }
}

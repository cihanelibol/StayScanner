namespace Report.Application.Services.Abstract
{
    public interface IRabbitMqService
    {
        public Task SendAsync(string queueName, string exchageName, string routingName, object data);
        public Task<object> ConsumeAsync(string queueName, string exchangeName, string routingKey, CancellationToken cancellationToken);
    }
}

using CosmosBase;

using Hotel.Application.DependencyInjection;
using RabbitMQ.Client;
using Report.Application.DependencyInjection;
using Serilog;
using Serilog.Formatting.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCosmosBase(builder.Configuration);
builder.Services.AddHotelConfiguration(builder.Configuration);
builder.Services.AddReportConfiguration(builder.Configuration);


#region Serilog RabbitMq Settings
var factory = new ConnectionFactory { HostName = "rabbitmq" };

using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(queue: "log.elastic", durable: true, exclusive: false, autoDelete: false, arguments: null);
await channel.ExchangeDeclareAsync(exchange: "log", type: ExchangeType.Direct, durable: true);
await channel.QueueBindAsync(queue: "log.elastic", exchange: "log", routingKey: "log.elastic");

Log.Logger = new LoggerConfiguration()
   .MinimumLevel.Debug()
   .WriteTo.Console()
   .WriteTo.RabbitMQ(
        username: "logstash",
        password: "logstash",
        hostnames: new[] { "rabbitmq" },
        exchange:"log",
        routingKey:"log.elastic",
        exchangeType:"direct",
        port: 5672,
        formatter: new JsonFormatter()
    ).CreateLogger();

        Log.Warning("Selam Dünyalý!!!");


#endregion


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
#region Global logger
app.Use(async (context, next) =>
{
    Log.Information("Request Headers: {Headers}", context.Request.Headers);

    context.Request.EnableBuffering(); 
    var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
    context.Request.Body.Position = 0; 
    Log.Information("Request Body: {Body}", requestBody);

    var originalResponseBodyStream = context.Response.Body;
    using var responseBodyStream = new MemoryStream();
    context.Response.Body = responseBodyStream;

    await next.Invoke();

    Log.Information("Response Headers: {Headers}", context.Response.Headers);

    responseBodyStream.Seek(0, SeekOrigin.Begin);
    var responseBody = await new StreamReader(responseBodyStream).ReadToEndAsync();
    responseBodyStream.Seek(0, SeekOrigin.Begin);
    await responseBodyStream.CopyToAsync(originalResponseBodyStream);
    Log.Information("Response Body: {Body}", responseBody);

    Log.Information("Response Status Code: {StatusCode}", context.Response.StatusCode);
});
#endregion

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

app.Run();

using System.Text;
using concert.API.IntegrationEvents.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace concert.API.IntegrationEvents;

public class RabbitMQTest: IEventBus, IHostedService
{
    private readonly ILogger<RabbitMQTest> _logger;

    public RabbitMQTest(ILogger<RabbitMQTest> logger)
    {
        _logger = logger;
    }

    public Task PublishAsync(IntegrationEvent @event)
    {
        var factory = new ConnectionFactory() { HostName = "rabbitmq"};
        
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "hello",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        // string message = $"hello world, id: {@event.Id}";
        var message = @event.ToString();
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: string.Empty,
            routingKey: "hello",
            basicProperties: null,
            body: body);
        
        _logger.LogInformation($" [X] Sent {message} with id: {@event.Id}");
        
        return Task.CompletedTask;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var factory = new ConnectionFactory() { HostName = "rabbitmq" };
        // var factory = new ConnectionFactory()
        // {
        //     HostName = "172.23.0.2",
        //     Port = 5672,
        //     UserName = "guest",
        //     Password = "guest"
        // };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "hello",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
        
        _logger.LogInformation("Waiting for messages");

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            _logger.LogInformation($" [X] received {message}");
        };
        channel.BasicConsume(queue: "hello",
            autoAck: true,
            consumer: consumer);
        
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
using System.Text.Json;
using concert.API.IntegrationEvents.Events;
using RabbitMQ.Client;

namespace concert.API.IntegrationEvents;

public class EventBus : IEventBus
{
    private const string ExchangeName = "cm_event_bus";

    private readonly ILogger<EventBus> _logger;
    private IConnection _connection;
    
    public EventBus(ILogger<EventBus> logger)
    {
        _logger = logger;
    }

    public async Task PublishAsync(IntegrationEvent @event)
    {
        var routingKey = @event.GetType().Name;
        
        var factory = new ConnectionFactory() { HostName = "rabbitmq", UserName = "user", Password = "AXivLKzSbR2yDupO"};
        // var factory = new ConnectionFactory() { HostName = "rabbitmq", UserName = "guest", Password = "guest", Port = 5672}; // docker
        _connection = factory.CreateConnection();

        using var channel = _connection?.CreateModel() ??
                            throw new InvalidOperationException("RabbitMQ connection is not open") ;
        
        channel.ExchangeDeclare(exchange: ExchangeName, type: ExchangeType.Direct);

        var body = SerialiseMessage(@event);

        try
        {
            _logger.LogInformation($"{@event.GetType().Name} with id {@event.Id} is ready");
            _logger.LogInformation($"Routing key: {routingKey}");
            
            _logger.LogInformation(@event.ToString());
            
            channel.BasicPublish(exchange: ExchangeName, 
                routingKey: routingKey, 
                mandatory: true, 
                basicProperties: null,
                body: body);
            _logger.LogInformation($"{@event.GetType().Name} is published");
            
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private byte[] SerialiseMessage(IntegrationEvent @event)
    {
        return JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType(), new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }); 
    }
}
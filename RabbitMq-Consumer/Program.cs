using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Threading.Channels;
using System.Text;

var factory = new ConnectionFactory { HostName= "localhost" };
var connection = factory.CreateConnection();
var channel = connection.CreateModel();
channel.QueueDeclare(queue: "test", false, false, false, null);
Console.WriteLine("Ожидаем сообщения...!");

while (true)
{
    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine($"Сообщение получено: {message}");
    };

    channel.BasicConsume(queue: "test",
                        autoAck: true,
                        consumer: consumer);
    
    Console.WriteLine("Чтобы закрыть приложение введите {quit}");
    var message = Console.ReadLine();
    if (message == "quit")
    {
        break;
    }
}


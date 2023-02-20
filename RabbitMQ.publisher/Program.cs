// See https://aka.ms/new-console-template for more information

using RabbitMQ.Client;
using System.Text;
using System.Threading.Channels;

var factory = new ConnectionFactory()
{
    Uri = new Uri("amqps://avlbkmsz:VczzGlHupMoF7ximnepb8fKOx4cieMn0@woodpecker.rmq.cloudamqp.com/avlbkmsz")
};
//factory.Uri = new Uri("amqps://avlbkmsz:VczzGlHupMoF7ximnepb8fKOx4cieMn0@woodpecker.rmq.cloudamqp.com/avlbkmsz");

using var connection = factory.CreateConnection();
var channel = connection.CreateModel();

//exclure eğer true yaparsak saece bu kanal üzerinden ulaşabilirz. Ama biz başka processlerden ulaşmmak için false deriz
///channel.QueueDeclare("hello-queue", true, false,false);//exchange kullanmadan direkt kuyruk declare etmiştik 

channel.ExchangeDeclare("logs-direct",durable:true,type:ExchangeType.Direct);
var x = Enum.GetNames(typeof(LogNames)).ToList();
x.ForEach(x =>
{
    var routeKey = $"route-{x}";
    var queueName = $"direct-queue-{x}";
    channel.QueueDeclare(queueName, true, false, false);
    channel.QueueBind(queueName, "logs-direct", routeKey, null);
});
Enumerable.Range(1, 50).ToList().ForEach(x =>
{
    LogNames log =(LogNames) new Random().Next(1,5);
    string message = $"log-type:{log}";

  //  string message = $"Message{x}";
    var messageBody = Encoding.UTF8.GetBytes(message);

    var routeKey = $"route-{log}";
    channel.BasicPublish("logs-direct",routeKey,null, messageBody);

    Console.WriteLine($"Log gönderildi: {message}");
});


Console.ReadLine();

public enum LogNames
{
    Critical = 1,
    Error = 2,
    Warning = 3,
    Info = 4
}
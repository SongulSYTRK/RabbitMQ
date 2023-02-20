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

channel.ExchangeDeclare("logs-topic",durable:true,type:ExchangeType.Topic);
var x = Enum.GetNames(typeof(LogNames)).ToList();
x.ForEach(x =>
{
    Random rnd=new Random();

    /* var routeKey = $"route-{x}";
    var queueName = $"direct-queue-{x}";
    channel.QueueDeclare(queueName, true, false, false);
    channel.QueueBind(queueName, "logs-direct", routeKey, null);*/
});

Random rnd=new Random ();
Enumerable.Range(1, 50).ToList().ForEach(x =>
{
  //  LogNames log =(LogNames) new Random().Next(1,5);
  

  //  string message = $"Message{x}";  

    LogNames log1 = (LogNames)rnd.Next(1, 5);
    LogNames log2 = (LogNames)rnd.Next(1, 5);
    LogNames log3 = (LogNames)rnd.Next(1, 5);

    var routeKey = $"{log1}.{log2}.{log3}";
    string message = $"log-type:{log1}-{log2}-{log3}";
    var messageBody = Encoding.UTF8.GetBytes(message);
    //  var routeKey = $"route-{log}";
    channel.BasicPublish("logs-topic",routeKey,null, messageBody);

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
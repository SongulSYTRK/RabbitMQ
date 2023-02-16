// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory()
{
    Uri = new Uri("amqps://avlbkmsz:VczzGlHupMoF7ximnepb8fKOx4cieMn0@woodpecker.rmq.cloudamqp.com/avlbkmsz")
};
//factory.Uri = new Uri("amqps://avlbkmsz:VczzGlHupMoF7ximnepb8fKOx4cieMn0@woodpecker.rmq.cloudamqp.com/avlbkmsz");

using var connection = factory.CreateConnection();
var channel = connection.CreateModel();
//exclure eğer true yaparsak saece bu kanal üzerinden ulaşabilirz. Ama biz başka processlerden ulaşmmak için false deriz


channel.QueueDeclare("hello-queue", true, false,false);//exchange kullanmadan direkt kuyruk declare etmiştik 

Enumerable.Range(1, 50).ToList().ForEach(x =>
{
    string message = $"Message{x}";
    var messageBody = Encoding.UTF8.GetBytes(message);
    channel.BasicPublish(string.Empty, "hello-queue", null, messageBody);
    Console.WriteLine($"Mesaj gönderildi: {message}");
});
Console.ReadLine();

// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory()
{
    Uri = new Uri("amqps://avlbkmsz:VczzGlHupMoF7ximnepb8fKOx4cieMn0@woodpecker.rmq.cloudamqp.com/avlbkmsz")
};
//factory.Uri = new Uri("amqps://avlbkmsz:VczzGlHupMoF7ximnepb8fKOx4cieMn0@woodpecker.rmq.cloudamqp.com/avlbkmsz");

using var connection = factory.CreateConnection();
var channel = connection.CreateModel();
//exclure eğer true yaparsak saece bu kanal üzerinden ulaşabilirz. Ama biz başka processlerden ulaşmmak için false deriz
//kıuyruk var olduğu için hata olmaz.Burda yazamasak da olur zaten kuyruk publisher da oluştu
//channel.QueueDeclare("hello-queue", true, false);

channel.BasicQos(0,1,false);
var consumer = new EventingBasicConsumer(channel);
channel.BasicConsume("hello-queue", false, consumer);
consumer.Received += (object sender, BasicDeliverEventArgs e) =>
{
    var message = Encoding.UTF8.GetString(e.Body.ToArray());

    Thread.Sleep(1500);
    Console.WriteLine("GelenMessage :" + message);
    channel.BasicAck(e.DeliveryTag, false);
};



Console.ReadLine();

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


//channel.ExchangeDeclare("logs-fanout", durable: true, type: ExchangeType.Fanout);

///var randomQueueName = "log-database-save";// channel.QueueDeclare().QueueName;

//channel.QueueDeclare(randomQueueName, true, false, false);
///channel.QueueBind(randomQueueName, "logs-fanout","",null);



channel.BasicQos(0,1,false);
var consumer = new EventingBasicConsumer(channel);

var queueName = "direct-queue-Critical";
channel.BasicConsume(queueName, false, consumer);
Console.WriteLine("loglar dinleniyor ");
consumer.Received += (object sender, BasicDeliverEventArgs e) =>
{
    var message = Encoding.UTF8.GetString(e.Body.ToArray());

    Thread.Sleep(150);
    Console.WriteLine("GelenMessage :" + message);
    File.AppendAllText("log-critical.txt",message + "\n");

    channel.BasicAck(e.DeliveryTag, false);
};



Console.ReadLine();

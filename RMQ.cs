using System;
using System.Text;
using RabbitMQ.Client;
using System.Threading.Tasks;
using System.Diagnostics;
using RabbitMQ.Client.Events;
using System.Collections.Generic;
using System.Linq;

namespace Broadcaster
{
    class RMQ
    {
        public ConnectionFactory connectionFactory;
        public IConnection connection;
        public IModel channel;

        public void InitRMQConnection(string host = "cloudrmqserver.pptik.id", int port = 5672, string user = "tmdgdai",
        string pass = "tmdgdai", string vhost = "/tmdgdai")
        {
            connectionFactory = new ConnectionFactory();
            connectionFactory.HostName = host;
            connectionFactory.Port = port;
            connectionFactory.UserName = user;
            connectionFactory.Password = pass;
            connectionFactory.VirtualHost = vhost;
        }

        public void CreateRMQConnection()
        {
            connection = connectionFactory.CreateConnection();
            //Console.WriteLine("Koneksi " + (connection.IsOpen ? "Berhasil!" : "Gagal!"));
        }

        public void CreateRMQChannel(string exchange_name = "logs", string type = "fanout")
        {
            if (connection.IsOpen)
            {
                channel = connection.CreateModel();
                //Console.WriteLine("Channel " + (channel.IsOpen ? "Berhasil!" : "Gagal!"));
            }

            if (channel.IsOpen)
            {
                channel.ExchangeDeclare(exchange: exchange_name, type: "fanout");
                //channel.QueueDeclare(queue: queue_name,
                //                     durable: true,
                //                     exclusive: false,
                //                     autoDelete: false,
                //                     arguments: null);
                //Console.WriteLine("Queue telah dideklarasikan..");
            }
        }               
        public void SendMessage(string exchange_name = "logs", string routingKey = "", string msg = "send")
        {
            byte[] responseBytes = Encoding.UTF8.GetBytes(msg);// konversi pesan dalam bentuk string menjadi byte

            channel.BasicPublish (exchange:exchange_name,
                                    routingKey: "", 
                                    basicProperties: null,
                                    body: responseBytes);
            //Console.WriteLine("Pesan: '" + msg + "' telah dikirim.");
        }

        public void Disconnect()
        {
            channel.Close();
            channel = null;
            //Console.WriteLine("Channel ditutup!");
            if (connection.IsOpen)
            {
                connection.Close();
            }

            //Console.WriteLine("Koneksi diputus!");
            connection.Dispose();
            connection = null;
        }
    }
}

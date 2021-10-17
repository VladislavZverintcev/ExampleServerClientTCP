using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ExampleServerTCP
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Example Server TCP connection");
            // Get computer name
            String host = System.Net.Dns.GetHostName();
            // Get current IP
            System.Net.IPAddress ip = Dns.GetHostEntry(host).AddressList[2];
            ip.ToString();
            Console.WriteLine("Pleace entry server connection Port:");
            int port = 0;
            BeginWritePort:
            try
            {
                port = Convert.ToInt32(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("Writed port is not valid, pleace write correct port:");
                goto BeginWritePort;
            }

            var tcpEndPoint = new IPEndPoint(ip, port);
            var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            tcpSocket.Bind(tcpEndPoint);
            tcpSocket.Listen(5);
            while(true)
            {
                var listener = tcpSocket.Accept();
                var buffer = new byte[256];
                var size = 0;
                var data = new StringBuilder();
                do
                {
                    size = listener.Receive(buffer);
                    data.Append(Encoding.UTF8.GetString(buffer, 0, size));
                }
                while (listener.Available > 0);
                Console.WriteLine(data.ToString());
                //Send answer massage
                listener.Send(Encoding.UTF8.GetBytes("Server received an incoming message"));
                listener.Shutdown(SocketShutdown.Both);
                listener.Close();
            }
        }
    }
}

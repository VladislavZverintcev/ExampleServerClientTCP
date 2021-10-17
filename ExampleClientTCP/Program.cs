using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ExampleClientTCP
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("Welcome to Example Client TCP connection");
            Console.WriteLine("Pleace entry server ip-adress:");
            BeginWriteIP:
            string ip = Console.ReadLine();
            if(!IsAddressValid(ip))
            {
                Console.WriteLine("Writed ip-adress is not valid, pleace write correct ip:");
                goto BeginWriteIP;
            }
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

            while (true)
            {
                var tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Console.WriteLine("Enter your message");
                var message = Console.ReadLine();
                var data = Encoding.UTF8.GetBytes(message);
                try
                {
                    tcpSocket.Connect(tcpEndPoint);
                    tcpSocket.Send(data);
                    var buffer = new byte[256];
                    var size = 0;
                    var answer = new StringBuilder();
                    do
                    {
                        size = tcpSocket.Receive(buffer);
                        answer.Append(Encoding.UTF8.GetString(buffer, 0, size));
                    }
                    while (tcpSocket.Available > 0);
                    Console.WriteLine(answer.ToString());
                    tcpSocket.Shutdown(SocketShutdown.Both);
                    tcpSocket.Close();
                }
                catch(SocketException)
                {
                    Console.WriteLine("Server connection is not availabe");
                }
            }
            
        }
        public static bool IsAddressValid(string addrString)
        {
            IPAddress address;
            return IPAddress.TryParse(addrString, out address);
        }
    }
}

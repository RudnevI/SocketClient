using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace SocketClient
{
    class Program
    {
        static void Main(string[] args)
        {
            SendMessageFromSocket(5001);
        }

        static void SendMessageFromSocket(int port)
        {
            byte[] bytes = new byte[1024];

            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, port);

            Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            sender.Connect(ipEndPoint);
            Console.WriteLine("Type in your message: ");
            string msg = Console.ReadLine();

            byte[] senMsg = Encoding.UTF8.GetBytes(msg);
            int bytesSent = sender.Send(senMsg);

            int byteReceived = sender.Receive(bytes);
            Console.WriteLine($"Server's response: {Encoding.UTF8.GetString(bytes)}");

            if(msg.IndexOf("<eof>") == -1)
            {
                SendMessageFromSocket(port);
            }

            sender.Shutdown(SocketShutdown.Both);
            sender.Close();

        }
    }
}

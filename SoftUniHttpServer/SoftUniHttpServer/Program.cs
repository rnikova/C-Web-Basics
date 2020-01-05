using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SoftUniHttpServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var tcpListener = new TcpListener(IPAddress.Loopback, 80);

            tcpListener.Start();

            while (true)
            {
                var client = tcpListener.AcceptTcpClient();
                Task.Run(() => ProccessClient(client));
            }
        }

        public static async Task ProccessClient(TcpClient client)
        {
            const string NewLine = "\r\n";

            using var stream = client.GetStream();

            var requestBytes = new byte[100000];
            var readBytes = await stream.ReadAsync(requestBytes, 0, requestBytes.Length);
            var stringRequest = Encoding.UTF8.GetString(requestBytes, 0, readBytes);

            Console.WriteLine(new string('=', 70));
            Console.WriteLine(stringRequest);

            string responseBody = DateTime.Now.ToString();

            var response = "HTTP/1.0 200 OK" + NewLine
                + "Content-Type: text/html" + NewLine
                + "Server: MyCustomServer/1.0" + NewLine
                + $"Content-Length: {responseBody.Length}" + NewLine + NewLine
                + responseBody;

            var responseBytes = Encoding.UTF8.GetBytes(response);
            await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
        }
    }
}

using System.Net;
using System.Net.Sockets;
using System.Text;

namespace server;

public class Server
{
    public void StartServer()
    {
        // Establish local endpoint for socket
        var ipHost = Dns.GetHostEntry(Dns.GetHostName());
        var ipAddr = ipHost.AddressList[0];
        var localEndPoint = new IPEndPoint(ipAddr, 1337);

        // Create TCP/IP socket
        Socket listener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            // Associate network address to socket
            listener.Bind(localEndPoint);

            // Place socket in a listening mode
            listener.Listen();

            while (true)
            {
                Console.WriteLine("Waiting for connection...");

                // Wait fot client to connect
                var clientSocket = listener.Accept();

                // 1MB Data buffer
                var bytes = new byte[1024];
                string? data = null;

                while (true)
                {
                    int numByte = clientSocket.Receive(bytes);

                    data += Encoding.ASCII.GetString(bytes, 0, numByte);

                    if (data.IndexOf("<EOF>", StringComparison.Ordinal) > -1)
                        break;
                }

                Console.WriteLine("Text received -> {0} ", data);
                byte[] message = Encoding.ASCII.GetBytes("Test Server");

                // Send message to client
                clientSocket.Send(message);

                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}
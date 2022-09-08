using System.Net;
using System.Net.Sockets;
using System.Text;

namespace client;

public class Client
{
    public void StartClient()
    {
        try
        {
            // Establish local endpoint for socket
            var ipHost = Dns.GetHostEntry(Dns.GetHostName());
            var ipAddr = ipHost.AddressList[0];
            var localEndPoint = new IPEndPoint(ipAddr, 1337);

            // Create TCP/IP socket
            var sender = new Socket(ipAddr.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            try
            {
                // Connect Socket to the server
                sender.Connect(localEndPoint);

                // Print EndPoint information
                Console.WriteLine("Socket connected to -> {0} ",
                    sender.RemoteEndPoint);

                // Create the message that we will send to server
                byte[] message = Encoding.ASCII.GetBytes("Test Client<EOF>");
                int byteSent = sender.Send(message);

                // Data buffer
                byte[] messageReceived = new byte[1024];
                
                // Receive message from server in bytes
                // and convert it to string
                int byteRecv = sender.Receive(messageReceived);
                Console.WriteLine("Message from Server -> {0}",
                    Encoding.ASCII.GetString(messageReceived,
                        0, byteRecv));
                
                // Shutdown and close socket
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
            }

            // Manage of Socket's Exceptions
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane);
            }

            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se);
            }

            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e);
            }
        }

        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}
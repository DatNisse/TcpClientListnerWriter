using System;
using System.Net; //added
using System.Net.Sockets; //added


namespace ovning2NatProg
{
    class Program
    {

        static void Main(string[] args)
        {

            Console.WriteLine("Read or write? (w/r)");
            string choice = Console.ReadLine();

            if (choice == "r")
            {
                // starts to listen for incoming TCP connections
                ListenTCP();
            }
            else if (choice == "w")
            {
                // Tries to connect to the specified machine
                ConnectTcp("GamePC", "HelloWorld!");
            }
        }

        static void ListenTCP()
        {
            try
            {
                TcpListener tcpListener = new TcpListener(GetLocalIP(), 11000);
                tcpListener.Start();
                Byte[] bytes = new Byte[256];
                string msgR = null;

                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    TcpClient tcpClient = tcpListener.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    msgR = null;

                    NetworkStream stream = tcpClient.GetStream();

                    int i;

                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        msgR = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("Recived: {0}", msgR);

                        msgR = msgR.ToUpper();

                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(msgR);

                        stream.Write(msg, 0, msg.Length);
                        Console.WriteLine("Sent: {0}", msgR);


                    }
                    tcpClient.Close();

                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.Read();
            }
        }

        //Tries to connect to an Tcp tcpClient
        static void ConnectTcp(String server, String message)
        {
            do
            {
                try
                {
                    int port = 11000;

                    TcpClient client = new TcpClient(server, port);
                    // if no connection could be established an exception will be thrown
                    Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                    NetworkStream stream = client.GetStream();

                    stream.Write(data, 0, data.Length);

                    Console.WriteLine("Sent: {0}", message);

                    data = new byte[256];

                    String responseData = String.Empty;

                    int bytes = stream.Read(data, 0, data.Length);
                    responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                    Console.WriteLine("Recived: {0}", responseData);

                    stream.Close();
                    client.Close();
                }

                catch (ArgumentNullException e)
                {
                    Console.WriteLine("ArgumentNullException: {0}", e);
                }
                catch (SocketException e)
                {
                    Console.WriteLine("SocketException: {0}", e);
                }

                Console.WriteLine("\n Press Enter to continue...");
                Console.Read();
            } while (true);

        }

        static private IPAddress GetLocalIP()
        {
            //Retrives the local computers IP from the DNS
            IPAddress[] iPAddresses = Dns.GetHostAddresses(Dns.GetHostName());
            return iPAddresses[3];
        }
    }

}

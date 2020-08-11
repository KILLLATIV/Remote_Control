using System;
using System.Net.Sockets;
using System.Text;

namespace Remote_Control_Client
{
    class Client
    {
        private const int Port = 4444;
        private const string IP = "127.0.0.1";

        static void Main(string[] args)
        {
            try
            {
                TcpClient client = new TcpClient();
                client.Connect(IP, Port);
                byte[] data = new byte[256];
                StringBuilder response = new StringBuilder();
                NetworkStream stream = client.GetStream();

                while (true)
                {
                    response.Clear();
                    string command = Console.ReadLine();
                    if (command == "exit")
                        break;

                    stream.Write(Encoding.UTF8.GetBytes(command));

                    do
                    {
                        int bytes = stream.Read(data, 0, data.Length);
                        response.Append(Encoding.UTF8.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    Console.WriteLine(response.ToString());
                }

                stream.Close();
                client.Close();
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }
        }
    }
}

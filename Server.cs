using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Remote_Control 
{
    class Server
    {
        private const int Port = 4444;
        private const string IP = "127.0.0.1";

        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    TcpListener server = new TcpListener(IPAddress.Parse(IP), Port);
                    StringBuilder request = new StringBuilder();
                    server.Start();
                    TcpClient client = server.AcceptTcpClient();
                    NetworkStream stream = client.GetStream();

                    while (true)
                    {
                        request.Clear();
                        byte[] receiveData = new byte[256];

                        do
                        {
                            int bytes = stream.Read(receiveData, 0, receiveData.Length);
                            request.Append(Encoding.UTF8.GetString(receiveData, 0, bytes));
                        }
                        while (stream.DataAvailable);
                        string response = ExecuteCommand(request.ToString());

                        byte[] sendData = Encoding.UTF8.GetBytes(response);
                        stream.Write(sendData, 0, sendData.Length);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public static string ExecuteCommand(string command)
        {
            try
            {
                System.Diagnostics.ProcessStartInfo procStartInfo =
                    new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command);
                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                procStartInfo.CreateNoWindow = true;
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                string result = proc.StandardOutput.ReadToEnd();
                return result;
            }
            catch (Exception objException)
            {
                return objException.Message;
            }
        }
    }
}

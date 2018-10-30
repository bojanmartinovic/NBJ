using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server_Side
{
    class Program
    {
        static string max(Student[] x)
        {
            int k = 0;
            for (int i = 0; i < x.Length; i++)
                if (x[i].Prosek > x[k].Prosek)
                    k = i;
            return x[k].Ime;
        }

        static void Main(string[] args)
        {
            TcpListener listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 5000);
            TcpClient client = default(TcpClient);
            try
            {
                listener.Start();
            }
            catch
            {
                Console.WriteLine("error");
            }
            while (true)
            {
                client = listener.AcceptTcpClient();
                FileStream file = File.Create(@"Files\output.json");
                NetworkStream stream = client.GetStream();
                int readBytes;
                do
                {
                    byte[] buffer = new byte[1024];
                    readBytes = stream.Read(buffer, 0, 1024);
                    file.Write(buffer, 0, readBytes);
                } while (stream.DataAvailable);
                file.Close();
                Student[] students = JsonConvert.DeserializeObject<Student[]>(File.ReadAllText(@"Files\output.json"));
                byte[] returnBuffer = Encoding.ASCII.GetBytes(max(students));
                stream.Write(returnBuffer, 0, returnBuffer.Length);
                stream.Close();
                client.Close();
            }
        }
    }
}

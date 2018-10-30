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

        static double convert(string s, int p, int q)
        {
            int x = 0;
            int i;
            for (i = p; i <= q && s[i] != '.'; i++)
                x = 10 * x + s[i] - '0';
            if (i > q)
                return x;
            i++;
            double y = 0;
            for (int e = 10; i <= q; e /= 10, i++)
                y += (s[i] - '0') / e;
            return x + y;
        }

        static double calculate(string s)
        {
            int i;
            for (i = 0; char.IsDigit(s[i]); i++) ;
            double a = convert(s, 0, i - 1), b = convert(s, i + 1, s.Length - 1);
            if (s[i] == '+')
                return a + b;
            if (s[i] == '-')
                return a - b;
            if (s[i] == '*')
                return a * b;
            if (s[i] == '/' && b != 0)
                return a / b;
            return 0;
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
                NetworkStream stream = client.GetStream();
                /*FileStream file = File.Create(@"Files\output.json");
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
                stream.Write(returnBuffer, 0, returnBuffer.Length);*/

                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, 1024); 
                double s = calculate(Encoding.ASCII.GetString(buffer, 0, bytesRead));
                byte[] returnBuffer = BitConverter.GetBytes(s);
                stream.Write(returnBuffer, 0, returnBuffer.Length);

                stream.Close();
                client.Close();
            }
        }
    }
}

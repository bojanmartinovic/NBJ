using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client_Side
{
    class Program
    {
        static Student[] input()
        {
            Student[] students = new Student[int.Parse(Console.ReadLine())];
            for(int i = 0; i < students.Length; i++)
            {
                string ime = Console.ReadLine();
                double prosek = double.Parse(Console.ReadLine());
                students[i] = new Student(ime, prosek);
            }
            return students;
        }

        static void Main(string[] args)
        {
            //Student[] students = input();
            //File.WriteAllText(@"Files\list.json", JsonConvert.SerializeObject(students));
            TcpClient client = new TcpClient("127.0.0.1", 5000);
            //client.Client.SendFile(@"Files\list.json");
            NetworkStream stream = client.GetStream();
            byte[] input = Encoding.ASCII.GetBytes(Console.ReadLine());
            stream.Write(input, 0, input.Length);

            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, 1024);
            double result = BitConverter.ToDouble(buffer, 0);
            Console.WriteLine("{0}", result);
            Console.ReadKey();
        }
    }
}

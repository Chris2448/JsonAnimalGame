using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Client
{
    class Program //Client side
    {
        public static void Send(StreamWriter writer)
        {
            //Send messages to Server
            var sendtoserverMessage = Console.ReadLine();
            var messageToSend = JsonConvert.SerializeObject(sendtoserverMessage);
            writer.WriteLine(messageToSend);
            writer.Flush();
        }

        public static void Receive(StreamReader reader)
        {
            //Receive messages from Server
            var receivefromserverMessage = JsonConvert.DeserializeObject<string>(reader.ReadLine());
            Console.WriteLine("Server:" + receivefromserverMessage);
        }
        static void Main(string[] args)
        {
            var tcpClient = new TcpClient("127.0.0.1", 1988);
            Console.WriteLine("----------------------CLIENT--------------------------");
            var stream = tcpClient.GetStream();
            Console.WriteLine("Successfully connected to the server");
            Console.WriteLine("");
            Console.WriteLine("WELCOME TO THE ANIMAL GUESSING GAME!");
            Console.WriteLine("");
            Console.WriteLine("Think of one of the following animals: Eagle, Dog, Fish, Cobra, Crab, or Gorilla");
            Console.WriteLine("You will be asked a series of questions. Simply answer yes or no.");
            Console.WriteLine("");

            //Read and Write
            var reader = new StreamReader(stream);
            var writer = new StreamWriter(stream);

            //Data exchange
            while (true)
            {
                Receive(reader);
                Send(writer);
            }

            Console.ReadKey();

        }
    }
}

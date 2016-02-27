using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace Server
{
    class Program //Server side
    {
        public static void Send(StreamWriter writer, String sendtoclientMessage)
        {
            //Send message to Client
            var jsonMessage = JsonConvert.SerializeObject(sendtoclientMessage);
            writer.WriteLine(jsonMessage);
            writer.Flush();
        }

        public static string Receive(StreamReader reader,TcpClient tcpClient)
        { 
            //Receive message from Client
            var receivefromclientMessage = JsonConvert.DeserializeObject<string>(reader.ReadLine());
            Console.WriteLine("Client:" + receivefromclientMessage);

            
            return receivefromclientMessage;
        }

        static void Main(string[] args)
        {
            //Establish connection
            var tcpListener = new TcpListener(IPAddress.Any, 1988);
            tcpListener.Start();
            Console.WriteLine("------------------------SERVER----------------------------");
            var tcpClient = tcpListener.AcceptTcpClient();
            Console.WriteLine("Client has succesfully connected");
            var stream = tcpClient.GetStream();

            //Read and Write
            var reader = new StreamReader(stream);
            var writer = new StreamWriter(stream);

            //Questionnaire
            var questionsList = new Dictionary<string, string>
            {
                {"Fly", "Does it fly?"},
                {"FourLegs", "Does it have four legs?"},
                {"Swim", "Does it swim?"},
                {"Shell", "Does it have a shell?"},
                {"Thumb", "Does it have thumbs?"},
                {"Poison","Is it poisonous?" }

            };

            //Create List of Animals
            var animalsList = new List<Animal>
            {
                new Animal()
                {
                    Name = "Eagle",
                    Fly = true,
                    FourLegs = false,
                    Shell = false,
                    Swim = false,
                    Thumb = false,
                    Poison = false
                },
                new Animal()
                {
                    Name = "Dog",
                    Fly = false,
                    FourLegs = true,
                    Shell = false,
                    Swim = false,
                    Thumb = false,
                    Poison = false
                },
                new Animal()
                {
                    Name = "Fish",
                    Fly = false,
                    FourLegs = false,
                    Shell = false,
                    Swim = true,
                    Thumb = false,
                    Poison = false
                },
                new Animal()
                {
                    Name = "Crab",
                    Fly = false,
                    FourLegs = false,
                    Shell = true,
                    Swim = true,
                    Thumb = false,
                    Poison = false
                },
                new Animal()
                {
                    Name = "Gorilla",
                    Fly = false,
                    FourLegs = false,
                    Shell = false,
                    Swim = false,
                    Thumb = true,
                    Poison = false
                },
                new Animal()
                {
                    Name = "Cobra",
                    Fly = false,
                    FourLegs = false,
                    Shell = false,
                    Swim = false,
                    Thumb = false,
                    Poison = true
                },

            };

            //Animal List Copy
            var animalListCopy = new List<Animal>();
            animalListCopy.AddRange(animalsList);

            //Game
            foreach (var question in questionsList)
            {
                Send(writer, question.Value);
                var answer = Receive(reader, tcpClient) == "yes";

                foreach (var animal in animalsList)
                {
                    var property = animal.GetType().GetProperty(question.Key);
                    var attribute = (bool)property.GetValue(animal);

                    if (attribute != answer)
                    {
                        animalListCopy.Remove(animal);
                    }

                }

                if (animalListCopy.Count == 1)
                    break;
            }

            if (animalListCopy.Count == 1)
                Send(writer, "Your animal is a(n) " + animalListCopy.ElementAt(0).Name);
            else
            {
                Send(writer, "Animal not found");
            }

        }
    }
}


using System;
using System.IO;
using System.Xml;
using Newtonsoft.Json;

namespace epoxy
{
    class Program
    {
        static void Main(string[] args)
        {
            const string usageMessage = "Usage:\nepoxy <configuration json file>";
            if (args.Length != 1 || args[0] == "-h" || args[0] == "--help")
            { 
                Console.WriteLine(usageMessage);
                return;
            }

            string configurationJsonFilePath = args[0];
            if (!File.Exists(configurationJsonFilePath))
            {
                Console.WriteLine($"Configuration file '{configurationJsonFilePath}' does not exist.");

                if (!Path.IsPathRooted(configurationJsonFilePath))
                {
                    Console.WriteLine($"Path appears to be relative and the current working directory is '{Directory.GetCurrentDirectory()}'.");
                }

                return;
            }

            Configuration config = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(configurationJsonFilePath));

            Console.WriteLine("Hello Epoxy!");
        }
    }
}

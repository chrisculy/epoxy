using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using Epoxy.Api;
using Epoxy.Utility;
using Newtonsoft.Json;

namespace Epoxy
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
            (bool configIsValid, string message) = config.Validate();
            if (!configIsValid)
            {
                if (!message.IsNullOrEmpty())
                {
                    Console.WriteLine($"Invalid configuration: {message}");
                }

                return;
            }

            Graph graph = GraphLoader.LoadGraph(Directory.EnumerateFiles(config.DoxygenXmlDirectory).Where(file => Path.GetExtension(file) == ".xml"));

            // Generate bindings using binders

            Console.WriteLine("SUCCESS");
        }
    }
}

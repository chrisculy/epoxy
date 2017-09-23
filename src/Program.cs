using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using Epoxy.Api;
using Epoxy.Binders;
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
                Console.Error.WriteLine(usageMessage);
                return;
            }

            string configurationJsonFilePath = args[0];
            if (!File.Exists(configurationJsonFilePath))
            {
                Console.Error.WriteLine($"Configuration file '{configurationJsonFilePath}' does not exist.");

                if (!Path.IsPathRooted(configurationJsonFilePath))
                {
                    Console.Error.WriteLine($"Path appears to be relative and the current working directory is '{Directory.GetCurrentDirectory()}'.");
                }

                return;
            }

            Configuration config = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(configurationJsonFilePath));
            (bool configIsValid, string message) = config.Validate();
            if (!configIsValid)
            {
                if (!message.IsNullOrEmpty())
                {
                    Console.Error.WriteLine($"Invalid configuration: {message}");
                }

                return;
            }

            Graph graph = GraphLoader.LoadGraph(Directory.EnumerateFiles(config.DoxygenXmlDirectory).Where(file => Path.GetExtension(file) == ".xml"));
            foreach (BinderConfiguration binderConfig in config.Binders)
            {
                IBinder binder = BinderFactory.GetBinder(binderConfig);
                if (binder == null)
                {
                    Console.WriteLine($"Warning: language '{binderConfig.Language}' currently does not have a binder implemented-skipping.");
                    continue;
                }

                binder.GenerateNativeBindings(graph);
                binder.GenerateLanguageBindings(graph);
            }

            Console.WriteLine("SUCCESS");
        }
    }
}

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Epoxy.Utility
{
    public static class ConfigurationValidation
    {
        public static (bool isValid, string message) Validate(this Configuration configuration)
        {
            foreach (string language in configuration.Languages)
            {
                if (!c_supportedLanguages.Contains(language))
                {
                    return (false, $"'{language}' is not a supported language ('{c_supportedLanguages.Join(", ")}').");
                }
            }

            if (!Directory.Exists(configuration.DoxygenXmlDirectory))
            {
                return (false, $"Given Doxygen XML directory '{configuration.DoxygenXmlDirectory}' does not exist.");
            }

            return (true, string.Empty);
        }

        private static readonly HashSet<string> c_supportedLanguages = new HashSet<string>{ "c#", "java", "javascript", "typescript" };
    }
}
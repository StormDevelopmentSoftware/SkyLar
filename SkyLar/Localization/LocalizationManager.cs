using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace SkyLar.Localization
{
    public static class LocalizationManager
    {
        public static List<Localization> Localizations = new List<Localization>();
        public static void Load()
        {
            var directory = Directory.GetCurrentDirectory() + "/localizations/";
            var files = Directory.GetFiles(directory, "*.json");

            foreach (var file in files)
            {
                var locale = JsonConvert.DeserializeObject<Localization>(File.ReadAllText(file));
                Localizations.Add(locale);
            }
        }
    }
}

using System;
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
            var directory = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "Localizations"));

            if (!directory.Exists)
                directory.Create();

            foreach (var file in directory.GetFiles("*.json", SearchOption.TopDirectoryOnly))
            {
                using (var reader = file.OpenText())
                {
                    try
                    {
                        var locale = JsonConvert.DeserializeObject<Localization>(reader.ReadToEnd());
                        Localizations.Add(locale);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("Cannot load localization file {0}\n{1}", file.FullName, ex);
                    }
                }
            }
        }
    }
}

// File LocalizationManager.cs created by Animadoria (me@animadoria.cf) at 11/2/2019 12:29 AM.
// (C) Animadoria 2019 - All Rights Reserved
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

// File Localization.cs for the SkyLar Discord bot at 11/2/2019 12:14 AM.
// (C) Storm Development Software - 2019. All Rights Reserved
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SkyLar.Localization
{
    public class Localization
    {
        [JsonProperty, JsonRequired]
        public Language Language { get; set; }

        [JsonProperty, JsonRequired]
        public string Name { get; set; }

        [JsonProperty, JsonRequired]
        public string CountryCode { get; set; }

        [JsonProperty, JsonRequired]
        public string CultureCode { get; set; }

        [JsonProperty, JsonRequired]
        public char Unit { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Include)]
        public Dictionary<string, string> Strings { get; set; }

        public string GetString(string key)
        {
            if (Strings.ContainsKey(key))
                return Strings[key];
            return key;
        }

        [JsonIgnore]
        public string this[string key]
        {
            get => GetString(key);
        }
    }

    public enum Language
    {
        English = 1,
        Portuguese = 2
    }
}

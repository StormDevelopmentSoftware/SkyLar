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
            if (this.Strings.ContainsKey(key))
                return this.Strings[key];
            return key;
        }

        [JsonIgnore]
        public string this[string key]
        {
            get => this.GetString(key);
        }
    }

    public enum Language
    {
        English = 1,
        Portuguese = 2
    }
}

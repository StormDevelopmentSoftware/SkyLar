using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace SkyLar.Utilities
{
    public static class FileUtilities
    {
        private static readonly JsonSerializerSettings DEFAULT_JSON_SETTINGS = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy(true, false)
            }
        };

        public static object ReadJson(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            var fileinfo = new FileInfo(path);

            if (!fileinfo.Exists)
                throw new FileNotFoundException(string.Empty, path);

            using (var fs = fileinfo.OpenRead())
            using (var sr = new StreamReader(fs))
                return JsonConvert.DeserializeObject(sr.ReadToEnd());
        }

        public static T ReadJson<T>(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            var fileinfo = new FileInfo(path);

            if (!fileinfo.Exists)
                throw new FileNotFoundException(string.Empty, path);

            using (var fs = fileinfo.OpenRead())
            using (var sr = new StreamReader(fs))
                return JsonConvert.DeserializeObject<T>(sr.ReadToEnd(), DEFAULT_JSON_SETTINGS);
        }

        public static bool Exists(string path)
        {
            return File.Exists(path);
        }

        public static void WriteJson(object obj, string path, bool replace = true, JsonSerializerSettings settings = null)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            if (Exists(path) && !replace)
                return;

            using (var fs = new FileStream(path, FileMode.Create))
            using (var sw = new StreamWriter(fs))
            {
                sw.WriteLine(JsonConvert.SerializeObject(obj, Formatting.Indented, settings ?? DEFAULT_JSON_SETTINGS));
                sw.Flush();
            }
        }

        public static JObject FromJson(string raw)
        {
            return JObject.Parse(raw);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SkyLar.Utilities
{
    public static class FileUtilities
    {
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
                return JsonConvert.DeserializeObject<T>(sr.ReadToEnd());
        }

        public static bool Exists(string path)
        {
            return File.Exists(path);
        }

        public static void WriteJson(object obj, string path, bool replace = true)
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
                sw.WriteLine(JsonConvert.SerializeObject(obj, Formatting.Indented));
                sw.Flush();
            }
        }

        public static JObject ParseJson(string raw)
        {
            return JObject.Parse(raw);
        }
    }
}

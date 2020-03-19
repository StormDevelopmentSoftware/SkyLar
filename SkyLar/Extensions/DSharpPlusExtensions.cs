using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using System.Linq;
using DSharpPlus.CommandsNext.Converters;
using System.Reflection;
using SkyLar.Attributes;
using System;

namespace SkyLar.Extensions
{
    public static class DSharpPlusExtensions
    {
        /// <summary>
        /// Registra automaticamente todos os conversores no módulo de comandos.
        /// </summary>
        /// <param name="ext">Instância do módulo de comandos.</param>
        public static void RegisterArgumentConverters(this CommandsNextExtension ext)
        {
            var rc = ext.GetType().GetMethod("RegisterConverter");
            var rct = ext.GetType().GetMethod("RegisterUserFriendlyTypeName");

            var types = from t in typeof(DSharpPlusExtensions).Assembly.GetTypes()
                        where typeof(IArgumentConverter).IsAssignableFrom(t)
                        where t.GetCustomAttribute<ArgumentConverterAttribute>() != null
                        select new { Type = t, MetaData = t.GetCustomAttribute<ArgumentConverterAttribute>() };

            foreach (var iterator in types)
            {
                try
                {
                    // RegisterConverter<T>(IArgumentConverter<T> converter)

                    var rci = rc.MakeGenericMethod(iterator.MetaData.ArgumentType);
                    var instance = Activator.CreateInstance(iterator.Type);
                    rci.Invoke(ext, new object[] { instance });

                    Console.WriteLine("[ArgumentConverter::RegisterConverter] Argument Type: {0}; Impl: {1}",
                        iterator.MetaData.ArgumentType.Name, iterator.Type.Name);

                    // RegisterUserFriendlyTypeName<T>(string name)

                    var rcti = rct.MakeGenericMethod(iterator.MetaData.ArgumentType);
                    rcti.Invoke(ext, new object[] { iterator.MetaData.Name });

                    Console.WriteLine("[ArgumentConverter::RegisterUserFriendlyTypeName] Argument Type: {0}; Name: {1}",
                        iterator.MetaData.ArgumentType.Name, iterator.MetaData.Name);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[DSharpPlusExtensions::RegisterArgumentConverters] Failed to register argument converter: {0}. {1}",
                        iterator.Type.FullName, ex);
                }
            }
        }
    }
}

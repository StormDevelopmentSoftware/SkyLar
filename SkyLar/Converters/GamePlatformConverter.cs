using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.Entities;
using SkyLar.Attributes;
using SkyLar.Entities;

namespace SkyLar.Converters
{
    [ArgumentConverter(Name = "platform", ArgumentType = typeof(GamePlatform))]
    public class GamePlatformConverter : IArgumentConverter<GamePlatform>
    {
        public async Task<Optional<GamePlatform>> ConvertAsync(string value, CommandContext ctx)
        {
            await Task.Yield();

            if (string.IsNullOrEmpty(value))
                return Optional.FromNoValue<GamePlatform>();

            switch (value.ToLowerInvariant())
            {
                case "pc":
                case "computer":
                    return Optional.FromValue(GamePlatform.PC);

                case "ps":
                case "ps4":
                case "psn":
                    return Optional.FromValue(GamePlatform.PS4);

                case "xbx":
                case "xbox":
                case "xone":
                case "xbox1":
                    return Optional.FromValue(GamePlatform.XBOX);

                default:
                    return Optional.FromNoValue<GamePlatform>();
            }
        }
    }
}
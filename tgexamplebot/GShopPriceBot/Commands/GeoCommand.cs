using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace GShopPriceBot.Commands
{
    /// <summary>
    /// "/geo" command handler
    /// </summary>
    public class GeoCommand : BaseCommand
    {
        public override string Name => "/geo";

        public GeoCommand(ITelegramBotClient bot)
            : base(bot)
        {

        }

        public override async Task ExecuteAsync(CommandEventArgs e)
        {
            await e.Bot.SendLocationAsync(e.ChatId, 41.315506f, 69.284932f);
        }
    }
}

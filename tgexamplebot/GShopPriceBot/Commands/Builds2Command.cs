using GShopPriceBot.Storage;
using Telegram.Bot;

namespace GShopPriceBot.Commands
{
    /// <summary>
    /// "/b" command handler
    /// </summary>
    public class Builds2Command : FindCommand
    {
        public override string Name => "/b";

        public Builds2Command(ITelegramBotClient bot, IPriceStorage storage)
            : base(bot, storage)
        {
           
        }
    }
}

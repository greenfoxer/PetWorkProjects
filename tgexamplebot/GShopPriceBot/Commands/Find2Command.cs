using GShopPriceBot.Storage;
using Telegram.Bot;

namespace GShopPriceBot.Commands
{
    /// <summary>
    /// "/f" command handler
    /// </summary>
    public class Find2Command : FindCommand
    {
        public override string Name => "/f";

        public Find2Command(ITelegramBotClient bot, IPriceStorage storage)
            : base(bot, storage)
        {
           
        }
    }
}

using GShopPriceBot.Extensions;
using GShopPriceBot.Storage;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace GShopPriceBot.Commands
{
    /// <summary>
    /// "/cat" command handler
    /// </summary>
    public class Categories2Command : CategoriesCommand
    {
        public override string Name => "/cat";

        public Categories2Command(ITelegramBotClient bot, IPriceStorage storage)
            : base(bot, storage)
        {
        }
    }
}

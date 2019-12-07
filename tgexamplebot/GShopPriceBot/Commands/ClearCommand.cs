using GShopPriceBot.Storage;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace GShopPriceBot.Commands
{
    /// <summary>
    /// "/clear" command handler
    /// </summary>
    public class ClearCommand : BaseCommand
    {
        private readonly IPriceStorage _storage;

        public override string Name => "/clear";

        public ClearCommand(ITelegramBotClient bot, IPriceStorage storage)
            : base(bot)
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        public override async Task ExecuteAsync(CommandEventArgs e)
        {
            if (!e.IsBotOwner)
                return;

            _storage.ClearPrices();

            await Bot.SendTextMessageAsync(e.ChatId, "База данных очищена.", replyToMessageId: (int)e.MessageId);
        }
    }
}

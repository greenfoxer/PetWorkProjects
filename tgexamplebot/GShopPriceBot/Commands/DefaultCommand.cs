﻿using System.Threading.Tasks;
using Telegram.Bot;

namespace GShopPriceBot.Commands
{
    public class DefaultCommand : BaseCommand
    {
        public override string Name => string.Empty;

        public DefaultCommand(ITelegramBotClient bot)
            : base(bot)
        {

        }

        public override async Task ExecuteAsync(CommandEventArgs e)
        {
            await Bot.SendTextMessageAsync(e.ChatId, "Я не знаю такую команду. Предлагаю воспользоваться справкой /help", replyToMessageId: (int)e.MessageId);
        }
    }
}

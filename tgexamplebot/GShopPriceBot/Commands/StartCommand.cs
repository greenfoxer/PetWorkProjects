using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace GShopPriceBot.Commands
{
    /// <summary>
    /// "/start" command handler
    /// </summary>
    public class StartCommand : BaseCommand
    {
        public override string Name => "/start";

        public StartCommand(ITelegramBotClient bot)
            : base(bot)
        {

        }

        public override async Task ExecuteAsync(CommandEventArgs e)
        {
            await Bot.SendTextMessageAsync(e.ChatId, 
                "Этот бот умеет искать позиции в официальном прайсе G-SHOP.UZ.\n" +
                "Введите /help, чтобы получить справочную информацию.");
        }
    }
}

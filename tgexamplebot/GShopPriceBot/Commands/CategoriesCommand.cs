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
    /// "/categories" command handler
    /// </summary>
    public class CategoriesCommand : BaseCommand
    {
        private readonly IPriceStorage _storage;

        public override string Name => "/categories";

        public CategoriesCommand(ITelegramBotClient bot, IPriceStorage storage)
            : base(bot)
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        public override async Task ExecuteAsync(CommandEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.CommandLine))
                return;

            var me = await Bot.GetMeAsync();
            var filtered = _storage.Find(e.CommandLine, null, 10)
                .Where(p => p.RetailPrice > 0m)
                .ToArray();

            if (filtered.Length == 0)
            {
                await Bot.SendTextMessageAsync(e.ChatId, "Ничего не найдено, сорянба :(", replyToMessageId: (int)e.MessageId);
            }
            else
            {
                var rate = filtered.FirstOrDefault()?.CurrencyRate ?? 1.0m;
                var result = filtered.GroupBy(p => p.Category)
                    .Select(g => g.ToFindResult())
                    .ToArray();

                var message = result.ToFindResult(e.CommandLine, rate, me.Username);
                await Bot.SendTextMessageAsync(e.ChatId, message, ParseMode.Markdown, replyToMessageId: (int)e.MessageId);
            }
        }
    }
}

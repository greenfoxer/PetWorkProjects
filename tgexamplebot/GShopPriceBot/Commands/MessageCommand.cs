using GShopPriceBot.Storage;
using System;
using System.IO;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace GShopPriceBot.Commands
{
    /// <summary>
    /// "/data" command handler
    /// </summary>
    public class MessageCommand : BaseCommand
    {
        private readonly IPriceStorage _storage;

        public override string Name => "/msg";

        public MessageCommand(ITelegramBotClient bot, IPriceStorage storage)
            : base(bot)
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        public override async Task ExecuteAsync(CommandEventArgs e)
        {
            if (!e.IsBotOwner)
                return;

            // get all unique users
            var userIds = _storage.GetUniqueUserIds();
            
            foreach (var userId in userIds)
            {
                await e.Bot.SendTextMessageAsync(new ChatId(userId), e.CommandLine, ParseMode.Default)
                    .ContinueWith(_ => Task.CompletedTask); // skip errors
            }
        }
    }
}

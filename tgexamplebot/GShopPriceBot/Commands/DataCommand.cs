using System.IO;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace GShopPriceBot.Commands
{
    /// <summary>
    /// "/data" command handler
    /// </summary>
    public class DataCommand : BaseCommand
    {
        public override string Name => "/data";

        public DataCommand(ITelegramBotClient bot)
            : base(bot)
        {

        }

        public override async Task ExecuteAsync(CommandEventArgs e)
        {
            if (!e.IsBotOwner)
                return;

            using (var stream = File.OpenRead("/app/data/prices.db3"))
            {
                var document = new InputOnlineFile(stream, "data.db3");
                await e.Bot.SendDocumentAsync(e.ChatId, document, replyToMessageId: (int)e.MessageId);
            }
        }
    }
}

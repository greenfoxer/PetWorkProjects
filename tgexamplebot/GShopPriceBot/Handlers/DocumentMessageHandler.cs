using GShopPriceBot.Extensions;
using GShopPriceBot.Parsers;
using GShopPriceBot.Storage;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace GShopPriceBot.Handlers
{
    /// <summary>
    /// Document message handler
    /// </summary>
    public class DocumentMessageHandler : BaseMessageHandler
    {
        private readonly ILogger _log;
        private readonly IPriceParcer _parser;
        private readonly IPriceStorage _storage;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bot">Reference to bot</param>
        /// <param name="defaultCommand">If no commands matched, then default command will be executed</param>
        /// <param name="commands">All possible commands list</param>
        public DocumentMessageHandler(
            ILogger logger,
            ITelegramBotClient bot,
            IPriceParcer parser,
            IPriceStorage storage)
            : base(bot)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _log = logger.ForContext<DocumentMessageHandler>();
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        /// <inheritdoc />
        public override async Task HandleAsync(Message message)
        {
            _log.Information("Handling a new document {filename}...", message.Document.FileName);

            var member = await Bot.GetChatMemberAsync(message.Chat.Id, message.From.Id);
            if (!message.Document.FileName.EndsWith("xlsx") ||
                !member.IsBotOwner())
                return;

            _log.Information("Downloading the file {filename}", message.Document.FileName);

            var file = await Bot.GetFileAsync(message.Document.FileId);
            
            using (var stream = new MemoryStream())
            {
                await Bot.DownloadFileAsync(file.FilePath, stream);
                stream.Seek(0, SeekOrigin.Begin);

                var parsed = _parser.Parse(stream);

                if (parsed.Length == 0)
                {
                    await Bot.SendTextMessageAsync(message.Chat.Id, "Позиции отсутсвуют.", replyToMessageId: message.MessageId);
                    return;
                }

                // remove old positions
                _storage.ClearPrices();

                // add new positions
                _storage.AddPositions(parsed);
            }

            await Bot.SendTextMessageAsync(message.Chat.Id, "Прайс добавлен.",  replyToMessageId: message.MessageId);
        }
    }
}

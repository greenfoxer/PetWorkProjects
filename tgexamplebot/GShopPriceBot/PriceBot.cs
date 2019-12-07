using GShopPriceBot.Commands;
using GShopPriceBot.Handlers;
using GShopPriceBot.Parsers;
using GShopPriceBot.Storage;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;

namespace GShopPriceBot
{
    public class PriceBot
    {
        private readonly ILogger _log;
        private readonly ITelegramBotClient _bot;
        private readonly IDictionary<MessageType, IMessageHandler> _messageHandlers;
        private IQueryHandler _queryHandler;

        public PriceBot(ILogger logger, ITelegramBotClient bot)
        {
            _log = logger.ForContext<PriceBot>();
            _bot = bot;
            _messageHandlers = new Dictionary<MessageType, IMessageHandler>();

            InitEvents();
            InitHandlers();
        }

        private void InitEvents()
        {
            _log.Information("Registering event handlers...");

            _bot.OnMessage += async (s, e) => await OnMessage(s, e);
            _bot.OnInlineQuery += async (s, e) => await OnInlineQuery(s, e);
        }

        private void InitHandlers()
        {
            _log.Information("Registering message handlers...");

            var storage = new SqlitePriceStorage_V2(_log);
            storage.Init();

            var parser = new PriceParser_NPOI_V2(_log);

            var commands = new ICommand[]
            {
                new HelpCommand(_bot),
                new StartCommand(_bot),
                new FindCommand(_bot, storage),
                new Find2Command(_bot, storage),
                new ClearCommand(_bot, storage),
                new GeoCommand(_bot),
                new DataCommand(_bot),
                new MessageCommand(_bot, storage)
            };

            _messageHandlers[MessageType.Text] = new TextMessageHandler(_log, _bot, storage, new DefaultCommand(_bot), commands);
            _messageHandlers[MessageType.Document] = new DocumentMessageHandler(_log, _bot, parser, storage);

            _queryHandler = new InlineQueryHandler(_log, _bot, storage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private async Task OnMessage(object sender, MessageEventArgs e)
        {
            _log.Information("Message {messageId} received.", e.Message.MessageId);

            if (_messageHandlers.TryGetValue(e.Message.Type, out var handler))
            {
                try
                {
                    await handler.HandleAsync(e.Message);
                }
                catch (Exception ex)
                {
                    _log.Error(ex, "Message handling error.");
                }
            }
            else
            {
                Log.Warning("Unhandled message {@message}", e.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async Task OnInlineQuery(object sender, InlineQueryEventArgs e)
        {
            _log.Information("Inline query {queryId} received.", e.InlineQuery.Id);

            try
            {
                await _queryHandler.HandleAsync(e.InlineQuery);
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Inline query error");
            }
        }

        /// <summary>
        /// But bot infinite loop and receive commands
        /// </summary>
        /// <param name="token"></param>
        public void Run(ManualResetEvent resetEvent)
        {
            _log.Information("Starting the bot...");
            _bot.StartReceiving();
            _log.Information("Bot started. Listening commands...");

            resetEvent.WaitOne();
        }
    }
}

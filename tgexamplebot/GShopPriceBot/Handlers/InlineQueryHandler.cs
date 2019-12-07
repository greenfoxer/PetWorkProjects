using GShopPriceBot.Commands;
using GShopPriceBot.Extensions;
using GShopPriceBot.Models;
using GShopPriceBot.Storage;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;

namespace GShopPriceBot.Handlers
{
    /// <summary>
    /// Inline query handler
    /// </summary>
    public class InlineQueryHandler : BaseQueryHandler
    {
        private readonly ILogger _log;
        private readonly IPriceStorage _storage;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bot">Reference to bot</param>
        /// <param name="defaultCommand">If no commands matched, then default command will be executed</param>
        /// <param name="commands">All possible commands list</param>
        public InlineQueryHandler(
            ILogger logger,
            ITelegramBotClient bot,
            IPriceStorage storage)
            : base(bot)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));
            _log = logger.ForContext<TextMessageHandler>();
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        /// <inheritdoc />
        public override async Task HandleAsync(InlineQuery query)
        {
            _log.Information("Handling a new inline query: {query}", query.Query);

            if (string.IsNullOrWhiteSpace(query.Query))
                return;

            var me = await Bot.GetMeAsync();

            // log user query
            _storage.AddUserQuery(new UserQuery(query.From, query.Query));

            var filtered = _storage.Find(query.Query, null, 10)
                .Where(p => p.RetailPrice > 0m)
                .ToArray();

            if (filtered.Length == 0)
            {
                await Bot.AnswerInlineQueryAsync(query.Id, Enumerable.Empty<InlineQueryResultBase>());
            }
            else
            {
                var rate = filtered.FirstOrDefault()?.CurrencyRate ?? 1.0m;
                var result = filtered
                    .Select((p, i) => p.ToInlineResult(i, query.Query, rate, me.Username));
                await Bot.AnswerInlineQueryAsync(query.Id, result);
            }
        }
    }
}

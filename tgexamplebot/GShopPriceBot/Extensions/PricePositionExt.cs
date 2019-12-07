using GShopPriceBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;

namespace GShopPriceBot.Extensions
{
    /// <summary>
    /// PricePosition extension methods
    /// </summary>
    public static class PricePositionExt
    {
        private static readonly string ResultsTemplate =
            "Результат поиска на запрос `{0}`:\n" +
            "Курс `1` у.е. = `{1:#,##0.00}` сум\n\n" +
            "{2}\n\n" +
            "[https://t.me/{3}](https://t.me/{3})";

        /// <summary>
        /// Build result reponse message from position
        /// </summary>
        /// <param name="p"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string ToFindResult(this PricePosition p)
        {
            return $">>> `{p.Name}`\n\n" +
                    $"   Цена: `{p.RetailPrice:#,##0.00}` у.е. / `{p.RetailPrice * p.CurrencyRate:#,##0.00}` сум\n" +
                    (p.BundlePrice > 0m
                        ? $"   В сборке: `{p.BundlePrice:#,##0.00}` у.е / `{p.BundlePrice * p.CurrencyRate:#,##0.00}` сум\n"
                        : string.Empty) +
                    (p.SpecialOfferPrice > 0m
                        ? $"   По акции: `{p.SpecialOfferPrice:#,##0.00}` у.е / `{p.SpecialOfferPrice * p.CurrencyRate:#,##0.00}` сум\n"
                        : string.Empty);
        }

        /// <summary>
        /// Build inline query result from price position
        /// </summary>
        /// <param name="p"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static InlineQueryResultBase ToInlineResult(
            this PricePosition p,
            int i,
            string query,
            decimal rate,
            string userName)
        {
            var message = string.Format(
                ResultsTemplate,
                query,
                rate,
                string.Format("{0}:\n\n{1}", p.Category, p.ToFindResult()),
                userName);

            var content = new InputTextMessageContent(message)
            {
                ParseMode = ParseMode.Markdown
            };
            return new InlineQueryResultArticle(i.ToString(), p.Name, content);
        }

        /// <summary>
        /// Convert positions search result to the response message
        /// </summary>
        /// <param name="result"></param>
        /// <param name="query"></param>
        /// <param name="rate"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public static string ToFindResult(this IEnumerable<string> result, string query, decimal rate, string username)
        {
            return string.Format(
                ResultsTemplate,
                query,
                rate,
                string.Join('\n', result),
                username);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grp"></param>
        /// <returns></returns>
        public static string ToFindResult(this IGrouping<string, PricePosition> grp)
        {
            return string.Format(
                "{0}:\n\n{1}",
                grp.Key,
                string.Join('\n', grp.Select(p => p.ToFindResult())));
        }
    }
}

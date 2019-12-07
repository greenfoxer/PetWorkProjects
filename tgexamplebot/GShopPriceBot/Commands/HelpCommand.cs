using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace GShopPriceBot.Commands
{
    /// <summary>
    /// "/help" command handler
    /// </summary>
    public class HelpCommand : BaseCommand
    {
        public override string Name => "/help";

        public HelpCommand(ITelegramBotClient bot)
            : base(bot)
        {

        }

        public override async Task ExecuteAsync(CommandEventArgs e)
        {
            const string message = 
                "Доступные команды:\n" +
                "/cat `<название>` - Смотрите комманду /categories\n" +
                "/categories `<название>` - Получить все позиции по названию категории.\n" +
                "/geo - Получить геолокацию магазина\n" +
                "/f `<название>` - Смотрите комманду /find\n" +
                "/find `<название>` - Вызов поиска позиций по названию\n" +
                "/help - Выводит справку\n" +
                "Используйте комманду без названия, чтобы получить весь список категорий\n" +
                "\n\n" +
                "Так же вы можете вызвать бота в любом чате, даже там, где нет самого бота.\n" +
                "Для этого начните писать сообщение в чате следующим образом:\n" +
                "`@gshopuz_price_bot <название>`\n" +
                "где `<название>` это название товара для поиска.";

            await e.Bot.SendTextMessageAsync(e.ChatId, message, ParseMode.Markdown);
        }
    }
}

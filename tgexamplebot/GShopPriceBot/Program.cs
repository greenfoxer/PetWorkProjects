using Serilog;
using System;
using System.Globalization;
using System.Text;
using System.Threading;
using Telegram.Bot;

namespace GShopPriceBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
#if DEBUG
                .MinimumLevel.Debug()
#endif
                .WriteTo.File("./logs/log.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.Console()
                .CreateLogger();

            var resetEvent = new ManualResetEvent(false);

            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");

            if (Environment.UserInteractive)
            {
                Console.OutputEncoding = Encoding.Unicode;
            }

            AppDomain.CurrentDomain.ProcessExit += (s, e) => resetEvent.Set();
            Console.CancelKeyPress += (s, e) =>
            {
                Log.Warning("Program exit requested. Exiting...");
                resetEvent.Set();
                e.Cancel = true;
            };

            var token = Environment.GetEnvironmentVariable("GSHOP_BOT_TOKEN");

            if (token == null)
            {
                Log.Error("Bot token was not specified. Please, check your GSHOP_BOT_TOKEN environment variable");
                return;
            }

            var client = new TelegramBotClient(token);
            var bot = new PriceBot(Log.Logger, client);

            bot.Run(resetEvent);
        }
    }
}

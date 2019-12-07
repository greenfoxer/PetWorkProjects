using GShopPriceBot.Parsers;
using Serilog;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserTests
{
    class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var parser = new PriceParser_NPOI(Log.Logger);
            var fileName = @"./data/прайс_G_Computers_у_е_18.04.2019.xlsx";
            var categories = parser.Parse(fileName)
                .GroupBy(p => p.Category);

            foreach (var category in categories)
            {
                Console.WriteLine(category.Key);
                foreach (var position in category)
                {
                    Console.WriteLine("\t" + position);
                }
            }
        }
    }
}

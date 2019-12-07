using GShopPriceBot.Models;
using System.IO;

namespace GShopPriceBot.Parsers
{
    /// <summary>
    /// G-SHOP.UZ price parser interface
    /// </summary>
    public interface IPriceParcer
    {
        /// <summary>
        /// Parse price file and return price items
        /// </summary>
        /// <param name="fileName">Path to price filename</param>
        /// <returns></returns>
        PricePosition[] Parse(string fileName);

        /// <summary>
        /// Parse price stream and return price items
        /// </summary>
        /// <param name="stream">Stream that contains price data</param>
        /// <returns></returns>
        PricePosition[] Parse(Stream stream);
    }
}

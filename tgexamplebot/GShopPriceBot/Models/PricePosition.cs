using System;

namespace GShopPriceBot.Models
{
    /// <summary>
    /// G-SHOP.UZ Price list item position
    /// </summary>
    public class PricePosition
    {
        /// <summary>
        /// Item name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// UZS to USD course rate
        /// </summary>
        public decimal CurrencyRate { get; set; }

        /// <summary>
        /// Item retail price
        /// </summary>
        public decimal RetailPrice { get; set; }

        /// <summary>
        /// Item bundle price
        /// </summary>
        public decimal BundlePrice { get; set; }

        /// <summary>
        /// Special offer price
        /// </summary>
        public decimal SpecialOfferPrice { get; set; }

        /// <summary>
        /// Was price changed (red color)
        /// </summary>
        public bool IsPriceChanged { get; set; }

        /// <summary>
        /// Item main category
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Item sub-category
        /// </summary>
        public string SubCategory { get; set; }

        /// <summary>
        /// Position created (parsed) datetime
        /// </summary>
        public DateTime CreatedAt { get; set; }

        public override string ToString()
        {
            return $"{Name} | {RetailPrice} | {BundlePrice}";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace GShopPriceBot.Extensions
{
    /// <summary>
    /// String extension methods
    /// </summary>
    public static class StringExt
    {
        /// <summary>
        /// Sanitize SQL value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Sanitize(this string value) => value.Replace("'", "''").Replace(@"\", @"\\");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Prepare(this string value) => "%" + Regex.Replace(value, @"\s+", "%") + "%";
    }
}

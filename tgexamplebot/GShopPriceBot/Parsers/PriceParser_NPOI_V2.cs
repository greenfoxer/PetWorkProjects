using GShopPriceBot.Models;
using GShopPriceBot.Parsers;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using Serilog;
using System;
using System.IO;

namespace GShopPriceBot.Parsers
{
    /// <summary>
    /// Price parser based on NPOI
    /// </summary>
    public class PriceParser_NPOI_V2 : IPriceParcer
    {
        private readonly ILogger _log;
        public PriceParser_NPOI_V2(ILogger logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _log = logger.ForContext<PriceParser_NPOI_V2>();
        }

        public PricePosition[] Parse(Stream stream)
        {
            try
            {
                var xlsx = new XSSFWorkbook(stream);
                var sheet = xlsx[0];

                // first row contains price data
                var priceLine = sheet.GetRow(0).GetCell(0).StringCellValue;
                var currencyRate = string.IsNullOrWhiteSpace(priceLine)
                        ? 0m
                        : decimal.TryParse(Regex.Match(priceLine, "[0-9]+").Value, out var rate) ? rate : 0m;

                _log.Information("Currency rate extracted: {currencyRate}", currencyRate);

                _log.Information("Parsing lines...");

                var result = Enumerable.Empty<PricePosition>()
                    .Concat(ParseColumn(sheet, 0, currencyRate))
                    .Concat(ParseColumn(sheet, 4, currencyRate))
                    .Concat(ParseColumn(sheet, 8, currencyRate, false, false))
                    .Concat(ParseColumn(sheet, 10, currencyRate, includeSpecialOfferPrice: false))
                    .ToArray();

                xlsx.Close();

                _log.Information("Parsing finished. {lines} lines parsed", result.Length);

                return result;
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Parsing error");
                return new PricePosition[0];
            }
        }

        public PricePosition[] Parse(string fileName)
        {
            _log.Information("Parsing started: {fileName}", fileName);

            using (var stream = File.OpenRead(fileName))
                return Parse(stream);
        }

        /// <summary>
        /// Parse price column
        /// </summary>
        /// <param name="sheet">Price sheet</param>
        /// <param name="columnOffset">Column offset</param>
        /// <param name="currencyRate">Currency rate</param>
        /// <param name="includeBundlePrice">Look for bundle price</param>
        /// <returns></returns>
        private IEnumerable<PricePosition> ParseColumn(
            ISheet sheet, 
            int columnOffset, 
            decimal currencyRate, 
            bool includeBundlePrice = true,
            bool includeSpecialOfferPrice = true)
        {
            var rowId = 2; // actual data starts from third row

            var category = string.Empty;
            var subCategory = string.Empty;
            var catSetFlag = false;

            while (true)
            {
                var row = sheet.GetRow(rowId);
                if (row == null)
                    break;

                var nameCell = row.GetCell(columnOffset + 0);
                var retailPriceCell = row.GetCell(columnOffset + 1);
                var bundlePriceCell = row.GetCell(columnOffset + 2);
                var specialOfferPriceCell = row.GetCell(columnOffset + 3);

                // break parsing if name cell is empty
                if (nameCell == null)
                    break;

                var nameCellStyle = (XSSFCellStyle)nameCell.CellStyle;
                if (nameCellStyle.FillForegroundColorColor?.RGB != null &&
                    nameCellStyle.FillForegroundColorColor.RGB[0] == 235 &&
                    nameCellStyle.FillForegroundColorColor.RGB[1] == 239 &&
                    nameCellStyle.FillForegroundColorColor.RGB[2] == 148)
                {
                    if (catSetFlag)
                    {
                        subCategory = nameCell.StringCellValue.Trim();
                        catSetFlag = false;
                    }
                    else
                    {
                        category = nameCell.StringCellValue.Trim();
                        catSetFlag = true;
                    }
                }
                else
                {
                    catSetFlag = false;

                    var name = nameCell.StringCellValue.Trim();

                    // break if name is empty
                    if (string.IsNullOrEmpty(name))
                        break;

                    _log.Debug("Parsing line {name}", name);

                    var retailPrice = retailPriceCell.CellType != CellType.Numeric
                        ? 0m
                        : (decimal)retailPriceCell.NumericCellValue;

                    var bundlePrice = !includeBundlePrice || bundlePriceCell == null || bundlePriceCell.CellType != CellType.Numeric
                        ? 0m
                        : (decimal)bundlePriceCell.NumericCellValue;

                    var specialOfferPrice = !includeSpecialOfferPrice || specialOfferPriceCell == null || specialOfferPriceCell.CellType != CellType.Numeric
                        ? 0m
                        : (decimal)specialOfferPriceCell.NumericCellValue;

                    var priceChanged = (retailPriceCell?.CellStyle as XSSFCellStyle)?.FillForegroundXSSFColor != null ||
                        (bundlePriceCell?.CellStyle as XSSFCellStyle)?.FillForegroundXSSFColor != null;

                    yield return new PricePosition
                    {
                        Name = name,
                        Category = category,
                        SubCategory = subCategory,
                        RetailPrice = retailPrice,
                        BundlePrice = bundlePrice,
                        SpecialOfferPrice = specialOfferPrice,
                        IsPriceChanged = priceChanged,
                        CurrencyRate = currencyRate,
                        CreatedAt = DateTime.Now,
                    };
                }

                rowId++;
            }
        }
    }
}

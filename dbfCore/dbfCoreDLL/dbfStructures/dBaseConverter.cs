using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading.Tasks;

namespace dbfCoreDLL.dbfStructures
{
        /// <summary>
        /// Contains functions to convert dBase data types to .NET types and the other way around
        /// </summary>
        public static class dBaseConverter
        {
            /// <summary>
            /// Converts a logical byte ('L') to a boolean value
            /// </summary>
            /// <param name="dBaseByte">the logical byte from dBase</param>
            /// <returns>The boolean value</returns>
            public static bool L_ToBool(byte dBaseByte)
            {
                if (dBaseByte == 'Y' || dBaseByte == 'y' || dBaseByte == 'T' || dBaseByte == 't')
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            /// <summary>
            /// Converts a float byte array('F') to a double value
            /// </summary>
            /// <param name="dBaseByte">The float byte array from dBase</param>
            /// <returns>The double value</returns>
            public static double F_ToDouble(byte[] dBaseFloatBytes)
            {
                string uiSep = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                string NumberString = Encoding.ASCII.GetString(dBaseFloatBytes).Replace(".", uiSep).Trim();
                double ReturnDouble;
                if (double.TryParse(NumberString, out ReturnDouble))
                {
                    return ReturnDouble;
                }
                else
                {
                    return ReturnDouble = 0.0F;
                }
            }

            /// <summary>
            /// Converts a timestamp byte array('T') to a datetime value
            /// </summary>
            /// <param name="dBaseTimeBytes">The timestamp byte array from dBase(8 bytes long)</param>
            /// <returns>The datetime value</returns>
            public static DateTime T_ToDateTime(byte[] dBaseTimeBytes)
            {
                // Date is the number of days since 01/01/4713 BC (Julian Days)
                // Time is hours * 3600000L + minutes * 60000L + Seconds * 1000L (Milliseconds since midnight)

                long lDate = BitConverter.ToInt32(dBaseTimeBytes, 0);
                long lTime = BitConverter.ToInt32(dBaseTimeBytes, 4);
                return JulianToDateTime(lDate).AddTicks(lTime);
            }

            private static Encoding _defaultEncoding = System.Text.Encoding.Default;

            /// <summary>
            /// Converts a character byte array('C') to a string value
            /// </summary>
            /// <param name="dBaseCharacterBytes">The character byte array from dBase</param>
            /// <returns>A string value</returns>
            public static string C_ToString(byte[] dBaseCharacterBytes, Encoding encoding = null)
            {
                if (encoding == null)
                    encoding = _defaultEncoding;

                //return encoding.GetString(dBaseCharacterBytes).TrimEnd(new char[] { ' ' });
                return encoding.GetString(dBaseCharacterBytes).TrimEnd(new char[] { ' ', '\0' });
                //string s = encoding.GetString(dBaseCharacterBytes).TrimEnd(new char[] { ' ' });
                //if (replaceZeroChar) s = s.Replace("\0", string.Empty);
                //return s;
            }

            /// <summary>
            /// Converts a datetime byte array('D') to a DateTime value
            /// </summary>
            /// <param name="dBaseDateTimeBytes">The datetime byte array from dBase</param>
            /// <returns>A datetime value</returns>
            public static DateTime D_ToDateTime(byte[] dBaseDateTimeBytes)
            {
                DateTime ReturnDateTime = DateTime.MinValue;
                string DateTimeString = Encoding.ASCII.GetString(dBaseDateTimeBytes);
                string sYear = DateTimeString.Substring(0, 4);
                int iYear;
                string sMonth = DateTimeString.Substring(4, 2);
                int iMonth;
                string sDay = DateTimeString.Substring(6, 2);
                int iDay;
                if (Int32.TryParse(sYear, out iYear) && Int32.TryParse(sMonth, out iMonth) && Int32.TryParse(sDay, out iDay))
                {
                    if (iYear > 1900)
                    {
                        ReturnDateTime = new DateTime(iYear, iMonth, iDay);
                    }
                }

                return ReturnDateTime;
            }

            /// <summary>
            /// Differentiate between Integer or Decimal numbers comming from dBase
            /// </summary>
            /// <param name="dBaseNumberBytes">the number byte array from dBase</param>
            /// <returns>True if the submittet byte array is a decimal number</returns>
            public static bool N_IsDecimal(byte[] dBaseNumberBytes)
            {
                if (Encoding.ASCII.GetString(dBaseNumberBytes).Contains("."))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            /// <summary>
            /// Converts a number byte array('N') to a Decimal value
            /// </summary>
            /// <param name="dBaseNumberBytes">The number byte array from dBase</param>
            /// <returns>A Decimal value</returns>
            public static decimal N_ToDecimal(byte[] dBaseNumberBytes)
            {
                string uiSep = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                decimal ReturnDecimal;
                if (decimal.TryParse(Encoding.ASCII.GetString(dBaseNumberBytes).Replace(".", uiSep).Trim(), out ReturnDecimal))
                {
                    return ReturnDecimal;
                }
                else
                {
                    return 0;
                }
            }

            /// <summary>
            /// Converts a number byte array('N') to a Integer value
            /// </summary>
            /// <param name="dBaseNumberBytes">The number byte array from dBase</param>
            /// <returns>A Integer value</returns>
            public static int N_ToInt(byte[] dBaseNumberBytes)
            {
                int ReturnInt;
                if (int.TryParse(Encoding.ASCII.GetString(dBaseNumberBytes).Trim(), out ReturnInt))
                {
                    return ReturnInt;
                }
                else
                {
                    return 0;
                }
            }
            public static StringBuilder DateTime_ToD(StringBuilder dateTime)
            {
                DateTime date = new DateTime();
                StringBuilder result = new StringBuilder();
                if (DateTime.TryParse(dateTime.ToString(), out date))
                {
                    result.Append(date.ToString("yyyMMdd"));
                }
                return result;
            }
            /// <summary>
            /// Convert a Julian Date to a .NET DateTime structure
            /// Implemented from pseudo code at http://en.wikipedia.org/wiki/Julian_day
            /// </summary>
            /// <param name="lJDN">Julian Date to convert (days since 01/01/4713 BC)</param>
            /// <returns>DateTime</returns>
            private static DateTime JulianToDateTime(long lJDN)
            {
                double p = Convert.ToDouble(lJDN);
                double s1 = p + 68569;
                double n = Math.Floor(4 * s1 / 146097);
                double s2 = s1 - Math.Floor((146097 * n + 3) / 4);
                double i = Math.Floor(4000 * (s2 + 1) / 1461001);
                double s3 = s2 - Math.Floor(1461 * i / 4) + 31;
                double q = Math.Floor(80 * s3 / 2447);
                double d = s3 - Math.Floor(2447 * q / 80);
                double s4 = Math.Floor(q / 11);
                double m = q + 2 - 12 * s4;
                double j = 100 * (n - 49) + i + s4;
                return new DateTime(Convert.ToInt32(j), Convert.ToInt32(m), Convert.ToInt32(d));
            }
        }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HistoryTracking
{
    public static class Extensions
    {
        public static string ToJson(this object objectForSerialization)
        {
            if (objectForSerialization == null)
            {
                return string.Empty;
            }
            return JsonConvert.SerializeObject(objectForSerialization);
        }

        public static string ToFormattedString(this Exception exception)
        {
            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
            }
            return $"Exception message: {exception.Message}. {Environment.NewLine}" +
                   $"Stack-trace: {exception.StackTrace}.";
        }

        public static IEnumerable<DateTime> EnumerateTo(this DateTime startDate, DateTime endDate)
        {
            return Enumerable.Range(0, (endDate - startDate).Days + 1).Select(d => startDate.AddDays(d));
        }

        public static decimal ToRoundedRate(this decimal rate)
        {
            return Math.Round(rate, 8, MidpointRounding.AwayFromZero);
        }

        public static bool IsNullOrEmpty(this string st)
        {
            return String.IsNullOrEmpty(st);
        }

        public static string ToLowerFirstChar(this string st)
        {
            if (st.IsNullOrEmpty())
                return st;

            return st.First().ToString().ToLower() + st.Substring(1);
        }

        public static string TrimByLength(this string st, int maxLength)
        {
            if (st.Length <= maxLength)
                return st;

            return st.Substring(0, maxLength);
        }

        public static TEnum AsEnum<TEnum>(this string value) where TEnum : struct, IConvertible
        {
            return EnumHelper.Parse<TEnum>(value);
        }
    }
}

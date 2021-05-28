using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoryTracking
{
    public static class EnumHelper
    {
        public static List<T> ToList<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }

        public static T[] ToArray<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToArray();
        }

        public static string GetDescription<TEnum>(TEnum value) where TEnum : struct, IConvertible
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            return value.ToString();
        }

        private static T GetEnumItemByDescription<T>(string description) where T : struct, IConvertible
        {
            var enumItems = ToList<T>().ToDictionary(x => x, x => GetDescription(x));
            return (T)Enum.ToObject(typeof(T), enumItems.First(item => item.Value.ToUpper() == description.Trim().ToUpper()).Key);
        }

        public static T Parse<T>(string value) where T : struct, IConvertible
        {
            try
            {
                return (T)Enum.Parse(typeof(T), value.Trim(), true);
            }
            catch (Exception)
            {
                // ignored
            }

            return GetEnumItemByDescription<T>(value);
        }

        public static T? TryParse<T>(string value) where T : struct, IConvertible
        {
            try
            {
                return Parse<T>(value);
            }
            catch (Exception)
            {
                // ignored
            }

            return null;
        }

        public static Dictionary<string, string> ToDictionary<T>() where  T : struct, IConvertible
        {
            var result = new Dictionary<string, string>();
            foreach (var item in EnumHelper.ToList<T>().OrderBy(x=>x))
            {
                result.Add(item.ToString(), GetDescription(item));
            }

            return result;
        }
    }
}

using Mtf.Extensions.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Mtf.Extensions
{
    public static class EnumExtensions
    {
        public static IEnumerable<object> GetEnumAttribute<TAttribute>(this Enum value, string propertyName)
        {
            var type = typeof(TAttribute);
            var field = value.GetType().GetField(value.ToString());
            if (field == null)
            {
                return Enumerable.Empty<object>();
            }

            var attributes = field.GetCustomAttributes(type, false) as TAttribute[];
            return attributes.Select(attribute => type.GetProperty(propertyName).GetValue(attribute));
        }

        public static object GetSingleEnumAttribute<TAttribute>(this Enum value, string propertyName)
        {
            var attributes = value.GetEnumAttribute<TAttribute>(propertyName);
            if (attributes.Any())
            {
                return attributes.Single();
            }

            return null;
        }

        public static IEnumerable<T> GetEnabledValues<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T))
                .Cast<T>()
                .Where(value => value.GetType()
                    .GetField(value.ToString())
                    .GetCustomAttributes(typeof(DisabledAttribute), false)
                    .Length == 0);
        }

        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            if (field == null)
            {
                return value.ToString();
            }

            var attribute = field.GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Description ?? value.ToString();
        }

        public static string GetDescription<TValueType>(this TValueType value)
            where TValueType : Enum
        {
            var valueStr = value.ToString();
            var type = value.GetType();
            var memberInfo = type.GetMember(valueStr);

            if (memberInfo == null || memberInfo.Length < 1)
            {
                return value.ToString();
            }

            var descriptionAttribute = (DescriptionAttribute[])memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            return descriptionAttribute.FirstOrDefault()?.Description ?? valueStr;
        }

        public static TEnum GetFromDescription<TEnum>(string formattedString) where TEnum : Enum
        {
            if (formattedString == null)
            {
                throw new ArgumentNullException(nameof(formattedString));
            }

            var name = formattedString.Split('(')[0].Trim();
            if (Enum.IsDefined(typeof(TEnum), name))
            {
                return (TEnum)Enum.Parse(typeof(TEnum), name);
            }

            throw new ArgumentException($"No enum value found for '{name}' in {typeof(TEnum).Name}");
        }

        public static T GetValueFromDescription<T>(string description) where T : Enum
        {
            var type = typeof(T);

            foreach (var field in type.GetFields())
            {
                var attribute = field.GetCustomAttribute<DescriptionAttribute>();
                if (field != null && attribute != null && attribute.Description == description)
                {
                    return (T)field.GetValue(null);
                }
            }

            throw new ArgumentException($"No enum value found for description '{description}'", nameof(description));
        }

        public static string GetComplexDescription<TValueType>(this TValueType value)
            where TValueType : Enum
        {
            if (value.Equals(Enum.ToObject(typeof(TValueType), 0)))
            {
                return value.GetDescription();
            }

            var descriptions = Enum.GetValues(typeof(TValueType))
                                   .Cast<TValueType>()
                                   .Where(v => value.HasFlag(v) && !v.Equals(Enum.ToObject(typeof(TValueType), 0)))
                                   .Select(v => v.GetDescription())
                                   .ToList();

            return String.Join(", ", descriptions);
        }

        public static List<object> GetIndividualValues(Type enumType)
        {
            return Enum.GetValues(enumType)
                       .Cast<object>()
                       .Where(value => IsPowerOfTwo(Convert.ToInt64(value)))
                       .ToList();
        }

        private static bool IsPowerOfTwo(long value) => value != 0 && (value & (value - 1)) == 0;

        public static bool HasAnyFlag(this Enum value, params Enum[] flags)
        {
            return value == null ? throw new ArgumentNullException(nameof(value)) : flags.Any(value.HasFlag);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace NuGet.CommandLine
{
    internal static class TypeHelper
    {
        public static Type RemoveNullableFromType(Type type)
        {
            return Nullable.GetUnderlyingType(type) ?? type;
        }

        public static object ChangeType(object value, Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (value == null)
            {
                if (TypeAllowsNull(type))
                {
                    return null;
                }
                return Convert.ChangeType(value, type, CultureInfo.CurrentCulture);
            }

            type = RemoveNullableFromType(type);

            if (value.GetType() == type)
            {
                return value;
            }

            TypeConverter converter = TypeDescriptor.GetConverter(type);
            if (converter.CanConvertFrom(value.GetType()))
            {
                return converter.ConvertFrom(value);
            }

            TypeConverter otherConverter = TypeDescriptor.GetConverter(value.GetType());
            if (otherConverter.CanConvertTo(type))
            {
                return otherConverter.ConvertTo(value, type);
            }

            throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture,
                LocalizedResourceManager.GetString("UnableToConvertTypeError"), value.GetType(), type));
        }

        public static bool TypeAllowsNull(Type type)
        {
            return Nullable.GetUnderlyingType(type) != null || !type.IsValueType;
        }

        public static Type GetGenericCollectionType(Type type)
        {
            return GetInterfaceType(type, typeof(ICollection<>));
        }

        public static Type GetDictionaryType(Type type)
        {
            return GetInterfaceType(type, typeof(IDictionary<,>));
        }

        private static Type GetInterfaceType(Type type, Type interfaceType)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == interfaceType)
            {
                return type;
            }
            return (from t in type.GetInterfaces()
                    where t.IsGenericType && t.GetGenericTypeDefinition() == interfaceType
                    select t).SingleOrDefault();
        }

        public static bool IsKeyValueProperty(PropertyInfo property)
        {
            return GetDictionaryType(property.PropertyType) != null;
        }

        public static bool IsMultiValuedProperty(PropertyInfo property)
        {
            return GetGenericCollectionType(property.PropertyType) != null || IsKeyValueProperty(property);
        }

        public static bool IsEnumProperty(PropertyInfo property)
        {
            return property.PropertyType.IsEnum;
        }
    }
}

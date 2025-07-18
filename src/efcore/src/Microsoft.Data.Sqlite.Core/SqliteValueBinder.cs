// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Data.Sqlite.Properties;

namespace Microsoft.Data.Sqlite
{
    // TODO: Make generic
    internal abstract class SqliteValueBinder(object? value, SqliteType? sqliteType)
    {
        private static readonly bool Pre10TimeZoneHandling =
            AppContext.TryGetSwitch("Microsoft.Data.Sqlite.Pre10TimeZoneHandling", out var enabled) && enabled;

        protected SqliteValueBinder(object? value)
            : this(value, null)
        {
        }

        protected abstract void BindInt64(long value);

        protected virtual void BindDouble(double value)
        {
            if (double.IsNaN(value))
            {
                throw new InvalidOperationException(Resources.CannotStoreNaN);
            }

            BindDoubleCore(value);
        }

        protected abstract void BindDoubleCore(double value);

        protected abstract void BindText(string value);

        protected abstract void BindBlob(byte[] value);

        protected abstract void BindNull();

        public virtual void Bind()
        {
            if (value == null)
            {
                BindNull();

                return;
            }

            var type = value.GetType().UnwrapNullableType().UnwrapEnumType();
            if (type == typeof(bool))
            {
                var value1 = (bool)value ? 1L : 0;
                BindInt64(value1);
            }
            else if (type == typeof(byte))
            {
                var value1 = (long)(byte)value;
                BindInt64(value1);
            }
            else if (type == typeof(byte[]))
            {
                var value1 = (byte[])value;
                BindBlob(value1);
            }
            else if (type == typeof(char))
            {
                var chr = (char)value;
                if (sqliteType != SqliteType.Integer)
                {
                    var value = new string(chr, 1);
                    BindText(value);
                }
                else
                {
                    var value = (long)chr;
                    BindInt64(value);
                }
            }
            else if (type == typeof(DateTime))
            {
                var dateTime = (DateTime)value;
                if (sqliteType == SqliteType.Real)
                {
                    var value = ToJulianDate(dateTime);
                    BindDouble(value);
                }
                else
                {
                    var value = dateTime.ToString(@"yyyy\-MM\-dd HH\:mm\:ss.FFFFFFF", CultureInfo.InvariantCulture);
                    BindText(value);
                }
            }
            else if (type == typeof(DateTimeOffset))
            {
                var dateTimeOffset = (DateTimeOffset)value;
                if (sqliteType == SqliteType.Real)
                {
                    // Before .NET 10, handling DateTimeOffset ignored offset and wrote incorrect value into database.
                    var value = Pre10TimeZoneHandling
                        ? ToJulianDate(dateTimeOffset.DateTime)
                        : ToJulianDate(dateTimeOffset.ToUniversalTime().DateTime);
                    BindDouble(value);
                }
                else
                {
                    var value = dateTimeOffset.ToString(@"yyyy\-MM\-dd HH\:mm\:ss.FFFFFFFzzz", CultureInfo.InvariantCulture);
                    BindText(value);
                }
            }
#if NET6_0_OR_GREATER
            else if (type == typeof(DateOnly))
            {
                var dateOnly = (DateOnly)value;
                if (sqliteType == SqliteType.Real)
                {
                    var value = ToJulianDate(dateOnly.Year, dateOnly.Month, dateOnly.Day, 0, 0, 0, 0);
                    BindDouble(value);
                }
                else
                {
                    var value = dateOnly.ToString(@"yyyy\-MM\-dd", CultureInfo.InvariantCulture);
                    BindText(value);
                }
            }
            else if (type == typeof(TimeOnly))
            {
                var timeOnly = (TimeOnly)value;
                if (sqliteType == SqliteType.Real)
                {
                    var value = GetTotalDays(timeOnly.Hour, timeOnly.Minute, timeOnly.Second, timeOnly.Millisecond);
                    BindDouble(value);
                }
                else
                {
                    var value = timeOnly.Ticks % 10000000 == 0
                        ? timeOnly.ToString(@"HH:mm:ss", CultureInfo.InvariantCulture)
                        : timeOnly.ToString(@"HH:mm:ss.fffffff", CultureInfo.InvariantCulture);
                    BindText(value);
                }
            }
#endif
            else if (type == typeof(DBNull))
            {
                BindNull();
            }
            else if (type == typeof(decimal))
            {
                var value1 = ((decimal)value).ToString("0.0###########################", CultureInfo.InvariantCulture);
                BindText(value1);
            }
            else if (type == typeof(double))
            {
                var value1 = (double)value;
                BindDouble(value1);
            }
            else if (type == typeof(float))
            {
                var value1 = (double)(float)value;
                BindDouble(value1);
            }
            else if (type == typeof(Guid))
            {
                var guid = (Guid)value;
                if (sqliteType != SqliteType.Blob)
                {
                    var value = guid.ToString().ToUpperInvariant();
                    BindText(value);
                }
                else
                {
                    var value = guid.ToByteArray();
                    BindBlob(value);
                }
            }
            else if (type == typeof(int))
            {
                var value1 = (long)(int)value;
                BindInt64(value1);
            }
            else if (type == typeof(long))
            {
                var value1 = (long)value;
                BindInt64(value1);
            }
            else if (type == typeof(sbyte))
            {
                var value1 = (long)(sbyte)value;
                BindInt64(value1);
            }
            else if (type == typeof(short))
            {
                var value1 = (long)(short)value;
                BindInt64(value1);
            }
            else if (type == typeof(string))
            {
                var value1 = (string)value;
                BindText(value1);
            }
            else if (type == typeof(TimeSpan))
            {
                var timeSpan = (TimeSpan)value;
                if (sqliteType == SqliteType.Real)
                {
                    var value = timeSpan.TotalDays;
                    BindDouble(value);
                }
                else
                {
                    var value = timeSpan.ToString("c");
                    BindText(value);
                }
            }
            else if (type == typeof(uint))
            {
                var value1 = (long)(uint)value;
                BindInt64(value1);
            }
            else if (type == typeof(ulong))
            {
                var value1 = (long)(ulong)value;
                BindInt64(value1);
            }
            else if (type == typeof(ushort))
            {
                var value1 = (long)(ushort)value;
                BindInt64(value1);
            }
            else
            {
                throw new InvalidOperationException(Resources.UnknownDataType(type));
            }
        }

        private static readonly Dictionary<Type, SqliteType> _sqliteTypeMapping =
            new()
            {
                { typeof(bool), SqliteType.Integer },
                { typeof(byte), SqliteType.Integer },
                { typeof(byte[]), SqliteType.Blob },
                { typeof(char), SqliteType.Text },
                { typeof(DateTime), SqliteType.Text },
                { typeof(DateTimeOffset), SqliteType.Text },
#if NET6_0_OR_GREATER
                { typeof(DateOnly), SqliteType.Text },
                { typeof(TimeOnly), SqliteType.Text },
#endif
                { typeof(DBNull), SqliteType.Text },
                { typeof(decimal), SqliteType.Text },
                { typeof(double), SqliteType.Real },
                { typeof(float), SqliteType.Real },
                { typeof(Guid), SqliteType.Text },
                { typeof(int), SqliteType.Integer },
                { typeof(long), SqliteType.Integer },
                { typeof(sbyte), SqliteType.Integer },
                { typeof(short), SqliteType.Integer },
                { typeof(string), SqliteType.Text },
                { typeof(TimeSpan), SqliteType.Text },
                { typeof(uint), SqliteType.Integer },
                { typeof(ulong), SqliteType.Integer },
                { typeof(ushort), SqliteType.Integer }
            };

        internal static SqliteType GetSqliteType(object? value)
        {
            if (value == null)
            {
                return SqliteType.Text;
            }

            var type = value.GetType().UnwrapNullableType().UnwrapEnumType();
            if (_sqliteTypeMapping.TryGetValue(type, out var sqliteType))
            {
                return sqliteType;
            }

            throw new InvalidOperationException(Resources.UnknownDataType(type));
        }

        private static double ToJulianDate(DateTime dateTime)
            => ToJulianDate(
                dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond);

        private static double ToJulianDate(int year, int month, int day, int hour, int minute, int second, int millisecond)
        {
            // computeJD
            var Y = year;
            var M = month;
            var D = day;

            if (M <= 2)
            {
                Y--;
                M += 12;
            }

            var A = Y / 100;
            var B = 2 - A + (A / 4);
            var X1 = 36525 * (Y + 4716) / 100;
            var X2 = 306001 * (M + 1) / 10000;
            var iJD = (long)((X1 + X2 + D + B - 1524.5) * 86400000);

            iJD += hour * 3600000 + minute * 60000 + (long)((second + millisecond / 1000.0) * 1000);

            return iJD / 86400000.0;
        }

        private static double GetTotalDays(int hour, int minute, int second, int millisecond)
        {
            var iJD = hour * 3600000 + minute * 60000 + (long)((second + millisecond / 1000.0) * 1000);

            return iJD / 86400000.0;
        }
    }
}

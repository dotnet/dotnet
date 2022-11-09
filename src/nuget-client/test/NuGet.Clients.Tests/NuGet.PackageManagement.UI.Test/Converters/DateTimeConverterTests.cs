// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using Xunit;

namespace NuGet.PackageManagement.UI.Test.Converters
{
    public class DateTimeConverterTests
    {
        [Fact]
        public void Convert_NullValue()
        {
            var converter = new DateTimeConverter();

            var converted = converter.Convert(
                null,
                typeof(string),
                null,
                new CultureInfo("en-US"));

            Assert.Null(converted);
        }

        // Only test languages that have a Visual Studio language pack
        [Theory]
        [InlineData("cs-CZ", "úterý 20. listopadu 2018 (20.11.2018)")]
        [InlineData("de-DE", "Dienstag, 20. November 2018 (20.11.2018)")]
        [InlineData("en-US", "Tuesday, November 20, 2018 (11/20/2018)")]
        [InlineData("es-ES", "martes, 20 de noviembre de 2018 (20/11/2018)")]
        [InlineData("fr-FR", "mardi 20 novembre 2018 (20/11/2018)")]
        [InlineData("it-IT", "martedì 20 novembre 2018 (20/11/2018)")]
        [InlineData("ja-JP", "2018年11月20日 火曜日 (2018/11/20)")]
        [InlineData("ko-KR", "2018년 11월 20일 화요일 (2018-11-20)")]
        [InlineData("pl-PL", "wtorek, 20 listopada 2018 (20.11.2018)")]
        [InlineData("pt-BR", "terça-feira, 20 de novembro de 2018 (20/11/2018)")]
        [InlineData("ru-RU", "20 ноября 2018 г. (20.11.2018)")]
        [InlineData("tr-TR", "20 Kasım 2018 Salı (20.11.2018)")]
        [InlineData("zh-CN", "2018年11月20日 (2018/11/20)")]
        [InlineData("zh-TW", "2018年11月20日 (2018/11/20)")]
        public void Convert_WithConverterParameter_UsesVersionFormatter(string locale, string expected)
        {
            var culture = CultureInfo.GetCultureInfoByIetfLanguageTag(locale);
            var value = new DateTimeOffset(2018, 11, 20, 13, 44, 31, TimeSpan.FromHours(-8));

            var converter = new DateTimeConverter();

            var converted = converter.Convert(
                value,
                typeof(string),
                null,
                culture);

            Assert.Equal(expected, converted);
        }

        [Fact]
        public void Convert_DateTime()
        {
            var converter = new DateTimeConverter();

            var converted = converter.Convert(new DateTime(2018, 11, 20),
                typeof(string),
                null,
                CultureInfo.GetCultureInfo("en-US"));

            Assert.Equal("Tuesday, November 20, 2018 (11/20/2018)", converted);
        }

        [Fact]
        public void Convert_DateTimeParsableObject()
        {
            var converter = new DateTimeConverter();

            var converted = converter.Convert("2018-11-20",
                typeof(string),
                null,
                CultureInfo.GetCultureInfo("en-US"));

            Assert.Equal("Tuesday, November 20, 2018 (11/20/2018)", converted);
        }

        [Fact]
        public void Convert_ObjectDateTimeCanNotParse()
        {
            var converter = new DateTimeConverter();

            var converted = converter.Convert(Guid.Empty,
                typeof(string),
                null,
                CultureInfo.GetCultureInfo("en-US"));

            Assert.Null(converted);
        }

    }
}

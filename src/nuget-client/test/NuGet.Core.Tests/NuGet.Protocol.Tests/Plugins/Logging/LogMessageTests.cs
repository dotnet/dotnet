// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public abstract class LogMessageTests
    {
        private static readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings()
        {
            DateParseHandling = DateParseHandling.None
        };

        internal JObject VerifyOuterMessageAndReturnInnerMessage(IPluginLogMessage logMessage, DateTimeOffset expectedNow, string expectedType)
        {
            var json = logMessage.ToString();

            var actualResult = JsonConvert.DeserializeObject<JObject>(json, _jsonSettings);

            Assert.Equal(3, actualResult.Count);

            VerifyNow(expectedNow, actualResult);
            VerifyType(expectedType, actualResult);

            return VerifyAndReturnMessage(actualResult);
        }

        internal JObject VerifyOuterMessageAndReturnInnerMessage(string json, DateTimeOffset expectedNowStart, DateTimeOffset expectedNowEnd, string expectedType)
        {
            var actualResult = JsonConvert.DeserializeObject<JObject>(json, _jsonSettings);

            Assert.Equal(3, actualResult.Count);

            VerifyNow(expectedNowStart, expectedNowEnd, actualResult);
            VerifyType(expectedType, actualResult);

            return VerifyAndReturnMessage(actualResult);
        }

        private static DateTime ParseDateTime(string value)
        {
            return DateTime.Parse(value, provider: null, styles: DateTimeStyles.RoundtripKind);
        }

        private static JObject VerifyAndReturnMessage(JObject actualResult)
        {
            var message = actualResult.Value<JObject>("message");

            Assert.NotNull(message);

            return message;
        }

        private static void VerifyNow(DateTimeOffset expectedNow, JObject actualResult)
        {
            var value = actualResult.Value<string>("now");
            var actualNow = ParseDateTime(value);

            Assert.Equal(DateTimeKind.Utc, actualNow.Kind);
            Assert.Equal(expectedNow.Ticks, actualNow.Ticks);
        }

        private static void VerifyNow(DateTimeOffset expectedNowStart, DateTimeOffset expectedNowEnd, JObject actualResult)
        {
            var value = actualResult.Value<string>("now");
            var actualNow = ParseDateTime(value);

            Assert.Equal(DateTimeKind.Utc, actualNow.Kind);
            Assert.InRange(actualNow, expectedNowStart, expectedNowEnd);
        }

        private static void VerifyType(string expectedType, JObject actualResult)
        {
            var actualType = actualResult.Value<string>("type");

            Assert.Equal(expectedType, actualType);
        }
    }
}

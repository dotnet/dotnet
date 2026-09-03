// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Newtonsoft.Json;
using Xunit;

namespace NuGet.Credentials.Test
{
    public class PluginCredentialResponseDeserializationTests
    {
        private static PluginCredentialResponse? DeserializeWithStj(string json)
        {
            return System.Text.Json.JsonSerializer.Deserialize(json, PluginCredentialResponseJsonContext.Default.PluginCredentialResponse);
        }

        private static PluginCredentialResponse? DeserializeWithNsj(string json)
        {
            return JsonConvert.DeserializeObject<PluginCredentialResponse>(json);
        }

        private static void AssertResponsesEqual(PluginCredentialResponse? nsj, PluginCredentialResponse? stj)
        {
            Assert.Equal(nsj?.Username, stj?.Username);
            Assert.Equal(nsj?.Password, stj?.Password);
            Assert.Equal(nsj?.Message, stj?.Message);
            Assert.Equal(nsj?.AuthTypes, stj?.AuthTypes);
            Assert.Equal(nsj?.IsValid, stj?.IsValid);
        }

        [Theory]
        [InlineData(@"{""Username"":""user"",""Password"":""pass"",""Message"":""msg"",""AuthTypes"":[""basic"",""digest""]}")]
        [InlineData(@"{""username"":""u"",""password"":""p"",""message"":""m"",""authTypes"":[""basic""]}")]
        [InlineData(@"{""USERNAME"":""u"",""PassWord"":""p""}")]
        [InlineData(@"{""Username"":""u""}")]
        [InlineData(@"{}")]
        [InlineData(@"{""Username"":null,""Password"":null,""AuthTypes"":null}")]
        [InlineData(@"{""Username"":""u"",""Password"":""p"",""AuthTypes"":[]}")]
        [InlineData(@"{""Username"":""u"",""Password"":""p"",""ExtraProp"":""val"",""Another"":123}")]
        [InlineData(@"{""Username"":"""",""Password"":"""",""Message"":""""}")]
        [InlineData(@"{""Username"":"" user "",""Password"":"" pass ""}")]
        [InlineData(@"{""Username"":""\u0041\u0042"",""Password"":""\u00e9""}")]
        [InlineData(@"{""Username"":""user"",""Password"":""p@$$w0rd!#%&*""}")]
        [InlineData(@"{""Username"":""u"",""Password"":""p"",""Message"":""""}")]
        [InlineData(@"   {""Username"":""u"",""Password"":""p""}   ")]
        [InlineData(@"{""Message"":""Extra message.""}")]
        [InlineData(@"{""username"":""u"", ""password"":""p"", ""Message"":""""}")]
        [InlineData(@"{""Username"":""u"",""Password"":""p"",""AuthTypes"":[""basic"",""digest"",""negotiate"",""ntlm""]}")]
        public void StjAndNsjProduceSameResult(string json)
        {
            PluginCredentialResponse? nsjResult = DeserializeWithNsj(json);
            PluginCredentialResponse? stjResult = DeserializeWithStj(json);

            AssertResponsesEqual(nsjResult, stjResult);
        }

        [Theory]
        [InlineData("null")]
        public void StjAndNsjBothReturnNull(string json)
        {
            PluginCredentialResponse? nsjResult = DeserializeWithNsj(json);
            PluginCredentialResponse? stjResult = DeserializeWithStj(json);

            Assert.Null(nsjResult);
            Assert.Null(stjResult);
        }

        [Theory]
        [InlineData("not json")]
        [InlineData("{invalid}")]
        public void StjAndNsjBothThrowOnInvalidJson(string json)
        {
            Assert.ThrowsAny<JsonReaderException>(() => DeserializeWithNsj(json));
            Assert.ThrowsAny<System.Text.Json.JsonException>(() => DeserializeWithStj(json));
        }

        [Fact]
        public void StjAndNsjBothThrowOnJsonArrayInput()
        {
            var json = "[1,2,3]";

            Assert.ThrowsAny<JsonSerializationException>(() => DeserializeWithNsj(json));
            Assert.ThrowsAny<System.Text.Json.JsonException>(() => DeserializeWithStj(json));
        }
    }
}

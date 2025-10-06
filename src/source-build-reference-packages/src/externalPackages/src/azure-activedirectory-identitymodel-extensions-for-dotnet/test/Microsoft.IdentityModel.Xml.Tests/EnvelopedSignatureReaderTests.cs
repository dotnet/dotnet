// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens.Saml;
using Microsoft.IdentityModel.Tokens.Saml2;
using Microsoft.IdentityModel.Xml;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Tokens.Xml.Tests
{
    public class EnvelopedSignatureReaderTests
    {
        [Fact]
        public void GetSets()
        {
            var type = typeof(EnvelopedSignatureReader);
            var properties = type.GetProperties();
            Assert.True(properties.Length == 34, $"Number of properties has changed from 34 to: {properties.Length}, adjust tests");

            var reader = XmlUtilities.CreateEnvelopedSignatureReader(Default.OuterXml);
            var defaultSerializer = reader.Serializer;
            var context = new GetSetContext
            {
                PropertyNamesAndSetGetValue = new List<KeyValuePair<string, List<object>>>
                {
                    new KeyValuePair<string, List<object>>("Serializer", new List<object>{ defaultSerializer, DSigSerializer.Default}),
                },
                Object = reader,
            };

            TestUtilities.GetSet(context);
            TestUtilities.AssertFailIfErrors($"{this}.GetSets", context.Errors);
        }

        [Theory, MemberData(nameof(ConstructorTheoryData))]
        public void Constructor(EnvelopedSignatureTheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.Constructor", theoryData);
            try
            {
                var envelopedReader = new EnvelopedSignatureReader(theoryData.XmlReader);
                while (envelopedReader.Read()) ;

                if (theoryData.ExpectSignature)
                {
                    if (envelopedReader.Signature == null)
                        Assert.False(true, "theoryData.ExpectSignature == true && envelopedReader.ExpectSignature == null");

                    envelopedReader.Signature.Verify(theoryData.SecurityKey, theoryData.SecurityKey.CryptoProviderFactory);
                }

                theoryData.ExpectedException.ProcessNoException();
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex);
            }
        }

        public static TheoryData<EnvelopedSignatureTheoryData> ConstructorTheoryData
        {
            get
            {
                var theoryData = new TheoryData<EnvelopedSignatureTheoryData>();

                theoryData.Add(new EnvelopedSignatureTheoryData
                {
                    ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                    First = true,
                    TestId = "Null XmlReader",
                    XmlReader = null
                });

                return theoryData;
            }
        }

        [Theory, MemberData(nameof(ReadSignedXmlTheoryData))]
        public void ReadSignedXml(EnvelopedSignatureTheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.ReadSignedXml", theoryData);
            var context = new CompareContext($"{this}.ReadSignedXml : {theoryData.TestId}.");
            try
            {
                var envelopedReader = XmlUtilities.CreateEnvelopedSignatureReader(theoryData.Xml);
                while (envelopedReader.Read());

                if (theoryData.ExpectSignature)
                {
                    if (envelopedReader.Signature == null)
                        Assert.False(true, "theoryData.ExpectSignature == true && envelopedReader.Signature == null");

                    envelopedReader.Signature.Verify(theoryData.SecurityKey, theoryData.CryptoProviderFactory);
                }

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<EnvelopedSignatureTheoryData> ReadSignedXmlTheoryData
        {
            get
            {
                return new TheoryData<EnvelopedSignatureTheoryData>
                {
                    new EnvelopedSignatureTheoryData
                    {
                        ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                        First = true,
                        TestId = nameof(ReferenceXml.Saml2TokenValidSigned) + ":SecurityKey==null",
                        Xml = ReferenceXml.Saml2TokenValidSigned
                    },
                    new EnvelopedSignatureTheoryData
                    {
                        ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                        SecurityKey = KeyingMaterial.DefaultAADSigningKey,
                        CryptoProviderFactory = null,
                        TestId = nameof(ReferenceXml.Saml2TokenValidSigned) + ":CryptoProviderFactory==null",
                        Xml = ReferenceXml.Saml2TokenValidSigned
                    },
                    new EnvelopedSignatureTheoryData
                    {
                        SecurityKey = KeyingMaterial.DefaultAADSigningKey,
                        TestId = nameof(ReferenceXml.Saml2TokenValidSigned),
                        Xml = ReferenceXml.Saml2TokenValidSigned
                    },
                    new EnvelopedSignatureTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(XmlReadException), "IDX30019:"),
                        SecurityKey = KeyingMaterial.DefaultAADSigningKey,
                        TestId = nameof(ReferenceXml.Saml2TokenTwoSignatures),
                        Xml = ReferenceXml.Saml2TokenTwoSignatures
                    },
                    new EnvelopedSignatureTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(XmlValidationException)),
                        SecurityKey = KeyingMaterial.DefaultAADSigningKey,
                        TestId = nameof(ReferenceXml.Saml2TokenValidSignatureNOTFormated),
                        Xml = ReferenceXml.Saml2TokenValidSignatureNOTFormated
                    },
                    new EnvelopedSignatureTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(XmlValidationException)),
                        SecurityKey = KeyingMaterial.DefaultAADSigningKey,
                        TestId = nameof(ReferenceXml.Saml2TokenValidFormated),
                        Xml = ReferenceXml.Saml2TokenValidFormated
                    }
                };
            }
        }

        [Theory, MemberData(nameof(ReadXmlElementsTheoryData))]
        public void ReadXmlElements(EnvelopedSignatureTheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.ReadXmlElements", theoryData);
            var context = new CompareContext($"{this}.ReadXmlElements : {theoryData.TestId}.");
            try
            {
                XmlReader reader = XmlUtilities.CreateDictionaryReader(theoryData.Xml);

                EnvelopedSignatureReader envelopedReader;
                if (theoryData.XmlElementReader == null)
                    envelopedReader = new EnvelopedSignatureReader(reader);
                else
                    envelopedReader = new EnvelopedSignatureReader(reader, theoryData.XmlElementReader);

                while (envelopedReader.Read());

                if (theoryData.XmlElementReader != null)
                {
                    foreach (var item in theoryData.XmlElementReader.Items)
                    {
                        if (item is SamlSecurityToken samlToken)
                            samlToken.Assertion.Signature.Verify(theoryData.TokenSecurityKey);

                        if (item is Saml2SecurityToken saml2Token)
                            saml2Token.Assertion.Signature.Verify(theoryData.TokenSecurityKey);
                    }
                }

                if (envelopedReader.Signature != null)
                {
                    envelopedReader.Signature.Verify(theoryData.SecurityKey, theoryData.SecurityKey.CryptoProviderFactory);
                }
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<EnvelopedSignatureTheoryData> ReadXmlElementsTheoryData
        {
            get
            {
                var samlString = CreateSamlTokenString();
                var saml2String = CreateSaml2TokenString();
                var samlpMesage = File.ReadAllText("SamlpMessage.xml");
                var samlpTokenKey = new X509SecurityKey(new X509Certificate2(Convert.FromBase64String("MIIGvzCCBKegAwIBAgICAZUwDQYJKoZIhvcNAQELBQAwgYYxCzAJBgNVBAYTAkNaMQ8wDQYDVQQHEwZQcmFndWUxGTAXBgNVBAoTEENaLk5JQywgei5zLnAuby4xMTAvBgNVBAMTKENaLk5JQyBTSEEyIFJvb3QgQ2VydGlmaWNhdGlvbiBBdXRob3JpdHkxGDAWBgkqhkiG9w0BCQEWCWNhQG5pYy5jejAeFw0xODAyMDYxMDM5MDJaFw0yMDAyMDYxMDM5MDJaMH0xCzAJBgNVBAYTAkNaMQ8wDQYDVQQHEwZQcmFndWUxDzANBgNVBAoTBkNaLk5JQzEwMC4GA1UEAxMnbW9qZWlkLnJlZ3Rlc3QubmljLmN6IHNpZGFzYW1sIG1ldGFkYXRhMRowGAYJKoZIhvcNAQkBFgtyb290QG5pYy5jejCCAiIwDQYJKoZIhvcNAQEBBQADggIPADCCAgoCggIBANUq8haz+ZNnLBWRLPLQKSyg7TlQDVyLxxMmxFhCT8OXArkjIsZvcwNNeMDB6aOhk0Zs97t+7lEgC2MuiY9GnoC0DV+TAUf7+MzlHuE0oCmR9jiJPsikBEsRDHpvHi9rZv4HK2TCsee/5dDi7tP7bMnRvDOAt7lR+KuLQNaXtDWrXC4bjeNdO/mcy3UeKy+dW2Diqz8YMbRrxM29wAweaUSJ6npU9KTnx0/dq/+IM4R1gO62t+6vjxqiryEFcvdb6lGHc6qC9TYuHaGZBfXiT2goK4NbOr9dfLuixQ8Jd3oN88Qqt7r5u20VLCB06BIQHBgzJTHaSsi5MT5ymtx8lTpkR6MHXGue//QTZPi5DVBonb1B+ilgWdG7jK5yTBA6BkQSbbFp4uHM3IWdExErV/FPeyN9T7Au0kf7Jp73m7gMjD6ytC9xSI082ELufkjmerLTB0SoNPEsfAzUgDQeJ4DhsWg9kiK1/nhakjENefVW3FA2rZYBsRZkZr/uGdg/XEnw34ooeh+sTsj4QF5nWeuGmq0nu08hSTLv6YYfGwJny0TNekmfNoNL7Ip1RoRenl2ayruqvMSEzh4z5D1m4hW6zwmsRj0X8FJOk9pOr0NbHVsr2RuefmKNntk2bXMYq8dO3xSRASmdVmgmoyoQGJrnh1E0SihxCFNgiHqyGQ1JAgMBAAGjggE9MIIBOTAJBgNVHRMEAjAAMBEGCWCGSAGG+EIBAQQEAwIEsDAdBgNVHQ4EFgQU0HAvIqGfYUW/akxHd9SNK2O4GwEwgbsGA1UdIwSBszCBsIAUzxJQXpMfXwleF48WV0F6n3lyhLihgYykgYkwgYYxCzAJBgNVBAYTAkNaMQ8wDQYDVQQHEwZQcmFndWUxGTAXBgNVBAoTEENaLk5JQywgei5zLnAuby4xMTAvBgNVBAMTKENaLk5JQyBTSEEyIFJvb3QgQ2VydGlmaWNhdGlvbiBBdXRob3JpdHkxGDAWBgkqhkiG9w0BCQEWCWNhQG5pYy5jeoIJAJxtRGsvNinfMBQGA1UdEgQNMAuBCWNhQG5pYy5jejAWBgNVHREEDzANgQtyb290QG5pYy5jejAOBgNVHQ8BAf8EBAMCBaAwDQYJKoZIhvcNAQELBQADggIBAIytUqXh7AU/OunkSpUTaEY5Ze2sdV76JPwYVzNn2O6hCyzGvPXARP0IchUv8Vy+eCFLctUxvCbUS6aC+ObKgGXq4MxoSV8lijMLEW9crCpFDoLLd3LQw0GjVk2mCE7XTuaT0choPYlZmjv+wF2ZKm5/B+Qjek2j7SkY1yn7hxgJdd5ljHE6wmDXLJ8gHuVBNwvc5iHDjHYh7jL5c5jCDBcr1fFCsIARU05RAkfpWurl8GKY8t6IPm7iopOLjru3Gl45ZBdVrAPMQ8Fz1M9VElUJ6ngeKkXHkSwGhCBG3X0MYsltND6mZSkJqN0nOs+cJ6HHO8IZW1f2pH3aCTUSDYWoaZbK74NC6d61sr5Rth4foLQnMzCS5RaXuANMvyZW3Ol5ScvLl/KRZM4f4CB6rmYinyHfXIoPF+uCjavyOYnW1RDBASg0Ld/WUJlWb75m5GNkRELIc4c5FU54ysMW9o5wnGpQvtXNdCBJK8tAyZO9Wf2hjZeOZgJ6r1IngfSeSFu7EOFqWnwVOF+3juwWOLCwxrKcURAEngwH01ydwU3oG/rN+7JtdS3IwfaBt9sfDiLQ60qec/6PQc643UztE6oToHLRXsidwrObwyAKLSFJoh/uxWT85JgAoekq5zBen94HfELfMEc9tex6Qlf1tLDs7OWD6Mlw6j9aAcw/4Nfh")));
                var samlpKey = new X509SecurityKey(new X509Certificate2(Convert.FromBase64String("MIIGvzCCBKegAwIBAgICAZUwDQYJKoZIhvcNAQELBQAwgYYxCzAJBgNVBAYTAkNaMQ8wDQYDVQQHEwZQcmFndWUxGTAXBgNVBAoTEENaLk5JQywgei5zLnAuby4xMTAvBgNVBAMTKENaLk5JQyBTSEEyIFJvb3QgQ2VydGlmaWNhdGlvbiBBdXRob3JpdHkxGDAWBgkqhkiG9w0BCQEWCWNhQG5pYy5jejAeFw0xODAyMDYxMDM5MDJaFw0yMDAyMDYxMDM5MDJaMH0xCzAJBgNVBAYTAkNaMQ8wDQYDVQQHEwZQcmFndWUxDzANBgNVBAoTBkNaLk5JQzEwMC4GA1UEAxMnbW9qZWlkLnJlZ3Rlc3QubmljLmN6IHNpZGFzYW1sIG1ldGFkYXRhMRowGAYJKoZIhvcNAQkBFgtyb290QG5pYy5jejCCAiIwDQYJKoZIhvcNAQEBBQADggIPADCCAgoCggIBANUq8haz + ZNnLBWRLPLQKSyg7TlQDVyLxxMmxFhCT8OXArkjIsZvcwNNeMDB6aOhk0Zs97t + 7lEgC2MuiY9GnoC0DV + TAUf7 + MzlHuE0oCmR9jiJPsikBEsRDHpvHi9rZv4HK2TCsee / 5dDi7tP7bMnRvDOAt7lR + KuLQNaXtDWrXC4bjeNdO / mcy3UeKy + dW2Diqz8YMbRrxM29wAweaUSJ6npU9KTnx0 / dq / +IM4R1gO62t + 6vjxqiryEFcvdb6lGHc6qC9TYuHaGZBfXiT2goK4NbOr9dfLuixQ8Jd3oN88Qqt7r5u20VLCB06BIQHBgzJTHaSsi5MT5ymtx8lTpkR6MHXGue//QTZPi5DVBonb1B+ilgWdG7jK5yTBA6BkQSbbFp4uHM3IWdExErV/FPeyN9T7Au0kf7Jp73m7gMjD6ytC9xSI082ELufkjmerLTB0SoNPEsfAzUgDQeJ4DhsWg9kiK1/nhakjENefVW3FA2rZYBsRZkZr/uGdg/XEnw34ooeh+sTsj4QF5nWeuGmq0nu08hSTLv6YYfGwJny0TNekmfNoNL7Ip1RoRenl2ayruqvMSEzh4z5D1m4hW6zwmsRj0X8FJOk9pOr0NbHVsr2RuefmKNntk2bXMYq8dO3xSRASmdVmgmoyoQGJrnh1E0SihxCFNgiHqyGQ1JAgMBAAGjggE9MIIBOTAJBgNVHRMEAjAAMBEGCWCGSAGG+EIBAQQEAwIEsDAdBgNVHQ4EFgQU0HAvIqGfYUW/akxHd9SNK2O4GwEwgbsGA1UdIwSBszCBsIAUzxJQXpMfXwleF48WV0F6n3lyhLihgYykgYkwgYYxCzAJBgNVBAYTAkNaMQ8wDQYDVQQHEwZQcmFndWUxGTAXBgNVBAoTEENaLk5JQywgei5zLnAuby4xMTAvBgNVBAMTKENaLk5JQyBTSEEyIFJvb3QgQ2VydGlmaWNhdGlvbiBBdXRob3JpdHkxGDAWBgkqhkiG9w0BCQEWCWNhQG5pYy5jeoIJAJxtRGsvNinfMBQGA1UdEgQNMAuBCWNhQG5pYy5jejAWBgNVHREEDzANgQtyb290QG5pYy5jejAOBgNVHQ8BAf8EBAMCBaAwDQYJKoZIhvcNAQELBQADggIBAIytUqXh7AU/OunkSpUTaEY5Ze2sdV76JPwYVzNn2O6hCyzGvPXARP0IchUv8Vy+eCFLctUxvCbUS6aC+ObKgGXq4MxoSV8lijMLEW9crCpFDoLLd3LQw0GjVk2mCE7XTuaT0choPYlZmjv+wF2ZKm5/B+Qjek2j7SkY1yn7hxgJdd5ljHE6wmDXLJ8gHuVBNwvc5iHDjHYh7jL5c5jCDBcr1fFCsIARU05RAkfpWurl8GKY8t6IPm7iopOLjru3Gl45ZBdVrAPMQ8Fz1M9VElUJ6ngeKkXHkSwGhCBG3X0MYsltND6mZSkJqN0nOs+cJ6HHO8IZW1f2pH3aCTUSDYWoaZbK74NC6d61sr5Rth4foLQnMzCS5RaXuANMvyZW3Ol5ScvLl/KRZM4f4CB6rmYinyHfXIoPF+uCjavyOYnW1RDBASg0Ld/WUJlWb75m5GNkRELIc4c5FU54ysMW9o5wnGpQvtXNdCBJK8tAyZO9Wf2hjZeOZgJ6r1IngfSeSFu7EOFqWnwVOF+3juwWOLCwxrKcURAEngwH01ydwU3oG/rN+7JtdS3IwfaBt9sfDiLQ60qec/6PQc643UztE6oToHLRXsidwrObwyAKLSFJoh/uxWT85JgAoekq5zBen94HfELfMEc9tex6Qlf1tLDs7OWD6Mlw6j9aAcw/4Nfh")));
                var xmlWithTwoSamlTokens = CreateSignedXmlWithEmbededTokens(new List<SecurityToken> { CreateSamlToken(), CreateSamlToken()}, Default.SymmetricSigningCredentials, Default.AsymmetricSigningCredentials);
                var xmlWithSamlAndSaml2Tokens = CreateSignedXmlWithEmbededTokens(new List<SecurityToken> { CreateSamlToken(), CreateSaml2Token() }, Default.SymmetricSigningCredentials, Default.AsymmetricSigningCredentials);
                var xmlWithSaml2AndSamlTokens = CreateSignedXmlWithEmbededTokens(new List<SecurityToken> { CreateSaml2Token(), CreateSamlToken() }, Default.SymmetricSigningCredentials, Default.AsymmetricSigningCredentials);
                var xmlWithTwoSaml2Tokens = CreateSignedXmlWithEmbededTokens(new List<SecurityToken> { CreateSaml2Token(), CreateSaml2Token() }, Default.SymmetricSigningCredentials, Default.AsymmetricSigningCredentials);
                var xmlWithOneSamlToken = CreateSignedXmlWithEmbededTokens(new List<SecurityToken> { CreateSamlToken() }, Default.SymmetricSigningCredentials, Default.AsymmetricSigningCredentials);
                var xmlWithOneSaml2Token = CreateSignedXmlWithEmbededTokens(new List<SecurityToken> { CreateSaml2Token() }, Default.SymmetricSigningCredentials, Default.AsymmetricSigningCredentials);

                return new TheoryData<EnvelopedSignatureTheoryData>
                {
                    new EnvelopedSignatureTheoryData
                    {
                       SecurityKey = samlpKey,
                       TestId = "SamlpMessageWithSaml2Handler",
                       TokenSecurityKey = samlpTokenKey,
                       Xml = samlpMesage,
                       XmlElementReader = new TokenReaders(new List<SecurityTokenHandler>{new Saml2SecurityTokenHandler()})
                    },
                    new EnvelopedSignatureTheoryData
                    {
                       SecurityKey = Default.SymmetricSigningCredentials.Key,
                       TestId = "XmlWithTwoSamlTokens",
                       TokenSecurityKey = Default.AsymmetricSigningCredentials.Key,
                       Xml = xmlWithTwoSamlTokens,
                       XmlElementReader = new TokenReaders(new List<SecurityTokenHandler>{new SamlSecurityTokenHandler()})
                    },
                    new EnvelopedSignatureTheoryData
                    {
                       SecurityKey = Default.SymmetricSigningCredentials.Key,
                       TestId = "XmlWithSamlAndSaml2Tokens",
                       TokenSecurityKey = Default.AsymmetricSigningCredentials.Key,
                       Xml = xmlWithSamlAndSaml2Tokens,
                       XmlElementReader = new TokenReaders(new List<SecurityTokenHandler>{new SamlSecurityTokenHandler(), new Saml2SecurityTokenHandler()})
                    },
                    new EnvelopedSignatureTheoryData
                    {
                       SecurityKey = Default.SymmetricSigningCredentials.Key,
                       TestId = "XmlWithSaml2AndSamlTokens",
                       TokenSecurityKey = Default.AsymmetricSigningCredentials.Key,
                       Xml = xmlWithSamlAndSaml2Tokens,
                       XmlElementReader = new TokenReaders(new List<SecurityTokenHandler>{new SamlSecurityTokenHandler(), new Saml2SecurityTokenHandler()})
                    },
                    new EnvelopedSignatureTheoryData
                    {
                       SecurityKey = Default.SymmetricSigningCredentials.Key,
                       TestId = "XmlWithTwoSaml2Tokens",
                       TokenSecurityKey = Default.AsymmetricSigningCredentials.Key,
                       Xml = xmlWithTwoSaml2Tokens,
                       XmlElementReader = new TokenReaders(new List<SecurityTokenHandler>{new Saml2SecurityTokenHandler()})
                    },
                    new EnvelopedSignatureTheoryData
                    {
                       SecurityKey = Default.SymmetricSigningCredentials.Key,
                       TestId = "XmlWithOneSaml2Token",
                       TokenSecurityKey = Default.AsymmetricSigningCredentials.Key,
                       Xml = xmlWithOneSaml2Token,
                       XmlElementReader = new TokenReaders(new List<SecurityTokenHandler>{new Saml2SecurityTokenHandler()})
                    },
                    new EnvelopedSignatureTheoryData
                    {
                       SecurityKey = Default.SymmetricSigningCredentials.Key,
                       TestId = "XmlWithOneSamlToken",
                       TokenSecurityKey = Default.AsymmetricSigningCredentials.Key,
                       Xml = xmlWithOneSamlToken,
                       XmlElementReader = new TokenReaders(new List<SecurityTokenHandler>{new SamlSecurityTokenHandler()})
                    },
                    new EnvelopedSignatureTheoryData
                    {
                       TestId = "SamlTokenWithReader",
                       TokenSecurityKey = Default.AsymmetricSigningCredentials.Key,
                       Xml = samlString,
                       XmlElementReader = new TokenReaders(new List<SecurityTokenHandler>{new SamlSecurityTokenHandler()})
                    },
                    new EnvelopedSignatureTheoryData
                    {
                       TestId = "Saml2TokenWithReader",
                       TokenSecurityKey = Default.AsymmetricSigningCredentials.Key,
                       Xml = saml2String,
                       XmlElementReader = new TokenReaders(new List<SecurityTokenHandler>{new Saml2SecurityTokenHandler()})
                    },
                    new EnvelopedSignatureTheoryData
                    {
                       ExpectedException = new ExpectedException(typeof(XmlReadException), "IDX30019:"),
                       TestId = "TwoSamlTokensWithoutReader",
                       TokenSecurityKey = Default.AsymmetricSigningCredentials.Key,
                       Xml = "<StartElement>" + samlString+samlString + "</StartElement>",
                    },
                    new EnvelopedSignatureTheoryData
                    {
                       ExpectedException = new ExpectedException(typeof(XmlReadException), "IDX30019:"),
                       TestId = "TwoSaml2TokensWithoutReaders",
                       TokenSecurityKey = Default.AsymmetricSigningCredentials.Key,
                       Xml = "<StartElement>" + saml2String+saml2String + "</StartElement>",
                    },
                    new EnvelopedSignatureTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(XmlReadException), "IDX30019:"),
                        TestId = "XmlWithTwoSamlTokensWithoutReaders",
                        Xml = xmlWithTwoSamlTokens
                    },
                    new EnvelopedSignatureTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(XmlReadException), "IDX30019:"),
                        TestId = "SamlpMessageWithoutReaders",
                        Xml = samlpMesage
                    },
                    new EnvelopedSignatureTheoryData
                    {
                       ExpectedException = new ExpectedException(typeof(XmlReadException), "IDX30019:"),
                       SecurityKey = samlpKey,
                       TestId = "SamlpMessageWithSamlHandler",
                       TokenSecurityKey = samlpTokenKey,
                       Xml = samlpMesage,
                       XmlElementReader = new TokenReaders(new List<SecurityTokenHandler>{new SamlSecurityTokenHandler()})
                    }
                };
            }
        }

        private static string CreateSignedXmlWithEmbededTokens(IList<SecurityToken> samlTokens, SigningCredentials xmlSigningCredentials, SigningCredentials tokenSigningCredentials )
        {
            var ms = new MemoryStream();
            var writer = XmlDictionaryWriter.CreateTextWriter(ms, Encoding.UTF8, false);
            var samlTokenHandler = new SamlSecurityTokenHandler();
            var saml2TokenHandler = new Saml2SecurityTokenHandler();
            var envelopedWriter = new EnvelopedSignatureWriter(writer, xmlSigningCredentials, "ref#1");

            envelopedWriter.WriteStartElement("local", "elementName", "http://elementnamespace");
            envelopedWriter.WriteElementString("localElement", "SamlWillBeEmbeded");

            foreach (var token in samlTokens)
            {
                if (token is SamlSecurityToken)
                    samlTokenHandler.WriteToken(envelopedWriter, token);
                else
                    saml2TokenHandler.WriteToken(envelopedWriter, token);
            }

            envelopedWriter.WriteStartElement("local", "elementName2", "http://elementnamespace");
            envelopedWriter.WriteElementString("localElement", "SamlWillBeEmbeded2");
            foreach (var token in samlTokens)
            {
                if (token is SamlSecurityToken)
                    samlTokenHandler.WriteToken(envelopedWriter, token);
                else
                    saml2TokenHandler.WriteToken(envelopedWriter, token);
            }

            envelopedWriter.WriteEndElement();
            envelopedWriter.WriteEndElement();
            envelopedWriter.Flush();
            var xml = Encoding.UTF8.GetString(ms.ToArray());
            return xml;
        }

        static SecurityToken CreateSamlToken()
        {
            return CreateToken(new SamlSecurityTokenHandler());
        }

        static SecurityToken CreateSaml2Token()
        {
            return CreateToken(new Saml2SecurityTokenHandler());
        }

        static string CreateSamlTokenString()
        {
            var tokenHandler = new SamlSecurityTokenHandler();
            return tokenHandler.WriteToken(CreateSamlToken());
        }

        static string CreateSaml2TokenString()
        {
            var tokenHandler = new Saml2SecurityTokenHandler();
            return tokenHandler.WriteToken(CreateSaml2Token());
        }

        static SecurityToken CreateToken(SecurityTokenHandler tokenHandler)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow + TimeSpan.FromDays(1),
                Audience = Default.Audience,
                SigningCredentials = Default.AsymmetricSigningCredentials,
                Issuer = Default.Issuer,
                Subject = Default.SamlClaimsIdentity
            };

            return tokenHandler.CreateToken(tokenDescriptor);
        }
    }

    class TokenReaders : IXmlElementReader
    {
        private IEnumerable<SecurityTokenHandler> _tokenHandlers;
        private IList<object> _items = new List<object>();

        public TokenReaders(IEnumerable<SecurityTokenHandler> tokenHandlers)
        {
            _tokenHandlers = tokenHandlers;
        }

        public bool CanRead(XmlReader reader)
        {
            foreach (var tokenHandler in _tokenHandlers)
            {
                if (tokenHandler.CanReadToken(reader))
                    return true;
            }

            return false;
        }

        public void Read(XmlReader reader)
        {
            foreach (var tokenHandler in _tokenHandlers)
            {
                if (tokenHandler.CanReadToken(reader))
                    _items.Add(tokenHandler.ReadToken(reader));
            }
        }

        public IList<object> Items => _items;
    }

}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Json;
using NuGet.Frameworks;
using NuGet.Versioning;
using Xunit;

namespace NuGet.RuntimeModel.Test
{
    public class JsonRuntimeFormatTests
    {
        private const string SimpleRuntimeGraphContent = """{"runtimes":{"any":{}}}""";

        [Theory]
        [InlineData("{}")]
        [InlineData("{\"runtimes\":{}}")]
        public void CanParseEmptyRuntimeJsons(string content)
        {
            Assert.Equal(RuntimeGraph.Empty, ParseRuntimeJsonString(content));
        }

        [Fact]
        public void CanParseSupportsSection()
        {
            const string content = @"
{
    ""supports"": {
        ""windows-frob"": {
            ""netcore50"": [ ""winfrob-x86"", ""winfrob-x64"" ]
        }
    }
}";
            Assert.Equal(
                new RuntimeGraph(new[]
                    {
                        new CompatibilityProfile("windows-frob", new []
                            {
                                new FrameworkRuntimePair(FrameworkConstants.CommonFrameworks.NetCore50, "winfrob-x86"),
                                new FrameworkRuntimePair(FrameworkConstants.CommonFrameworks.NetCore50, "winfrob-x64")
                            })
                    }),
                ParseRuntimeJsonString(content));
        }

        [Fact]
        public void CanParseSupportsAsFoundInProjectFiles()
        {
            const string content = @"
{
    ""supports"": {
        ""windows-frob"": {}
    }
}";
            Assert.Equal(
                new RuntimeGraph(new[]
                    {
                        new CompatibilityProfile("windows-frob")
                    }),
                ParseRuntimeJsonString(content));
        }

        [Fact]
        public void CanParseCompatProfilesWithoutRuntimeIDs()
        {
            const string content = @"
{
    ""supports"": {
        ""windows-phone-8"": {
            ""wp8"": """"
        }
    }
}";
            Assert.Equal(
                new RuntimeGraph(new[]
                    {
                        new CompatibilityProfile("windows-phone-8", new [] {
                            new FrameworkRuntimePair(FrameworkConstants.CommonFrameworks.WP8, null)
                        })
                    }),
                ParseRuntimeJsonString(content));
        }

        [Fact]
        public void CanParseSimpleRuntimeJson()
        {
            const string content = @"
{
    ""runtimes"": {
        ""any"": {},
        ""win8-x86"": {
            ""#import"": [
                ""win8"",
                ""win7-x86""
            ],
            ""Some.Package"": {
                ""Some.Package.For.win8-x86"": ""4.2""
            }
        },
        ""win8"": {
            ""#import"": [
                ""win7""
            ]
        }
    }
}";

            Assert.Equal(
                new RuntimeGraph(new[]
                    {
                        new RuntimeDescription("any"),
                        new RuntimeDescription("win8-x86", new[]
                            {
                                "win8",
                                "win7-x86"
                            }, new[]
                                {
                                    new RuntimeDependencySet("Some.Package", new[]
                                        {
                                            new RuntimePackageDependency("Some.Package.For.win8-x86", new VersionRange(new NuGetVersion("4.2")))
                                        })
                                }),
                        new RuntimeDescription("win8", new[] { "win7" })
                    }), ParseRuntimeJsonString(content));
        }

        [Fact]
        public void ReadRuntimeGraph_WithStream_ParsesRuntimeGraph()
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(SimpleRuntimeGraphContent)))
            {
                Assert.Equal(CreateSimpleRuntimeGraph(), JsonRuntimeFormat.ReadRuntimeGraph(stream));
            }
        }

        [Fact]
        public void ReadRuntimeGraph_WithTextReader_ParsesRuntimeGraphAndDisposesReader()
        {
            var reader = new StringReader(SimpleRuntimeGraphContent);

#pragma warning disable CS0618 // Type or member is obsolete
            RuntimeGraph graph = JsonRuntimeFormat.ReadRuntimeGraph(reader);
#pragma warning restore CS0618 // Type or member is obsolete

            Assert.Equal(CreateSimpleRuntimeGraph(), graph);
            // The Newtonsoft-backed overload historically takes ownership of the supplied reader.
            Assert.Throws<ObjectDisposedException>(() => reader.Read());
        }

        [Fact]
        public void ReadRuntimeGraphWithSystemTextJson_WithLeadingTriviaLargerThanBuffer_ParsesRuntimeGraph()
        {
            string content = new string(' ', 20_000) + SimpleRuntimeGraphContent;
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));

            Assert.Equal(CreateSimpleRuntimeGraph(), JsonRuntimeFormat.ReadRuntimeGraph(stream));
        }

        [Fact]
        public void ReadRuntimeGraphWithSystemTextJson_WithUtf8Bom_ParsesRuntimeGraph()
        {
            // The former StreamReader path accepted a UTF-8 BOM.
            var stream = new MemoryStream();
            using (var writer = new StreamWriter(
                stream,
                new UTF8Encoding(encoderShouldEmitUTF8Identifier: true),
                bufferSize: 1024,
                leaveOpen: true))
            {
                writer.Write(SimpleRuntimeGraphContent);
            }
            stream.Position = 0;

            Assert.Equal(CreateSimpleRuntimeGraph(), JsonRuntimeFormat.ReadRuntimeGraph(stream));
        }

        [Theory]
        [InlineData("""{"runtimes":null}""")]
        [InlineData("""{"runtimes":"invalid"}""")]
        [InlineData("""{"runtimes":[]}""")]
        [InlineData("""{"supports":null}""")]
        [InlineData("""{"supports":"invalid"}""")]
        [InlineData("""{"supports":[]}""")]
        [InlineData("""{"runtimes":{"win":null}}""")]
        [InlineData("""{"runtimes":{"win":"invalid"}}""")]
        [InlineData("""{"runtimes":{"win":{"Package":null}}}""")]
        [InlineData("""{"runtimes":{"win":{"Package":"invalid"}}}""")]
        [InlineData("""{"supports":{"desktop":null}}""")]
        [InlineData("""{"supports":{"desktop":"invalid"}}""")]
        public void ReadRuntimeGraph_WithHistoricallyToleratedShape_MatchesTextReader(string content)
        {
            // Newtonsoft treats non-object sections and entries as empty rather than rejecting them.
            AssertStreamAndTextReaderResultsEqual(content);
        }

        [Theory]
        [InlineData("""{"runtimes":{"win":{"P":{"Q":1}}}}""")]
        [InlineData("""{"runtimes":{"win":{"#import":[1,true,null]}}}""")]
        [InlineData("""{"supports":{"desktop":{"net10.0":[1,true,null]}}}""")]
        public void ReadRuntimeGraph_WithConvertibleScalarValues_MatchesTextReader(string content)
        {
            // Newtonsoft coerces numeric and Boolean JTokens to strings and preserves null in these positions.
            AssertStreamAndTextReaderResultsEqual(content);
        }

        [Theory]
        [UseCulture("fr-FR")]
        [InlineData("""{"runtimes":{"win":{"P":{"Q":1.5}}}}""")]
        [InlineData("""{"runtimes":{"win":{"#import":[1.5]}}}""")]
        [InlineData("""{"supports":{"desktop":{"net10.0":[1.5]}}}""")]
        public void ReadRuntimeGraph_WithFractionalScalarValue_UsesInvariantConversion(string content)
        {
            // Newtonsoft formats fractional JToken values invariantly even under a decimal-comma culture.
            AssertStreamAndTextReaderResultsEqual(content);
            Assert.Equal("fr-FR", CultureInfo.CurrentCulture.Name);
        }

        [Theory]
        [UseCulture("fr-FR")]
        [InlineData("""{"runtimes":{"win":{"P":{"Q":1.5e1}}}}""")]
        [InlineData("""{"runtimes":{"win":{"#import":[1.5e1]}}}""")]
        [InlineData("""{"supports":{"desktop":{"net10.0":[1.5e1]}}}""")]
        public void ReadRuntimeGraph_WithExponentScalarValue_MatchesTextReader(string content)
        {
            // Newtonsoft formats exponent-form JToken values invariantly in all scalar positions.
            AssertStreamAndTextReaderResultsEqual(content);
            Assert.Equal("fr-FR", CultureInfo.CurrentCulture.Name);
        }

        [Theory]
        [InlineData("""{"runtimes":{"win":{"P":{"Q":9223372036854775808}}}}""")]
        [InlineData("""{"runtimes":{"win":{"#import":[9223372036854775808]}}}""")]
        [InlineData("""{"supports":{"desktop":{"net10.0":[9223372036854775808]}}}""")]
        public void ReadRuntimeGraph_WithOversizedInteger_MatchesTextReaderException(string content)
        {
            // Newtonsoft rejects BigInteger-to-string conversion with this exact exception shape.
            Exception streamException = Assert.ThrowsAny<Exception>(
                () => JsonRuntimeFormat.ReadRuntimeGraph(new MemoryStream(Encoding.UTF8.GetBytes(content))));

#pragma warning disable CS0618 // Type or member is obsolete
            Exception textReaderException = Assert.ThrowsAny<Exception>(
                () => JsonRuntimeFormat.ReadRuntimeGraph(new StringReader(content)));
#pragma warning restore CS0618 // Type or member is obsolete

            Assert.IsType<InvalidCastException>(streamException);
            Assert.Equal(textReaderException.GetType(), streamException.GetType());
            Assert.Equal(textReaderException.Message, streamException.Message);
        }

        [Fact]
        public void ReadRuntimeGraph_WithBooleanDependencyScalar_MatchesTextReaderException()
        {
            const string content = """{"runtimes":{"win":{"P":{"Q":true}}}}""";
            // Newtonsoft passes Boolean text to VersionRange.Parse, preserving its exception type.
            Type streamExceptionType = Assert.ThrowsAny<Exception>(
                () => JsonRuntimeFormat.ReadRuntimeGraph(new MemoryStream(Encoding.UTF8.GetBytes(content)))).GetType();

#pragma warning disable CS0618 // Type or member is obsolete
            Type textReaderExceptionType = Assert.ThrowsAny<Exception>(
                () => JsonRuntimeFormat.ReadRuntimeGraph(new StringReader(content))).GetType();
#pragma warning restore CS0618 // Type or member is obsolete

            Assert.Equal(textReaderExceptionType, streamExceptionType);
        }

        [Fact]
        public void ReadRuntimeGraphWithSystemTextJson_WithNullDependencyVersion_Throws()
        {
            const string content = """{"runtimes":{"win":{"P":{"Q":null}}}}""";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));

            Assert.Throws<JsonException>(() => JsonRuntimeFormat.ReadRuntimeGraph(stream));
        }

        [Fact]
        public void ReadRuntimeGraphWithSystemTextJson_WithDuplicateRuntimeProperties_Throws()
        {
            const string content = """
                {
                    "runtimes": {
                        "win": { "#import": [ "win7" ] },
                        "win": { "#import": [ "win8" ] }
                    }
                }
                """;
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));

            Assert.Throws<ArgumentException>(() => JsonRuntimeFormat.ReadRuntimeGraph(stream));
        }

        [Fact]
        public void ReadRuntimeGraphWithSystemTextJson_WithDuplicateRootProperties_UsesLastValue()
        {
            // JObject property lookup observes the last duplicate root section.
            const string content = """
                {
                    "runtimes": "invalid",
                    "runtimes": {
                        "win": { "#import": [ "win8" ] }
                    }
                }
                """;
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));

            RuntimeGraph graph = JsonRuntimeFormat.ReadRuntimeGraph(stream);

            Assert.Equal("win", Assert.Single(graph.Runtimes).Key);
        }

        [Fact]
        public void ReadRuntimeGraph_WithCommentsAndTrailingCommas_ParsesRuntimeGraph()
        {
            const string content = """
                {
                    // Runtime identifiers
                    "runtimes": {
                        "any": {
                            "#import": [],
                        },
                    },
                }
                """;

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(content)))
            {
                Assert.Equal(CreateSimpleRuntimeGraph(), JsonRuntimeFormat.ReadRuntimeGraph(stream));
            }
        }

        private static RuntimeGraph CreateSimpleRuntimeGraph()
        {
            return new RuntimeGraph(new[] { new RuntimeDescription("any") });
        }

        private static void AssertStreamAndTextReaderResultsEqual(string content)
        {
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            RuntimeGraph streamResult = JsonRuntimeFormat.ReadRuntimeGraph(stream);

#pragma warning disable CS0618 // Type or member is obsolete
            RuntimeGraph textReaderResult = JsonRuntimeFormat.ReadRuntimeGraph(new StringReader(content));
#pragma warning restore CS0618 // Type or member is obsolete

            Assert.Equal(textReaderResult, streamResult);
        }

        private static RuntimeGraph ParseRuntimeJsonString(string content)
        {
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            return JsonRuntimeFormat.ReadRuntimeGraph(stream);
        }
    }
}

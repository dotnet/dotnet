// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using NuGet.Frameworks;
using NuGet.Versioning;
using Xunit;

namespace NuGet.RuntimeModel.Test
{
    public class JsonRuntimeFormatTests
    {
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

        private RuntimeGraph ParseRuntimeJsonString(string content)
        {
            using (var reader = new StringReader(content))
            {
                return JsonRuntimeFormat.ReadRuntimeGraph(reader);
            }
        }
    }
}

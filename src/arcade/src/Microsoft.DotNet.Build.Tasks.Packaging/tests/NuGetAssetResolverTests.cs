// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using NuGet.Frameworks;
using Xunit;
using Xunit.Abstractions;
using FluentAssertions;

namespace Microsoft.DotNet.Build.Tasks.Packaging.Tests
{
    public class NuGetAssetResolverTests
    {
        private Log _log;


        public NuGetAssetResolverTests(ITestOutputHelper output)
        {
            _log = new Log(output);
        }

        [Fact]
        public void RuntimeResolutionTest()
        {
            string[] items =
            {
                "runtimes/any/lib/netcore50/System.Xml.XmlSerializer.dll",
                "runtimes/aot/lib/netcore50/_._"
            };

            NuGetAssetResolver resolver = new NuGetAssetResolver("runtime.json", items);

            var runtimeItems = resolver.GetRuntimeItems(NuGetFramework.Parse("netcore50"), "win10-x64-aot");

            runtimeItems.Should().NotBeNull();
            runtimeItems.Items.Should().HaveCount(1);

            // Fails due to https://github.com/NuGet/Home/issues/1676
            // Assert.Equal(items[1], runtimeItems.Items.FirstOrDefault().Path);
        }
    }
}

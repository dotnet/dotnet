// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Net.Http;
using NuGet.Protocol.Core.Types;
using Xunit;
using Xunit.Abstractions;

namespace NuGet.Protocol.Tests
{
    public class UserAgentStringBuilderTests
    {
        private readonly ITestOutputHelper _output;

        public UserAgentStringBuilderTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void UsesTestClientNameInTestMode()
        {
            var builder = new UserAgentStringBuilder();

            var userAgentString = NuGetTestMode.InvokeTestFunctionAgainstTestMode(
                () => builder.Build(),
                testModeEnabled: true);

            _output.WriteLine(userAgentString);
            Assert.True(userAgentString.StartsWith(NuGetTestMode.NuGetTestClientName));
        }

        [Fact]
        public void UsesDefaultClientNameWhenNotInTestModeAndNoneSet()
        {
            var builder = new UserAgentStringBuilder();

            var userAgentString = NuGetTestMode.InvokeTestFunctionAgainstTestMode(
                () => builder.Build(),
                testModeEnabled: false);

            _output.WriteLine(userAgentString);
            Assert.True(userAgentString.StartsWith(UserAgentStringBuilder.DefaultNuGetClientName));
        }

        [Fact]
        public void UsesProvidedClientNameWhenNotInTestMode()
        {
            var builder = new UserAgentStringBuilder("Dummy Test Client Name");

            var userAgentString = NuGetTestMode.InvokeTestFunctionAgainstTestMode(
                () => builder.Build(),
                testModeEnabled: false);

            _output.WriteLine(userAgentString);
            Assert.True(userAgentString.StartsWith("Dummy Test Client Name"));
        }

        [Fact]
        public void UsesProvidedOSDescription()
        {
            var osDescription = "OSName/OSVersion";
            var builder = new UserAgentStringBuilder();
            var userAgentString = builder.WithOSDescription(osDescription).Build();
            var userAgentString2 = builder.WithOSDescription(osDescription).WithVisualStudioSKU("VS SKU/Version").Build();

            Assert.True(userAgentString.Contains($"({osDescription})"));
            Assert.True(userAgentString2.Contains($"({osDescription}, VS SKU/Version)"));
        }

        [Fact]
        public void UsesProvidedVisualStudioInfo()
        {
            var vsInfo = "VS SKU/Version";
            var builder = new UserAgentStringBuilder();
            var userAgentString = builder.WithOSDescription("OSName/OSVersion").WithVisualStudioSKU(vsInfo).Build();

            _output.WriteLine(userAgentString);
            Assert.True(userAgentString.Contains($"(OSName/OSVersion, {vsInfo})"));
        }

        [Fact]
        public void UsesComputedNuGetClientVersion()
        {
            var builder = new UserAgentStringBuilder();

            var userAgentString = builder.WithOSDescription("OSName/OSVersion").WithVisualStudioSKU("VS SKU/Version").Build();
            var userAgentString2 = builder.WithOSDescription("OSName/OSVersion").Build();
            var userAgentString3 = builder.WithVisualStudioSKU("VS SKU/Version").Build();
            var userAgentString4 = builder.Build();

            _output.WriteLine("NuGet client version: " + builder.NuGetClientVersion);
            Assert.NotNull(builder.NuGetClientVersion);
            Assert.NotEmpty(builder.NuGetClientVersion);
            Assert.True(userAgentString.Contains(builder.NuGetClientVersion));
            Assert.True(userAgentString2.Contains(builder.NuGetClientVersion));
            Assert.True(userAgentString3.Contains(builder.NuGetClientVersion));
            Assert.True(userAgentString4.Contains(builder.NuGetClientVersion));
        }

        [Theory]
        [InlineData("Custom Kernel (123")]
        [InlineData("Custom Kernel 123)")]
        public void Build_OsDescriptionWithUnmatchedParenthesis_IsValid(string osDescription)
        {
            // Arrange
            UserAgentStringBuilder target = new();

            // Act
            string result = target.WithOSDescription(osDescription).Build();

            // Assert
            HttpRequestMessage httpRequest = new();
            httpRequest.Headers.Add("User-Agent", result);
        }
    }
}

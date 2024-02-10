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
            Assert.StartsWith(NuGetTestMode.NuGetTestClientName, userAgentString);
        }

        [Fact]
        public void UsesDefaultClientNameWhenNotInTestModeAndNoneSet()
        {
            var builder = new UserAgentStringBuilder();

            var userAgentString = NuGetTestMode.InvokeTestFunctionAgainstTestMode(
                () => builder.Build(),
                testModeEnabled: false);

            _output.WriteLine(userAgentString);
            Assert.StartsWith(UserAgentStringBuilder.DefaultNuGetClientName, userAgentString);
        }

        [Fact]
        public void UsesProvidedClientNameWhenNotInTestMode()
        {
            var builder = new UserAgentStringBuilder("Dummy Test Client Name");

            var userAgentString = NuGetTestMode.InvokeTestFunctionAgainstTestMode(
                () => builder.Build(),
                testModeEnabled: false);

            _output.WriteLine(userAgentString);
            Assert.StartsWith("Dummy Test Client Name", userAgentString);
        }

        [Fact]
        public void UsesProvidedOSDescription()
        {
            var osDescription = "OSName/OSVersion";
            var builder = new UserAgentStringBuilder();
            var userAgentString = builder.WithOSDescription(osDescription).Build();
            var userAgentString2 = builder.WithOSDescription(osDescription).WithVisualStudioSKU("VS SKU/Version").Build();

            Assert.Contains($"({osDescription})", userAgentString);
            Assert.Contains($"({osDescription}, VS SKU/Version)", userAgentString2);
        }

        [Fact]
        public void UsesProvidedVisualStudioInfo()
        {
            var vsInfo = "VS SKU/Version";
            var builder = new UserAgentStringBuilder();
            var userAgentString = builder.WithOSDescription("OSName/OSVersion").WithVisualStudioSKU(vsInfo).Build();

            _output.WriteLine(userAgentString);
            Assert.Contains($"(OSName/OSVersion, {vsInfo})", userAgentString);
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
            Assert.Contains(builder.NuGetClientVersion, userAgentString);
            Assert.Contains(builder.NuGetClientVersion, userAgentString2);
            Assert.Contains(builder.NuGetClientVersion, userAgentString3);
            Assert.Contains(builder.NuGetClientVersion, userAgentString4);
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

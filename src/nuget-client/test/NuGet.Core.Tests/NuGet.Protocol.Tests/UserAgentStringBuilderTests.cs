// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using FluentAssertions;
using NuGet.Packaging;
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
        public void AddsOSInfo()
        {
            var clientName = "clientName";
            var clientVersion = MinClientVersionUtility.GetNuGetClientVersion().ToNormalizedString();
            var os = UserAgentStringBuilder.GetOS();
            var vs = "VS SKU/Version";

            var builder = new UserAgentStringBuilder(clientName);
            var userAgentString = builder.Build();
            var userAgentString2 = builder.WithVisualStudioSKU(vs).Build();

            userAgentString.Should().Be($"{clientName}/{clientVersion} ({os})");
            userAgentString2.Should().Be($"{clientName}/{clientVersion} ({os}, {vs})");
        }

        [Fact]
        public void UsesProvidedVisualStudioInfo()
        {
            var vsInfo = "VS SKU/Version";
            var builder = new UserAgentStringBuilder();
            var userAgentString = builder.WithVisualStudioSKU(vsInfo).Build();

            _output.WriteLine(userAgentString);
            Assert.Contains($", {vsInfo})", userAgentString);
        }

        [Fact]
        public void UsesComputedNuGetClientVersion()
        {
            var builder = new UserAgentStringBuilder();

            var userAgentString = builder.WithVisualStudioSKU("VS SKU/Version").Build();
            var userAgentString2 = builder.Build();

            _output.WriteLine("NuGet client version: " + builder.NuGetClientVersion);
            Assert.NotNull(builder.NuGetClientVersion);
            Assert.NotEmpty(builder.NuGetClientVersion);
            Assert.Contains(builder.NuGetClientVersion, userAgentString);
            Assert.Contains(builder.NuGetClientVersion, userAgentString2);
        }
    }
}

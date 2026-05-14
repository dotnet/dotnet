// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using FluentAssertions;
using NuGet.Packaging;
using NuGet.Protocol.Core.Types;
using Test.Utility;
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
            var environmentReader = new TestEnvironmentVariableReader(new Dictionary<string, string>());
            var clientName = "clientName";
            var clientVersion = MinClientVersionUtility.GetNuGetClientVersion().ToNormalizedString();
            var os = UserAgentStringBuilder.GetOS();
            var vs = "VS SKU/Version";

            var builder = new UserAgentStringBuilder(clientName, environmentReader);
            var userAgentString = builder.Build();
            var userAgentString2 = builder.WithVisualStudioSKU(vs).Build();

            userAgentString.Should().Contain($"{clientName}/{clientVersion} ({os})");
            userAgentString2.Should().Contain($"{clientName}/{clientVersion} ({os}, {vs})");
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

        [Fact]
        public void Build_WhenInAzureDevOps_AppendsCIInfoInParentheses()
        {
            // Arrange
            var environmentReader = new TestEnvironmentVariableReader(new Dictionary<string, string>
            {
                { "TF_BUILD", "True" }
            });
            var clientName = "TestClient";
            var builder = new UserAgentStringBuilder(clientName, environmentReader);

            // Act
            var userAgent = builder.Build();

            // Assert
            _output.WriteLine(userAgent);
            userAgent.Should().Contain("CI: Azure DevOps");
        }

        [Fact]
        public void Build_WhenInAppVeyor_AppendsCIInfoInParentheses()
        {
            // Arrange
            var environmentReader = new TestEnvironmentVariableReader(new Dictionary<string, string>
            {
                { "APPVEYOR", "True" }
            });
            var clientName = "TestClient";
            var builder = new UserAgentStringBuilder(clientName, environmentReader);

            // Act
            var userAgent = builder.Build();

            // Assert
            _output.WriteLine(userAgent);
            userAgent.Should().Contain("CI: AppVeyor");
        }

        [Fact]
        public void Build_WhenInTravisCI_AppendsCIInfoInParentheses()
        {
            // Arrange
            var environmentReader = new TestEnvironmentVariableReader(new Dictionary<string, string>
            {
                { "TRAVIS", "True" }
            });
            var clientName = "TestClient";
            var builder = new UserAgentStringBuilder(clientName, environmentReader);

            // Act
            var userAgent = builder.Build();

            // Assert
            _output.WriteLine(userAgent);
            userAgent.Should().Contain("CI: Travis CI");
        }

        [Fact]
        public void Build_WhenInGitLabCI_AppendsCIInfoInParentheses()
        {
            // Arrange
            var environmentReader = new TestEnvironmentVariableReader(new Dictionary<string, string>
            {
                { "GITLAB_CI", "true" }
            });
            var clientName = "TestClient";
            var builder = new UserAgentStringBuilder(clientName, environmentReader);

            // Act
            var userAgent = builder.Build();

            // Assert
            _output.WriteLine(userAgent);
            userAgent.Should().Contain("CI: GitLab CI");
        }

        [Fact]
        public void Build_WhenInCircleCI_AppendsCIInfoInParentheses()
        {
            // Arrange
            var environmentReader = new TestEnvironmentVariableReader(new Dictionary<string, string>
            {
                { "CIRCLECI", "True" }
            });
            var clientName = "TestClient";
            var builder = new UserAgentStringBuilder(clientName, environmentReader);

            // Act
            var userAgent = builder.Build();

            // Assert
            _output.WriteLine(userAgent);
            userAgent.Should().Contain("CI: CircleCI");
        }

        [Fact]
        public void Build_WhenInAWSCodeBuild_AppendsCIInfoInParentheses()
        {
            // Arrange
            var environmentReader = new TestEnvironmentVariableReader(new Dictionary<string, string>
            {
                { "CODEBUILD_BUILD_ID", "build-123" }
            });
            var clientName = "TestClient";
            var builder = new UserAgentStringBuilder(clientName, environmentReader);

            // Act
            var userAgent = builder.Build();

            // Assert
            _output.WriteLine(userAgent);
            userAgent.Should().Contain("CI: AWS CodeBuild");
        }

        [Fact]
        public void Build_WhenInJenkins_AppendsCIInfoInParentheses()
        {
            // Arrange
            var environmentReader = new TestEnvironmentVariableReader(new Dictionary<string, string>
            {
                { "BUILD_ID", "build-123" },
                { "BUILD_URL", "http://jenkins.example.com/job/123" }
            });
            var clientName = "TestClient";
            var builder = new UserAgentStringBuilder(clientName, environmentReader);

            // Act
            var userAgent = builder.Build();

            // Assert
            _output.WriteLine(userAgent);
            userAgent.Should().Contain("CI: Jenkins");
        }

        [Fact]
        public void Build_WhenInGoogleCloud_AppendsCIInfoInParentheses()
        {
            // Arrange
            var environmentReader = new TestEnvironmentVariableReader(new Dictionary<string, string>
            {
                { "BUILD_ID", "build-123" },
                { "PROJECT_ID", "my-project" }
            });
            var clientName = "TestClient";
            var builder = new UserAgentStringBuilder(clientName, environmentReader);

            // Act
            var userAgent = builder.Build();

            // Assert
            _output.WriteLine(userAgent);
            userAgent.Should().Contain("CI: Google Cloud");
        }

        [Fact]
        public void Build_WhenInTeamCity_AppendsCIInfoInParentheses()
        {
            // Arrange
            var environmentReader = new TestEnvironmentVariableReader(new Dictionary<string, string>
            {
                { "TEAMCITY_VERSION", "2023.11.1" }
            });
            var clientName = "TestClient";
            var builder = new UserAgentStringBuilder(clientName, environmentReader);

            // Act
            var userAgent = builder.Build();

            // Assert
            _output.WriteLine(userAgent);
            userAgent.Should().Contain("CI: TeamCity");
        }

        [Fact]
        public void Build_WhenInJetBrainsSpace_AppendsCIInfoInParentheses()
        {
            // Arrange
            var environmentReader = new TestEnvironmentVariableReader(new Dictionary<string, string>
            {
                { "JB_SPACE_API_URL", "https://space.example.com/api" }
            });
            var clientName = "TestClient";
            var builder = new UserAgentStringBuilder(clientName, environmentReader);

            // Act
            var userAgent = builder.Build();

            // Assert
            _output.WriteLine(userAgent);
            userAgent.Should().Contain("CI: JetBrains Space");
        }

        [Fact]
        public void Build_WhenInGenericCI_AppendsCIInfoInParentheses()
        {
            // Arrange
            var environmentReader = new TestEnvironmentVariableReader(new Dictionary<string, string>
            {
                { "CI", "True" }
            });
            var clientName = "TestClient";
            var builder = new UserAgentStringBuilder(clientName, environmentReader);

            // Act
            var userAgent = builder.Build();

            // Assert
            _output.WriteLine(userAgent);
            userAgent.Should().Contain("CI: other)");
        }

        [Fact]
        public void Build_WhenNotInCI_DoesNotAppendCIInfo()
        {
            // Arrange
            var environmentReader = new TestEnvironmentVariableReader(new Dictionary<string, string>());
            var clientName = "TestClient";
            var clientVersion = MinClientVersionUtility.GetNuGetClientVersion().ToNormalizedString();
            var os = UserAgentStringBuilder.GetOS();
            var builder = new UserAgentStringBuilder(clientName, environmentReader);

            // Act
            var userAgent = builder.Build();

            // Assert
            _output.WriteLine(userAgent);
            userAgent.Should().NotContain("CI:");
            userAgent.Should().Be($"{clientName}/{clientVersion} ({os})");
        }

        [Fact]
        public void Build_WhenInGitHubActions_WithVisualStudioSKU_AppendsCIInfo()
        {
            // Arrange
            var environmentReader = new TestEnvironmentVariableReader(new Dictionary<string, string>
            {
                { "GITHUB_ACTIONS", "true" }
            });
            var clientName = "TestClient";
            var vsInfo = "VS Enterprise/17.0";
            var os = UserAgentStringBuilder.GetOS();
            var builder = new UserAgentStringBuilder(clientName, environmentReader);

            // Act
            var userAgent = builder.WithVisualStudioSKU(vsInfo).Build();

            // Assert
            _output.WriteLine(userAgent);
            userAgent.Should().Contain(vsInfo);
            userAgent.Should().Contain($"{os}, CI: GitHub Actions");
        }

        [Fact]
        public void BuildMetadataString_WithAllInfo_ReturnsCommaSeparatedItems()
        {
            // Arrange
            var environmentReader = new TestEnvironmentVariableReader(new Dictionary<string, string>
            {
                { "GITHUB_ACTIONS", "true" }
            });
            var builder = new UserAgentStringBuilder("TestClient", environmentReader);
            builder.WithVisualStudioSKU("VS Enterprise/17.0");
            var os = UserAgentStringBuilder.GetOS();

            // Act
            string result = builder.BuildMetadataString();

            // Assert - all items separated by ", "
            result.Should().Be($"{os}, CI: GitHub Actions, VS Enterprise/17.0");
        }

        [Fact]
        public void BuildMetadataString_WithNoCI_ReturnsOSAndVS()
        {
            // Arrange
            var environmentReader = new TestEnvironmentVariableReader(new Dictionary<string, string>());
            var builder = new UserAgentStringBuilder("TestClient", environmentReader);
            builder.WithVisualStudioSKU("VS Enterprise/17.0");
            var os = UserAgentStringBuilder.GetOS();

            // Act
            string result = builder.BuildMetadataString();

            // Assert
            result.Should().Be($"{os}, VS Enterprise/17.0");
        }

        [Fact]
        public void BuildMetadataString_WithOnlyOS_ReturnsOS()
        {
            // Arrange
            var environmentReader = new TestEnvironmentVariableReader(new Dictionary<string, string>());
            var builder = new UserAgentStringBuilder("TestClient", environmentReader);
            var os = UserAgentStringBuilder.GetOS();

            // Act
            string result = builder.BuildMetadataString();

            // Assert
            result.Should().Be(os);
        }
    }
}

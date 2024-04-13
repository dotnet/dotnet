// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using FluentAssertions;
using NuGet.Common;
using Xunit;

namespace NuGet.Configuration.Test
{
    public class PackageSourceTests
    {
        [Fact]
        public void Clone_CopiesAllPropertyValuesFromSource()
        {
            // Arrange
            var credentials = new PackageSourceCredential("SourceName", "username", "password", isPasswordClearText: false, validAuthenticationTypesText: null);
            var source = new PackageSource("Source", "SourceName", isEnabled: false)
            {
                Credentials = credentials,
                ProtocolVersion = 43,
                AllowInsecureConnections = true,
                DisableTLSCertificateValidation = true
            };

            // Act
            var result = source.Clone();

            // Assert

            // source data
            Assert.Equal(source.Source, result.Source);
            Assert.Equal(source.Name, result.Name);
            Assert.Equal(source.IsEnabled, result.IsEnabled);
            Assert.Equal(source.ProtocolVersion, result.ProtocolVersion);
            Assert.Equal(source.AllowInsecureConnections, result.AllowInsecureConnections);
            Assert.Equal(source.DisableTLSCertificateValidation, result.DisableTLSCertificateValidation);

            // source credential
            result.Credentials.Should().NotBeNull();
            result.Credentials!.Source.Should().BeEquivalentTo(source.Credentials.Source);
            result.Credentials.Username.Should().BeEquivalentTo(source.Credentials.Username);
            result.Credentials.IsPasswordClearText.Should().Be(source.Credentials.IsPasswordClearText);
        }

        [Fact]
        public void AsSourceItem_WorksCorrectly()
        {
            var source = new PackageSource("Source", "SourceName", isEnabled: false)
            {
                ProtocolVersion = 43,
                AllowInsecureConnections = true,
                DisableTLSCertificateValidation = true
            };
            var result = source.AsSourceItem();

            var expectedItem = new SourceItem("SourceName", "Source", "43", "True", "True");

            SettingsTestUtils.DeepEquals(result, expectedItem).Should().BeTrue();
        }

        [Fact]
        void CalculatedMembers_ForHttpsSource_HasExpectedValues()
        {
            // Arrange & Act
            PackageSource source = new("https://my.test/v3.index.json");

            // Assert
            source.IsHttps.Should().BeTrue();
            source.IsHttp.Should().BeTrue();
            source.IsLocal.Should().BeFalse();
        }

        [Fact]
        void CalculatedMembers_ForHttpSource_HasExpectedValues()
        {
            // Arrange & Act
            PackageSource source = new("http://my.test/v3.index.json");

            // Assert
            source.IsHttps.Should().BeFalse();
            source.IsHttp.Should().BeTrue();
            source.IsLocal.Should().BeFalse();
        }

        [Fact]
        void CalculatedMembers_ForLocalSource_HasExpectedValues()
        {
            // Arrange & Act
            var path = RuntimeEnvironmentHelper.IsWindows
                ? @"c:\path\to\packages"
                : "/path/to/packages";
            PackageSource source = new(path);

            // Assert
            source.IsHttps.Should().BeFalse();
            source.IsHttp.Should().BeFalse();
            source.IsLocal.Should().BeTrue();
        }

        [Fact]
        public void CalculatedMembers_ChangingSource_UpdatesValues()
        {
            // Arrange
            PackageSource source = new(source: "https://my.test/v3/index.json", name: "MySource");
            bool httpBefore = source.IsHttp;
            bool httpsBefore = source.IsHttps;
            bool localBefore = source.IsLocal;
            int hashCodeBefore = source.GetHashCode();

            // Act
            source.Source = @"c:\path\to\packages";

            // Assert
            httpBefore.Should().BeTrue();
            httpsBefore.Should().BeTrue();
            localBefore.Should().BeFalse();

            source.IsHttp.Should().BeFalse();
            source.IsHttps.Should().BeFalse();
            source.IsLocal.Should().BeTrue();
            source.GetHashCode().Should().NotBe(hashCodeBefore);
        }
    }
}

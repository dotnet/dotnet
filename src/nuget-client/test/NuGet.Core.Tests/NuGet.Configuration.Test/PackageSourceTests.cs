// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using NuGet.Common;
using NuGet.Test.Utility;
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
        public void MinPublishAge_IsReadAsTimeSpanAndWrittenAsHours()
        {
            var source = new PackageSource("Source", "SourceName")
            {
                MinPublishAge = TimeSpan.FromHours(72)
            };

            var result = source.AsSourceItem();

            result.MinPublishAgeHours.Should().Be("72");
            source.Clone().MinPublishAge.Should().Be(TimeSpan.FromHours(72));
        }

        [Fact]
        public void MinPublishAge_WhenSetToNegativeTimeSpan_ThrowsArgumentOutOfRangeException()
        {
            var source = new PackageSource("Source", "SourceName");

            Action action = () => source.MinPublishAge = TimeSpan.FromHours(-1);

            action.Should().Throw<ArgumentOutOfRangeException>()
                .Which.ParamName.Should().Be("value");
        }

        [Fact]
        public void MinPublishAge_WhenSetToFractionalHour_ThrowsArgumentOutOfRangeException()
        {
            var source = new PackageSource("Source", "SourceName");

            Action action = () => source.MinPublishAge = TimeSpan.FromMinutes(30);

            action.Should().Throw<ArgumentOutOfRangeException>()
                .Which.ParamName.Should().Be("value");
        }

        [Theory]
        [InlineData("-1")]
        [InlineData("not-a-number")]
        public void ReadPackageSource_InvalidMinPublishAge_ThrowsWithSourceValueAndPath(string value)
        {
            using var directory = TestDirectory.Create();
            string fileName = Settings.DefaultSettingsFileName;
            SettingsTestUtils.CreateConfigurationFile(
                fileName,
                directory,
                $"""
                <configuration>
                    <packageSources>
                        <add key="test-source" value="https://test.test/v3/index.json" minPublishAgeHours="{value}" />
                    </packageSources>
                </configuration>
                """);

            var settingsFile = new SettingsFile(directory);
            var sourceItem = settingsFile.GetSection(ConfigurationConstants.PackageSources)!.Items.Cast<SourceItem>().Single();

            var exception = Record.Exception(() =>
                PackageSourceProvider.ReadPackageSource(sourceItem, isEnabled: true, NullSettings.Instance, EnvironmentVariableWrapper.Instance));

            exception.Should().BeOfType<NuGetConfigurationException>();
            exception.Message.Should().Contain("test-source");
            exception.Message.Should().Contain(value);
            exception.Message.Should().Contain(Path.Combine(directory, fileName));
        }

        [Fact]
        public void CalculatedMembers_ForHttpsSource_HasExpectedValues()
        {
            // Arrange & Act
            PackageSource source = new("https://my.test/v3.index.json");

            // Assert
            source.IsHttps.Should().BeTrue();
            source.IsHttp.Should().BeTrue();
            source.IsLocal.Should().BeFalse();
        }

        [Fact]
        public void CalculatedMembers_ForHttpSource_HasExpectedValues()
        {
            // Arrange & Act
            PackageSource source = new("http://my.test/v3.index.json");

            // Assert
            source.IsHttps.Should().BeFalse();
            source.IsHttp.Should().BeTrue();
            source.IsLocal.Should().BeFalse();
        }

        [Fact]
        public void CalculatedMembers_ForLocalSource_HasExpectedValues()
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

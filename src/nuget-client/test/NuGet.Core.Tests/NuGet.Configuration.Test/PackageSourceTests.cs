// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using FluentAssertions;
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
                ProtocolVersion = 43
            };

            // Act
            var result = source.Clone();

            // Assert

            // source data
            Assert.Equal(source.Source, result.Source);
            Assert.Equal(source.Name, result.Name);
            Assert.Equal(source.IsEnabled, result.IsEnabled);
            Assert.Equal(source.ProtocolVersion, result.ProtocolVersion);

            // source credential
            result.Credentials.Should().NotBeNull();
            result.Credentials.Source.Should().BeEquivalentTo(source.Credentials.Source);
            result.Credentials.Username.Should().BeEquivalentTo(source.Credentials.Username);
            result.Credentials.IsPasswordClearText.Should().Be(source.Credentials.IsPasswordClearText);
        }

        [Fact]
        public void AsSourceItem_WorksCorrectly()
        {
            var source = new PackageSource("Source", "SourceName", isEnabled: false)
            {
                ProtocolVersion = 43
            };

            var expectedItem = new SourceItem("SourceName", "Source", "43");

            SettingsTestUtils.DeepEquals(source.AsSourceItem(), expectedItem).Should().BeTrue();
        }
    }
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Net;
using NuGet.Common;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class AuthTypeFilteredCredentialTests
    {
        [Theory]
        [InlineData("negotiate")]
        [InlineData("basic")]
        [InlineData("somethirdthing")]
        void GetCredential_AllowsAnyWhenFilterEmpty(string authType)
        {
            // Arrange
            var expected = new NetworkCredential("username", "password");
            var credential = new AuthTypeFilteredCredentials(expected, Enumerable.Empty<string>());

            // Act
            var actual = credential.GetCredential(new Uri("https://example.com/"), authType);

            // Assert
            Assert.Same(expected, actual);
        }

        [Theory]
        [InlineData("basic")]
        [InlineData("somethirdthing")]
        void GetCredential_PassesThroughWhenAuthTypeInFilter(string authType)
        {
            // Arrange
            var expected = new NetworkCredential("username", "password");
            var credential = new AuthTypeFilteredCredentials(expected, new[] { "basic", "somethirdthing" });

            // Act
            var actual = credential.GetCredential(new Uri("https://example.com/"), authType);

            // Assert
            Assert.Same(expected, actual);
        }

        [Fact]
        void GetCredential_PassesThroughWhenAuthTypeIsNull()
        {
            // Arrange
            var expected = new NetworkCredential("username", "password");
            var credential = new AuthTypeFilteredCredentials(expected, new[] { "basic", "somethirdthing" });

            // Act
            var actual = credential.GetCredential(new Uri("https://example.com/"), null);

            // Assert
            Assert.Same(expected, actual);
        }

        [Theory]
        [InlineData("negotiate")]
        [InlineData("anotherunknownvalue")]
        void GetCredential_ReturnsNullWhenAuthTypeNotInFilter(string authType)
        {
            // Arrange
            var unexpected = new NetworkCredential("username", "password");
            var credential = new AuthTypeFilteredCredentials(unexpected, new[] { "basic", "somethirdthing" });

            // Act
            var actual = credential.GetCredential(new Uri("https://example.com/"), authType);

            // Assert
            Assert.Null(actual);
        }
    }
}

// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using NuGet.Test.Utility;
using Test.Utility;
using Xunit;

namespace NuGet.CommandLine.Test
{
    public class UILanguageOverrideTests
    {
        [Theory]
        [InlineData("en-US")]
        [InlineData("fr-FR")]
        [InlineData("pt-BR")]
        [InlineData("it-IT")]
        [InlineData("")] // Invariant culture
        [InlineData("cs")]
        public void GetOverriddenUILanguage_WithEnvironmentVariableSet_ReturnsCultureInfoWithSameName(string cultureName)
        {
            // Arrange
            var envVars = new Dictionary<string, string>()
            {
                { "NUGET_CLI_LANGUAGE", cultureName },
            };
            var envVarWrapper = new TestEnvironmentVariableReader(envVars);
            var logger = new TestLogger();

            // Act
            CultureInfo cultureInfo = UILanguageOverride.GetOverriddenUILanguage(envVarWrapper, logger);

            // Assert
            Assert.Equal(cultureName, cultureInfo.Name);
        }

        [Fact]
        public void Setup_NullArgument_Throws()
        {
            // Arrange, Act and Assert
            Assert.Throws<ArgumentNullException>(() => UILanguageOverride.Setup(logger: null));
        }

        [Fact]
        public void GetOverriddenUILanguage_WithInvalidValue_LogsErrorMessage()
        {
            // Arrange
            var envVars = new Dictionary<string, string>()
            {
                { "NUGET_CLI_LANGUAGE", "invalid-value" },
            };
            var envVarWrapper = new TestEnvironmentVariableReader(envVars);
            var logger = new TestLogger();

            // Act
            CultureInfo cultureInfo = UILanguageOverride.GetOverriddenUILanguage(envVarWrapper, logger);

            // Assert
            Assert.Null(cultureInfo);
            string errorMessage = string.Format(CultureInfo.InvariantCulture, NuGet.CommandLine.NuGetResources.Error_InvalidCultureInfo, "NUGET_CLI_LANGUAGE", "invalid-value");
            Assert.Collection(logger.Messages, msg => Assert.Equal(errorMessage, msg));
        }
    }
}

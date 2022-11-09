// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Moq;
using Xunit;

namespace NuGet.CommandLine.Test
{
    public class LocalizedResourceManagerTests
    {
        [Theory]
        [InlineData("A_String_With_No_Name")]
        [InlineData("An_unknown_String")]
        public void GetString_NonExistentResource_ReturnsNull(string stringResourceName)
        {
            // Arrange & Act
            string resource = LocalizedResourceManager.GetString(stringResourceName);

            // Assert
            Assert.Null(resource);
        }

        [Theory]
        [InlineData("SpecCommandCreatedNuSpec")]
        public void GetString_ExistingResourceInNuGetResources_ReturnsStringResource(string stringResourceName)
        {
            // Arrange & Act
            string resource = LocalizedResourceManager.GetString(stringResourceName);

            // Assert
            Assert.NotEmpty(resource);
        }

        [Theory]
        [InlineData("SpecCommandCreatedNuSpec", typeof(NuGetResources) )]
        [InlineData("UpdateCommandPrerelease", typeof(NuGetCommand))]
        public void GetString_ExistingResourcesInOtherResources_ReturnsStringResource(string resourceName, Type resourceType)
        {
            // Arrange
            PropertyInfo property = resourceType.GetProperty("ResourceManager", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            ResourceManager resourceManager = (ResourceManager)property.GetGetMethod(nonPublic: true).Invoke(obj: null, parameters: null);

            // Act
            string resource = LocalizedResourceManager.GetString(resourceName, resourceManager);

            // Assert
            Assert.NotEmpty(resource);
        }

        [Fact]
        public void GetString_ExistingResourceInNuGetResources_ReturnsSameValueFromResourceClass()
        {
            // Arrange & Act
            string resource = LocalizedResourceManager.GetString(nameof(NuGetResources.ListCommandNoPackages));

            // Assert
            Assert.Equal(NuGetResources.ListCommandNoPackages, resource);
        }

        [Fact]
        public void GetString_NullArgument_Throws()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentException>(() => LocalizedResourceManager.GetString(resourceName: null));
            Assert.Throws<ArgumentException>(() => LocalizedResourceManager.GetString(resourceName: null, resourceManager: null));
            Assert.Throws<ArgumentException>(() => LocalizedResourceManager.GetString(resourceName: "", resourceManager: It.IsAny<ResourceManager>()));
            Assert.Throws<ArgumentNullException>(() => LocalizedResourceManager.GetString(resourceName: "e", resourceManager: null));
        }

        [Theory]
        [InlineData("zh-Hant", "zh-Hant")] // Traditional Chinese
        [InlineData("zh-Hans", "zh-Hans")] // Simplified Chinese
        [InlineData("es", "es-hn")] // Spanish, Honduras
        [InlineData("es", "es-es")] // Spanish, Spain
        [InlineData("pt", "pt-Br")] // Portuguese, Brazil
        [InlineData("fr", "fr-fr")] // French, France
        [InlineData("fr", "fr-ca")] // French, Canada
        [InlineData("de", "de-de")] // Deutsch, Germany
        [InlineData("it", "it-it")] // Italian, Italy
        [InlineData("it", "it-ch")] // Italian, Switzerland
        [InlineData("pl", "pl-pl")] // Polish, Poland
        [InlineData("tr", "tr-tr")] // Turkish, Turkey
        [InlineData("tr", "tr")] // Turkish
        public void GetNeutralCulture_SupportedLocales_ReturnsExpectedLocale(string expectedLocale, string initialLocale)
        {
            Assert.Equal(new CultureInfo(expectedLocale), LocalizedResourceManager.GetNeutralCulture(new CultureInfo(initialLocale)));
        }

        [SkipMonoTheoryAttribute]
        [InlineData("cs", "cs-cs")] // Czech, Czech Republic
        [InlineData("ko", "ko-kr")] // Korean, Republic of Korea
        [InlineData("pt", "pt-pt")] // Portuguese, Portugal
        [InlineData("ja", "ja-jp")] // Japanese, Japan
        [InlineData("de", "de-be")] // Deutsch, Belgium
        [InlineData("ru", "ru-by")] // Russian, Belarus
        public void GetNeutralCulture_SupportedLocales_ReturnsExpectedLocaleInWindows(string expectedLocale, string initialLocale)
        {
            Assert.Equal(new CultureInfo(expectedLocale), LocalizedResourceManager.GetNeutralCulture(new CultureInfo(initialLocale)));
        }
    }
}

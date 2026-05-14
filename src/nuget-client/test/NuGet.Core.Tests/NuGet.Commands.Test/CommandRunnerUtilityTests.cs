// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Moq;
using NuGet.Configuration;
using NuGet.Test.Utility;
using Test.Utility;
using Xunit;

namespace NuGet.Commands.Test
{
    public class CommandRunnerUtilityTests
    {
        private readonly string _apikey = Guid.NewGuid().ToString();
        private const string EndpointUrl = "https://www.nuget.org/api/v2/package";

        [PlatformFact(Platform.Windows)]
        public void GetApiKey_ReturnsApiKeyForMatchingEndpointUrl_Success()
        {
            // Arrange
            string encryptedApiKey = EncryptionUtility.EncryptString(_apikey);

            var settings = new Mock<ISettings>(MockBehavior.Strict);
            settings.Setup(s => s.GetSection("apikeys"))
                                .Returns(new MockSettingSection("apikeys",
                                    new AddItem(EndpointUrl, encryptedApiKey)
                                ));

            //Act
            string apikey = CommandRunnerUtility.GetApiKey(settings.Object, EndpointUrl, NuGetConstants.V3FeedUrl);

            //Assert
            Assert.Equal(_apikey, apikey);
        }

        [PlatformFact(Platform.Windows)]
        public void GetApiKey_ReturnsApiKeyForMatchingPackageSourceUrl_Success()
        {
            // Arrange
            string encryptedApiKey = EncryptionUtility.EncryptString(_apikey);

            var settings = new Mock<ISettings>(MockBehavior.Strict);
            settings.Setup(s => s.GetSection("apikeys"))
                        .Returns(new MockSettingSection("apikeys",
                            new AddItem("http://endpointUrl", _apikey),//dummy endpoint url passed to ensure apikey is read from source url config entry
                            new AddItem(NuGetConstants.V3FeedUrl, encryptedApiKey)
                        ));

            //Act
            string apikey = CommandRunnerUtility.GetApiKey(settings.Object, EndpointUrl, NuGetConstants.V3FeedUrl);

            //Assert
            Assert.Equal(_apikey, apikey);
        }

        [PlatformFact(Platform.Windows)]
        public void GetApiKey_ReturnsDefaultGalleryServerUrlApiKeyIfSourceHostNameIsNuGetOrg_Success()
        {
            // Arrange
            string encryptedApiKey = EncryptionUtility.EncryptString(_apikey);

            var settings = new Mock<ISettings>(MockBehavior.Strict);
            settings.Setup(s => s.GetSection("apikeys"))
                        .Returns(new MockSettingSection("apikeys",
                            new AddItem("http://endpointUrl", _apikey),
                            new AddItem(NuGetConstants.DefaultGalleryServerUrl, encryptedApiKey)
                        ));

            //Act
            string apikey = CommandRunnerUtility.GetApiKey(settings.Object, EndpointUrl, NuGetConstants.V3FeedUrl);

            //Assert
            Assert.Equal(_apikey, apikey);
        }

        [PlatformFact(Platform.Windows)]
        public void GetApiKey_ReturnsNullWhenApiKeyIsNotFound_Success()
        {
            // Arrange
            string encryptedApiKey = EncryptionUtility.EncryptString(_apikey);

            var settings = new Mock<ISettings>(MockBehavior.Strict);
            settings.Setup(s => s.GetSection("apikeys"))
                        .Returns(new MockSettingSection("apikeys",
                            new AddItem("http://endpointUrl", encryptedApiKey),
                            new AddItem(NuGetConstants.DefaultGalleryServerUrl, encryptedApiKey),
                            new AddItem("https://sourceUrl", encryptedApiKey)
                        ));

            //Act
            string apikey = CommandRunnerUtility.GetApiKey(settings.Object, EndpointUrl, "https://someothersourceUrl");

            //Assert
            Assert.True(string.IsNullOrEmpty(apikey));
        }
    }
}

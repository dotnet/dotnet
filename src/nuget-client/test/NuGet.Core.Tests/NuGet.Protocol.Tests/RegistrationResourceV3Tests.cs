// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NuGet.Protocol.Core.Types;
using Test.Utility;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class RegistrationResourceV3Tests
    {
        [Theory]
        [InlineData("../contoso")]
        [InlineData("../contoso../?")]
        public void GetUri_CreateOrNull_ValidUriTemplate_ReturnsResource(string id)
        {
            var networkResponses = new Dictionary<string, string> { { "https://contoso", "network" } };
            var messageHandler = new TestMessageHandler(networkResponses, string.Empty);
            var handlerResource = new TestHttpHandler(messageHandler);
            var resource = new RegistrationResourceV3(
                new HttpSource(new Configuration.PackageSource("https://contoso"),
                () => Task.FromResult((HttpHandlerResource)handlerResource),
                new Mock<IThrottle>().Object),
                new System.Uri("https://contoso"));

            // Act & Assert
            var excetion = Assert.Throws<Packaging.InvalidPackageIdException>(() => resource.GetUri(id));
            excetion.Message.Should().Contain(string.Format(Strings.Error_Invalid_package_id, id));
        }
    }
}

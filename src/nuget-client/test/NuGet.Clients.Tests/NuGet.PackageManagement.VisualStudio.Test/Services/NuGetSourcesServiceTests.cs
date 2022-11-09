// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.ServiceHub.Framework;
using Microsoft.ServiceHub.Framework.Services;
using Moq;
using Xunit;

namespace NuGet.PackageManagement.VisualStudio.Test
{
    using ExceptionUtility = global::Test.Utility.ExceptionUtility;

    public class NuGetSourcesServiceTests
    {
        [Fact]
        public void Constructor_WhenServiceBrokerIsNull_Throws()
        {
            Exception exception = Assert.ThrowsAny<Exception>(
                () => new NuGetSourcesService(
                    default(ServiceActivationOptions),
                    serviceBroker: null,
                    new AuthorizationServiceClient(Mock.Of<IAuthorizationService>()),
                    Mock.Of<ISharedServiceState>()));

            ExceptionUtility.AssertMicrosoftAssumesException(exception);
        }

        [Fact]
        public void Constructor_WhenAuthorizationServiceClientIsNull_Throws()
        {
            Exception exception = Assert.ThrowsAny<Exception>(
                () => new NuGetSourcesService(
                    default(ServiceActivationOptions),
                    Mock.Of<IServiceBroker>(),
                    authorizationServiceClient: null,
                    Mock.Of<ISharedServiceState>()));

            ExceptionUtility.AssertMicrosoftAssumesException(exception);
        }

        [Fact]
        public void Constructor_WhenStateIsNull_Throws()
        {
            Exception exception = Assert.ThrowsAny<Exception>(
                () => new NuGetSourcesService(
                    default(ServiceActivationOptions),
                    Mock.Of<IServiceBroker>(),
                    new AuthorizationServiceClient(Mock.Of<IAuthorizationService>()),
                    state: null));

            ExceptionUtility.AssertMicrosoftAssumesException(exception);
        }
    }
}

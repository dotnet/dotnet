// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System;
using Moq;
using Xunit;

namespace Microsoft.IdentityModel.Abstractions.Tests
{
    /// <summary>
    /// This test suite is meant to act as a defense in depth to warn when someone changes the public interface which
    /// will be breaking.
    /// </summary>
    public class ITelemetryClientTests
    {
        [Fact]
        public void ValidatePublicContract()
        {
            // WARNING: Updating this code likely means you're breaking the public contract and should be avoided.
            var mockObject = new Mock<ITelemetryClient>().Object;
            mockObject.ClientId = "ClientId";
            mockObject.Initialize();
            mockObject.IsEnabled();
            mockObject.IsEnabled("fetch_metadata");
            mockObject.TrackEvent(new Mock<TelemetryEventDetails>().Object);
            mockObject.TrackEvent(
                "validate_token",
                new Dictionary<string, string>() { { "string", "value" } },
                new Dictionary<string, long>() { { "long", 1L } },
                new Dictionary<string, bool>() { { "bool", true } },
                new Dictionary<string, DateTime>() { { "DateTime", DateTime.UtcNow } },
                new Dictionary<string, double>() { { "double", 1.0d } },
                new Dictionary<string, Guid>() { { "Guid", Guid.NewGuid() } });

        }
    }
}

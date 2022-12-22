// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.VisualStudio.Telemetry;
using Xunit;

namespace NuGet.VisualStudio.Common.Test.Telemetry
{
    public class InstanceCloseTelemetryEmitterTests
    {
        [Fact]
        public void CreateTelemetryEvent_HasExpectedNameAndFaultsCount()
        {
            var telemetryEvent = InstanceCloseTelemetryEmitter.CreateTelemetryEvent();
            Assert.Equal("InstanceClose", telemetryEvent.Name);
            Assert.NotNull(telemetryEvent["faults.sessioncount"]);
            Assert.Equal(typeof(long), telemetryEvent["faults.sessioncount"].GetType());
        }
    }
}

// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Instrumentation.AspNet.Implementation;
using OpenTelemetry.Tests;
using Xunit;

namespace OpenTelemetry.Instrumentation.AspNet.Tests;

public class EventSourceTest
{
    [Fact]
    public void EventSourceTest_AspNetInstrumentationEventSource()
    {
        EventSourceTestHelper.MethodsAreImplementedConsistentlyWithTheirAttributes(AspNetInstrumentationEventSource.Log);
    }
}

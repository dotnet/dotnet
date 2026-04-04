// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Tests;
using Xunit;

namespace OpenTelemetry.PersistentStorage.Abstractions.Tests;

public class EventSourceTests
{
    [Fact]
    public void EventSourceTest_PersistentStorageEventSource()
    {
        EventSourceTestHelper.MethodsAreImplementedConsistentlyWithTheirAttributes(PersistentStorageAbstractionsEventSource.Log);
    }
}

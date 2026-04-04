// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Tests;
using Xunit;

namespace OpenTelemetry.PersistentStorage.FileSystem.Tests;

public class EventSourceTests
{
    [Fact]
    public void EventSourceTest_PersistentStorageEventSource()
    {
        EventSourceTestHelper.MethodsAreImplementedConsistentlyWithTheirAttributes(PersistentStorageEventSource.Log);
    }
}

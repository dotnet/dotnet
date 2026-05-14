// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Microsoft.TestPlatform.AcceptanceTests.Performance.PerfInstrumentation;

/// <summary>
/// The test platform task.
/// </summary>
public class TestPlatformTask
{
    public string TaskName { get; set; }

    public double EventStarted { get; set; }

    public double EventStopped { get; set; }

    public IDictionary<string, string> PayLoadProperties { get; set; }

    public TestPlatformTask(string taskName, double eventStarted)
    {
        EventStarted = eventStarted;
        TaskName = taskName;
        PayLoadProperties = new Dictionary<string, string>();
    }
}

public class TestPlatformEvent
{
    public TestPlatformEvent(string eventName, double timeSinceStart)
    {
        Name = eventName;
        TimeSinceStart = timeSinceStart;
    }

    public string Name { get; }
    public double TimeSinceStart { get; }
}

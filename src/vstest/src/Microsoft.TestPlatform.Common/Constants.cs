// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.Common;

/// <summary>
/// Defines the defaults used across different components.
/// </summary>
public static class TestPlatformDefaults
{
    /// <summary>
    /// string in the vstest.console.exe.config that specifies the bound on no of jobs in the job queue.
    /// </summary>
    public const string MaxNumberOfEventsLoggerEventQueueCanHold = "MaxNumberOfEventsLoggerEventQueueCanHold";

    /// <summary>
    /// Default bound on the job queue.
    /// </summary>
    public const int DefaultMaxNumberOfEventsLoggerEventQueueCanHold = 500;

    /// <summary>
    /// string in the vstest.console.exe.config that specifies the size bound on job queue.
    /// </summary>
    public const string MaxBytesLoggerEventQueueCanHold = "MaxBytesLoggerEventQueueCanHold";

    /// <summary>
    /// string in the rocksteady.exe.config that specifies whether or not we should try to keep memory usage by queue bounded.
    /// </summary>
    public const string EnableBoundsOnLoggerEventQueue = "EnableBoundsOnLoggerEventQueue";

    /// <summary>
    /// Default bound on the total size of all objects in the job queue. (25MB)
    /// </summary>
    public const int DefaultMaxBytesLoggerEventQueueCanHold = 25000000;

    /// <summary>
    /// Default value of the boolean that determines whether or not job queue should be bounded.
    /// </summary>
    public const bool DefaultEnableBoundsOnLoggerEventQueue = true;
}

/// <summary>
/// Defines the constants used across different components.
/// </summary>
public static class TestPlatformConstants
{
    /// <summary>
    /// Pattern used to find the test adapters library using String.EndWith
    /// </summary>
    public const string TestAdapterEndsWithPattern = @"TestAdapter.dll";

    /// <summary>
    /// Pattern used to find the test loggers library using String.EndWith
    /// </summary>
    public const string TestLoggerEndsWithPattern = @"TestLogger.dll";

    /// <summary>
    /// Pattern used to find the data collectors library using String.EndWith
    /// </summary>
    public const string DataCollectorEndsWithPattern = @"Collector.dll";

    /// <summary>
    /// Pattern used to find the run time providers library using String.EndWith
    /// </summary>
    public const string RunTimeEndsWithPattern = @"RuntimeProvider.dll";
}

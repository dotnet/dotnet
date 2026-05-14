// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.Serialization;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine.ClientProtocol;

/// <summary>
/// Stores information about test execution context.
/// </summary>
[DataContract]
public class TestExecutionContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestExecutionContext"/> class.
    /// </summary>
    /// <remarks>This constructor doesn't perform any parameter validation, it is meant to be used for serialization."/></remarks>
    public TestExecutionContext()
    {
        // Default constructor for Serialization.
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestExecutionContext"/> class.
    /// </summary>
    /// <param name="frequencyOfRunStatsChangeEvent">Frequency of run stats event.</param>
    /// <param name="runStatsChangeEventTimeout">Timeout that triggers sending results regardless of cache size.</param>
    /// <param name="inIsolation">Whether execution is out of process</param>
    /// <param name="keepAlive">Whether executor process should be kept running after test run completion</param>
    /// <param name="isDataCollectionEnabled">Whether data collection is enabled or not.</param>
    /// <param name="areTestCaseLevelEventsRequired">Indicates whether test case level events are required.</param>
    /// <param name="hasTestRun">True if ExecutionContext is associated with Test run, false otherwise.</param>
    /// <param name="isDebug">True if ExecutionContext needs debugger, false otherwise.</param>
    /// <param name="testCaseFilter">Filter criteria string for filtering tests.</param>
    /// <param name="filterOptions">Additional options for test case filter.</param>
    public TestExecutionContext(
        long frequencyOfRunStatsChangeEvent,
        TimeSpan runStatsChangeEventTimeout,
        bool inIsolation,
        bool keepAlive,
        bool isDataCollectionEnabled,
        bool areTestCaseLevelEventsRequired,
        bool hasTestRun,
        bool isDebug,
        string? testCaseFilter,
        FilterOptions? filterOptions)
    {
        FrequencyOfRunStatsChangeEvent = frequencyOfRunStatsChangeEvent;
        RunStatsChangeEventTimeout = runStatsChangeEventTimeout;
        InIsolation = inIsolation;
        KeepAlive = keepAlive;
        IsDataCollectionEnabled = isDataCollectionEnabled;
        AreTestCaseLevelEventsRequired = areTestCaseLevelEventsRequired;

        IsDebug = isDebug;

        HasTestRun = hasTestRun;
        TestCaseFilter = testCaseFilter;
        FilterOptions = filterOptions;
    }

    /// <summary>
    /// Gets or sets the frequency of run stats event.
    /// </summary>
    [DataMember]
    public long FrequencyOfRunStatsChangeEvent
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the timeout that triggers sending results regardless of cache size.
    /// </summary>
    [DataMember]
    public TimeSpan RunStatsChangeEventTimeout
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets a value indicating whether execution is out of process.
    /// </summary>
    [DataMember]
    public bool InIsolation
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets a value indicating whether executor process should be kept running after test run completion.
    /// </summary>
    [DataMember]
    public bool KeepAlive
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets a value indicating whether test case level events need to be sent or not
    /// </summary>
    [DataMember]
    public bool AreTestCaseLevelEventsRequired
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets a value indicating whether execution is in debug mode.
    /// </summary>
    [DataMember]
    public bool IsDebug
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the filter criteria for run with sources to filter test cases.
    /// </summary>
    [DataMember]
    public string? TestCaseFilter
    {
        get;
        set;
    }


    /// <summary>
    /// Gets or sets additional options for filtering.
    /// </summary>
    [DataMember]
    public FilterOptions? FilterOptions
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets a value indicating whether data collection is enabled or not.
    /// </summary>
    /// <remarks>This does not need to be serialized over to the test host process.</remarks>
    [IgnoreDataMember]
    public bool IsDataCollectionEnabled
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets a value indicating whether execution context is associated with a test run.
    /// </summary>
    [IgnoreDataMember]
    public bool HasTestRun
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets a configuration associated with this run.
    /// </summary>
    /// <remarks>It is not serialized over <c>wcf </c> as the information is available in the run settings</remarks>
    [IgnoreDataMember]
    public RunConfiguration? TestRunConfiguration
    {
        get;
        set;
    }

}

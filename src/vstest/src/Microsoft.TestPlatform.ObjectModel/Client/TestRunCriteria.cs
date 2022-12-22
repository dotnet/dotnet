// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using Microsoft.VisualStudio.TestPlatform.CoreUtilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Interfaces;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

/// <summary>
/// Defines the test run criterion.
/// </summary>
public class TestRunCriteria : BaseTestRunCriteria, ITestRunConfiguration
{
    private string? _testCaseFilter;
    private FilterOptions? _filterOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestRunCriteria"/> class.
    /// </summary>
    ///
    /// <param name="sources">Sources which contains tests that should be executed.</param>
    /// <param name="frequencyOfRunStatsChangeEvent">Frequency of run stats event.</param>
    public TestRunCriteria(
        IEnumerable<string> sources,
        long frequencyOfRunStatsChangeEvent)
        : this(
            sources,
            frequencyOfRunStatsChangeEvent,
            keepAlive: true)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestRunCriteria"/> class.
    /// </summary>
    ///
    /// <param name="sources">Sources which contains tests that should be executed.</param>
    /// <param name="frequencyOfRunStatsChangeEvent">Frequency of run stats event.</param>
    /// <param name="keepAlive">
    /// Whether the execution process should be kept alive after the run is finished or not.
    /// </param>
    public TestRunCriteria(
        IEnumerable<string> sources,
        long frequencyOfRunStatsChangeEvent,
        bool keepAlive)
        : this(
            sources,
            frequencyOfRunStatsChangeEvent,
            keepAlive,
            testSettings: string.Empty)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestRunCriteria"/> class.
    /// </summary>
    ///
    /// <param name="sources">Sources which contains tests that should be executed.</param>
    /// <param name="frequencyOfRunStatsChangeEvent">Frequency of run stats event.</param>
    /// <param name="keepAlive">
    /// Whether the execution process should be kept alive after the run is finished or not.
    /// </param>
    /// <param name="testSettings">Settings used for this run.</param>
    public TestRunCriteria(
        IEnumerable<string> sources,
        long frequencyOfRunStatsChangeEvent,
        bool keepAlive,
        string? testSettings)
        : this(
            sources,
            frequencyOfRunStatsChangeEvent,
            keepAlive,
            testSettings,
            TimeSpan.MaxValue)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestRunCriteria"/> class.
    /// </summary>
    ///
    /// <param name="sources">Sources which contains tests that should be executed.</param>
    /// <param name="frequencyOfRunStatsChangeEvent">Frequency of run stats event.</param>
    /// <param name="keepAlive">
    /// Whether the execution process should be kept alive after the run is finished or not.
    /// </param>
    /// <param name="testSettings">Settings used for this run.</param>
    /// <param name="runStatsChangeEventTimeout">
    /// Timeout that triggers sending results regardless of cache size.
    /// </param>
    public TestRunCriteria(
        IEnumerable<string> sources,
        long frequencyOfRunStatsChangeEvent,
        bool keepAlive,
        string? testSettings,
        TimeSpan runStatsChangeEventTimeout)
        : this(
            sources,
            frequencyOfRunStatsChangeEvent,
            keepAlive,
            testSettings,
            runStatsChangeEventTimeout,
            testHostLauncher: null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestRunCriteria"/> class.
    /// </summary>
    ///
    /// <param name="sources">Sources which contains tests that should be executed.</param>
    /// <param name="frequencyOfRunStatsChangeEvent">Frequency of run stats event.</param>
    /// <param name="keepAlive">
    /// Whether the execution process should be kept alive after the run is finished or not.
    /// </param>
    /// <param name="testSettings">Settings used for this run.</param>
    /// <param name="runStatsChangeEventTimeout">
    /// Timeout that triggers sending results regardless of cache size.
    /// </param>
    /// <param name="testHostLauncher">
    /// Test host launcher. If null then default will be used.
    /// </param>
    public TestRunCriteria(
        IEnumerable<string> sources,
        long frequencyOfRunStatsChangeEvent,
        bool keepAlive,
        string? testSettings,
        TimeSpan runStatsChangeEventTimeout,
        ITestHostLauncher? testHostLauncher)
        : this(
            sources,
            frequencyOfRunStatsChangeEvent,
            keepAlive,
            testSettings,
            runStatsChangeEventTimeout,
            testHostLauncher,
            testCaseFilter: null,
            filterOptions: null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestRunCriteria"/> class.
    /// </summary>
    ///
    /// <param name="sources">Sources which contains tests that should be executed.</param>
    /// <param name="frequencyOfRunStatsChangeEvent">Frequency of run stats event.</param>
    /// <param name="keepAlive">
    /// Whether the execution process should be kept alive after the run is finished or not.
    /// </param>
    /// <param name="testSettings">Settings used for this run.</param>
    /// <param name="runStatsChangeEventTimeout">
    /// Timeout that triggers sending results regardless of cache size.
    /// </param>
    /// <param name="testHostLauncher">
    /// Test host launcher. If null then default will be used.
    /// </param>
    /// <param name="testCaseFilter">Test case filter.</param>
    /// <param name="filterOptions">Filter options.</param>
    public TestRunCriteria(
        IEnumerable<string> sources,
        long frequencyOfRunStatsChangeEvent,
        bool keepAlive,
        string? testSettings,
        TimeSpan runStatsChangeEventTimeout,
        ITestHostLauncher? testHostLauncher,
        string? testCaseFilter,
        FilterOptions? filterOptions)
        : this(
            sources,
            frequencyOfRunStatsChangeEvent,
            keepAlive,
            testSettings,
            runStatsChangeEventTimeout,
            testHostLauncher,
            testCaseFilter,
            filterOptions,
            testSessionInfo: null,
            debugEnabledForTestSession: false)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestRunCriteria"/> class.
    /// </summary>
    ///
    /// <param name="sources">Sources which contains tests that should be executed.</param>
    /// <param name="frequencyOfRunStatsChangeEvent">Frequency of run stats event.</param>
    /// <param name="keepAlive">
    /// Whether the execution process should be kept alive after the run is finished or not.
    /// </param>
    /// <param name="testSettings">Settings used for this run.</param>
    /// <param name="runStatsChangeEventTimeout">
    /// Timeout that triggers sending results regardless of cache size.
    /// </param>
    /// <param name="testHostLauncher">
    /// Test host launcher. If null then default will be used.
    /// </param>
    /// <param name="testCaseFilter">Test case filter.</param>
    /// <param name="filterOptions">Filter options.</param>
    /// <param name="testSessionInfo">The test session info object.</param>
    /// <param name="debugEnabledForTestSession">
    /// Indicates if debugging should be enabled when we have test session info available.
    /// </param>
    public TestRunCriteria(
        IEnumerable<string> sources,
        long frequencyOfRunStatsChangeEvent,
        bool keepAlive,
        string? testSettings,
        TimeSpan runStatsChangeEventTimeout,
        ITestHostLauncher? testHostLauncher,
        string? testCaseFilter,
        FilterOptions? filterOptions,
        TestSessionInfo? testSessionInfo,
        bool debugEnabledForTestSession)
        : base(
            frequencyOfRunStatsChangeEvent,
            keepAlive,
            testSettings,
            runStatsChangeEventTimeout,
            testHostLauncher)
    {
        var testSources = sources as IList<string> ?? sources.ToList();
        ValidateArg.NotNullOrEmpty(testSources, nameof(sources));

        AdapterSourceMap = new Dictionary<string, IEnumerable<string>>
        {
            { Constants.UnspecifiedAdapterPath, testSources }
        };

        TestCaseFilter = testCaseFilter;
        FilterOptions = filterOptions;

        TestSessionInfo = testSessionInfo;
        DebugEnabledForTestSession = debugEnabledForTestSession;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestRunCriteria"/> class.
    /// Create the TestRunCriteria for a test run.
    /// </summary>
    ///
    /// <param name="sources">Sources which contains tests that should be executed.</param>
    /// <param name="testRunCriteria">The test run criteria.</param>
    public TestRunCriteria(
        IEnumerable<string> sources,
        TestRunCriteria testRunCriteria)
        : base(testRunCriteria)
    {
        var testSources = sources as IList<string> ?? sources.ToArray();
        ValidateArg.NotNullOrEmpty(testSources, nameof(sources));

        AdapterSourceMap = new Dictionary<string, IEnumerable<string>>
        {
            { Constants.UnspecifiedAdapterPath, testSources }
        };

        TestCaseFilter = testRunCriteria._testCaseFilter;
        FilterOptions = testRunCriteria._filterOptions;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestRunCriteria"/> class.
    /// </summary>
    ///
    /// <param name="adapterSourceMap">
    /// Sources which contains tests that should be executed.
    /// </param>
    /// <param name="frequencyOfRunStatsChangeEvent">Frequency of run stats event.</param>
    /// <param name="keepAlive">
    /// Whether the execution process should be kept alive after the run is finished or not.
    /// </param>
    /// <param name="testSettings">Settings used for this run.</param>
    /// <param name="runStatsChangeEventTimeout">
    /// Timeout that triggers sending results regardless of cache size.
    /// </param>
    /// <param name="testHostLauncher">
    /// Test host launcher. If null then default will be used.
    /// </param>
    public TestRunCriteria(
        Dictionary<string, IEnumerable<string>> adapterSourceMap,
        long frequencyOfRunStatsChangeEvent,
        bool keepAlive,
        string? testSettings,
        TimeSpan runStatsChangeEventTimeout,
        ITestHostLauncher? testHostLauncher)
        : base(
            frequencyOfRunStatsChangeEvent,
            keepAlive,
            testSettings,
            runStatsChangeEventTimeout,
            testHostLauncher)
    {
        ValidateArg.NotNullOrEmpty(adapterSourceMap, nameof(adapterSourceMap));

        AdapterSourceMap = adapterSourceMap;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestRunCriteria"/> class.
    /// </summary>
    ///
    /// <param name="tests">Tests which should be executed.</param>
    /// <param name="frequencyOfRunStatsChangeEvent">Frequency of run stats event.</param>
    public TestRunCriteria(
        IEnumerable<TestCase> tests,
        long frequencyOfRunStatsChangeEvent)
        : this(
            tests,
            frequencyOfRunStatsChangeEvent,
            false)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestRunCriteria"/> class.
    /// </summary>
    ///
    /// <param name="tests">Tests which should be executed.</param>
    /// <param name="frequencyOfRunStatsChangeEvent">Frequency of run stats event.</param>
    /// <param name="keepAlive">
    /// Whether or not to keep the test executor process alive after run completion.
    /// </param>
    public TestRunCriteria(
        IEnumerable<TestCase> tests,
        long frequencyOfRunStatsChangeEvent,
        bool keepAlive)
        : this(
            tests,
            frequencyOfRunStatsChangeEvent,
            keepAlive,
            string.Empty)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestRunCriteria"/> class.
    /// </summary>
    ///
    /// <param name="tests">Tests which should be executed.</param>
    /// <param name="frequencyOfRunStatsChangeEvent">Frequency of run stats event.</param>
    /// <param name="keepAlive">
    /// Whether or not to keep the test executor process alive after run completion.
    /// </param>
    /// <param name="testSettings">Settings used for this run.</param>
    public TestRunCriteria(
        IEnumerable<TestCase> tests,
        long frequencyOfRunStatsChangeEvent,
        bool keepAlive,
        string testSettings)
        : this(
            tests,
            frequencyOfRunStatsChangeEvent,
            keepAlive,
            testSettings,
            TimeSpan.MaxValue)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestRunCriteria"/> class.
    /// </summary>
    ///
    /// <param name="tests">Tests which should be executed.</param>
    /// <param name="frequencyOfRunStatsChangeEvent">Frequency of run stats event.</param>
    /// <param name="keepAlive">
    /// Whether or not to keep the test executor process alive after run completion.
    /// </param>
    /// <param name="testSettings">Settings used for this run.</param>
    /// <param name="runStatsChangeEventTimeout">
    /// Timeout that triggers sending results regardless of cache size.
    /// </param>
    public TestRunCriteria(
        IEnumerable<TestCase> tests,
        long frequencyOfRunStatsChangeEvent,
        bool keepAlive,
        string testSettings,
        TimeSpan runStatsChangeEventTimeout)
        : this(
            tests,
            frequencyOfRunStatsChangeEvent,
            keepAlive,
            testSettings,
            runStatsChangeEventTimeout,
            null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestRunCriteria"/> class.
    /// </summary>
    ///
    /// <param name="tests">Tests which should be executed.</param>
    /// <param name="baseTestRunCriteria">The base test run criteria.</param>
    public TestRunCriteria(IEnumerable<TestCase> tests, BaseTestRunCriteria baseTestRunCriteria)
        : base(baseTestRunCriteria)
    {
        var testCases = tests as IList<TestCase> ?? tests.ToList();
        ValidateArg.NotNullOrEmpty(testCases, nameof(tests));

        Tests = testCases;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestRunCriteria"/> class.
    /// </summary>
    ///
    /// <param name="tests">Tests which should be executed.</param>
    /// <param name="frequencyOfRunStatsChangeEvent">Frequency of run stats event.</param>
    /// <param name="keepAlive">
    /// Whether or not to keep the test executor process alive after run completion.
    /// </param>
    /// <param name="testSettings">Settings used for this run.</param>
    /// <param name="runStatsChangeEventTimeout">
    /// Timeout that triggers sending results regardless of cache size.
    /// </param>
    /// <param name="testHostLauncher">
    /// Test host launcher. If null then default will be used.
    /// </param>
    public TestRunCriteria(
        IEnumerable<TestCase> tests,
        long frequencyOfRunStatsChangeEvent,
        bool keepAlive,
        string testSettings,
        TimeSpan runStatsChangeEventTimeout,
        ITestHostLauncher? testHostLauncher)
        : this(
            tests,
            frequencyOfRunStatsChangeEvent,
            keepAlive,
            testSettings,
            runStatsChangeEventTimeout,
            testHostLauncher,
            testSessionInfo: null,
            debugEnabledForTestSession: false)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestRunCriteria"/> class.
    /// </summary>
    ///
    /// <param name="tests">Tests which should be executed.</param>
    /// <param name="frequencyOfRunStatsChangeEvent">Frequency of run stats event.</param>
    /// <param name="keepAlive">
    /// Whether or not to keep the test executor process alive after run completion.
    /// </param>
    /// <param name="testSettings">Settings used for this run.</param>
    /// <param name="runStatsChangeEventTimeout">
    /// Timeout that triggers sending results regardless of cache size.
    /// </param>
    /// <param name="testHostLauncher">
    /// Test host launcher. If null then default will be used.
    /// </param>
    /// <param name="testSessionInfo">The test session info object.</param>
    /// <param name="debugEnabledForTestSession">
    /// Indicates if debugging should be enabled when we have test session info available.
    /// </param>
    public TestRunCriteria(
        IEnumerable<TestCase> tests,
        long frequencyOfRunStatsChangeEvent,
        bool keepAlive,
        string? testSettings,
        TimeSpan runStatsChangeEventTimeout,
        ITestHostLauncher? testHostLauncher,
        TestSessionInfo? testSessionInfo,
        bool debugEnabledForTestSession)
        : base(
            frequencyOfRunStatsChangeEvent,
            keepAlive,
            testSettings,
            runStatsChangeEventTimeout,
            testHostLauncher)
    {
        var testCases = tests as IList<TestCase> ?? tests?.ToList();
        ValidateArg.NotNullOrEmpty(testCases, nameof(tests));

        Tests = testCases;
        TestSessionInfo = testSessionInfo;
        DebugEnabledForTestSession = debugEnabledForTestSession;
    }

    /// <summary>
    /// Gets the test containers (e.g. DLL/EXE/artifacts to scan).
    /// </summary>
    [IgnoreDataMember]
    public IEnumerable<string>? Sources
    {
        get
        {
            IEnumerable<string> sources = new List<string>();
            return AdapterSourceMap?.Values?.Aggregate(
                sources,
                (current, enumerable) => current.Concat(enumerable));
        }
    }

    /// <summary>
    /// Gets the test adapter and source map which would look like below:
    /// <code>
    /// { C:\temp\testAdapter1.dll : [ source1.dll, source2.dll ], C:\temp\testadapter2.dll : [ source3.dll, source2.dll ]
    /// </code>
    /// </summary>
    [DataMember]
    public Dictionary<string, IEnumerable<string>>? AdapterSourceMap { get; private set; }

    /// <summary>
    /// Gets the tests that need to executed in this test run.
    /// This will be null if test run is created with specific test containers.
    /// </summary>
    [DataMember]
    public IEnumerable<TestCase>? Tests { get; private set; }

    /// <summary>
    /// Gets or sets the criteria for filtering test cases.
    /// </summary>
    /// <remarks>This is only for with sources.</remarks>
    [DataMember]
    public string? TestCaseFilter
    {
        get => _testCaseFilter;

        private set
        {
            if (value != null && !HasSpecificSources)
            {
                throw new InvalidOperationException(Resources.Resources.NoTestCaseFilterForSpecificTests);
            }

            _testCaseFilter = value;
        }
    }

    /// <summary>
    /// Gets or sets the filter options.
    /// </summary>
    /// <remarks>This is only applicable when TestCaseFilter is present.</remarks>
    [DataMember]
    public FilterOptions? FilterOptions
    {
        get => _filterOptions;

        private set
        {
            if (value != null && !HasSpecificSources)
            {
                throw new InvalidOperationException(Resources.Resources.NoTestCaseFilterForSpecificTests);
            }

            _filterOptions = value;
        }
    }

    /// <summary>
    /// Gets or sets the test session info object.
    /// </summary>
    public TestSessionInfo? TestSessionInfo { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating if debugging should be enabled when we have test session
    /// info available.
    /// </summary>
    public bool DebugEnabledForTestSession { get; set; }

    /// <summary>
    /// Gets a value indicating whether run criteria is based on specific tests.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Tests))]
    [MemberNotNullWhen(false, nameof(Sources))]
    public bool HasSpecificTests
    {
        get
        {
            if (Tests != null)
            {
                return true;
            }

            TPDebug.Assert(Sources is not null, "Sources is null and Tests is null");
            return false;
        }
    }

    /// <summary>
    /// Gets a value indicating whether run criteria is based on specific sources.
    /// </summary>
    [DataMember]
    [MemberNotNullWhen(true, nameof(Sources))]
    [MemberNotNullWhen(false, nameof(Tests))]
    public bool HasSpecificSources
    {
        get
        {
            if (Sources != null)
            {
                return true;
            }

            TPDebug.Assert(Tests is not null, "Tests is null and Sources is null");
            return false;
        }
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        StringBuilder sb = new();
        sb.AppendLine(string.Format(CultureInfo.CurrentCulture, "TestRunCriteria:"));
        sb.AppendLine(string.Format(
            CultureInfo.CurrentCulture,
            "   KeepAlive={0},FrequencyOfRunStatsChangeEvent={1},RunStatsChangeEventTimeout={2},TestCaseFilter={3},TestExecutorLauncher={4}",
            KeepAlive,
            FrequencyOfRunStatsChangeEvent,
            RunStatsChangeEventTimeout,
            TestCaseFilter,
            TestHostLauncher));
        sb.AppendLine(string.Format(CultureInfo.CurrentCulture, "   Settingsxml={0}", TestRunSettings));

        return sb.ToString();
    }

    protected bool Equals(TestRunCriteria? other)
        => other is not null
            && base.Equals(other)
            && string.Equals(TestCaseFilter, other.TestCaseFilter)
            && Equals(FilterOptions, other.FilterOptions);

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return obj.GetType() == GetType() && Equals((TestRunCriteria)obj);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = base.GetHashCode();
            hashCode = (hashCode * 397) ^ (_testCaseFilter != null ? _testCaseFilter.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (AdapterSourceMap != null ? AdapterSourceMap.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (Tests != null ? Tests.GetHashCode() : 0);
            return hashCode;
        }
    }
}

/// <summary>
/// Defines the base test run criterion.
/// </summary>
public class BaseTestRunCriteria
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseTestRunCriteria"/> class.
    /// </summary>
    ///
    /// <param name="runCriteria">Run criteria to clone.</param>
    public BaseTestRunCriteria(BaseTestRunCriteria runCriteria)
    {
        ValidateArg.NotNull(runCriteria, nameof(runCriteria));
        FrequencyOfRunStatsChangeEvent = runCriteria.FrequencyOfRunStatsChangeEvent;
        KeepAlive = runCriteria.KeepAlive;
        TestRunSettings = runCriteria.TestRunSettings;
        RunStatsChangeEventTimeout = runCriteria.RunStatsChangeEventTimeout;
        TestHostLauncher = runCriteria.TestHostLauncher;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseTestRunCriteria"/> class.
    /// </summary>
    ///
    /// <param name="frequencyOfRunStatsChangeEvent">
    /// Frequency of <c>TestRunChangedEvent</c>.
    /// </param>
    public BaseTestRunCriteria(long frequencyOfRunStatsChangeEvent)
        : this(frequencyOfRunStatsChangeEvent, true)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseTestRunCriteria"/> class.
    /// </summary>
    ///
    /// <param name="frequencyOfRunStatsChangeEvent">
    /// Frequency of <c>TestRunChangedEvent</c>.
    /// </param>
    /// <param name="keepAlive">
    /// Specify if the test host process should be stay alive after run.
    /// </param>
    public BaseTestRunCriteria(long frequencyOfRunStatsChangeEvent, bool keepAlive)
        : this(frequencyOfRunStatsChangeEvent, keepAlive, string.Empty)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseTestRunCriteria"/> class.
    /// </summary>
    ///
    /// <param name="frequencyOfRunStatsChangeEvent">
    /// Frequency of <c>TestRunChangedEvent</c>.
    /// </param>
    /// <param name="keepAlive">
    /// Specify if the test host process should be stay alive after run.
    /// </param>
    /// <param name="testSettings">Test run settings.</param>
    public BaseTestRunCriteria(
        long frequencyOfRunStatsChangeEvent,
        bool keepAlive,
        string? testSettings)
        : this(
            frequencyOfRunStatsChangeEvent,
            keepAlive,
            testSettings,
            TimeSpan.MaxValue)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseTestRunCriteria"/> class.
    /// </summary>
    ///
    /// <param name="frequencyOfRunStatsChangeEvent">
    /// Frequency of <c>TestRunChangedEvent</c>.
    /// </param>
    /// <param name="keepAlive">
    /// Specify if the test host process should be stay alive after run.
    /// </param>
    /// <param name="testSettings">Test run settings.</param>
    /// <param name="runStatsChangeEventTimeout">
    /// Timeout for a <c>TestRunChangedEvent</c>.
    /// </param>
    public BaseTestRunCriteria(
        long frequencyOfRunStatsChangeEvent,
        bool keepAlive,
        string? testSettings,
        TimeSpan runStatsChangeEventTimeout)
        : this(
            frequencyOfRunStatsChangeEvent,
            keepAlive,
            testSettings,
            runStatsChangeEventTimeout,
            null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseTestRunCriteria"/> class.
    /// </summary>
    ///
    /// <param name="frequencyOfRunStatsChangeEvent">
    /// Frequency of <c>TestRunChangedEvent</c>.
    /// </param>
    /// <param name="keepAlive">
    /// Specify if the test host process should be stay alive after run.
    /// </param>
    /// <param name="testSettings">Test run settings.</param>
    /// <param name="runStatsChangeEventTimeout">
    /// Timeout for a <c>TestRunChangedEvent</c>.
    /// </param>
    /// <param name="testHostLauncher">Test host launcher.</param>
    public BaseTestRunCriteria(
        long frequencyOfRunStatsChangeEvent,
        bool keepAlive,
        string? testSettings,
        TimeSpan runStatsChangeEventTimeout,
        ITestHostLauncher? testHostLauncher)
    {
        if (frequencyOfRunStatsChangeEvent <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(frequencyOfRunStatsChangeEvent),
                Resources.Resources.NotificationFrequencyIsNotPositive);
        }

        if (runStatsChangeEventTimeout <= TimeSpan.MinValue)
        {
            throw new ArgumentOutOfRangeException(
                nameof(runStatsChangeEventTimeout),
                Resources.Resources.NotificationTimeoutIsZero);
        }

        FrequencyOfRunStatsChangeEvent = frequencyOfRunStatsChangeEvent;
        KeepAlive = keepAlive;
        TestRunSettings = testSettings;
        RunStatsChangeEventTimeout = runStatsChangeEventTimeout;
        TestHostLauncher = testHostLauncher;
    }

    /// <summary>
    /// Gets a value indicating whether the test executor process should remain alive after
    /// run completion.
    /// </summary>
    [DataMember]
    public bool KeepAlive { get; private set; }

    /// <summary>
    /// Gets the settings used for this run.
    /// </summary>
    [DataMember]
    public string? TestRunSettings { get; private set; }

    /// <summary>
    /// Gets the custom launcher for test executor.
    /// </summary>
    [DataMember]
    public ITestHostLauncher? TestHostLauncher { get; private set; }

    /// <summary>
    /// Gets the frequency of run stats test event.
    /// </summary>
    ///
    /// <remarks>
    /// Run stats change event will be raised after completion of these number of tests.
    /// Note that this event is raised asynchronously and the underlying execution process is not
    /// paused during the listener invocation. So if the event handler, you try to query the
    /// next set of results, you may get more than 'FrequencyOfRunStatsChangeEvent'.
    /// </remarks>
    [DataMember]
    public long FrequencyOfRunStatsChangeEvent { get; private set; }

    /// <summary>
    /// Gets the timeout that triggers sending results regardless of cache size.
    /// </summary>
    [DataMember]
    public TimeSpan RunStatsChangeEventTimeout { get; private set; }

    protected bool Equals(BaseTestRunCriteria? other)
        => other is not null
            && KeepAlive == other.KeepAlive
            && string.Equals(TestRunSettings, other.TestRunSettings)
            && FrequencyOfRunStatsChangeEvent == other.FrequencyOfRunStatsChangeEvent
            && RunStatsChangeEventTimeout.Equals(other.RunStatsChangeEventTimeout);

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return obj.GetType() == GetType() && Equals((BaseTestRunCriteria)obj);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = KeepAlive.GetHashCode();
            hashCode = (hashCode * 397) ^ (TestRunSettings != null ? TestRunSettings.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ FrequencyOfRunStatsChangeEvent.GetHashCode();
            hashCode = (hashCode * 397) ^ RunStatsChangeEventTimeout.GetHashCode();
            return hashCode;
        }
    }
}

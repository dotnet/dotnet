// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.IO;

using System;
using System.Linq;

using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Session;
using System.Globalization;

namespace Microsoft.TestPlatform.PerformanceTests.PerfInstrumentation;

/// <summary>
/// The performance analyzer.
/// </summary>
public class PerfAnalyzer
{
    /// <summary>
    /// The etw session provider name.
    /// </summary>
    private const string EtwSessionProviderName = "TestPlatform";

    private readonly string _perfDataFileName;
    private readonly TraceEventSession _traceEventSession;
    private readonly Dictionary<string, List<TestPlatformTask>> _testPlatformTaskMap;

    /// <summary>
    /// Initializes a new instance of the <see cref="PerfAnalyzer"/> class.
    /// </summary>
    public PerfAnalyzer()
    {
        // It is important to make the file and session name random,
        // otherwise there are a lot of errors from wmi that are "random".
        var name = $"TestPlatform{Guid.NewGuid()}";
        _perfDataFileName = $"{name}.etl";
        _testPlatformTaskMap = new Dictionary<string, List<TestPlatformTask>>();
        _traceEventSession = new TraceEventSession(name, _perfDataFileName);
    }

    /// <summary>
    /// Returns disposable wrapper that starts and stops performance tracing, and analyzes the data up on dispose.
    /// </summary>
    /// <returns></returns>
    public PerfTracker Start()
    {
        return new PerfTracker(this);
    }

    public List<TestPlatformEvent> Events { get; } = new();

    /// <summary>
    /// The enable provider.
    /// </summary>
    private void EnableProvider()
    {
        _traceEventSession.StopOnDispose = true;
        _traceEventSession.EnableProvider(EtwSessionProviderName);
    }

    /// <summary>
    /// The disable provider.
    /// </summary>
    private void DisableProvider()
    {
        Console.WriteLine($"Lost events: {_traceEventSession.EventsLost}");
        _traceEventSession.Flush();
        _traceEventSession.Dispose();
    }

    /// <summary>
    /// The analyze events data.
    /// </summary>
    private void AnalyzeEventsData()
    {
        using var source = new ETWTraceEventSource(_perfDataFileName);
        // Open the file
        var parser = new DynamicTraceEventParser(source);
        parser.All += delegate (TraceEvent data)
        {
            try
            {
                if (data.ProviderName.Equals("TestPlatform") && !data.EventName.Equals("ManifestData"))
                {
                    Console.WriteLine("Received Event : {0}", data.ToString(CultureInfo.CurrentCulture));
                    var key = $"{data.ProcessID}_{data.ThreadID}_{data.TaskName}";
                    Events.Add(new TestPlatformEvent(data.EventName, data.TimeStampRelativeMSec));

                    if (!_testPlatformTaskMap.ContainsKey(key))
                    {
                        var list = new List<TestPlatformTask> { CreateTestPlatformTask(data) };
                        _testPlatformTaskMap.Add(key, list);
                    }
                    else
                    {
                        if (data.Opcode == TraceEventOpcode.Start)
                        {
                            _testPlatformTaskMap[key].Add(CreateTestPlatformTask(data));
                        }
                        else
                        {
                            UpdateTask(_testPlatformTaskMap[key].Last(), data);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        };
        source.Process(); // Read the file, processing the callbacks.

        source.StopProcessing();
        File.Delete(_perfDataFileName);
    }

    /// <summary>
    /// The get elapsed time by task name.
    /// </summary>
    /// <param name="taskName">
    /// The task name.
    /// </param>
    /// <returns>
    /// The <see cref="double"/>.
    /// </returns>
    public double GetElapsedTimeByTaskName(string taskName)
    {
        var timeTaken = 0.0;
        var key = GetEventKey(taskName);

        if (key != null)
        {
            var task = _testPlatformTaskMap[key].First();
            timeTaken = task.EventStopped - task.EventStarted;
        }
        return timeTaken;
    }

    /// <summary>
    /// The get event data by task name.
    /// </summary>
    /// <param name="taskName">
    /// The task name.
    /// </param>
    /// <returns>
    /// The <see cref="IDictionary"/>.
    /// </returns>
    public IDictionary<string, string> GetEventDataByTaskName(string taskName)
    {
        IDictionary<string, string> properties = new Dictionary<string, string>();
        var key = GetEventKey(taskName);

        if (key != null)
        {
            properties = _testPlatformTaskMap[key].First().PayLoadProperties;
        }

        return properties;
    }

    public double GetAdapterExecutionTime(string executorUri)
    {
        var timeTaken = 0.0;
        var key = GetEventKey(Constants.AdapterExecutionTask);

        if (key != null)
        {
            var task = _testPlatformTaskMap[key].Find(t => t.PayLoadProperties["executorUri"].Equals(executorUri));
            timeTaken = task.EventStopped - task.EventStarted;
        }

        return timeTaken;
    }

    public long GetAdapterExecutedTests(string executorUri)
    {
        long totalTestsExecuted = 0;
        var key = GetEventKey(Constants.AdapterExecutionTask);

        if (key != null)
        {
            var task = _testPlatformTaskMap[key].Find(t => t.PayLoadProperties["executorUri"].Equals(executorUri));
            _ = long.TryParse(task.PayLoadProperties["numberOfTests"], out totalTestsExecuted);
        }

        return totalTestsExecuted;
    }


    private string? GetEventKey(string taskName)
    {
        string? key = null;

        key = _testPlatformTaskMap.Keys.FirstOrDefault(k => k.Split('_')[2].Equals(taskName));

        return key;
    }

    private static TestPlatformTask CreateTestPlatformTask(TraceEvent data)
    {
        var task = new TestPlatformTask(data.TaskName, data.TimeStampRelativeMSec);
        task.PayLoadProperties = GetPayloadProperties(data);
        return task;
    }

    private static void UpdateTask(TestPlatformTask task, TraceEvent data)
    {
        task.EventStopped = data.TimeStampRelativeMSec;
        var payLoadProperties = GetPayloadProperties(data);

        //Merging dictionaries look for better way
        foreach (var k in payLoadProperties.Keys)
        {
            if (!task.PayLoadProperties.ContainsKey(k))
            {
                task.PayLoadProperties.Add(k, payLoadProperties[k]);
            }
        }
    }

    private static IDictionary<string, string> GetPayloadProperties(TraceEvent data)
    {
        var payLoadProperties = new Dictionary<string, string>();

        foreach (var payLoad in data.PayloadNames)
        {
            var payLoadData = data.PayloadByName(payLoad).ToString();
            if (!payLoadProperties.ContainsKey(payLoad))
            {
                payLoadProperties.Add(payLoad, payLoadData);
            }
            else
            {
                payLoadProperties[payLoad] = payLoadData;
            }
        }

        return payLoadProperties;
    }

    public class PerfTracker : IDisposable
    {
        private readonly PerfAnalyzer _perfAnalyzer;
        private bool _isDisposed;

        public PerfTracker(PerfAnalyzer perfAnalyzer)
        {
            _perfAnalyzer = perfAnalyzer;
            _perfAnalyzer.EnableProvider();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            if (disposing)
            {
                _perfAnalyzer.DisableProvider();
                _perfAnalyzer.AnalyzeEventsData();
            }

            _isDisposed = true;
        }
    }
}

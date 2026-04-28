// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

using Microsoft.TestPlatform.VsTestConsole.TranslationLayer;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace TestPlatform.Playground;

internal class Program
{
    static void Main()
    {
        // This project references TranslationLayer, vstest.console, TestHostProvider, testhost and MSTest1 projects, to make sure
        // we build all the dependencies of that are used to run tests via VSTestConsoleWrapper. It then copies the components from
        // their original build locations, to $(TargetDir)\netfx\vstest.console directory, and its subfolders to create an executable
        // copy of TestPlatform that is similar to what we ship.
        //
        // The copying might trigger only on re-build, if you see outdated dependencies, Rebuild this project instead of just Build.
        //
        // Use this as playground for your debugging of end-to-end scenarios, it will automatically attach vstest.console and teshost
        // sub-processes. It won't stop at entry-point automatically, don't forget to set your breakpoints, or remove VSTEST_DEBUG_NOBP
        // from the environment variables of this project.

        var thisAssemblyPath = Assembly.GetEntryAssembly()!.Location;
        var here = Path.GetDirectoryName(thisAssemblyPath)!;
        var playground = Path.GetFullPath(Path.Combine(here, "..", "..", "..", ".."));

        Environment.SetEnvironmentVariable("VSTEST_DEBUG_ATTACHVS_PATH", Path.Combine(here, "AttachVS.exe"));

        var console = Path.Combine(here, "vstest.console", "netfx", "vstest.console.exe");

        var sourceSettings = $$$"""
            <RunSettings>
                <RunConfiguration>

                    <!-- <MaxCpuCount>1</MaxCpuCount> -->
                    <!-- <DisableParallelization>True<DisableParallelization> -->
                    <!-- <TargetPlatform>x86</TargetPlatform> -->
                    <!-- <TargetFrameworkVersion>net472</TargetFrameworkVersion> -->

                    <!-- Per test coverage support -->
                    <!--
                    <MaxCpuCount>0</MaxCpuCount>
                    <ForceOneTestAtTimePerTestHost>True</ForceOneTestAtTimePerTestHost>
                    <TargetFrameworkTestHostDemultiplexer>4</TargetFrameworkTestHostDemultiplexer>
                    -->

                    <!-- The settings below are what VS sends by default. -->
                    <CollectSourceInformation>true</CollectSourceInformation>
                    <DesignMode>True</DesignMode>
                </RunConfiguration>
                <BoostTestInternalSettings xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
                    <VSProcessId>999999</VSProcessId>
                </BoostTestInternalSettings>
                <GoogleTestAdapterSettings xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
                <SolutionSettings>
                  <Settings />
                </SolutionSettings>
                <ProjectSettings />
                </GoogleTestAdapterSettings>

                <!-- Blame hang -->
                <!-- <LoggerRunSettings>
                  <Loggers>
                    <Logger friendlyName="blame" enabled="True" />
                  </Loggers>
                </LoggerRunSettings>
                <DataCollectionRunSettings>
                  <DataCollectors>
                    <DataCollector friendlyName="blame" enabled="True">
                      <Configuration>
                        <CollectDumpOnTestSessionHang TestTimeout="10s" HangDumpType="Full" />
                      </Configuration>
                    </DataCollector>
                  </DataCollectors>
                </DataCollectionRunSettings> -->
            </RunSettings>
            """;

        var sources = new[] {
            Path.Combine(playground, "bin", "MSTest1", "Debug", "net48", "MSTest1.dll"),
            Path.Combine(playground, "bin", "MSTest2", "Debug", "net48", "MSTest2.dll"),
            // The built in .NET projects don't now work right now in Playground, there is some conflict with Arcade.
            // But if you create one outside of Playground it will work. 
            //Path.Combine(playground, "bin", "MSTest1", "Debug", "net7.0", "MSTest1.dll"),
        };

        //// Console mode
        //// Uncomment if providing command line parameters is easier for you
        //// than converting them to settings, or when you debug command line scenario specifically.
        //var settingsFile = Path.GetTempFileName();
        //try
        //{
        //    File.WriteAllText(settingsFile, sourceSettings);
        //    var processStartInfo = new ProcessStartInfo
        //    {
        //        FileName = console,
        //        Arguments = $"{string.Join(" ", sources)} --settings:{settingsFile} --logger:trx;LogFileName=my.trx;WarnOnFileOverwrite=false",
        //        UseShellExecute = false,
        //    };
        //    EnvironmentVariables.Variables.ToList().ForEach(processStartInfo.Environment.Add);
        //    var process = Process.Start(processStartInfo);
        //    process.WaitForExit();
        //    if (process.ExitCode != 0)
        //    {
        //        throw new Exception($"Process failed with {process.ExitCode}");
        //    }
        //}
        //finally
        //{
        //    try { File.Delete(settingsFile); } catch { }
        //}

        // design mode
        var detailedOutput = true;
        var consoleOptions = new ConsoleParameters
        {
            EnvironmentVariables = EnvironmentVariables.Variables,
            LogFilePath = Path.Combine(here, "logs", "log.txt"),
            TraceLevel = TraceLevel.Verbose,
        };
        var options = new TestPlatformOptions
        {
            CollectMetrics = true,
        };
        var r = new VsTestConsoleWrapper(console, consoleOptions);
        var sessionHandler = new TestSessionHandler();
#pragma warning disable CS0618 // Type or member is obsolete
        //// TestSessions
        // r.StartTestSession(sources, sourceSettings, sessionHandler);
#pragma warning restore CS0618 // Type or member is obsolete
        var discoveryHandler = new PlaygroundTestDiscoveryHandler(detailedOutput);
        var sw = Stopwatch.StartNew();
        // Discovery
        r.DiscoverTests(sources, sourceSettings, options, sessionHandler.TestSessionInfo, discoveryHandler);
        var discoveryDuration = sw.ElapsedMilliseconds;
        Console.WriteLine($"Discovery done in {discoveryDuration} ms");
        sw.Restart();
        // Run with test cases and custom testhost launcher
        //r.RunTestsWithCustomTestHost(discoveryHandler.TestCases, sourceSettings, options, sessionHandler.TestSessionInfo, new TestRunHandler(detailedOutput), new DebuggerTestHostLauncher());
        //// Run with test cases and without custom testhost launcher
        r.RunTests(discoveryHandler.TestCases, sourceSettings, options, sessionHandler.TestSessionInfo, new TestRunHandler(detailedOutput));
        //// Run with sources and custom testhost launcher and debugging
        //r.RunTestsWithCustomTestHost(sources, sourceSettings, options, sessionHandler.TestSessionInfo, new TestRunHandler(detailedOutput), new DebuggerTestHostLauncher());
        //// Run with sources
        //r.RunTests(sources, sourceSettings, options, sessionHandler.TestSessionInfo, new TestRunHandler(detailedOutput));
        var rd = sw.ElapsedMilliseconds;
        Console.WriteLine($"Discovery: {discoveryDuration} ms, Run: {rd} ms, Total: {discoveryDuration + rd} ms");
        // Console.WriteLine($"Settings:\n{sourceSettings}");
    }

    public class PlaygroundTestDiscoveryHandler : ITestDiscoveryEventsHandler, ITestDiscoveryEventsHandler2
    {
        private int _testCasesCount;
        private readonly bool _detailedOutput;

        public PlaygroundTestDiscoveryHandler(bool detailedOutput)
        {
            _detailedOutput = detailedOutput;
        }

        public List<TestCase> TestCases { get; internal set; } = new List<TestCase>();

        public void HandleDiscoveredTests(IEnumerable<TestCase>? discoveredTestCases)
        {
            if (_detailedOutput)
            {
                Console.WriteLine($"[DISCOVERY.PROGRESS]");
                Console.WriteLine(WriteTests(discoveredTestCases));
            }
            _testCasesCount += discoveredTestCases!.Count();
            if (discoveredTestCases != null) { TestCases.AddRange(discoveredTestCases); }
        }

        public void HandleDiscoveryComplete(long totalTests, IEnumerable<TestCase>? lastChunk, bool isAborted)
        {
            Console.WriteLine($"[DISCOVERY.COMPLETE] aborted? {isAborted}, tests count: {totalTests}");
            if (_detailedOutput)
            {
                Console.WriteLine("Last chunk:");
                Console.WriteLine(WriteTests(lastChunk));
            }
            if (lastChunk != null) { TestCases.AddRange(lastChunk); }
        }

        public void HandleDiscoveryComplete(DiscoveryCompleteEventArgs discoveryCompleteEventArgs, IEnumerable<TestCase>? lastChunk)
        {
            Console.WriteLine($"[DISCOVERY.COMPLETE] aborted? {discoveryCompleteEventArgs.IsAborted}, tests count: {discoveryCompleteEventArgs.TotalCount}, discovered count: {_testCasesCount}");
            if (_detailedOutput)
            {
                Console.WriteLine("Last chunk:");
                Console.WriteLine(WriteTests(lastChunk));
            }
            Console.WriteLine("Fully discovered:");
            Console.WriteLine(WriteSources(discoveryCompleteEventArgs.FullyDiscoveredSources));
            Console.WriteLine("Partially discovered:");
            Console.WriteLine(WriteSources(discoveryCompleteEventArgs.PartiallyDiscoveredSources));
            Console.WriteLine("Skipped discovery:");
            Console.WriteLine(WriteSources(discoveryCompleteEventArgs.SkippedDiscoveredSources));
            Console.WriteLine("Not discovered:");
            Console.WriteLine(WriteSources(discoveryCompleteEventArgs.NotDiscoveredSources));
            if (lastChunk != null) { TestCases.AddRange(lastChunk); }
        }

        public void HandleLogMessage(TestMessageLevel level, string? message)
        {
            Console.WriteLine($"[DISCOVERY.{level.ToString().ToUpper(CultureInfo.InvariantCulture)}] {message}");
        }

        public void HandleRawMessage(string rawMessage)
        {
            Console.WriteLine($"[DISCOVERY.MESSAGE] {rawMessage}");
        }

        private static string WriteTests(IEnumerable<TestCase>? testCases)
            => testCases?.Any() == true
                ? "\t" + string.Join("\n\t", testCases!.Select(r => r.Source + " " + r.DisplayName))
                : "\t<empty>";

        private static string WriteSources(IEnumerable<string>? sources)
            => sources?.Any() == true
                ? "\t" + string.Join("\n\t", sources)
                : "\t<empty>";
    }

    public class TestRunHandler : ITestRunEventsHandler
    {
        private readonly bool _detailedOutput;

        public TestRunHandler(bool detailedOutput)
        {
            _detailedOutput = detailedOutput;
        }

        public void HandleLogMessage(TestMessageLevel level, string? message)
        {
            Console.WriteLine($"[{level.ToString().ToUpper(CultureInfo.InvariantCulture)}]: {message}");
        }

        public void HandleRawMessage(string rawMessage)
        {
            if (_detailedOutput)
            {
                Console.WriteLine($"[RUN.MESSAGE]: {rawMessage}");
            }
        }

        public void HandleTestRunComplete(TestRunCompleteEventArgs testRunCompleteArgs, TestRunChangedEventArgs? lastChunkArgs, ICollection<AttachmentSet>? runContextAttachments, ICollection<string>? executorUris)
        {
            Console.WriteLine($"[RUN.COMPLETE]: err: {testRunCompleteArgs.Error}, lastChunk:");
            if (_detailedOutput)
            {
                Console.WriteLine(WriteTests(lastChunkArgs?.NewTestResults));
            }
        }

        public void HandleTestRunStatsChange(TestRunChangedEventArgs? testRunChangedArgs)
        {
            if (_detailedOutput)
            {
                Console.WriteLine($"[RUN.PROGRESS]");
                Console.WriteLine(WriteTests(testRunChangedArgs?.NewTestResults));
            }
        }

        public int LaunchProcessWithDebuggerAttached(TestProcessStartInfo testProcessStartInfo)
        {
            throw new NotImplementedException();
        }

        private static string WriteTests(IEnumerable<TestResult>? testResults)
            => WriteTests(testResults?.Select(t => t.TestCase));

        private static string WriteTests(IEnumerable<TestCase>? testCases)
            => testCases?.Any() == true
                ? "\t" + string.Join("\n\t", testCases.Select(r => r.DisplayName))
                : "\t<empty>";
    }

    internal class DebuggerTestHostLauncher : ITestHostLauncher2
    {
        public bool IsDebug => true;

        public bool AttachDebuggerToProcess(int pid)
        {
            return true;
        }

        public bool AttachDebuggerToProcess(int pid, CancellationToken cancellationToken)
        {
            return true;
        }

        public int LaunchTestHost(TestProcessStartInfo defaultTestHostStartInfo)
        {
            return 1;
        }

        public int LaunchTestHost(TestProcessStartInfo defaultTestHostStartInfo, CancellationToken cancellationToken)
        {
            return 1;
        }
    }
}

internal class TestSessionHandler : ITestSessionEventsHandler
{
    public TestSessionHandler() { }
    public TestSessionInfo? TestSessionInfo { get; private set; }

    public void HandleLogMessage(TestMessageLevel level, string? message)
    {

    }

    public void HandleRawMessage(string rawMessage)
    {

    }

    public void HandleStartTestSessionComplete(StartTestSessionCompleteEventArgs? eventArgs)
    {
        TestSessionInfo = eventArgs?.TestSessionInfo;
    }

    public void HandleStopTestSessionComplete(StopTestSessionCompleteEventArgs? eventArgs)
    {

    }
}

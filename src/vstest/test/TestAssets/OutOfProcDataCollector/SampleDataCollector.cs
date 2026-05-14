// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;

namespace OutOfProcDataCollector
{
    [DataCollectorFriendlyName("SampleDataCollector")]
    [DataCollectorTypeUri("my://sample/datacollector")]
    public class SampleDataCollector : DataCollector, ITestExecutionEnvironmentSpecifier
    {
        int _i;
        private DataCollectionSink? _dataCollectionSink;
        private DataCollectionEnvironmentContext? _context;
        private DataCollectionLogger? _logger;
        private readonly string _tempDirectoryPath = Environment.GetEnvironmentVariable("TEST_ASSET_SAMPLE_COLLECTOR_PATH") ?? Path.GetTempPath();

        public override void Initialize(
            System.Xml.XmlElement? configurationElement,
            DataCollectionEvents events,
            DataCollectionSink dataSink,
            DataCollectionLogger logger,
            DataCollectionEnvironmentContext? environmentContext)
        {

            events.TestHostLaunched += TestHostLaunched_Handler;
            events.SessionStart += SessionStarted_Handler;
            events.SessionEnd += SessionEnded_Handler;
            events.TestCaseStart += Events_TestCaseStart;
            events.TestCaseEnd += Events_TestCaseEnd;
            _dataCollectionSink = dataSink;
            _context = environmentContext;
            _logger = logger;
        }

        private void Events_TestCaseEnd(object? sender, TestCaseEndEventArgs e)
        {
            _logger!.LogWarning(_context!.SessionDataCollectionContext, "TestCaseEnded " + e.TestCaseName);
        }

        private void Events_TestCaseStart(object? sender, TestCaseStartEventArgs e)
        {
            _logger!.LogWarning(_context!.SessionDataCollectionContext, "TestCaseStarted " + e.TestCaseName);
            _logger.LogWarning(_context.SessionDataCollectionContext, "TestCaseStarted " + e.TestElement!.FullyQualifiedName);
            var filename = Path.Combine(_tempDirectoryPath, "testcasefilename" + _i++ + ".txt");
            File.WriteAllText(filename, string.Empty);
            _dataCollectionSink!.SendFileAsync(e.Context, filename, true);
        }

        private void SessionStarted_Handler(object? sender, SessionStartEventArgs args)
        {
            var filename = Path.Combine(_tempDirectoryPath, "filename.txt");
            File.WriteAllText(filename, string.Empty);
            _dataCollectionSink!.SendFileAsync(_context!.SessionDataCollectionContext, filename, true);
            _logger!.LogWarning(_context.SessionDataCollectionContext, "SessionStarted");
        }

        private void SessionEnded_Handler(object? sender, SessionEndEventArgs args)
        {
            _logger!.LogError(_context!.SessionDataCollectionContext, new Exception("my exception"));
            _logger.LogWarning(_context.SessionDataCollectionContext, "my warning");
            _logger.LogException(_context.SessionDataCollectionContext, new Exception("abc"), DataCollectorMessageLevel.Error);

            _logger.LogWarning(_context.SessionDataCollectionContext, "SessionEnded");
        }

        private void TestHostLaunched_Handler(object? sender, TestHostLaunchedEventArgs e)
        {
            _logger!.LogWarning(_context!.SessionDataCollectionContext, "TestHostLaunched " + e.TestHostProcessId);
        }

        public IEnumerable<KeyValuePair<string, string>> GetTestExecutionEnvironmentVariables()
        {
            return new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("key", "value") };
        }

        protected override void Dispose(bool disposing)
        {
            _logger!.LogWarning(_context!.SessionDataCollectionContext, "Dispose called.");
            base.Dispose(disposing);
        }
    }
}

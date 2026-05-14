// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// This dll is a TestProject as well as a test adapter.We use it as a baseline for performance.
// It does no actual discovery or run.It just returns as many results as we ask it to by TEST_COUNT env variable.
// It has almost no overhead, so all the overhead we see is coming from TestPlatform.

using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace PerfyPassing
{
    [ExtensionUri(Id)]
    [DefaultExecutorUri(Id)]
    public class Perfy : ITestDiscoverer, ITestExecutor
    {
        static Perfy()
        {
            // No meaning to the number, it is just easy to find when it breaks. Better than returning 1000 or 0 which are both
            // less suspicious. It could throw, but that is bad for interactive debugging.
            Count = int.TryParse(Environment.GetEnvironmentVariable("TEST_COUNT") ?? "10000", out var count) ? count : 356;
        }

        public const string Id = "executor://perfy.testadapter";
        public static readonly Uri Uri = new Uri(Id);

        public static int Count { get; }

        public void Cancel()
        {
            // noop
        }

        public void RunTests(IEnumerable<TestCase> tests, IRunContext runContext, IFrameworkHandle frameworkHandle)
        {
            var sources = tests.Select(t => t.Source).Distinct().ToList();
            RunTests(sources, runContext, frameworkHandle);
        }

        public void RunTests(IEnumerable<string> sources, IRunContext runContext, IFrameworkHandle frameworkHandle)
        {
            var location = typeof(Perfy).Assembly.Location;
            for (var i = 0; i < Count; i++)
            {
                var tc = new TestCase($"Test{i}", Uri, location);
                frameworkHandle.RecordResult(new TestResult(tc)
                {
                    Outcome = TestOutcome.Passed
                });
            }
        }

        public void DiscoverTests(IEnumerable<string> _, IDiscoveryContext _2,
            IMessageLogger _3, ITestCaseDiscoverySink discoverySink)
        {
            var location = typeof(Perfy).Assembly.Location;
            var tps = new List<TestProperty>();
            Func<object, bool> validator = (object o) => !string.IsNullOrWhiteSpace(o as string);

            for (var i = 0; i < Count; i++)
            {
                var tc = new TestCase($"Test{i}", Uri, location);
                discoverySink.SendTestCase(tc);
            }
        }
    }
}

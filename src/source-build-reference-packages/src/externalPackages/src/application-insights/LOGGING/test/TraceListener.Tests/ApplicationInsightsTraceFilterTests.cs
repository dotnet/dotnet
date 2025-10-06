// -----------------------------------------------------------------------
// <copyright file="ApplicationInsightsTraceFilterTests.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. 
// All rights reserved.  2013
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.ApplicationInsights.TraceListener.Tests
{
    using System;
    using System.Diagnostics;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Tracing.Tests;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "Disposing the object on the TestCleanup method")]
    public class ApplicationInsightsTraceFilterTests
    {
        private AdapterHelper adapterHelper;

        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            this.adapterHelper = new AdapterHelper();
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.adapterHelper.Dispose();
        }

        [TestMethod]
        [TestCategory("TraceListener")]
        public void RespectFilterForWrite()
        {
            this.TraceFilterTestHelper(
                (ApplicationInsightsTraceListener traceListener, TraceEventCache shimTraceEventCache) =>
                    traceListener.Write("message"),
                    false,
                    SourceLevels.Warning);
        }

        [TestMethod]
        [TestCategory("TraceListener")]
        public void TreatWriteAsVerbose()
        {
            this.TraceFilterTestHelper(
                (ApplicationInsightsTraceListener traceListener, TraceEventCache shimTraceEventCache) =>
                    traceListener.Write("message"),
                    true,
                    SourceLevels.Verbose);
        }

        [TestMethod]
        [TestCategory("TraceListener")]
        public void RespectFilterForWriteLine()
        {
            this.TraceFilterTestHelper(
                (ApplicationInsightsTraceListener traceListener, TraceEventCache shimTraceEventCache) =>
                    traceListener.WriteLine("message"),
                    false,
                    SourceLevels.Warning);
        }

        [TestMethod]
        [TestCategory("TraceListener")]
        public void TreatWriteLineAsVerbose()
        {
            this.TraceFilterTestHelper(
                (ApplicationInsightsTraceListener traceListener, TraceEventCache shimTraceEventCache) =>
                    traceListener.WriteLine("message"),
                    true,
                    SourceLevels.Verbose);
        }

        private void TraceFilterTestHelper(
            Action<ApplicationInsightsTraceListener, TraceEventCache> callTraceEent,
            bool shouldTrace,
            SourceLevels filterLevel = SourceLevels.Warning)
        {
            TraceEventCache shimTraceEventCache = new TraceEventCache();

            using (var traceListener = new ApplicationInsightsTraceListener(this.adapterHelper.InstrumentationKey))
            {
                var telemetryConfiguration = new TelemetryConfiguration
                {
                    InstrumentationKey = Guid.NewGuid().ToString(),
                    TelemetryChannel = this.adapterHelper.Channel
                };

                traceListener.TelemetryClient = new TelemetryClient(telemetryConfiguration);

                var traceFilter = new EventTypeFilter(filterLevel);
                traceListener.Filter = traceFilter;

                callTraceEent(traceListener, shimTraceEventCache);

                Assert.AreEqual(shouldTrace, this.adapterHelper.Channel.SentItems.Length == 1);
            }
        }
    }
}

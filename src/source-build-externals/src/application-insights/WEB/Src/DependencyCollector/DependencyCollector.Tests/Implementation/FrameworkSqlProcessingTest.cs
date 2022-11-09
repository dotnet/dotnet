#if NET452
namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Threading;

    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.DependencyCollector;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation.Operation;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.ApplicationInsights.Web.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public sealed class FrameworkSqlProcessingTest : IDisposable
    {
        private const int TimeAccuracyMilliseconds = 50;
        private const int SleepTimeMsecBetweenBeginAndEnd = 100;
        private TelemetryConfiguration configuration;
        private List<ITelemetry> sendItems;
        private FrameworkSqlProcessing sqlProcessingFramework;

        [TestInitialize]
        public void TestInitialize()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            Activity.ForceDefaultIdFormat = true;

            this.configuration = new TelemetryConfiguration();
            this.sendItems = new List<ITelemetry>();
            this.configuration.TelemetryChannel = new StubTelemetryChannel { OnSend = item => this.sendItems.Add(item) };
            this.configuration.InstrumentationKey = Guid.NewGuid().ToString();
            this.configuration.TelemetryInitializers.Add(new OperationCorrelationTelemetryInitializer());
            this.sqlProcessingFramework = new FrameworkSqlProcessing(this.configuration, new CacheBasedOperationHolder("testCache", 100 * 1000));
        }

        [TestCleanup]
        public void Cleanup()
        {
            while (Activity.Current != null)
            {
                Activity.Current.Stop();
            }
        }

        #region ExecuteReader

        /// <summary>
        /// Validates SQLProcessingFramework sends correct telemetry for non stored procedure in async call.
        /// </summary>
        [TestMethod]
        [Description("Validates SQLProcessingFramework sends correct telemetry for non stored procedure in async call.")]
        public void RddTestSqlProcessingFrameworkSendsCorrectTelemetrySqlQuerySuccess()
        {
            Stopwatch stopwatchMax = Stopwatch.StartNew();
            this.sqlProcessingFramework.OnBeginExecuteCallback(
                id: 1111,
                database: "mydatabase",
                dataSource: "ourdatabase.database.windows.net",
                commandText: string.Empty);
            Stopwatch stopwatchMin = Stopwatch.StartNew();

            Thread.Sleep(SleepTimeMsecBetweenBeginAndEnd);

            stopwatchMin.Stop();
            this.sqlProcessingFramework.OnEndExecuteCallback(id: 1111, success: true, sqlExceptionNumber: 0);
            stopwatchMax.Stop();

            Assert.IsNull(Activity.Current);
            Assert.AreEqual(1, this.sendItems.Count, "Only one telemetry item should be sent");
            ValidateTelemetryPacket(
                this.sendItems[0] as DependencyTelemetry,
                "ourdatabase.database.windows.net | mydatabase",
                "ourdatabase.database.windows.net | mydatabase",
                RemoteDependencyConstants.SQL,
                true,
                stopwatchMin.Elapsed.TotalMilliseconds,
                stopwatchMax.Elapsed.TotalMilliseconds,
                string.Empty,
                null);
        }

        [TestMethod]
        public void RddTestSqlProcessingFrameworkSendsCorrectTelemetrySqlQuerySuccessParentActivity()
        {
            var parentActivity = new Activity("parent").Start();
            parentActivity.AddBaggage("k", "v");
            parentActivity.TraceStateString = "tracestate";

            Stopwatch stopwatchMax = Stopwatch.StartNew();
            this.sqlProcessingFramework.OnBeginExecuteCallback(
                id: 1111,
                database: "mydatabase",
                dataSource: "ourdatabase.database.windows.net",
                commandText: string.Empty);
            Stopwatch stopwatchMin = Stopwatch.StartNew();

            Thread.Sleep(SleepTimeMsecBetweenBeginAndEnd);

            stopwatchMin.Stop();
            this.sqlProcessingFramework.OnEndExecuteCallback(id: 1111, success: true, sqlExceptionNumber: 0);
            stopwatchMax.Stop();

            Assert.AreEqual(parentActivity, Activity.Current);
            Assert.AreEqual(1, this.sendItems.Count, "Only one telemetry item should be sent");
            ValidateTelemetryPacket(
                this.sendItems[0] as DependencyTelemetry,
                "ourdatabase.database.windows.net | mydatabase",
                "ourdatabase.database.windows.net | mydatabase",
                RemoteDependencyConstants.SQL,
                true,
                stopwatchMin.Elapsed.TotalMilliseconds,
                stopwatchMax.Elapsed.TotalMilliseconds,
                string.Empty,
                parentActivity);
        }

        /// <summary>
        /// Validates SQLProcessingFramework sends correct telemetry for non stored procedure in async call.
        /// </summary>
        [TestMethod]
        public void RddTestSqlProcessingFrameworkSendsCorrectTelemetrySqlQuerySuccessW3COff()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.Hierarchical;
            Activity.ForceDefaultIdFormat = true;

            var parentActivity = new Activity("parent").Start();
            parentActivity.AddBaggage("k", "v");

            Stopwatch stopwatchMax = Stopwatch.StartNew();
            this.sqlProcessingFramework.OnBeginExecuteCallback(
                id: 1111,
                database: "mydatabase",
                dataSource: "ourdatabase.database.windows.net",
                commandText: string.Empty);
            Stopwatch stopwatchMin = Stopwatch.StartNew();

            Thread.Sleep(SleepTimeMsecBetweenBeginAndEnd);

            stopwatchMin.Stop();
            this.sqlProcessingFramework.OnEndExecuteCallback(id: 1111, success: true, sqlExceptionNumber: 0);
            stopwatchMax.Stop();

            Assert.AreEqual(parentActivity, Activity.Current);
            Assert.AreEqual(1, this.sendItems.Count, "Only one telemetry item should be sent");
            ValidateTelemetryPacket(
                this.sendItems[0] as DependencyTelemetry,
                "ourdatabase.database.windows.net | mydatabase",
                "ourdatabase.database.windows.net | mydatabase",
                RemoteDependencyConstants.SQL,
                true,
                stopwatchMin.Elapsed.TotalMilliseconds,
                stopwatchMax.Elapsed.TotalMilliseconds,
                string.Empty,
                parentActivity);
        }

        /// <summary>
        /// Validates SQLProcessingFramework sends correct telemetry for non stored procedure in async call.
        /// </summary>
        [TestMethod]
        [Description("Validates SQLProcessingFramework sends correct telemetry for non stored procedure in async call.")]
        public void RddTestSqlProcessingFrameworkSendsCorrectTelemetryMultipleItems()
        {
            var parent = new Activity("parent").Start();

            for (int i = 0; i < 10; i++)
            {
                Stopwatch stopwatchMax = Stopwatch.StartNew();
                this.sqlProcessingFramework.OnBeginExecuteCallback(
                    id: i,
                    database: "mydatabase",
                    dataSource: "ourdatabase.database.windows.net",
                    commandText: string.Empty);
                Stopwatch stopwatchMin = Stopwatch.StartNew();

                Thread.Sleep(SleepTimeMsecBetweenBeginAndEnd);

                stopwatchMin.Stop();
                this.sqlProcessingFramework.OnEndExecuteCallback(id: i, success: true, sqlExceptionNumber: 0);
                stopwatchMax.Stop();

                Assert.AreEqual(parent, Activity.Current);
                Assert.AreEqual(i + 1, this.sendItems.Count, "Only one telemetry item should be sent");

                var dependencyTelemetry = this.sendItems[i] as DependencyTelemetry;
                ValidateTelemetryPacket(
                    dependencyTelemetry,
                    "ourdatabase.database.windows.net | mydatabase",
                    "ourdatabase.database.windows.net | mydatabase",
                    RemoteDependencyConstants.SQL,
                    true,
                    stopwatchMin.Elapsed.TotalMilliseconds,
                    stopwatchMax.Elapsed.TotalMilliseconds,
                    string.Empty,
                    parent);
            }
        }

        /// <summary>
        /// Validates SQLProcessingFramework sends correct telemetry for non stored procedure in async call.
        /// </summary>
        [TestMethod]
        [Description("Validates SQLProcessingFramework sends correct telemetry for non stored procedure in async call.")]
        public void RddTestSqlProcessingFrameworkSendsCorrectTelemetrySqlQueryAsync()
        {
            Stopwatch stopwatchMax = Stopwatch.StartNew();
            this.sqlProcessingFramework.OnBeginExecuteCallback(
                id: 1111,
                database: "mydatabase",
                dataSource: "ourdatabase.database.windows.net",
                commandText: string.Empty);
            Stopwatch stopwatchMin = Stopwatch.StartNew();

            Thread.Sleep(SleepTimeMsecBetweenBeginAndEnd);

            stopwatchMin.Stop();
            this.sqlProcessingFramework.OnEndExecuteCallback(id: 1111, success: true, sqlExceptionNumber: 0);
            stopwatchMax.Stop();
            Assert.IsNull(Activity.Current);
            Assert.AreEqual(1, this.sendItems.Count, "Only one telemetry item should be sent");
            ValidateTelemetryPacket(
                this.sendItems[0] as DependencyTelemetry,
                "ourdatabase.database.windows.net | mydatabase",
                "ourdatabase.database.windows.net | mydatabase",
                RemoteDependencyConstants.SQL,
                true,
                stopwatchMin.Elapsed.TotalMilliseconds,
                stopwatchMax.Elapsed.TotalMilliseconds,
                string.Empty,
                null);
        }

        /// <summary>
        /// Validates SQLProcessingFramework sends correct telemetry for non stored procedure in failed call.
        /// </summary>
        [TestMethod]
        [Description("Validates sqlProcessingFramework sends correct telemetry for non stored procedure in failed call.")]
        public void RddTestSqlProcessingFrameworkSendsCorrectTelemetrySqlQueryFailed()
        {
            Stopwatch stopwatchMax = Stopwatch.StartNew();
            this.sqlProcessingFramework.OnBeginExecuteCallback(
                id: 1111,
                database: "mydatabase",
                dataSource: "ourdatabase.database.windows.net",
                commandText: string.Empty);
            Stopwatch stopwatchMin = Stopwatch.StartNew();

            Thread.Sleep(SleepTimeMsecBetweenBeginAndEnd);

            stopwatchMin.Stop();
            this.sqlProcessingFramework.OnEndExecuteCallback(id: 1111, success: false, sqlExceptionNumber: 1);
            stopwatchMax.Stop();

            Assert.IsNull(Activity.Current);
            Assert.AreEqual(1, this.sendItems.Count, "Only one telemetry item should be sent");
            ValidateTelemetryPacket(
                this.sendItems[0] as DependencyTelemetry,
                "ourdatabase.database.windows.net | mydatabase",
                "ourdatabase.database.windows.net | mydatabase",
                RemoteDependencyConstants.SQL,
                false,
                stopwatchMin.Elapsed.TotalMilliseconds,
                stopwatchMax.Elapsed.TotalMilliseconds,
                "1",
                null);
        }

#if !NET452
        /// <summary>
        /// Validates SQLProcessingFramework sends correct telemetry.
        /// </summary>
        [TestMethod]
        [Description("Validates SQLProcessingFramework sends correct telemetry for stored procedure.")]
        public void RddTestSqlProcessingFrameworkSendsCorrectTelemetryStoredProc()
        {
            Stopwatch stopwatchMax = Stopwatch.StartNew();
            this.sqlProcessingFramework.OnBeginExecuteCallback(
                id: 1111,
                dataSource: "ourdatabase.database.windows.net",
                database: "mydatabase",
                commandText: "apm.MyFavouriteStoredProcedure");
            Stopwatch stopwatchMin = Stopwatch.StartNew();

            Thread.Sleep(SleepTimeMsecBetweenBeginAndEnd);

            stopwatchMin.Stop();
            this.sqlProcessingFramework.OnEndExecuteCallback(id: 1111, success: true, sqlExceptionNumber: 0);
            stopwatchMax.Stop();

            Assert.IsNull(Activity.Current);
            Assert.AreEqual(1, this.sendItems.Count, "Only one telemetry item should be sent");
            ValidateTelemetryPacket(
                this.sendItems[0] as DependencyTelemetry,
                "ourdatabase.database.windows.net | mydatabase",
                "apm.MyFavouriteStoredProcedure",
                RemoteDependencyConstants.SQL,
                true,
                stopwatchMin.Elapsed.TotalMilliseconds,
                stopwatchMax.Elapsed.TotalMilliseconds,
                string.Empty,
                null);
        }
#endif

        #endregion

        #region Disposable
        public void Dispose()
        {
            this.configuration.Dispose();
            GC.SuppressFinalize(this);
        }
        #endregion Disposable

        #region Helpers

        private static void ValidateTelemetryPacket(
            DependencyTelemetry remoteDependencyTelemetryActual,
            string target,
            string name,
            string type,
            bool success,
            double minDependencyDurationMs,
            double maxDependencyDurationMs,
            string errorCode,
            Activity parentActivity)
        {
            Assert.AreEqual(name, remoteDependencyTelemetryActual.Name, true, "Resource name in the sent telemetry is wrong");
            Assert.AreEqual(target, remoteDependencyTelemetryActual.Target, true, "Resource target in the sent telemetry is wrong");
            Assert.AreEqual(type.ToString(), remoteDependencyTelemetryActual.Type, "DependencyKind in the sent telemetry is wrong");
            Assert.AreEqual(success, remoteDependencyTelemetryActual.Success, "Success in the sent telemetry is wrong");
            Assert.AreEqual(errorCode, remoteDependencyTelemetryActual.ResultCode, "ResultCode in the sent telemetry is wrong");

            Assert.IsTrue(
                remoteDependencyTelemetryActual.Duration.TotalMilliseconds <= maxDependencyDurationMs,
                $"Dependency duration {remoteDependencyTelemetryActual.Duration.TotalMilliseconds} must be smaller than time between before-start and after-end: '{maxDependencyDurationMs}'");

            Assert.IsTrue(
                remoteDependencyTelemetryActual.Duration.TotalMilliseconds >= minDependencyDurationMs,
                $"Dependency duration {remoteDependencyTelemetryActual.Duration.TotalMilliseconds} must be bigger than time between after-start and before-end '{minDependencyDurationMs}'");

            string expectedVersion = SdkVersionHelper.GetExpectedSdkVersion(typeof(DependencyTrackingTelemetryModule), prefix: "rddf:");
            Assert.AreEqual(expectedVersion, remoteDependencyTelemetryActual.Context.GetInternalContext().SdkVersion);

            if (parentActivity != null)
            {
                if (parentActivity.IdFormat == ActivityIdFormat.W3C)
                {
                    Assert.AreEqual(parentActivity.TraceId.ToHexString(), remoteDependencyTelemetryActual.Context.Operation.Id);
                    Assert.AreEqual(parentActivity.SpanId.ToHexString(), remoteDependencyTelemetryActual.Context.Operation.ParentId);
                    if (parentActivity.TraceStateString != null)
                    {
                        Assert.IsTrue(remoteDependencyTelemetryActual.Properties.ContainsKey("tracestate"));
                        Assert.AreEqual(parentActivity.TraceStateString, remoteDependencyTelemetryActual.Properties["tracestate"]);
                    }
                    else
                    {
                        Assert.IsFalse(remoteDependencyTelemetryActual.Properties.ContainsKey("tracestate"));
                    }
                }
                else
                {
                    Assert.AreEqual(parentActivity.RootId, remoteDependencyTelemetryActual.Context.Operation.Id);
                    Assert.AreEqual(parentActivity.Id, remoteDependencyTelemetryActual.Context.Operation.ParentId);
                }
            }
            else
            {
                Assert.IsNotNull(remoteDependencyTelemetryActual.Context.Operation.Id);
                Assert.IsNull(remoteDependencyTelemetryActual.Context.Operation.ParentId);
            }
        }

        #endregion Helpers
    }
}
#endif
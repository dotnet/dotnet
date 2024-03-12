namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;

    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation.SqlClientDiagnostics;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.Web.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    [TestClass]
    public class SqlClientDiagnosticSourceListenerTestsCopy : IDisposable
    {
        private const string TestConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Database=master";

        private IList<ITelemetry> sendItems;
        private StubTelemetryChannel stubTelemetryChannel;
        private TelemetryConfiguration configuration;
        private FakeSqlClientDiagnosticSource fakeSqlClientDiagnosticSource;
        private SqlClientDiagnosticSourceListener sqlClientDiagnosticSourceListener;

        public SqlClientDiagnosticSourceListenerTestsCopy()
        {
            this.sendItems = new List<ITelemetry>();
            this.stubTelemetryChannel = new StubTelemetryChannel { OnSend = item => this.sendItems.Add(item) };

            this.configuration = new TelemetryConfiguration
            {
                InstrumentationKey = Guid.NewGuid().ToString(),
                TelemetryChannel = this.stubTelemetryChannel
            };

            this.fakeSqlClientDiagnosticSource = new FakeSqlClientDiagnosticSource();
            this.sqlClientDiagnosticSourceListener = new SqlClientDiagnosticSourceListener(this.configuration, true);
        }

        public void Dispose()
        {
            this.sqlClientDiagnosticSourceListener.Dispose();
            this.fakeSqlClientDiagnosticSource.Dispose();
            this.configuration.Dispose();
            this.stubTelemetryChannel.Dispose();

            while (Activity.Current != null)
            {
                Activity.Current.Stop();
            }
        }

        [TestMethod]
        public void TracksCommandErrorWhenSqlException_Sql() => this.RunTest(SqlClientDiagnosticSourceListener.SqlBeforeExecuteCommand, SqlClientDiagnosticSourceListener.SqlErrorExecuteCommand);

        [TestMethod]
        public void TracksCommandErrorWhenSqlException_SqlMicrosoft() => this.RunTest(SqlClientDiagnosticSourceListener.SqlMicrosoftBeforeExecuteCommand, SqlClientDiagnosticSourceListener.SqlMicrosoftErrorExecuteCommand);

        public void RunTest(string beforeCommand, string errorCommand)
        {
            var operationId = Guid.NewGuid();
            var sqlConnection = new SqlConnection(TestConnectionString);
            var sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = "select * from orders";

            var beforeExecuteEventData = new
            {
                OperationId = operationId,
                Command = sqlCommand,
                Timestamp = (long?)1000000L
            };

            this.fakeSqlClientDiagnosticSource.Write(
                beforeCommand,
                beforeExecuteEventData);

            // Need to create SqlException via reflection because ctor is not public!
            var sqlErrorCtor
                = typeof(SqlError).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)
                    .Single(c => c.GetParameters().Count() == 8);

            var sqlError = sqlErrorCtor.Invoke(
                new object[]
                {
                    42, // error number
                    default(byte),
                    default(byte),
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    0,
                    default(Exception)
                });

            var sqlErrorCollectionCtor
                = typeof(SqlErrorCollection).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)
                    .Single();

            var sqlErrorCollection = sqlErrorCollectionCtor.Invoke(new object[] { });

            typeof(SqlErrorCollection).GetMethod("Add", BindingFlags.NonPublic | BindingFlags.Instance)
                .Invoke(sqlErrorCollection, new object[] { sqlError });

            var sqlExceptionCtor = typeof(SqlException).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)[0];

            var sqlException = (SqlException)sqlExceptionCtor.Invoke(
                new object[]
                {
                    "Boom!",
                    sqlErrorCollection,
                    null,
                    Guid.NewGuid()
                });

            var commandErrorEventData = new
            {
                OperationId = operationId,
                Command = sqlCommand,
                Exception = (Exception)sqlException,
                Timestamp = 2000000L
            };

            this.fakeSqlClientDiagnosticSource.Write(
                errorCommand,
                commandErrorEventData);

            var dependencyTelemetry = (DependencyTelemetry)this.sendItems.Single();

            Assert.AreEqual(commandErrorEventData.Exception.ToInvariantString(), dependencyTelemetry.Properties["Exception"]);
            Assert.IsFalse(dependencyTelemetry.Success.Value);
            Assert.AreEqual("42", dependencyTelemetry.ResultCode);
        }

        private class FakeSqlClientDiagnosticSource : IDisposable
        {
            private readonly DiagnosticListener listener;

            public FakeSqlClientDiagnosticSource()
            {
                this.listener = new DiagnosticListener(SqlClientDiagnosticSourceListener.DiagnosticListenerName);
            }

            public void Write(string name, object value)
            {
                this.listener.Write(name, value);
            }

            public void Dispose()
            {
                this.listener.Dispose();
            }
        }
    }

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
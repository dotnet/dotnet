#if NET452
namespace Microsoft.ApplicationInsights.DependencyCollector.Implementation
{
    using System;
    using System.Diagnostics;
    using System.Globalization;

    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation.Operation;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;

    internal sealed class FrameworkSqlProcessing
    {
        internal CacheBasedOperationHolder TelemetryTable;
        private readonly TelemetryClient telemetryClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameworkSqlProcessing"/> class.
        /// </summary>
        internal FrameworkSqlProcessing(TelemetryConfiguration configuration, CacheBasedOperationHolder telemetryTupleHolder)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            this.TelemetryTable = telemetryTupleHolder ?? throw new ArgumentNullException(nameof(telemetryTupleHolder));
            this.telemetryClient = new TelemetryClient(configuration);

            // Since dependencySource is no longer set, sdk version is prepended with information which can identify whether RDD was collected by profiler/framework

            // For directly using TrackDependency(), version will be simply what is set by core
            string prefix = "rdd" + RddSource.Framework + ":";
            this.telemetryClient.Context.GetInternalContext().SdkVersion = SdkVersionUtils.GetSdkVersion(prefix);
        }

        #region Sql callbacks

        /// <summary>
        /// On begin callback from Framework event source.
        /// </summary>
        /// <param name="id">Identifier of SQL connection object.</param>
        /// <param name="dataSource">Data source name.</param>
        /// <param name="database">Database name.</param>
        /// <param name="commandText">Command text.</param>
        public void OnBeginExecuteCallback(long id, string dataSource, string database, string commandText)
        {
            try
            {
                var resourceName = GetResourceName(dataSource, database);

                DependencyCollectorEventSource.Log.BeginCallbackCalled(id, resourceName);

                if (string.IsNullOrEmpty(resourceName))
                {
                    DependencyCollectorEventSource.Log.NotExpectedCallback(id, "OnBeginSql", "resourceName is empty");
                    return;
                }

                var telemetryTuple = this.TelemetryTable.Get(id);
                if (telemetryTuple == null)
                {
                    var telemetry = ClientServerDependencyTracker.BeginTracking(this.telemetryClient);
                    telemetry.Name = resourceName;
                    telemetry.Target = string.Join(" | ", dataSource, database);
                    telemetry.Type = RemoteDependencyConstants.SQL;
                    telemetry.Data = commandText;
                    this.TelemetryTable.Store(id, new Tuple<DependencyTelemetry, bool>(telemetry, false));
                }
            }
            catch (Exception exception)
            {
                DependencyCollectorEventSource.Log.CallbackError(id, "OnBeginSql", exception);
            }
            finally
            {
                Activity current = Activity.Current;
                if (current?.OperationName == ClientServerDependencyTracker.DependencyActivityName)
                {
                    current.Stop();
                }
            }
        }

        /// <summary>
        /// On end callback from Framework event source.
        /// </summary>        
        /// <param name="id">Identifier of SQL connection object.</param>
        /// <param name="success">Indicate whether operation completed successfully.</param>
        /// <param name="sqlExceptionNumber">SQL exception number.</param>
        public void OnEndExecuteCallback(long id, bool success, int sqlExceptionNumber)
        {
            DependencyCollectorEventSource.Log.EndCallbackCalled(id.ToString(CultureInfo.InvariantCulture));

            var telemetryTuple = this.TelemetryTable.Get(id);

            if (telemetryTuple == null)
            {
                DependencyCollectorEventSource.Log.EndCallbackWithNoBegin(id.ToString(CultureInfo.InvariantCulture));
                return;
            }

            if (!telemetryTuple.Item2)
            {
                this.TelemetryTable.Remove(id);
                var telemetry = telemetryTuple.Item1;
                telemetry.Success = success;
                telemetry.ResultCode = sqlExceptionNumber != 0 ? sqlExceptionNumber.ToString(CultureInfo.InvariantCulture) : string.Empty;
                DependencyCollectorEventSource.Log.AutoTrackingDependencyItem(telemetry.Name);
                ClientServerDependencyTracker.EndTracking(this.telemetryClient, telemetry);
            }
        }

        #endregion

        /// <summary>
        /// Gets SQL command resource name.
        /// </summary>
        /// <param name="dataSource">DataSource name.</param>
        /// <param name="database">Database name.</param>
        /// <returns>The resource name if possible otherwise empty string.</returns>
        private static string GetResourceName(string dataSource, string database)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} | {1}", dataSource, database);
        }
    }
}
#endif
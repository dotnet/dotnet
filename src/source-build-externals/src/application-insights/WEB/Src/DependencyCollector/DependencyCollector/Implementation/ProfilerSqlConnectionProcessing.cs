#if NET452
namespace Microsoft.ApplicationInsights.DependencyCollector.Implementation
{
    using System.Data.SqlClient;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation.Operation;
    using Microsoft.ApplicationInsights.Extensibility;

    /// <summary>
    /// Concrete class with all processing logic to generate dependencies from the callbacks received from Profiler instrumentation for SQL connection.   
    /// </summary>
    internal sealed class ProfilerSqlConnectionProcessing : ProfilerSqlProcessingBase
    {
        /// <summary>
        /// Constant command text to return.
        /// </summary> 
        private const string SqlConnectionCommandText = "Open";

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfilerSqlConnectionProcessing"/> class.
        /// </summary>
        internal ProfilerSqlConnectionProcessing(TelemetryConfiguration configuration, string agentVersion, ObjectInstanceBasedOperationHolder<DependencyTelemetry> telemetryTupleHolder)
            : base(configuration, agentVersion, telemetryTupleHolder)
        {
        }              

        /// <summary>
        /// Gets SQL connection resource name.
        /// </summary>
        /// <param name="thisObj">The SQL connection.</param>
        /// <returns>The resource name if possible otherwise empty string.</returns>
        internal override string GetDependencyName(object thisObj)
        {
            string resource = string.Empty;

            SqlConnection connection = thisObj as SqlConnection;
            if (connection != null)
            {
                resource = string.Join(" | ", connection.DataSource, connection.Database, SqlConnectionCommandText);
            }

            return resource;
        }

        /// <summary>
        /// Gets SQL connection resource target name.
        /// </summary>
        /// <param name="thisObj">The SQL connection.</param>
        /// <returns>The resource target name if possible otherwise empty string.</returns>
        internal override string GetDependencyTarget(object thisObj)
        {
            string resource = string.Empty;

            SqlConnection connection = thisObj as SqlConnection;
            if (connection != null)
            {
                resource = string.Join(" | ", connection.DataSource, connection.Database);
            }

            return resource;
        }

        /// <summary>
        /// Gets SQL connection command text.
        /// </summary>
        /// <param name="thisObj">The SQL connection.</param>
        /// <returns>Returns predefined command text.</returns>
        internal override string GetCommandName(object thisObj)
        {            
            return SqlConnectionCommandText;
        }
    }
}
#endif
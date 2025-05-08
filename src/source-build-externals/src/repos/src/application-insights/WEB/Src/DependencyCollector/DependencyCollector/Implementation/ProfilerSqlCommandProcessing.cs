#if NET452
namespace Microsoft.ApplicationInsights.DependencyCollector.Implementation
{
    using System.Data;
    using System.Data.SqlClient;

    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation.Operation;
    using Microsoft.ApplicationInsights.Extensibility;

    /// <summary>
    /// Concrete class with all processing logic to generate dependencies from the callbacks received from Profiler instrumentation for SQL command.    
    /// </summary>
    internal sealed class ProfilerSqlCommandProcessing : ProfilerSqlProcessingBase
    {
        private readonly bool collectCommandText;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfilerSqlCommandProcessing"/> class.
        /// </summary>
        internal ProfilerSqlCommandProcessing(TelemetryConfiguration configuration, string agentVersion, ObjectInstanceBasedOperationHolder<DependencyTelemetry> telemetryTupleHolder, bool collectCommandText)
            : base(configuration, agentVersion, telemetryTupleHolder)
        {
            this.collectCommandText = collectCommandText;
        }

        /// <summary>
        /// Gets SQL command resource name.
        /// </summary>
        /// <param name="thisObj">The SQL command.</param>
        /// <returns>The resource name if possible otherwise empty string.</returns>
        internal override string GetDependencyName(object thisObj)
        {
            SqlCommand command = thisObj as SqlCommand;
            string resource = string.Empty;
            if (command != null)
            {
                if (command.Connection != null)
                {
                    string commandName = command.CommandType == CommandType.StoredProcedure
                        ? command.CommandText
                        : string.Empty;

                    resource = string.IsNullOrEmpty(commandName)
                        ? string.Join(" | ", command.Connection.DataSource, command.Connection.Database)
                        : string.Join(" | ", command.Connection.DataSource, command.Connection.Database, commandName);
                }
            }

            return resource;
        }

        /// <summary>
        /// Gets SQL resource target name.
        /// </summary>
        /// <param name="thisObj">The SQL command.</param>
        /// <returns>The resource target name if possible otherwise empty string.</returns>
        internal override string GetDependencyTarget(object thisObj)
        {
            SqlCommand command = thisObj as SqlCommand;
            string result = string.Empty;
            if (command != null)
            {
                if (command.Connection != null)
                {
                    result = string.Join(" | ", command.Connection.DataSource, command.Connection.Database);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets SQL resource command text.
        /// </summary>
        /// <param name="thisObj">The SQL command.</param>
        /// <returns>Returns the command text or empty.</returns>
        internal override string GetCommandName(object thisObj)
        {
            if (this.collectCommandText)
            {
                SqlCommand command = thisObj as SqlCommand;

                if (command != null)
                {
                    return command.CommandText ?? string.Empty;
                }
            }

            return string.Empty;
        }
    }
}
#endif
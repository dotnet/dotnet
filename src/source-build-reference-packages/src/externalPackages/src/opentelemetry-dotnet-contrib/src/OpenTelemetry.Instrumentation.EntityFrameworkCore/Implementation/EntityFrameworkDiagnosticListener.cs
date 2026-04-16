// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Data;
using System.Diagnostics;
using System.Reflection;
using OpenTelemetry.Internal;
using OpenTelemetry.Trace;

namespace OpenTelemetry.Instrumentation.EntityFrameworkCore.Implementation;

internal sealed class EntityFrameworkDiagnosticListener : ListenerHandler
{
    internal const string DiagnosticSourceName = "Microsoft.EntityFrameworkCore";

    internal const string EntityFrameworkCoreCommandCreated = "Microsoft.EntityFrameworkCore.Database.Command.CommandCreated";
    internal const string EntityFrameworkCoreCommandExecuting = "Microsoft.EntityFrameworkCore.Database.Command.CommandExecuting";
    internal const string EntityFrameworkCoreCommandExecuted = "Microsoft.EntityFrameworkCore.Database.Command.CommandExecuted";
    internal const string EntityFrameworkCoreCommandError = "Microsoft.EntityFrameworkCore.Database.Command.CommandError";

    internal static readonly Assembly Assembly = typeof(EntityFrameworkDiagnosticListener).Assembly;
    internal static readonly string ActivitySourceName = Assembly.GetName().Name!;
    internal static readonly string ActivityName = ActivitySourceName + ".Execute";
    internal static readonly ActivitySource EntityFrameworkActivitySource = new(ActivitySourceName, Assembly.GetPackageVersion());

    private readonly PropertyFetcher<object> commandFetcher = new("Command");
    private readonly PropertyFetcher<object> connectionFetcher = new("Connection");
    private readonly PropertyFetcher<object> dbContextFetcher = new("Context");
    private readonly PropertyFetcher<object> dbContextDatabaseFetcher = new("Database");
    private readonly PropertyFetcher<string> providerNameFetcher = new("ProviderName");
    private readonly PropertyFetcher<object> dataSourceFetcher = new("DataSource");
    private readonly PropertyFetcher<object> databaseFetcher = new("Database");
    private readonly PropertyFetcher<CommandType> commandTypeFetcher = new("CommandType");
    private readonly PropertyFetcher<string> commandTextFetcher = new("CommandText");
    private readonly PropertyFetcher<Exception> exceptionFetcher = new("Exception");

    private readonly EntityFrameworkInstrumentationOptions options;

    public EntityFrameworkDiagnosticListener(string sourceName, EntityFrameworkInstrumentationOptions? options)
        : base(sourceName)
    {
        this.options = options ?? new EntityFrameworkInstrumentationOptions();
    }

    public override bool SupportsNullActivity => true;

    public override void OnEventWritten(string name, object? payload)
    {
        var activity = Activity.Current;

        switch (name)
        {
            case EntityFrameworkCoreCommandCreated:
                {
                    activity = EntityFrameworkActivitySource.StartActivity(ActivityName, ActivityKind.Client);
                    if (activity == null)
                    {
                        // There is no listener or it decided not to sample the current request.
                        return;
                    }

                    var command = this.commandFetcher.Fetch(payload);
                    if (command == null)
                    {
                        EntityFrameworkInstrumentationEventSource.Log.NullPayload(nameof(EntityFrameworkDiagnosticListener), name);
                        activity.Stop();
                        return;
                    }

                    var connection = this.connectionFetcher.Fetch(command);
                    var database = (string)this.databaseFetcher.Fetch(connection);
                    activity.DisplayName = database;

                    if (activity.IsAllDataRequested)
                    {
                        string? providerOrCommandName = null;

                        if (this.dbContextFetcher.Fetch(payload) is { } dbContext)
                        {
                            var dbContextDatabase = this.dbContextDatabaseFetcher.Fetch(dbContext);
                            providerOrCommandName = this.providerNameFetcher.Fetch(dbContextDatabase);
                        }
                        else
                        {
                            // Try to infer the database name from the command
                            // type if the DbContext is not available.
                            providerOrCommandName = command.GetType().FullName;
                        }

                        this.AddDbSystemNameTag(activity, providerOrCommandName);

                        var dataSource = (string)this.dataSourceFetcher.Fetch(connection);
                        if (!string.IsNullOrEmpty(dataSource))
                        {
                            var connectionDetails = SqlConnectionDetails.ParseFromDataSource(dataSource);

                            var serverAddress = connectionDetails.ServerHostName ?? connectionDetails.ServerIpAddress;
                            if (!string.IsNullOrEmpty(serverAddress))
                            {
                                this.AddTag(activity, ("peer.service", SemanticConventions.AttributeServerAddress), serverAddress);

                                if (this.options.EmitNewAttributes && connectionDetails.Port is { } port)
                                {
                                    activity.AddTag(SemanticConventions.AttributeServerPort, port);
                                }
                            }
                        }

                        this.AddTag(activity, (SemanticConventions.AttributeDbName, SemanticConventions.AttributeDbNamespace), database);
                    }
                }

                break;

            case EntityFrameworkCoreCommandExecuting:
                {
                    if (activity == null)
                    {
                        EntityFrameworkInstrumentationEventSource.Log.NullActivity(name);
                        return;
                    }

                    if (activity.Source != EntityFrameworkActivitySource)
                    {
                        return;
                    }

                    if (activity.IsAllDataRequested)
                    {
                        var command = this.commandFetcher.Fetch(payload);
                        string? providerName = null;

                        try
                        {
                            if (this.dbContextFetcher.Fetch(payload) is { } dbContext)
                            {
                                var dbContextDatabase = this.dbContextDatabaseFetcher.Fetch(dbContext);
                                providerName = this.providerNameFetcher.Fetch(dbContextDatabase);
                            }

                            if (command is IDbCommand typedCommand && this.options.Filter?.Invoke(providerName, typedCommand) == false)
                            {
                                EntityFrameworkInstrumentationEventSource.Log.CommandIsFilteredOut(activity.OperationName);
                                activity.IsAllDataRequested = false;
                                activity.ActivityTraceFlags &= ~ActivityTraceFlags.Recorded;
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            EntityFrameworkInstrumentationEventSource.Log.CommandFilterException(ex);
                            activity.IsAllDataRequested = false;
                            activity.ActivityTraceFlags &= ~ActivityTraceFlags.Recorded;
                            return;
                        }

                        if (this.options.EmitNewAttributes && this.options.SetDbQueryParameters)
                        {
                            SqlParameterProcessor.AddQueryParameters(activity, command);
                        }

                        if (this.commandTypeFetcher.Fetch(command) is CommandType commandType)
                        {
                            var commandText = this.commandTextFetcher.Fetch(command);
                            switch (commandType)
                            {
                                case CommandType.StoredProcedure:
                                    DatabaseSemanticConventionHelper.ApplyConventionsForStoredProcedure(
                                        activity,
                                        commandText,
                                        this.options.EmitOldAttributes,
                                        this.options.EmitNewAttributes);
                                    break;

                                case CommandType.Text:
                                    // Only SQL-like providers support sanitization as we are not
                                    // able to sanitize arbitrary commands for other query dialects.
                                    bool sanitizeQuery = IsSqlLikeProvider(providerName);

                                    DatabaseSemanticConventionHelper.ApplyConventionsForQueryText(
                                        activity,
                                        commandText,
                                        this.options.EmitOldAttributes,
                                        this.options.EmitNewAttributes,
                                        sanitizeQuery);
                                    break;

                                case CommandType.TableDirect:
                                default:
                                    break;
                            }
                        }

                        try
                        {
                            if (command is IDbCommand typedCommand)
                            {
                                this.options.EnrichWithIDbCommand?.Invoke(activity, typedCommand);
                            }
                        }
                        catch (Exception ex)
                        {
                            EntityFrameworkInstrumentationEventSource.Log.EnrichmentException(nameof(EntityFrameworkCoreCommandExecuting), ex);
                        }
                    }
                }

                break;

            case EntityFrameworkCoreCommandExecuted:
                {
                    if (activity == null)
                    {
                        EntityFrameworkInstrumentationEventSource.Log.NullActivity(name);
                        return;
                    }

                    if (activity.Source == EntityFrameworkActivitySource)
                    {
                        activity.Stop();
                    }

                    // For some reason this EF event comes before the SQLClient SqlMicrosoftAfterExecuteCommand event.
                    // EF span should not be parent of any other span except SQLClient, because of that it can be closed safely.
                    // Can result in a slightly strange timeline where the EF span finishes before its child SQLClient but based on EventSource's it is true.
                    if (activity.Parent?.Source == EntityFrameworkActivitySource)
                    {
                        activity.Parent.Stop();
                    }
                }

                break;

            case EntityFrameworkCoreCommandError:
                {
                    if (activity == null)
                    {
                        EntityFrameworkInstrumentationEventSource.Log.NullActivity(name);
                        return;
                    }

                    if (activity.Source != EntityFrameworkActivitySource)
                    {
                        return;
                    }

                    try
                    {
                        if (activity.IsAllDataRequested)
                        {
                            if (this.exceptionFetcher.Fetch(payload) is Exception exception)
                            {
                                activity.SetStatus(ActivityStatusCode.Error, exception.Message);
                            }
                            else
                            {
                                EntityFrameworkInstrumentationEventSource.Log.NullPayload(nameof(EntityFrameworkDiagnosticListener), name);
                            }
                        }
                    }
                    finally
                    {
                        activity.Stop();
                    }
                }

                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Gets the <c>db.system</c> and <c>db.system.name</c> values to use for the given provider or command name.
    /// </summary>
    /// <param name="providerOrCommandName">The provider or command name.</param>
    /// <returns>
    /// A tuple containing the respective <c>db.system</c> and <c>db.system.name</c> values.
    /// </returns>
    internal static (string Old, string New) GetDbSystemNames(string? providerOrCommandName) =>
        //// "${Attribute} has the following list of well-known values. If one of them applies, then the respective value MUST be used"
        providerOrCommandName switch
        {
            //// These names are defined in the Semantic Conventions
            "Microsoft.Data.SqlClient.SqlCommand" or
            "Microsoft.EntityFrameworkCore.SqlServer"
                => (DbSystems.Mssql, DbSystemNames.MicrosoftSqlServer),
            "Microsoft.EntityFrameworkCore.Cosmos"
                => (DbSystems.Cosmosdb, DbSystemNames.AzureCosmosDb),
            "Devart.Data.SQLite.Entity.EFCore" or
            "Microsoft.Data.Sqlite.SqliteCommand" or
            "Microsoft.EntityFrameworkCore.Sqlite"
                => (DbSystems.Sqlite, DbSystemNames.Sqlite),
            "Devart.Data.MySql.Entity.EFCore" or
            "Devart.Data.MySql.MySqlCommand" or
            "MySql.Data.EntityFrameworkCore" or
            "MySql.Data.MySqlClient.MySqlCommand" or
            "MySql.EntityFrameworkCore" or
            "Pomelo.EntityFrameworkCore.MySql"
                => (DbSystems.Mysql, DbSystemNames.Mysql),
            "Npgsql.EntityFrameworkCore.PostgreSQL" or
            "Npgsql.NpgsqlCommand" or
            "Devart.Data.PostgreSql.Entity.EFCore" or
            "Devart.Data.PostgreSql.PgSqlCommand"
                => (DbSystems.Postgresql, DbSystemNames.Postgresql),
            "Oracle.EntityFrameworkCore" or
            "Oracle.ManagedDataAccess.Client.OracleCommand" or
            "Devart.Data.Oracle.Entity.EFCore" or
            "Devart.Data.Oracle.OracleCommand"
                => (DbSystems.Oracle, DbSystemNames.OracleDb),
            "FirebirdSql.Data.FirebirdClient.FbCommand" or
            "FirebirdSql.EntityFrameworkCore.Firebird"
                => (DbSystems.Firebird, DbSystemNames.Firebirdsql),
            "Google.Cloud.EntityFrameworkCore.Spanner" or
            "Google.Cloud.Spanner.Data.SpannerCommand"
                => (DbSystems.Spanner, DbSystemNames.GcpSpanner),
            "Teradata.Client.Provider.TdCommand" or
            "Teradata.EntityFrameworkCore"
                => (DbSystems.Teradata, DbSystemNames.Teradata),
            "MongoDB.EntityFrameworkCore"
                => (DbSystems.Mongodb, DbSystemNames.Mongodb),
            "Couchbase.EntityFrameworkCore" or
            "Couchbase.EntityFrameworkCore.Storage.Internal"
                => (DbSystems.Couchbase, DbSystemNames.Couchbase),
            "IBM.EntityFrameworkCore" or
            "IBM.EntityFrameworkCore-lnx" or
            "IBM.EntityFrameworkCore-osx"
                => (DbSystems.Db2, DbSystemNames.IbmDb2),
            //// Otherwise use the fallback defined in the Semantic Conventions
            _ => (DbSystems.OtherSql, DbSystemNames.OtherSql),
        };

    /// <summary>
    /// Returns whether the given provider or command name is SQL-like.
    /// </summary>
    /// <param name="providerOrCommandName">The provider or command name.</param>
    /// <returns>
    /// <see langword="true"/> if the provider or command name is SQL-like; otherwise, <see langword="false"/>.
    /// </returns>
    internal static bool IsSqlLikeProvider(string? providerOrCommandName)
    {
        (_, var dbSystemName) = GetDbSystemNames(providerOrCommandName);

        return dbSystemName switch
        {
            DbSystemNames.Firebirdsql or
            DbSystemNames.GcpSpanner or
            DbSystemNames.IbmDb2 or
            DbSystemNames.MicrosoftSqlServer or
            DbSystemNames.Mysql or
            DbSystemNames.OracleDb or
            DbSystemNames.Postgresql or
            DbSystemNames.Sqlite or
            DbSystemNames.Teradata
              => true,
            _ => false,
        };
    }

    private void AddTag(Activity activity, (string Old, string New) attributes, string? value)
        => this.AddTag(activity, attributes, (value, value));

    private void AddTag(Activity activity, (string Old, string New) attributes, (string? Old, string? New) values)
    {
        if (this.options.EmitOldAttributes)
        {
            activity.AddTag(attributes.Old, values.Old);
        }

        if (this.options.EmitNewAttributes)
        {
            activity.AddTag(attributes.New, values.New);
        }
    }

    private void AddDbSystemNameTag(Activity activity, string? providerOrCommandName)
    {
        var values = GetDbSystemNames(providerOrCommandName);

        // Custom tag for backwards compatibility only
        if (this.options.EmitOldAttributes && (values == (DbSystems.OtherSql, DbSystemNames.OtherSql)))
        {
            activity.AddTag("ef.provider", providerOrCommandName);
        }

        this.AddTag(activity, (SemanticConventions.AttributeDbSystem, SemanticConventions.AttributeDbSystemName), values);
    }

    // v1.36.0 database conventions:
    // https://github.com/open-telemetry/semantic-conventions/tree/v1.36.0/docs/database

    /// <summary>
    /// Known (and used) values for the <c>db.system.name</c> attributes.
    /// </summary>
    private static class DbSystemNames
    {
        public const string OtherSql = "other_sql";
        public const string AzureCosmosDb = "azure.cosmosdb";
        public const string Couchbase = "couchbase";
        public const string Firebirdsql = "firebirdsql";
        public const string GcpSpanner = "gcp.spanner";
        public const string IbmDb2 = "ibm.db2";
        public const string MicrosoftSqlServer = "microsoft.sql_server";
        public const string Mongodb = "mongodb";
        public const string Mysql = "mysql";
        public const string OracleDb = "oracle.db";
        public const string Postgresql = "postgresql";
        public const string Sqlite = "sqlite";
        public const string Teradata = "teradata";
    }

    /// <summary>
    /// Known (and used) values for the <c>db.system</c> attributes.
    /// </summary>
    private static class DbSystems
    {
        public const string OtherSql = "other_sql";
        public const string Cosmosdb = "cosmosdb";
        public const string Couchbase = "couchbase";
        public const string Db2 = "db2";
        public const string Firebird = "firebird";
        public const string Mongodb = "mongodb";
        public const string Mssql = "mssql";
        public const string Mysql = "mysql";
        public const string Oracle = "oracle";
        public const string Postgresql = "postgresql";
        public const string Spanner = "spanner";
        public const string Sqlite = "sqlite";
        public const string Teradata = "teradata";
    }
}

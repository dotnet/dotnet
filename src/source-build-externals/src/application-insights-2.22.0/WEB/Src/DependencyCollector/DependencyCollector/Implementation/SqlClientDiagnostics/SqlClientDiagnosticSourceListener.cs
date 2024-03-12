namespace Microsoft.ApplicationInsights.DependencyCollector.Implementation.SqlClientDiagnostics
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;

    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation.Operation;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    using static Microsoft.ApplicationInsights.DependencyCollector.Implementation.SqlClientDiagnostics.SqlClientDiagnosticFetcherTypes;

    internal class SqlClientDiagnosticSourceListener : IObserver<KeyValuePair<string, object>>, IDisposable
    {
        // Event ids defined at: https://github.com/dotnet/corefx/blob/master/src/System.Data.SqlClient/src/System/Data/SqlClient/SqlClientDiagnosticListenerExtensions.cs
        public const string DiagnosticListenerName = "SqlClientDiagnosticListener";

        public const string SqlBeforeExecuteCommand = SqlClientPrefix + "WriteCommandBefore";
        public const string SqlMicrosoftBeforeExecuteCommand = SqlMicrosoftClientPrefix + "WriteCommandBefore";

        public const string SqlAfterExecuteCommand = SqlClientPrefix + "WriteCommandAfter";
        public const string SqlMicrosoftAfterExecuteCommand = SqlMicrosoftClientPrefix + "WriteCommandAfter";

        public const string SqlErrorExecuteCommand = SqlClientPrefix + "WriteCommandError";
        public const string SqlMicrosoftErrorExecuteCommand = SqlMicrosoftClientPrefix + "WriteCommandError";

        public const string SqlBeforeOpenConnection = SqlClientPrefix + "WriteConnectionOpenBefore";
        public const string SqlMicrosoftBeforeOpenConnection = SqlMicrosoftClientPrefix + "WriteConnectionOpenBefore";

        public const string SqlAfterOpenConnection = SqlClientPrefix + "WriteConnectionOpenAfter";
        public const string SqlMicrosoftAfterOpenConnection = SqlMicrosoftClientPrefix + "WriteConnectionOpenAfter";

        public const string SqlErrorOpenConnection = SqlClientPrefix + "WriteConnectionOpenError";
        public const string SqlMicrosoftErrorOpenConnection = SqlMicrosoftClientPrefix + "WriteConnectionOpenError";

        public const string SqlBeforeCloseConnection = SqlClientPrefix + "WriteConnectionCloseBefore";
        public const string SqlMicrosoftBeforeCloseConnection = SqlMicrosoftClientPrefix + "WriteConnectionCloseBefore";

        public const string SqlAfterCloseConnection = SqlClientPrefix + "WriteConnectionCloseAfter";
        public const string SqlMicrosoftAfterCloseConnection = SqlMicrosoftClientPrefix + "WriteConnectionCloseAfter";

        public const string SqlErrorCloseConnection = SqlClientPrefix + "WriteConnectionCloseError";
        public const string SqlMicrosoftErrorCloseConnection = SqlMicrosoftClientPrefix + "WriteConnectionCloseError";

        public const string SqlBeforeCommitTransaction = SqlClientPrefix + "WriteTransactionCommitBefore";
        public const string SqlMicrosoftBeforeCommitTransaction = SqlMicrosoftClientPrefix + "WriteTransactionCommitBefore";

        public const string SqlAfterCommitTransaction = SqlClientPrefix + "WriteTransactionCommitAfter";
        public const string SqlMicrosoftAfterCommitTransaction = SqlMicrosoftClientPrefix + "WriteTransactionCommitAfter";

        public const string SqlErrorCommitTransaction = SqlClientPrefix + "WriteTransactionCommitError";
        public const string SqlMicrosoftErrorCommitTransaction = SqlMicrosoftClientPrefix + "WriteTransactionCommitError";

        public const string SqlBeforeRollbackTransaction = SqlClientPrefix + "WriteTransactionRollbackBefore";
        public const string SqlMicrosoftBeforeRollbackTransaction = SqlMicrosoftClientPrefix + "WriteTransactionRollbackBefore";

        public const string SqlAfterRollbackTransaction = SqlClientPrefix + "WriteTransactionRollbackAfter";
        public const string SqlMicrosoftAfterRollbackTransaction = SqlMicrosoftClientPrefix + "WriteTransactionRollbackAfter";

        public const string SqlErrorRollbackTransaction = SqlClientPrefix + "WriteTransactionRollbackError";
        public const string SqlMicrosoftErrorRollbackTransaction = SqlMicrosoftClientPrefix + "WriteTransactionRollbackError";

        private const string SqlClientPrefix = "System.Data.SqlClient.";
        private const string SqlMicrosoftClientPrefix = "Microsoft.Data.SqlClient.";

        private static readonly ActiveSubsciptionManager SubscriptionManager = new ActiveSubsciptionManager();
        private readonly TelemetryClient client;
        private readonly SqlClientDiagnosticSourceSubscriber subscriber;

        private readonly ObjectInstanceBasedOperationHolder<DependencyTelemetry> operationHolder = new ObjectInstanceBasedOperationHolder<DependencyTelemetry>();
        private readonly bool collectCommandText;

        public SqlClientDiagnosticSourceListener(TelemetryConfiguration configuration, bool collectCommandText)
        {
            this.client = new TelemetryClient(configuration);
            this.client.Context.GetInternalContext().SdkVersion =
                SdkVersionUtils.GetSdkVersion("rdd" + RddSource.DiagnosticSourceCore + ":");

            this.subscriber = new SqlClientDiagnosticSourceSubscriber(this);
            this.collectCommandText = collectCommandText;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        void IObserver<KeyValuePair<string, object>>.OnCompleted()
        {
        }

        void IObserver<KeyValuePair<string, object>>.OnError(Exception error)
        {
        }

        void IObserver<KeyValuePair<string, object>>.OnNext(KeyValuePair<string, object> evnt)
        {
            try
            {
                // It's possible to host multiple apps (ASP.NET Core or generic hosts) in the same process
                // Each of this apps has it's own DependencyTrackingModule and corresponding SQL listener.
                // We should ignore events for all of them except one
                if (!SubscriptionManager.IsActive(this))
                {
                    DependencyCollectorEventSource.Log.NotActiveListenerNoTracking(evnt.Key, Activity.Current?.Id);
                    return;
                }

                switch (evnt.Key)
                {
                    case SqlBeforeExecuteCommand:
                        {
                            this.BeforeExecuteHelper(evnt, CommandBefore.OperationId,
                                CommandBefore.Command,
                                CommandBefore.CommandText,
                                CommandBefore.Connection,
                                CommandBefore.DataSource,
                                CommandBefore.Database,
                                CommandBefore.CommandType,
                                CommandBefore.Timestamp);
                            break;
                        }

                    case SqlMicrosoftBeforeExecuteCommand:
                        {
                            this.BeforeExecuteHelper(evnt, CommandBeforeMicrosoft.OperationId,
                                CommandBeforeMicrosoft.Command,
                                CommandBeforeMicrosoft.CommandText,
                                CommandBeforeMicrosoft.Connection,
                                CommandBeforeMicrosoft.DataSource,
                                CommandBeforeMicrosoft.Database,
                                CommandBeforeMicrosoft.CommandType,
                                CommandBeforeMicrosoft.Timestamp);
                            break;
                        }

                    case SqlAfterExecuteCommand:
                        {
                            this.AfterExecuteHelper(evnt, CommandAfter.OperationId, CommandAfter.Command, CommandAfter.Timestamp);
                            break;
                        }

                    case SqlMicrosoftAfterExecuteCommand:
                        {
                            this.AfterExecuteHelper(evnt, CommandAfterMicrosoft.OperationId,
                                CommandAfterMicrosoft.Command, CommandAfterMicrosoft.Timestamp);
                            break;
                        }

                    case SqlErrorExecuteCommand:
                        {
                            this.ErrorExecuteHelper(evnt, CommandError.OperationId, CommandError.Command, CommandError.Timestamp,
                                CommandError.Exception, CommandError.Number);
                            break;
                        }

                    case SqlMicrosoftErrorExecuteCommand:
                        {
                            this.ErrorExecuteHelper(evnt, CommandErrorMicrosoft.OperationId, CommandErrorMicrosoft.Command,
                                CommandErrorMicrosoft.Timestamp, CommandErrorMicrosoft.Exception, CommandErrorMicrosoft.Number);
                            break;
                        }

                    case SqlBeforeOpenConnection:
                        {
                            this.BeforeOpenConnectionHelper(evnt, ConnectionBefore.OperationId,
                                ConnectionBefore.Connection,
                                ConnectionBefore.Operation,
                                ConnectionBefore.Timestamp,
                                ConnectionBefore.DataSource,
                                ConnectionBefore.Database);
                            break;
                        }

                    case SqlMicrosoftBeforeOpenConnection:
                        {
                            this.BeforeOpenConnectionHelper(evnt, ConnectionBeforeMicrosoft.OperationId,
                                ConnectionBeforeMicrosoft.Connection,
                                ConnectionBeforeMicrosoft.Operation,
                                ConnectionBeforeMicrosoft.Timestamp,
                                ConnectionBeforeMicrosoft.DataSource,
                                ConnectionBeforeMicrosoft.Database);
                            break;
                        }

                    case SqlAfterOpenConnection:
                        {
                            this.AfterOpenConnectionHelper(evnt, ConnectionAfter.OperationId, ConnectionAfter.Connection);
                            break;
                        }

                    case SqlMicrosoftAfterOpenConnection:
                        {
                            this.AfterOpenConnectionHelper(evnt, ConnectionAfterMicrosoft.OperationId, ConnectionAfterMicrosoft.Connection);
                            break;
                        }

                    case SqlErrorOpenConnection:
                        {
                            this.ErrorOpenConnectionHelper(evnt, ConnectionError.OperationId, ConnectionError.Connection,
                                ConnectionError.Timestamp, ConnectionError.Exception, ConnectionError.Number);
                            break;
                        }

                    case SqlMicrosoftErrorOpenConnection:
                        {
                            this.ErrorOpenConnectionHelper(evnt, ConnectionErrorMicrosoft.OperationId, ConnectionErrorMicrosoft.Connection,
                                ConnectionErrorMicrosoft.Timestamp, ConnectionErrorMicrosoft.Exception, ConnectionErrorMicrosoft.Number);
                            break;
                        }

                    case SqlBeforeCommitTransaction:
                        {
                            this.BeforeCommitHelper(evnt, TransactionCommitBefore.OperationId,
                                TransactionCommitBefore.Connection,
                                TransactionCommitBefore.Operation,
                                TransactionCommitBefore.Timestamp,
                                TransactionCommitBefore.IsolationLevel,
                                TransactionCommitBefore.DataSource,
                                TransactionCommitBefore.Database);
                            break;
                        }

                    case SqlMicrosoftBeforeCommitTransaction:
                        {
                            this.BeforeCommitHelper(evnt, TransactionCommitBeforeMicrosoft.OperationId,
                                TransactionCommitBeforeMicrosoft.Connection,
                                TransactionCommitBeforeMicrosoft.Operation,
                                TransactionCommitBeforeMicrosoft.Timestamp,
                                TransactionCommitBeforeMicrosoft.IsolationLevel,
                                TransactionCommitBeforeMicrosoft.DataSource,
                                TransactionCommitBeforeMicrosoft.Database);
                            break;
                        }

                    case SqlBeforeRollbackTransaction:
                        {
                            this.BeforeRollbackHelper(evnt, TransactionRollbackBefore.OperationId,
                                TransactionRollbackBefore.Connection,
                                TransactionRollbackBefore.Operation,
                                TransactionRollbackBefore.Timestamp,
                                TransactionRollbackBefore.IsolationLevel,
                                TransactionRollbackBefore.DataSource,
                                TransactionRollbackBefore.Database);
                            break;
                        }

                    case SqlMicrosoftBeforeRollbackTransaction:
                        {
                            this.BeforeRollbackHelper(evnt, TransactionRollbackBeforeMicrosoft.OperationId,
                                TransactionRollbackBeforeMicrosoft.Connection,
                                TransactionRollbackBeforeMicrosoft.Operation,
                                TransactionRollbackBeforeMicrosoft.Timestamp,
                                TransactionRollbackBeforeMicrosoft.IsolationLevel,
                                TransactionRollbackBeforeMicrosoft.DataSource,
                                TransactionRollbackBeforeMicrosoft.Database);
                            break;
                        }

                    case SqlAfterCommitTransaction:
                        {
                            this.AfterCommitHelper(evnt, TransactionCommitAfter.OperationId,
                                TransactionCommitAfter.Connection, TransactionCommitAfter.Timestamp);
                            break;
                        }

                    case SqlMicrosoftAfterCommitTransaction:
                        {
                            this.AfterCommitHelper(evnt, TransactionCommitAfterMicrosoft.OperationId,
                                TransactionCommitAfterMicrosoft.Connection, TransactionCommitAfterMicrosoft.Timestamp);
                            break;
                        }

                    case SqlAfterRollbackTransaction:
                        {
                            this.AfterRollBackHelper(evnt, TransactionRollbackAfter.OperationId,
                                TransactionRollbackAfter.Connection,
                                TransactionRollbackAfter.Timestamp);
                            break;
                        }

                    case SqlMicrosoftAfterRollbackTransaction:
                        {
                            this.AfterRollBackHelper(evnt, TransactionRollbackAfterMicrosoft.OperationId,
                                TransactionRollbackAfterMicrosoft.Connection,
                                TransactionRollbackAfterMicrosoft.Timestamp);
                            break;
                        }

                    case SqlErrorCommitTransaction:
                        {
                            this.ErrorCommitHelper(evnt, TransactionCommitError.OperationId,
                                TransactionCommitError.Connection,
                                TransactionCommitError.Timestamp,
                                TransactionCommitError.Exception,
                                TransactionCommitError.Number);
                            break;
                        }

                    case SqlMicrosoftErrorCommitTransaction:
                        {
                            this.ErrorCommitHelper(evnt, TransactionCommitErrorMicrosoft.OperationId,
                                TransactionCommitErrorMicrosoft.Connection,
                                TransactionCommitErrorMicrosoft.Timestamp,
                                TransactionCommitErrorMicrosoft.Exception,
                                TransactionCommitErrorMicrosoft.Number);
                            break;
                        }

                    case SqlErrorRollbackTransaction:
                        {
                            this.ErrorRollbackHelper(evnt, TransactionRollbackError.OperationId,
                                TransactionRollbackError.Connection,
                                TransactionRollbackError.Timestamp,
                                TransactionRollbackError.Exception,
                                TransactionRollbackError.Number);
                            break;
                        }

                    case SqlMicrosoftErrorRollbackTransaction:
                        {
                            this.ErrorRollbackHelper(evnt, TransactionRollbackErrorMicrosoft.OperationId,
                                TransactionRollbackErrorMicrosoft.Connection,
                                TransactionRollbackErrorMicrosoft.Timestamp,
                                TransactionRollbackErrorMicrosoft.Exception,
                                TransactionRollbackErrorMicrosoft.Number);
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                DependencyCollectorEventSource.Log
                    .SqlClientDiagnosticSourceListenerOnNextFailed(ExceptionUtilities.GetExceptionDetailString(ex));
            }
        }

        private static void InitializeTelemetry(DependencyTelemetry telemetry, Guid operationId, long timestamp)
        {
            telemetry.Start(timestamp);

            var activity = Activity.Current;

            if (activity != null)
            {
                // SQL Client does NOT create Activity.
                // We initialize SQL dependency using Activity from incoming Request
                // and it is the parent of the SQL dependency

                if (activity.IdFormat == ActivityIdFormat.W3C)
                {
                    var traceId = activity.TraceId.ToHexString();
                    telemetry.Context.Operation.Id = traceId;
                    telemetry.Context.Operation.ParentId = activity.SpanId.ToHexString();
                }
                else
                {
                    telemetry.Context.Operation.Id = activity.RootId;
                    telemetry.Context.Operation.ParentId = activity.Id;
                }

                foreach (var item in activity.Baggage)
                {
                    if (!telemetry.Properties.ContainsKey(item.Key))
                    {
                        telemetry.Properties[item.Key] = item.Value;
                    }
                }
            }
            else
            {
                telemetry.Context.Operation.Id = operationId.ToStringInvariant("N");
            }
        }

        private static void ConfigureExceptionTelemetry(DependencyTelemetry telemetry, Exception exception, PropertyFetcher numberFetcher)
        {
            telemetry.Success = false;
            telemetry.Properties["Exception"] = exception.ToInvariantString();

            try
            {
                var exceptionNumber = (int)numberFetcher.Fetch(exception);
                telemetry.ResultCode = exceptionNumber.ToString(CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                // Ignore as it simply indicate exception was not a SqlException
            }
        }

        private void BeforeExecuteHelper(KeyValuePair<string, object> evnt,
            PropertyFetcher operationIdFetcher,
            PropertyFetcher commandFetcher,
            PropertyFetcher commandTextFetcher,
            PropertyFetcher connectionFetcher,
            PropertyFetcher dataSourceFetcher,
            PropertyFetcher databaseFetcher,
            PropertyFetcher commandTypeFetcher,
            PropertyFetcher timeStampFetcher)
        {
            var fet = CommandBefore.OperationId;
            var operationId = (Guid)operationIdFetcher.Fetch(evnt.Value);
            DependencyCollectorEventSource.Log.SqlClientDiagnosticSubscriberCallbackCalled(operationId, evnt.Key);

            var command = commandFetcher.Fetch(evnt.Value);

            if (this.operationHolder.Get(command) == null)
            {
                var dependencyName = string.Empty;
                var target = string.Empty;
                var commandType = (int)commandTypeFetcher.Fetch(command);
                var commandText = string.Empty;

                // https://docs.microsoft.com/dotnet/api/system.data.commandtype
                // 4 indicate StoredProcedure
                if (this.collectCommandText)
                {
                    commandText = (string)commandTextFetcher.Fetch(command);
                }

                var con = connectionFetcher.Fetch(command);
                if (con != null)
                {
                    var dataSource = dataSourceFetcher.Fetch(con);
                    var database = databaseFetcher.Fetch(con);
                    target = string.Join(" | ", dataSource, database);

                    // https://docs.microsoft.com/dotnet/api/system.data.commandtype
                    // 4 indicate StoredProcedure
                    var commandName = commandType == 4
                        ? commandText
                        : string.Empty;

                    dependencyName = string.IsNullOrEmpty(commandName)
                        ? string.Join(" | ", dataSource, database)
                        : string.Join(" | ", dataSource, database, commandName);
                }

                var timestamp = timeStampFetcher.Fetch(evnt.Value) as long?
                            ?? Stopwatch.GetTimestamp(); // TODO corefx#20748 - timestamp missing from event data

                var telemetry = new DependencyTelemetry()
                {
                    Id = operationId.ToStringInvariant("N"),
                    Name = dependencyName,
                    Type = RemoteDependencyConstants.SQL,
                    Target = target,
                    Data = commandText,
                    Success = true,
                };

                // Populate the operation details for initializers
                telemetry.SetOperationDetail(OperationDetailConstants.SqlCommandOperationDetailName, command);

                InitializeTelemetry(telemetry, operationId, timestamp);

                this.operationHolder.Store(command, Tuple.Create(telemetry, /* isCustomCreated: */ false));
            }
            else
            {
                DependencyCollectorEventSource.Log.TrackingAnExistingTelemetryItemVerbose();
            }
        }

        private void AfterExecuteHelper(KeyValuePair<string, object> evnt,
            PropertyFetcher operationIdFetcher,
            PropertyFetcher commandFetcher,
            PropertyFetcher timestampFetcher)
        {
            var operationId = (Guid)operationIdFetcher.Fetch(evnt.Value);

            DependencyCollectorEventSource.Log.SqlClientDiagnosticSubscriberCallbackCalled(operationId, evnt.Key);

            var command = commandFetcher.Fetch(evnt.Value);
            var tuple = this.operationHolder.Get(command);

            if (tuple != null)
            {
                this.operationHolder.Remove(command);

                var telemetry = tuple.Item1;

                var timestamp = (long)timestampFetcher.Fetch(evnt.Value);

                telemetry.Stop(timestamp);

                this.client.TrackDependency(telemetry);
            }
            else
            {
                DependencyCollectorEventSource.Log.EndCallbackWithNoBegin(operationId.ToStringInvariant("N"));
            }
        }

        private void ErrorExecuteHelper(KeyValuePair<string, object> evnt,
            PropertyFetcher operationIdFetcher,
            PropertyFetcher commandFetcher,
            PropertyFetcher timestampFetcher,
            PropertyFetcher exceptionFetcher,
            PropertyFetcher numberFetcher)
        {
            var operationId = (Guid)operationIdFetcher.Fetch(evnt.Value);

            DependencyCollectorEventSource.Log.SqlClientDiagnosticSubscriberCallbackCalled(operationId, evnt.Key);

            var command = commandFetcher.Fetch(evnt.Value);
            var tuple = this.operationHolder.Get(command);

            if (tuple != null)
            {
                this.operationHolder.Remove(command);

                var telemetry = tuple.Item1;

                var timestamp = (long)timestampFetcher.Fetch(evnt.Value);

                telemetry.Stop(timestamp);

                var exception = (Exception)exceptionFetcher.Fetch(evnt.Value);

                ConfigureExceptionTelemetry(telemetry, exception, numberFetcher);

                this.client.TrackDependency(telemetry);
            }
            else
            {
                DependencyCollectorEventSource.Log.EndCallbackWithNoBegin(operationId.ToStringInvariant("N"));
            }
        }

        private void BeforeOpenConnectionHelper(KeyValuePair<string, object> evnt,
            PropertyFetcher operationIdFetcher,
            PropertyFetcher connectionFetcher,
            PropertyFetcher operationFetcher,
            PropertyFetcher timestampFetcher,
            PropertyFetcher dataSourceFetcher,
            PropertyFetcher databaseFetcher)
        {
            var operationId = (Guid)operationIdFetcher.Fetch(evnt.Value);

            DependencyCollectorEventSource.Log.SqlClientDiagnosticSubscriberCallbackCalled(operationId, evnt.Key);

            var connection = connectionFetcher.Fetch(evnt.Value);

            if (this.operationHolder.Get(connection) == null)
            {
                var operation = (string)operationFetcher.Fetch(evnt.Value);
                var timestamp = (long)timestampFetcher.Fetch(evnt.Value);
                var dataSource = (string)dataSourceFetcher.Fetch(connection);
                var database = (string)databaseFetcher.Fetch(connection);
                var telemetry = new DependencyTelemetry()
                {
                    Id = operationId.ToStringInvariant("N"),
                    Name = string.Join(" | ", dataSource, database, operation),
                    Type = RemoteDependencyConstants.SQL,
                    Target = string.Join(" | ", dataSource, database),
                    Data = operation,
                    Success = true,
                };

                InitializeTelemetry(telemetry, operationId, timestamp);

                this.operationHolder.Store(connection, Tuple.Create(telemetry, /* isCustomCreated: */ false));
            }
            else
            {
                DependencyCollectorEventSource.Log.TrackingAnExistingTelemetryItemVerbose();
            }
        }

        private void AfterOpenConnectionHelper(KeyValuePair<string, object> evnt,
            PropertyFetcher operationIdFetcher,
            PropertyFetcher connectionFetcher)
        {
            var operationId = (Guid)operationIdFetcher.Fetch(evnt.Value);

            DependencyCollectorEventSource.Log.SqlClientDiagnosticSubscriberCallbackCalled(operationId, evnt.Key);

            var connection = connectionFetcher.Fetch(evnt.Value);
            var tuple = this.operationHolder.Get(connection);

            if (tuple != null)
            {
                this.operationHolder.Remove(connection);
            }
            else
            {
                DependencyCollectorEventSource.Log.EndCallbackWithNoBegin(operationId.ToStringInvariant("N"));
            }
        }

        private void ErrorOpenConnectionHelper(KeyValuePair<string, object> evnt,
            PropertyFetcher operationIdFetcher,
            PropertyFetcher connectionFetcher,
            PropertyFetcher timestampFetcher,
            PropertyFetcher exceptionFetcher,
            PropertyFetcher numberFetcher)
        {
            var operationId = (Guid)operationIdFetcher.Fetch(evnt.Value);

            DependencyCollectorEventSource.Log.SqlClientDiagnosticSubscriberCallbackCalled(operationId, evnt.Key);

            var connection = connectionFetcher.Fetch(evnt.Value);
            var tuple = this.operationHolder.Get(connection);

            if (tuple != null)
            {
                this.operationHolder.Remove(connection);

                var telemetry = tuple.Item1;

                var timestamp = (long)timestampFetcher.Fetch(evnt.Value);

                telemetry.Stop(timestamp);

                var exception = (Exception)exceptionFetcher.Fetch(evnt.Value);

                ConfigureExceptionTelemetry(telemetry, exception, numberFetcher);

                this.client.TrackDependency(telemetry);
            }
            else
            {
                DependencyCollectorEventSource.Log.EndCallbackWithNoBegin(operationId.ToStringInvariant("N"));
            }
        }

        private void BeforeCommitHelper(KeyValuePair<string, object> evnt,
            PropertyFetcher operationIdFetcher,
            PropertyFetcher connectionFetcher,
            PropertyFetcher operationFetcher,
            PropertyFetcher timestampFetcher,
            PropertyFetcher isolationFetcher,
            PropertyFetcher datasourceFetcher,
            PropertyFetcher databaseFetcher)
        {
            var operationId = (Guid)operationIdFetcher.Fetch(evnt.Value);

            DependencyCollectorEventSource.Log.SqlClientDiagnosticSubscriberCallbackCalled(operationId, evnt.Key);

            var connection = connectionFetcher.Fetch(evnt.Value);

            if (this.operationHolder.Get(connection) == null)
            {
                var operation = (string)operationFetcher.Fetch(evnt.Value);
                var timestamp = (long)timestampFetcher.Fetch(evnt.Value);
                var isolationLevel = isolationFetcher.Fetch(evnt.Value);
                var dataSource = (string)datasourceFetcher.Fetch(connection);
                var database = (string)databaseFetcher.Fetch(connection);

                var telemetry = new DependencyTelemetry()
                {
                    Id = operationId.ToStringInvariant("N"),
                    Name = string.Join(" | ", dataSource, database, operation, isolationLevel),
                    Type = RemoteDependencyConstants.SQL,
                    Target = string.Join(" | ", dataSource, database),
                    Data = operation,
                    Success = true,
                };

                InitializeTelemetry(telemetry, operationId, timestamp);

                this.operationHolder.Store(connection, Tuple.Create(telemetry, /* isCustomCreated: */ false));
            }
            else
            {
                DependencyCollectorEventSource.Log.TrackingAnExistingTelemetryItemVerbose();
            }
        }

        private void BeforeRollbackHelper(KeyValuePair<string, object> evnt,
            PropertyFetcher operationIdFetcher,
            PropertyFetcher connectionFetcher,
            PropertyFetcher operationFetcher,
            PropertyFetcher timestampFetcher,
            PropertyFetcher isolationFetcher,
            PropertyFetcher datasourceFetcher,
            PropertyFetcher databaseFetcher)
        {
            {
                var operationId = (Guid)operationIdFetcher.Fetch(evnt.Value);

                DependencyCollectorEventSource.Log.SqlClientDiagnosticSubscriberCallbackCalled(operationId, evnt.Key);

                var connection = connectionFetcher.Fetch(evnt.Value);

                if (this.operationHolder.Get(connection) == null)
                {
                    var operation = (string)operationFetcher.Fetch(evnt.Value);
                    var timestamp = (long)timestampFetcher.Fetch(evnt.Value);
                    var isolationLevel = isolationFetcher.Fetch(evnt.Value);
                    var dataSource = (string)datasourceFetcher.Fetch(connection);
                    var database = (string)databaseFetcher.Fetch(connection);

                    var telemetry = new DependencyTelemetry()
                    {
                        Id = operationId.ToStringInvariant("N"),
                        Name = string.Join(" | ", dataSource, database, operation, isolationLevel),
                        Type = RemoteDependencyConstants.SQL,
                        Target = string.Join(" | ", dataSource, database),
                        Data = operation,
                        Success = true,
                    };

                    InitializeTelemetry(telemetry, operationId, timestamp);

                    this.operationHolder.Store(connection, Tuple.Create(telemetry, /* isCustomCreated: */ false));
                }
                else
                {
                    DependencyCollectorEventSource.Log.TrackingAnExistingTelemetryItemVerbose();
                }
            }
        }

        private void AfterCommitHelper(KeyValuePair<string, object> evnt,
            PropertyFetcher operationIdFetcher,
            PropertyFetcher connectionFetcher,
            PropertyFetcher timestampFetcher)
        {
            var operationId = (Guid)operationIdFetcher.Fetch(evnt.Value);

            DependencyCollectorEventSource.Log.SqlClientDiagnosticSubscriberCallbackCalled(operationId,
                evnt.Key);

            var connection = connectionFetcher.Fetch(evnt.Value);
            var tuple = this.operationHolder.Get(connection);

            if (tuple != null)
            {
                this.operationHolder.Remove(connection);

                var telemetry = tuple.Item1;

                var timestamp = (long)timestampFetcher.Fetch(evnt.Value);

                telemetry.Stop(timestamp);

                this.client.TrackDependency(telemetry);
            }
            else
            {
                DependencyCollectorEventSource.Log.EndCallbackWithNoBegin(
                    operationId.ToStringInvariant("N"));
            }
        }

        private void AfterRollBackHelper(KeyValuePair<string, object> evnt,
            PropertyFetcher operationIdFetcher,
            PropertyFetcher connectionFetcher,
            PropertyFetcher timestampFetcher)
        {
            var operationId = (Guid)operationIdFetcher.Fetch(evnt.Value);

            DependencyCollectorEventSource.Log.SqlClientDiagnosticSubscriberCallbackCalled(operationId, evnt.Key);

            var connection = connectionFetcher.Fetch(evnt.Value);
            var tuple = this.operationHolder.Get(connection);

            if (tuple != null)
            {
                this.operationHolder.Remove(connection);

                var telemetry = tuple.Item1;

                var timestamp = (long)timestampFetcher.Fetch(evnt.Value);

                telemetry.Stop(timestamp);

                this.client.TrackDependency(telemetry);
            }
            else
            {
                DependencyCollectorEventSource.Log.EndCallbackWithNoBegin(operationId.ToStringInvariant("N"));
            }
        }

        private void ErrorCommitHelper(KeyValuePair<string, object> evnt,
            PropertyFetcher operationIdFetcher,
            PropertyFetcher connectionFetcher,
            PropertyFetcher timestampFetcher,
            PropertyFetcher exceptionFetcher,
            PropertyFetcher numberFetcher)
        {
            var operationId = (Guid)operationIdFetcher.Fetch(evnt.Value);

            DependencyCollectorEventSource.Log.SqlClientDiagnosticSubscriberCallbackCalled(operationId, evnt.Key);

            var connection = connectionFetcher.Fetch(evnt.Value);
            var tuple = this.operationHolder.Get(connection);

            if (tuple != null)
            {
                this.operationHolder.Remove(connection);

                var telemetry = tuple.Item1;

                var timestamp = (long)timestampFetcher.Fetch(evnt.Value);

                telemetry.Stop(timestamp);

                var exception = (Exception)exceptionFetcher.Fetch(evnt.Value);

                ConfigureExceptionTelemetry(telemetry, exception, numberFetcher);

                this.client.TrackDependency(telemetry);
            }
            else
            {
                DependencyCollectorEventSource.Log.EndCallbackWithNoBegin(operationId.ToStringInvariant("N"));
            }
        }

        private void ErrorRollbackHelper(KeyValuePair<string, object> evnt,
            PropertyFetcher operationIdFetcher,
            PropertyFetcher connectionFetcher,
            PropertyFetcher timestampFetcher,
            PropertyFetcher exceptionFetcher,
            PropertyFetcher numberFetcher)
        {
            var operationId = (Guid)operationIdFetcher.Fetch(evnt.Value);

            DependencyCollectorEventSource.Log.SqlClientDiagnosticSubscriberCallbackCalled(operationId, evnt.Key);

            var connection = connectionFetcher.Fetch(evnt.Value);
            var tuple = this.operationHolder.Get(connection);

            if (tuple != null)
            {
                this.operationHolder.Remove(connection);

                var telemetry = tuple.Item1;

                var timestamp = (long)timestampFetcher.Fetch(evnt.Value);

                telemetry.Stop(timestamp);

                var exception = (Exception)exceptionFetcher.Fetch(evnt.Value);

                ConfigureExceptionTelemetry(telemetry, exception, numberFetcher);

                this.client.TrackDependency(telemetry);
            }
            else
            {
                DependencyCollectorEventSource.Log.EndCallbackWithNoBegin(operationId.ToStringInvariant("N"));
            }
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.subscriber != null)
                {
                    this.subscriber.Dispose();
                }
            }
        }

        private sealed class SqlClientDiagnosticSourceSubscriber : IObserver<DiagnosticListener>, IDisposable
        {
            private readonly SqlClientDiagnosticSourceListener sqlDiagnosticListener;
            private readonly IDisposable listenerSubscription;

            private IDisposable eventSubscription;

            internal SqlClientDiagnosticSourceSubscriber(SqlClientDiagnosticSourceListener listener)
            {
                this.sqlDiagnosticListener = listener;

                try
                {
                    this.listenerSubscription = DiagnosticListener.AllListeners.Subscribe(this);
                }
                catch (Exception ex)
                {
                    DependencyCollectorEventSource.Log.SqlClientDiagnosticSubscriberFailedToSubscribe(ex.ToInvariantString());
                }

                SubscriptionManager.Attach(this.sqlDiagnosticListener);
            }

            public void OnNext(DiagnosticListener value)
            {
                if (value != null)
                {
                    if (value.Name == DiagnosticListenerName)
                    {
                        this.eventSubscription = value.Subscribe(this.sqlDiagnosticListener);
                    }
                }
            }

            public void OnCompleted()
            {
            }

            public void OnError(Exception error)
            {
            }

            public void Dispose()
            {
                SubscriptionManager.Detach(this.sqlDiagnosticListener);
                this.eventSubscription?.Dispose();

                this.listenerSubscription?.Dispose();
            }
        }
    }
}
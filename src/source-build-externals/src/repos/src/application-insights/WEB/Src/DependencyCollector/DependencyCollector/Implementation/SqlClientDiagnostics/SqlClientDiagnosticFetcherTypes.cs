namespace Microsoft.ApplicationInsights.DependencyCollector.Implementation.SqlClientDiagnostics
{
    using Microsoft.ApplicationInsights.Common;

    internal static class SqlClientDiagnosticFetcherTypes
    {
        //// These types map to the anonymous types defined here: 
        //// System.Data.SqlClient.SqlClientDiagnosticListenerExtensions. 
        //// and 
        //// Microsoft.Data.SqlClient.SqlClientDiagnosticListenerExtensions. 
        //// http://github.com/dotnet/corefx/blob/master/src/System.Data.SqlClient/src/System/Data/SqlClient/SqlClientDiagnosticListenerExtensions.cs
        //// https://github.com/dotnet/SqlClient/blob/master/src/Microsoft.Data.SqlClient/netcore/src/Microsoft/Data/SqlClient/SqlClientDiagnosticListenerExtensions.cs

        #region "System.Data fetchers"

        /// <summary> Fetchers for execute command before event. </summary>
        internal static class CommandBefore
        {
            public static readonly PropertyFetcher OperationId = new PropertyFetcher(nameof(OperationId));
            public static readonly PropertyFetcher Command = new PropertyFetcher(nameof(Command));
            public static readonly PropertyFetcher Connection = new PropertyFetcher(nameof(Connection));
            public static readonly PropertyFetcher DataSource = new PropertyFetcher(nameof(DataSource));
            public static readonly PropertyFetcher Database = new PropertyFetcher(nameof(Database));
            public static readonly PropertyFetcher CommandType = new PropertyFetcher(nameof(CommandType));
            public static readonly PropertyFetcher CommandText = new PropertyFetcher(nameof(CommandText));
            public static readonly PropertyFetcher Timestamp = new PropertyFetcher(nameof(Timestamp));
        }

        /// <summary> Fetchers for execute command after event. </summary>
        internal static class CommandAfter
        {
            public static readonly PropertyFetcher OperationId = new PropertyFetcher(nameof(OperationId));
            public static readonly PropertyFetcher Command = new PropertyFetcher(nameof(Command));
            public static readonly PropertyFetcher Timestamp = new PropertyFetcher(nameof(Timestamp));
        }

        /// <summary> Fetchers for execute command error event. </summary>
        internal static class CommandError
        {
            public static readonly PropertyFetcher OperationId = new PropertyFetcher(nameof(OperationId));
            public static readonly PropertyFetcher Command = new PropertyFetcher(nameof(Command));
            public static readonly PropertyFetcher Exception = new PropertyFetcher(nameof(Exception));
            public static readonly PropertyFetcher Timestamp = new PropertyFetcher(nameof(Timestamp));
            public static readonly PropertyFetcher Number = new PropertyFetcher(nameof(Number));
        }

        /// <summary> Fetchers for connection open/close before events. </summary>
        internal static class ConnectionBefore
        {
            public static readonly PropertyFetcher OperationId = new PropertyFetcher(nameof(OperationId));
            public static readonly PropertyFetcher Operation = new PropertyFetcher(nameof(Operation));
            public static readonly PropertyFetcher Connection = new PropertyFetcher(nameof(Connection));
            public static readonly PropertyFetcher Timestamp = new PropertyFetcher(nameof(Timestamp));
            public static readonly PropertyFetcher DataSource = new PropertyFetcher(nameof(DataSource));
            public static readonly PropertyFetcher Database = new PropertyFetcher(nameof(Database));
        }

        /// <summary> Fetchers for connection open/close after events. </summary>
        internal static class ConnectionAfter
        {
            public static readonly PropertyFetcher OperationId = new PropertyFetcher(nameof(OperationId));
            public static readonly PropertyFetcher Connection = new PropertyFetcher(nameof(Connection));
        }

        /// <summary> Fetchers for connection open/close error events. </summary>
        internal static class ConnectionError
        {
            public static readonly PropertyFetcher OperationId = new PropertyFetcher(nameof(OperationId));
            public static readonly PropertyFetcher Connection = new PropertyFetcher(nameof(Connection));
            public static readonly PropertyFetcher Exception = new PropertyFetcher(nameof(Exception));
            public static readonly PropertyFetcher Timestamp = new PropertyFetcher(nameof(Timestamp));
            public static readonly PropertyFetcher Number = new PropertyFetcher(nameof(Number));
        }

        /// <summary> Fetchers for transaction commit before events. </summary>
        internal static class TransactionCommitBefore
        {
            public static readonly PropertyFetcher OperationId = new PropertyFetcher(nameof(OperationId));
            public static readonly PropertyFetcher Operation = new PropertyFetcher(nameof(Operation));
            public static readonly PropertyFetcher IsolationLevel = new PropertyFetcher(nameof(IsolationLevel));
            public static readonly PropertyFetcher Connection = new PropertyFetcher(nameof(Connection));
            public static readonly PropertyFetcher DataSource = new PropertyFetcher(nameof(DataSource));
            public static readonly PropertyFetcher Database = new PropertyFetcher(nameof(Database));
            public static readonly PropertyFetcher Timestamp = new PropertyFetcher(nameof(Timestamp));
        }

        /// <summary> Fetchers for transaction rollback before events. </summary>
        internal static class TransactionRollbackBefore
        {
            public static readonly PropertyFetcher OperationId = new PropertyFetcher(nameof(OperationId));
            public static readonly PropertyFetcher Operation = new PropertyFetcher(nameof(Operation));
            public static readonly PropertyFetcher IsolationLevel = new PropertyFetcher(nameof(IsolationLevel));
            public static readonly PropertyFetcher Connection = new PropertyFetcher(nameof(Connection));
            public static readonly PropertyFetcher DataSource = new PropertyFetcher(nameof(DataSource));
            public static readonly PropertyFetcher Database = new PropertyFetcher(nameof(Database));
            public static readonly PropertyFetcher TransactionName = new PropertyFetcher(nameof(TransactionName));
            public static readonly PropertyFetcher Timestamp = new PropertyFetcher(nameof(Timestamp));
        }

        /// <summary> Fetchers for transaction rollback after events. </summary>
        internal static class TransactionRollbackAfter
        {
            public static readonly PropertyFetcher OperationId = new PropertyFetcher(nameof(OperationId));
            public static readonly PropertyFetcher Connection = new PropertyFetcher(nameof(Connection));
            public static readonly PropertyFetcher Timestamp = new PropertyFetcher(nameof(Timestamp));
        }

        /// <summary> Fetchers for transaction commit after events. </summary>
        internal static class TransactionCommitAfter
        {
            public static readonly PropertyFetcher OperationId = new PropertyFetcher(nameof(OperationId));
            public static readonly PropertyFetcher Connection = new PropertyFetcher(nameof(Connection));
            public static readonly PropertyFetcher Timestamp = new PropertyFetcher(nameof(Timestamp));
        }

        /// <summary> Fetchers for transaction rollback error events. </summary>
        internal static class TransactionRollbackError
        {
            public static readonly PropertyFetcher OperationId = new PropertyFetcher(nameof(OperationId));
            public static readonly PropertyFetcher Connection = new PropertyFetcher(nameof(Connection));
            public static readonly PropertyFetcher Exception = new PropertyFetcher(nameof(Exception));
            public static readonly PropertyFetcher Timestamp = new PropertyFetcher(nameof(Timestamp));
            public static readonly PropertyFetcher Number = new PropertyFetcher(nameof(Number));
        }

        /// <summary> Fetchers for transaction commit error events. </summary>
        internal static class TransactionCommitError
        {
            public static readonly PropertyFetcher OperationId = new PropertyFetcher(nameof(OperationId));
            public static readonly PropertyFetcher Connection = new PropertyFetcher(nameof(Connection));
            public static readonly PropertyFetcher Exception = new PropertyFetcher(nameof(Exception));
            public static readonly PropertyFetcher Timestamp = new PropertyFetcher(nameof(Timestamp));
            public static readonly PropertyFetcher Number = new PropertyFetcher(nameof(Number));
        }
        #endregion

        #region "Microsoft.Data fetchers"

        /// <summary> Fetchers for execute command before event. </summary>
        internal static class CommandBeforeMicrosoft
        {
            public static readonly PropertyFetcher OperationId = new PropertyFetcher(nameof(OperationId));
            public static readonly PropertyFetcher Command = new PropertyFetcher(nameof(Command));
            public static readonly PropertyFetcher Connection = new PropertyFetcher(nameof(Connection));
            public static readonly PropertyFetcher DataSource = new PropertyFetcher(nameof(DataSource));
            public static readonly PropertyFetcher Database = new PropertyFetcher(nameof(Database));
            public static readonly PropertyFetcher CommandType = new PropertyFetcher(nameof(CommandType));
            public static readonly PropertyFetcher CommandText = new PropertyFetcher(nameof(CommandText));
            public static readonly PropertyFetcher Timestamp = new PropertyFetcher(nameof(Timestamp));
        }

        /// <summary> Fetchers for execute command after event. </summary>
        internal static class CommandAfterMicrosoft
        {
            public static readonly PropertyFetcher OperationId = new PropertyFetcher(nameof(OperationId));
            public static readonly PropertyFetcher Command = new PropertyFetcher(nameof(Command));
            public static readonly PropertyFetcher Timestamp = new PropertyFetcher(nameof(Timestamp));
        }

        /// <summary> Fetchers for execute command error event. </summary>
        internal static class CommandErrorMicrosoft
        {
            public static readonly PropertyFetcher OperationId = new PropertyFetcher(nameof(OperationId));
            public static readonly PropertyFetcher Command = new PropertyFetcher(nameof(Command));
            public static readonly PropertyFetcher Exception = new PropertyFetcher(nameof(Exception));
            public static readonly PropertyFetcher Timestamp = new PropertyFetcher(nameof(Timestamp));
            public static readonly PropertyFetcher Number = new PropertyFetcher(nameof(Number));
        }

        /// <summary> Fetchers for connection open/close before events. </summary>
        internal static class ConnectionBeforeMicrosoft
        {
            public static readonly PropertyFetcher OperationId = new PropertyFetcher(nameof(OperationId));
            public static readonly PropertyFetcher Operation = new PropertyFetcher(nameof(Operation));
            public static readonly PropertyFetcher Connection = new PropertyFetcher(nameof(Connection));
            public static readonly PropertyFetcher Timestamp = new PropertyFetcher(nameof(Timestamp));
            public static readonly PropertyFetcher DataSource = new PropertyFetcher(nameof(DataSource));
            public static readonly PropertyFetcher Database = new PropertyFetcher(nameof(Database));
        }

        /// <summary> Fetchers for connection open/close after events. </summary>
        internal static class ConnectionAfterMicrosoft
        {
            public static readonly PropertyFetcher OperationId = new PropertyFetcher(nameof(OperationId));
            public static readonly PropertyFetcher Connection = new PropertyFetcher(nameof(Connection));
        }

        /// <summary> Fetchers for connection open/close error events. </summary>
        internal static class ConnectionErrorMicrosoft
        {
            public static readonly PropertyFetcher OperationId = new PropertyFetcher(nameof(OperationId));
            public static readonly PropertyFetcher Connection = new PropertyFetcher(nameof(Connection));
            public static readonly PropertyFetcher Exception = new PropertyFetcher(nameof(Exception));
            public static readonly PropertyFetcher Timestamp = new PropertyFetcher(nameof(Timestamp));
            public static readonly PropertyFetcher Number = new PropertyFetcher(nameof(Number));
        }

        /// <summary> Fetchers for transaction commit before events. </summary>
        internal static class TransactionCommitBeforeMicrosoft
        {
            public static readonly PropertyFetcher OperationId = new PropertyFetcher(nameof(OperationId));
            public static readonly PropertyFetcher Operation = new PropertyFetcher(nameof(Operation));
            public static readonly PropertyFetcher IsolationLevel = new PropertyFetcher(nameof(IsolationLevel));
            public static readonly PropertyFetcher Connection = new PropertyFetcher(nameof(Connection));
            public static readonly PropertyFetcher DataSource = new PropertyFetcher(nameof(DataSource));
            public static readonly PropertyFetcher Database = new PropertyFetcher(nameof(Database));
            public static readonly PropertyFetcher Timestamp = new PropertyFetcher(nameof(Timestamp));
        }

        /// <summary> Fetchers for transaction rollback before events. </summary>
        internal static class TransactionRollbackBeforeMicrosoft
        {
            public static readonly PropertyFetcher OperationId = new PropertyFetcher(nameof(OperationId));
            public static readonly PropertyFetcher Operation = new PropertyFetcher(nameof(Operation));
            public static readonly PropertyFetcher IsolationLevel = new PropertyFetcher(nameof(IsolationLevel));
            public static readonly PropertyFetcher Connection = new PropertyFetcher(nameof(Connection));
            public static readonly PropertyFetcher DataSource = new PropertyFetcher(nameof(DataSource));
            public static readonly PropertyFetcher Database = new PropertyFetcher(nameof(Database));
            public static readonly PropertyFetcher TransactionName = new PropertyFetcher(nameof(TransactionName));
            public static readonly PropertyFetcher Timestamp = new PropertyFetcher(nameof(Timestamp));
        }

        /// <summary> Fetchers for transaction rollback after events. </summary>
        internal static class TransactionRollbackAfterMicrosoft
        {
            public static readonly PropertyFetcher OperationId = new PropertyFetcher(nameof(OperationId));
            public static readonly PropertyFetcher Connection = new PropertyFetcher(nameof(Connection));
            public static readonly PropertyFetcher Timestamp = new PropertyFetcher(nameof(Timestamp));
        }

        /// <summary> Fetchers for transaction commit after events. </summary>
        internal static class TransactionCommitAfterMicrosoft
        {
            public static readonly PropertyFetcher OperationId = new PropertyFetcher(nameof(OperationId));
            public static readonly PropertyFetcher Connection = new PropertyFetcher(nameof(Connection));
            public static readonly PropertyFetcher Timestamp = new PropertyFetcher(nameof(Timestamp));
        }

        /// <summary> Fetchers for transaction rollback error events. </summary>
        internal static class TransactionRollbackErrorMicrosoft
        {
            public static readonly PropertyFetcher OperationId = new PropertyFetcher(nameof(OperationId));
            public static readonly PropertyFetcher Connection = new PropertyFetcher(nameof(Connection));
            public static readonly PropertyFetcher Exception = new PropertyFetcher(nameof(Exception));
            public static readonly PropertyFetcher Timestamp = new PropertyFetcher(nameof(Timestamp));
            public static readonly PropertyFetcher Number = new PropertyFetcher(nameof(Number));
        }

        /// <summary> Fetchers for transaction commit error events. </summary>
        internal static class TransactionCommitErrorMicrosoft
        {
            public static readonly PropertyFetcher OperationId = new PropertyFetcher(nameof(OperationId));
            public static readonly PropertyFetcher Connection = new PropertyFetcher(nameof(Connection));
            public static readonly PropertyFetcher Exception = new PropertyFetcher(nameof(Exception));
            public static readonly PropertyFetcher Timestamp = new PropertyFetcher(nameof(Timestamp));
            public static readonly PropertyFetcher Number = new PropertyFetcher(nameof(Number));
        }
        #endregion
    }
}

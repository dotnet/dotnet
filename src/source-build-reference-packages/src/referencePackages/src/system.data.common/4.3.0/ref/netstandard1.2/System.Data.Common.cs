// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;

[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: AllowPartiallyTrustedCallers]
[assembly: ReferenceAssembly]
[assembly: AssemblyTitle("System.Data.Common")]
[assembly: AssemblyDescription("System.Data.Common")]
[assembly: AssemblyDefaultAlias("System.Data.Common")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("1.0.24212.01")]
[assembly: AssemblyInformationalVersion("1.0.24212.01 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("4.1.0.0")]




namespace System
{
    public sealed partial class DBNull
    {
        internal DBNull() { }
        public static readonly System.DBNull Value;
        public override string ToString() { throw null; }
        public string ToString(System.IFormatProvider provider) { throw null; }
    }
}
namespace System.Data
{
    [System.FlagsAttribute]
    public enum CommandBehavior
    {
        Default = 0,
        SingleResult = 1,
        SchemaOnly = 2,
        KeyInfo = 4,
        SingleRow = 8,
        SequentialAccess = 16,
        CloseConnection = 32,
    }
    public enum CommandType
    {
        Text = 1,
        StoredProcedure = 4,
        TableDirect = 512,
    }
    [System.FlagsAttribute]
    public enum ConnectionState
    {
        Closed = 0,
        Open = 1,
        Connecting = 2,
        Executing = 4,
        Fetching = 8,
        Broken = 16,
    }
    public enum DataRowVersion
    {
        Default = 1536,
    }
    public partial class DataTable
    {
        internal DataTable() { }
    }
    public enum DbType
    {
        AnsiString = 0,
        Binary = 1,
        Byte = 2,
        Boolean = 3,
        Currency = 4,
        Date = 5,
        DateTime = 6,
        Decimal = 7,
        Double = 8,
        Guid = 9,
        Int16 = 10,
        Int32 = 11,
        Int64 = 12,
        Object = 13,
        SByte = 14,
        Single = 15,
        String = 16,
        Time = 17,
        UInt16 = 18,
        UInt32 = 19,
        UInt64 = 20,
        VarNumeric = 21,
        AnsiStringFixedLength = 22,
        StringFixedLength = 23,
        Xml = 25,
        DateTime2 = 26,
        DateTimeOffset = 27,
    }
    public partial interface IDataParameter
    {
        System.Data.DbType DbType { get; set; }
        System.Data.ParameterDirection Direction { get; set; }
        bool IsNullable { get; }
        string ParameterName { get; set; }
        string SourceColumn { get; set; }
        System.Data.DataRowVersion SourceVersion { get; set; }
        object Value { get; set; }
    }
    public partial interface IDataParameterCollection : System.Collections.ICollection, System.Collections.IEnumerable, System.Collections.IList
    {
        object this[string parameterName] { get; set; }
        bool Contains(string parameterName);
        int IndexOf(string parameterName);
        void RemoveAt(string parameterName);
    }
    public partial interface IDataReader : System.Data.IDataRecord, System.IDisposable
    {
        int Depth { get; }
        bool IsClosed { get; }
        int RecordsAffected { get; }
        void Close();
        System.Data.DataTable GetSchemaTable();
        bool NextResult();
        bool Read();
    }
    public partial interface IDataRecord
    {
        int FieldCount { get; }
        object this[int i] { get; }
        object this[string name] { get; }
        bool GetBoolean(int i);
        byte GetByte(int i);
        long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length);
        char GetChar(int i);
        long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length);
        System.Data.IDataReader GetData(int i);
        string GetDataTypeName(int i);
        System.DateTime GetDateTime(int i);
        decimal GetDecimal(int i);
        double GetDouble(int i);
        System.Type GetFieldType(int i);
        float GetFloat(int i);
        System.Guid GetGuid(int i);
        short GetInt16(int i);
        int GetInt32(int i);
        long GetInt64(int i);
        string GetName(int i);
        int GetOrdinal(string name);
        string GetString(int i);
        object GetValue(int i);
        int GetValues(object[] values);
        bool IsDBNull(int i);
    }
    public partial interface IDbCommand : System.IDisposable
    {
        string CommandText { get; set; }
        int CommandTimeout { get; set; }
        System.Data.CommandType CommandType { get; set; }
        System.Data.IDbConnection Connection { get; set; }
        System.Data.IDataParameterCollection Parameters { get; }
        System.Data.IDbTransaction Transaction { get; set; }
        System.Data.UpdateRowSource UpdatedRowSource { get; set; }
        void Cancel();
        System.Data.IDbDataParameter CreateParameter();
        int ExecuteNonQuery();
        System.Data.IDataReader ExecuteReader();
        System.Data.IDataReader ExecuteReader(System.Data.CommandBehavior behavior);
        object ExecuteScalar();
        void Prepare();
    }
    public partial interface IDbConnection : System.IDisposable
    {
        string ConnectionString { get; set; }
        int ConnectionTimeout { get; }
        string Database { get; }
        System.Data.ConnectionState State { get; }
        System.Data.IDbTransaction BeginTransaction();
        System.Data.IDbTransaction BeginTransaction(System.Data.IsolationLevel il);
        void ChangeDatabase(string databaseName);
        void Close();
        System.Data.IDbCommand CreateCommand();
        void Open();
    }
    public partial interface IDbDataParameter : System.Data.IDataParameter
    {
        byte Precision { get; set; }
        byte Scale { get; set; }
        int Size { get; set; }
    }
    public partial interface IDbTransaction : System.IDisposable
    {
        System.Data.IDbConnection Connection { get; }
        System.Data.IsolationLevel IsolationLevel { get; }
        void Commit();
        void Rollback();
    }
    public enum IsolationLevel
    {
        Unspecified = -1,
        Chaos = 16,
        ReadUncommitted = 256,
        ReadCommitted = 4096,
        RepeatableRead = 65536,
        Serializable = 1048576,
        Snapshot = 16777216,
    }
    public enum ParameterDirection
    {
        Input = 1,
        Output = 2,
        InputOutput = 3,
        ReturnValue = 6,
    }
    public sealed partial class StateChangeEventArgs : System.EventArgs
    {
        public StateChangeEventArgs(System.Data.ConnectionState originalState, System.Data.ConnectionState currentState) { }
        public System.Data.ConnectionState CurrentState { get { throw null; } }
        public System.Data.ConnectionState OriginalState { get { throw null; } }
    }
    public delegate void StateChangeEventHandler(object sender, System.Data.StateChangeEventArgs e);
    public enum UpdateRowSource
    {
        None = 0,
        OutputParameters = 1,
        FirstReturnedRecord = 2,
        Both = 3,
    }
}
namespace System.Data.Common
{
    public abstract partial class DbColumn
    {
        protected DbColumn() { }
        public bool? AllowDBNull { get { throw null; } protected set { } }
        public string BaseCatalogName { get { throw null; } protected set { } }
        public string BaseColumnName { get { throw null; } protected set { } }
        public string BaseSchemaName { get { throw null; } protected set { } }
        public string BaseServerName { get { throw null; } protected set { } }
        public string BaseTableName { get { throw null; } protected set { } }
        public string ColumnName { get { throw null; } protected set { } }
        public int? ColumnOrdinal { get { throw null; } protected set { } }
        public int? ColumnSize { get { throw null; } protected set { } }
        public System.Type DataType { get { throw null; } protected set { } }
        public string DataTypeName { get { throw null; } protected set { } }
        public bool? IsAliased { get { throw null; } protected set { } }
        public bool? IsAutoIncrement { get { throw null; } protected set { } }
        public bool? IsExpression { get { throw null; } protected set { } }
        public bool? IsHidden { get { throw null; } protected set { } }
        public bool? IsIdentity { get { throw null; } protected set { } }
        public bool? IsKey { get { throw null; } protected set { } }
        public bool? IsLong { get { throw null; } protected set { } }
        public bool? IsReadOnly { get { throw null; } protected set { } }
        public bool? IsUnique { get { throw null; } protected set { } }
        public virtual object this[string property] { get { throw null; } }
        public int? NumericPrecision { get { throw null; } protected set { } }
        public int? NumericScale { get { throw null; } protected set { } }
        public string UdtAssemblyQualifiedName { get { throw null; } protected set { } }
    }
    public abstract partial class DbCommand : System.Data.IDbCommand, System.IDisposable
    {
        protected DbCommand() { }
        public abstract string CommandText { get; set; }
        public abstract int CommandTimeout { get; set; }
        public abstract System.Data.CommandType CommandType { get; set; }
        public System.Data.Common.DbConnection Connection { get { throw null; } set { } }
        protected abstract System.Data.Common.DbConnection DbConnection { get; set; }
        protected abstract System.Data.Common.DbParameterCollection DbParameterCollection { get; }
        protected abstract System.Data.Common.DbTransaction DbTransaction { get; set; }
        public abstract bool DesignTimeVisible { get; set; }
        public System.Data.Common.DbParameterCollection Parameters { get { throw null; } }
        System.Data.IDbConnection System.Data.IDbCommand.Connection { get { throw null; } set { } }
        System.Data.IDataParameterCollection System.Data.IDbCommand.Parameters { get { throw null; } }
        System.Data.IDbTransaction System.Data.IDbCommand.Transaction { get { throw null; } set { } }
        public System.Data.Common.DbTransaction Transaction { get { throw null; } set { } }
        public abstract System.Data.UpdateRowSource UpdatedRowSource { get; set; }
        public abstract void Cancel();
        protected abstract System.Data.Common.DbParameter CreateDbParameter();
        public System.Data.Common.DbParameter CreateParameter() { throw null; }
        public void Dispose() { }
        protected virtual void Dispose(bool disposing) { }
        protected abstract System.Data.Common.DbDataReader ExecuteDbDataReader(System.Data.CommandBehavior behavior);
        protected virtual System.Threading.Tasks.Task<System.Data.Common.DbDataReader> ExecuteDbDataReaderAsync(System.Data.CommandBehavior behavior, System.Threading.CancellationToken cancellationToken) { throw null; }
        public abstract int ExecuteNonQuery();
        public System.Threading.Tasks.Task<int> ExecuteNonQueryAsync() { throw null; }
        public virtual System.Threading.Tasks.Task<int> ExecuteNonQueryAsync(System.Threading.CancellationToken cancellationToken) { throw null; }
        public System.Data.Common.DbDataReader ExecuteReader() { throw null; }
        public System.Data.Common.DbDataReader ExecuteReader(System.Data.CommandBehavior behavior) { throw null; }
        public System.Threading.Tasks.Task<System.Data.Common.DbDataReader> ExecuteReaderAsync() { throw null; }
        public System.Threading.Tasks.Task<System.Data.Common.DbDataReader> ExecuteReaderAsync(System.Data.CommandBehavior behavior) { throw null; }
        public System.Threading.Tasks.Task<System.Data.Common.DbDataReader> ExecuteReaderAsync(System.Data.CommandBehavior behavior, System.Threading.CancellationToken cancellationToken) { throw null; }
        public System.Threading.Tasks.Task<System.Data.Common.DbDataReader> ExecuteReaderAsync(System.Threading.CancellationToken cancellationToken) { throw null; }
        public abstract object ExecuteScalar();
        public System.Threading.Tasks.Task<object> ExecuteScalarAsync() { throw null; }
        public virtual System.Threading.Tasks.Task<object> ExecuteScalarAsync(System.Threading.CancellationToken cancellationToken) { throw null; }
        public abstract void Prepare();
        System.Data.IDbDataParameter System.Data.IDbCommand.CreateParameter() { throw null; }
        System.Data.IDataReader System.Data.IDbCommand.ExecuteReader() { throw null; }
        System.Data.IDataReader System.Data.IDbCommand.ExecuteReader(System.Data.CommandBehavior behavior) { throw null; }
    }
    public abstract partial class DbConnection : System.Data.IDbConnection, System.IDisposable
    {
        protected DbConnection() { }
        public abstract string ConnectionString { get; set; }
        public virtual int ConnectionTimeout { get { throw null; } }
        public abstract string Database { get; }
        public abstract string DataSource { get; }
        public abstract string ServerVersion { get; }
        public abstract System.Data.ConnectionState State { get; }
        public virtual event System.Data.StateChangeEventHandler StateChange { add { } remove { } }
        protected abstract System.Data.Common.DbTransaction BeginDbTransaction(System.Data.IsolationLevel isolationLevel);
        public System.Data.Common.DbTransaction BeginTransaction() { throw null; }
        public System.Data.Common.DbTransaction BeginTransaction(System.Data.IsolationLevel isolationLevel) { throw null; }
        public abstract void ChangeDatabase(string databaseName);
        public abstract void Close();
        public System.Data.Common.DbCommand CreateCommand() { throw null; }
        protected abstract System.Data.Common.DbCommand CreateDbCommand();
        public void Dispose() { }
        protected virtual void Dispose(bool disposing) { }
        protected virtual void OnStateChange(System.Data.StateChangeEventArgs stateChange) { }
        public abstract void Open();
        public System.Threading.Tasks.Task OpenAsync() { throw null; }
        public virtual System.Threading.Tasks.Task OpenAsync(System.Threading.CancellationToken cancellationToken) { throw null; }
        System.Data.IDbTransaction System.Data.IDbConnection.BeginTransaction() { throw null; }
        System.Data.IDbTransaction System.Data.IDbConnection.BeginTransaction(System.Data.IsolationLevel isolationLevel) { throw null; }
        System.Data.IDbCommand System.Data.IDbConnection.CreateCommand() { throw null; }
    }
    public partial class DbConnectionStringBuilder : System.Collections.ICollection, System.Collections.IDictionary, System.Collections.IEnumerable
    {
        public DbConnectionStringBuilder() { }
        public string ConnectionString { get { throw null; } set { } }
        public virtual int Count { get { throw null; } }
        public virtual object this[string keyword] { get { throw null; } set { } }
        public virtual System.Collections.ICollection Keys { get { throw null; } }
        bool System.Collections.ICollection.IsSynchronized { get { throw null; } }
        object System.Collections.ICollection.SyncRoot { get { throw null; } }
        bool System.Collections.IDictionary.IsFixedSize { get { throw null; } }
        bool System.Collections.IDictionary.IsReadOnly { get { throw null; } }
        object System.Collections.IDictionary.this[object keyword] { get { throw null; } set { } }
        public virtual System.Collections.ICollection Values { get { throw null; } }
        public void Add(string keyword, object value) { }
        public static void AppendKeyValuePair(System.Text.StringBuilder builder, string keyword, string value) { }
        public virtual void Clear() { }
        public virtual bool ContainsKey(string keyword) { throw null; }
        public virtual bool EquivalentTo(System.Data.Common.DbConnectionStringBuilder connectionStringBuilder) { throw null; }
        public virtual bool Remove(string keyword) { throw null; }
        public virtual bool ShouldSerialize(string keyword) { throw null; }
        void System.Collections.ICollection.CopyTo(System.Array array, int index) { }
        void System.Collections.IDictionary.Add(object keyword, object value) { }
        bool System.Collections.IDictionary.Contains(object keyword) { throw null; }
        System.Collections.IDictionaryEnumerator System.Collections.IDictionary.GetEnumerator() { throw null; }
        void System.Collections.IDictionary.Remove(object keyword) { }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
        public override string ToString() { throw null; }
        public virtual bool TryGetValue(string keyword, out object value) { throw null; }
    }
    public abstract partial class DbDataReader : System.Collections.IEnumerable, System.Data.IDataReader, System.Data.IDataRecord, System.IDisposable
    {
        protected DbDataReader() { }
        public abstract int Depth { get; }
        public abstract int FieldCount { get; }
        public abstract bool HasRows { get; }
        public abstract bool IsClosed { get; }
        public abstract object this[int ordinal] { get; }
        public abstract object this[string name] { get; }
        public abstract int RecordsAffected { get; }
        public virtual int VisibleFieldCount { get { throw null; } }
        public void Dispose() { }
        protected virtual void Dispose(bool disposing) { }
        public abstract bool GetBoolean(int ordinal);
        public abstract byte GetByte(int ordinal);
        public abstract long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length);
        public abstract char GetChar(int ordinal);
        public abstract long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length);
        public System.Data.Common.DbDataReader GetData(int ordinal) { throw null; }
        public abstract string GetDataTypeName(int ordinal);
        public abstract System.DateTime GetDateTime(int ordinal);
        protected virtual System.Data.Common.DbDataReader GetDbDataReader(int ordinal) { throw null; }
        public abstract decimal GetDecimal(int ordinal);
        public abstract double GetDouble(int ordinal);
        public abstract System.Collections.IEnumerator GetEnumerator();
        public abstract System.Type GetFieldType(int ordinal);
        public System.Threading.Tasks.Task<T> GetFieldValueAsync<T>(int ordinal) { throw null; }
        public virtual System.Threading.Tasks.Task<T> GetFieldValueAsync<T>(int ordinal, System.Threading.CancellationToken cancellationToken) { throw null; }
        public virtual T GetFieldValue<T>(int ordinal) { throw null; }
        public abstract float GetFloat(int ordinal);
        public abstract System.Guid GetGuid(int ordinal);
        public abstract short GetInt16(int ordinal);
        public abstract int GetInt32(int ordinal);
        public abstract long GetInt64(int ordinal);
        public abstract string GetName(int ordinal);
        public abstract int GetOrdinal(string name);
        public virtual System.Type GetProviderSpecificFieldType(int ordinal) { throw null; }
        public virtual object GetProviderSpecificValue(int ordinal) { throw null; }
        public virtual int GetProviderSpecificValues(object[] values) { throw null; }
        public virtual System.IO.Stream GetStream(int ordinal) { throw null; }
        public abstract string GetString(int ordinal);
        public virtual System.IO.TextReader GetTextReader(int ordinal) { throw null; }
        public abstract object GetValue(int ordinal);
        public abstract int GetValues(object[] values);
        public abstract bool IsDBNull(int ordinal);
        public System.Threading.Tasks.Task<bool> IsDBNullAsync(int ordinal) { throw null; }
        public virtual System.Threading.Tasks.Task<bool> IsDBNullAsync(int ordinal, System.Threading.CancellationToken cancellationToken) { throw null; }
        public abstract bool NextResult();
        public System.Threading.Tasks.Task<bool> NextResultAsync() { throw null; }
        public virtual System.Threading.Tasks.Task<bool> NextResultAsync(System.Threading.CancellationToken cancellationToken) { throw null; }
        public abstract bool Read();
        public System.Threading.Tasks.Task<bool> ReadAsync() { throw null; }
        public virtual System.Threading.Tasks.Task<bool> ReadAsync(System.Threading.CancellationToken cancellationToken) { throw null; }
        void System.Data.IDataReader.Close() { }
        System.Data.DataTable System.Data.IDataReader.GetSchemaTable() { throw null; }
        System.Data.IDataReader System.Data.IDataRecord.GetData(int ordinal) { throw null; }
    }
    public static partial class DbDataReaderExtensions
    {
        public static bool CanGetColumnSchema(this System.Data.Common.DbDataReader reader) { throw null; }
        public static System.Collections.ObjectModel.ReadOnlyCollection<System.Data.Common.DbColumn> GetColumnSchema(this System.Data.Common.DbDataReader reader) { throw null; }
    }
    public abstract partial class DbDataRecord : System.Data.IDataRecord
    {
        protected DbDataRecord() { }
        public abstract int FieldCount { get; }
        public abstract object this[int i] { get; }
        public abstract object this[string name] { get; }
        public abstract bool GetBoolean(int i);
        public abstract byte GetByte(int i);
        public abstract long GetBytes(int i, long dataIndex, byte[] buffer, int bufferIndex, int length);
        public abstract char GetChar(int i);
        public abstract long GetChars(int i, long dataIndex, char[] buffer, int bufferIndex, int length);
        public System.Data.IDataReader GetData(int i) { throw null; }
        public abstract string GetDataTypeName(int i);
        public abstract System.DateTime GetDateTime(int i);
        protected virtual System.Data.Common.DbDataReader GetDbDataReader(int i) { throw null; }
        public abstract decimal GetDecimal(int i);
        public abstract double GetDouble(int i);
        public abstract System.Type GetFieldType(int i);
        public abstract float GetFloat(int i);
        public abstract System.Guid GetGuid(int i);
        public abstract short GetInt16(int i);
        public abstract int GetInt32(int i);
        public abstract long GetInt64(int i);
        public abstract string GetName(int i);
        public abstract int GetOrdinal(string name);
        public abstract string GetString(int i);
        public abstract object GetValue(int i);
        public abstract int GetValues(object[] values);
        public abstract bool IsDBNull(int i);
    }
    public partial class DbEnumerator : System.Collections.IEnumerator
    {
        public DbEnumerator(System.Data.IDataReader reader, bool closeReader) { }
        public object Current { get { throw null; } }
        public bool MoveNext() { throw null; }
        public void Reset() { }
    }
    public abstract partial class DbException : System.Exception
    {
        protected DbException() { }
        protected DbException(string message) { }
        protected DbException(string message, System.Exception innerException) { }
    }
    public abstract partial class DbParameter : System.Data.IDataParameter, System.Data.IDbDataParameter
    {
        protected DbParameter() { }
        public abstract System.Data.DbType DbType { get; set; }
        public abstract System.Data.ParameterDirection Direction { get; set; }
        public abstract bool IsNullable { get; set; }
        public abstract string ParameterName { get; set; }
        public virtual byte Precision { get { throw null; } set { } }
        public virtual byte Scale { get { throw null; } set { } }
        public abstract int Size { get; set; }
        public abstract string SourceColumn { get; set; }
        public abstract bool SourceColumnNullMapping { get; set; }
        System.Data.DataRowVersion System.Data.IDataParameter.SourceVersion { get { throw null; } set { } }
        byte System.Data.IDbDataParameter.Precision { get { throw null; } set { } }
        byte System.Data.IDbDataParameter.Scale { get { throw null; } set { } }
        public abstract object Value { get; set; }
        public abstract void ResetDbType();
    }
    public abstract partial class DbParameterCollection : System.Collections.ICollection, System.Collections.IEnumerable, System.Collections.IList, System.Data.IDataParameterCollection
    {
        protected DbParameterCollection() { }
        public abstract int Count { get; }
        public System.Data.Common.DbParameter this[int index] { get { throw null; } set { } }
        public System.Data.Common.DbParameter this[string parameterName] { get { throw null; } set { } }
        public abstract object SyncRoot { get; }
        bool System.Collections.ICollection.IsSynchronized { get { throw null; } }
        bool System.Collections.IList.IsFixedSize { get { throw null; } }
        bool System.Collections.IList.IsReadOnly { get { throw null; } }
        object System.Collections.IList.this[int index] { get { throw null; } set { } }
        object System.Data.IDataParameterCollection.this[string parameterName] { get { throw null; } set { } }
        public abstract int Add(object value);
        public abstract void AddRange(System.Array values);
        public abstract void Clear();
        public abstract bool Contains(object value);
        public abstract bool Contains(string value);
        public abstract void CopyTo(System.Array array, int index);
        public abstract System.Collections.IEnumerator GetEnumerator();
        protected abstract System.Data.Common.DbParameter GetParameter(int index);
        protected abstract System.Data.Common.DbParameter GetParameter(string parameterName);
        public abstract int IndexOf(object value);
        public abstract int IndexOf(string parameterName);
        public abstract void Insert(int index, object value);
        public abstract void Remove(object value);
        public abstract void RemoveAt(int index);
        public abstract void RemoveAt(string parameterName);
        protected abstract void SetParameter(int index, System.Data.Common.DbParameter value);
        protected abstract void SetParameter(string parameterName, System.Data.Common.DbParameter value);
    }
    public abstract partial class DbProviderFactory
    {
        protected DbProviderFactory() { }
        public virtual System.Data.Common.DbCommand CreateCommand() { throw null; }
        public virtual System.Data.Common.DbConnection CreateConnection() { throw null; }
        public virtual System.Data.Common.DbConnectionStringBuilder CreateConnectionStringBuilder() { throw null; }
        public virtual System.Data.Common.DbParameter CreateParameter() { throw null; }
    }
    public abstract partial class DbTransaction : System.Data.IDbTransaction, System.IDisposable
    {
        protected DbTransaction() { }
        public System.Data.Common.DbConnection Connection { get { throw null; } }
        protected abstract System.Data.Common.DbConnection DbConnection { get; }
        public abstract System.Data.IsolationLevel IsolationLevel { get; }
        System.Data.IDbConnection System.Data.IDbTransaction.Connection { get { throw null; } }
        public abstract void Commit();
        public void Dispose() { }
        protected virtual void Dispose(bool disposing) { }
        public abstract void Rollback();
    }
    public partial interface IDbColumnSchemaGenerator
    {
        System.Collections.ObjectModel.ReadOnlyCollection<System.Data.Common.DbColumn> GetColumnSchema();
    }
}

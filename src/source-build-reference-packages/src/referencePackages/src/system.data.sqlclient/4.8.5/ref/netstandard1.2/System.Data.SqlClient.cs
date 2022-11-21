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
[assembly: AssemblyTitle("System.Data.SqlClient")]
[assembly: AssemblyDescription("System.Data.SqlClient")]
[assembly: AssemblyDefaultAlias("System.Data.SqlClient")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("1.0.24212.01")]
[assembly: AssemblyInformationalVersion("1.0.24212.01 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("4.0.0.0")]




namespace Microsoft.SqlServer.Server
{
    public partial class SqlDataRecord
    {
        public SqlDataRecord(params Microsoft.SqlServer.Server.SqlMetaData[] metaData) { }
        public virtual int FieldCount { get { throw null; } }
        public virtual object this[int ordinal] { get { throw null; } }
        public virtual object this[string name] { get { throw null; } }
        public virtual bool GetBoolean(int ordinal) { throw null; }
        public virtual byte GetByte(int ordinal) { throw null; }
        public virtual long GetBytes(int ordinal, long fieldOffset, byte[] buffer, int bufferOffset, int length) { throw null; }
        public virtual char GetChar(int ordinal) { throw null; }
        public virtual long GetChars(int ordinal, long fieldOffset, char[] buffer, int bufferOffset, int length) { throw null; }
        public virtual string GetDataTypeName(int ordinal) { throw null; }
        public virtual System.DateTime GetDateTime(int ordinal) { throw null; }
        public virtual System.DateTimeOffset GetDateTimeOffset(int ordinal) { throw null; }
        public virtual System.Decimal GetDecimal(int ordinal) { throw null; }
        public virtual double GetDouble(int ordinal) { throw null; }
        public virtual System.Type GetFieldType(int ordinal) { throw null; }
        public virtual float GetFloat(int ordinal) { throw null; }
        public virtual System.Guid GetGuid(int ordinal) { throw null; }
        public virtual short GetInt16(int ordinal) { throw null; }
        public virtual int GetInt32(int ordinal) { throw null; }
        public virtual long GetInt64(int ordinal) { throw null; }
        public virtual string GetName(int ordinal) { throw null; }
        public virtual int GetOrdinal(string name) { throw null; }
        public virtual System.Data.SqlTypes.SqlBinary GetSqlBinary(int ordinal) { throw null; }
        public virtual System.Data.SqlTypes.SqlBoolean GetSqlBoolean(int ordinal) { throw null; }
        public virtual System.Data.SqlTypes.SqlByte GetSqlByte(int ordinal) { throw null; }
        public virtual System.Data.SqlTypes.SqlBytes GetSqlBytes(int ordinal) { throw null; }
        public virtual System.Data.SqlTypes.SqlChars GetSqlChars(int ordinal) { throw null; }
        public virtual System.Data.SqlTypes.SqlDateTime GetSqlDateTime(int ordinal) { throw null; }
        public virtual System.Data.SqlTypes.SqlDecimal GetSqlDecimal(int ordinal) { throw null; }
        public virtual System.Data.SqlTypes.SqlDouble GetSqlDouble(int ordinal) { throw null; }
        public virtual System.Type GetSqlFieldType(int ordinal) { throw null; }
        public virtual System.Data.SqlTypes.SqlGuid GetSqlGuid(int ordinal) { throw null; }
        public virtual System.Data.SqlTypes.SqlInt16 GetSqlInt16(int ordinal) { throw null; }
        public virtual System.Data.SqlTypes.SqlInt32 GetSqlInt32(int ordinal) { throw null; }
        public virtual System.Data.SqlTypes.SqlInt64 GetSqlInt64(int ordinal) { throw null; }
        public virtual Microsoft.SqlServer.Server.SqlMetaData GetSqlMetaData(int ordinal) { throw null; }
        public virtual System.Data.SqlTypes.SqlMoney GetSqlMoney(int ordinal) { throw null; }
        public virtual System.Data.SqlTypes.SqlSingle GetSqlSingle(int ordinal) { throw null; }
        public virtual System.Data.SqlTypes.SqlString GetSqlString(int ordinal) { throw null; }
        public virtual object GetSqlValue(int ordinal) { throw null; }
        public virtual int GetSqlValues(object[] values) { throw null; }
        public virtual System.Data.SqlTypes.SqlXml GetSqlXml(int ordinal) { throw null; }
        public virtual string GetString(int ordinal) { throw null; }
        public virtual System.TimeSpan GetTimeSpan(int ordinal) { throw null; }
        public virtual object GetValue(int ordinal) { throw null; }
        public virtual int GetValues(object[] values) { throw null; }
        public virtual bool IsDBNull(int ordinal) { throw null; }
        public virtual void SetBoolean(int ordinal, bool value) { }
        public virtual void SetByte(int ordinal, byte value) { }
        public virtual void SetBytes(int ordinal, long fieldOffset, byte[] buffer, int bufferOffset, int length) { }
        public virtual void SetChar(int ordinal, char value) { }
        public virtual void SetChars(int ordinal, long fieldOffset, char[] buffer, int bufferOffset, int length) { }
        public virtual void SetDateTime(int ordinal, System.DateTime value) { }
        public virtual void SetDateTimeOffset(int ordinal, System.DateTimeOffset value) { }
        public virtual void SetDBNull(int ordinal) { }
        public virtual void SetDecimal(int ordinal, System.Decimal value) { }
        public virtual void SetDouble(int ordinal, double value) { }
        public virtual void SetFloat(int ordinal, float value) { }
        public virtual void SetGuid(int ordinal, System.Guid value) { }
        public virtual void SetInt16(int ordinal, short value) { }
        public virtual void SetInt32(int ordinal, int value) { }
        public virtual void SetInt64(int ordinal, long value) { }
        public virtual void SetSqlBinary(int ordinal, System.Data.SqlTypes.SqlBinary value) { }
        public virtual void SetSqlBoolean(int ordinal, System.Data.SqlTypes.SqlBoolean value) { }
        public virtual void SetSqlByte(int ordinal, System.Data.SqlTypes.SqlByte value) { }
        public virtual void SetSqlBytes(int ordinal, System.Data.SqlTypes.SqlBytes value) { }
        public virtual void SetSqlChars(int ordinal, System.Data.SqlTypes.SqlChars value) { }
        public virtual void SetSqlDateTime(int ordinal, System.Data.SqlTypes.SqlDateTime value) { }
        public virtual void SetSqlDecimal(int ordinal, System.Data.SqlTypes.SqlDecimal value) { }
        public virtual void SetSqlDouble(int ordinal, System.Data.SqlTypes.SqlDouble value) { }
        public virtual void SetSqlGuid(int ordinal, System.Data.SqlTypes.SqlGuid value) { }
        public virtual void SetSqlInt16(int ordinal, System.Data.SqlTypes.SqlInt16 value) { }
        public virtual void SetSqlInt32(int ordinal, System.Data.SqlTypes.SqlInt32 value) { }
        public virtual void SetSqlInt64(int ordinal, System.Data.SqlTypes.SqlInt64 value) { }
        public virtual void SetSqlMoney(int ordinal, System.Data.SqlTypes.SqlMoney value) { }
        public virtual void SetSqlSingle(int ordinal, System.Data.SqlTypes.SqlSingle value) { }
        public virtual void SetSqlString(int ordinal, System.Data.SqlTypes.SqlString value) { }
        public virtual void SetSqlXml(int ordinal, System.Data.SqlTypes.SqlXml value) { }
        public virtual void SetString(int ordinal, string value) { }
        public virtual void SetTimeSpan(int ordinal, System.TimeSpan value) { }
        public virtual void SetValue(int ordinal, object value) { }
        public virtual int SetValues(params object[] values) { throw null; }
    }
    public sealed partial class SqlMetaData
    {
        public SqlMetaData(string name, System.Data.SqlDbType dbType) { }
        public SqlMetaData(string name, System.Data.SqlDbType dbType, bool useServerDefault, bool isUniqueKey, System.Data.SqlClient.SortOrder columnSortOrder, int sortOrdinal) { }
        public SqlMetaData(string name, System.Data.SqlDbType dbType, byte precision, byte scale) { }
        public SqlMetaData(string name, System.Data.SqlDbType dbType, byte precision, byte scale, bool useServerDefault, bool isUniqueKey, System.Data.SqlClient.SortOrder columnSortOrder, int sortOrdinal) { }
        public SqlMetaData(string name, System.Data.SqlDbType dbType, long maxLength) { }
        public SqlMetaData(string name, System.Data.SqlDbType dbType, long maxLength, bool useServerDefault, bool isUniqueKey, System.Data.SqlClient.SortOrder columnSortOrder, int sortOrdinal) { }
        public SqlMetaData(string name, System.Data.SqlDbType dbType, long maxLength, byte precision, byte scale, long locale, System.Data.SqlTypes.SqlCompareOptions compareOptions, System.Type userDefinedType) { }
        public SqlMetaData(string name, System.Data.SqlDbType dbType, long maxLength, byte precision, byte scale, long localeId, System.Data.SqlTypes.SqlCompareOptions compareOptions, System.Type userDefinedType, bool useServerDefault, bool isUniqueKey, System.Data.SqlClient.SortOrder columnSortOrder, int sortOrdinal) { }
        public SqlMetaData(string name, System.Data.SqlDbType dbType, long maxLength, long locale, System.Data.SqlTypes.SqlCompareOptions compareOptions) { }
        public SqlMetaData(string name, System.Data.SqlDbType dbType, long maxLength, long locale, System.Data.SqlTypes.SqlCompareOptions compareOptions, bool useServerDefault, bool isUniqueKey, System.Data.SqlClient.SortOrder columnSortOrder, int sortOrdinal) { }
        public SqlMetaData(string name, System.Data.SqlDbType dbType, string database, string owningSchema, string objectName) { }
        public SqlMetaData(string name, System.Data.SqlDbType dbType, string database, string owningSchema, string objectName, bool useServerDefault, bool isUniqueKey, System.Data.SqlClient.SortOrder columnSortOrder, int sortOrdinal) { }
        public System.Data.SqlTypes.SqlCompareOptions CompareOptions { get { throw null; } }
        public bool IsUniqueKey { get { throw null; } }
        public long LocaleId { get { throw null; } }
        public static long Max { get { throw null; } }
        public long MaxLength { get { throw null; } }
        public string Name { get { throw null; } }
        public byte Precision { get { throw null; } }
        public byte Scale { get { throw null; } }
        public System.Data.SqlClient.SortOrder SortOrder { get { throw null; } }
        public int SortOrdinal { get { throw null; } }
        public System.Data.SqlDbType SqlDbType { get { throw null; } }
        public string TypeName { get { throw null; } }
        public bool UseServerDefault { get { throw null; } }
        public string XmlSchemaCollectionDatabase { get { throw null; } }
        public string XmlSchemaCollectionName { get { throw null; } }
        public string XmlSchemaCollectionOwningSchema { get { throw null; } }
        public bool Adjust(bool value) { throw null; }
        public byte Adjust(byte value) { throw null; }
        public byte[] Adjust(byte[] value) { throw null; }
        public char Adjust(char value) { throw null; }
        public char[] Adjust(char[] value) { throw null; }
        public System.Data.SqlTypes.SqlBinary Adjust(System.Data.SqlTypes.SqlBinary value) { throw null; }
        public System.Data.SqlTypes.SqlBoolean Adjust(System.Data.SqlTypes.SqlBoolean value) { throw null; }
        public System.Data.SqlTypes.SqlByte Adjust(System.Data.SqlTypes.SqlByte value) { throw null; }
        public System.Data.SqlTypes.SqlBytes Adjust(System.Data.SqlTypes.SqlBytes value) { throw null; }
        public System.Data.SqlTypes.SqlChars Adjust(System.Data.SqlTypes.SqlChars value) { throw null; }
        public System.Data.SqlTypes.SqlDateTime Adjust(System.Data.SqlTypes.SqlDateTime value) { throw null; }
        public System.Data.SqlTypes.SqlDecimal Adjust(System.Data.SqlTypes.SqlDecimal value) { throw null; }
        public System.Data.SqlTypes.SqlDouble Adjust(System.Data.SqlTypes.SqlDouble value) { throw null; }
        public System.Data.SqlTypes.SqlGuid Adjust(System.Data.SqlTypes.SqlGuid value) { throw null; }
        public System.Data.SqlTypes.SqlInt16 Adjust(System.Data.SqlTypes.SqlInt16 value) { throw null; }
        public System.Data.SqlTypes.SqlInt32 Adjust(System.Data.SqlTypes.SqlInt32 value) { throw null; }
        public System.Data.SqlTypes.SqlInt64 Adjust(System.Data.SqlTypes.SqlInt64 value) { throw null; }
        public System.Data.SqlTypes.SqlMoney Adjust(System.Data.SqlTypes.SqlMoney value) { throw null; }
        public System.Data.SqlTypes.SqlSingle Adjust(System.Data.SqlTypes.SqlSingle value) { throw null; }
        public System.Data.SqlTypes.SqlString Adjust(System.Data.SqlTypes.SqlString value) { throw null; }
        public System.Data.SqlTypes.SqlXml Adjust(System.Data.SqlTypes.SqlXml value) { throw null; }
        public System.DateTime Adjust(System.DateTime value) { throw null; }
        public System.DateTimeOffset Adjust(System.DateTimeOffset value) { throw null; }
        public System.Decimal Adjust(System.Decimal value) { throw null; }
        public double Adjust(double value) { throw null; }
        public System.Guid Adjust(System.Guid value) { throw null; }
        public short Adjust(short value) { throw null; }
        public int Adjust(int value) { throw null; }
        public long Adjust(long value) { throw null; }
        public object Adjust(object value) { throw null; }
        public float Adjust(float value) { throw null; }
        public string Adjust(string value) { throw null; }
        public System.TimeSpan Adjust(System.TimeSpan value) { throw null; }
        public static Microsoft.SqlServer.Server.SqlMetaData InferFromValue(object value, string name) { throw null; }
    }
}
namespace System.Data
{
    public enum SqlDbType
    {
        BigInt = 0,
        Binary = 1,
        Bit = 2,
        Char = 3,
        DateTime = 4,
        Decimal = 5,
        Float = 6,
        Image = 7,
        Int = 8,
        Money = 9,
        NChar = 10,
        NText = 11,
        NVarChar = 12,
        Real = 13,
        UniqueIdentifier = 14,
        SmallDateTime = 15,
        SmallInt = 16,
        SmallMoney = 17,
        Text = 18,
        Timestamp = 19,
        TinyInt = 20,
        VarBinary = 21,
        VarChar = 22,
        Variant = 23,
        Xml = 25,
        Udt = 29,
        Structured = 30,
        Date = 31,
        Time = 32,
        DateTime2 = 33,
        DateTimeOffset = 34,
    }
    public sealed partial class StatementCompletedEventArgs : System.EventArgs
    {
        public StatementCompletedEventArgs(int recordCount) { }
        public int RecordCount { get { throw null; } }
    }
    public delegate void StatementCompletedEventHandler(object sender, System.Data.StatementCompletedEventArgs e);
}
namespace System.Data.SqlClient
{
    public enum ApplicationIntent
    {
        ReadWrite = 0,
        ReadOnly = 1,
    }
    public enum SortOrder
    {
        Unspecified = -1,
        Ascending = 0,
        Descending = 1,
    }
    public sealed partial class SqlClientFactory : System.Data.Common.DbProviderFactory
    {
        internal SqlClientFactory() { }
        public static readonly System.Data.SqlClient.SqlClientFactory Instance;
        public override System.Data.Common.DbCommand CreateCommand() { throw null; }
        public override System.Data.Common.DbConnection CreateConnection() { throw null; }
        public override System.Data.Common.DbConnectionStringBuilder CreateConnectionStringBuilder() { throw null; }
        public override System.Data.Common.DbParameter CreateParameter() { throw null; }
    }
    public sealed partial class SqlCommand : System.Data.Common.DbCommand
    {
        public SqlCommand() { }
        public SqlCommand(string cmdText) { }
        public SqlCommand(string cmdText, System.Data.SqlClient.SqlConnection connection) { }
        public SqlCommand(string cmdText, System.Data.SqlClient.SqlConnection connection, System.Data.SqlClient.SqlTransaction transaction) { }
        public override string CommandText { get { throw null; } set { } }
        public override int CommandTimeout { get { throw null; } set { } }
        public override System.Data.CommandType CommandType { get { throw null; } set { } }
        public new System.Data.SqlClient.SqlConnection Connection { get { throw null; } set { } }
        protected override System.Data.Common.DbConnection DbConnection { get { throw null; } set { } }
        protected override System.Data.Common.DbParameterCollection DbParameterCollection { get { throw null; } }
        protected override System.Data.Common.DbTransaction DbTransaction { get { throw null; } set { } }
        public override bool DesignTimeVisible { get { throw null; } set { } }
        public new System.Data.SqlClient.SqlParameterCollection Parameters { get { throw null; } }
        public new System.Data.SqlClient.SqlTransaction Transaction { get { throw null; } set { } }
        public override System.Data.UpdateRowSource UpdatedRowSource { get { throw null; } set { } }
        public event System.Data.StatementCompletedEventHandler StatementCompleted { add { } remove { } }
        public override void Cancel() { }
        protected override System.Data.Common.DbParameter CreateDbParameter() { throw null; }
        public new System.Data.SqlClient.SqlParameter CreateParameter() { throw null; }
        protected override void Dispose(bool disposing) { }
        protected override System.Data.Common.DbDataReader ExecuteDbDataReader(System.Data.CommandBehavior behavior) { throw null; }
        protected override System.Threading.Tasks.Task<System.Data.Common.DbDataReader> ExecuteDbDataReaderAsync(System.Data.CommandBehavior behavior, System.Threading.CancellationToken cancellationToken) { throw null; }
        public override int ExecuteNonQuery() { throw null; }
        public override System.Threading.Tasks.Task<int> ExecuteNonQueryAsync(System.Threading.CancellationToken cancellationToken) { throw null; }
        public new System.Data.SqlClient.SqlDataReader ExecuteReader() { throw null; }
        public new System.Data.SqlClient.SqlDataReader ExecuteReader(System.Data.CommandBehavior behavior) { throw null; }
        public new System.Threading.Tasks.Task<System.Data.SqlClient.SqlDataReader> ExecuteReaderAsync() { throw null; }
        public new System.Threading.Tasks.Task<System.Data.SqlClient.SqlDataReader> ExecuteReaderAsync(System.Data.CommandBehavior behavior) { throw null; }
        public new System.Threading.Tasks.Task<System.Data.SqlClient.SqlDataReader> ExecuteReaderAsync(System.Data.CommandBehavior behavior, System.Threading.CancellationToken cancellationToken) { throw null; }
        public new System.Threading.Tasks.Task<System.Data.SqlClient.SqlDataReader> ExecuteReaderAsync(System.Threading.CancellationToken cancellationToken) { throw null; }
        public override object ExecuteScalar() { throw null; }
        public override System.Threading.Tasks.Task<object> ExecuteScalarAsync(System.Threading.CancellationToken cancellationToken) { throw null; }
        public System.Xml.XmlReader ExecuteXmlReader() { throw null; }
        public System.Threading.Tasks.Task<System.Xml.XmlReader> ExecuteXmlReaderAsync() { throw null; }
        public System.Threading.Tasks.Task<System.Xml.XmlReader> ExecuteXmlReaderAsync(System.Threading.CancellationToken cancellationToken) { throw null; }
        public override void Prepare() { }
    }
    public sealed partial class SqlConnection : System.Data.Common.DbConnection
    {
        public SqlConnection() { }
        public SqlConnection(string connectionString) { }
        public System.Guid ClientConnectionId { get { throw null; } }
        public override string ConnectionString { get { throw null; } set { } }
        public override int ConnectionTimeout { get { throw null; } }
        public override string Database { get { throw null; } }
        public override string DataSource { get { throw null; } }
        public bool FireInfoMessageEventOnUserErrors { get { throw null; } set { } }
        public int PacketSize { get { throw null; } }
        public override string ServerVersion { get { throw null; } }
        public override System.Data.ConnectionState State { get { throw null; } }
        public bool StatisticsEnabled { get { throw null; } set { } }
        public string WorkstationId { get { throw null; } }
        public event System.Data.SqlClient.SqlInfoMessageEventHandler InfoMessage { add { } remove { } }
        protected override System.Data.Common.DbTransaction BeginDbTransaction(System.Data.IsolationLevel isolationLevel) { throw null; }
        public new System.Data.SqlClient.SqlTransaction BeginTransaction() { throw null; }
        public new System.Data.SqlClient.SqlTransaction BeginTransaction(System.Data.IsolationLevel iso) { throw null; }
        public System.Data.SqlClient.SqlTransaction BeginTransaction(System.Data.IsolationLevel iso, string transactionName) { throw null; }
        public System.Data.SqlClient.SqlTransaction BeginTransaction(string transactionName) { throw null; }
        public override void ChangeDatabase(string database) { }
        public static void ClearAllPools() { }
        public static void ClearPool(System.Data.SqlClient.SqlConnection connection) { }
        public override void Close() { }
        public new System.Data.SqlClient.SqlCommand CreateCommand() { throw null; }
        protected override System.Data.Common.DbCommand CreateDbCommand() { throw null; }
        protected override void Dispose(bool disposing) { }
        public override void Open() { }
        public override System.Threading.Tasks.Task OpenAsync(System.Threading.CancellationToken cancellationToken) { throw null; }
        public void ResetStatistics() { }
        public System.Collections.IDictionary RetrieveStatistics() { throw null; }
    }
    public sealed partial class SqlConnectionStringBuilder : System.Data.Common.DbConnectionStringBuilder
    {
        public SqlConnectionStringBuilder() { }
        public SqlConnectionStringBuilder(string connectionString) { }
        public System.Data.SqlClient.ApplicationIntent ApplicationIntent { get { throw null; } set { } }
        public string ApplicationName { get { throw null; } set { } }
        public string AttachDBFilename { get { throw null; } set { } }
        public int ConnectRetryCount { get { throw null; } set { } }
        public int ConnectRetryInterval { get { throw null; } set { } }
        public int ConnectTimeout { get { throw null; } set { } }
        public string CurrentLanguage { get { throw null; } set { } }
        public string DataSource { get { throw null; } set { } }
        public bool Encrypt { get { throw null; } set { } }
        public string FailoverPartner { get { throw null; } set { } }
        public string InitialCatalog { get { throw null; } set { } }
        public bool IntegratedSecurity { get { throw null; } set { } }
        public override object this[string keyword] { get { throw null; } set { } }
        public override System.Collections.ICollection Keys { get { throw null; } }
        public int LoadBalanceTimeout { get { throw null; } set { } }
        public int MaxPoolSize { get { throw null; } set { } }
        public int MinPoolSize { get { throw null; } set { } }
        public bool MultipleActiveResultSets { get { throw null; } set { } }
        public bool MultiSubnetFailover { get { throw null; } set { } }
        public int PacketSize { get { throw null; } set { } }
        public string Password { get { throw null; } set { } }
        public bool PersistSecurityInfo { get { throw null; } set { } }
        public bool Pooling { get { throw null; } set { } }
        public bool Replication { get { throw null; } set { } }
        public bool TrustServerCertificate { get { throw null; } set { } }
        public string TypeSystemVersion { get { throw null; } set { } }
        public string UserID { get { throw null; } set { } }
        public bool UserInstance { get { throw null; } set { } }
        public override System.Collections.ICollection Values { get { throw null; } }
        public string WorkstationID { get { throw null; } set { } }
        public override void Clear() { }
        public override bool ContainsKey(string keyword) { throw null; }
        public override bool Remove(string keyword) { throw null; }
        public override bool ShouldSerialize(string keyword) { throw null; }
        public override bool TryGetValue(string keyword, out object value) { throw null; }
    }
    public partial class SqlDataReader : System.Data.Common.DbDataReader, System.IDisposable
    {
        internal SqlDataReader() { }
        protected System.Data.SqlClient.SqlConnection Connection { get { throw null; } }
        public override int Depth { get { throw null; } }
        public override int FieldCount { get { throw null; } }
        public override bool HasRows { get { throw null; } }
        public override bool IsClosed { get { throw null; } }
        public override object this[int i] { get { throw null; } }
        public override object this[string name] { get { throw null; } }
        public override int RecordsAffected { get { throw null; } }
        public override int VisibleFieldCount { get { throw null; } }
        public override bool GetBoolean(int i) { throw null; }
        public override byte GetByte(int i) { throw null; }
        public override long GetBytes(int i, long dataIndex, byte[] buffer, int bufferIndex, int length) { throw null; }
        public override char GetChar(int i) { throw null; }
        public override long GetChars(int i, long dataIndex, char[] buffer, int bufferIndex, int length) { throw null; }
        public override string GetDataTypeName(int i) { throw null; }
        public override System.DateTime GetDateTime(int i) { throw null; }
        public virtual System.DateTimeOffset GetDateTimeOffset(int i) { throw null; }
        public override System.Decimal GetDecimal(int i) { throw null; }
        public override double GetDouble(int i) { throw null; }
        public override System.Collections.IEnumerator GetEnumerator() { throw null; }
        public override System.Type GetFieldType(int i) { throw null; }
        public override System.Threading.Tasks.Task<T> GetFieldValueAsync<T>(int i, System.Threading.CancellationToken cancellationToken) { throw null; }
        public override T GetFieldValue<T>(int i) { throw null; }
        public override float GetFloat(int i) { throw null; }
        public override System.Guid GetGuid(int i) { throw null; }
        public override short GetInt16(int i) { throw null; }
        public override int GetInt32(int i) { throw null; }
        public override long GetInt64(int i) { throw null; }
        public override string GetName(int i) { throw null; }
        public override int GetOrdinal(string name) { throw null; }
        public override System.Type GetProviderSpecificFieldType(int i) { throw null; }
        public override object GetProviderSpecificValue(int i) { throw null; }
        public override int GetProviderSpecificValues(object[] values) { throw null; }
        public virtual System.Data.SqlTypes.SqlBinary GetSqlBinary(int i) { throw null; }
        public virtual System.Data.SqlTypes.SqlBoolean GetSqlBoolean(int i) { throw null; }
        public virtual System.Data.SqlTypes.SqlByte GetSqlByte(int i) { throw null; }
        public virtual System.Data.SqlTypes.SqlBytes GetSqlBytes(int i) { throw null; }
        public virtual System.Data.SqlTypes.SqlChars GetSqlChars(int i) { throw null; }
        public virtual System.Data.SqlTypes.SqlDateTime GetSqlDateTime(int i) { throw null; }
        public virtual System.Data.SqlTypes.SqlDecimal GetSqlDecimal(int i) { throw null; }
        public virtual System.Data.SqlTypes.SqlDouble GetSqlDouble(int i) { throw null; }
        public virtual System.Data.SqlTypes.SqlGuid GetSqlGuid(int i) { throw null; }
        public virtual System.Data.SqlTypes.SqlInt16 GetSqlInt16(int i) { throw null; }
        public virtual System.Data.SqlTypes.SqlInt32 GetSqlInt32(int i) { throw null; }
        public virtual System.Data.SqlTypes.SqlInt64 GetSqlInt64(int i) { throw null; }
        public virtual System.Data.SqlTypes.SqlMoney GetSqlMoney(int i) { throw null; }
        public virtual System.Data.SqlTypes.SqlSingle GetSqlSingle(int i) { throw null; }
        public virtual System.Data.SqlTypes.SqlString GetSqlString(int i) { throw null; }
        public virtual object GetSqlValue(int i) { throw null; }
        public virtual int GetSqlValues(object[] values) { throw null; }
        public virtual System.Data.SqlTypes.SqlXml GetSqlXml(int i) { throw null; }
        public override System.IO.Stream GetStream(int i) { throw null; }
        public override string GetString(int i) { throw null; }
        public override System.IO.TextReader GetTextReader(int i) { throw null; }
        public virtual System.TimeSpan GetTimeSpan(int i) { throw null; }
        public override object GetValue(int i) { throw null; }
        public override int GetValues(object[] values) { throw null; }
        public virtual System.Xml.XmlReader GetXmlReader(int i) { throw null; }
        public override bool IsDBNull(int i) { throw null; }
        public override System.Threading.Tasks.Task<bool> IsDBNullAsync(int i, System.Threading.CancellationToken cancellationToken) { throw null; }
        public override bool NextResult() { throw null; }
        public override System.Threading.Tasks.Task<bool> NextResultAsync(System.Threading.CancellationToken cancellationToken) { throw null; }
        public override bool Read() { throw null; }
        public override System.Threading.Tasks.Task<bool> ReadAsync(System.Threading.CancellationToken cancellationToken) { throw null; }
    }
    public sealed partial class SqlError
    {
        internal SqlError() { }
        public byte Class { get { throw null; } }
        public int LineNumber { get { throw null; } }
        public string Message { get { throw null; } }
        public int Number { get { throw null; } }
        public string Procedure { get { throw null; } }
        public string Server { get { throw null; } }
        public string Source { get { throw null; } }
        public byte State { get { throw null; } }
        public override string ToString() { throw null; }
    }
    public sealed partial class SqlErrorCollection : System.Collections.ICollection, System.Collections.IEnumerable
    {
        internal SqlErrorCollection() { }
        public int Count { get { throw null; } }
        public System.Data.SqlClient.SqlError this[int index] { get { throw null; } }
        bool System.Collections.ICollection.IsSynchronized { get { throw null; } }
        object System.Collections.ICollection.SyncRoot { get { throw null; } }
        public void CopyTo(System.Array array, int index) { }
        public void CopyTo(System.Data.SqlClient.SqlError[] array, int index) { }
        public System.Collections.IEnumerator GetEnumerator() { throw null; }
    }
    public sealed partial class SqlException : System.Data.Common.DbException
    {
        internal SqlException() { }
        public byte Class { get { throw null; } }
        public System.Guid ClientConnectionId { get { throw null; } }
        public System.Data.SqlClient.SqlErrorCollection Errors { get { throw null; } }
        public int LineNumber { get { throw null; } }
        public int Number { get { throw null; } }
        public string Procedure { get { throw null; } }
        public string Server { get { throw null; } }
        public override string Source { get { throw null; } }
        public byte State { get { throw null; } }
        public override string ToString() { throw null; }
    }
    public sealed partial class SqlInfoMessageEventArgs : System.EventArgs
    {
        internal SqlInfoMessageEventArgs() { }
        public System.Data.SqlClient.SqlErrorCollection Errors { get { throw null; } }
        public string Message { get { throw null; } }
        public string Source { get { throw null; } }
        public override string ToString() { throw null; }
    }
    public delegate void SqlInfoMessageEventHandler(object sender, System.Data.SqlClient.SqlInfoMessageEventArgs e);
    public sealed partial class SqlParameter : System.Data.Common.DbParameter
    {
        public SqlParameter() { }
        public SqlParameter(string parameterName, System.Data.SqlDbType dbType) { }
        public SqlParameter(string parameterName, System.Data.SqlDbType dbType, int size) { }
        public SqlParameter(string parameterName, System.Data.SqlDbType dbType, int size, string sourceColumn) { }
        public SqlParameter(string parameterName, object value) { }
        public System.Data.SqlTypes.SqlCompareOptions CompareInfo { get { throw null; } set { } }
        public override System.Data.DbType DbType { get { throw null; } set { } }
        public override System.Data.ParameterDirection Direction { get { throw null; } set { } }
        public override bool IsNullable { get { throw null; } set { } }
        public int LocaleId { get { throw null; } set { } }
        public int Offset { get { throw null; } set { } }
        public override string ParameterName { get { throw null; } set { } }
        public new byte Precision { get { throw null; } set { } }
        public new byte Scale { get { throw null; } set { } }
        public override int Size { get { throw null; } set { } }
        public override string SourceColumn { get { throw null; } set { } }
        public override bool SourceColumnNullMapping { get { throw null; } set { } }
        public System.Data.SqlDbType SqlDbType { get { throw null; } set { } }
        public object SqlValue { get { throw null; } set { } }
        public string TypeName { get { throw null; } set { } }
        public override object Value { get { throw null; } set { } }
        public string XmlSchemaCollectionDatabase { get { throw null; } set { } }
        public string XmlSchemaCollectionName { get { throw null; } set { } }
        public string XmlSchemaCollectionOwningSchema { get { throw null; } set { } }
        public override void ResetDbType() { }
        public void ResetSqlDbType() { }
        public override string ToString() { throw null; }
    }
    public sealed partial class SqlParameterCollection : System.Data.Common.DbParameterCollection
    {
        internal SqlParameterCollection() { }
        public override int Count { get { throw null; } }
        public new System.Data.SqlClient.SqlParameter this[int index] { get { throw null; } set { } }
        public new System.Data.SqlClient.SqlParameter this[string parameterName] { get { throw null; } set { } }
        public override object SyncRoot { get { throw null; } }
        public System.Data.SqlClient.SqlParameter Add(System.Data.SqlClient.SqlParameter value) { throw null; }
        public override int Add(object value) { throw null; }
        public System.Data.SqlClient.SqlParameter Add(string parameterName, System.Data.SqlDbType sqlDbType) { throw null; }
        public System.Data.SqlClient.SqlParameter Add(string parameterName, System.Data.SqlDbType sqlDbType, int size) { throw null; }
        public override void AddRange(System.Array values) { }
        public void AddRange(System.Data.SqlClient.SqlParameter[] values) { }
        public System.Data.SqlClient.SqlParameter AddWithValue(string parameterName, object value) { throw null; }
        public override void Clear() { }
        public bool Contains(System.Data.SqlClient.SqlParameter value) { throw null; }
        public override bool Contains(object value) { throw null; }
        public override bool Contains(string value) { throw null; }
        public override void CopyTo(System.Array array, int index) { }
        public void CopyTo(System.Data.SqlClient.SqlParameter[] array, int index) { }
        public override System.Collections.IEnumerator GetEnumerator() { throw null; }
        protected override System.Data.Common.DbParameter GetParameter(int index) { throw null; }
        protected override System.Data.Common.DbParameter GetParameter(string parameterName) { throw null; }
        public int IndexOf(System.Data.SqlClient.SqlParameter value) { throw null; }
        public override int IndexOf(object value) { throw null; }
        public override int IndexOf(string parameterName) { throw null; }
        public void Insert(int index, System.Data.SqlClient.SqlParameter value) { }
        public override void Insert(int index, object value) { }
        public void Remove(System.Data.SqlClient.SqlParameter value) { }
        public override void Remove(object value) { }
        public override void RemoveAt(int index) { }
        public override void RemoveAt(string parameterName) { }
        protected override void SetParameter(int index, System.Data.Common.DbParameter value) { }
        protected override void SetParameter(string parameterName, System.Data.Common.DbParameter value) { }
    }
    public sealed partial class SqlTransaction : System.Data.Common.DbTransaction
    {
        internal SqlTransaction() { }
        public new System.Data.SqlClient.SqlConnection Connection { get { throw null; } }
        protected override System.Data.Common.DbConnection DbConnection { get { throw null; } }
        public override System.Data.IsolationLevel IsolationLevel { get { throw null; } }
        public override void Commit() { }
        protected override void Dispose(bool disposing) { }
        public override void Rollback() { }
        public void Rollback(string transactionName) { }
        public void Save(string savePointName) { }
    }
}
namespace System.Data.SqlTypes
{
    public partial interface INullable
    {
        bool IsNull { get; }
    }
    public partial struct SqlBinary : System.Data.SqlTypes.INullable, System.IComparable
    {
        public static readonly System.Data.SqlTypes.SqlBinary Null;
        public SqlBinary(byte[] value) { throw null; }
        public bool IsNull { get { throw null; } }
        public byte this[int index] { get { throw null; } }
        public int Length { get { throw null; } }
        public byte[] Value { get { throw null; } }
        public static System.Data.SqlTypes.SqlBinary Add(System.Data.SqlTypes.SqlBinary x, System.Data.SqlTypes.SqlBinary y) { throw null; }
        public int CompareTo(System.Data.SqlTypes.SqlBinary value) { throw null; }
        public int CompareTo(object value) { throw null; }
        public static System.Data.SqlTypes.SqlBinary Concat(System.Data.SqlTypes.SqlBinary x, System.Data.SqlTypes.SqlBinary y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean Equals(System.Data.SqlTypes.SqlBinary x, System.Data.SqlTypes.SqlBinary y) { throw null; }
        public override bool Equals(object value) { throw null; }
        public override int GetHashCode() { throw null; }
        public static System.Data.SqlTypes.SqlBoolean GreaterThan(System.Data.SqlTypes.SqlBinary x, System.Data.SqlTypes.SqlBinary y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean GreaterThanOrEqual(System.Data.SqlTypes.SqlBinary x, System.Data.SqlTypes.SqlBinary y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean LessThan(System.Data.SqlTypes.SqlBinary x, System.Data.SqlTypes.SqlBinary y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean LessThanOrEqual(System.Data.SqlTypes.SqlBinary x, System.Data.SqlTypes.SqlBinary y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean NotEquals(System.Data.SqlTypes.SqlBinary x, System.Data.SqlTypes.SqlBinary y) { throw null; }
        public static System.Data.SqlTypes.SqlBinary operator +(System.Data.SqlTypes.SqlBinary x, System.Data.SqlTypes.SqlBinary y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator ==(System.Data.SqlTypes.SqlBinary x, System.Data.SqlTypes.SqlBinary y) { throw null; }
        public static explicit operator byte[] (System.Data.SqlTypes.SqlBinary x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlBinary (System.Data.SqlTypes.SqlGuid x) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator >(System.Data.SqlTypes.SqlBinary x, System.Data.SqlTypes.SqlBinary y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator >=(System.Data.SqlTypes.SqlBinary x, System.Data.SqlTypes.SqlBinary y) { throw null; }
        public static implicit operator System.Data.SqlTypes.SqlBinary (byte[] x) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator !=(System.Data.SqlTypes.SqlBinary x, System.Data.SqlTypes.SqlBinary y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator <(System.Data.SqlTypes.SqlBinary x, System.Data.SqlTypes.SqlBinary y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator <=(System.Data.SqlTypes.SqlBinary x, System.Data.SqlTypes.SqlBinary y) { throw null; }
        public System.Data.SqlTypes.SqlGuid ToSqlGuid() { throw null; }
        public override string ToString() { throw null; }
    }
    public partial struct SqlBoolean : System.Data.SqlTypes.INullable, System.IComparable
    {
        public static readonly System.Data.SqlTypes.SqlBoolean False;
        public static readonly System.Data.SqlTypes.SqlBoolean Null;
        public static readonly System.Data.SqlTypes.SqlBoolean One;
        public static readonly System.Data.SqlTypes.SqlBoolean True;
        public static readonly System.Data.SqlTypes.SqlBoolean Zero;
        public SqlBoolean(bool value) { throw null; }
        public SqlBoolean(int value) { throw null; }
        public byte ByteValue { get { throw null; } }
        public bool IsFalse { get { throw null; } }
        public bool IsNull { get { throw null; } }
        public bool IsTrue { get { throw null; } }
        public bool Value { get { throw null; } }
        public static System.Data.SqlTypes.SqlBoolean And(System.Data.SqlTypes.SqlBoolean x, System.Data.SqlTypes.SqlBoolean y) { throw null; }
        public int CompareTo(System.Data.SqlTypes.SqlBoolean value) { throw null; }
        public int CompareTo(object value) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean Equals(System.Data.SqlTypes.SqlBoolean x, System.Data.SqlTypes.SqlBoolean y) { throw null; }
        public override bool Equals(object value) { throw null; }
        public override int GetHashCode() { throw null; }
        public static System.Data.SqlTypes.SqlBoolean GreaterThan(System.Data.SqlTypes.SqlBoolean x, System.Data.SqlTypes.SqlBoolean y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean GreaterThanOrEquals(System.Data.SqlTypes.SqlBoolean x, System.Data.SqlTypes.SqlBoolean y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean LessThan(System.Data.SqlTypes.SqlBoolean x, System.Data.SqlTypes.SqlBoolean y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean LessThanOrEquals(System.Data.SqlTypes.SqlBoolean x, System.Data.SqlTypes.SqlBoolean y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean NotEquals(System.Data.SqlTypes.SqlBoolean x, System.Data.SqlTypes.SqlBoolean y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean OnesComplement(System.Data.SqlTypes.SqlBoolean x) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator &(System.Data.SqlTypes.SqlBoolean x, System.Data.SqlTypes.SqlBoolean y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator |(System.Data.SqlTypes.SqlBoolean x, System.Data.SqlTypes.SqlBoolean y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator ==(System.Data.SqlTypes.SqlBoolean x, System.Data.SqlTypes.SqlBoolean y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator ^(System.Data.SqlTypes.SqlBoolean x, System.Data.SqlTypes.SqlBoolean y) { throw null; }
        public static explicit operator bool (System.Data.SqlTypes.SqlBoolean x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlBoolean (System.Data.SqlTypes.SqlByte x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlBoolean (System.Data.SqlTypes.SqlDecimal x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlBoolean (System.Data.SqlTypes.SqlDouble x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlBoolean (System.Data.SqlTypes.SqlInt16 x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlBoolean (System.Data.SqlTypes.SqlInt32 x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlBoolean (System.Data.SqlTypes.SqlInt64 x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlBoolean (System.Data.SqlTypes.SqlMoney x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlBoolean (System.Data.SqlTypes.SqlSingle x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlBoolean (System.Data.SqlTypes.SqlString x) { throw null; }
        public static bool operator false(System.Data.SqlTypes.SqlBoolean x) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator >(System.Data.SqlTypes.SqlBoolean x, System.Data.SqlTypes.SqlBoolean y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator >=(System.Data.SqlTypes.SqlBoolean x, System.Data.SqlTypes.SqlBoolean y) { throw null; }
        public static implicit operator System.Data.SqlTypes.SqlBoolean (bool x) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator !=(System.Data.SqlTypes.SqlBoolean x, System.Data.SqlTypes.SqlBoolean y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator <(System.Data.SqlTypes.SqlBoolean x, System.Data.SqlTypes.SqlBoolean y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator <=(System.Data.SqlTypes.SqlBoolean x, System.Data.SqlTypes.SqlBoolean y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator !(System.Data.SqlTypes.SqlBoolean x) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator ~(System.Data.SqlTypes.SqlBoolean x) { throw null; }
        public static bool operator true(System.Data.SqlTypes.SqlBoolean x) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean Or(System.Data.SqlTypes.SqlBoolean x, System.Data.SqlTypes.SqlBoolean y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean Parse(string s) { throw null; }
        public System.Data.SqlTypes.SqlByte ToSqlByte() { throw null; }
        public System.Data.SqlTypes.SqlDecimal ToSqlDecimal() { throw null; }
        public System.Data.SqlTypes.SqlDouble ToSqlDouble() { throw null; }
        public System.Data.SqlTypes.SqlInt16 ToSqlInt16() { throw null; }
        public System.Data.SqlTypes.SqlInt32 ToSqlInt32() { throw null; }
        public System.Data.SqlTypes.SqlInt64 ToSqlInt64() { throw null; }
        public System.Data.SqlTypes.SqlMoney ToSqlMoney() { throw null; }
        public System.Data.SqlTypes.SqlSingle ToSqlSingle() { throw null; }
        public System.Data.SqlTypes.SqlString ToSqlString() { throw null; }
        public override string ToString() { throw null; }
        public static System.Data.SqlTypes.SqlBoolean Xor(System.Data.SqlTypes.SqlBoolean x, System.Data.SqlTypes.SqlBoolean y) { throw null; }
    }
    public partial struct SqlByte : System.Data.SqlTypes.INullable, System.IComparable
    {
        public static readonly System.Data.SqlTypes.SqlByte MaxValue;
        public static readonly System.Data.SqlTypes.SqlByte MinValue;
        public static readonly System.Data.SqlTypes.SqlByte Null;
        public static readonly System.Data.SqlTypes.SqlByte Zero;
        public SqlByte(byte value) { throw null; }
        public bool IsNull { get { throw null; } }
        public byte Value { get { throw null; } }
        public static System.Data.SqlTypes.SqlByte Add(System.Data.SqlTypes.SqlByte x, System.Data.SqlTypes.SqlByte y) { throw null; }
        public static System.Data.SqlTypes.SqlByte BitwiseAnd(System.Data.SqlTypes.SqlByte x, System.Data.SqlTypes.SqlByte y) { throw null; }
        public static System.Data.SqlTypes.SqlByte BitwiseOr(System.Data.SqlTypes.SqlByte x, System.Data.SqlTypes.SqlByte y) { throw null; }
        public int CompareTo(System.Data.SqlTypes.SqlByte value) { throw null; }
        public int CompareTo(object value) { throw null; }
        public static System.Data.SqlTypes.SqlByte Divide(System.Data.SqlTypes.SqlByte x, System.Data.SqlTypes.SqlByte y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean Equals(System.Data.SqlTypes.SqlByte x, System.Data.SqlTypes.SqlByte y) { throw null; }
        public override bool Equals(object value) { throw null; }
        public override int GetHashCode() { throw null; }
        public static System.Data.SqlTypes.SqlBoolean GreaterThan(System.Data.SqlTypes.SqlByte x, System.Data.SqlTypes.SqlByte y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean GreaterThanOrEqual(System.Data.SqlTypes.SqlByte x, System.Data.SqlTypes.SqlByte y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean LessThan(System.Data.SqlTypes.SqlByte x, System.Data.SqlTypes.SqlByte y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean LessThanOrEqual(System.Data.SqlTypes.SqlByte x, System.Data.SqlTypes.SqlByte y) { throw null; }
        public static System.Data.SqlTypes.SqlByte Mod(System.Data.SqlTypes.SqlByte x, System.Data.SqlTypes.SqlByte y) { throw null; }
        public static System.Data.SqlTypes.SqlByte Modulus(System.Data.SqlTypes.SqlByte x, System.Data.SqlTypes.SqlByte y) { throw null; }
        public static System.Data.SqlTypes.SqlByte Multiply(System.Data.SqlTypes.SqlByte x, System.Data.SqlTypes.SqlByte y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean NotEquals(System.Data.SqlTypes.SqlByte x, System.Data.SqlTypes.SqlByte y) { throw null; }
        public static System.Data.SqlTypes.SqlByte OnesComplement(System.Data.SqlTypes.SqlByte x) { throw null; }
        public static System.Data.SqlTypes.SqlByte operator +(System.Data.SqlTypes.SqlByte x, System.Data.SqlTypes.SqlByte y) { throw null; }
        public static System.Data.SqlTypes.SqlByte operator &(System.Data.SqlTypes.SqlByte x, System.Data.SqlTypes.SqlByte y) { throw null; }
        public static System.Data.SqlTypes.SqlByte operator |(System.Data.SqlTypes.SqlByte x, System.Data.SqlTypes.SqlByte y) { throw null; }
        public static System.Data.SqlTypes.SqlByte operator /(System.Data.SqlTypes.SqlByte x, System.Data.SqlTypes.SqlByte y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator ==(System.Data.SqlTypes.SqlByte x, System.Data.SqlTypes.SqlByte y) { throw null; }
        public static System.Data.SqlTypes.SqlByte operator ^(System.Data.SqlTypes.SqlByte x, System.Data.SqlTypes.SqlByte y) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlByte (System.Data.SqlTypes.SqlBoolean x) { throw null; }
        public static explicit operator byte (System.Data.SqlTypes.SqlByte x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlByte (System.Data.SqlTypes.SqlDecimal x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlByte (System.Data.SqlTypes.SqlDouble x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlByte (System.Data.SqlTypes.SqlInt16 x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlByte (System.Data.SqlTypes.SqlInt32 x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlByte (System.Data.SqlTypes.SqlInt64 x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlByte (System.Data.SqlTypes.SqlMoney x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlByte (System.Data.SqlTypes.SqlSingle x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlByte (System.Data.SqlTypes.SqlString x) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator >(System.Data.SqlTypes.SqlByte x, System.Data.SqlTypes.SqlByte y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator >=(System.Data.SqlTypes.SqlByte x, System.Data.SqlTypes.SqlByte y) { throw null; }
        public static implicit operator System.Data.SqlTypes.SqlByte (byte x) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator !=(System.Data.SqlTypes.SqlByte x, System.Data.SqlTypes.SqlByte y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator <(System.Data.SqlTypes.SqlByte x, System.Data.SqlTypes.SqlByte y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator <=(System.Data.SqlTypes.SqlByte x, System.Data.SqlTypes.SqlByte y) { throw null; }
        public static System.Data.SqlTypes.SqlByte operator %(System.Data.SqlTypes.SqlByte x, System.Data.SqlTypes.SqlByte y) { throw null; }
        public static System.Data.SqlTypes.SqlByte operator *(System.Data.SqlTypes.SqlByte x, System.Data.SqlTypes.SqlByte y) { throw null; }
        public static System.Data.SqlTypes.SqlByte operator ~(System.Data.SqlTypes.SqlByte x) { throw null; }
        public static System.Data.SqlTypes.SqlByte operator -(System.Data.SqlTypes.SqlByte x, System.Data.SqlTypes.SqlByte y) { throw null; }
        public static System.Data.SqlTypes.SqlByte Parse(string s) { throw null; }
        public static System.Data.SqlTypes.SqlByte Subtract(System.Data.SqlTypes.SqlByte x, System.Data.SqlTypes.SqlByte y) { throw null; }
        public System.Data.SqlTypes.SqlBoolean ToSqlBoolean() { throw null; }
        public System.Data.SqlTypes.SqlDecimal ToSqlDecimal() { throw null; }
        public System.Data.SqlTypes.SqlDouble ToSqlDouble() { throw null; }
        public System.Data.SqlTypes.SqlInt16 ToSqlInt16() { throw null; }
        public System.Data.SqlTypes.SqlInt32 ToSqlInt32() { throw null; }
        public System.Data.SqlTypes.SqlInt64 ToSqlInt64() { throw null; }
        public System.Data.SqlTypes.SqlMoney ToSqlMoney() { throw null; }
        public System.Data.SqlTypes.SqlSingle ToSqlSingle() { throw null; }
        public System.Data.SqlTypes.SqlString ToSqlString() { throw null; }
        public override string ToString() { throw null; }
        public static System.Data.SqlTypes.SqlByte Xor(System.Data.SqlTypes.SqlByte x, System.Data.SqlTypes.SqlByte y) { throw null; }
    }
    public sealed partial class SqlBytes : System.Data.SqlTypes.INullable
    {
        public SqlBytes() { }
        public SqlBytes(byte[] buffer) { }
        public SqlBytes(System.Data.SqlTypes.SqlBinary value) { }
        public SqlBytes(System.IO.Stream s) { }
        public byte[] Buffer { get { throw null; } }
        public bool IsNull { get { throw null; } }
        public byte this[long offset] { get { throw null; } set { } }
        public long Length { get { throw null; } }
        public long MaxLength { get { throw null; } }
        public static System.Data.SqlTypes.SqlBytes Null { get { throw null; } }
        public System.IO.Stream Stream { get { throw null; } set { } }
        public byte[] Value { get { throw null; } }
        public static explicit operator System.Data.SqlTypes.SqlBytes (System.Data.SqlTypes.SqlBinary value) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlBinary (System.Data.SqlTypes.SqlBytes value) { throw null; }
        public long Read(long offset, byte[] buffer, int offsetInBuffer, int count) { throw null; }
        public void SetLength(long value) { }
        public void SetNull() { }
        public System.Data.SqlTypes.SqlBinary ToSqlBinary() { throw null; }
        public void Write(long offset, byte[] buffer, int offsetInBuffer, int count) { }
    }
    public sealed partial class SqlChars : System.Data.SqlTypes.INullable
    {
        public SqlChars() { }
        public SqlChars(char[] buffer) { }
        public SqlChars(System.Data.SqlTypes.SqlString value) { }
        public char[] Buffer { get { throw null; } }
        public bool IsNull { get { throw null; } }
        public char this[long offset] { get { throw null; } set { } }
        public long Length { get { throw null; } }
        public long MaxLength { get { throw null; } }
        public static System.Data.SqlTypes.SqlChars Null { get { throw null; } }
        public char[] Value { get { throw null; } }
        public static explicit operator System.Data.SqlTypes.SqlString (System.Data.SqlTypes.SqlChars value) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlChars (System.Data.SqlTypes.SqlString value) { throw null; }
        public long Read(long offset, char[] buffer, int offsetInBuffer, int count) { throw null; }
        public void SetLength(long value) { }
        public void SetNull() { }
        public System.Data.SqlTypes.SqlString ToSqlString() { throw null; }
        public void Write(long offset, char[] buffer, int offsetInBuffer, int count) { }
    }
    [System.FlagsAttribute]
    public enum SqlCompareOptions
    {
        None = 0,
        IgnoreCase = 1,
        IgnoreNonSpace = 2,
        IgnoreKanaType = 8,
        IgnoreWidth = 16,
        BinarySort2 = 16384,
        BinarySort = 32768,
    }
    public partial struct SqlDateTime : System.Data.SqlTypes.INullable, System.IComparable
    {
        public static readonly System.Data.SqlTypes.SqlDateTime MaxValue;
        public static readonly System.Data.SqlTypes.SqlDateTime MinValue;
        public static readonly System.Data.SqlTypes.SqlDateTime Null;
        public static readonly int SQLTicksPerHour;
        public static readonly int SQLTicksPerMinute;
        public static readonly int SQLTicksPerSecond;
        public SqlDateTime(System.DateTime value) { throw null; }
        public SqlDateTime(int dayTicks, int timeTicks) { throw null; }
        public SqlDateTime(int year, int month, int day) { throw null; }
        public SqlDateTime(int year, int month, int day, int hour, int minute, int second) { throw null; }
        public SqlDateTime(int year, int month, int day, int hour, int minute, int second, double millisecond) { throw null; }
        public SqlDateTime(int year, int month, int day, int hour, int minute, int second, int bilisecond) { throw null; }
        public int DayTicks { get { throw null; } }
        public bool IsNull { get { throw null; } }
        public int TimeTicks { get { throw null; } }
        public System.DateTime Value { get { throw null; } }
        public static System.Data.SqlTypes.SqlDateTime Add(System.Data.SqlTypes.SqlDateTime x, System.TimeSpan t) { throw null; }
        public int CompareTo(System.Data.SqlTypes.SqlDateTime value) { throw null; }
        public int CompareTo(object value) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean Equals(System.Data.SqlTypes.SqlDateTime x, System.Data.SqlTypes.SqlDateTime y) { throw null; }
        public override bool Equals(object value) { throw null; }
        public override int GetHashCode() { throw null; }
        public static System.Data.SqlTypes.SqlBoolean GreaterThan(System.Data.SqlTypes.SqlDateTime x, System.Data.SqlTypes.SqlDateTime y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean GreaterThanOrEqual(System.Data.SqlTypes.SqlDateTime x, System.Data.SqlTypes.SqlDateTime y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean LessThan(System.Data.SqlTypes.SqlDateTime x, System.Data.SqlTypes.SqlDateTime y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean LessThanOrEqual(System.Data.SqlTypes.SqlDateTime x, System.Data.SqlTypes.SqlDateTime y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean NotEquals(System.Data.SqlTypes.SqlDateTime x, System.Data.SqlTypes.SqlDateTime y) { throw null; }
        public static System.Data.SqlTypes.SqlDateTime operator +(System.Data.SqlTypes.SqlDateTime x, System.TimeSpan t) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator ==(System.Data.SqlTypes.SqlDateTime x, System.Data.SqlTypes.SqlDateTime y) { throw null; }
        public static explicit operator System.DateTime (System.Data.SqlTypes.SqlDateTime x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlDateTime (System.Data.SqlTypes.SqlString x) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator >(System.Data.SqlTypes.SqlDateTime x, System.Data.SqlTypes.SqlDateTime y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator >=(System.Data.SqlTypes.SqlDateTime x, System.Data.SqlTypes.SqlDateTime y) { throw null; }
        public static implicit operator System.Data.SqlTypes.SqlDateTime (System.DateTime value) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator !=(System.Data.SqlTypes.SqlDateTime x, System.Data.SqlTypes.SqlDateTime y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator <(System.Data.SqlTypes.SqlDateTime x, System.Data.SqlTypes.SqlDateTime y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator <=(System.Data.SqlTypes.SqlDateTime x, System.Data.SqlTypes.SqlDateTime y) { throw null; }
        public static System.Data.SqlTypes.SqlDateTime operator -(System.Data.SqlTypes.SqlDateTime x, System.TimeSpan t) { throw null; }
        public static System.Data.SqlTypes.SqlDateTime Parse(string s) { throw null; }
        public static System.Data.SqlTypes.SqlDateTime Subtract(System.Data.SqlTypes.SqlDateTime x, System.TimeSpan t) { throw null; }
        public System.Data.SqlTypes.SqlString ToSqlString() { throw null; }
        public override string ToString() { throw null; }
    }
    public partial struct SqlDecimal : System.Data.SqlTypes.INullable, System.IComparable
    {
        public static readonly byte MaxPrecision;
        public static readonly byte MaxScale;
        public static readonly System.Data.SqlTypes.SqlDecimal MaxValue;
        public static readonly System.Data.SqlTypes.SqlDecimal MinValue;
        public static readonly System.Data.SqlTypes.SqlDecimal Null;
        public SqlDecimal(byte bPrecision, byte bScale, bool fPositive, int data1, int data2, int data3, int data4) { throw null; }
        public SqlDecimal(byte bPrecision, byte bScale, bool fPositive, int[] bits) { throw null; }
        public SqlDecimal(System.Decimal value) { throw null; }
        public SqlDecimal(double dVal) { throw null; }
        public SqlDecimal(int value) { throw null; }
        public SqlDecimal(long value) { throw null; }
        public byte[] BinData { get { throw null; } }
        public int[] Data { get { throw null; } }
        public bool IsNull { get { throw null; } }
        public bool IsPositive { get { throw null; } }
        public byte Precision { get { throw null; } }
        public byte Scale { get { throw null; } }
        public System.Decimal Value { get { throw null; } }
        public static System.Data.SqlTypes.SqlDecimal Abs(System.Data.SqlTypes.SqlDecimal n) { throw null; }
        public static System.Data.SqlTypes.SqlDecimal Add(System.Data.SqlTypes.SqlDecimal x, System.Data.SqlTypes.SqlDecimal y) { throw null; }
        public static System.Data.SqlTypes.SqlDecimal AdjustScale(System.Data.SqlTypes.SqlDecimal n, int digits, bool fRound) { throw null; }
        public static System.Data.SqlTypes.SqlDecimal Ceiling(System.Data.SqlTypes.SqlDecimal n) { throw null; }
        public int CompareTo(System.Data.SqlTypes.SqlDecimal value) { throw null; }
        public int CompareTo(object value) { throw null; }
        public static System.Data.SqlTypes.SqlDecimal ConvertToPrecScale(System.Data.SqlTypes.SqlDecimal n, int precision, int scale) { throw null; }
        public static System.Data.SqlTypes.SqlDecimal Divide(System.Data.SqlTypes.SqlDecimal x, System.Data.SqlTypes.SqlDecimal y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean Equals(System.Data.SqlTypes.SqlDecimal x, System.Data.SqlTypes.SqlDecimal y) { throw null; }
        public override bool Equals(object value) { throw null; }
        public static System.Data.SqlTypes.SqlDecimal Floor(System.Data.SqlTypes.SqlDecimal n) { throw null; }
        public override int GetHashCode() { throw null; }
        public static System.Data.SqlTypes.SqlBoolean GreaterThan(System.Data.SqlTypes.SqlDecimal x, System.Data.SqlTypes.SqlDecimal y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean GreaterThanOrEqual(System.Data.SqlTypes.SqlDecimal x, System.Data.SqlTypes.SqlDecimal y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean LessThan(System.Data.SqlTypes.SqlDecimal x, System.Data.SqlTypes.SqlDecimal y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean LessThanOrEqual(System.Data.SqlTypes.SqlDecimal x, System.Data.SqlTypes.SqlDecimal y) { throw null; }
        public static System.Data.SqlTypes.SqlDecimal Multiply(System.Data.SqlTypes.SqlDecimal x, System.Data.SqlTypes.SqlDecimal y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean NotEquals(System.Data.SqlTypes.SqlDecimal x, System.Data.SqlTypes.SqlDecimal y) { throw null; }
        public static System.Data.SqlTypes.SqlDecimal operator +(System.Data.SqlTypes.SqlDecimal x, System.Data.SqlTypes.SqlDecimal y) { throw null; }
        public static System.Data.SqlTypes.SqlDecimal operator /(System.Data.SqlTypes.SqlDecimal x, System.Data.SqlTypes.SqlDecimal y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator ==(System.Data.SqlTypes.SqlDecimal x, System.Data.SqlTypes.SqlDecimal y) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlDecimal (System.Data.SqlTypes.SqlBoolean x) { throw null; }
        public static explicit operator System.Decimal (System.Data.SqlTypes.SqlDecimal x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlDecimal (System.Data.SqlTypes.SqlDouble x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlDecimal (System.Data.SqlTypes.SqlSingle x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlDecimal (System.Data.SqlTypes.SqlString x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlDecimal (double x) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator >(System.Data.SqlTypes.SqlDecimal x, System.Data.SqlTypes.SqlDecimal y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator >=(System.Data.SqlTypes.SqlDecimal x, System.Data.SqlTypes.SqlDecimal y) { throw null; }
        public static implicit operator System.Data.SqlTypes.SqlDecimal (System.Data.SqlTypes.SqlByte x) { throw null; }
        public static implicit operator System.Data.SqlTypes.SqlDecimal (System.Data.SqlTypes.SqlInt16 x) { throw null; }
        public static implicit operator System.Data.SqlTypes.SqlDecimal (System.Data.SqlTypes.SqlInt32 x) { throw null; }
        public static implicit operator System.Data.SqlTypes.SqlDecimal (System.Data.SqlTypes.SqlInt64 x) { throw null; }
        public static implicit operator System.Data.SqlTypes.SqlDecimal (System.Data.SqlTypes.SqlMoney x) { throw null; }
        public static implicit operator System.Data.SqlTypes.SqlDecimal (System.Decimal x) { throw null; }
        public static implicit operator System.Data.SqlTypes.SqlDecimal (long x) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator !=(System.Data.SqlTypes.SqlDecimal x, System.Data.SqlTypes.SqlDecimal y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator <(System.Data.SqlTypes.SqlDecimal x, System.Data.SqlTypes.SqlDecimal y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator <=(System.Data.SqlTypes.SqlDecimal x, System.Data.SqlTypes.SqlDecimal y) { throw null; }
        public static System.Data.SqlTypes.SqlDecimal operator *(System.Data.SqlTypes.SqlDecimal x, System.Data.SqlTypes.SqlDecimal y) { throw null; }
        public static System.Data.SqlTypes.SqlDecimal operator -(System.Data.SqlTypes.SqlDecimal x, System.Data.SqlTypes.SqlDecimal y) { throw null; }
        public static System.Data.SqlTypes.SqlDecimal operator -(System.Data.SqlTypes.SqlDecimal x) { throw null; }
        public static System.Data.SqlTypes.SqlDecimal Parse(string s) { throw null; }
        public static System.Data.SqlTypes.SqlDecimal Power(System.Data.SqlTypes.SqlDecimal n, double exp) { throw null; }
        public static System.Data.SqlTypes.SqlDecimal Round(System.Data.SqlTypes.SqlDecimal n, int position) { throw null; }
        public static System.Data.SqlTypes.SqlInt32 Sign(System.Data.SqlTypes.SqlDecimal n) { throw null; }
        public static System.Data.SqlTypes.SqlDecimal Subtract(System.Data.SqlTypes.SqlDecimal x, System.Data.SqlTypes.SqlDecimal y) { throw null; }
        public double ToDouble() { throw null; }
        public System.Data.SqlTypes.SqlBoolean ToSqlBoolean() { throw null; }
        public System.Data.SqlTypes.SqlByte ToSqlByte() { throw null; }
        public System.Data.SqlTypes.SqlDouble ToSqlDouble() { throw null; }
        public System.Data.SqlTypes.SqlInt16 ToSqlInt16() { throw null; }
        public System.Data.SqlTypes.SqlInt32 ToSqlInt32() { throw null; }
        public System.Data.SqlTypes.SqlInt64 ToSqlInt64() { throw null; }
        public System.Data.SqlTypes.SqlMoney ToSqlMoney() { throw null; }
        public System.Data.SqlTypes.SqlSingle ToSqlSingle() { throw null; }
        public System.Data.SqlTypes.SqlString ToSqlString() { throw null; }
        public override string ToString() { throw null; }
        public static System.Data.SqlTypes.SqlDecimal Truncate(System.Data.SqlTypes.SqlDecimal n, int position) { throw null; }
    }
    public partial struct SqlDouble : System.Data.SqlTypes.INullable, System.IComparable
    {
        public static readonly System.Data.SqlTypes.SqlDouble MaxValue;
        public static readonly System.Data.SqlTypes.SqlDouble MinValue;
        public static readonly System.Data.SqlTypes.SqlDouble Null;
        public static readonly System.Data.SqlTypes.SqlDouble Zero;
        public SqlDouble(double value) { throw null; }
        public bool IsNull { get { throw null; } }
        public double Value { get { throw null; } }
        public static System.Data.SqlTypes.SqlDouble Add(System.Data.SqlTypes.SqlDouble x, System.Data.SqlTypes.SqlDouble y) { throw null; }
        public int CompareTo(System.Data.SqlTypes.SqlDouble value) { throw null; }
        public int CompareTo(object value) { throw null; }
        public static System.Data.SqlTypes.SqlDouble Divide(System.Data.SqlTypes.SqlDouble x, System.Data.SqlTypes.SqlDouble y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean Equals(System.Data.SqlTypes.SqlDouble x, System.Data.SqlTypes.SqlDouble y) { throw null; }
        public override bool Equals(object value) { throw null; }
        public override int GetHashCode() { throw null; }
        public static System.Data.SqlTypes.SqlBoolean GreaterThan(System.Data.SqlTypes.SqlDouble x, System.Data.SqlTypes.SqlDouble y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean GreaterThanOrEqual(System.Data.SqlTypes.SqlDouble x, System.Data.SqlTypes.SqlDouble y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean LessThan(System.Data.SqlTypes.SqlDouble x, System.Data.SqlTypes.SqlDouble y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean LessThanOrEqual(System.Data.SqlTypes.SqlDouble x, System.Data.SqlTypes.SqlDouble y) { throw null; }
        public static System.Data.SqlTypes.SqlDouble Multiply(System.Data.SqlTypes.SqlDouble x, System.Data.SqlTypes.SqlDouble y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean NotEquals(System.Data.SqlTypes.SqlDouble x, System.Data.SqlTypes.SqlDouble y) { throw null; }
        public static System.Data.SqlTypes.SqlDouble operator +(System.Data.SqlTypes.SqlDouble x, System.Data.SqlTypes.SqlDouble y) { throw null; }
        public static System.Data.SqlTypes.SqlDouble operator /(System.Data.SqlTypes.SqlDouble x, System.Data.SqlTypes.SqlDouble y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator ==(System.Data.SqlTypes.SqlDouble x, System.Data.SqlTypes.SqlDouble y) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlDouble (System.Data.SqlTypes.SqlBoolean x) { throw null; }
        public static explicit operator double (System.Data.SqlTypes.SqlDouble x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlDouble (System.Data.SqlTypes.SqlString x) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator >(System.Data.SqlTypes.SqlDouble x, System.Data.SqlTypes.SqlDouble y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator >=(System.Data.SqlTypes.SqlDouble x, System.Data.SqlTypes.SqlDouble y) { throw null; }
        public static implicit operator System.Data.SqlTypes.SqlDouble (System.Data.SqlTypes.SqlByte x) { throw null; }
        public static implicit operator System.Data.SqlTypes.SqlDouble (System.Data.SqlTypes.SqlDecimal x) { throw null; }
        public static implicit operator System.Data.SqlTypes.SqlDouble (System.Data.SqlTypes.SqlInt16 x) { throw null; }
        public static implicit operator System.Data.SqlTypes.SqlDouble (System.Data.SqlTypes.SqlInt32 x) { throw null; }
        public static implicit operator System.Data.SqlTypes.SqlDouble (System.Data.SqlTypes.SqlInt64 x) { throw null; }
        public static implicit operator System.Data.SqlTypes.SqlDouble (System.Data.SqlTypes.SqlMoney x) { throw null; }
        public static implicit operator System.Data.SqlTypes.SqlDouble (System.Data.SqlTypes.SqlSingle x) { throw null; }
        public static implicit operator System.Data.SqlTypes.SqlDouble (double x) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator !=(System.Data.SqlTypes.SqlDouble x, System.Data.SqlTypes.SqlDouble y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator <(System.Data.SqlTypes.SqlDouble x, System.Data.SqlTypes.SqlDouble y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator <=(System.Data.SqlTypes.SqlDouble x, System.Data.SqlTypes.SqlDouble y) { throw null; }
        public static System.Data.SqlTypes.SqlDouble operator *(System.Data.SqlTypes.SqlDouble x, System.Data.SqlTypes.SqlDouble y) { throw null; }
        public static System.Data.SqlTypes.SqlDouble operator -(System.Data.SqlTypes.SqlDouble x, System.Data.SqlTypes.SqlDouble y) { throw null; }
        public static System.Data.SqlTypes.SqlDouble operator -(System.Data.SqlTypes.SqlDouble x) { throw null; }
        public static System.Data.SqlTypes.SqlDouble Parse(string s) { throw null; }
        public static System.Data.SqlTypes.SqlDouble Subtract(System.Data.SqlTypes.SqlDouble x, System.Data.SqlTypes.SqlDouble y) { throw null; }
        public System.Data.SqlTypes.SqlBoolean ToSqlBoolean() { throw null; }
        public System.Data.SqlTypes.SqlByte ToSqlByte() { throw null; }
        public System.Data.SqlTypes.SqlDecimal ToSqlDecimal() { throw null; }
        public System.Data.SqlTypes.SqlInt16 ToSqlInt16() { throw null; }
        public System.Data.SqlTypes.SqlInt32 ToSqlInt32() { throw null; }
        public System.Data.SqlTypes.SqlInt64 ToSqlInt64() { throw null; }
        public System.Data.SqlTypes.SqlMoney ToSqlMoney() { throw null; }
        public System.Data.SqlTypes.SqlSingle ToSqlSingle() { throw null; }
        public System.Data.SqlTypes.SqlString ToSqlString() { throw null; }
        public override string ToString() { throw null; }
    }
    public partial struct SqlGuid : System.Data.SqlTypes.INullable, System.IComparable
    {
        public static readonly System.Data.SqlTypes.SqlGuid Null;
        public SqlGuid(byte[] value) { throw null; }
        public SqlGuid(System.Guid g) { throw null; }
        public SqlGuid(int a, short b, short c, byte d, byte e, byte f, byte g, byte h, byte i, byte j, byte k) { throw null; }
        public SqlGuid(string s) { throw null; }
        public bool IsNull { get { throw null; } }
        public System.Guid Value { get { throw null; } }
        public int CompareTo(System.Data.SqlTypes.SqlGuid value) { throw null; }
        public int CompareTo(object value) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean Equals(System.Data.SqlTypes.SqlGuid x, System.Data.SqlTypes.SqlGuid y) { throw null; }
        public override bool Equals(object value) { throw null; }
        public override int GetHashCode() { throw null; }
        public static System.Data.SqlTypes.SqlBoolean GreaterThan(System.Data.SqlTypes.SqlGuid x, System.Data.SqlTypes.SqlGuid y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean GreaterThanOrEqual(System.Data.SqlTypes.SqlGuid x, System.Data.SqlTypes.SqlGuid y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean LessThan(System.Data.SqlTypes.SqlGuid x, System.Data.SqlTypes.SqlGuid y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean LessThanOrEqual(System.Data.SqlTypes.SqlGuid x, System.Data.SqlTypes.SqlGuid y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean NotEquals(System.Data.SqlTypes.SqlGuid x, System.Data.SqlTypes.SqlGuid y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator ==(System.Data.SqlTypes.SqlGuid x, System.Data.SqlTypes.SqlGuid y) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlGuid (System.Data.SqlTypes.SqlBinary x) { throw null; }
        public static explicit operator System.Guid (System.Data.SqlTypes.SqlGuid x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlGuid (System.Data.SqlTypes.SqlString x) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator >(System.Data.SqlTypes.SqlGuid x, System.Data.SqlTypes.SqlGuid y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator >=(System.Data.SqlTypes.SqlGuid x, System.Data.SqlTypes.SqlGuid y) { throw null; }
        public static implicit operator System.Data.SqlTypes.SqlGuid (System.Guid x) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator !=(System.Data.SqlTypes.SqlGuid x, System.Data.SqlTypes.SqlGuid y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator <(System.Data.SqlTypes.SqlGuid x, System.Data.SqlTypes.SqlGuid y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator <=(System.Data.SqlTypes.SqlGuid x, System.Data.SqlTypes.SqlGuid y) { throw null; }
        public static System.Data.SqlTypes.SqlGuid Parse(string s) { throw null; }
        public byte[] ToByteArray() { throw null; }
        public System.Data.SqlTypes.SqlBinary ToSqlBinary() { throw null; }
        public System.Data.SqlTypes.SqlString ToSqlString() { throw null; }
        public override string ToString() { throw null; }
    }
    public partial struct SqlInt16 : System.Data.SqlTypes.INullable, System.IComparable
    {
        public static readonly System.Data.SqlTypes.SqlInt16 MaxValue;
        public static readonly System.Data.SqlTypes.SqlInt16 MinValue;
        public static readonly System.Data.SqlTypes.SqlInt16 Null;
        public static readonly System.Data.SqlTypes.SqlInt16 Zero;
        public SqlInt16(short value) { throw null; }
        public bool IsNull { get { throw null; } }
        public short Value { get { throw null; } }
        public static System.Data.SqlTypes.SqlInt16 Add(System.Data.SqlTypes.SqlInt16 x, System.Data.SqlTypes.SqlInt16 y) { throw null; }
        public static System.Data.SqlTypes.SqlInt16 BitwiseAnd(System.Data.SqlTypes.SqlInt16 x, System.Data.SqlTypes.SqlInt16 y) { throw null; }
        public static System.Data.SqlTypes.SqlInt16 BitwiseOr(System.Data.SqlTypes.SqlInt16 x, System.Data.SqlTypes.SqlInt16 y) { throw null; }
        public int CompareTo(System.Data.SqlTypes.SqlInt16 value) { throw null; }
        public int CompareTo(object value) { throw null; }
        public static System.Data.SqlTypes.SqlInt16 Divide(System.Data.SqlTypes.SqlInt16 x, System.Data.SqlTypes.SqlInt16 y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean Equals(System.Data.SqlTypes.SqlInt16 x, System.Data.SqlTypes.SqlInt16 y) { throw null; }
        public override bool Equals(object value) { throw null; }
        public override int GetHashCode() { throw null; }
        public static System.Data.SqlTypes.SqlBoolean GreaterThan(System.Data.SqlTypes.SqlInt16 x, System.Data.SqlTypes.SqlInt16 y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean GreaterThanOrEqual(System.Data.SqlTypes.SqlInt16 x, System.Data.SqlTypes.SqlInt16 y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean LessThan(System.Data.SqlTypes.SqlInt16 x, System.Data.SqlTypes.SqlInt16 y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean LessThanOrEqual(System.Data.SqlTypes.SqlInt16 x, System.Data.SqlTypes.SqlInt16 y) { throw null; }
        public static System.Data.SqlTypes.SqlInt16 Mod(System.Data.SqlTypes.SqlInt16 x, System.Data.SqlTypes.SqlInt16 y) { throw null; }
        public static System.Data.SqlTypes.SqlInt16 Modulus(System.Data.SqlTypes.SqlInt16 x, System.Data.SqlTypes.SqlInt16 y) { throw null; }
        public static System.Data.SqlTypes.SqlInt16 Multiply(System.Data.SqlTypes.SqlInt16 x, System.Data.SqlTypes.SqlInt16 y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean NotEquals(System.Data.SqlTypes.SqlInt16 x, System.Data.SqlTypes.SqlInt16 y) { throw null; }
        public static System.Data.SqlTypes.SqlInt16 OnesComplement(System.Data.SqlTypes.SqlInt16 x) { throw null; }
        public static System.Data.SqlTypes.SqlInt16 operator +(System.Data.SqlTypes.SqlInt16 x, System.Data.SqlTypes.SqlInt16 y) { throw null; }
        public static System.Data.SqlTypes.SqlInt16 operator &(System.Data.SqlTypes.SqlInt16 x, System.Data.SqlTypes.SqlInt16 y) { throw null; }
        public static System.Data.SqlTypes.SqlInt16 operator |(System.Data.SqlTypes.SqlInt16 x, System.Data.SqlTypes.SqlInt16 y) { throw null; }
        public static System.Data.SqlTypes.SqlInt16 operator /(System.Data.SqlTypes.SqlInt16 x, System.Data.SqlTypes.SqlInt16 y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator ==(System.Data.SqlTypes.SqlInt16 x, System.Data.SqlTypes.SqlInt16 y) { throw null; }
        public static System.Data.SqlTypes.SqlInt16 operator ^(System.Data.SqlTypes.SqlInt16 x, System.Data.SqlTypes.SqlInt16 y) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlInt16 (System.Data.SqlTypes.SqlBoolean x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlInt16 (System.Data.SqlTypes.SqlDecimal x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlInt16 (System.Data.SqlTypes.SqlDouble x) { throw null; }
        public static explicit operator short (System.Data.SqlTypes.SqlInt16 x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlInt16 (System.Data.SqlTypes.SqlInt32 x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlInt16 (System.Data.SqlTypes.SqlInt64 x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlInt16 (System.Data.SqlTypes.SqlMoney x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlInt16 (System.Data.SqlTypes.SqlSingle x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlInt16 (System.Data.SqlTypes.SqlString x) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator >(System.Data.SqlTypes.SqlInt16 x, System.Data.SqlTypes.SqlInt16 y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator >=(System.Data.SqlTypes.SqlInt16 x, System.Data.SqlTypes.SqlInt16 y) { throw null; }
        public static implicit operator System.Data.SqlTypes.SqlInt16 (System.Data.SqlTypes.SqlByte x) { throw null; }
        public static implicit operator System.Data.SqlTypes.SqlInt16 (short x) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator !=(System.Data.SqlTypes.SqlInt16 x, System.Data.SqlTypes.SqlInt16 y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator <(System.Data.SqlTypes.SqlInt16 x, System.Data.SqlTypes.SqlInt16 y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator <=(System.Data.SqlTypes.SqlInt16 x, System.Data.SqlTypes.SqlInt16 y) { throw null; }
        public static System.Data.SqlTypes.SqlInt16 operator %(System.Data.SqlTypes.SqlInt16 x, System.Data.SqlTypes.SqlInt16 y) { throw null; }
        public static System.Data.SqlTypes.SqlInt16 operator *(System.Data.SqlTypes.SqlInt16 x, System.Data.SqlTypes.SqlInt16 y) { throw null; }
        public static System.Data.SqlTypes.SqlInt16 operator ~(System.Data.SqlTypes.SqlInt16 x) { throw null; }
        public static System.Data.SqlTypes.SqlInt16 operator -(System.Data.SqlTypes.SqlInt16 x, System.Data.SqlTypes.SqlInt16 y) { throw null; }
        public static System.Data.SqlTypes.SqlInt16 operator -(System.Data.SqlTypes.SqlInt16 x) { throw null; }
        public static System.Data.SqlTypes.SqlInt16 Parse(string s) { throw null; }
        public static System.Data.SqlTypes.SqlInt16 Subtract(System.Data.SqlTypes.SqlInt16 x, System.Data.SqlTypes.SqlInt16 y) { throw null; }
        public System.Data.SqlTypes.SqlBoolean ToSqlBoolean() { throw null; }
        public System.Data.SqlTypes.SqlByte ToSqlByte() { throw null; }
        public System.Data.SqlTypes.SqlDecimal ToSqlDecimal() { throw null; }
        public System.Data.SqlTypes.SqlDouble ToSqlDouble() { throw null; }
        public System.Data.SqlTypes.SqlInt32 ToSqlInt32() { throw null; }
        public System.Data.SqlTypes.SqlInt64 ToSqlInt64() { throw null; }
        public System.Data.SqlTypes.SqlMoney ToSqlMoney() { throw null; }
        public System.Data.SqlTypes.SqlSingle ToSqlSingle() { throw null; }
        public System.Data.SqlTypes.SqlString ToSqlString() { throw null; }
        public override string ToString() { throw null; }
        public static System.Data.SqlTypes.SqlInt16 Xor(System.Data.SqlTypes.SqlInt16 x, System.Data.SqlTypes.SqlInt16 y) { throw null; }
    }
    public partial struct SqlInt32 : System.Data.SqlTypes.INullable, System.IComparable
    {
        public static readonly System.Data.SqlTypes.SqlInt32 MaxValue;
        public static readonly System.Data.SqlTypes.SqlInt32 MinValue;
        public static readonly System.Data.SqlTypes.SqlInt32 Null;
        public static readonly System.Data.SqlTypes.SqlInt32 Zero;
        public SqlInt32(int value) { throw null; }
        public bool IsNull { get { throw null; } }
        public int Value { get { throw null; } }
        public static System.Data.SqlTypes.SqlInt32 Add(System.Data.SqlTypes.SqlInt32 x, System.Data.SqlTypes.SqlInt32 y) { throw null; }
        public static System.Data.SqlTypes.SqlInt32 BitwiseAnd(System.Data.SqlTypes.SqlInt32 x, System.Data.SqlTypes.SqlInt32 y) { throw null; }
        public static System.Data.SqlTypes.SqlInt32 BitwiseOr(System.Data.SqlTypes.SqlInt32 x, System.Data.SqlTypes.SqlInt32 y) { throw null; }
        public int CompareTo(System.Data.SqlTypes.SqlInt32 value) { throw null; }
        public int CompareTo(object value) { throw null; }
        public static System.Data.SqlTypes.SqlInt32 Divide(System.Data.SqlTypes.SqlInt32 x, System.Data.SqlTypes.SqlInt32 y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean Equals(System.Data.SqlTypes.SqlInt32 x, System.Data.SqlTypes.SqlInt32 y) { throw null; }
        public override bool Equals(object value) { throw null; }
        public override int GetHashCode() { throw null; }
        public static System.Data.SqlTypes.SqlBoolean GreaterThan(System.Data.SqlTypes.SqlInt32 x, System.Data.SqlTypes.SqlInt32 y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean GreaterThanOrEqual(System.Data.SqlTypes.SqlInt32 x, System.Data.SqlTypes.SqlInt32 y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean LessThan(System.Data.SqlTypes.SqlInt32 x, System.Data.SqlTypes.SqlInt32 y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean LessThanOrEqual(System.Data.SqlTypes.SqlInt32 x, System.Data.SqlTypes.SqlInt32 y) { throw null; }
        public static System.Data.SqlTypes.SqlInt32 Mod(System.Data.SqlTypes.SqlInt32 x, System.Data.SqlTypes.SqlInt32 y) { throw null; }
        public static System.Data.SqlTypes.SqlInt32 Modulus(System.Data.SqlTypes.SqlInt32 x, System.Data.SqlTypes.SqlInt32 y) { throw null; }
        public static System.Data.SqlTypes.SqlInt32 Multiply(System.Data.SqlTypes.SqlInt32 x, System.Data.SqlTypes.SqlInt32 y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean NotEquals(System.Data.SqlTypes.SqlInt32 x, System.Data.SqlTypes.SqlInt32 y) { throw null; }
        public static System.Data.SqlTypes.SqlInt32 OnesComplement(System.Data.SqlTypes.SqlInt32 x) { throw null; }
        public static System.Data.SqlTypes.SqlInt32 operator +(System.Data.SqlTypes.SqlInt32 x, System.Data.SqlTypes.SqlInt32 y) { throw null; }
        public static System.Data.SqlTypes.SqlInt32 operator &(System.Data.SqlTypes.SqlInt32 x, System.Data.SqlTypes.SqlInt32 y) { throw null; }
        public static System.Data.SqlTypes.SqlInt32 operator |(System.Data.SqlTypes.SqlInt32 x, System.Data.SqlTypes.SqlInt32 y) { throw null; }
        public static System.Data.SqlTypes.SqlInt32 operator /(System.Data.SqlTypes.SqlInt32 x, System.Data.SqlTypes.SqlInt32 y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator ==(System.Data.SqlTypes.SqlInt32 x, System.Data.SqlTypes.SqlInt32 y) { throw null; }
        public static System.Data.SqlTypes.SqlInt32 operator ^(System.Data.SqlTypes.SqlInt32 x, System.Data.SqlTypes.SqlInt32 y) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlInt32 (System.Data.SqlTypes.SqlBoolean x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlInt32 (System.Data.SqlTypes.SqlDecimal x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlInt32 (System.Data.SqlTypes.SqlDouble x) { throw null; }
        public static explicit operator int (System.Data.SqlTypes.SqlInt32 x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlInt32 (System.Data.SqlTypes.SqlInt64 x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlInt32 (System.Data.SqlTypes.SqlMoney x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlInt32 (System.Data.SqlTypes.SqlSingle x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlInt32 (System.Data.SqlTypes.SqlString x) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator >(System.Data.SqlTypes.SqlInt32 x, System.Data.SqlTypes.SqlInt32 y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator >=(System.Data.SqlTypes.SqlInt32 x, System.Data.SqlTypes.SqlInt32 y) { throw null; }
        public static implicit operator System.Data.SqlTypes.SqlInt32 (System.Data.SqlTypes.SqlByte x) { throw null; }
        public static implicit operator System.Data.SqlTypes.SqlInt32 (System.Data.SqlTypes.SqlInt16 x) { throw null; }
        public static implicit operator System.Data.SqlTypes.SqlInt32 (int x) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator !=(System.Data.SqlTypes.SqlInt32 x, System.Data.SqlTypes.SqlInt32 y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator <(System.Data.SqlTypes.SqlInt32 x, System.Data.SqlTypes.SqlInt32 y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator <=(System.Data.SqlTypes.SqlInt32 x, System.Data.SqlTypes.SqlInt32 y) { throw null; }
        public static System.Data.SqlTypes.SqlInt32 operator %(System.Data.SqlTypes.SqlInt32 x, System.Data.SqlTypes.SqlInt32 y) { throw null; }
        public static System.Data.SqlTypes.SqlInt32 operator *(System.Data.SqlTypes.SqlInt32 x, System.Data.SqlTypes.SqlInt32 y) { throw null; }
        public static System.Data.SqlTypes.SqlInt32 operator ~(System.Data.SqlTypes.SqlInt32 x) { throw null; }
        public static System.Data.SqlTypes.SqlInt32 operator -(System.Data.SqlTypes.SqlInt32 x, System.Data.SqlTypes.SqlInt32 y) { throw null; }
        public static System.Data.SqlTypes.SqlInt32 operator -(System.Data.SqlTypes.SqlInt32 x) { throw null; }
        public static System.Data.SqlTypes.SqlInt32 Parse(string s) { throw null; }
        public static System.Data.SqlTypes.SqlInt32 Subtract(System.Data.SqlTypes.SqlInt32 x, System.Data.SqlTypes.SqlInt32 y) { throw null; }
        public System.Data.SqlTypes.SqlBoolean ToSqlBoolean() { throw null; }
        public System.Data.SqlTypes.SqlByte ToSqlByte() { throw null; }
        public System.Data.SqlTypes.SqlDecimal ToSqlDecimal() { throw null; }
        public System.Data.SqlTypes.SqlDouble ToSqlDouble() { throw null; }
        public System.Data.SqlTypes.SqlInt16 ToSqlInt16() { throw null; }
        public System.Data.SqlTypes.SqlInt64 ToSqlInt64() { throw null; }
        public System.Data.SqlTypes.SqlMoney ToSqlMoney() { throw null; }
        public System.Data.SqlTypes.SqlSingle ToSqlSingle() { throw null; }
        public System.Data.SqlTypes.SqlString ToSqlString() { throw null; }
        public override string ToString() { throw null; }
        public static System.Data.SqlTypes.SqlInt32 Xor(System.Data.SqlTypes.SqlInt32 x, System.Data.SqlTypes.SqlInt32 y) { throw null; }
    }
    public partial struct SqlInt64 : System.Data.SqlTypes.INullable, System.IComparable
    {
        public static readonly System.Data.SqlTypes.SqlInt64 MaxValue;
        public static readonly System.Data.SqlTypes.SqlInt64 MinValue;
        public static readonly System.Data.SqlTypes.SqlInt64 Null;
        public static readonly System.Data.SqlTypes.SqlInt64 Zero;
        public SqlInt64(long value) { throw null; }
        public bool IsNull { get { throw null; } }
        public long Value { get { throw null; } }
        public static System.Data.SqlTypes.SqlInt64 Add(System.Data.SqlTypes.SqlInt64 x, System.Data.SqlTypes.SqlInt64 y) { throw null; }
        public static System.Data.SqlTypes.SqlInt64 BitwiseAnd(System.Data.SqlTypes.SqlInt64 x, System.Data.SqlTypes.SqlInt64 y) { throw null; }
        public static System.Data.SqlTypes.SqlInt64 BitwiseOr(System.Data.SqlTypes.SqlInt64 x, System.Data.SqlTypes.SqlInt64 y) { throw null; }
        public int CompareTo(System.Data.SqlTypes.SqlInt64 value) { throw null; }
        public int CompareTo(object value) { throw null; }
        public static System.Data.SqlTypes.SqlInt64 Divide(System.Data.SqlTypes.SqlInt64 x, System.Data.SqlTypes.SqlInt64 y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean Equals(System.Data.SqlTypes.SqlInt64 x, System.Data.SqlTypes.SqlInt64 y) { throw null; }
        public override bool Equals(object value) { throw null; }
        public override int GetHashCode() { throw null; }
        public static System.Data.SqlTypes.SqlBoolean GreaterThan(System.Data.SqlTypes.SqlInt64 x, System.Data.SqlTypes.SqlInt64 y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean GreaterThanOrEqual(System.Data.SqlTypes.SqlInt64 x, System.Data.SqlTypes.SqlInt64 y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean LessThan(System.Data.SqlTypes.SqlInt64 x, System.Data.SqlTypes.SqlInt64 y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean LessThanOrEqual(System.Data.SqlTypes.SqlInt64 x, System.Data.SqlTypes.SqlInt64 y) { throw null; }
        public static System.Data.SqlTypes.SqlInt64 Mod(System.Data.SqlTypes.SqlInt64 x, System.Data.SqlTypes.SqlInt64 y) { throw null; }
        public static System.Data.SqlTypes.SqlInt64 Modulus(System.Data.SqlTypes.SqlInt64 x, System.Data.SqlTypes.SqlInt64 y) { throw null; }
        public static System.Data.SqlTypes.SqlInt64 Multiply(System.Data.SqlTypes.SqlInt64 x, System.Data.SqlTypes.SqlInt64 y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean NotEquals(System.Data.SqlTypes.SqlInt64 x, System.Data.SqlTypes.SqlInt64 y) { throw null; }
        public static System.Data.SqlTypes.SqlInt64 OnesComplement(System.Data.SqlTypes.SqlInt64 x) { throw null; }
        public static System.Data.SqlTypes.SqlInt64 operator +(System.Data.SqlTypes.SqlInt64 x, System.Data.SqlTypes.SqlInt64 y) { throw null; }
        public static System.Data.SqlTypes.SqlInt64 operator &(System.Data.SqlTypes.SqlInt64 x, System.Data.SqlTypes.SqlInt64 y) { throw null; }
        public static System.Data.SqlTypes.SqlInt64 operator |(System.Data.SqlTypes.SqlInt64 x, System.Data.SqlTypes.SqlInt64 y) { throw null; }
        public static System.Data.SqlTypes.SqlInt64 operator /(System.Data.SqlTypes.SqlInt64 x, System.Data.SqlTypes.SqlInt64 y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator ==(System.Data.SqlTypes.SqlInt64 x, System.Data.SqlTypes.SqlInt64 y) { throw null; }
        public static System.Data.SqlTypes.SqlInt64 operator ^(System.Data.SqlTypes.SqlInt64 x, System.Data.SqlTypes.SqlInt64 y) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlInt64 (System.Data.SqlTypes.SqlBoolean x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlInt64 (System.Data.SqlTypes.SqlDecimal x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlInt64 (System.Data.SqlTypes.SqlDouble x) { throw null; }
        public static explicit operator long (System.Data.SqlTypes.SqlInt64 x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlInt64 (System.Data.SqlTypes.SqlMoney x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlInt64 (System.Data.SqlTypes.SqlSingle x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlInt64 (System.Data.SqlTypes.SqlString x) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator >(System.Data.SqlTypes.SqlInt64 x, System.Data.SqlTypes.SqlInt64 y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator >=(System.Data.SqlTypes.SqlInt64 x, System.Data.SqlTypes.SqlInt64 y) { throw null; }
        public static implicit operator System.Data.SqlTypes.SqlInt64 (System.Data.SqlTypes.SqlByte x) { throw null; }
        public static implicit operator System.Data.SqlTypes.SqlInt64 (System.Data.SqlTypes.SqlInt16 x) { throw null; }
        public static implicit operator System.Data.SqlTypes.SqlInt64 (System.Data.SqlTypes.SqlInt32 x) { throw null; }
        public static implicit operator System.Data.SqlTypes.SqlInt64 (long x) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator !=(System.Data.SqlTypes.SqlInt64 x, System.Data.SqlTypes.SqlInt64 y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator <(System.Data.SqlTypes.SqlInt64 x, System.Data.SqlTypes.SqlInt64 y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator <=(System.Data.SqlTypes.SqlInt64 x, System.Data.SqlTypes.SqlInt64 y) { throw null; }
        public static System.Data.SqlTypes.SqlInt64 operator %(System.Data.SqlTypes.SqlInt64 x, System.Data.SqlTypes.SqlInt64 y) { throw null; }
        public static System.Data.SqlTypes.SqlInt64 operator *(System.Data.SqlTypes.SqlInt64 x, System.Data.SqlTypes.SqlInt64 y) { throw null; }
        public static System.Data.SqlTypes.SqlInt64 operator ~(System.Data.SqlTypes.SqlInt64 x) { throw null; }
        public static System.Data.SqlTypes.SqlInt64 operator -(System.Data.SqlTypes.SqlInt64 x, System.Data.SqlTypes.SqlInt64 y) { throw null; }
        public static System.Data.SqlTypes.SqlInt64 operator -(System.Data.SqlTypes.SqlInt64 x) { throw null; }
        public static System.Data.SqlTypes.SqlInt64 Parse(string s) { throw null; }
        public static System.Data.SqlTypes.SqlInt64 Subtract(System.Data.SqlTypes.SqlInt64 x, System.Data.SqlTypes.SqlInt64 y) { throw null; }
        public System.Data.SqlTypes.SqlBoolean ToSqlBoolean() { throw null; }
        public System.Data.SqlTypes.SqlByte ToSqlByte() { throw null; }
        public System.Data.SqlTypes.SqlDecimal ToSqlDecimal() { throw null; }
        public System.Data.SqlTypes.SqlDouble ToSqlDouble() { throw null; }
        public System.Data.SqlTypes.SqlInt16 ToSqlInt16() { throw null; }
        public System.Data.SqlTypes.SqlInt32 ToSqlInt32() { throw null; }
        public System.Data.SqlTypes.SqlMoney ToSqlMoney() { throw null; }
        public System.Data.SqlTypes.SqlSingle ToSqlSingle() { throw null; }
        public System.Data.SqlTypes.SqlString ToSqlString() { throw null; }
        public override string ToString() { throw null; }
        public static System.Data.SqlTypes.SqlInt64 Xor(System.Data.SqlTypes.SqlInt64 x, System.Data.SqlTypes.SqlInt64 y) { throw null; }
    }
    public partial struct SqlMoney : System.Data.SqlTypes.INullable, System.IComparable
    {
        public static readonly System.Data.SqlTypes.SqlMoney MaxValue;
        public static readonly System.Data.SqlTypes.SqlMoney MinValue;
        public static readonly System.Data.SqlTypes.SqlMoney Null;
        public static readonly System.Data.SqlTypes.SqlMoney Zero;
        public SqlMoney(System.Decimal value) { throw null; }
        public SqlMoney(double value) { throw null; }
        public SqlMoney(int value) { throw null; }
        public SqlMoney(long value) { throw null; }
        public bool IsNull { get { throw null; } }
        public System.Decimal Value { get { throw null; } }
        public static System.Data.SqlTypes.SqlMoney Add(System.Data.SqlTypes.SqlMoney x, System.Data.SqlTypes.SqlMoney y) { throw null; }
        public int CompareTo(System.Data.SqlTypes.SqlMoney value) { throw null; }
        public int CompareTo(object value) { throw null; }
        public static System.Data.SqlTypes.SqlMoney Divide(System.Data.SqlTypes.SqlMoney x, System.Data.SqlTypes.SqlMoney y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean Equals(System.Data.SqlTypes.SqlMoney x, System.Data.SqlTypes.SqlMoney y) { throw null; }
        public override bool Equals(object value) { throw null; }
        public override int GetHashCode() { throw null; }
        public static System.Data.SqlTypes.SqlBoolean GreaterThan(System.Data.SqlTypes.SqlMoney x, System.Data.SqlTypes.SqlMoney y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean GreaterThanOrEqual(System.Data.SqlTypes.SqlMoney x, System.Data.SqlTypes.SqlMoney y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean LessThan(System.Data.SqlTypes.SqlMoney x, System.Data.SqlTypes.SqlMoney y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean LessThanOrEqual(System.Data.SqlTypes.SqlMoney x, System.Data.SqlTypes.SqlMoney y) { throw null; }
        public static System.Data.SqlTypes.SqlMoney Multiply(System.Data.SqlTypes.SqlMoney x, System.Data.SqlTypes.SqlMoney y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean NotEquals(System.Data.SqlTypes.SqlMoney x, System.Data.SqlTypes.SqlMoney y) { throw null; }
        public static System.Data.SqlTypes.SqlMoney operator +(System.Data.SqlTypes.SqlMoney x, System.Data.SqlTypes.SqlMoney y) { throw null; }
        public static System.Data.SqlTypes.SqlMoney operator /(System.Data.SqlTypes.SqlMoney x, System.Data.SqlTypes.SqlMoney y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator ==(System.Data.SqlTypes.SqlMoney x, System.Data.SqlTypes.SqlMoney y) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlMoney (System.Data.SqlTypes.SqlBoolean x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlMoney (System.Data.SqlTypes.SqlDecimal x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlMoney (System.Data.SqlTypes.SqlDouble x) { throw null; }
        public static explicit operator System.Decimal (System.Data.SqlTypes.SqlMoney x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlMoney (System.Data.SqlTypes.SqlSingle x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlMoney (System.Data.SqlTypes.SqlString x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlMoney (double x) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator >(System.Data.SqlTypes.SqlMoney x, System.Data.SqlTypes.SqlMoney y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator >=(System.Data.SqlTypes.SqlMoney x, System.Data.SqlTypes.SqlMoney y) { throw null; }
        public static implicit operator System.Data.SqlTypes.SqlMoney (System.Data.SqlTypes.SqlByte x) { throw null; }
        public static implicit operator System.Data.SqlTypes.SqlMoney (System.Data.SqlTypes.SqlInt16 x) { throw null; }
        public static implicit operator System.Data.SqlTypes.SqlMoney (System.Data.SqlTypes.SqlInt32 x) { throw null; }
        public static implicit operator System.Data.SqlTypes.SqlMoney (System.Data.SqlTypes.SqlInt64 x) { throw null; }
        public static implicit operator System.Data.SqlTypes.SqlMoney (System.Decimal x) { throw null; }
        public static implicit operator System.Data.SqlTypes.SqlMoney (long x) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator !=(System.Data.SqlTypes.SqlMoney x, System.Data.SqlTypes.SqlMoney y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator <(System.Data.SqlTypes.SqlMoney x, System.Data.SqlTypes.SqlMoney y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator <=(System.Data.SqlTypes.SqlMoney x, System.Data.SqlTypes.SqlMoney y) { throw null; }
        public static System.Data.SqlTypes.SqlMoney operator *(System.Data.SqlTypes.SqlMoney x, System.Data.SqlTypes.SqlMoney y) { throw null; }
        public static System.Data.SqlTypes.SqlMoney operator -(System.Data.SqlTypes.SqlMoney x, System.Data.SqlTypes.SqlMoney y) { throw null; }
        public static System.Data.SqlTypes.SqlMoney operator -(System.Data.SqlTypes.SqlMoney x) { throw null; }
        public static System.Data.SqlTypes.SqlMoney Parse(string s) { throw null; }
        public static System.Data.SqlTypes.SqlMoney Subtract(System.Data.SqlTypes.SqlMoney x, System.Data.SqlTypes.SqlMoney y) { throw null; }
        public System.Decimal ToDecimal() { throw null; }
        public double ToDouble() { throw null; }
        public int ToInt32() { throw null; }
        public long ToInt64() { throw null; }
        public System.Data.SqlTypes.SqlBoolean ToSqlBoolean() { throw null; }
        public System.Data.SqlTypes.SqlByte ToSqlByte() { throw null; }
        public System.Data.SqlTypes.SqlDecimal ToSqlDecimal() { throw null; }
        public System.Data.SqlTypes.SqlDouble ToSqlDouble() { throw null; }
        public System.Data.SqlTypes.SqlInt16 ToSqlInt16() { throw null; }
        public System.Data.SqlTypes.SqlInt32 ToSqlInt32() { throw null; }
        public System.Data.SqlTypes.SqlInt64 ToSqlInt64() { throw null; }
        public System.Data.SqlTypes.SqlSingle ToSqlSingle() { throw null; }
        public System.Data.SqlTypes.SqlString ToSqlString() { throw null; }
        public override string ToString() { throw null; }
    }
    public sealed partial class SqlNullValueException : System.Data.SqlTypes.SqlTypeException
    {
        public SqlNullValueException() { }
        public SqlNullValueException(string message) { }
        public SqlNullValueException(string message, System.Exception e) { }
    }
    public partial struct SqlSingle : System.Data.SqlTypes.INullable, System.IComparable
    {
        public static readonly System.Data.SqlTypes.SqlSingle MaxValue;
        public static readonly System.Data.SqlTypes.SqlSingle MinValue;
        public static readonly System.Data.SqlTypes.SqlSingle Null;
        public static readonly System.Data.SqlTypes.SqlSingle Zero;
        public SqlSingle(double value) { throw null; }
        public SqlSingle(float value) { throw null; }
        public bool IsNull { get { throw null; } }
        public float Value { get { throw null; } }
        public static System.Data.SqlTypes.SqlSingle Add(System.Data.SqlTypes.SqlSingle x, System.Data.SqlTypes.SqlSingle y) { throw null; }
        public int CompareTo(System.Data.SqlTypes.SqlSingle value) { throw null; }
        public int CompareTo(object value) { throw null; }
        public static System.Data.SqlTypes.SqlSingle Divide(System.Data.SqlTypes.SqlSingle x, System.Data.SqlTypes.SqlSingle y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean Equals(System.Data.SqlTypes.SqlSingle x, System.Data.SqlTypes.SqlSingle y) { throw null; }
        public override bool Equals(object value) { throw null; }
        public override int GetHashCode() { throw null; }
        public static System.Data.SqlTypes.SqlBoolean GreaterThan(System.Data.SqlTypes.SqlSingle x, System.Data.SqlTypes.SqlSingle y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean GreaterThanOrEqual(System.Data.SqlTypes.SqlSingle x, System.Data.SqlTypes.SqlSingle y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean LessThan(System.Data.SqlTypes.SqlSingle x, System.Data.SqlTypes.SqlSingle y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean LessThanOrEqual(System.Data.SqlTypes.SqlSingle x, System.Data.SqlTypes.SqlSingle y) { throw null; }
        public static System.Data.SqlTypes.SqlSingle Multiply(System.Data.SqlTypes.SqlSingle x, System.Data.SqlTypes.SqlSingle y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean NotEquals(System.Data.SqlTypes.SqlSingle x, System.Data.SqlTypes.SqlSingle y) { throw null; }
        public static System.Data.SqlTypes.SqlSingle operator +(System.Data.SqlTypes.SqlSingle x, System.Data.SqlTypes.SqlSingle y) { throw null; }
        public static System.Data.SqlTypes.SqlSingle operator /(System.Data.SqlTypes.SqlSingle x, System.Data.SqlTypes.SqlSingle y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator ==(System.Data.SqlTypes.SqlSingle x, System.Data.SqlTypes.SqlSingle y) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlSingle (System.Data.SqlTypes.SqlBoolean x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlSingle (System.Data.SqlTypes.SqlDouble x) { throw null; }
        public static explicit operator float (System.Data.SqlTypes.SqlSingle x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlSingle (System.Data.SqlTypes.SqlString x) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator >(System.Data.SqlTypes.SqlSingle x, System.Data.SqlTypes.SqlSingle y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator >=(System.Data.SqlTypes.SqlSingle x, System.Data.SqlTypes.SqlSingle y) { throw null; }
        public static implicit operator System.Data.SqlTypes.SqlSingle (System.Data.SqlTypes.SqlByte x) { throw null; }
        public static implicit operator System.Data.SqlTypes.SqlSingle (System.Data.SqlTypes.SqlDecimal x) { throw null; }
        public static implicit operator System.Data.SqlTypes.SqlSingle (System.Data.SqlTypes.SqlInt16 x) { throw null; }
        public static implicit operator System.Data.SqlTypes.SqlSingle (System.Data.SqlTypes.SqlInt32 x) { throw null; }
        public static implicit operator System.Data.SqlTypes.SqlSingle (System.Data.SqlTypes.SqlInt64 x) { throw null; }
        public static implicit operator System.Data.SqlTypes.SqlSingle (System.Data.SqlTypes.SqlMoney x) { throw null; }
        public static implicit operator System.Data.SqlTypes.SqlSingle (float x) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator !=(System.Data.SqlTypes.SqlSingle x, System.Data.SqlTypes.SqlSingle y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator <(System.Data.SqlTypes.SqlSingle x, System.Data.SqlTypes.SqlSingle y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator <=(System.Data.SqlTypes.SqlSingle x, System.Data.SqlTypes.SqlSingle y) { throw null; }
        public static System.Data.SqlTypes.SqlSingle operator *(System.Data.SqlTypes.SqlSingle x, System.Data.SqlTypes.SqlSingle y) { throw null; }
        public static System.Data.SqlTypes.SqlSingle operator -(System.Data.SqlTypes.SqlSingle x, System.Data.SqlTypes.SqlSingle y) { throw null; }
        public static System.Data.SqlTypes.SqlSingle operator -(System.Data.SqlTypes.SqlSingle x) { throw null; }
        public static System.Data.SqlTypes.SqlSingle Parse(string s) { throw null; }
        public static System.Data.SqlTypes.SqlSingle Subtract(System.Data.SqlTypes.SqlSingle x, System.Data.SqlTypes.SqlSingle y) { throw null; }
        public System.Data.SqlTypes.SqlBoolean ToSqlBoolean() { throw null; }
        public System.Data.SqlTypes.SqlByte ToSqlByte() { throw null; }
        public System.Data.SqlTypes.SqlDecimal ToSqlDecimal() { throw null; }
        public System.Data.SqlTypes.SqlDouble ToSqlDouble() { throw null; }
        public System.Data.SqlTypes.SqlInt16 ToSqlInt16() { throw null; }
        public System.Data.SqlTypes.SqlInt32 ToSqlInt32() { throw null; }
        public System.Data.SqlTypes.SqlInt64 ToSqlInt64() { throw null; }
        public System.Data.SqlTypes.SqlMoney ToSqlMoney() { throw null; }
        public System.Data.SqlTypes.SqlString ToSqlString() { throw null; }
        public override string ToString() { throw null; }
    }
    public partial struct SqlString : System.Data.SqlTypes.INullable, System.IComparable
    {
        public static readonly int BinarySort;
        public static readonly int BinarySort2;
        public static readonly int IgnoreCase;
        public static readonly int IgnoreKanaType;
        public static readonly int IgnoreNonSpace;
        public static readonly int IgnoreWidth;
        public static readonly System.Data.SqlTypes.SqlString Null;
        public SqlString(int lcid, System.Data.SqlTypes.SqlCompareOptions compareOptions, byte[] data) { throw null; }
        public SqlString(int lcid, System.Data.SqlTypes.SqlCompareOptions compareOptions, byte[] data, bool fUnicode) { throw null; }
        public SqlString(int lcid, System.Data.SqlTypes.SqlCompareOptions compareOptions, byte[] data, int index, int count) { throw null; }
        public SqlString(int lcid, System.Data.SqlTypes.SqlCompareOptions compareOptions, byte[] data, int index, int count, bool fUnicode) { throw null; }
        public SqlString(string data) { throw null; }
        public SqlString(string data, int lcid) { throw null; }
        public SqlString(string data, int lcid, System.Data.SqlTypes.SqlCompareOptions compareOptions) { throw null; }
        public System.Globalization.CompareInfo CompareInfo { get { throw null; } }
        public System.Globalization.CultureInfo CultureInfo { get { throw null; } }
        public bool IsNull { get { throw null; } }
        public int LCID { get { throw null; } }
        public System.Data.SqlTypes.SqlCompareOptions SqlCompareOptions { get { throw null; } }
        public string Value { get { throw null; } }
        public static System.Data.SqlTypes.SqlString Add(System.Data.SqlTypes.SqlString x, System.Data.SqlTypes.SqlString y) { throw null; }
        public System.Data.SqlTypes.SqlString Clone() { throw null; }
        public static System.Globalization.CompareOptions CompareOptionsFromSqlCompareOptions(System.Data.SqlTypes.SqlCompareOptions compareOptions) { throw null; }
        public int CompareTo(System.Data.SqlTypes.SqlString value) { throw null; }
        public int CompareTo(object value) { throw null; }
        public static System.Data.SqlTypes.SqlString Concat(System.Data.SqlTypes.SqlString x, System.Data.SqlTypes.SqlString y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean Equals(System.Data.SqlTypes.SqlString x, System.Data.SqlTypes.SqlString y) { throw null; }
        public override bool Equals(object value) { throw null; }
        public override int GetHashCode() { throw null; }
        public byte[] GetNonUnicodeBytes() { throw null; }
        public byte[] GetUnicodeBytes() { throw null; }
        public static System.Data.SqlTypes.SqlBoolean GreaterThan(System.Data.SqlTypes.SqlString x, System.Data.SqlTypes.SqlString y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean GreaterThanOrEqual(System.Data.SqlTypes.SqlString x, System.Data.SqlTypes.SqlString y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean LessThan(System.Data.SqlTypes.SqlString x, System.Data.SqlTypes.SqlString y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean LessThanOrEqual(System.Data.SqlTypes.SqlString x, System.Data.SqlTypes.SqlString y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean NotEquals(System.Data.SqlTypes.SqlString x, System.Data.SqlTypes.SqlString y) { throw null; }
        public static System.Data.SqlTypes.SqlString operator +(System.Data.SqlTypes.SqlString x, System.Data.SqlTypes.SqlString y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator ==(System.Data.SqlTypes.SqlString x, System.Data.SqlTypes.SqlString y) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlString (System.Data.SqlTypes.SqlBoolean x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlString (System.Data.SqlTypes.SqlByte x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlString (System.Data.SqlTypes.SqlDateTime x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlString (System.Data.SqlTypes.SqlDecimal x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlString (System.Data.SqlTypes.SqlDouble x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlString (System.Data.SqlTypes.SqlGuid x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlString (System.Data.SqlTypes.SqlInt16 x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlString (System.Data.SqlTypes.SqlInt32 x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlString (System.Data.SqlTypes.SqlInt64 x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlString (System.Data.SqlTypes.SqlMoney x) { throw null; }
        public static explicit operator System.Data.SqlTypes.SqlString (System.Data.SqlTypes.SqlSingle x) { throw null; }
        public static explicit operator string (System.Data.SqlTypes.SqlString x) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator >(System.Data.SqlTypes.SqlString x, System.Data.SqlTypes.SqlString y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator >=(System.Data.SqlTypes.SqlString x, System.Data.SqlTypes.SqlString y) { throw null; }
        public static implicit operator System.Data.SqlTypes.SqlString (string x) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator !=(System.Data.SqlTypes.SqlString x, System.Data.SqlTypes.SqlString y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator <(System.Data.SqlTypes.SqlString x, System.Data.SqlTypes.SqlString y) { throw null; }
        public static System.Data.SqlTypes.SqlBoolean operator <=(System.Data.SqlTypes.SqlString x, System.Data.SqlTypes.SqlString y) { throw null; }
        public System.Data.SqlTypes.SqlBoolean ToSqlBoolean() { throw null; }
        public System.Data.SqlTypes.SqlByte ToSqlByte() { throw null; }
        public System.Data.SqlTypes.SqlDateTime ToSqlDateTime() { throw null; }
        public System.Data.SqlTypes.SqlDecimal ToSqlDecimal() { throw null; }
        public System.Data.SqlTypes.SqlDouble ToSqlDouble() { throw null; }
        public System.Data.SqlTypes.SqlGuid ToSqlGuid() { throw null; }
        public System.Data.SqlTypes.SqlInt16 ToSqlInt16() { throw null; }
        public System.Data.SqlTypes.SqlInt32 ToSqlInt32() { throw null; }
        public System.Data.SqlTypes.SqlInt64 ToSqlInt64() { throw null; }
        public System.Data.SqlTypes.SqlMoney ToSqlMoney() { throw null; }
        public System.Data.SqlTypes.SqlSingle ToSqlSingle() { throw null; }
        public override string ToString() { throw null; }
    }
    public sealed partial class SqlTruncateException : System.Data.SqlTypes.SqlTypeException
    {
        public SqlTruncateException() { }
        public SqlTruncateException(string message) { }
        public SqlTruncateException(string message, System.Exception e) { }
    }
    public partial class SqlTypeException : System.Exception
    {
        public SqlTypeException() { }
        public SqlTypeException(string message) { }
        public SqlTypeException(string message, System.Exception e) { }
    }
    public sealed partial class SqlXml : System.Data.SqlTypes.INullable
    {
        public SqlXml() { }
        public SqlXml(System.IO.Stream value) { }
        public SqlXml(System.Xml.XmlReader value) { }
        public bool IsNull { get { throw null; } }
        public static System.Data.SqlTypes.SqlXml Null { get { throw null; } }
        public string Value { get { throw null; } }
        public System.Xml.XmlReader CreateReader() { throw null; }
    }
}

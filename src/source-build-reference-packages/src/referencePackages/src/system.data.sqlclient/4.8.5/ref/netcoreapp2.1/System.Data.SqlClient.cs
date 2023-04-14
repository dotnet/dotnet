// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Data.SqlClient")]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("System.Data.SqlClient")]
[assembly: System.Reflection.AssemblyFileVersion("4.700.22.51706")]
[assembly: System.Reflection.AssemblyInformationalVersion("3.1.31+1acb337ac383d3f7d4a30bd3e46ef771459baf03")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Core")]
[assembly: System.Reflection.AssemblyTitle("System.Data.SqlClient")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.CLSCompliant(true)]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.System32)]
[assembly: System.Reflection.AssemblyVersionAttribute("4.6.1.5")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Data.SqlDbType))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Data.SqlTypes.INullable))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Data.SqlTypes.SqlBinary))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Data.SqlTypes.SqlBoolean))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Data.SqlTypes.SqlByte))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Data.SqlTypes.SqlBytes))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Data.SqlTypes.SqlChars))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Data.SqlTypes.SqlCompareOptions))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Data.SqlTypes.SqlDateTime))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Data.SqlTypes.SqlDecimal))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Data.SqlTypes.SqlDouble))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Data.SqlTypes.SqlGuid))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Data.SqlTypes.SqlInt16))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Data.SqlTypes.SqlInt32))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Data.SqlTypes.SqlInt64))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Data.SqlTypes.SqlMoney))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Data.SqlTypes.SqlNullValueException))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Data.SqlTypes.SqlSingle))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Data.SqlTypes.SqlString))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Data.SqlTypes.SqlTruncateException))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Data.SqlTypes.SqlTypeException))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Data.SqlTypes.SqlXml))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Data.StatementCompletedEventArgs))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Data.StatementCompletedEventHandler))]
namespace Microsoft.SqlServer.Server
{
    public enum DataAccessKind
    {
        None = 0,
        Read = 1
    }

    public enum Format
    {
        Unknown = 0,
        Native = 1,
        UserDefined = 2
    }

    public partial interface IBinarySerialize
    {
        void Read(System.IO.BinaryReader r);
        void Write(System.IO.BinaryWriter w);
    }

    public sealed partial class InvalidUdtException : System.SystemException
    {
    }

    public partial class SqlDataRecord : System.Data.IDataRecord
    {
        public SqlDataRecord(params SqlMetaData[] metaData) { }

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

        public virtual decimal GetDecimal(int ordinal) { throw null; }

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

        public virtual SqlMetaData GetSqlMetaData(int ordinal) { throw null; }

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

        public virtual void SetDecimal(int ordinal, decimal value) { }

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

        System.Data.IDataReader System.Data.IDataRecord.GetData(int ordinal) { throw null; }
    }

    [System.AttributeUsage(System.AttributeTargets.Property | System.AttributeTargets.Field | System.AttributeTargets.Parameter | System.AttributeTargets.ReturnValue, AllowMultiple = false, Inherited = false)]
    public partial class SqlFacetAttribute : System.Attribute
    {
        public SqlFacetAttribute() { }

        public bool IsFixedLength { get { throw null; } set { } }

        public bool IsNullable { get { throw null; } set { } }

        public int MaxSize { get { throw null; } set { } }

        public int Precision { get { throw null; } set { } }

        public int Scale { get { throw null; } set { } }
    }

    [System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public partial class SqlFunctionAttribute : System.Attribute
    {
        public SqlFunctionAttribute() { }

        public DataAccessKind DataAccess { get { throw null; } set { } }

        public string FillRowMethodName { get { throw null; } set { } }

        public bool IsDeterministic { get { throw null; } set { } }

        public bool IsPrecise { get { throw null; } set { } }

        public string Name { get { throw null; } set { } }

        public SystemDataAccessKind SystemDataAccess { get { throw null; } set { } }

        public string TableDefinition { get { throw null; } set { } }
    }

    public sealed partial class SqlMetaData
    {
        public SqlMetaData(string name, System.Data.SqlDbType dbType, bool useServerDefault, bool isUniqueKey, System.Data.SqlClient.SortOrder columnSortOrder, int sortOrdinal) { }

        public SqlMetaData(string name, System.Data.SqlDbType dbType, byte precision, byte scale, bool useServerDefault, bool isUniqueKey, System.Data.SqlClient.SortOrder columnSortOrder, int sortOrdinal) { }

        public SqlMetaData(string name, System.Data.SqlDbType dbType, byte precision, byte scale) { }

        public SqlMetaData(string name, System.Data.SqlDbType dbType, long maxLength, bool useServerDefault, bool isUniqueKey, System.Data.SqlClient.SortOrder columnSortOrder, int sortOrdinal) { }

        public SqlMetaData(string name, System.Data.SqlDbType dbType, long maxLength, byte precision, byte scale, long localeId, System.Data.SqlTypes.SqlCompareOptions compareOptions, System.Type userDefinedType, bool useServerDefault, bool isUniqueKey, System.Data.SqlClient.SortOrder columnSortOrder, int sortOrdinal) { }

        public SqlMetaData(string name, System.Data.SqlDbType dbType, long maxLength, byte precision, byte scale, long locale, System.Data.SqlTypes.SqlCompareOptions compareOptions, System.Type userDefinedType) { }

        public SqlMetaData(string name, System.Data.SqlDbType dbType, long maxLength, long locale, System.Data.SqlTypes.SqlCompareOptions compareOptions, bool useServerDefault, bool isUniqueKey, System.Data.SqlClient.SortOrder columnSortOrder, int sortOrdinal) { }

        public SqlMetaData(string name, System.Data.SqlDbType dbType, long maxLength, long locale, System.Data.SqlTypes.SqlCompareOptions compareOptions) { }

        public SqlMetaData(string name, System.Data.SqlDbType dbType, long maxLength) { }

        public SqlMetaData(string name, System.Data.SqlDbType dbType, string database, string owningSchema, string objectName, bool useServerDefault, bool isUniqueKey, System.Data.SqlClient.SortOrder columnSortOrder, int sortOrdinal) { }

        public SqlMetaData(string name, System.Data.SqlDbType dbType, string database, string owningSchema, string objectName) { }

        public SqlMetaData(string name, System.Data.SqlDbType dbType, System.Type userDefinedType, string serverTypeName, bool useServerDefault, bool isUniqueKey, System.Data.SqlClient.SortOrder columnSortOrder, int sortOrdinal) { }

        public SqlMetaData(string name, System.Data.SqlDbType dbType, System.Type userDefinedType, string serverTypeName) { }

        public SqlMetaData(string name, System.Data.SqlDbType dbType, System.Type userDefinedType) { }

        public SqlMetaData(string name, System.Data.SqlDbType dbType) { }

        public System.Data.SqlTypes.SqlCompareOptions CompareOptions { get { throw null; } }

        public System.Data.DbType DbType { get { throw null; } }

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

        public System.Type Type { get { throw null; } }

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

        public decimal Adjust(decimal value) { throw null; }

        public double Adjust(double value) { throw null; }

        public System.Guid Adjust(System.Guid value) { throw null; }

        public short Adjust(short value) { throw null; }

        public int Adjust(int value) { throw null; }

        public long Adjust(long value) { throw null; }

        public object Adjust(object value) { throw null; }

        public float Adjust(float value) { throw null; }

        public string Adjust(string value) { throw null; }

        public System.TimeSpan Adjust(System.TimeSpan value) { throw null; }

        public static SqlMetaData InferFromValue(object value, string name) { throw null; }
    }

    [System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed partial class SqlMethodAttribute : SqlFunctionAttribute
    {
        public SqlMethodAttribute() { }

        public bool InvokeIfReceiverIsNull { get { throw null; } set { } }

        public bool IsMutator { get { throw null; } set { } }

        public bool OnNullCall { get { throw null; } set { } }
    }

    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public sealed partial class SqlUserDefinedAggregateAttribute : System.Attribute
    {
        public const int MaxByteSizeValue = 8000;
        public SqlUserDefinedAggregateAttribute(Format format) { }

        public Format Format { get { throw null; } }

        public bool IsInvariantToDuplicates { get { throw null; } set { } }

        public bool IsInvariantToNulls { get { throw null; } set { } }

        public bool IsInvariantToOrder { get { throw null; } set { } }

        public bool IsNullIfEmpty { get { throw null; } set { } }

        public int MaxByteSize { get { throw null; } set { } }

        public string Name { get { throw null; } set { } }
    }

    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
    public sealed partial class SqlUserDefinedTypeAttribute : System.Attribute
    {
        public SqlUserDefinedTypeAttribute(Format format) { }

        public Format Format { get { throw null; } }

        public bool IsByteOrdered { get { throw null; } set { } }

        public bool IsFixedLength { get { throw null; } set { } }

        public int MaxByteSize { get { throw null; } set { } }

        public string Name { get { throw null; } set { } }

        public string ValidationMethodName { get { throw null; } set { } }
    }

    public enum SystemDataAccessKind
    {
        None = 0,
        Read = 1
    }
}

namespace System.Data
{
    public sealed partial class OperationAbortedException : SystemException
    {
    }
}

namespace System.Data.Sql
{
    public sealed partial class SqlNotificationRequest
    {
        public SqlNotificationRequest() { }

        public SqlNotificationRequest(string userData, string options, int timeout) { }

        public string Options { get { throw null; } set { } }

        public int Timeout { get { throw null; } set { } }

        public string UserData { get { throw null; } set { } }
    }
}

namespace System.Data.SqlClient
{
    public enum ApplicationIntent
    {
        ReadWrite = 0,
        ReadOnly = 1
    }

    public delegate void OnChangeEventHandler(object sender, SqlNotificationEventArgs e);
    public enum PoolBlockingPeriod
    {
        Auto = 0,
        AlwaysBlock = 1,
        NeverBlock = 2
    }

    public enum SortOrder
    {
        Unspecified = -1,
        Ascending = 0,
        Descending = 1
    }

    public sealed partial class SqlBulkCopy : IDisposable
    {
        public SqlBulkCopy(SqlConnection connection, SqlBulkCopyOptions copyOptions, SqlTransaction externalTransaction) { }

        public SqlBulkCopy(SqlConnection connection) { }

        public SqlBulkCopy(string connectionString, SqlBulkCopyOptions copyOptions) { }

        public SqlBulkCopy(string connectionString) { }

        public int BatchSize { get { throw null; } set { } }

        public int BulkCopyTimeout { get { throw null; } set { } }

        public SqlBulkCopyColumnMappingCollection ColumnMappings { get { throw null; } }

        public string DestinationTableName { get { throw null; } set { } }

        public bool EnableStreaming { get { throw null; } set { } }

        public int NotifyAfter { get { throw null; } set { } }

        public event SqlRowsCopiedEventHandler SqlRowsCopied { add { } remove { } }

        public void Close() { }

        void IDisposable.Dispose() { }

        public void WriteToServer(Common.DbDataReader reader) { }

        public void WriteToServer(DataRow[] rows) { }

        public void WriteToServer(DataTable table, DataRowState rowState) { }

        public void WriteToServer(DataTable table) { }

        public void WriteToServer(IDataReader reader) { }

        public Threading.Tasks.Task WriteToServerAsync(Common.DbDataReader reader, Threading.CancellationToken cancellationToken) { throw null; }

        public Threading.Tasks.Task WriteToServerAsync(Common.DbDataReader reader) { throw null; }

        public Threading.Tasks.Task WriteToServerAsync(DataRow[] rows, Threading.CancellationToken cancellationToken) { throw null; }

        public Threading.Tasks.Task WriteToServerAsync(DataRow[] rows) { throw null; }

        public Threading.Tasks.Task WriteToServerAsync(DataTable table, DataRowState rowState, Threading.CancellationToken cancellationToken) { throw null; }

        public Threading.Tasks.Task WriteToServerAsync(DataTable table, DataRowState rowState) { throw null; }

        public Threading.Tasks.Task WriteToServerAsync(DataTable table, Threading.CancellationToken cancellationToken) { throw null; }

        public Threading.Tasks.Task WriteToServerAsync(DataTable table) { throw null; }

        public Threading.Tasks.Task WriteToServerAsync(IDataReader reader, Threading.CancellationToken cancellationToken) { throw null; }

        public Threading.Tasks.Task WriteToServerAsync(IDataReader reader) { throw null; }
    }

    public sealed partial class SqlBulkCopyColumnMapping
    {
        public SqlBulkCopyColumnMapping() { }

        public SqlBulkCopyColumnMapping(int sourceColumnOrdinal, int destinationOrdinal) { }

        public SqlBulkCopyColumnMapping(int sourceColumnOrdinal, string destinationColumn) { }

        public SqlBulkCopyColumnMapping(string sourceColumn, int destinationOrdinal) { }

        public SqlBulkCopyColumnMapping(string sourceColumn, string destinationColumn) { }

        public string DestinationColumn { get { throw null; } set { } }

        public int DestinationOrdinal { get { throw null; } set { } }

        public string SourceColumn { get { throw null; } set { } }

        public int SourceOrdinal { get { throw null; } set { } }
    }

    public sealed partial class SqlBulkCopyColumnMappingCollection : Collections.CollectionBase
    {
        public SqlBulkCopyColumnMapping this[int index] { get { throw null; } }

        public SqlBulkCopyColumnMapping Add(SqlBulkCopyColumnMapping bulkCopyColumnMapping) { throw null; }

        public SqlBulkCopyColumnMapping Add(int sourceColumnIndex, int destinationColumnIndex) { throw null; }

        public SqlBulkCopyColumnMapping Add(int sourceColumnIndex, string destinationColumn) { throw null; }

        public SqlBulkCopyColumnMapping Add(string sourceColumn, int destinationColumnIndex) { throw null; }

        public SqlBulkCopyColumnMapping Add(string sourceColumn, string destinationColumn) { throw null; }

        public new void Clear() { }

        public bool Contains(SqlBulkCopyColumnMapping value) { throw null; }

        public void CopyTo(SqlBulkCopyColumnMapping[] array, int index) { }

        public int IndexOf(SqlBulkCopyColumnMapping value) { throw null; }

        public void Insert(int index, SqlBulkCopyColumnMapping value) { }

        public void Remove(SqlBulkCopyColumnMapping value) { }

        public new void RemoveAt(int index) { }
    }

    [Flags]
    public enum SqlBulkCopyOptions
    {
        Default = 0,
        KeepIdentity = 1,
        CheckConstraints = 2,
        TableLock = 4,
        KeepNulls = 8,
        FireTriggers = 16,
        UseInternalTransaction = 32
    }

    public sealed partial class SqlClientFactory : Common.DbProviderFactory
    {
        public static readonly SqlClientFactory Instance;
        public override Common.DbCommand CreateCommand() { throw null; }

        public override Common.DbCommandBuilder CreateCommandBuilder() { throw null; }

        public override Common.DbConnection CreateConnection() { throw null; }

        public override Common.DbConnectionStringBuilder CreateConnectionStringBuilder() { throw null; }

        public override Common.DbDataAdapter CreateDataAdapter() { throw null; }

        public override Common.DbParameter CreateParameter() { throw null; }
    }

    public static partial class SqlClientMetaDataCollectionNames
    {
        public static readonly string Columns;
        public static readonly string Databases;
        public static readonly string ForeignKeys;
        public static readonly string IndexColumns;
        public static readonly string Indexes;
        public static readonly string Parameters;
        public static readonly string ProcedureColumns;
        public static readonly string Procedures;
        public static readonly string Tables;
        public static readonly string UserDefinedTypes;
        public static readonly string Users;
        public static readonly string ViewColumns;
        public static readonly string Views;
    }

    public sealed partial class SqlCommand : Common.DbCommand, ICloneable
    {
        public SqlCommand() { }

        public SqlCommand(string cmdText, SqlConnection connection, SqlTransaction transaction) { }

        public SqlCommand(string cmdText, SqlConnection connection) { }

        public SqlCommand(string cmdText) { }

        public override string CommandText { get { throw null; } set { } }

        public override int CommandTimeout { get { throw null; } set { } }

        public override CommandType CommandType { get { throw null; } set { } }

        public new SqlConnection Connection { get { throw null; } set { } }

        protected override Common.DbConnection DbConnection { get { throw null; } set { } }

        protected override Common.DbParameterCollection DbParameterCollection { get { throw null; } }

        protected override Common.DbTransaction DbTransaction { get { throw null; } set { } }

        public override bool DesignTimeVisible { get { throw null; } set { } }

        public Sql.SqlNotificationRequest Notification { get { throw null; } set { } }

        public new SqlParameterCollection Parameters { get { throw null; } }

        public new SqlTransaction Transaction { get { throw null; } set { } }

        public override UpdateRowSource UpdatedRowSource { get { throw null; } set { } }

        public event StatementCompletedEventHandler StatementCompleted { add { } remove { } }

        public IAsyncResult BeginExecuteNonQuery() { throw null; }

        public IAsyncResult BeginExecuteNonQuery(AsyncCallback callback, object stateObject) { throw null; }

        public IAsyncResult BeginExecuteReader() { throw null; }

        public IAsyncResult BeginExecuteReader(AsyncCallback callback, object stateObject, CommandBehavior behavior) { throw null; }

        public IAsyncResult BeginExecuteReader(AsyncCallback callback, object stateObject) { throw null; }

        public IAsyncResult BeginExecuteReader(CommandBehavior behavior) { throw null; }

        public IAsyncResult BeginExecuteXmlReader() { throw null; }

        public IAsyncResult BeginExecuteXmlReader(AsyncCallback callback, object stateObject) { throw null; }

        public override void Cancel() { }

        public SqlCommand Clone() { throw null; }

        protected override Common.DbParameter CreateDbParameter() { throw null; }

        public new SqlParameter CreateParameter() { throw null; }

        protected override void Dispose(bool disposing) { }

        public int EndExecuteNonQuery(IAsyncResult asyncResult) { throw null; }

        public SqlDataReader EndExecuteReader(IAsyncResult asyncResult) { throw null; }

        public Xml.XmlReader EndExecuteXmlReader(IAsyncResult asyncResult) { throw null; }

        protected override Common.DbDataReader ExecuteDbDataReader(CommandBehavior behavior) { throw null; }

        protected override Threading.Tasks.Task<Common.DbDataReader> ExecuteDbDataReaderAsync(CommandBehavior behavior, Threading.CancellationToken cancellationToken) { throw null; }

        public override int ExecuteNonQuery() { throw null; }

        public override Threading.Tasks.Task<int> ExecuteNonQueryAsync(Threading.CancellationToken cancellationToken) { throw null; }

        public new SqlDataReader ExecuteReader() { throw null; }

        public new SqlDataReader ExecuteReader(CommandBehavior behavior) { throw null; }

        public new Threading.Tasks.Task<SqlDataReader> ExecuteReaderAsync() { throw null; }

        public new Threading.Tasks.Task<SqlDataReader> ExecuteReaderAsync(CommandBehavior behavior, Threading.CancellationToken cancellationToken) { throw null; }

        public new Threading.Tasks.Task<SqlDataReader> ExecuteReaderAsync(CommandBehavior behavior) { throw null; }

        public new Threading.Tasks.Task<SqlDataReader> ExecuteReaderAsync(Threading.CancellationToken cancellationToken) { throw null; }

        public override object ExecuteScalar() { throw null; }

        public override Threading.Tasks.Task<object> ExecuteScalarAsync(Threading.CancellationToken cancellationToken) { throw null; }

        public Xml.XmlReader ExecuteXmlReader() { throw null; }

        public Threading.Tasks.Task<Xml.XmlReader> ExecuteXmlReaderAsync() { throw null; }

        public Threading.Tasks.Task<Xml.XmlReader> ExecuteXmlReaderAsync(Threading.CancellationToken cancellationToken) { throw null; }

        public override void Prepare() { }

        public void ResetCommandTimeout() { }

        object ICloneable.Clone() { throw null; }
    }

    public sealed partial class SqlCommandBuilder : Common.DbCommandBuilder
    {
        public SqlCommandBuilder() { }

        public SqlCommandBuilder(SqlDataAdapter adapter) { }

        public override Common.CatalogLocation CatalogLocation { get { throw null; } set { } }

        public override string CatalogSeparator { get { throw null; } set { } }

        public new SqlDataAdapter DataAdapter { get { throw null; } set { } }

        public override string QuotePrefix { get { throw null; } set { } }

        public override string QuoteSuffix { get { throw null; } set { } }

        public override string SchemaSeparator { get { throw null; } set { } }

        protected override void ApplyParameterInfo(Common.DbParameter parameter, DataRow datarow, StatementType statementType, bool whereClause) { }

        public static void DeriveParameters(SqlCommand command) { }

        public new SqlCommand GetDeleteCommand() { throw null; }

        public new SqlCommand GetDeleteCommand(bool useColumnsForParameterNames) { throw null; }

        public new SqlCommand GetInsertCommand() { throw null; }

        public new SqlCommand GetInsertCommand(bool useColumnsForParameterNames) { throw null; }

        protected override string GetParameterName(int parameterOrdinal) { throw null; }

        protected override string GetParameterName(string parameterName) { throw null; }

        protected override string GetParameterPlaceholder(int parameterOrdinal) { throw null; }

        protected override DataTable GetSchemaTable(Common.DbCommand srcCommand) { throw null; }

        public new SqlCommand GetUpdateCommand() { throw null; }

        public new SqlCommand GetUpdateCommand(bool useColumnsForParameterNames) { throw null; }

        protected override Common.DbCommand InitializeCommand(Common.DbCommand command) { throw null; }

        public override string QuoteIdentifier(string unquotedIdentifier) { throw null; }

        protected override void SetRowUpdatingHandler(Common.DbDataAdapter adapter) { }

        public override string UnquoteIdentifier(string quotedIdentifier) { throw null; }
    }

    public sealed partial class SqlConnection : Common.DbConnection, ICloneable
    {
        public SqlConnection() { }

        public SqlConnection(string connectionString, SqlCredential credential) { }

        public SqlConnection(string connectionString) { }

        public string AccessToken { get { throw null; } set { } }

        public Guid ClientConnectionId { get { throw null; } }

        public override string ConnectionString { get { throw null; } set { } }

        public override int ConnectionTimeout { get { throw null; } }

        public SqlCredential Credential { get { throw null; } set { } }

        public override string Database { get { throw null; } }

        public override string DataSource { get { throw null; } }

        public bool FireInfoMessageEventOnUserErrors { get { throw null; } set { } }

        public int PacketSize { get { throw null; } }

        public override string ServerVersion { get { throw null; } }

        public override ConnectionState State { get { throw null; } }

        public bool StatisticsEnabled { get { throw null; } set { } }

        public string WorkstationId { get { throw null; } }

        public event SqlInfoMessageEventHandler InfoMessage { add { } remove { } }

        protected override Common.DbTransaction BeginDbTransaction(IsolationLevel isolationLevel) { throw null; }

        public new SqlTransaction BeginTransaction() { throw null; }

        public SqlTransaction BeginTransaction(IsolationLevel iso, string transactionName) { throw null; }

        public new SqlTransaction BeginTransaction(IsolationLevel iso) { throw null; }

        public SqlTransaction BeginTransaction(string transactionName) { throw null; }

        public override void ChangeDatabase(string database) { }

        public static void ChangePassword(string connectionString, SqlCredential credential, Security.SecureString newPassword) { }

        public static void ChangePassword(string connectionString, string newPassword) { }

        public static void ClearAllPools() { }

        public static void ClearPool(SqlConnection connection) { }

        public override void Close() { }

        public new SqlCommand CreateCommand() { throw null; }

        protected override Common.DbCommand CreateDbCommand() { throw null; }

        protected override void Dispose(bool disposing) { }

        public override DataTable GetSchema() { throw null; }

        public override DataTable GetSchema(string collectionName, string[] restrictionValues) { throw null; }

        public override DataTable GetSchema(string collectionName) { throw null; }

        public override void Open() { }

        public override Threading.Tasks.Task OpenAsync(Threading.CancellationToken cancellationToken) { throw null; }

        public void ResetStatistics() { }

        public Collections.IDictionary RetrieveStatistics() { throw null; }

        object ICloneable.Clone() { throw null; }
    }

    public sealed partial class SqlConnectionStringBuilder : Common.DbConnectionStringBuilder
    {
        public SqlConnectionStringBuilder() { }

        public SqlConnectionStringBuilder(string connectionString) { }

        public ApplicationIntent ApplicationIntent { get { throw null; } set { } }

        public string ApplicationName { get { throw null; } set { } }

        public string AttachDBFilename { get { throw null; } set { } }

        public int ConnectRetryCount { get { throw null; } set { } }

        public int ConnectRetryInterval { get { throw null; } set { } }

        public int ConnectTimeout { get { throw null; } set { } }

        public string CurrentLanguage { get { throw null; } set { } }

        public string DataSource { get { throw null; } set { } }

        public bool Encrypt { get { throw null; } set { } }

        public bool Enlist { get { throw null; } set { } }

        public string FailoverPartner { get { throw null; } set { } }

        public string InitialCatalog { get { throw null; } set { } }

        public bool IntegratedSecurity { get { throw null; } set { } }

        public override object this[string keyword] { get { throw null; } set { } }

        public override Collections.ICollection Keys { get { throw null; } }

        public int LoadBalanceTimeout { get { throw null; } set { } }

        public int MaxPoolSize { get { throw null; } set { } }

        public int MinPoolSize { get { throw null; } set { } }

        public bool MultipleActiveResultSets { get { throw null; } set { } }

        public bool MultiSubnetFailover { get { throw null; } set { } }

        public int PacketSize { get { throw null; } set { } }

        public string Password { get { throw null; } set { } }

        public bool PersistSecurityInfo { get { throw null; } set { } }

        public PoolBlockingPeriod PoolBlockingPeriod { get { throw null; } set { } }

        public bool Pooling { get { throw null; } set { } }

        public bool Replication { get { throw null; } set { } }

        public string TransactionBinding { get { throw null; } set { } }

        public bool TrustServerCertificate { get { throw null; } set { } }

        public string TypeSystemVersion { get { throw null; } set { } }

        public string UserID { get { throw null; } set { } }

        public bool UserInstance { get { throw null; } set { } }

        public override Collections.ICollection Values { get { throw null; } }

        public string WorkstationID { get { throw null; } set { } }

        public override void Clear() { }

        public override bool ContainsKey(string keyword) { throw null; }

        public override bool Remove(string keyword) { throw null; }

        public override bool ShouldSerialize(string keyword) { throw null; }

        public override bool TryGetValue(string keyword, out object value) { throw null; }
    }

    public sealed partial class SqlCredential
    {
        public SqlCredential(string userId, Security.SecureString password) { }

        public Security.SecureString Password { get { throw null; } }

        public string UserId { get { throw null; } }
    }

    public sealed partial class SqlDataAdapter : Common.DbDataAdapter, IDataAdapter, IDbDataAdapter, ICloneable
    {
        public SqlDataAdapter() { }

        public SqlDataAdapter(SqlCommand selectCommand) { }

        public SqlDataAdapter(string selectCommandText, SqlConnection selectConnection) { }

        public SqlDataAdapter(string selectCommandText, string selectConnectionString) { }

        public new SqlCommand DeleteCommand { get { throw null; } set { } }

        public new SqlCommand InsertCommand { get { throw null; } set { } }

        public new SqlCommand SelectCommand { get { throw null; } set { } }

        IDbCommand IDbDataAdapter.DeleteCommand { get { throw null; } set { } }

        IDbCommand IDbDataAdapter.InsertCommand { get { throw null; } set { } }

        IDbCommand IDbDataAdapter.SelectCommand { get { throw null; } set { } }

        IDbCommand IDbDataAdapter.UpdateCommand { get { throw null; } set { } }

        public override int UpdateBatchSize { get { throw null; } set { } }

        public new SqlCommand UpdateCommand { get { throw null; } set { } }

        public event SqlRowUpdatedEventHandler RowUpdated { add { } remove { } }

        public event SqlRowUpdatingEventHandler RowUpdating { add { } remove { } }

        protected override void OnRowUpdated(Common.RowUpdatedEventArgs value) { }

        protected override void OnRowUpdating(Common.RowUpdatingEventArgs value) { }

        object ICloneable.Clone() { throw null; }
    }

    public partial class SqlDataReader : Common.DbDataReader, IDisposable, Common.IDbColumnSchemaGenerator
    {
        protected SqlConnection Connection { get { throw null; } }

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

        public Collections.ObjectModel.ReadOnlyCollection<Common.DbColumn> GetColumnSchema() { throw null; }

        public override string GetDataTypeName(int i) { throw null; }

        public override DateTime GetDateTime(int i) { throw null; }

        public virtual DateTimeOffset GetDateTimeOffset(int i) { throw null; }

        public override decimal GetDecimal(int i) { throw null; }

        public override double GetDouble(int i) { throw null; }

        public override Collections.IEnumerator GetEnumerator() { throw null; }

        public override Type GetFieldType(int i) { throw null; }

        public override T GetFieldValue<T>(int i) { throw null; }

        public override Threading.Tasks.Task<T> GetFieldValueAsync<T>(int i, Threading.CancellationToken cancellationToken) { throw null; }

        public override float GetFloat(int i) { throw null; }

        public override Guid GetGuid(int i) { throw null; }

        public override short GetInt16(int i) { throw null; }

        public override int GetInt32(int i) { throw null; }

        public override long GetInt64(int i) { throw null; }

        public override string GetName(int i) { throw null; }

        public override int GetOrdinal(string name) { throw null; }

        public override Type GetProviderSpecificFieldType(int i) { throw null; }

        public override object GetProviderSpecificValue(int i) { throw null; }

        public override int GetProviderSpecificValues(object[] values) { throw null; }

        public override DataTable GetSchemaTable() { throw null; }

        public virtual SqlTypes.SqlBinary GetSqlBinary(int i) { throw null; }

        public virtual SqlTypes.SqlBoolean GetSqlBoolean(int i) { throw null; }

        public virtual SqlTypes.SqlByte GetSqlByte(int i) { throw null; }

        public virtual SqlTypes.SqlBytes GetSqlBytes(int i) { throw null; }

        public virtual SqlTypes.SqlChars GetSqlChars(int i) { throw null; }

        public virtual SqlTypes.SqlDateTime GetSqlDateTime(int i) { throw null; }

        public virtual SqlTypes.SqlDecimal GetSqlDecimal(int i) { throw null; }

        public virtual SqlTypes.SqlDouble GetSqlDouble(int i) { throw null; }

        public virtual SqlTypes.SqlGuid GetSqlGuid(int i) { throw null; }

        public virtual SqlTypes.SqlInt16 GetSqlInt16(int i) { throw null; }

        public virtual SqlTypes.SqlInt32 GetSqlInt32(int i) { throw null; }

        public virtual SqlTypes.SqlInt64 GetSqlInt64(int i) { throw null; }

        public virtual SqlTypes.SqlMoney GetSqlMoney(int i) { throw null; }

        public virtual SqlTypes.SqlSingle GetSqlSingle(int i) { throw null; }

        public virtual SqlTypes.SqlString GetSqlString(int i) { throw null; }

        public virtual object GetSqlValue(int i) { throw null; }

        public virtual int GetSqlValues(object[] values) { throw null; }

        public virtual SqlTypes.SqlXml GetSqlXml(int i) { throw null; }

        public override IO.Stream GetStream(int i) { throw null; }

        public override string GetString(int i) { throw null; }

        public override IO.TextReader GetTextReader(int i) { throw null; }

        public virtual TimeSpan GetTimeSpan(int i) { throw null; }

        public override object GetValue(int i) { throw null; }

        public override int GetValues(object[] values) { throw null; }

        public virtual Xml.XmlReader GetXmlReader(int i) { throw null; }

        protected internal bool IsCommandBehavior(CommandBehavior condition) { throw null; }

        public override bool IsDBNull(int i) { throw null; }

        public override Threading.Tasks.Task<bool> IsDBNullAsync(int i, Threading.CancellationToken cancellationToken) { throw null; }

        public override bool NextResult() { throw null; }

        public override Threading.Tasks.Task<bool> NextResultAsync(Threading.CancellationToken cancellationToken) { throw null; }

        public override bool Read() { throw null; }

        public override Threading.Tasks.Task<bool> ReadAsync(Threading.CancellationToken cancellationToken) { throw null; }
    }

    public sealed partial class SqlDependency
    {
        public SqlDependency() { }

        public SqlDependency(SqlCommand command, string options, int timeout) { }

        public SqlDependency(SqlCommand command) { }

        public bool HasChanges { get { throw null; } }

        public string Id { get { throw null; } }

        public event OnChangeEventHandler OnChange { add { } remove { } }

        public void AddCommandDependency(SqlCommand command) { }

        public static bool Start(string connectionString, string queue) { throw null; }

        public static bool Start(string connectionString) { throw null; }

        public static bool Stop(string connectionString, string queue) { throw null; }

        public static bool Stop(string connectionString) { throw null; }
    }

    public sealed partial class SqlError
    {
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

    public sealed partial class SqlErrorCollection : Collections.ICollection, Collections.IEnumerable
    {
        public int Count { get { throw null; } }

        public SqlError this[int index] { get { throw null; } }

        bool Collections.ICollection.IsSynchronized { get { throw null; } }

        object Collections.ICollection.SyncRoot { get { throw null; } }

        public void CopyTo(Array array, int index) { }

        public void CopyTo(SqlError[] array, int index) { }

        public Collections.IEnumerator GetEnumerator() { throw null; }
    }

    public sealed partial class SqlException : Common.DbException
    {
        public byte Class { get { throw null; } }

        public Guid ClientConnectionId { get { throw null; } }

        public SqlErrorCollection Errors { get { throw null; } }

        public int LineNumber { get { throw null; } }

        public int Number { get { throw null; } }

        public string Procedure { get { throw null; } }

        public string Server { get { throw null; } }

        public override string Source { get { throw null; } }

        public byte State { get { throw null; } }

        public override void GetObjectData(Runtime.Serialization.SerializationInfo si, Runtime.Serialization.StreamingContext context) { }

        public override string ToString() { throw null; }
    }

    public sealed partial class SqlInfoMessageEventArgs : EventArgs
    {
        public SqlErrorCollection Errors { get { throw null; } }

        public string Message { get { throw null; } }

        public string Source { get { throw null; } }

        public override string ToString() { throw null; }
    }

    public delegate void SqlInfoMessageEventHandler(object sender, SqlInfoMessageEventArgs e);
    public partial class SqlNotificationEventArgs : EventArgs
    {
        public SqlNotificationEventArgs(SqlNotificationType type, SqlNotificationInfo info, SqlNotificationSource source) { }

        public SqlNotificationInfo Info { get { throw null; } }

        public SqlNotificationSource Source { get { throw null; } }

        public SqlNotificationType Type { get { throw null; } }
    }

    public enum SqlNotificationInfo
    {
        AlreadyChanged = -2,
        Unknown = -1,
        Truncate = 0,
        Insert = 1,
        Update = 2,
        Delete = 3,
        Drop = 4,
        Alter = 5,
        Restart = 6,
        Error = 7,
        Query = 8,
        Invalid = 9,
        Options = 10,
        Isolation = 11,
        Expired = 12,
        Resource = 13,
        PreviousFire = 14,
        TemplateLimit = 15,
        Merge = 16
    }

    public enum SqlNotificationSource
    {
        Client = -2,
        Unknown = -1,
        Data = 0,
        Timeout = 1,
        Object = 2,
        Database = 3,
        System = 4,
        Statement = 5,
        Environment = 6,
        Execution = 7,
        Owner = 8
    }

    public enum SqlNotificationType
    {
        Unknown = -1,
        Change = 0,
        Subscribe = 1
    }

    public sealed partial class SqlParameter : Common.DbParameter, ICloneable
    {
        public SqlParameter() { }

        public SqlParameter(string parameterName, SqlDbType dbType, int size, ParameterDirection direction, bool isNullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value) { }

        public SqlParameter(string parameterName, SqlDbType dbType, int size, ParameterDirection direction, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, bool sourceColumnNullMapping, object value, string xmlSchemaCollectionDatabase, string xmlSchemaCollectionOwningSchema, string xmlSchemaCollectionName) { }

        public SqlParameter(string parameterName, SqlDbType dbType, int size, string sourceColumn) { }

        public SqlParameter(string parameterName, SqlDbType dbType, int size) { }

        public SqlParameter(string parameterName, SqlDbType dbType) { }

        public SqlParameter(string parameterName, object value) { }

        public SqlTypes.SqlCompareOptions CompareInfo { get { throw null; } set { } }

        public override DbType DbType { get { throw null; } set { } }

        public override ParameterDirection Direction { get { throw null; } set { } }

        public override bool IsNullable { get { throw null; } set { } }

        public int LocaleId { get { throw null; } set { } }

        public int Offset { get { throw null; } set { } }

        public override string ParameterName { get { throw null; } set { } }

        public new byte Precision { get { throw null; } set { } }

        public new byte Scale { get { throw null; } set { } }

        public override int Size { get { throw null; } set { } }

        public override string SourceColumn { get { throw null; } set { } }

        public override bool SourceColumnNullMapping { get { throw null; } set { } }

        public override DataRowVersion SourceVersion { get { throw null; } set { } }

        public SqlDbType SqlDbType { get { throw null; } set { } }

        public object SqlValue { get { throw null; } set { } }

        public string TypeName { get { throw null; } set { } }

        public string UdtTypeName { get { throw null; } set { } }

        public override object Value { get { throw null; } set { } }

        public string XmlSchemaCollectionDatabase { get { throw null; } set { } }

        public string XmlSchemaCollectionName { get { throw null; } set { } }

        public string XmlSchemaCollectionOwningSchema { get { throw null; } set { } }

        public override void ResetDbType() { }

        public void ResetSqlDbType() { }

        object ICloneable.Clone() { throw null; }

        public override string ToString() { throw null; }
    }

    public sealed partial class SqlParameterCollection : Common.DbParameterCollection
    {
        public override int Count { get { throw null; } }

        public override bool IsFixedSize { get { throw null; } }

        public override bool IsReadOnly { get { throw null; } }

        public new SqlParameter this[int index] { get { throw null; } set { } }

        public new SqlParameter this[string parameterName] { get { throw null; } set { } }

        public override object SyncRoot { get { throw null; } }

        public SqlParameter Add(SqlParameter value) { throw null; }

        public override int Add(object value) { throw null; }

        public SqlParameter Add(string parameterName, SqlDbType sqlDbType, int size, string sourceColumn) { throw null; }

        public SqlParameter Add(string parameterName, SqlDbType sqlDbType, int size) { throw null; }

        public SqlParameter Add(string parameterName, SqlDbType sqlDbType) { throw null; }

        public override void AddRange(Array values) { }

        public void AddRange(SqlParameter[] values) { }

        public SqlParameter AddWithValue(string parameterName, object value) { throw null; }

        public override void Clear() { }

        public bool Contains(SqlParameter value) { throw null; }

        public override bool Contains(object value) { throw null; }

        public override bool Contains(string value) { throw null; }

        public override void CopyTo(Array array, int index) { }

        public void CopyTo(SqlParameter[] array, int index) { }

        public override Collections.IEnumerator GetEnumerator() { throw null; }

        protected override Common.DbParameter GetParameter(int index) { throw null; }

        protected override Common.DbParameter GetParameter(string parameterName) { throw null; }

        public int IndexOf(SqlParameter value) { throw null; }

        public override int IndexOf(object value) { throw null; }

        public override int IndexOf(string parameterName) { throw null; }

        public void Insert(int index, SqlParameter value) { }

        public override void Insert(int index, object value) { }

        public void Remove(SqlParameter value) { }

        public override void Remove(object value) { }

        public override void RemoveAt(int index) { }

        public override void RemoveAt(string parameterName) { }

        protected override void SetParameter(int index, Common.DbParameter value) { }

        protected override void SetParameter(string parameterName, Common.DbParameter value) { }
    }

    public partial class SqlRowsCopiedEventArgs : EventArgs
    {
        public SqlRowsCopiedEventArgs(long rowsCopied) { }

        public bool Abort { get { throw null; } set { } }

        public long RowsCopied { get { throw null; } }
    }

    public delegate void SqlRowsCopiedEventHandler(object sender, SqlRowsCopiedEventArgs e);
    public sealed partial class SqlRowUpdatedEventArgs : Common.RowUpdatedEventArgs
    {
        public SqlRowUpdatedEventArgs(DataRow row, IDbCommand command, StatementType statementType, Common.DataTableMapping tableMapping) : base(default!, default!, default, default!) { }

        public new SqlCommand Command { get { throw null; } }
    }

    public delegate void SqlRowUpdatedEventHandler(object sender, SqlRowUpdatedEventArgs e);
    public sealed partial class SqlRowUpdatingEventArgs : Common.RowUpdatingEventArgs
    {
        public SqlRowUpdatingEventArgs(DataRow row, IDbCommand command, StatementType statementType, Common.DataTableMapping tableMapping) : base(default!, default!, default, default!) { }

        protected override IDbCommand BaseCommand { get { throw null; } set { } }

        public new SqlCommand Command { get { throw null; } set { } }
    }

    public delegate void SqlRowUpdatingEventHandler(object sender, SqlRowUpdatingEventArgs e);
    public sealed partial class SqlTransaction : Common.DbTransaction
    {
        public new SqlConnection Connection { get { throw null; } }

        protected override Common.DbConnection DbConnection { get { throw null; } }

        public override IsolationLevel IsolationLevel { get { throw null; } }

        public override void Commit() { }

        protected override void Dispose(bool disposing) { }

        public override void Rollback() { }

        public void Rollback(string transactionName) { }

        public void Save(string savePointName) { }
    }
}

namespace System.Data.SqlTypes
{
    public sealed partial class SqlFileStream : IO.Stream
    {
        public SqlFileStream(string path, byte[] transactionContext, IO.FileAccess access, IO.FileOptions options, long allocationSize) { }

        public SqlFileStream(string path, byte[] transactionContext, IO.FileAccess access) { }

        public override bool CanRead { get { throw null; } }

        public override bool CanSeek { get { throw null; } }

        public override bool CanWrite { get { throw null; } }

        public override long Length { get { throw null; } }

        public string Name { get { throw null; } }

        public override long Position { get { throw null; } set { } }

        public byte[] TransactionContext { get { throw null; } }

        public override void Flush() { }

        public override int Read(byte[] buffer, int offset, int count) { throw null; }

        public override long Seek(long offset, IO.SeekOrigin origin) { throw null; }

        public override void SetLength(long value) { }

        public override void Write(byte[] buffer, int offset, int count) { }
    }
}
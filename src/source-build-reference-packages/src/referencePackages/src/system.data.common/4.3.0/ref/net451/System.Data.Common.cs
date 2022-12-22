// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------

using System;
using System.Data;
using System.Data.Common;
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
[assembly: AssemblyFileVersion("4.6.24705.01")]
[assembly: AssemblyInformationalVersion("4.6.24705.01 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("4.1.1.0")]

[assembly: TypeForwardedTo(typeof(System.Data.CommandBehavior))]
[assembly: TypeForwardedTo(typeof(System.Data.CommandType))]
[assembly: TypeForwardedTo(typeof(System.Data.Common.DbCommand))]
[assembly: TypeForwardedTo(typeof(System.Data.Common.DbConnection))]
[assembly: TypeForwardedTo(typeof(System.Data.Common.DbConnectionStringBuilder))]
[assembly: TypeForwardedTo(typeof(System.Data.Common.DbDataReader))]
[assembly: TypeForwardedTo(typeof(System.Data.Common.DbDataRecord))]
[assembly: TypeForwardedTo(typeof(System.Data.Common.DbEnumerator))]
[assembly: TypeForwardedTo(typeof(System.Data.Common.DbException))]
[assembly: TypeForwardedTo(typeof(System.Data.Common.DbParameter))]
[assembly: TypeForwardedTo(typeof(System.Data.Common.DbParameterCollection))]
[assembly: TypeForwardedTo(typeof(System.Data.Common.DbProviderFactory))]
[assembly: TypeForwardedTo(typeof(System.Data.Common.DbTransaction))]
[assembly: TypeForwardedTo(typeof(System.Data.ConnectionState))]
[assembly: TypeForwardedTo(typeof(System.Data.DataRowVersion))]
[assembly: TypeForwardedTo(typeof(System.Data.DataTable))]
[assembly: TypeForwardedTo(typeof(System.Data.DbType))]
[assembly: TypeForwardedTo(typeof(System.Data.IDataParameter))]
[assembly: TypeForwardedTo(typeof(System.Data.IDataParameterCollection))]
[assembly: TypeForwardedTo(typeof(System.Data.IDataReader))]
[assembly: TypeForwardedTo(typeof(System.Data.IDataRecord))]
[assembly: TypeForwardedTo(typeof(System.Data.IDbCommand))]
[assembly: TypeForwardedTo(typeof(System.Data.IDbConnection))]
[assembly: TypeForwardedTo(typeof(System.Data.IDbDataParameter))]
[assembly: TypeForwardedTo(typeof(System.Data.IDbTransaction))]
[assembly: TypeForwardedTo(typeof(System.Data.IsolationLevel))]
[assembly: TypeForwardedTo(typeof(System.Data.ParameterDirection))]
[assembly: TypeForwardedTo(typeof(System.Data.StateChangeEventArgs))]
[assembly: TypeForwardedTo(typeof(System.Data.StateChangeEventHandler))]
[assembly: TypeForwardedTo(typeof(System.Data.UpdateRowSource))]
[assembly: TypeForwardedTo(typeof(System.DBNull))]



namespace System.Data.Common
{
    public abstract partial class DbColumn
    {
        protected DbColumn() { }
        public System.Nullable<bool> AllowDBNull { get { throw null; } protected set { } }
        public string BaseCatalogName { get { throw null; } protected set { } }
        public string BaseColumnName { get { throw null; } protected set { } }
        public string BaseSchemaName { get { throw null; } protected set { } }
        public string BaseServerName { get { throw null; } protected set { } }
        public string BaseTableName { get { throw null; } protected set { } }
        public string ColumnName { get { throw null; } protected set { } }
        public System.Nullable<int> ColumnOrdinal { get { throw null; } protected set { } }
        public System.Nullable<int> ColumnSize { get { throw null; } protected set { } }
        public System.Type DataType { get { throw null; } protected set { } }
        public string DataTypeName { get { throw null; } protected set { } }
        public System.Nullable<bool> IsAliased { get { throw null; } protected set { } }
        public System.Nullable<bool> IsAutoIncrement { get { throw null; } protected set { } }
        public System.Nullable<bool> IsExpression { get { throw null; } protected set { } }
        public System.Nullable<bool> IsHidden { get { throw null; } protected set { } }
        public System.Nullable<bool> IsIdentity { get { throw null; } protected set { } }
        public System.Nullable<bool> IsKey { get { throw null; } protected set { } }
        public System.Nullable<bool> IsLong { get { throw null; } protected set { } }
        public System.Nullable<bool> IsReadOnly { get { throw null; } protected set { } }
        public System.Nullable<bool> IsUnique { get { throw null; } protected set { } }
        public virtual object this[string property] { get { throw null; } }
        public System.Nullable<int> NumericPrecision { get { throw null; } protected set { } }
        public System.Nullable<int> NumericScale { get { throw null; } protected set { } }
        public string UdtAssemblyQualifiedName { get { throw null; } protected set { } }
    }
    public static partial class DbDataReaderExtensions
    {
        public static bool CanGetColumnSchema(this System.Data.Common.DbDataReader reader) { throw null; }
        public static System.Collections.ObjectModel.ReadOnlyCollection<System.Data.Common.DbColumn> GetColumnSchema(this System.Data.Common.DbDataReader reader) { throw null; }
    }
    public partial interface IDbColumnSchemaGenerator
    {
        System.Collections.ObjectModel.ReadOnlyCollection<System.Data.Common.DbColumn> GetColumnSchema();
    }
}

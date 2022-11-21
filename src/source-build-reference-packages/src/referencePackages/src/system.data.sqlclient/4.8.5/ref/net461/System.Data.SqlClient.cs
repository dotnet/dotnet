// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------

using Microsoft.SqlServer.Server;
using System;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
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
[assembly: AssemblyFileVersion("4.700.22.51706")]
[assembly: AssemblyInformationalVersion("4.700.22.51706 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("4.6.1.5")]

[assembly: TypeForwardedTo(typeof(System.Data.SqlDbType))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlTypes.INullable))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlTypes.SqlBinary))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlTypes.SqlBoolean))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlTypes.SqlByte))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlTypes.SqlBytes))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlTypes.SqlChars))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlTypes.SqlCompareOptions))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlTypes.SqlDateTime))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlTypes.SqlDecimal))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlTypes.SqlDouble))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlTypes.SqlGuid))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlTypes.SqlInt16))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlTypes.SqlInt32))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlTypes.SqlInt64))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlTypes.SqlMoney))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlTypes.SqlNullValueException))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlTypes.SqlSingle))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlTypes.SqlString))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlTypes.SqlTruncateException))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlTypes.SqlTypeException))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlTypes.SqlXml))]
[assembly: TypeForwardedTo(typeof(System.Data.StatementCompletedEventArgs))]
[assembly: TypeForwardedTo(typeof(System.Data.StatementCompletedEventHandler))]
[assembly: TypeForwardedTo(typeof(System.Data.OperationAbortedException))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlClient.ApplicationIntent))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlClient.OnChangeEventHandler))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlClient.SortOrder))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlClient.SqlBulkCopy))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlClient.SqlBulkCopyColumnMapping))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlClient.SqlBulkCopyColumnMappingCollection))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlClient.SqlBulkCopyOptions))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlClient.SqlClientFactory))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlClient.SqlClientMetaDataCollectionNames))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlClient.SqlCommand))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlClient.SqlCommandBuilder))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlClient.SqlConnection))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlClient.SqlConnectionStringBuilder))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlClient.SqlCredential))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlClient.SqlDataAdapter))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlClient.SqlDataReader))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlClient.SqlDependency))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlClient.SqlError))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlClient.SqlErrorCollection))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlClient.SqlException))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlClient.SqlInfoMessageEventArgs))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlClient.SqlInfoMessageEventHandler))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlClient.SqlNotificationEventArgs))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlClient.SqlNotificationInfo))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlClient.SqlNotificationSource))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlClient.SqlNotificationType))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlClient.SqlParameter))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlClient.SqlParameterCollection))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlClient.SqlRowsCopiedEventArgs))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlClient.SqlRowsCopiedEventHandler))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlClient.SqlRowUpdatedEventArgs))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlClient.SqlRowUpdatedEventHandler))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlClient.SqlRowUpdatingEventArgs))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlClient.SqlRowUpdatingEventHandler))]
[assembly: TypeForwardedTo(typeof(System.Data.SqlClient.SqlTransaction))]
[assembly: TypeForwardedTo(typeof(System.Data.Sql.SqlNotificationRequest))]
[assembly: TypeForwardedTo(typeof(Microsoft.SqlServer.Server.DataAccessKind))]
[assembly: TypeForwardedTo(typeof(Microsoft.SqlServer.Server.Format))]
[assembly: TypeForwardedTo(typeof(Microsoft.SqlServer.Server.IBinarySerialize))]
[assembly: TypeForwardedTo(typeof(Microsoft.SqlServer.Server.InvalidUdtException))]
[assembly: TypeForwardedTo(typeof(Microsoft.SqlServer.Server.SqlDataRecord))]
[assembly: TypeForwardedTo(typeof(Microsoft.SqlServer.Server.SqlFunctionAttribute))]
[assembly: TypeForwardedTo(typeof(Microsoft.SqlServer.Server.SqlMetaData))]
[assembly: TypeForwardedTo(typeof(Microsoft.SqlServer.Server.SqlMethodAttribute))]
[assembly: TypeForwardedTo(typeof(Microsoft.SqlServer.Server.SqlUserDefinedAggregateAttribute))]
[assembly: TypeForwardedTo(typeof(Microsoft.SqlServer.Server.SqlUserDefinedTypeAttribute))]
[assembly: TypeForwardedTo(typeof(Microsoft.SqlServer.Server.SystemDataAccessKind))]




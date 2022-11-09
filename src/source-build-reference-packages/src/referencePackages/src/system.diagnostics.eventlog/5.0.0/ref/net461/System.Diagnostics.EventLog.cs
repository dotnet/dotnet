// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;

[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: AllowPartiallyTrustedCallers]
[assembly: ReferenceAssembly]
[assembly: AssemblyTitle("System.Diagnostics.EventLog")]
[assembly: AssemblyDescription("System.Diagnostics.EventLog")]
[assembly: AssemblyDefaultAlias("System.Diagnostics.EventLog")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("5.0.20.51904")]
[assembly: AssemblyInformationalVersion("5.0.20.51904 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("5.0.0.0")]

[assembly: TypeForwardedTo(typeof(System.Diagnostics.EntryWrittenEventArgs))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.EntryWrittenEventHandler))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.EventInstance))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.EventLog))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.EventLogEntry))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.EventLogEntryCollection))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.EventLogEntryType))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.EventLogTraceListener))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.EventSourceCreationData))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.OverflowAction))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Eventing.Reader.EventBookmark))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Eventing.Reader.EventKeyword))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Eventing.Reader.EventLevel))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Eventing.Reader.EventLogConfiguration))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Eventing.Reader.EventLogException))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Eventing.Reader.EventLogInformation))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Eventing.Reader.EventLogInvalidDataException))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Eventing.Reader.EventLogIsolation))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Eventing.Reader.EventLogLink))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Eventing.Reader.EventLogMode))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Eventing.Reader.EventLogNotFoundException))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Eventing.Reader.EventLogPropertySelector))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Eventing.Reader.EventLogProviderDisabledException))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Eventing.Reader.EventLogQuery))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Eventing.Reader.EventLogReader))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Eventing.Reader.EventLogReadingException))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Eventing.Reader.EventLogRecord))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Eventing.Reader.EventLogSession))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Eventing.Reader.EventLogStatus))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Eventing.Reader.EventLogType))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Eventing.Reader.EventLogWatcher))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Eventing.Reader.EventMetadata))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Eventing.Reader.EventOpcode))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Eventing.Reader.EventProperty))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Eventing.Reader.EventRecord))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Eventing.Reader.EventRecordWrittenEventArgs))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Eventing.Reader.EventTask))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Eventing.Reader.PathType))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Eventing.Reader.ProviderMetadata))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Eventing.Reader.SessionAuthentication))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Eventing.Reader.StandardEventKeywords))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Eventing.Reader.StandardEventLevel))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Eventing.Reader.StandardEventOpcode))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Eventing.Reader.StandardEventTask))]




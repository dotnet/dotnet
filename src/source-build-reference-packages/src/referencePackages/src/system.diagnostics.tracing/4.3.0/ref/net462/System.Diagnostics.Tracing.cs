// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;

[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: AllowPartiallyTrustedCallers]
[assembly: ReferenceAssembly]
[assembly: AssemblyTitle("System.Diagnostics.Tracing")]
[assembly: AssemblyDescription("System.Diagnostics.Tracing")]
[assembly: AssemblyDefaultAlias("System.Diagnostics.Tracing")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("4.6.24705.01")]
[assembly: AssemblyInformationalVersion("4.6.24705.01 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("4.1.1.0")]

[assembly: TypeForwardedTo(typeof(System.Diagnostics.Tracing.EventActivityOptions))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Tracing.EventAttribute))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Tracing.EventChannel))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Tracing.EventCommand))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Tracing.EventCommandEventArgs))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Tracing.EventDataAttribute))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Tracing.EventFieldAttribute))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Tracing.EventFieldFormat))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Tracing.EventFieldTags))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Tracing.EventIgnoreAttribute))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Tracing.EventKeywords))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Tracing.EventLevel))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Tracing.EventListener))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Tracing.EventManifestOptions))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Tracing.EventOpcode))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Tracing.EventSource))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Tracing.EventSourceAttribute))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Tracing.EventSourceException))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Tracing.EventSourceOptions))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Tracing.EventSourceSettings))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Tracing.EventTags))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Tracing.EventTask))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Tracing.EventWrittenEventArgs))]
[assembly: TypeForwardedTo(typeof(System.Diagnostics.Tracing.NonEventAttribute))]



namespace System.Diagnostics.Tracing
{
    public partial class EventCounter
    {
        public EventCounter(string name, System.Diagnostics.Tracing.EventSource eventSource) { }
        public void WriteMetric(float value) { }
    }
}

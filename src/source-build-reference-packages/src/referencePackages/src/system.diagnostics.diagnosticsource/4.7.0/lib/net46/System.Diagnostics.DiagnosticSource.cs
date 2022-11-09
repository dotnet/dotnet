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
[assembly: AssemblyTitle("System.Diagnostics.DiagnosticSource")]
[assembly: AssemblyDescription("System.Diagnostics.DiagnosticSource")]
[assembly: AssemblyDefaultAlias("System.Diagnostics.DiagnosticSource")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("4.700.19.56404")]
[assembly: AssemblyInformationalVersion("4.700.19.56404 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("4.0.5.0")]




namespace System.Diagnostics
{
    public partial class Activity
    {
        public Activity(string operationName) { }
        public System.Diagnostics.ActivityTraceFlags ActivityTraceFlags { get { throw null; } set { } }
        public System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, string>> Baggage { get { throw null; } }
        public static System.Diagnostics.Activity Current { get { throw null; } set { } }
        public static System.Diagnostics.ActivityIdFormat DefaultIdFormat { get { throw null; } set { } }
        public System.TimeSpan Duration { get { throw null; } }
        public static bool ForceDefaultIdFormat { get { throw null; } set { } }
        public string Id { get { throw null; } }
        public System.Diagnostics.ActivityIdFormat IdFormat { get { throw null; } }
        public string OperationName { get { throw null; } }
        public System.Diagnostics.Activity Parent { get { throw null; } }
        public string ParentId { get { throw null; } }
        public System.Diagnostics.ActivitySpanId ParentSpanId { get { throw null; } }
        public bool Recorded { get { throw null; } }
        public string RootId { get { throw null; } }
        public System.Diagnostics.ActivitySpanId SpanId { get { throw null; } }
        public System.DateTime StartTimeUtc { get { throw null; } }
        public System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, string>> Tags { get { throw null; } }
        public System.Diagnostics.ActivityTraceId TraceId { get { throw null; } }
        public string TraceStateString { get { throw null; } set { } }
        public System.Diagnostics.Activity AddBaggage(string key, string value) { throw null; }
        public System.Diagnostics.Activity AddTag(string key, string value) { throw null; }
        public string GetBaggageItem(string key) { throw null; }
        public System.Diagnostics.Activity SetEndTime(System.DateTime endTimeUtc) { throw null; }
        public System.Diagnostics.Activity SetIdFormat(System.Diagnostics.ActivityIdFormat format) { throw null; }
        public System.Diagnostics.Activity SetParentId(System.Diagnostics.ActivityTraceId traceId, System.Diagnostics.ActivitySpanId spanId, System.Diagnostics.ActivityTraceFlags activityTraceFlags = System.Diagnostics.ActivityTraceFlags.None) { throw null; }
        public System.Diagnostics.Activity SetParentId(string parentId) { throw null; }
        public System.Diagnostics.Activity SetStartTime(System.DateTime startTimeUtc) { throw null; }
        public System.Diagnostics.Activity Start() { throw null; }
        public void Stop() { }
    }
    public enum ActivityIdFormat
    {
        Unknown = 0,
        Hierarchical = 1,
        W3C = 2,
    }
    public readonly partial struct ActivitySpanId : System.IEquatable<System.Diagnostics.ActivitySpanId>
    {
        private readonly object _dummy;
        public void CopyTo(System.Span<byte> destination) { }
        public static System.Diagnostics.ActivitySpanId CreateFromBytes(System.ReadOnlySpan<byte> idData) { throw null; }
        public static System.Diagnostics.ActivitySpanId CreateFromString(System.ReadOnlySpan<char> idData) { throw null; }
        public static System.Diagnostics.ActivitySpanId CreateFromUtf8String(System.ReadOnlySpan<byte> idData) { throw null; }
        public static System.Diagnostics.ActivitySpanId CreateRandom() { throw null; }
        public bool Equals(System.Diagnostics.ActivitySpanId spanId) { throw null; }
        public override bool Equals(object obj) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Diagnostics.ActivitySpanId spanId1, System.Diagnostics.ActivitySpanId spandId2) { throw null; }
        public static bool operator !=(System.Diagnostics.ActivitySpanId spanId1, System.Diagnostics.ActivitySpanId spandId2) { throw null; }
        public string ToHexString() { throw null; }
        public override string ToString() { throw null; }
    }
    [System.FlagsAttribute]
    public enum ActivityTraceFlags
    {
        None = 0,
        Recorded = 1,
    }
    public readonly partial struct ActivityTraceId : System.IEquatable<System.Diagnostics.ActivityTraceId>
    {
        private readonly object _dummy;
        public void CopyTo(System.Span<byte> destination) { }
        public static System.Diagnostics.ActivityTraceId CreateFromBytes(System.ReadOnlySpan<byte> idData) { throw null; }
        public static System.Diagnostics.ActivityTraceId CreateFromString(System.ReadOnlySpan<char> idData) { throw null; }
        public static System.Diagnostics.ActivityTraceId CreateFromUtf8String(System.ReadOnlySpan<byte> idData) { throw null; }
        public static System.Diagnostics.ActivityTraceId CreateRandom() { throw null; }
        public bool Equals(System.Diagnostics.ActivityTraceId traceId) { throw null; }
        public override bool Equals(object obj) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Diagnostics.ActivityTraceId traceId1, System.Diagnostics.ActivityTraceId traceId2) { throw null; }
        public static bool operator !=(System.Diagnostics.ActivityTraceId traceId1, System.Diagnostics.ActivityTraceId traceId2) { throw null; }
        public string ToHexString() { throw null; }
        public override string ToString() { throw null; }
    }
    public partial class DiagnosticListener : System.Diagnostics.DiagnosticSource, System.IDisposable, System.IObservable<System.Collections.Generic.KeyValuePair<string, object>>
    {
        public DiagnosticListener(string name) { }
        public static System.IObservable<System.Diagnostics.DiagnosticListener> AllListeners { get { throw null; } }
        public string Name { get { throw null; } }
        public virtual void Dispose() { }
        public bool IsEnabled() { throw null; }
        public override bool IsEnabled(string name) { throw null; }
        public override bool IsEnabled(string name, object arg1, object arg2 = null) { throw null; }
        public override void OnActivityExport(System.Diagnostics.Activity activity, object payload) { }
        public override void OnActivityImport(System.Diagnostics.Activity activity, object payload) { }
        public virtual System.IDisposable Subscribe(System.IObserver<System.Collections.Generic.KeyValuePair<string, object>> observer) { throw null; }
        public virtual System.IDisposable Subscribe(System.IObserver<System.Collections.Generic.KeyValuePair<string, object>> observer, System.Func<string, object, object, bool> isEnabled) { throw null; }
        public virtual System.IDisposable Subscribe(System.IObserver<System.Collections.Generic.KeyValuePair<string, object>> observer, System.Func<string, object, object, bool> isEnabled, System.Action<System.Diagnostics.Activity, object> onActivityImport = null, System.Action<System.Diagnostics.Activity, object> onActivityExport = null) { throw null; }
        public virtual System.IDisposable Subscribe(System.IObserver<System.Collections.Generic.KeyValuePair<string, object>> observer, System.Predicate<string> isEnabled) { throw null; }
        public override string ToString() { throw null; }
        public override void Write(string name, object value) { }
    }
    public abstract partial class DiagnosticSource
    {
        protected DiagnosticSource() { }
        public abstract bool IsEnabled(string name);
        public virtual bool IsEnabled(string name, object arg1, object arg2 = null) { throw null; }
        public virtual void OnActivityExport(System.Diagnostics.Activity activity, object payload) { }
        public virtual void OnActivityImport(System.Diagnostics.Activity activity, object payload) { }
        public System.Diagnostics.Activity StartActivity(System.Diagnostics.Activity activity, object args) { throw null; }
        public void StopActivity(System.Diagnostics.Activity activity, object args) { }
        public abstract void Write(string name, object value);
    }
}

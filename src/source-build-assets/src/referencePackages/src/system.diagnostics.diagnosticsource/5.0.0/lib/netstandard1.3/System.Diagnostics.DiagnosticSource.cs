// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v1.3", FrameworkDisplayName = "")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Diagnostics.DiagnosticSource")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("System.Diagnostics.DiagnosticSource")]
[assembly: System.Reflection.AssemblyFileVersion("5.0.20.51904")]
[assembly: System.Reflection.AssemblyInformationalVersion("5.0.0+cf258a14b70ad9069470a108f13765e0e5988f51")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("System.Diagnostics.DiagnosticSource")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "git://github.com/dotnet/runtime")]
[assembly: System.Reflection.AssemblyVersionAttribute("5.0.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Diagnostics
{
    public partial class Activity : IDisposable
    {
        public Activity(string operationName) { }

        public ActivityTraceFlags ActivityTraceFlags { get { throw null; } set { } }

        public Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, string?>> Baggage { get { throw null; } }

        public ActivityContext Context { get { throw null; } }

        public static Activity? Current { get { throw null; } set { } }

        public static ActivityIdFormat DefaultIdFormat { get { throw null; } set { } }

        public string DisplayName { get { throw null; } set { } }

        public TimeSpan Duration { get { throw null; } }

        public Collections.Generic.IEnumerable<ActivityEvent> Events { get { throw null; } }

        public static bool ForceDefaultIdFormat { get { throw null; } set { } }

        public string? Id { get { throw null; } }

        public ActivityIdFormat IdFormat { get { throw null; } }

        public bool IsAllDataRequested { get { throw null; } set { } }

        public ActivityKind Kind { get { throw null; } }

        public Collections.Generic.IEnumerable<ActivityLink> Links { get { throw null; } }

        public string OperationName { get { throw null; } }

        public Activity? Parent { get { throw null; } }

        public string? ParentId { get { throw null; } }

        public ActivitySpanId ParentSpanId { get { throw null; } }

        public bool Recorded { get { throw null; } }

        public string? RootId { get { throw null; } }

        public ActivitySource Source { get { throw null; } }

        public ActivitySpanId SpanId { get { throw null; } }

        public DateTime StartTimeUtc { get { throw null; } }

        public Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, object?>> TagObjects { get { throw null; } }

        public Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, string?>> Tags { get { throw null; } }

        public ActivityTraceId TraceId { get { throw null; } }

        public string? TraceStateString { get { throw null; } set { } }

        public Activity AddBaggage(string key, string? value) { throw null; }

        public Activity AddEvent(ActivityEvent e) { throw null; }

        public Activity AddTag(string key, object? value) { throw null; }

        public Activity AddTag(string key, string? value) { throw null; }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        public string? GetBaggageItem(string key) { throw null; }

        public object? GetCustomProperty(string propertyName) { throw null; }

        public void SetCustomProperty(string propertyName, object? propertyValue) { }

        public Activity SetEndTime(DateTime endTimeUtc) { throw null; }

        public Activity SetIdFormat(ActivityIdFormat format) { throw null; }

        public Activity SetParentId(ActivityTraceId traceId, ActivitySpanId spanId, ActivityTraceFlags activityTraceFlags = ActivityTraceFlags.None) { throw null; }

        public Activity SetParentId(string parentId) { throw null; }

        public Activity SetStartTime(DateTime startTimeUtc) { throw null; }

        public Activity SetTag(string key, object? value) { throw null; }

        public Activity Start() { throw null; }

        public void Stop() { }
    }

    public readonly partial struct ActivityContext : IEquatable<ActivityContext>
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public ActivityContext(ActivityTraceId traceId, ActivitySpanId spanId, ActivityTraceFlags traceFlags, string? traceState = null, bool isRemote = false) { }

        public bool IsRemote { get { throw null; } }

        public ActivitySpanId SpanId { get { throw null; } }

        public ActivityTraceFlags TraceFlags { get { throw null; } }

        public ActivityTraceId TraceId { get { throw null; } }

        public string? TraceState { get { throw null; } }

        public readonly bool Equals(ActivityContext value) { throw null; }

        public override readonly bool Equals(object? obj) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(ActivityContext left, ActivityContext right) { throw null; }

        public static bool operator !=(ActivityContext left, ActivityContext right) { throw null; }

        public static ActivityContext Parse(string traceParent, string? traceState) { throw null; }

        public static bool TryParse(string traceParent, string? traceState, out ActivityContext context) { throw null; }
    }

    public readonly partial struct ActivityCreationOptions<T>
    {
        private readonly T _Parent_k__BackingField;
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public ActivityKind Kind { get { throw null; } }

        public Collections.Generic.IEnumerable<ActivityLink>? Links { get { throw null; } }

        public string Name { get { throw null; } }

        public T Parent { get { throw null; } }

        public ActivityTagsCollection SamplingTags { get { throw null; } }

        public ActivitySource Source { get { throw null; } }

        public Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, object?>>? Tags { get { throw null; } }

        public ActivityTraceId TraceId { get { throw null; } }
    }

    public readonly partial struct ActivityEvent
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public ActivityEvent(string name, DateTimeOffset timestamp = default, ActivityTagsCollection? tags = null) { }

        public ActivityEvent(string name) { }

        public string Name { get { throw null; } }

        public Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, object?>> Tags { get { throw null; } }

        public DateTimeOffset Timestamp { get { throw null; } }
    }

    public enum ActivityIdFormat
    {
        Unknown = 0,
        Hierarchical = 1,
        W3C = 2
    }

    public enum ActivityKind
    {
        Internal = 0,
        Server = 1,
        Client = 2,
        Producer = 3,
        Consumer = 4
    }

    public readonly partial struct ActivityLink : IEquatable<ActivityLink>
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public ActivityLink(ActivityContext context, ActivityTagsCollection? tags = null) { }

        public ActivityContext Context { get { throw null; } }

        public Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, object?>>? Tags { get { throw null; } }

        public readonly bool Equals(ActivityLink value) { throw null; }

        public override readonly bool Equals(object? obj) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(ActivityLink left, ActivityLink right) { throw null; }

        public static bool operator !=(ActivityLink left, ActivityLink right) { throw null; }
    }

    public sealed partial class ActivityListener : IDisposable
    {
        public Action<Activity>? ActivityStarted { get { throw null; } set { } }

        public Action<Activity>? ActivityStopped { get { throw null; } set { } }

        public SampleActivity<ActivityContext>? Sample { get { throw null; } set { } }

        public SampleActivity<string>? SampleUsingParentId { get { throw null; } set { } }

        public Func<ActivitySource, bool>? ShouldListenTo { get { throw null; } set { } }

        public void Dispose() { }
    }

    public enum ActivitySamplingResult
    {
        None = 0,
        PropagationData = 1,
        AllData = 2,
        AllDataAndRecorded = 3
    }

    public sealed partial class ActivitySource : IDisposable
    {
        public ActivitySource(string name, string? version = "") { }

        public string Name { get { throw null; } }

        public string? Version { get { throw null; } }

        public static void AddActivityListener(ActivityListener listener) { }

        public void Dispose() { }

        public bool HasListeners() { throw null; }

        public Activity? StartActivity(string name, ActivityKind kind, ActivityContext parentContext, Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, object?>>? tags = null, Collections.Generic.IEnumerable<ActivityLink>? links = null, DateTimeOffset startTime = default) { throw null; }

        public Activity? StartActivity(string name, ActivityKind kind, string parentId, Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, object?>>? tags = null, Collections.Generic.IEnumerable<ActivityLink>? links = null, DateTimeOffset startTime = default) { throw null; }

        public Activity? StartActivity(string name, ActivityKind kind = ActivityKind.Internal) { throw null; }
    }

    public readonly partial struct ActivitySpanId : IEquatable<ActivitySpanId>
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public readonly void CopyTo(Span<byte> destination) { }

        public static ActivitySpanId CreateFromBytes(ReadOnlySpan<byte> idData) { throw null; }

        public static ActivitySpanId CreateFromString(ReadOnlySpan<char> idData) { throw null; }

        public static ActivitySpanId CreateFromUtf8String(ReadOnlySpan<byte> idData) { throw null; }

        public static ActivitySpanId CreateRandom() { throw null; }

        public readonly bool Equals(ActivitySpanId spanId) { throw null; }

        public override readonly bool Equals(object? obj) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(ActivitySpanId spanId1, ActivitySpanId spandId2) { throw null; }

        public static bool operator !=(ActivitySpanId spanId1, ActivitySpanId spandId2) { throw null; }

        public readonly string ToHexString() { throw null; }

        public override readonly string ToString() { throw null; }
    }

    public partial class ActivityTagsCollection : Collections.Generic.IDictionary<string, object?>, Collections.Generic.ICollection<Collections.Generic.KeyValuePair<string, object?>>, Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, object?>>, Collections.IEnumerable
    {
        public ActivityTagsCollection() { }

        public ActivityTagsCollection(Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, object?>> list) { }

        public int Count { get { throw null; } }

        public bool IsReadOnly { get { throw null; } }

        public object? this[string key] { get { throw null; } set { } }

        public Collections.Generic.ICollection<string> Keys { get { throw null; } }

        public Collections.Generic.ICollection<object?> Values { get { throw null; } }

        public void Add(Collections.Generic.KeyValuePair<string, object?> item) { }

        public void Add(string key, object? value) { }

        public void Clear() { }

        public bool Contains(Collections.Generic.KeyValuePair<string, object?> item) { throw null; }

        public bool ContainsKey(string key) { throw null; }

        public void CopyTo(Collections.Generic.KeyValuePair<string, object?>[] array, int arrayIndex) { }

        public Enumerator GetEnumerator() { throw null; }

        public bool Remove(Collections.Generic.KeyValuePair<string, object?> item) { throw null; }

        public bool Remove(string key) { throw null; }

        Collections.Generic.IEnumerator<Collections.Generic.KeyValuePair<string, object>> Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, object>>.GetEnumerator() { throw null; }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

        public bool TryGetValue(string key, out object? value) { throw null; }

        public partial struct Enumerator : Collections.Generic.IEnumerator<Collections.Generic.KeyValuePair<string, object?>>, Collections.IEnumerator, IDisposable
        {
            private int _dummyPrimitive;
            public Collections.Generic.KeyValuePair<string, object?> Current { get { throw null; } }

            object Collections.IEnumerator.Current { get { throw null; } }

            public void Dispose() { }

            public bool MoveNext() { throw null; }

            void Collections.IEnumerator.Reset() { }
        }
    }

    [Flags]
    public enum ActivityTraceFlags
    {
        None = 0,
        Recorded = 1
    }

    public readonly partial struct ActivityTraceId : IEquatable<ActivityTraceId>
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public readonly void CopyTo(Span<byte> destination) { }

        public static ActivityTraceId CreateFromBytes(ReadOnlySpan<byte> idData) { throw null; }

        public static ActivityTraceId CreateFromString(ReadOnlySpan<char> idData) { throw null; }

        public static ActivityTraceId CreateFromUtf8String(ReadOnlySpan<byte> idData) { throw null; }

        public static ActivityTraceId CreateRandom() { throw null; }

        public readonly bool Equals(ActivityTraceId traceId) { throw null; }

        public override readonly bool Equals(object? obj) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(ActivityTraceId traceId1, ActivityTraceId traceId2) { throw null; }

        public static bool operator !=(ActivityTraceId traceId1, ActivityTraceId traceId2) { throw null; }

        public readonly string ToHexString() { throw null; }

        public override readonly string ToString() { throw null; }
    }

    public partial class DiagnosticListener : DiagnosticSource, IObservable<Collections.Generic.KeyValuePair<string, object?>>, IDisposable
    {
        public DiagnosticListener(string name) { }

        public static IObservable<DiagnosticListener> AllListeners { get { throw null; } }

        public string Name { get { throw null; } }

        public virtual void Dispose() { }

        public bool IsEnabled() { throw null; }

        public override bool IsEnabled(string name, object? arg1, object? arg2 = null) { throw null; }

        public override bool IsEnabled(string name) { throw null; }

        public override void OnActivityExport(Activity activity, object? payload) { }

        public override void OnActivityImport(Activity activity, object? payload) { }

        public virtual IDisposable Subscribe(IObserver<Collections.Generic.KeyValuePair<string, object?>> observer, Func<string, object?, object?, bool>? isEnabled, Action<Activity, object?>? onActivityImport = null, Action<Activity, object?>? onActivityExport = null) { throw null; }

        public virtual IDisposable Subscribe(IObserver<Collections.Generic.KeyValuePair<string, object?>> observer, Func<string, object?, object?, bool>? isEnabled) { throw null; }

        public virtual IDisposable Subscribe(IObserver<Collections.Generic.KeyValuePair<string, object?>> observer, Predicate<string>? isEnabled) { throw null; }

        public virtual IDisposable Subscribe(IObserver<Collections.Generic.KeyValuePair<string, object?>> observer) { throw null; }

        public override string ToString() { throw null; }

        public override void Write(string name, object? value) { }
    }

    public abstract partial class DiagnosticSource
    {
        public virtual bool IsEnabled(string name, object? arg1, object? arg2 = null) { throw null; }

        public abstract bool IsEnabled(string name);
        public virtual void OnActivityExport(Activity activity, object? payload) { }

        public virtual void OnActivityImport(Activity activity, object? payload) { }

        public Activity StartActivity(Activity activity, object? args) { throw null; }

        public void StopActivity(Activity activity, object? args) { }

        public abstract void Write(string name, object? value);
    }

    public delegate ActivitySamplingResult SampleActivity<T>(ref ActivityCreationOptions<T> options);
}
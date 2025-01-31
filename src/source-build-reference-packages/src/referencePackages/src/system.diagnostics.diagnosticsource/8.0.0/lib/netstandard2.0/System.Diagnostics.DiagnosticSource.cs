// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v2.0", FrameworkDisplayName = ".NET Standard 2.0")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Diagnostics.DiagnosticSource")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata("IsTrimmable", "True")]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.System32)]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Provides Classes that allow you to decouple code logging rich (unserializable) diagnostics/telemetry (e.g. framework) from code that consumes it (e.g. tools)\r\n\r\nCommonly Used Types:\r\nSystem.Diagnostics.DiagnosticListener\r\nSystem.Diagnostics.DiagnosticSource")]
[assembly: System.Reflection.AssemblyFileVersion("8.0.23.53103")]
[assembly: System.Reflection.AssemblyInformationalVersion("8.0.0+5535e31a712343a63f5d7d796cd874e563e5ac14")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("System.Diagnostics.DiagnosticSource")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/runtime")]
[assembly: System.Reflection.AssemblyVersionAttribute("8.0.0.0")]
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

        public bool HasRemoteParent { get { throw null; } }

        public string? Id { get { throw null; } }

        public ActivityIdFormat IdFormat { get { throw null; } }

        public bool IsAllDataRequested { get { throw null; } set { } }

        public bool IsStopped { get { throw null; } }

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

        public ActivityStatusCode Status { get { throw null; } }

        public string? StatusDescription { get { throw null; } }

        public Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, object?>> TagObjects { get { throw null; } }

        public Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, string?>> Tags { get { throw null; } }

        public ActivityTraceId TraceId { get { throw null; } }

        public static Func<ActivityTraceId>? TraceIdGenerator { get { throw null; } set { } }

        public string? TraceStateString { get { throw null; } set { } }

        public static event EventHandler<ActivityChangedEventArgs>? CurrentChanged { add { } remove { } }

        public Activity AddBaggage(string key, string? value) { throw null; }

        public Activity AddEvent(ActivityEvent e) { throw null; }

        public Activity AddTag(string key, object? value) { throw null; }

        public Activity AddTag(string key, string? value) { throw null; }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        public Enumerator<ActivityEvent> EnumerateEvents() { throw null; }

        public Enumerator<ActivityLink> EnumerateLinks() { throw null; }

        public Enumerator<Collections.Generic.KeyValuePair<string, object?>> EnumerateTagObjects() { throw null; }

        public string? GetBaggageItem(string key) { throw null; }

        public object? GetCustomProperty(string propertyName) { throw null; }

        public object? GetTagItem(string key) { throw null; }

        public Activity SetBaggage(string key, string? value) { throw null; }

        public void SetCustomProperty(string propertyName, object? propertyValue) { }

        public Activity SetEndTime(DateTime endTimeUtc) { throw null; }

        public Activity SetIdFormat(ActivityIdFormat format) { throw null; }

        public Activity SetParentId(ActivityTraceId traceId, ActivitySpanId spanId, ActivityTraceFlags activityTraceFlags = ActivityTraceFlags.None) { throw null; }

        public Activity SetParentId(string parentId) { throw null; }

        public Activity SetStartTime(DateTime startTimeUtc) { throw null; }

        public Activity SetStatus(ActivityStatusCode code, string? description = null) { throw null; }

        public Activity SetTag(string key, object? value) { throw null; }

        public Activity Start() { throw null; }

        public void Stop() { }

        public partial struct Enumerator<T>
        {
            private object _dummy;
            private int _dummyPrimitive;
            public ref T Current { get { throw null; } }

            public readonly Enumerator<T> GetEnumerator() { throw null; }

            public bool MoveNext() { throw null; }
        }
    }

    public readonly partial struct ActivityChangedEventArgs
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public Activity? Current { get { throw null; } init { } }

        public Activity? Previous { get { throw null; } init { } }
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

        public static bool TryParse(string? traceParent, string? traceState, bool isRemote, out ActivityContext context) { throw null; }

        public static bool TryParse(string? traceParent, string? traceState, out ActivityContext context) { throw null; }
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

        public string? TraceState { get { throw null; } init { } }
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

        public readonly Activity.Enumerator<Collections.Generic.KeyValuePair<string, object?>> EnumerateTagObjects() { throw null; }
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

        public readonly Activity.Enumerator<Collections.Generic.KeyValuePair<string, object?>> EnumerateTagObjects() { throw null; }

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

        public Activity? CreateActivity(string name, ActivityKind kind, ActivityContext parentContext, Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, object?>>? tags = null, Collections.Generic.IEnumerable<ActivityLink>? links = null, ActivityIdFormat idFormat = ActivityIdFormat.Unknown) { throw null; }

        public Activity? CreateActivity(string name, ActivityKind kind, string? parentId, Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, object?>>? tags = null, Collections.Generic.IEnumerable<ActivityLink>? links = null, ActivityIdFormat idFormat = ActivityIdFormat.Unknown) { throw null; }

        public Activity? CreateActivity(string name, ActivityKind kind) { throw null; }

        public void Dispose() { }

        public bool HasListeners() { throw null; }

        public Activity? StartActivity(ActivityKind kind, ActivityContext parentContext = default, Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, object?>>? tags = null, Collections.Generic.IEnumerable<ActivityLink>? links = null, DateTimeOffset startTime = default, string name = "") { throw null; }

        public Activity? StartActivity(string name, ActivityKind kind, ActivityContext parentContext, Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, object?>>? tags = null, Collections.Generic.IEnumerable<ActivityLink>? links = null, DateTimeOffset startTime = default) { throw null; }

        public Activity? StartActivity(string name, ActivityKind kind, string? parentId, Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, object?>>? tags = null, Collections.Generic.IEnumerable<ActivityLink>? links = null, DateTimeOffset startTime = default) { throw null; }

        public Activity? StartActivity(string name = "", ActivityKind kind = ActivityKind.Internal) { throw null; }
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

    public enum ActivityStatusCode
    {
        Unset = 0,
        Ok = 1,
        Error = 2
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

        public Activity StartActivity<T>(Activity activity, T args) { throw null; }

        public void StopActivity(Activity activity, object? args) { }

        public void StopActivity<T>(Activity activity, T args) { }

        public abstract void Write(string name, object? value);
        public void Write<T>(string name, T value) { }
    }

    public abstract partial class DistributedContextPropagator
    {
        public static DistributedContextPropagator Current { get { throw null; } set { } }

        public abstract Collections.Generic.IReadOnlyCollection<string> Fields { get; }

        public static DistributedContextPropagator CreateDefaultPropagator() { throw null; }

        public static DistributedContextPropagator CreateNoOutputPropagator() { throw null; }

        public static DistributedContextPropagator CreatePassThroughPropagator() { throw null; }

        public abstract Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, string?>>? ExtractBaggage(object? carrier, PropagatorGetterCallback? getter);
        public abstract void ExtractTraceIdAndState(object? carrier, PropagatorGetterCallback? getter, out string? traceId, out string? traceState);
        public abstract void Inject(Activity? activity, object? carrier, PropagatorSetterCallback? setter);
        public delegate void PropagatorGetterCallback(object? carrier, string fieldName, out string? fieldValue, out Collections.Generic.IEnumerable<string>? fieldValues);
        public delegate void PropagatorSetterCallback(object? carrier, string fieldName, string fieldValue);
    }

    public delegate ActivitySamplingResult SampleActivity<T>(ref ActivityCreationOptions<T> options);
    public partial struct TagList : Collections.Generic.IList<Collections.Generic.KeyValuePair<string, object?>>, Collections.Generic.ICollection<Collections.Generic.KeyValuePair<string, object?>>, Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, object?>>, Collections.IEnumerable, Collections.Generic.IReadOnlyList<Collections.Generic.KeyValuePair<string, object?>>, Collections.Generic.IReadOnlyCollection<Collections.Generic.KeyValuePair<string, object?>>
    {
        private object _dummy;
        private int _dummyPrimitive;
        public TagList(ReadOnlySpan<Collections.Generic.KeyValuePair<string, object?>> tagList) { }

        public int Count { get { throw null; } }

        public bool IsReadOnly { get { throw null; } }

        public Collections.Generic.KeyValuePair<string, object?> this[int index] { get { throw null; } set { } }

        public void Add(Collections.Generic.KeyValuePair<string, object?> tag) { }

        public void Add(string key, object? value) { }

        public void Clear() { }

        public readonly bool Contains(Collections.Generic.KeyValuePair<string, object?> item) { throw null; }

        public readonly void CopyTo(Collections.Generic.KeyValuePair<string, object?>[] array, int arrayIndex) { }

        public readonly void CopyTo(Span<Collections.Generic.KeyValuePair<string, object?>> tags) { }

        public readonly Collections.Generic.IEnumerator<Collections.Generic.KeyValuePair<string, object?>> GetEnumerator() { throw null; }

        public readonly int IndexOf(Collections.Generic.KeyValuePair<string, object?> item) { throw null; }

        public void Insert(int index, Collections.Generic.KeyValuePair<string, object?> item) { }

        public bool Remove(Collections.Generic.KeyValuePair<string, object?> item) { throw null; }

        public void RemoveAt(int index) { }

        readonly Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

        public partial struct Enumerator : Collections.Generic.IEnumerator<Collections.Generic.KeyValuePair<string, object?>>, Collections.IEnumerator, IDisposable
        {
            private int _dummyPrimitive;
            public Collections.Generic.KeyValuePair<string, object?> Current { get { throw null; } }

            object Collections.IEnumerator.Current { get { throw null; } }

            public void Dispose() { }

            public bool MoveNext() { throw null; }

            public void Reset() { }
        }
    }
}

namespace System.Diagnostics.Metrics
{
    public sealed partial class Counter<T> : Instrument<T> where T : struct
    {
        internal Counter() : base(default!, default!, default, default) { }

        public void Add(T delta, Collections.Generic.KeyValuePair<string, object?> tag1, Collections.Generic.KeyValuePair<string, object?> tag2, Collections.Generic.KeyValuePair<string, object?> tag3) { }

        public void Add(T delta, Collections.Generic.KeyValuePair<string, object?> tag1, Collections.Generic.KeyValuePair<string, object?> tag2) { }

        public void Add(T delta, Collections.Generic.KeyValuePair<string, object?> tag) { }

        public void Add(T delta, params Collections.Generic.KeyValuePair<string, object?>[] tags) { }

        public void Add(T delta, in TagList tagList) { }

        public void Add(T delta, ReadOnlySpan<Collections.Generic.KeyValuePair<string, object?>> tags) { }

        public void Add(T delta) { }
    }

    public sealed partial class Histogram<T> : Instrument<T> where T : struct
    {
        internal Histogram() : base(default!, default!, default, default) { }

        public void Record(T value, Collections.Generic.KeyValuePair<string, object?> tag1, Collections.Generic.KeyValuePair<string, object?> tag2, Collections.Generic.KeyValuePair<string, object?> tag3) { }

        public void Record(T value, Collections.Generic.KeyValuePair<string, object?> tag1, Collections.Generic.KeyValuePair<string, object?> tag2) { }

        public void Record(T value, Collections.Generic.KeyValuePair<string, object?> tag) { }

        public void Record(T value, params Collections.Generic.KeyValuePair<string, object?>[] tags) { }

        public void Record(T value, in TagList tagList) { }

        public void Record(T value, ReadOnlySpan<Collections.Generic.KeyValuePair<string, object?>> tags) { }

        public void Record(T value) { }
    }

    public partial interface IMeterFactory : IDisposable
    {
        Meter Create(MeterOptions options);
    }

    public abstract partial class Instrument
    {
        protected Instrument(Meter meter, string name, string? unit, string? description, Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, object?>>? tags) { }

        protected Instrument(Meter meter, string name, string? unit, string? description) { }

        public string? Description { get { throw null; } }

        public bool Enabled { get { throw null; } }

        public virtual bool IsObservable { get { throw null; } }

        public Meter Meter { get { throw null; } }

        public string Name { get { throw null; } }

        public Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, object?>>? Tags { get { throw null; } }

        public string? Unit { get { throw null; } }

        protected void Publish() { }
    }

    public abstract partial class Instrument<T> : Instrument where T : struct
    {
        protected Instrument(Meter meter, string name, string? unit, string? description, Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, object?>>? tags) : base(default!, default!, default, default) { }

        protected Instrument(Meter meter, string name, string? unit, string? description) : base(default!, default!, default, default) { }

        protected void RecordMeasurement(T measurement, Collections.Generic.KeyValuePair<string, object?> tag1, Collections.Generic.KeyValuePair<string, object?> tag2, Collections.Generic.KeyValuePair<string, object?> tag3) { }

        protected void RecordMeasurement(T measurement, Collections.Generic.KeyValuePair<string, object?> tag1, Collections.Generic.KeyValuePair<string, object?> tag2) { }

        protected void RecordMeasurement(T measurement, Collections.Generic.KeyValuePair<string, object?> tag) { }

        protected void RecordMeasurement(T measurement, in TagList tagList) { }

        protected void RecordMeasurement(T measurement, ReadOnlySpan<Collections.Generic.KeyValuePair<string, object?>> tags) { }

        protected void RecordMeasurement(T measurement) { }
    }

    public delegate void MeasurementCallback<T>(Instrument instrument, T measurement, ReadOnlySpan<Collections.Generic.KeyValuePair<string, object?>> tags, object? state)
        where T : struct;
    public readonly partial struct Measurement<T>
        where T : struct
    {
        private readonly T _Value_k__BackingField;
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public Measurement(T value, Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, object?>>? tags) { }

        public Measurement(T value, params Collections.Generic.KeyValuePair<string, object?>[]? tags) { }

        public Measurement(T value, ReadOnlySpan<Collections.Generic.KeyValuePair<string, object?>> tags) { }

        public Measurement(T value) { }

        public ReadOnlySpan<Collections.Generic.KeyValuePair<string, object?>> Tags { get { throw null; } }

        public T Value { get { throw null; } }
    }

    public partial class Meter : IDisposable
    {
        public Meter(MeterOptions options) { }

        public Meter(string name, string? version, Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, object?>>? tags, object? scope = null) { }

        public Meter(string name, string? version) { }

        public Meter(string name) { }

        public string Name { get { throw null; } }

        public object? Scope { get { throw null; } }

        public Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, object?>>? Tags { get { throw null; } }

        public string? Version { get { throw null; } }

        public Counter<T> CreateCounter<T>(string name, string? unit, string? description, Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, object?>>? tags)
            where T : struct { throw null; }

        public Counter<T> CreateCounter<T>(string name, string? unit = null, string? description = null)
            where T : struct { throw null; }

        public Histogram<T> CreateHistogram<T>(string name, string? unit, string? description, Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, object?>>? tags)
            where T : struct { throw null; }

        public Histogram<T> CreateHistogram<T>(string name, string? unit = null, string? description = null)
            where T : struct { throw null; }

        public ObservableCounter<T> CreateObservableCounter<T>(string name, Func<T> observeValue, string? unit, string? description, Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, object?>>? tags)
            where T : struct { throw null; }

        public ObservableCounter<T> CreateObservableCounter<T>(string name, Func<T> observeValue, string? unit = null, string? description = null)
            where T : struct { throw null; }

        public ObservableCounter<T> CreateObservableCounter<T>(string name, Func<Collections.Generic.IEnumerable<Measurement<T>>> observeValues, string? unit, string? description, Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, object?>>? tags)
            where T : struct { throw null; }

        public ObservableCounter<T> CreateObservableCounter<T>(string name, Func<Collections.Generic.IEnumerable<Measurement<T>>> observeValues, string? unit = null, string? description = null)
            where T : struct { throw null; }

        public ObservableCounter<T> CreateObservableCounter<T>(string name, Func<Measurement<T>> observeValue, string? unit, string? description, Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, object?>>? tags)
            where T : struct { throw null; }

        public ObservableCounter<T> CreateObservableCounter<T>(string name, Func<Measurement<T>> observeValue, string? unit = null, string? description = null)
            where T : struct { throw null; }

        public ObservableGauge<T> CreateObservableGauge<T>(string name, Func<T> observeValue, string? unit, string? description, Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, object?>>? tags)
            where T : struct { throw null; }

        public ObservableGauge<T> CreateObservableGauge<T>(string name, Func<T> observeValue, string? unit = null, string? description = null)
            where T : struct { throw null; }

        public ObservableGauge<T> CreateObservableGauge<T>(string name, Func<Collections.Generic.IEnumerable<Measurement<T>>> observeValues, string? unit, string? description, Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, object?>>? tags)
            where T : struct { throw null; }

        public ObservableGauge<T> CreateObservableGauge<T>(string name, Func<Collections.Generic.IEnumerable<Measurement<T>>> observeValues, string? unit = null, string? description = null)
            where T : struct { throw null; }

        public ObservableGauge<T> CreateObservableGauge<T>(string name, Func<Measurement<T>> observeValue, string? unit, string? description, Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, object?>>? tags)
            where T : struct { throw null; }

        public ObservableGauge<T> CreateObservableGauge<T>(string name, Func<Measurement<T>> observeValue, string? unit = null, string? description = null)
            where T : struct { throw null; }

        public ObservableUpDownCounter<T> CreateObservableUpDownCounter<T>(string name, Func<T> observeValue, string? unit, string? description, Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, object?>>? tags)
            where T : struct { throw null; }

        public ObservableUpDownCounter<T> CreateObservableUpDownCounter<T>(string name, Func<T> observeValue, string? unit = null, string? description = null)
            where T : struct { throw null; }

        public ObservableUpDownCounter<T> CreateObservableUpDownCounter<T>(string name, Func<Collections.Generic.IEnumerable<Measurement<T>>> observeValues, string? unit, string? description, Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, object?>>? tags)
            where T : struct { throw null; }

        public ObservableUpDownCounter<T> CreateObservableUpDownCounter<T>(string name, Func<Collections.Generic.IEnumerable<Measurement<T>>> observeValues, string? unit = null, string? description = null)
            where T : struct { throw null; }

        public ObservableUpDownCounter<T> CreateObservableUpDownCounter<T>(string name, Func<Measurement<T>> observeValue, string? unit, string? description, Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, object?>>? tags)
            where T : struct { throw null; }

        public ObservableUpDownCounter<T> CreateObservableUpDownCounter<T>(string name, Func<Measurement<T>> observeValue, string? unit = null, string? description = null)
            where T : struct { throw null; }

        public UpDownCounter<T> CreateUpDownCounter<T>(string name, string? unit, string? description, Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, object?>>? tags)
            where T : struct { throw null; }

        public UpDownCounter<T> CreateUpDownCounter<T>(string name, string? unit = null, string? description = null)
            where T : struct { throw null; }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }
    }

    public static partial class MeterFactoryExtensions
    {
        public static Meter Create(this IMeterFactory meterFactory, string name, string? version = null, Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, object?>>? tags = null) { throw null; }
    }

    public sealed partial class MeterListener : IDisposable
    {
        public Action<Instrument, MeterListener>? InstrumentPublished { get { throw null; } set { } }

        public Action<Instrument, object?>? MeasurementsCompleted { get { throw null; } set { } }

        public object? DisableMeasurementEvents(Instrument instrument) { throw null; }

        public void Dispose() { }

        public void EnableMeasurementEvents(Instrument instrument, object? state = null) { }

        public void RecordObservableInstruments() { }

        public void SetMeasurementEventCallback<T>(MeasurementCallback<T>? measurementCallback)
            where T : struct { }

        public void Start() { }
    }

    public partial class MeterOptions
    {
        public MeterOptions(string name) { }

        public string Name { get { throw null; } set { } }

        public object? Scope { get { throw null; } set { } }

        public Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, object?>>? Tags { get { throw null; } set { } }

        public string? Version { get { throw null; } set { } }
    }

    public sealed partial class ObservableCounter<T> : ObservableInstrument<T> where T : struct
    {
        internal ObservableCounter() : base(default!, default!, default, default) { }

        protected override Collections.Generic.IEnumerable<Measurement<T>> Observe() { throw null; }
    }

    public sealed partial class ObservableGauge<T> : ObservableInstrument<T> where T : struct
    {
        internal ObservableGauge() : base(default!, default!, default, default) { }

        protected override Collections.Generic.IEnumerable<Measurement<T>> Observe() { throw null; }
    }

    public abstract partial class ObservableInstrument<T> : Instrument where T : struct
    {
        protected ObservableInstrument(Meter meter, string name, string? unit, string? description, Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, object?>>? tags) : base(default!, default!, default, default) { }

        protected ObservableInstrument(Meter meter, string name, string? unit, string? description) : base(default!, default!, default, default) { }

        public override bool IsObservable { get { throw null; } }

        protected abstract Collections.Generic.IEnumerable<Measurement<T>> Observe();
    }

    public sealed partial class ObservableUpDownCounter<T> : ObservableInstrument<T> where T : struct
    {
        internal ObservableUpDownCounter() : base(default!, default!, default, default) { }

        protected override Collections.Generic.IEnumerable<Measurement<T>> Observe() { throw null; }
    }

    public sealed partial class UpDownCounter<T> : Instrument<T> where T : struct
    {
        internal UpDownCounter() : base(default!, default!, default, default) { }

        public void Add(T delta, Collections.Generic.KeyValuePair<string, object?> tag1, Collections.Generic.KeyValuePair<string, object?> tag2, Collections.Generic.KeyValuePair<string, object?> tag3) { }

        public void Add(T delta, Collections.Generic.KeyValuePair<string, object?> tag1, Collections.Generic.KeyValuePair<string, object?> tag2) { }

        public void Add(T delta, Collections.Generic.KeyValuePair<string, object?> tag) { }

        public void Add(T delta, params Collections.Generic.KeyValuePair<string, object?>[] tags) { }

        public void Add(T delta, in TagList tagList) { }

        public void Add(T delta, ReadOnlySpan<Collections.Generic.KeyValuePair<string, object?>> tags) { }

        public void Add(T delta) { }
    }
}
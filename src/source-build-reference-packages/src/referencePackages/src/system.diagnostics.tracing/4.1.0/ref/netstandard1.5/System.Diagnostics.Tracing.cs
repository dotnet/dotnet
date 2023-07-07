// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Security.AllowPartiallyTrustedCallers]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyTitle("System.Diagnostics.Tracing")]
[assembly: System.Reflection.AssemblyDescription("System.Diagnostics.Tracing")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Diagnostics.Tracing")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("1.0.24212.01")]
[assembly: System.Reflection.AssemblyInformationalVersion("1.0.24212.01. Commit Hash: 9688ddbb62c04189cac4c4a06e31e93377dccd41")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.1.0.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Diagnostics.Tracing
{
    [Flags]
    public enum EventActivityOptions
    {
        None = 0,
        Disable = 2,
        Recursive = 4,
        Detachable = 8
    }

    [AttributeUsage(AttributeTargets.Method)]
    public sealed partial class EventAttribute : Attribute
    {
        public EventAttribute(int eventId) { }

        public EventActivityOptions ActivityOptions { get { throw null; } set { } }

        public EventChannel Channel { get { throw null; } set { } }

        public int EventId { get { throw null; } }

        public EventKeywords Keywords { get { throw null; } set { } }

        public EventLevel Level { get { throw null; } set { } }

        public string Message { get { throw null; } set { } }

        public EventOpcode Opcode { get { throw null; } set { } }

        public EventTags Tags { get { throw null; } set { } }

        public EventTask Task { get { throw null; } set { } }

        public byte Version { get { throw null; } set { } }
    }

    public enum EventChannel : byte
    {
        None = 0,
        Admin = 16,
        Operational = 17,
        Analytic = 18,
        Debug = 19
    }

    public enum EventCommand
    {
        Disable = -3,
        Enable = -2,
        SendManifest = -1,
        Update = 0
    }

    public partial class EventCommandEventArgs : EventArgs
    {
        internal EventCommandEventArgs() { }

        public Collections.Generic.IDictionary<string, string> Arguments { get { throw null; } }

        public EventCommand Command { get { throw null; } }

        public bool DisableEvent(int eventId) { throw null; }

        public bool EnableEvent(int eventId) { throw null; }
    }

    public partial class EventCounter
    {
        public EventCounter(string name, EventSource eventSource) { }

        public void WriteMetric(float value) { }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
    public partial class EventDataAttribute : Attribute
    {
        public string Name { get { throw null; } set { } }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public partial class EventFieldAttribute : Attribute
    {
        public EventFieldFormat Format { get { throw null; } set { } }

        public EventFieldTags Tags { get { throw null; } set { } }
    }

    public enum EventFieldFormat
    {
        Default = 0,
        String = 2,
        Boolean = 3,
        Hexadecimal = 4,
        Xml = 11,
        Json = 12,
        HResult = 15
    }

    [Flags]
    public enum EventFieldTags
    {
        None = 0
    }

    [AttributeUsage(AttributeTargets.Property)]
    public partial class EventIgnoreAttribute : Attribute
    {
    }

    [Flags]
    public enum EventKeywords : long
    {
        All = -1L,
        AuditFailure = 4503599627370496L,
        AuditSuccess = 9007199254740992L,
        CorrelationHint = 4503599627370496L,
        EventLogClassic = 36028797018963968L,
        None = 0L,
        Sqm = 2251799813685248L,
        WdiContext = 562949953421312L,
        WdiDiagnostic = 1125899906842624L
    }

    public enum EventLevel
    {
        LogAlways = 0,
        Critical = 1,
        Error = 2,
        Warning = 3,
        Informational = 4,
        Verbose = 5
    }

    public abstract partial class EventListener : IDisposable
    {
        public void DisableEvents(EventSource eventSource) { }

        public virtual void Dispose() { }

        public void EnableEvents(EventSource eventSource, EventLevel level, EventKeywords matchAnyKeyword, Collections.Generic.IDictionary<string, string> arguments) { }

        public void EnableEvents(EventSource eventSource, EventLevel level, EventKeywords matchAnyKeyword) { }

        public void EnableEvents(EventSource eventSource, EventLevel level) { }

        protected static int EventSourceIndex(EventSource eventSource) { throw null; }

        protected internal virtual void OnEventSourceCreated(EventSource eventSource) { }

        protected internal abstract void OnEventWritten(EventWrittenEventArgs eventData);
    }

    [Flags]
    public enum EventManifestOptions
    {
        None = 0,
        Strict = 1,
        AllCultures = 2,
        OnlyIfNeededForRegistration = 4,
        AllowEventSourceOverride = 8
    }

    public enum EventOpcode
    {
        Info = 0,
        Start = 1,
        Stop = 2,
        DataCollectionStart = 3,
        DataCollectionStop = 4,
        Extension = 5,
        Reply = 6,
        Resume = 7,
        Suspend = 8,
        Send = 9,
        Receive = 240
    }

    public partial class EventSource : IDisposable
    {
        protected EventSource() { }

        protected EventSource(bool throwOnEventWriteErrors) { }

        protected EventSource(EventSourceSettings settings, params string[] traits) { }

        protected EventSource(EventSourceSettings settings) { }

        public EventSource(string eventSourceName, EventSourceSettings config, params string[] traits) { }

        public EventSource(string eventSourceName, EventSourceSettings config) { }

        public EventSource(string eventSourceName) { }

        public Exception ConstructionException { get { throw null; } }

        public static Guid CurrentThreadActivityId { get { throw null; } }

        public Guid Guid { get { throw null; } }

        public string Name { get { throw null; } }

        public EventSourceSettings Settings { get { throw null; } }

        public event EventHandler<EventCommandEventArgs> EventCommandExecuted { add { } remove { } }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        ~EventSource() {
        }

        public static string GenerateManifest(Type eventSourceType, string assemblyPathToIncludeInManifest, EventManifestOptions flags) { throw null; }

        public static string GenerateManifest(Type eventSourceType, string assemblyPathToIncludeInManifest) { throw null; }

        public static Guid GetGuid(Type eventSourceType) { throw null; }

        public static string GetName(Type eventSourceType) { throw null; }

        public static Collections.Generic.IEnumerable<EventSource> GetSources() { throw null; }

        public string GetTrait(string key) { throw null; }

        public bool IsEnabled() { throw null; }

        public bool IsEnabled(EventLevel level, EventKeywords keywords, EventChannel channel) { throw null; }

        public bool IsEnabled(EventLevel level, EventKeywords keywords) { throw null; }

        protected virtual void OnEventCommand(EventCommandEventArgs command) { }

        public static void SendCommand(EventSource eventSource, EventCommand command, Collections.Generic.IDictionary<string, string> commandArguments) { }

        public static void SetCurrentThreadActivityId(Guid activityId, out Guid oldActivityThatWillContinue) { throw null; }

        public static void SetCurrentThreadActivityId(Guid activityId) { }

        public override string ToString() { throw null; }

        public void Write(string eventName, EventSourceOptions options) { }

        public void Write(string eventName) { }

        public void Write<T>(string eventName, T data) { }

        public void Write<T>(string eventName, EventSourceOptions options, T data) { }

        public void Write<T>(string eventName, ref EventSourceOptions options, ref T data) { }

        public void Write<T>(string eventName, ref EventSourceOptions options, ref Guid activityId, ref Guid relatedActivityId, ref T data) { }

        protected void WriteEvent(int eventId, byte[] arg1) { }

        protected void WriteEvent(int eventId, int arg1, int arg2, int arg3) { }

        protected void WriteEvent(int eventId, int arg1, int arg2) { }

        protected void WriteEvent(int eventId, int arg1, string arg2) { }

        protected void WriteEvent(int eventId, int arg1) { }

        protected void WriteEvent(int eventId, long arg1, byte[] arg2) { }

        protected void WriteEvent(int eventId, long arg1, long arg2, long arg3) { }

        protected void WriteEvent(int eventId, long arg1, long arg2) { }

        protected void WriteEvent(int eventId, long arg1, string arg2) { }

        protected void WriteEvent(int eventId, long arg1) { }

        protected void WriteEvent(int eventId, params object[] args) { }

        protected void WriteEvent(int eventId, string arg1, int arg2, int arg3) { }

        protected void WriteEvent(int eventId, string arg1, int arg2) { }

        protected void WriteEvent(int eventId, string arg1, long arg2) { }

        protected void WriteEvent(int eventId, string arg1, string arg2, string arg3) { }

        protected void WriteEvent(int eventId, string arg1, string arg2) { }

        protected void WriteEvent(int eventId, string arg1) { }

        protected void WriteEvent(int eventId) { }

        [CLSCompliant(false)]
        protected unsafe void WriteEventCore(int eventId, int eventDataCount, EventData* data) { }

        protected void WriteEventWithRelatedActivityId(int eventId, Guid relatedActivityId, params object[] args) { }

        [CLSCompliant(false)]
        protected unsafe void WriteEventWithRelatedActivityIdCore(int eventId, Guid* relatedActivityId, int eventDataCount, EventData* data) { }

        protected internal partial struct EventData
        {
            public IntPtr DataPointer { get { throw null; } set { } }

            public int Size { get { throw null; } set { } }
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public sealed partial class EventSourceAttribute : Attribute
    {
        public string Guid { get { throw null; } set { } }

        public string LocalizationResources { get { throw null; } set { } }

        public string Name { get { throw null; } set { } }
    }

    public partial class EventSourceException : Exception
    {
        public EventSourceException() { }

        public EventSourceException(string message, Exception innerException) { }

        public EventSourceException(string message) { }
    }

    public partial struct EventSourceOptions
    {
        public EventActivityOptions ActivityOptions { get { throw null; } set { } }

        public EventKeywords Keywords { get { throw null; } set { } }

        public EventLevel Level { get { throw null; } set { } }

        public EventOpcode Opcode { get { throw null; } set { } }

        public EventTags Tags { get { throw null; } set { } }
    }

    [Flags]
    public enum EventSourceSettings
    {
        Default = 0,
        ThrowOnEventWriteErrors = 1,
        EtwManifestEventFormat = 4,
        EtwSelfDescribingEventFormat = 8
    }

    [Flags]
    public enum EventTags
    {
        None = 0
    }

    public enum EventTask
    {
        None = 0
    }

    public partial class EventWrittenEventArgs : EventArgs
    {
        internal EventWrittenEventArgs() { }

        public Guid ActivityId { get { throw null; } }

        public EventChannel Channel { get { throw null; } }

        public int EventId { get { throw null; } }

        public string EventName { get { throw null; } }

        public EventSource EventSource { get { throw null; } }

        public EventKeywords Keywords { get { throw null; } }

        public EventLevel Level { get { throw null; } }

        public string Message { get { throw null; } }

        public EventOpcode Opcode { get { throw null; } }

        public Collections.ObjectModel.ReadOnlyCollection<object> Payload { get { throw null; } }

        public Collections.ObjectModel.ReadOnlyCollection<string> PayloadNames { get { throw null; } }

        public Guid RelatedActivityId { get { throw null; } }

        public EventTags Tags { get { throw null; } }

        public EventTask Task { get { throw null; } }

        public byte Version { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public sealed partial class NonEventAttribute : Attribute
    {
    }
}
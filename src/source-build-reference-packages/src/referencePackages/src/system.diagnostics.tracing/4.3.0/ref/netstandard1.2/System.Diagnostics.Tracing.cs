// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Security.AllowPartiallyTrustedCallers]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyTitle("System.Diagnostics.Tracing.dll")]
[assembly: System.Reflection.AssemblyDescription("System.Diagnostics.Tracing.dll")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Diagnostics.Tracing.dll")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyMetadata("BuildLabel", "130703.2")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyInformationalVersion("4.0.40013.0")]
[assembly: System.Reflection.AssemblyFileVersion("4.0.40013.0")]
[assembly: System.Reflection.AssemblyMetadata("BuildBranch", "Release\\ReferenceAssemblies\\1.0")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.10.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Diagnostics.Tracing
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed partial class EventAttribute : Attribute
    {
        public EventAttribute(int eventId) { }

        public int EventId { get { throw null; } }

        public EventKeywords Keywords { get { throw null; } set { } }

        public EventLevel Level { get { throw null; } set { } }

        public string Message { get { throw null; } set { } }

        public EventOpcode Opcode { get { throw null; } set { } }

        public EventTask Task { get { throw null; } set { } }

        public byte Version { get { throw null; } set { } }
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

    [Flags]
    public enum EventKeywords : long
    {
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

        public Exception ConstructionException { get { throw null; } }

        public static Guid CurrentThreadActivityId { get { throw null; } }

        public Guid Guid { get { throw null; } }

        public string Name { get { throw null; } }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        ~EventSource() {
        }

        public static string GenerateManifest(Type eventSourceType, string assemblyPathToIncludeInManifest) { throw null; }

        public static Guid GetGuid(Type eventSourceType) { throw null; }

        public static string GetName(Type eventSourceType) { throw null; }

        public static Collections.Generic.IEnumerable<EventSource> GetSources() { throw null; }

        public bool IsEnabled() { throw null; }

        public bool IsEnabled(EventLevel level, EventKeywords keywords) { throw null; }

        protected virtual void OnEventCommand(EventCommandEventArgs command) { }

        public static void SendCommand(EventSource eventSource, EventCommand command, Collections.Generic.IDictionary<string, string> commandArguments) { }

        public static void SetCurrentThreadActivityId(Guid activityId, out Guid oldActivityThatWillContinue) { throw null; }

        public static void SetCurrentThreadActivityId(Guid activityId) { }

        public override string ToString() { throw null; }

        protected void WriteEvent(int eventId, int arg1, int arg2, int arg3) { }

        protected void WriteEvent(int eventId, int arg1, int arg2) { }

        protected void WriteEvent(int eventId, int arg1) { }

        protected void WriteEvent(int eventId, long arg1, long arg2, long arg3) { }

        protected void WriteEvent(int eventId, long arg1, long arg2) { }

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

        protected void WriteEventWithRelatedActivityId(int eventId, Guid childActivityID, params object[] args) { }

        [CLSCompliant(false)]
        protected unsafe void WriteEventWithRelatedActivityIdCore(int eventId, Guid* childActivityID, int eventDataCount, EventData* data) { }

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

    public enum EventTask
    {
        None = 0
    }

    public partial class EventWrittenEventArgs : EventArgs
    {
        internal EventWrittenEventArgs() { }

        public Guid ActivityId { get { throw null; } }

        public int EventId { get { throw null; } }

        public EventSource EventSource { get { throw null; } }

        public EventKeywords Keywords { get { throw null; } }

        public EventLevel Level { get { throw null; } }

        public string Message { get { throw null; } }

        public EventOpcode Opcode { get { throw null; } }

        public Collections.ObjectModel.ReadOnlyCollection<object> Payload { get { throw null; } }

        public Guid RelatedActivityId { get { throw null; } }

        public EventTask Task { get { throw null; } }

        public byte Version { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public sealed partial class NonEventAttribute : Attribute
    {
    }
}
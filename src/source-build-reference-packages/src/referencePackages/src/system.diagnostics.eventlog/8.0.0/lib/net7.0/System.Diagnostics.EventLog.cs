// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.CompilerServices.DisableRuntimeMarshalling]
[assembly: System.Runtime.Versioning.TargetFramework(".NETCoreApp,Version=v7.0", FrameworkDisplayName = ".NET 7.0")]
[assembly: System.Reflection.AssemblyMetadata("NotSupported", "True")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Diagnostics.EventLog")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata("IsTrimmable", "True")]
[assembly: System.Runtime.Versioning.SupportedOSPlatform("windows")]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.System32)]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Provides the System.Diagnostics.EventLog class, which allows the applications to use the Windows event log service.\r\n\r\nCommonly Used Types:\r\nSystem.Diagnostics.EventLog")]
[assembly: System.Reflection.AssemblyFileVersion("8.0.23.53103")]
[assembly: System.Reflection.AssemblyInformationalVersion("8.0.0+5535e31a712343a63f5d7d796cd874e563e5ac14")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("System.Diagnostics.EventLog")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/runtime")]
[assembly: System.Reflection.AssemblyVersionAttribute("8.0.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Diagnostics
{
    public partial class EntryWrittenEventArgs : EventArgs
    {
        public EntryWrittenEventArgs() { }

        public EntryWrittenEventArgs(EventLogEntry entry) { }

        public EventLogEntry Entry { get { throw null; } }
    }

    public delegate void EntryWrittenEventHandler(object sender, EntryWrittenEventArgs e);
    public partial class EventInstance
    {
        public EventInstance(long instanceId, int categoryId, EventLogEntryType entryType) { }

        public EventInstance(long instanceId, int categoryId) { }

        public int CategoryId { get { throw null; } set { } }

        public EventLogEntryType EntryType { get { throw null; } set { } }

        public long InstanceId { get { throw null; } set { } }
    }

    [ComponentModel.DefaultEvent("EntryWritten")]
    public partial class EventLog : ComponentModel.Component, ComponentModel.ISupportInitialize
    {
        public EventLog() { }

        public EventLog(string logName, string machineName, string source) { }

        public EventLog(string logName, string machineName) { }

        public EventLog(string logName) { }

        [ComponentModel.Browsable(false)]
        [ComponentModel.DefaultValue(false)]
        public bool EnableRaisingEvents { get { throw null; } set { } }

        [ComponentModel.Browsable(false)]
        [ComponentModel.DesignerSerializationVisibility(ComponentModel.DesignerSerializationVisibility.Hidden)]
        public EventLogEntryCollection Entries { get { throw null; } }

        [ComponentModel.DefaultValue("")]
        [ComponentModel.ReadOnly(true)]
        [ComponentModel.SettingsBindable(true)]
        public string Log { get { throw null; } set { } }

        [ComponentModel.Browsable(false)]
        public string LogDisplayName { get { throw null; } }

        [ComponentModel.DefaultValue(".")]
        [ComponentModel.ReadOnly(true)]
        [ComponentModel.SettingsBindable(true)]
        public string MachineName { get { throw null; } set { } }

        [ComponentModel.Browsable(false)]
        [ComponentModel.DesignerSerializationVisibility(ComponentModel.DesignerSerializationVisibility.Hidden)]
        public long MaximumKilobytes { get { throw null; } set { } }

        [ComponentModel.Browsable(false)]
        public int MinimumRetentionDays { get { throw null; } }

        [ComponentModel.Browsable(false)]
        public OverflowAction OverflowAction { get { throw null; } }

        [ComponentModel.DefaultValue("")]
        [ComponentModel.ReadOnly(true)]
        [ComponentModel.SettingsBindable(true)]
        public string Source { get { throw null; } set { } }

        [ComponentModel.Browsable(false)]
        [ComponentModel.DefaultValue(null)]
        public ComponentModel.ISynchronizeInvoke SynchronizingObject { get { throw null; } set { } }

        public event EntryWrittenEventHandler EntryWritten { add { } remove { } }

        public void BeginInit() { }

        public void Clear() { }

        public void Close() { }

        public static void CreateEventSource(EventSourceCreationData sourceData) { }

        [Obsolete("EventLog.CreateEventSource has been deprecated. Use System.Diagnostics.EventLog.CreateEventSource(EventSourceCreationData sourceData) instead.")]
        public static void CreateEventSource(string source, string logName, string machineName) { }

        public static void CreateEventSource(string source, string logName) { }

        public static void Delete(string logName, string machineName) { }

        public static void Delete(string logName) { }

        public static void DeleteEventSource(string source, string machineName) { }

        public static void DeleteEventSource(string source) { }

        protected override void Dispose(bool disposing) { }

        public void EndInit() { }

        public static bool Exists(string logName, string machineName) { throw null; }

        public static bool Exists(string logName) { throw null; }

        public static EventLog[] GetEventLogs() { throw null; }

        public static EventLog[] GetEventLogs(string machineName) { throw null; }

        public static string LogNameFromSourceName(string source, string machineName) { throw null; }

        public void ModifyOverflowPolicy(OverflowAction action, int retentionDays) { }

        public void RegisterDisplayName(string resourceFile, long resourceId) { }

        public static bool SourceExists(string source, string machineName) { throw null; }

        public static bool SourceExists(string source) { throw null; }

        public void WriteEntry(string message, EventLogEntryType type, int eventID, short category, byte[] rawData) { }

        public void WriteEntry(string message, EventLogEntryType type, int eventID, short category) { }

        public void WriteEntry(string message, EventLogEntryType type, int eventID) { }

        public void WriteEntry(string message, EventLogEntryType type) { }

        public static void WriteEntry(string source, string message, EventLogEntryType type, int eventID, short category, byte[] rawData) { }

        public static void WriteEntry(string source, string message, EventLogEntryType type, int eventID, short category) { }

        public static void WriteEntry(string source, string message, EventLogEntryType type, int eventID) { }

        public static void WriteEntry(string source, string message, EventLogEntryType type) { }

        public static void WriteEntry(string source, string message) { }

        public void WriteEntry(string message) { }

        public void WriteEvent(EventInstance instance, byte[] data, params object[] values) { }

        public void WriteEvent(EventInstance instance, params object[] values) { }

        public static void WriteEvent(string source, EventInstance instance, byte[] data, params object[] values) { }

        public static void WriteEvent(string source, EventInstance instance, params object[] values) { }
    }

    [ComponentModel.DesignTimeVisible(false)]
    [ComponentModel.ToolboxItem(false)]
    public sealed partial class EventLogEntry : ComponentModel.Component, Runtime.Serialization.ISerializable
    {
        internal EventLogEntry() { }

        public string Category { get { throw null; } }

        public short CategoryNumber { get { throw null; } }

        public byte[] Data { get { throw null; } }

        public EventLogEntryType EntryType { get { throw null; } }

        [Obsolete("EventLogEntry.EventID has been deprecated. Use System.Diagnostics.EventLogEntry.InstanceId instead.")]
        public int EventID { get { throw null; } }

        public int Index { get { throw null; } }

        public long InstanceId { get { throw null; } }

        public string MachineName { get { throw null; } }

        public string Message { get { throw null; } }

        public string[] ReplacementStrings { get { throw null; } }

        public string Source { get { throw null; } }

        public DateTime TimeGenerated { get { throw null; } }

        public DateTime TimeWritten { get { throw null; } }

        public string UserName { get { throw null; } }

        public bool Equals(EventLogEntry otherEntry) { throw null; }

        void Runtime.Serialization.ISerializable.GetObjectData(Runtime.Serialization.SerializationInfo info, Runtime.Serialization.StreamingContext context) { }
    }

    public partial class EventLogEntryCollection : Collections.ICollection, Collections.IEnumerable
    {
        internal EventLogEntryCollection() { }

        public int Count { get { throw null; } }

        public virtual EventLogEntry this[int index] { get { throw null; } }

        bool Collections.ICollection.IsSynchronized { get { throw null; } }

        object Collections.ICollection.SyncRoot { get { throw null; } }

        public void CopyTo(EventLogEntry[] entries, int index) { }

        public Collections.IEnumerator GetEnumerator() { throw null; }

        void Collections.ICollection.CopyTo(Array array, int index) { }
    }

    public enum EventLogEntryType
    {
        Error = 1,
        Warning = 2,
        Information = 4,
        SuccessAudit = 8,
        FailureAudit = 16
    }

    public sealed partial class EventLogTraceListener : TraceListener
    {
        public EventLogTraceListener() { }

        public EventLogTraceListener(EventLog eventLog) { }

        public EventLogTraceListener(string source) { }

        public EventLog EventLog { get { throw null; } set { } }

        public override string Name { get { throw null; } set { } }

        public override void Close() { }

        protected override void Dispose(bool disposing) { }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType severity, int id, object data) { }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType severity, int id, params object[] data) { }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType severity, int id, string format, params object[] args) { }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType severity, int id, string message) { }

        public override void Write(string message) { }

        public override void WriteLine(string message) { }
    }

    public partial class EventSourceCreationData
    {
        public EventSourceCreationData(string source, string logName) { }

        public int CategoryCount { get { throw null; } set { } }

        public string CategoryResourceFile { get { throw null; } set { } }

        public string LogName { get { throw null; } set { } }

        public string MachineName { get { throw null; } set { } }

        public string MessageResourceFile { get { throw null; } set { } }

        public string ParameterResourceFile { get { throw null; } set { } }

        public string Source { get { throw null; } set { } }
    }

    public enum OverflowAction
    {
        DoNotOverwrite = -1,
        OverwriteAsNeeded = 0,
        OverwriteOlder = 1
    }
}

namespace System.Diagnostics.Eventing.Reader
{
    public sealed partial class EventBookmark
    {
        public EventBookmark(string bookmarkXml) { }

        public string BookmarkXml { get { throw null; } }
    }

    public sealed partial class EventKeyword
    {
        internal EventKeyword() { }

        public string DisplayName { get { throw null; } }

        public string Name { get { throw null; } }

        public long Value { get { throw null; } }
    }

    public sealed partial class EventLevel
    {
        internal EventLevel() { }

        public string DisplayName { get { throw null; } }

        public string Name { get { throw null; } }

        public int Value { get { throw null; } }
    }

    public partial class EventLogConfiguration : IDisposable
    {
        public EventLogConfiguration(string logName, EventLogSession session) { }

        public EventLogConfiguration(string logName) { }

        public bool IsClassicLog { get { throw null; } }

        public bool IsEnabled { get { throw null; } set { } }

        public string LogFilePath { get { throw null; } set { } }

        public EventLogIsolation LogIsolation { get { throw null; } }

        public EventLogMode LogMode { get { throw null; } set { } }

        public string LogName { get { throw null; } }

        public EventLogType LogType { get { throw null; } }

        public long MaximumSizeInBytes { get { throw null; } set { } }

        public string OwningProviderName { get { throw null; } }

        public int? ProviderBufferSize { get { throw null; } }

        public Guid? ProviderControlGuid { get { throw null; } }

        public long? ProviderKeywords { get { throw null; } set { } }

        public int? ProviderLatency { get { throw null; } }

        public int? ProviderLevel { get { throw null; } set { } }

        public int? ProviderMaximumNumberOfBuffers { get { throw null; } }

        public int? ProviderMinimumNumberOfBuffers { get { throw null; } }

        public Collections.Generic.IEnumerable<string> ProviderNames { get { throw null; } }

        public string SecurityDescriptor { get { throw null; } set { } }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        public void SaveChanges() { }
    }

    public partial class EventLogException : Exception
    {
        public EventLogException() { }

        protected EventLogException(int errorCode) { }

        protected EventLogException(Runtime.Serialization.SerializationInfo serializationInfo, Runtime.Serialization.StreamingContext streamingContext) { }

        public EventLogException(string message, Exception innerException) { }

        public EventLogException(string message) { }

        public override string Message { get { throw null; } }

        public override void GetObjectData(Runtime.Serialization.SerializationInfo info, Runtime.Serialization.StreamingContext context) { }
    }

    public sealed partial class EventLogInformation
    {
        internal EventLogInformation() { }

        public int? Attributes { get { throw null; } }

        public DateTime? CreationTime { get { throw null; } }

        public long? FileSize { get { throw null; } }

        public bool? IsLogFull { get { throw null; } }

        public DateTime? LastAccessTime { get { throw null; } }

        public DateTime? LastWriteTime { get { throw null; } }

        public long? OldestRecordNumber { get { throw null; } }

        public long? RecordCount { get { throw null; } }
    }

    public partial class EventLogInvalidDataException : EventLogException
    {
        public EventLogInvalidDataException() { }

        protected EventLogInvalidDataException(Runtime.Serialization.SerializationInfo serializationInfo, Runtime.Serialization.StreamingContext streamingContext) { }

        public EventLogInvalidDataException(string message, Exception innerException) { }

        public EventLogInvalidDataException(string message) { }
    }

    public enum EventLogIsolation
    {
        Application = 0,
        System = 1,
        Custom = 2
    }

    public sealed partial class EventLogLink
    {
        internal EventLogLink() { }

        public string DisplayName { get { throw null; } }

        public bool IsImported { get { throw null; } }

        public string LogName { get { throw null; } }
    }

    public enum EventLogMode
    {
        Circular = 0,
        AutoBackup = 1,
        Retain = 2
    }

    public partial class EventLogNotFoundException : EventLogException
    {
        public EventLogNotFoundException() { }

        protected EventLogNotFoundException(Runtime.Serialization.SerializationInfo serializationInfo, Runtime.Serialization.StreamingContext streamingContext) { }

        public EventLogNotFoundException(string message, Exception innerException) { }

        public EventLogNotFoundException(string message) { }
    }

    public partial class EventLogPropertySelector : IDisposable
    {
        public EventLogPropertySelector(Collections.Generic.IEnumerable<string> propertyQueries) { }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }
    }

    public partial class EventLogProviderDisabledException : EventLogException
    {
        public EventLogProviderDisabledException() { }

        protected EventLogProviderDisabledException(Runtime.Serialization.SerializationInfo serializationInfo, Runtime.Serialization.StreamingContext streamingContext) { }

        public EventLogProviderDisabledException(string message, Exception innerException) { }

        public EventLogProviderDisabledException(string message) { }
    }

    public partial class EventLogQuery
    {
        public EventLogQuery(string path, PathType pathType, string query) { }

        public EventLogQuery(string path, PathType pathType) { }

        public bool ReverseDirection { get { throw null; } set { } }

        public EventLogSession Session { get { throw null; } set { } }

        public bool TolerateQueryErrors { get { throw null; } set { } }
    }

    public partial class EventLogReader : IDisposable
    {
        public EventLogReader(EventLogQuery eventQuery, EventBookmark bookmark) { }

        public EventLogReader(EventLogQuery eventQuery) { }

        public EventLogReader(string path, PathType pathType) { }

        public EventLogReader(string path) { }

        public int BatchSize { get { throw null; } set { } }

        public Collections.Generic.IList<EventLogStatus> LogStatus { get { throw null; } }

        public void CancelReading() { }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        public EventRecord ReadEvent() { throw null; }

        public EventRecord ReadEvent(TimeSpan timeout) { throw null; }

        public void Seek(EventBookmark bookmark, long offset) { }

        public void Seek(EventBookmark bookmark) { }

        public void Seek(IO.SeekOrigin origin, long offset) { }
    }

    public partial class EventLogReadingException : EventLogException
    {
        public EventLogReadingException() { }

        protected EventLogReadingException(Runtime.Serialization.SerializationInfo serializationInfo, Runtime.Serialization.StreamingContext streamingContext) { }

        public EventLogReadingException(string message, Exception innerException) { }

        public EventLogReadingException(string message) { }
    }

    public partial class EventLogRecord : EventRecord
    {
        internal EventLogRecord() { }

        public override Guid? ActivityId { get { throw null; } }

        public override EventBookmark Bookmark { get { throw null; } }

        public string ContainerLog { get { throw null; } }

        public override int Id { get { throw null; } }

        public override long? Keywords { get { throw null; } }

        public override Collections.Generic.IEnumerable<string> KeywordsDisplayNames { get { throw null; } }

        public override byte? Level { get { throw null; } }

        public override string LevelDisplayName { get { throw null; } }

        public override string LogName { get { throw null; } }

        public override string MachineName { get { throw null; } }

        public Collections.Generic.IEnumerable<int> MatchedQueryIds { get { throw null; } }

        public override short? Opcode { get { throw null; } }

        public override string OpcodeDisplayName { get { throw null; } }

        public override int? ProcessId { get { throw null; } }

        public override Collections.Generic.IList<EventProperty> Properties { get { throw null; } }

        public override Guid? ProviderId { get { throw null; } }

        public override string ProviderName { get { throw null; } }

        public override int? Qualifiers { get { throw null; } }

        public override long? RecordId { get { throw null; } }

        public override Guid? RelatedActivityId { get { throw null; } }

        public override int? Task { get { throw null; } }

        public override string TaskDisplayName { get { throw null; } }

        public override int? ThreadId { get { throw null; } }

        public override DateTime? TimeCreated { get { throw null; } }

        public override Security.Principal.SecurityIdentifier UserId { get { throw null; } }

        public override byte? Version { get { throw null; } }

        protected override void Dispose(bool disposing) { }

        public override string FormatDescription() { throw null; }

        public override string FormatDescription(Collections.Generic.IEnumerable<object> values) { throw null; }

        public Collections.Generic.IList<object> GetPropertyValues(EventLogPropertySelector propertySelector) { throw null; }

        public override string ToXml() { throw null; }
    }

    public partial class EventLogSession : IDisposable
    {
        public EventLogSession() { }

        public EventLogSession(string server, string domain, string user, Security.SecureString password, SessionAuthentication logOnType) { }

        public EventLogSession(string server) { }

        public static EventLogSession GlobalSession { get { throw null; } }

        public void CancelCurrentOperations() { }

        public void ClearLog(string logName, string backupPath) { }

        public void ClearLog(string logName) { }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        public void ExportLog(string path, PathType pathType, string query, string targetFilePath, bool tolerateQueryErrors) { }

        public void ExportLog(string path, PathType pathType, string query, string targetFilePath) { }

        public void ExportLogAndMessages(string path, PathType pathType, string query, string targetFilePath, bool tolerateQueryErrors, Globalization.CultureInfo targetCultureInfo) { }

        public void ExportLogAndMessages(string path, PathType pathType, string query, string targetFilePath) { }

        public EventLogInformation GetLogInformation(string logName, PathType pathType) { throw null; }

        public Collections.Generic.IEnumerable<string> GetLogNames() { throw null; }

        public Collections.Generic.IEnumerable<string> GetProviderNames() { throw null; }
    }

    public sealed partial class EventLogStatus
    {
        internal EventLogStatus() { }

        public string LogName { get { throw null; } }

        public int StatusCode { get { throw null; } }
    }

    public enum EventLogType
    {
        Administrative = 0,
        Operational = 1,
        Analytical = 2,
        Debug = 3
    }

    public partial class EventLogWatcher : IDisposable
    {
        public EventLogWatcher(EventLogQuery eventQuery, EventBookmark bookmark, bool readExistingEvents) { }

        public EventLogWatcher(EventLogQuery eventQuery, EventBookmark bookmark) { }

        public EventLogWatcher(EventLogQuery eventQuery) { }

        public EventLogWatcher(string path) { }

        public bool Enabled { get { throw null; } set { } }

        public event EventHandler<EventRecordWrittenEventArgs> EventRecordWritten { add { } remove { } }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }
    }

    public sealed partial class EventMetadata
    {
        internal EventMetadata() { }

        public string Description { get { throw null; } }

        public long Id { get { throw null; } }

        public Collections.Generic.IEnumerable<EventKeyword> Keywords { get { throw null; } }

        public EventLevel Level { get { throw null; } }

        public EventLogLink LogLink { get { throw null; } }

        public EventOpcode Opcode { get { throw null; } }

        public EventTask Task { get { throw null; } }

        public string Template { get { throw null; } }

        public byte Version { get { throw null; } }
    }

    public sealed partial class EventOpcode
    {
        internal EventOpcode() { }

        public string DisplayName { get { throw null; } }

        public string Name { get { throw null; } }

        public int Value { get { throw null; } }
    }

    public sealed partial class EventProperty
    {
        internal EventProperty() { }

        public object Value { get { throw null; } }
    }

    public abstract partial class EventRecord : IDisposable
    {
        public abstract Guid? ActivityId { get; }
        public abstract EventBookmark Bookmark { get; }
        public abstract int Id { get; }
        public abstract long? Keywords { get; }
        public abstract Collections.Generic.IEnumerable<string> KeywordsDisplayNames { get; }
        public abstract byte? Level { get; }
        public abstract string LevelDisplayName { get; }
        public abstract string LogName { get; }
        public abstract string MachineName { get; }
        public abstract short? Opcode { get; }
        public abstract string OpcodeDisplayName { get; }
        public abstract int? ProcessId { get; }
        public abstract Collections.Generic.IList<EventProperty> Properties { get; }
        public abstract Guid? ProviderId { get; }
        public abstract string ProviderName { get; }
        public abstract int? Qualifiers { get; }
        public abstract long? RecordId { get; }
        public abstract Guid? RelatedActivityId { get; }
        public abstract int? Task { get; }
        public abstract string TaskDisplayName { get; }
        public abstract int? ThreadId { get; }
        public abstract DateTime? TimeCreated { get; }
        public abstract Security.Principal.SecurityIdentifier UserId { get; }
        public abstract byte? Version { get; }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        public abstract string FormatDescription();
        public abstract string FormatDescription(Collections.Generic.IEnumerable<object> values);
        public abstract string ToXml();
    }

    public sealed partial class EventRecordWrittenEventArgs : EventArgs
    {
        internal EventRecordWrittenEventArgs() { }

        public Exception EventException { get { throw null; } }

        public EventRecord EventRecord { get { throw null; } }
    }

    public sealed partial class EventTask
    {
        internal EventTask() { }

        public string DisplayName { get { throw null; } }

        public Guid EventGuid { get { throw null; } }

        public string Name { get { throw null; } }

        public int Value { get { throw null; } }
    }

    public enum PathType
    {
        LogName = 1,
        FilePath = 2
    }

    public partial class ProviderMetadata : IDisposable
    {
        public ProviderMetadata(string providerName, EventLogSession session, Globalization.CultureInfo targetCultureInfo) { }

        public ProviderMetadata(string providerName) { }

        public string DisplayName { get { throw null; } }

        public Collections.Generic.IEnumerable<EventMetadata> Events { get { throw null; } }

        public Uri HelpLink { get { throw null; } }

        public Guid Id { get { throw null; } }

        public Collections.Generic.IList<EventKeyword> Keywords { get { throw null; } }

        public Collections.Generic.IList<EventLevel> Levels { get { throw null; } }

        public Collections.Generic.IList<EventLogLink> LogLinks { get { throw null; } }

        public string MessageFilePath { get { throw null; } }

        public string Name { get { throw null; } }

        public Collections.Generic.IList<EventOpcode> Opcodes { get { throw null; } }

        public string ParameterFilePath { get { throw null; } }

        public string ResourceFilePath { get { throw null; } }

        public Collections.Generic.IList<EventTask> Tasks { get { throw null; } }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }
    }

    public enum SessionAuthentication
    {
        Default = 0,
        Negotiate = 1,
        Kerberos = 2,
        Ntlm = 3
    }

    [Flags]
    public enum StandardEventKeywords : long
    {
        AuditFailure = 4503599627370496L,
        AuditSuccess = 9007199254740992L,
        CorrelationHint = 4503599627370496L,
        CorrelationHint2 = 18014398509481984L,
        EventLogClassic = 36028797018963968L,
        None = 0L,
        ResponseTime = 281474976710656L,
        Sqm = 2251799813685248L,
        WdiContext = 562949953421312L,
        WdiDiagnostic = 1125899906842624L
    }

    public enum StandardEventLevel
    {
        LogAlways = 0,
        Critical = 1,
        Error = 2,
        Warning = 3,
        Informational = 4,
        Verbose = 5
    }

    public enum StandardEventOpcode
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

    public enum StandardEventTask
    {
        None = 0
    }
}
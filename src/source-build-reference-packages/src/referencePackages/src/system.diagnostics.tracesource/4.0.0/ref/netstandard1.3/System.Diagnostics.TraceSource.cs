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
[assembly: System.Reflection.AssemblyTitle("System.Diagnostics.TraceSource")]
[assembly: System.Reflection.AssemblyDescription("System.Diagnostics.TraceSource")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Diagnostics.TraceSource")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("1.0.24212.01")]
[assembly: System.Reflection.AssemblyInformationalVersion("1.0.24212.01. Commit Hash: 9688ddbb62c04189cac4c4a06e31e93377dccd41")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.0.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Diagnostics
{
    public partial class BooleanSwitch : Switch
    {
        public BooleanSwitch(string displayName, string description, string defaultSwitchValue) : base(default!, default!) { }

        public BooleanSwitch(string displayName, string description) : base(default!, default!) { }

        public bool Enabled { get { throw null; } set { } }

        protected override void OnValueChanged() { }
    }

    public partial class DefaultTraceListener : TraceListener
    {
        public override void Fail(string message, string detailMessage) { }

        public override void Fail(string message) { }

        public override void Write(string message) { }

        public override void WriteLine(string message) { }
    }

    public partial class EventTypeFilter : TraceFilter
    {
        public EventTypeFilter(SourceLevels level) { }

        public SourceLevels EventType { get { throw null; } set { } }

        public override bool ShouldTrace(TraceEventCache cache, string source, TraceEventType eventType, int id, string formatOrMessage, object[] args, object data1, object[] data) { throw null; }
    }

    public partial class SourceFilter : TraceFilter
    {
        public SourceFilter(string source) { }

        public string Source { get { throw null; } set { } }

        public override bool ShouldTrace(TraceEventCache cache, string source, TraceEventType eventType, int id, string formatOrMessage, object[] args, object data1, object[] data) { throw null; }
    }

    [Flags]
    public enum SourceLevels
    {
        All = -1,
        Off = 0,
        Critical = 1,
        Error = 3,
        Warning = 7,
        Information = 15,
        Verbose = 31
    }

    public partial class SourceSwitch : Switch
    {
        public SourceSwitch(string displayName, string defaultSwitchValue) : base(default!, default!) { }

        public SourceSwitch(string name) : base(default!, default!) { }

        public SourceLevels Level { get { throw null; } set { } }

        protected override void OnValueChanged() { }

        public bool ShouldTrace(TraceEventType eventType) { throw null; }
    }

    public abstract partial class Switch
    {
        protected Switch(string displayName, string description, string defaultSwitchValue) { }

        protected Switch(string displayName, string description) { }

        public string Description { get { throw null; } }

        public string DisplayName { get { throw null; } }

        protected int SwitchSetting { get { throw null; } set { } }

        protected string Value { get { throw null; } set { } }

        protected virtual void OnSwitchSettingChanged() { }

        protected virtual void OnValueChanged() { }
    }

    public sealed partial class Trace
    {
        internal Trace() { }

        public static bool AutoFlush { get { throw null; } set { } }

        public static int IndentLevel { get { throw null; } set { } }

        public static int IndentSize { get { throw null; } set { } }

        public static TraceListenerCollection Listeners { get { throw null; } }

        public static bool UseGlobalLock { get { throw null; } set { } }

        public static void Assert(bool condition, string message, string detailMessage) { }

        public static void Assert(bool condition, string message) { }

        public static void Assert(bool condition) { }

        public static void Close() { }

        public static void Fail(string message, string detailMessage) { }

        public static void Fail(string message) { }

        public static void Flush() { }

        public static void Indent() { }

        public static void Refresh() { }

        public static void TraceError(string format, params object[] args) { }

        public static void TraceError(string message) { }

        public static void TraceInformation(string format, params object[] args) { }

        public static void TraceInformation(string message) { }

        public static void TraceWarning(string format, params object[] args) { }

        public static void TraceWarning(string message) { }

        public static void Unindent() { }

        public static void Write(object value, string category) { }

        public static void Write(object value) { }

        public static void Write(string message, string category) { }

        public static void Write(string message) { }

        public static void WriteIf(bool condition, object value, string category) { }

        public static void WriteIf(bool condition, object value) { }

        public static void WriteIf(bool condition, string message, string category) { }

        public static void WriteIf(bool condition, string message) { }

        public static void WriteLine(object value, string category) { }

        public static void WriteLine(object value) { }

        public static void WriteLine(string message, string category) { }

        public static void WriteLine(string message) { }

        public static void WriteLineIf(bool condition, object value, string category) { }

        public static void WriteLineIf(bool condition, object value) { }

        public static void WriteLineIf(bool condition, string message, string category) { }

        public static void WriteLineIf(bool condition, string message) { }
    }

    public partial class TraceEventCache
    {
        public DateTime DateTime { get { throw null; } }

        public int ProcessId { get { throw null; } }

        public string ThreadId { get { throw null; } }

        public long Timestamp { get { throw null; } }
    }

    public enum TraceEventType
    {
        Critical = 1,
        Error = 2,
        Warning = 4,
        Information = 8,
        Verbose = 16
    }

    public abstract partial class TraceFilter
    {
        public abstract bool ShouldTrace(TraceEventCache cache, string source, TraceEventType eventType, int id, string formatOrMessage, object[] args, object data1, object[] data);
    }

    public enum TraceLevel
    {
        Off = 0,
        Error = 1,
        Warning = 2,
        Info = 3,
        Verbose = 4
    }

    public abstract partial class TraceListener : IDisposable
    {
        protected TraceListener() { }

        protected TraceListener(string name) { }

        public TraceFilter Filter { get { throw null; } set { } }

        public int IndentLevel { get { throw null; } set { } }

        public int IndentSize { get { throw null; } set { } }

        public virtual bool IsThreadSafe { get { throw null; } }

        public virtual string Name { get { throw null; } set { } }

        protected bool NeedIndent { get { throw null; } set { } }

        public TraceOptions TraceOutputOptions { get { throw null; } set { } }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        public virtual void Fail(string message, string detailMessage) { }

        public virtual void Fail(string message) { }

        public virtual void Flush() { }

        public virtual void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data) { }

        public virtual void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data) { }

        public virtual void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args) { }

        public virtual void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message) { }

        public virtual void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id) { }

        public virtual void Write(object o, string category) { }

        public virtual void Write(object o) { }

        public virtual void Write(string message, string category) { }

        public abstract void Write(string message);
        protected virtual void WriteIndent() { }

        public virtual void WriteLine(object o, string category) { }

        public virtual void WriteLine(object o) { }

        public virtual void WriteLine(string message, string category) { }

        public abstract void WriteLine(string message);
    }

    public partial class TraceListenerCollection : Collections.ICollection, Collections.IEnumerable, Collections.IList
    {
        internal TraceListenerCollection() { }

        public int Count { get { throw null; } }

        public TraceListener this[int i] { get { throw null; } set { } }

        public TraceListener this[string name] { get { throw null; } }

        bool Collections.ICollection.IsSynchronized { get { throw null; } }

        object Collections.ICollection.SyncRoot { get { throw null; } }

        bool Collections.IList.IsFixedSize { get { throw null; } }

        bool Collections.IList.IsReadOnly { get { throw null; } }

        object System.Collections.IList.this[int index] { get { throw null; } set { } }

        public int Add(TraceListener listener) { throw null; }

        public void AddRange(TraceListener[] value) { }

        public void AddRange(TraceListenerCollection value) { }

        public void Clear() { }

        public bool Contains(TraceListener listener) { throw null; }

        public void CopyTo(TraceListener[] listeners, int index) { }

        public Collections.IEnumerator GetEnumerator() { throw null; }

        public int IndexOf(TraceListener listener) { throw null; }

        public void Insert(int index, TraceListener listener) { }

        public void Remove(TraceListener listener) { }

        public void Remove(string name) { }

        public void RemoveAt(int index) { }

        void Collections.ICollection.CopyTo(Array array, int index) { }

        int Collections.IList.Add(object value) { throw null; }

        bool Collections.IList.Contains(object value) { throw null; }

        int Collections.IList.IndexOf(object value) { throw null; }

        void Collections.IList.Insert(int index, object value) { }

        void Collections.IList.Remove(object value) { }
    }

    [Flags]
    public enum TraceOptions
    {
        None = 0,
        DateTime = 2,
        Timestamp = 4,
        ProcessId = 8,
        ThreadId = 16
    }

    public partial class TraceSource
    {
        public TraceSource(string name, SourceLevels defaultLevel) { }

        public TraceSource(string name) { }

        public TraceListenerCollection Listeners { get { throw null; } }

        public string Name { get { throw null; } }

        public SourceSwitch Switch { get { throw null; } set { } }

        public void Close() { }

        public void Flush() { }

        public void TraceData(TraceEventType eventType, int id, object data) { }

        public void TraceData(TraceEventType eventType, int id, params object[] data) { }

        public void TraceEvent(TraceEventType eventType, int id, string format, params object[] args) { }

        public void TraceEvent(TraceEventType eventType, int id, string message) { }

        public void TraceEvent(TraceEventType eventType, int id) { }

        public void TraceInformation(string format, params object[] args) { }

        public void TraceInformation(string message) { }
    }

    public partial class TraceSwitch : Switch
    {
        public TraceSwitch(string displayName, string description, string defaultSwitchValue) : base(default!, default!) { }

        public TraceSwitch(string displayName, string description) : base(default!, default!) { }

        public TraceLevel Level { get { throw null; } set { } }

        public bool TraceError { get { throw null; } }

        public bool TraceInfo { get { throw null; } }

        public bool TraceVerbose { get { throw null; } }

        public bool TraceWarning { get { throw null; } }

        protected override void OnSwitchSettingChanged() { }

        protected override void OnValueChanged() { }
    }
}
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.Versioning.TargetFramework(".NETCoreApp,Version=v7.0", FrameworkDisplayName = ".NET 7.0")]
[assembly: System.Reflection.AssemblyMetadata("NotSupported", "True")]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.ServiceProcess.ServiceController")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata("IsTrimmable", "True")]
[assembly: System.Runtime.Versioning.SupportedOSPlatform("windows")]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.System32)]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Provides the System.ServiceProcess.ServiceContainer class, which allows you to connect to a running or stopped service, manipulate it, or get information about it.\r\n\r\nCommonly Used Types:\r\nSystem.ServiceProcess.ServiceController\r\nSystem.ServiceProcess.ServiceControllerStatus\r\nSystem.ServiceProcess.ServiceType")]
[assembly: System.Reflection.AssemblyFileVersion("7.0.22.51805")]
[assembly: System.Reflection.AssemblyInformationalVersion("7.0.0+d099f075e45d2aa6007a22b71b45a08758559f80")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("System.ServiceProcess.ServiceController")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/runtime")]
[assembly: System.Reflection.AssemblyVersionAttribute("7.0.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.ServiceProcess
{
    public enum PowerBroadcastStatus
    {
        QuerySuspend = 0,
        QuerySuspendFailed = 2,
        Suspend = 4,
        ResumeCritical = 6,
        ResumeSuspend = 7,
        BatteryLow = 9,
        PowerStatusChange = 10,
        OemEvent = 11,
        ResumeAutomatic = 18
    }

    public partial class ServiceBase : ComponentModel.Component
    {
        public const int MaxNameLength = 80;
        public ServiceBase() { }

        [ComponentModel.DefaultValue(true)]
        public bool AutoLog { get { throw null; } set { } }

        [ComponentModel.DefaultValue(false)]
        public bool CanHandlePowerEvent { get { throw null; } set { } }

        [ComponentModel.DefaultValue(false)]
        public bool CanHandleSessionChangeEvent { get { throw null; } set { } }

        [ComponentModel.DefaultValue(false)]
        public bool CanPauseAndContinue { get { throw null; } set { } }

        [ComponentModel.DefaultValue(false)]
        public bool CanShutdown { get { throw null; } set { } }

        [ComponentModel.DefaultValue(true)]
        public bool CanStop { get { throw null; } set { } }

        [ComponentModel.Browsable(false)]
        [ComponentModel.DesignerSerializationVisibility(ComponentModel.DesignerSerializationVisibility.Hidden)]
        public virtual Diagnostics.EventLog EventLog { get { throw null; } }

        public int ExitCode { get { throw null; } set { } }

        protected nint ServiceHandle { get { throw null; } }

        public string ServiceName { get { throw null; } set { } }

        protected override void Dispose(bool disposing) { }

        protected virtual void OnContinue() { }

        protected virtual void OnCustomCommand(int command) { }

        protected virtual void OnPause() { }

        protected virtual bool OnPowerEvent(PowerBroadcastStatus powerStatus) { throw null; }

        protected virtual void OnSessionChange(SessionChangeDescription changeDescription) { }

        protected virtual void OnShutdown() { }

        protected virtual void OnStart(string[] args) { }

        protected virtual void OnStop() { }

        public void RequestAdditionalTime(int milliseconds) { }

        public void RequestAdditionalTime(TimeSpan time) { }

        public static void Run(ServiceBase service) { }

        public static void Run(ServiceBase[] services) { }

        public void ServiceMainCallback(int argCount, nint argPointer) { }

        public void Stop() { }
    }

    public partial class ServiceController : ComponentModel.Component
    {
        public ServiceController() { }

        public ServiceController(string name, string machineName) { }

        public ServiceController(string name) { }

        public bool CanPauseAndContinue { get { throw null; } }

        public bool CanShutdown { get { throw null; } }

        public bool CanStop { get { throw null; } }

        public ServiceController[] DependentServices { get { throw null; } }

        public string DisplayName { get { throw null; } set { } }

        public string MachineName { get { throw null; } set { } }

        public Runtime.InteropServices.SafeHandle ServiceHandle { get { throw null; } }

        public string ServiceName { get { throw null; } set { } }

        public ServiceController[] ServicesDependedOn { get { throw null; } }

        public ServiceType ServiceType { get { throw null; } }

        public ServiceStartMode StartType { get { throw null; } }

        public ServiceControllerStatus Status { get { throw null; } }

        public void Close() { }

        public void Continue() { }

        protected override void Dispose(bool disposing) { }

        public void ExecuteCommand(int command) { }

        public static ServiceController[] GetDevices() { throw null; }

        public static ServiceController[] GetDevices(string machineName) { throw null; }

        public static ServiceController[] GetServices() { throw null; }

        public static ServiceController[] GetServices(string machineName) { throw null; }

        public void Pause() { }

        public void Refresh() { }

        public void Start() { }

        public void Start(string[] args) { }

        public void Stop() { }

        public void Stop(bool stopDependentServices) { }

        public void WaitForStatus(ServiceControllerStatus desiredStatus, TimeSpan timeout) { }

        public void WaitForStatus(ServiceControllerStatus desiredStatus) { }
    }

    public enum ServiceControllerStatus
    {
        Stopped = 1,
        StartPending = 2,
        StopPending = 3,
        Running = 4,
        ContinuePending = 5,
        PausePending = 6,
        Paused = 7
    }

    [AttributeUsage(AttributeTargets.All)]
    public partial class ServiceProcessDescriptionAttribute : ComponentModel.DescriptionAttribute
    {
        public ServiceProcessDescriptionAttribute(string description) { }

        public override string Description { get { throw null; } }
    }

    public enum ServiceStartMode
    {
        Boot = 0,
        System = 1,
        Automatic = 2,
        Manual = 3,
        Disabled = 4
    }

    [Flags]
    public enum ServiceType
    {
        KernelDriver = 1,
        FileSystemDriver = 2,
        Adapter = 4,
        RecognizerDriver = 8,
        Win32OwnProcess = 16,
        Win32ShareProcess = 32,
        InteractiveProcess = 256
    }

    public readonly partial struct SessionChangeDescription : IEquatable<SessionChangeDescription>
    {
        private readonly int _dummyPrimitive;
        public SessionChangeReason Reason { get { throw null; } }

        public int SessionId { get { throw null; } }

        public override readonly bool Equals(object? obj) { throw null; }

        public readonly bool Equals(SessionChangeDescription changeDescription) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(SessionChangeDescription a, SessionChangeDescription b) { throw null; }

        public static bool operator !=(SessionChangeDescription a, SessionChangeDescription b) { throw null; }
    }

    public enum SessionChangeReason
    {
        ConsoleConnect = 1,
        ConsoleDisconnect = 2,
        RemoteConnect = 3,
        RemoteDisconnect = 4,
        SessionLogon = 5,
        SessionLogoff = 6,
        SessionLock = 7,
        SessionUnlock = 8,
        SessionRemoteControl = 9
    }

    public partial class TimeoutException : SystemException
    {
        public TimeoutException() { }

        protected TimeoutException(Runtime.Serialization.SerializationInfo info, Runtime.Serialization.StreamingContext context) { }

        public TimeoutException(string? message, Exception? innerException) { }

        public TimeoutException(string? message) { }
    }
}
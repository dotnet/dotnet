// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Reflection.AssemblyDefaultAlias("Microsoft.Win32.SystemEvents")]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Microsoft.Win32.SystemEvents")]
[assembly: System.Reflection.AssemblyFileVersion("4.700.19.56404")]
[assembly: System.Reflection.AssemblyInformationalVersion("3.1.0+0f7f38c4fd323b26da10cce95f857f77f0f09b48")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Core")]
[assembly: System.Reflection.AssemblyTitle("Microsoft.Win32.SystemEvents")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.2.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace Microsoft.Win32
{
    public partial class PowerModeChangedEventArgs : System.EventArgs
    {
        public PowerModeChangedEventArgs(PowerModes mode) { }

        public PowerModes Mode { get { throw null; } }
    }

    public delegate void PowerModeChangedEventHandler(object sender, PowerModeChangedEventArgs e);
    public enum PowerModes
    {
        Resume = 1,
        StatusChange = 2,
        Suspend = 3
    }

    public partial class SessionEndedEventArgs : System.EventArgs
    {
        public SessionEndedEventArgs(SessionEndReasons reason) { }

        public SessionEndReasons Reason { get { throw null; } }
    }

    public delegate void SessionEndedEventHandler(object sender, SessionEndedEventArgs e);
    public partial class SessionEndingEventArgs : System.EventArgs
    {
        public SessionEndingEventArgs(SessionEndReasons reason) { }

        public bool Cancel { get { throw null; } set { } }

        public SessionEndReasons Reason { get { throw null; } }
    }

    public delegate void SessionEndingEventHandler(object sender, SessionEndingEventArgs e);
    public enum SessionEndReasons
    {
        Logoff = 1,
        SystemShutdown = 2
    }

    public partial class SessionSwitchEventArgs : System.EventArgs
    {
        public SessionSwitchEventArgs(SessionSwitchReason reason) { }

        public SessionSwitchReason Reason { get { throw null; } }
    }

    public delegate void SessionSwitchEventHandler(object sender, SessionSwitchEventArgs e);
    public enum SessionSwitchReason
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

    public sealed partial class SystemEvents
    {
        internal SystemEvents() { }

        public static event System.EventHandler DisplaySettingsChanged { add { } remove { } }

        public static event System.EventHandler DisplaySettingsChanging { add { } remove { } }

        public static event System.EventHandler EventsThreadShutdown { add { } remove { } }

        public static event System.EventHandler InstalledFontsChanged { add { } remove { } }

        [System.ComponentModel.Browsable(false)]
        [System.Obsolete("This event has been deprecated. https://go.microsoft.com/fwlink/?linkid=14202")]
        public static event System.EventHandler LowMemory { add { } remove { } }

        public static event System.EventHandler PaletteChanged { add { } remove { } }

        public static event PowerModeChangedEventHandler PowerModeChanged { add { } remove { } }

        public static event SessionEndedEventHandler SessionEnded { add { } remove { } }

        public static event SessionEndingEventHandler SessionEnding { add { } remove { } }

        public static event SessionSwitchEventHandler SessionSwitch { add { } remove { } }

        public static event System.EventHandler TimeChanged { add { } remove { } }

        public static event TimerElapsedEventHandler TimerElapsed { add { } remove { } }

        public static event UserPreferenceChangedEventHandler UserPreferenceChanged { add { } remove { } }

        public static event UserPreferenceChangingEventHandler UserPreferenceChanging { add { } remove { } }

        public static System.IntPtr CreateTimer(int interval) { throw null; }

        public static void InvokeOnEventsThread(System.Delegate method) { }

        public static void KillTimer(System.IntPtr timerId) { }
    }

    public partial class TimerElapsedEventArgs : System.EventArgs
    {
        public TimerElapsedEventArgs(System.IntPtr timerId) { }

        public System.IntPtr TimerId { get { throw null; } }
    }

    public delegate void TimerElapsedEventHandler(object sender, TimerElapsedEventArgs e);
    public enum UserPreferenceCategory
    {
        Accessibility = 1,
        Color = 2,
        Desktop = 3,
        General = 4,
        Icon = 5,
        Keyboard = 6,
        Menu = 7,
        Mouse = 8,
        Policy = 9,
        Power = 10,
        Screensaver = 11,
        Window = 12,
        Locale = 13,
        VisualStyle = 14
    }

    public partial class UserPreferenceChangedEventArgs : System.EventArgs
    {
        public UserPreferenceChangedEventArgs(UserPreferenceCategory category) { }

        public UserPreferenceCategory Category { get { throw null; } }
    }

    public delegate void UserPreferenceChangedEventHandler(object sender, UserPreferenceChangedEventArgs e);
    public partial class UserPreferenceChangingEventArgs : System.EventArgs
    {
        public UserPreferenceChangingEventArgs(UserPreferenceCategory category) { }

        public UserPreferenceCategory Category { get { throw null; } }
    }

    public delegate void UserPreferenceChangingEventHandler(object sender, UserPreferenceChangingEventArgs e);
}
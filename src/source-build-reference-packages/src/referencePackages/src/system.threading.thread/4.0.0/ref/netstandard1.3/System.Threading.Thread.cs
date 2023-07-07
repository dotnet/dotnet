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
[assembly: System.Reflection.AssemblyTitle("System.Threading.Thread")]
[assembly: System.Reflection.AssemblyDescription("System.Threading.Thread")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Threading.Thread")]
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
namespace System.Threading
{
    public delegate void ParameterizedThreadStart(object obj);
    public sealed partial class Thread
    {
        public Thread(ParameterizedThreadStart start) { }

        public Thread(ThreadStart start) { }

        public static Thread CurrentThread { get { throw null; } }

        public bool IsAlive { get { throw null; } }

        public bool IsBackground { get { throw null; } set { } }

        public int ManagedThreadId { get { throw null; } }

        public string Name { get { throw null; } set { } }

        public ThreadState ThreadState { get { throw null; } }

        public void Join() { }

        public bool Join(int millisecondsTimeout) { throw null; }

        public static void Sleep(int millisecondsTimeout) { }

        public static void Sleep(TimeSpan timeout) { }

        public void Start() { }

        public void Start(object parameter) { }
    }

    public delegate void ThreadStart();
    public sealed partial class ThreadStartException : Exception
    {
        internal ThreadStartException() { }
    }

    [Flags]
    public enum ThreadState
    {
        Running = 0,
        StopRequested = 1,
        SuspendRequested = 2,
        Background = 4,
        Unstarted = 8,
        Stopped = 16,
        WaitSleepJoin = 32,
        Suspended = 64,
        AbortRequested = 128,
        Aborted = 256
    }

    public partial class ThreadStateException : Exception
    {
        public ThreadStateException() { }

        public ThreadStateException(string message, Exception innerException) { }

        public ThreadStateException(string message) { }
    }
}
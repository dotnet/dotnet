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
[assembly: AssemblyTitle("System.Threading.Thread")]
[assembly: AssemblyDescription("System.Threading.Thread")]
[assembly: AssemblyDefaultAlias("System.Threading.Thread")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("1.0.24212.01")]
[assembly: AssemblyInformationalVersion("1.0.24212.01 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("4.0.0.0")]




namespace System.Threading
{
    public delegate void ParameterizedThreadStart(object obj);
    public sealed partial class Thread
    {
        public Thread(System.Threading.ParameterizedThreadStart start) { }
        public Thread(System.Threading.ThreadStart start) { }
        public static System.Threading.Thread CurrentThread { get { throw null; } }
        public bool IsAlive { get { throw null; } }
        public bool IsBackground { get { throw null; } set { } }
        public int ManagedThreadId { get { throw null; } }
        public string Name { get { throw null; } set { } }
        public System.Threading.ThreadState ThreadState { get { throw null; } }
        public void Join() { }
        public bool Join(int millisecondsTimeout) { throw null; }
        public static void Sleep(int millisecondsTimeout) { }
        public static void Sleep(System.TimeSpan timeout) { }
        public void Start() { }
        public void Start(object parameter) { }
    }
    public delegate void ThreadStart();
    public sealed partial class ThreadStartException : System.Exception
    {
        internal ThreadStartException() { }
    }
    [System.FlagsAttribute]
    public enum ThreadState
    {
        Aborted = 256,
        AbortRequested = 128,
        Background = 4,
        Running = 0,
        Stopped = 16,
        StopRequested = 1,
        Suspended = 64,
        SuspendRequested = 2,
        Unstarted = 8,
        WaitSleepJoin = 32,
    }
    public partial class ThreadStateException : System.Exception
    {
        public ThreadStateException() { }
        public ThreadStateException(string message) { }
        public ThreadStateException(string message, System.Exception innerException) { }
    }
}

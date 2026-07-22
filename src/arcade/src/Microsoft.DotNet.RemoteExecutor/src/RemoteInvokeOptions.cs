// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Microsoft.DotNet.RemoteExecutor
{
    /// <summary>
    /// The type of crash dump to collect. Maps to DOTNET_DbgMiniDumpType values
    /// as documented in <see href="https://learn.microsoft.com/dotnet/core/diagnostics/collect-dumps-crash#types-of-mini-dumps">the docs</see>. Only applies to .NET Core subprocesses.
    /// </summary>
    public enum CrashDumpCollectionType
    {
        /// <summary>
        /// Explicitly disables crash dump collection, removing any inherited DOTNET_DbgEnableMiniDump,
        /// DOTNET_DbgMiniDumpType, and DOTNET_DbgMiniDumpName environment variables from the subprocess.
        /// </summary>
        None = 0,
        /// <summary>
        /// A small dump containing module lists, thread lists, exception information, and all stacks.
        /// </summary>
        Mini = 1,
        /// <summary>
        /// A large and relatively comprehensive dump containing module lists, thread lists, all stacks,
        /// exception information, handle information, and all memory except for mapped images.
        /// </summary>
        Heap = 2,
        /// <summary>
        /// Same as <see cref="Mini"/>, but removes personal user information, such as paths and passwords.
        /// </summary>
        Triage = 3,
        /// <summary>
        /// The largest dump containing all memory including the module images.
        /// </summary>
        Full = 4
    }

    /// <summary>
    /// Options used with RemoteInvoke.
    /// </summary>
    public sealed class RemoteInvokeOptions
    {
        private bool _runAsSudo;

        public bool Start { get; set; } = true;

        public ProcessStartInfo StartInfo { get; set; } = new ProcessStartInfo();

        public bool EnableProfiling { get; set; } = true;

        public bool EnableTimeoutDumpCollection { get; set; } = true;

        public bool CheckExitCode { get; set; } = true;

        /// <summary>
        /// A timeout (milliseconds) after which a wait on a remote operation should be considered a failure.
        /// </summary>
        public int TimeOut { get; set; } = RemoteExecutor.FailWaitTimeoutMilliseconds;

        /// <summary>
        /// The exit code returned when the test process exits successfully.
        /// </summary>
        public int ExpectedExitCode { get; set; } = RemoteExecutor.SuccessExitCode;

        public string ExceptionFile { get; } = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

        /// <summary>
        /// Gets the runtimeconfig options (or AppContext settings) to set for the remote process.
        /// </summary>
        /// <remarks>
        /// This option only works with .NET Core processes.
        /// </remarks>
        public IDictionary<string, object> RuntimeConfigurationOptions { get; } = new Dictionary<string, object>();

        public bool RunAsSudo
        {
            get => _runAsSudo;
            set
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    throw new PlatformNotSupportedException();
                }

                _runAsSudo = value;
            }
        }

        /// <summary>
        /// Specifies the roll-forward policy for dotnet cli to use. Only applies when running .NET Core
        /// </summary>
        public string RollForward { get; set; }

        /// <summary>
        /// Gets or sets the type of crash dump to collect on the subprocess via
        /// DOTNET_DbgEnableMiniDump / DOTNET_DbgMiniDumpType / DOTNET_DbgMiniDumpName.
        /// When set to a value other than <see cref="CrashDumpCollectionType.None"/>,
        /// crash dump collection is enabled with that dump type.
        /// When set to <see cref="CrashDumpCollectionType.None"/>, crash dump collection is
        /// explicitly disabled (removing any inherited env vars).
        /// When null (default), the environment variables are left as-is.
        /// </summary>
        /// <remarks>
        /// Only applies to .NET Core subprocesses. When timeout dump collection is enabled,
        /// values other than <see cref="CrashDumpCollectionType.None"/> also select the
        /// timeout dump type. <see cref="CrashDumpCollectionType.None"/> only disables
        /// crash-time environment variable configuration; use <see cref="EnableTimeoutDumpCollection"/>
        /// to disable timeout diagnostics.
        /// </remarks>
        public CrashDumpCollectionType? CrashDumpCollectionType { get; set; }

        /// <summary>
        /// Gets or sets the path template for crash dump files. When <see cref="CrashDumpCollectionType"/> is set,
        /// this value is used for DOTNET_DbgMiniDumpName. Supports the same placeholders as createdump:
        /// %p (PID), %e (process name), %t (timestamp), etc.
        /// When null (default), defaults to HELIX_WORKITEM_UPLOAD_ROOT/%e.%p.%t.dmp if running in Helix,
        /// or the system temp directory otherwise.
        /// </summary>
        public string CrashDumpPath { get; set; }
    }
}

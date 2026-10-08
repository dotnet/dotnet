// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
#if !NETSTANDARD1_3 && !NETSTANDARD1_4 && !NETSTANDARD1_5 && !NETSTANDARD1_6
using System.Diagnostics;
#endif
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

#pragma warning disable IDE0161 // Linked into servicing branches whose compilers predate file-scoped namespaces.
namespace Microsoft.VisualStudio.TestPlatform.Utilities
{
    /// <summary>
    /// What could be learned about the process that started us, for the details printed under a
    /// connection negotiation failure.
    /// </summary>
    internal sealed class ParentProcessDetails
    {
        internal int ProcessId { get; set; }

        /// <summary>Only used when <see cref="Path"/> could not be read.</summary>
        internal string Name { get; set; } = string.Empty;

        internal string Path { get; set; } = string.Empty;

        internal string Version { get; set; } = string.Empty;

        /// <summary>
        /// Set when the parent turns out to host vstest.console rather than be it, which is what
        /// <c>dotnet test</c> does. This is the line that names the SDK to update.
        /// </summary>
        internal string VsTestConsolePath { get; set; } = string.Empty;

        internal string VsTestConsoleVersion { get; set; } = string.Empty;
    }

    /// <summary>
    /// Identifies the process that started this one, so a negotiation failure can name the component
    /// that is most likely outdated instead of guessing at it.
    ///
    /// Everything here is best effort. A field that cannot be read is left null and the corresponding
    /// line is omitted from the message; nothing about a failure or a timeout reaches the user, only
    /// the diagnostic log. This runs only on a path that is already failing, never on a healthy run.
    /// </summary>
    internal static class ParentProcessResolver
    {
        /// <summary>
        /// Reading another process's loader data can block if that process is suspended, mid-load, or
        /// being debugged. Measured cost is about 0.4 ms per module, so ~120 ms for a process with 300
        /// modules, but a loaded CI machine is far slower and losing the detail is worse than waiting.
        /// This sits well inside the 90 second connection timeout the other side is waiting on.
        /// </summary>
        private const int LookupBudgetMilliseconds = 10000;

        /// <summary>
        /// Identifies the parent from the <c>--parentprocessid</c> argument this process was started
        /// with. Every process that can report a negotiation failure receives it: testhost requires
        /// it, and vstest.console and datacollector both accept it.
        /// </summary>
        internal static ParentProcessDetails ResolveFromCommandLine()
            => Resolve(ReadParentProcessIdFromCommandLine());

        private static int ReadParentProcessIdFromCommandLine()
        {
#if !NETSTANDARD1_3 && !NETSTANDARD1_4 && !NETSTANDARD1_5 && !NETSTANDARD1_6
            try
            {
                var arguments = Environment.GetCommandLineArgs();
                for (var i = 0; i < arguments.Length; i++)
                {
                    var argument = arguments[i];
                    if (argument.Length < 2 || (argument[0] != '-' && argument[0] != '/'))
                    {
                        continue;
                    }

                    var name = argument.TrimStart('-', '/');
                    var value = i + 1 < arguments.Length ? arguments[i + 1] : null;

                    // vstest.console spells it --ParentProcessId and also accepts /ParentProcessId,
                    // and either form may carry its value after a separator rather than as the next
                    // argument.
                    var separator = name.IndexOfAny(new[] { '=', ':' });
                    if (separator >= 0)
                    {
                        value = name.Substring(separator + 1);
                        name = name.Substring(0, separator);
                    }

                    if (string.Equals(name, "parentprocessid", StringComparison.OrdinalIgnoreCase)
                        && value != null
                        && int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parentProcessId))
                    {
                        return parentProcessId;
                    }
                }
            }
            catch (Exception ex)
            {
                EqtTrace.Verbose("ParentProcessResolver.ReadParentProcessIdFromCommandLine: Could not read the parent process id. {0}", ex);
            }

#endif
            return -1;
        }

        internal static ParentProcessDetails Resolve(int parentProcessId)
        {
            var details = new ParentProcessDetails { ProcessId = parentProcessId };
            if (parentProcessId <= 0)
            {
                return details;
            }

#if !NETSTANDARD1_3 && !NETSTANDARD1_4 && !NETSTANDARD1_5 && !NETSTANDARD1_6
            try
            {
                var lookup = Task.Run(() => Fill(details));
                if (!lookup.Wait(LookupBudgetMilliseconds))
                {
                    EqtTrace.Verbose(
                        "ParentProcessResolver.Resolve: Identifying parent process {0} timed out after {1} seconds, the details were omitted.",
                        parentProcessId,
                        LookupBudgetMilliseconds / 1000);
                }
            }
            catch (Exception ex)
            {
                EqtTrace.Verbose("ParentProcessResolver.Resolve: Could not identify parent process {0}. {1}", parentProcessId, ex);
            }
#endif

            return details;
        }

#if !NETSTANDARD1_3 && !NETSTANDARD1_4 && !NETSTANDARD1_5 && !NETSTANDARD1_6
        private static void Fill(ParentProcessDetails details)
        {
            Process parent;
            try
            {
                parent = Process.GetProcessById(details.ProcessId);
            }
            catch (Exception ex)
            {
                // ArgumentException when the process already exited, InvalidOperationException when it
                // exits between the lookup and the read.
                EqtTrace.Verbose("ParentProcessResolver.Fill: Parent process {0} could not be opened. {1}", details.ProcessId, ex);
                return;
            }

            try
            {
                details.Name = parent.ProcessName;
            }
            catch (Exception ex)
            {
                EqtTrace.Verbose("ParentProcessResolver.Fill: Parent process {0} name could not be read. {1}", details.ProcessId, ex);
            }

            try
            {
                // Win32Exception when a 32 bit process reads a 64 bit parent, or when access is denied.
                details.Path = parent.MainModule == null ? string.Empty : (parent.MainModule.FileName ?? string.Empty);
                if (!string.IsNullOrEmpty(details.Path))
                {
                    details.Version = ReadVersion(details.Path);

                    // The name only adds information when there is no path. On Windows it is the file
                    // name without its extension, and on Unix the kernel truncates it to 15 characters.
                    details.Name = string.Empty;
                }
            }
            catch (Exception ex)
            {
                EqtTrace.Verbose("ParentProcessResolver.Fill: Parent process {0} path could not be read. {1}", details.ProcessId, ex);
            }

            FillVsTestConsole(details, parent);
        }

        /// <summary>
        /// Under <c>dotnet test</c> the parent is the .NET host, whose version is the host version and
        /// says nothing about which vstest.console ran. The vstest.console it loaded does, and its path
        /// carries the SDK folder.
        /// </summary>
        private static void FillVsTestConsole(ParentProcessDetails details, Process parent)
        {
            var found = string.Empty;

            try
            {
                foreach (ProcessModule module in parent.Modules)
                {
                    var name = module.ModuleName;
                    if (name != null && name.StartsWith("vstest.console", StringComparison.OrdinalIgnoreCase))
                    {
                        found = module.FileName ?? string.Empty;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                // PlatformNotSupportedException and NotSupportedException on Unix, Win32Exception for a
                // bitness or access mismatch on Windows.
                EqtTrace.Verbose("ParentProcessResolver.FillVsTestConsole: Modules of parent process {0} could not be read. {1}", details.ProcessId, ex);
                found = FromProcFileSystem(details.ProcessId);
            }

            if (found.Length == 0 || string.Equals(found, details.Path, StringComparison.OrdinalIgnoreCase))
            {
                // Nothing found, or the parent simply is vstest.console, in which case the path and
                // version already printed for the parent are the answer.
                return;
            }

            details.VsTestConsolePath = found;
            details.VsTestConsoleVersion = ReadVersion(found);
        }

        /// <summary>
        /// The Unix equivalent of reading the loaded modules. The command line of a <c>dotnet exec</c>
        /// carries the full path of the vstest.console that was started.
        /// </summary>
        private static string FromProcFileSystem(int processId)
        {
            try
            {
                var commandLinePath = "/proc/" + processId.ToString(System.Globalization.CultureInfo.InvariantCulture) + "/cmdline";
                if (!File.Exists(commandLinePath))
                {
                    return string.Empty;
                }

                // Arguments are separated by NUL rather than by spaces.
                foreach (var argument in File.ReadAllText(commandLinePath).Split('\0'))
                {
                    if (argument.EndsWith("vstest.console.dll", StringComparison.OrdinalIgnoreCase)
                        || argument.EndsWith("vstest.console.exe", StringComparison.OrdinalIgnoreCase)
                        || argument.EndsWith("vstest.console", StringComparison.OrdinalIgnoreCase))
                    {
                        return argument;
                    }
                }
            }
            catch (Exception ex)
            {
                EqtTrace.Verbose("ParentProcessResolver.FromProcFileSystem: Command line of process {0} could not be read. {1}", processId, ex);
            }

            return string.Empty;
        }

        /// <summary>
        /// The .NET host appends a commit hash to its product version, so keep only the version itself.
        /// </summary>
        private static string ReadVersion(string path)
        {
            try
            {
                var version = FileVersionInfo.GetVersionInfo(path).ProductVersion;
                if (string.IsNullOrEmpty(version))
                {
                    return string.Empty;
                }

                var separator = version.IndexOf(' ');
                return separator < 0 ? version : version.Substring(0, separator);
            }
            catch (Exception ex)
            {
                EqtTrace.Verbose("ParentProcessResolver.ReadVersion: Version of '{0}' could not be read. {1}", path, ex);
                return string.Empty;
            }
        }
#endif
    }
}
#pragma warning restore IDE0161

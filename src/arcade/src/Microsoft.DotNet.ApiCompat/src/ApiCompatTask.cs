// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using Microsoft.Build.Framework;
using Microsoft.DotNet.Build.Tasks;

namespace Microsoft.DotNet.ApiCompat
{
    /// <summary>
    /// MSBuild task that invokes Executor.Run in Microsoft.DotNet.ApiCompat.Core.
    /// The console app's entry point is ApiCompatRunner which invokes Executor.Run as well.
    /// </summary>
    public class ApiCompatTask : BuildTask
    {
        private TextWriter _outputStream;

        // Keep argument comments in sync with ApiCompatRunner.cs

        /// <summary>
        /// Comma delimited list of assemblies or directories of assemblies for all the contract assemblies.
        /// </summary>
        [Required]
        public string[] Contracts { get; set; }

        /// <summary>
        /// Comma delimited list of directories to find the implementation assemblies for each contract assembly.
        /// </summary>
        [Required]
        public string[] ImplementationDirectories { get; set; }

        /// <summary>
        /// Name for right operand in comparison, default is 'implementation'.
        /// </summary>
        public string RightOperand { get; set; }

        /// <summary>
        /// Name for left operand in comparison, default is 'contract'.
        /// </summary>
        public string LeftOperand { get; set; }

        /// <summary>
        /// Output file path. Default is the console.
        /// </summary>
        public string OutFilePath { get; set; }

        /// <summary>
        /// Comma delimited list of files to skip known diffs.
        /// </summary>
        public string[] BaselineFiles { get; set; }

        /// <summary>
        /// Validates that baseline files don't have invalid/unused diffs.
        /// </summary>
        public bool ValidateBaseline { get; set; }

        /// <summary>
        /// If a contract or implementation dependency cannot be found in the given directories,
        /// fallback to try to resolve against the framework directory on the machine.
        /// </summary>
        public bool ResolveFramework { get; set; }

        /// <summary>
        /// Skip unifying the assembly references to the loaded assemblies and the assemblies
        /// found in the given directories (contractDepends and implDirs).
        /// </summary>
        public bool SkipUnifyToLibPath { get; set; }

        /// <summary>
        /// Comma delimited list of directories used to resolve the dependencies of the contract assemblies.
        /// </summary>
        public string[] ContractDepends { get; set; }

        /// <summary>
        /// Simple name for the core assembly to use.
        /// </summary>
        public string ContractCoreAssembly { get; set; }

        /// <summary>
        /// Ignore design time facades in the contract set while analyzing.
        /// </summary>
        public bool IgnoreDesignTimeFacades { get; set; }

        /// <summary>
        /// Warn if the contract assembly cannot be found in the implementation directories.
        /// Default is to error and not do analysis.
        /// </summary>
        public bool WarnOnMissingAssemblies { get; set; }

        /// <summary>
        /// Include both internal and public APIs if assembly contains an InternalsVisibleTo attribute.
        /// Otherwise, include only public APIs.
        /// </summary>
        public bool RespectInternals { get; set; }

        /// <summary>
        /// Warn if the contract version number doesn't match the found implementation version number.
        /// </summary>
        public bool WarnOnIncorrectVersion { get; set; }

        /// <summary>
        /// Enforce optional rules, in addition to the mandatory set of rules.
        /// </summary>
        public bool EnforceOptionalRules { get; set; }

        /// <summary>
        /// Enforce MDIL servicing rules in addition to IL rules.
        /// </summary>
        public bool MDIL { get; set; }

        /// <summary>
        /// When MDIL servicing rules are not being enforced, exclude validation on types that are marked
        /// with EditorBrowsable(EditorBrowsableState.Never).
        /// </summary>
        public bool ExcludeNonBrowsable { get; set; }

        /// <summary>
        /// Exclude APIs marked with a CompilerGenerated attribute.
        /// </summary>
        public bool ExcludeCompilerGenerated { get; set; }

        /// <summary>
        /// File with a list of type and/or namespace remappings to consider apply to names while diffing.
        /// </summary>
        public string RemapFile { get; set; }

        /// <summary>
        /// Skip grouping the differences by assembly instead of flattening the namespaces.
        /// </summary>
        public bool SkipGroupByAssembly { get; private set; }

        /// <summary>
        /// Comma delimited list of files with types in DocId format of which attributes to exclude.
        /// </summary>
        public string[] ExcludeAttributes { get; set; }

        /// <summary>
        /// Allow default interface methods additions to not be considered breaks. This flag should only be
        /// used if you know your consumers support DIM
        /// </summary>
        public bool AllowDefaultInterfaceMethods { get; set; }

        /// <summary>
        /// Allows to disable the trace listener that emits messages when an assembly can't be resolved.
        /// </summary>
        public bool DisableAssemblyResolveTraceListener { get; set; }

        /// <summary>
        /// If true, the task ignores the exit code. Otherwise, the task returns false if the exit code is non-zero.
        /// </summary>
        public bool IgnoreExitCode { get; set; }

        /// <summary>
        /// The ExitCode of the task for a more detailed analysis.
        /// </summary>
        [Output]
        public int ExitCode { get; set; }

        public ApiCompatTask()
        {
        }

        public ApiCompatTask(TextWriter outputStream)
        {
            _outputStream = outputStream;
        }

        public override bool Execute()
        {
            bool usesMSBuildLog = false;

            // Use the MSBuildTextWriter if no output file path is passed in or
            // when the file cannot be opened or created.
            if (_outputStream == null && (string.IsNullOrWhiteSpace(OutFilePath) ||
                !OutputHelper.TryGetOutput(OutFilePath, out _outputStream)))
            {
                _outputStream = new MSBuildTextWriter(Log);
                usesMSBuildLog = true;
                DisableAssemblyResolveTraceListener = true;
            }

            ExitCode = Executor.Run(usesMSBuildLog,
                DisableAssemblyResolveTraceListener,
                Contracts,
                ImplementationDirectories,
                _outputStream,
                RightOperand,
                LeftOperand,
                listRules: false,
                BaselineFiles,
                ValidateBaseline,
                ResolveFramework,
                SkipUnifyToLibPath,
                ContractDepends,
                ContractCoreAssembly,
                IgnoreDesignTimeFacades,
                WarnOnMissingAssemblies,
                RespectInternals,
                WarnOnIncorrectVersion,
                EnforceOptionalRules,
                MDIL,
                ExcludeNonBrowsable,
                ExcludeCompilerGenerated,
                RemapFile,
                SkipGroupByAssembly,
                ExcludeAttributes,
                AllowDefaultInterfaceMethods);

            // If the tool exited cleanly, but logged errors then assign a failing exit code (-1)
            if (ExitCode == 0 && Log.HasLoggedErrors)
            {
                ExitCode = -1;
            }

            return IgnoreExitCode || ExitCode == 0;
        }
    }
}

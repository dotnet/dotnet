// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Wix
{
    /// <summary>
    /// A tool task to invoke the WiX linker (light.exe).
    /// </summary>
    public class LinkerToolTask : WixToolTaskBase
    {
        /// <summary>
        /// Adds file version information to the MsiAssemblyName table.
        /// </summary>
        public bool AddFileVersion
        {
            get;
            set;
        } = true;        

        /// <summary>
        /// The name of the output file to generate.
        /// </summary>
        public string OutputFile
        {
            get;
            set;
        }

        /// <summary>
        /// The source files (.wixobj) used to link the executable.
        /// </summary>
        public IEnumerable<string> SourceFiles
        {
            get;
            set;
        }

        /// <summary>
        /// Semicolon sepearated list of <see href="https://docs.microsoft.com/en-us/windows/win32/msi/ice-reference">internal consistency evaluators</see> (ICEs) to suppress.
        /// </summary>
        public string SuppressIces
        {
            get;
            set;
        }

        /// <summary>
        /// The name of the WiX linker.
        /// </summary>
        protected override string ToolName => "light.exe";

        /// <summary>
        /// Creates a new 
        /// </summary>
        /// <param name="engine"></param>
        /// <param name="wixToolsetPath"><inheritdoc/></param>
        public LinkerToolTask(IBuildEngine engine, string wixToolsetPath) : base(engine, wixToolsetPath)
        {
         
        }

        protected override bool HandleTaskExecutionErrors()
        {
            Log?.LogMessage(MessageImportance.High, $"Light exited with: {ExitCode}, HasLoggedErrors: {HasLoggedErrors}");

            return base.HandleTaskExecutionErrors();
        }

        protected override string GenerateCommandLineCommands()
        {
            CommandLineBuilder.AppendSwitchIfNotNull("-o ", OutputFile);
            CommandLineBuilder.AppendSwitchIfTrue("-fv", AddFileVersion);
            CommandLineBuilder.AppendArrayIfNotNull("-ext ", Extensions.ToArray());
            CommandLineBuilder.AppendSwitchIfNotNull("-sice:", SuppressIces);
            CommandLineBuilder.AppendFileNamesIfNotNull(SourceFiles.ToArray(), " ");

            return CommandLineBuilder.ToString();
        }
    }
}

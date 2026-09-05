// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Wix
{
    /// <summary>
    /// Serves as a base class for implementing a <see cref="ToolTask"/> to invoke a WiX command.
    /// </summary>
    public abstract class WixToolTaskBase : ToolTask
    {
        /// <summary>
        /// Provides utility methods for constructing a commandline.
        /// </summary>
        protected CommandLineBuilder CommandLineBuilder
        {
            get;
        } = new CommandLineBuilder();

        protected override MessageImportance StandardOutputLoggingImportance => MessageImportance.High;

        protected override string ToolName
        {
            get;
        }

        /// <summary>
        /// Creates a new instance of a <see cref="WixToolTaskBase"/>.
        /// </summary>
        /// <param name="engine">The build engine interface to use.</param>
        /// <param name="toolPath">The path of the tool executable, resolved against the project directory if relative.</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="FileNotFoundException"/>
        protected WixToolTaskBase(IBuildEngine engine, string toolPath)
        {
            BuildEngine = engine ?? throw new ArgumentNullException(nameof(engine));

            // GetAbsolutePath rejects a null or empty path with an ArgumentException. Keep the
            // documented FileNotFoundException for an unset tool path, since WixToolsetConfiguration
            // does not validate CliPath/HeatPath.
            if (string.IsNullOrEmpty(toolPath))
            {
                throw new FileNotFoundException("The specified tool executable was not found.", toolPath);
            }

            AbsolutePath toolFullPath = TaskEnvironment.GetAbsolutePath(toolPath);
            if (!File.Exists(toolFullPath))
            {
                throw new FileNotFoundException("The specified tool executable was not found.", toolFullPath);
            }

            ToolPath = Path.GetDirectoryName(toolFullPath);
            ToolName = Path.GetFileName(toolFullPath);
        }
        
        protected override string GenerateFullPathToTool() => Path.Combine(ToolPath, ToolName);
    }
}

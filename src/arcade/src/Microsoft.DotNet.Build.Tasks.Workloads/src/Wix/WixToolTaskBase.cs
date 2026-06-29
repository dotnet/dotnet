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
        /// <param name="toolPath">The fully qualified path of the tool executable.</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="FileNotFoundException"/>
        protected WixToolTaskBase(IBuildEngine engine, string toolPath)
        {
            BuildEngine = engine ?? throw new ArgumentNullException(nameof(engine));

            if (!File.Exists(toolPath))
            {
                throw new FileNotFoundException("The specified tool executable was not found.", toolPath);
            }

            ToolPath = Path.GetDirectoryName(toolPath);
            ToolName = Path.GetFileName(toolPath);
        }
        
        protected override string GenerateFullPathToTool() => Path.Combine(ToolPath, ToolName);
    }
}

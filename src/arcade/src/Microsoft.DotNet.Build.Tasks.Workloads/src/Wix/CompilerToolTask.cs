// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Wix
{
    /// <summary>
    /// A tool task used to invoke the WiX compiler (candle.exe).
    /// </summary>
    public class CompilerToolTask : WixToolTaskBase
    {
        private List<string> _sourceFiles = new();

        /// <summary>
        /// The default architecture used for packages, components, etc.
        /// </summary>
        public string Architecture
        {
            get;
            set;
        } = "x86";

        /// <summary>
        /// The directory where the compiler output will be generated. 
        /// </summary>
        public string OutputPath
        {
            get;
            set;
        }

        /// <summary>
        /// The name of the WiX compiler executable.
        /// </summary>
        protected override string ToolName => "candle.exe";

        /// <summary>
        /// Creates a new <see cref="CompilerToolTask"/> instance that can be used to create an MSI.
        /// </summary>
        /// <param name="engine"></param>
        /// <param name="wixToolsetPath"><inheritdoc/></param>
        /// <param name="outputPath"></param>
        /// <param name="architecture"></param>
        public CompilerToolTask(IBuildEngine engine, string wixToolsetPath, string outputPath, string architecture) : base(engine, wixToolsetPath)
        {
            OutputPath = outputPath;
            Architecture = architecture;
        }

        /// <summary>
        /// Adds one or more source file to compile.
        /// </summary>
        /// <param name="sourceFiles">The set of source files to compile.</param>
        public void AddSourceFiles(params string[] sourceFiles)
        {
            foreach (string sourceFile in sourceFiles)
            {
                _sourceFiles.Add(sourceFile);
            }
        }

        public void AddSourceFiles(IEnumerable<string> sourceFiles)
        {
            _sourceFiles.AddRange(sourceFiles);
        }

        /// <inheritdoc />
        protected override string GenerateCommandLineCommands()
        {
            CommandLineBuilder.AppendSwitchIfNotNull("-out ", OutputPath);
            // No trailing space, preprocessor definitions are passed as -d<Variable>=<Value>
            CommandLineBuilder.AppendArrayIfNotNull("-d", PreprocessorDefinitions.ToArray());
            CommandLineBuilder.AppendArrayIfNotNull("-ext ", Extensions.ToArray());
            CommandLineBuilder.AppendSwitchIfNotNull("-arch ", Architecture);
            CommandLineBuilder.AppendFileNamesIfNotNull(_sourceFiles.ToArray(), " ");
            return CommandLineBuilder.ToString();
        }
    }
}

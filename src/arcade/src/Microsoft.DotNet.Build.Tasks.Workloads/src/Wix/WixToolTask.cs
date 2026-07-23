// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.DotNet.Build.Tasks.Installers;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Wix
{
    /// <summary>
    /// Tool task for invoking the WiX CLI (version 5 and above). This commands is responsible for compiling and linking.
    /// </summary>
    public class WixToolTask : WixToolTaskBase
    {
        private List<string> _sourceFiles = new();
        private List<string> _preprocessorDefinitions = new();

        private WixToolsetConfiguration _wixToolsetConfig;

        /// <summary>
        /// The default architecture used for packages, components, etc.
        /// </summary>
        public string Architecture
        {
            get;
            set;
        } = "x86";

        /// <summary>
        /// The file system path of the MSI to generate.
        /// </summary>
        public string OutputPath
        {
            get;
            set;
        }

        /// <summary>
        /// All the source files to compile and link the MSI.
        /// </summary>
        public string[] SourceFiles => _sourceFiles.ToArray();

        /// <summary>
        /// Creates a new <see cref="WixToolTask"/> instance that can be used to invoke the WiX CLI to build an MSI.
        /// </summary>
        /// <param name="engine">The build engine interface to use.</param>
        /// <param name="wixToolsetConfig">Configuration information related to the WiX toolset.</param>
        public WixToolTask(IBuildEngine engine, WixToolsetConfiguration wixToolsetConfig) :
            base(engine, wixToolsetConfig.CliPath)
        {
            _wixToolsetConfig = wixToolsetConfig;
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

        /// <summary>
        /// Adds a list of source files to compile.
        /// </summary>
        /// <param name="sourceFiles">The source files to compile.</param>
        public void AddSourceFiles(IEnumerable<string> sourceFiles) =>
            _sourceFiles.AddRange(sourceFiles);

        /// <summary>
        /// Adds a preprocessor variable definition that will be passed to WiX.
        /// </summary>
        /// <param name="name">The name of the preprocessor variable.</param>
        /// <param name="value">The value of the preprocessor variable.</param>
        public void AddPreprocessorDefinition(string name, string value) =>
            _preprocessorDefinitions.Add($"{name}={value}");

        protected override string GenerateCommandLineCommands()
        {
            // Primary command to execute.
            CommandLineBuilder.AppendSwitch("build");

            CommandLineBuilder.AppendSwitchIfNotNull("-arch ", Architecture);
            // Ensure v3 backward compatible GUIDs. v5 introduced a breaking change when generating
            // GUIDs for components where a registry key is the keypath. 
            CommandLineBuilder.AppendSwitch("-bcgg");
            CommandLineBuilder.AppendArrayIfNotNull("-d ", _preprocessorDefinitions.ToArray());
            CommandLineBuilder.AppendArrayIfNotNull("-ext ", _wixToolsetConfig.Extensions.ToArray());
            CommandLineBuilder.AppendFileNamesIfNotNull(_sourceFiles.ToArray(), " ");
            CommandLineBuilder.AppendSwitchIfNotNull("-out ", OutputPath);

            return CommandLineBuilder.ToString();
        }

        public string GetWixCommandLine() => Path.Combine(ToolPath, ToolExe) + " " +
            CommandLineBuilder.ToString();

        /// <summary>
        /// Gets a new instance of the <see cref="CreateWixBuildWixpack"/> task that can be used to create a wixpack to sign
        /// the installer.
        /// </summary>
        /// <returns>A new instance of the <see cref="CreateWixBuildWixpack"/> task.</returns>
        public CreateWixBuildWixpack GetCreateWixBuildWixpackTask(IBuildEngine buildEngine, string intermediateOutputPath,
            string outputPath, string installerFile, string workingDirectory) =>
            new CreateWixBuildWixpack
            {
                AdditionalOptions = "-bcgg",
                BuildEngine = buildEngine,
                Cultures = ["en-us"],
                DefineConstants = [.. _preprocessorDefinitions.ToArray()],
                Extensions = [.. _wixToolsetConfig.Extensions.Select(e => new TaskItem(e))],
                InstallerFile = installerFile,
                InstallerPlatform = Architecture,
                IntermediateDirectory = new TaskItem(intermediateOutputPath),
                OutputFolder = outputPath,
                SourceFiles = [.. _sourceFiles.Select(s => new TaskItem(s))],
                WixpackWorkingDir = workingDirectory
            };
    }
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Frameworks;

namespace NuGet.Commands
{
    public class MSBuildPackTargetArgs
    {
        public IEnumerable<OutputLibFile> TargetPathsToSymbols { get; set; }
        public IEnumerable<OutputLibFile> TargetPathsToAssemblies { get; set; }
        public HashSet<string> AllowedOutputExtensionsInPackageBuildOutputFolder { get; set; }
        public HashSet<string> AllowedOutputExtensionsInSymbolsPackageBuildOutputFolder { get; set; }
        public string AssemblyName { get; set; }
        public string NuspecOutputPath { get; set; }
        public Dictionary<string, IEnumerable<ContentMetadata>> ContentFiles { get; set; }
        public ISet<NuGetFramework> TargetFrameworks { get; set; }
        public IDictionary<string, string> SourceFiles { get; set; }
        public bool IncludeBuildOutput { get; set; }
        public string[] BuildOutputFolder { get; set; }


        public MSBuildPackTargetArgs()
        {
            SourceFiles = new Dictionary<string, string>();
            TargetPathsToAssemblies = new List<OutputLibFile>();
            TargetPathsToSymbols = new List<OutputLibFile>();
        }
    }

    public struct OutputLibFile
    {
        /// <summary>
        /// This is the final output path of the assembly on disk as set by msbuild.
        /// </summary>
        public string FinalOutputPath { get; set; }

        /// <summary>
        /// This denotes the TargetPath as set by msbuild. Usually this is just the file name, but for satellite DLLs,
        /// this is Culture\filename.
        ///  </summary>
        public string TargetPath { get; set; }

        /// <summary>
        /// This is the target framework for which this assembly was built.
        /// </summary>
        public string TargetFramework { get; set; }
    }
}

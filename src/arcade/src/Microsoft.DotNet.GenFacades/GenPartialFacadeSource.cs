// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.DotNet.Build.Tasks;
using System;
using System.Linq;

namespace Microsoft.DotNet.GenFacades
{
    [MSBuildMultiThreadableTask]
    public class GenPartialFacadeSource : RoslynBuildTask, IMultiThreadableTask
    {
        /// <summary>Injected by MSBuild so paths resolve against the project directory in multithreaded builds.</summary>
        public TaskEnvironment TaskEnvironment { get; set; } = TaskEnvironment.Fallback;

        [Required]
        public ITaskItem[] ReferencePaths { get; set; }

        [Required]
        public string ReferenceAssembly { get; set; }

        public ITaskItem[] CompileFiles { get; set; }

        public string DefineConstants { get; set; }

        public string LangVersion { get; set; }

        public bool IgnoreMissingTypes { get; set; }

        public string[] IgnoreMissingTypesList { get; set; }

        public string[] OmitTypes { get; set; }

        public ITaskItem[] SeedTypePreferences { get; set; }

        [Required]
        public string OutputSourcePath { get; set; }
        
        public override bool ExecuteCore()
        {
            bool result = true;
            try
            {
                result = GenPartialFacadeSourceGenerator.Execute(
                    ReferencePaths?.Select(item => TaskEnvironment.GetAbsolutePath(item.ItemSpec)).ToArray(),
                    TaskEnvironment.GetAbsolutePath(ReferenceAssembly),
                    CompileFiles?.Select(item => TaskEnvironment.GetAbsolutePath(item.ItemSpec)).ToArray(),
                    DefineConstants,
                    LangVersion,
                    TaskEnvironment.GetAbsolutePath(OutputSourcePath),
                    Log,
                    IgnoreMissingTypes,
                    IgnoreMissingTypesList,
                    OmitTypes,
                    SeedTypePreferences);
            }
            catch (Exception e)
            {
                Log.LogErrorFromException(e, showStackTrace: false);
            }

            return result && !Log.HasLoggedErrors;
        }
    }
}

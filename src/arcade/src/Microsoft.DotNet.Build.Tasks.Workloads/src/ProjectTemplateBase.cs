// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Microsoft.DotNet.Build.Tasks.Workloads
{
    /// <summary>
    /// Base class used to create projects that produce workload related artifacts.
    /// </summary>
    public abstract class ProjectTemplateBase
    {
        /// <summary>
        /// The root output directory.
        /// </summary>
        public string BaseOutputPath
        {
            get;
            set;
        }

        /// <summary>
        /// The root intermediate output directory. 
        /// </summary>
        public string BaseIntermediateOutputPath
        {
            get;
            set;
        }

        /// <summary>
        /// The filename and extension of the generated project.
        /// </summary>
        protected abstract string ProjectFile
        {
            get;
        }        

        /// <summary>
        /// The directory where the project source is generated.
        /// </summary>
        protected abstract string ProjectSourceDirectory
        {
            get;
        }

        protected Dictionary<string, string> ReplacementTokens
        {
            get;
        } = new();

        /// <summary>
        /// The root directory for generated source files.
        /// </summary>
        public string SourceDirectory => Path.Combine(BaseIntermediateOutputPath, "src");

        public ProjectTemplateBase(string baseIntermediateOutputPath, string baseOutputPath)
        {
            BaseIntermediateOutputPath = baseIntermediateOutputPath;
            BaseOutputPath = baseOutputPath;
        }

        /// <summary>
        /// Generates the project template and returns the path to the project file.
        /// </summary>
        /// <returns>The path to the project file.</returns>
        public abstract string Create();        
    }
}

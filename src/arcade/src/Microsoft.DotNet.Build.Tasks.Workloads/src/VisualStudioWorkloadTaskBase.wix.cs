// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.DotNet.Build.Tasks.Workloads.Wix;

namespace Microsoft.DotNet.Build.Tasks.Workloads
{
    /// <summary>
    /// Base class used for build tasks that generate MSI installers that
    /// can be shipped with Visual Studio or used by the CLI using NuGet packages.
    /// </summary>
    public abstract class VisualStudioWorkloadTaskBase : Task
    {
        private WixToolsetConfiguration _wixToolsetConfiguration;

        /// <summary>
        /// A set of all supported MSI platforms.
        /// </summary>
        public static readonly string[] SupportedPlatforms = { "x86", "x64", "arm64" };

        /// <summary>
        /// The root intermediate output directory. This directory serves as a the base for generating
        /// installer sources and other projects used to create workload artifacts for Visual Studio.
        /// </summary>
        [Required]
        public string BaseIntermediateOutputPath
        {
            get;
            set;
        }

        /// <summary>
        /// The root output directory to use for compiled artifacts such as MSIs.
        /// </summary>
        [Required]
        public string BaseOutputPath
        {
            get;
            set;
        }

        /// <summary>
        /// Determines whether wixpack archives should be generated for each workload MSI.
        /// </summary>
        public bool CreateWixPacks
        {
            get;
            set;
        } = true;

        /// <summary>
        /// A set of items containing all the MSIs that were generated. Additional metadata
        /// is provided for the projects that need to be built to produce NuGet packages for
        /// the MSI.
        /// </summary>
        [Output]
        public ITaskItem[] Msis
        {
            get;
            protected set;
        }

        /// <summary>
        /// The output path where MSIs will be placed.
        /// </summary>
        protected string MsiOutputPath => Path.Combine(BaseOutputPath, "msi");

        /// <summary>
        /// Root directory where packages are extracted.
        /// </summary>
        protected string PackageRootDirectory => Path.Combine(BaseIntermediateOutputPath, "pkg");

        /// <summary>
        /// A set of items containing .swixproj files that can be build to generate
        /// Visual Studio Installer components for workloads.
        /// </summary>
        [Output]
        public ITaskItem[] SwixProjects
        {
            get;
            protected set;
        }

        /// <summary>
        /// The path to the WiX CLI (wix.exe).
        /// </summary>
        [Required]
        public string WixExe
        {
            get;
            set;
        }

        /// <summary>
        /// The path to the harvesting tool (heat.exe).
        /// </summary>
        [Required]
        public string HeatExe
        {
            get;
            set;
        }

        /// <summary>
        /// Set of all the extensions needed to build MSIs. Items must specify the full path to the extension assemblies.
        /// </summary>
        [Required]
        public ITaskItem[] WixExtensions
        {
            get;
            set;
        }        

        /// <summary>
        /// Gets the WiX toolset configuration (CLI, tools, extensions, etc.) to use.
        /// </summary>
        protected WixToolsetConfiguration WixToolsetConfig
        {
            get
            {
                if (_wixToolsetConfiguration is null)
                {
                    _wixToolsetConfiguration = WixToolsetConfiguration.Create(WixExe, HeatExe, [.. WixExtensions.Select(e => e.ItemSpec)]);
                }

                return _wixToolsetConfiguration;
            }
        }

        /// <summary>
        /// Core execution of the build task.
        /// </summary>
        /// <returns><see langword="true" /> if successful; otherwise <see langword="false"/>.</returns>
        protected abstract bool ExecuteCore();

        public sealed override bool Execute()
        {
            try
            {
                return ExecuteCore();
            }
            catch (Exception e)
            {
                Log.LogError(e.ToString());
            }

            return !Log.HasLoggedErrors;
        }
    }
}

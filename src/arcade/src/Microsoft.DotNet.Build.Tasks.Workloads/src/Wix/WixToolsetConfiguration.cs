// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Wix
{
    /// <summary>
    /// Defines the WiX toolset to use for building an MSI.
    /// </summary>
    public class WixToolsetConfiguration
    {
        private List<string> _extensions = new();

        /// <summary>
        /// File system path for wix.exe
        /// </summary>
        public string CliPath
        {
            get;
            set;
        }

        /// <summary>
        /// File system path for heat.exe
        /// </summary>
        public string HeatPath
        {
            get;
            set;
        }

        /// <summary>
        /// Collection of extensions to pass to the CLI when building.
        /// </summary>
        public IEnumerable<string> Extensions => _extensions;

        public void AddExtensions(string[] extensions)
        {
            _extensions.AddRange(extensions);
        }

        /// <summary>
        /// Creates a new <see cref="WixToolsetConfiguration"/>.
        /// </summary>
        /// <param name="cliPath">The file system path for the CLI (wix.exe).</param>
        /// <param name="heatPath">The file system path for Heat (heat.exe).</param>
        /// <param name="extensions">File system paths for WiX extensions to include.</param>
        /// <returns></returns>
        public static WixToolsetConfiguration Create(string cliPath, string heatPath, params string[] extensions)
        {
            var config = new WixToolsetConfiguration
            {
                CliPath = cliPath,
                HeatPath = heatPath,
            };

            config.AddExtensions(extensions);

            return config;
        }
    }
}

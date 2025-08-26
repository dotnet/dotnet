// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Build.Locator
{
    /// <summary>
    ///     Options to consider when querying for Visual Studio instances.
    /// </summary>
    public class VisualStudioInstanceQueryOptions
    {
        /// <summary>
        ///     Default query options (all instances).
        /// </summary>
        public static VisualStudioInstanceQueryOptions Default => new VisualStudioInstanceQueryOptions
        {
            DiscoveryTypes =
#if FEATURE_VISUALSTUDIOSETUP
                DiscoveryType.DeveloperConsole | DiscoveryType.VisualStudioSetup
#endif
#if NETCOREAPP
                DiscoveryType.DotNetSdk
#endif
        };

        /// <summary>
        ///     Discovery types for instances included in the query.
        /// </summary>
        public DiscoveryType DiscoveryTypes { get; set; }

#if NETCOREAPP
        /// <summary>
        ///     Allow discovery of .NET SDK versions that are unlikely to be successfully loaded in the current process.
        /// </summary>
        /// <remarks>
        ///     Defaults to <see langword="false"/>. Set this to <see langword="true"/> only if your application has special logic to handle loading an incompatible SDK, such as launching a new process with the target SDK's runtime.
        /// </remarks.
        public bool AllowAllRuntimeVersions { get; set; } = false;

        /// <summary>
        ///     Allow discovery of .NET SDK versions from all discovered dotnet install locations.
        /// </summary>
        /// <remarks>
        ///     Defaults to <see langword="false"/>. Set this to <see langword="true"/> only if you do not mind behaving differently than a command-line dotnet invocation.
        /// </remarks.
        public bool AllowAllDotnetLocations { get; set; } = false;
#endif

        /// <summary>
        ///     Working directory to use when querying for instances. Ensure it is the project directory to pick up the right global.json.
        /// </summary>
        public string WorkingDirectory { get; set; } = Environment.CurrentDirectory;
    }
}

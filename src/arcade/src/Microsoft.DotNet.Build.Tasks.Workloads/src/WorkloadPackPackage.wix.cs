// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using Microsoft.NET.Sdk.WorkloadManifestReader;

namespace Microsoft.DotNet.Build.Tasks.Workloads
{
    /// <summary>
    /// Represents a NuGet package for a workload pack.
    /// </summary>
    internal abstract class WorkloadPackPackage : WorkloadPackageBase
    {
        private readonly WorkloadPack _pack;

        public WorkloadPackKind Kind => _pack.Kind;

        /// <inheritdoc />
        public override Version MsiVersion
        {
            get;
        }

        /// <summary>
        /// An array of all the supported installer target platforms for this package.
        /// </summary>
        public string[] Platforms
        {
            get;
        }

        public WorkloadPackPackage(WorkloadPack pack, string packagePath, string[] platforms, string destinationBaseDirectory, 
            ITaskItem[]? shortNames = null, TaskLoggingHelper? log = null) : base(packagePath, destinationBaseDirectory, shortNames, log)
        {
            _pack = pack;
            Platforms = platforms;
            MsiVersion = Version;

            // Override the SWIX ID for MSI packages to use the shortened, non-aliased ID with the pack version. For example,
            // if a the manifest defines a pack as 
            //
            // "Microsoft.NET.Runtime.Emscripten.Python.net7" : {
            // "alias-to": {
            //   "win-x64": "Microsoft.NET.Runtime.Emscripten.3.1.12.Python.win-x64",
            //   "osx-x64": "Microsoft.NET.Runtime.Emscripten.3.1.12.Python.osx-x64",
            //   "osx-arm64": "Microsoft.NET.Runtime.Emscripten.3.1.12.Python.osx-x64"
            //  }
            //
            // We'll pick "Microsoft.NET.Runtime.Emscripten.Python.net7" as the SWIX ID + with the pack version, but the SWIX package
            // will point to the MSI generated from the aliased pack, e.g. Microsoft.NET.Runtime.Emscripten.3.1.12.Python.win-x64
            SwixPackageId = $"{pack.Id.ToString().Replace(shortNames)}.{Identity.Version}";
        }

        /// <summary>
        /// Gets all the packages associated with a specific workload pack for all supported platforms.
        /// </summary>
        /// <param name="pack"></param>
        /// <returns>An enumerable of tuples. Each tuple contains the full path of the NuGet package and support platforms.</returns>
        internal static IEnumerable<(string sourcePackage, string[] platforms)> GetSourcePackages(string packageSource, WorkloadPack pack)
        {
            if (pack.IsAlias && pack.AliasTo != null)
            {
                foreach (string rid in pack.AliasTo.Keys)
                {
                    string sourcePackage = Path.Combine(packageSource, $"{pack.AliasTo[rid]}.{pack.Version}.nupkg");

                    switch (rid)
                    {
                        case "win7":
                        case "win10":
                        case "win":
                        case "any":
                            yield return (sourcePackage, CreateVisualStudioWorkload.SupportedPlatforms);
                            break;
                        case "win-x64":
                            yield return (sourcePackage, new[] { "x64" });
                            break;
                        case "win-x86":
                            yield return (sourcePackage, new[] { "x86" });
                            break;
                        case "win-arm64":
                            yield return (sourcePackage, new[] { "arm64" });
                            break;
                        default:
                            // Unsupported RID.
                            continue;
                    }
                }
            }
            else
            {
                // For non-RID specific packs we'll produce MSIs for each supported platform.
                yield return (Path.Combine(packageSource, $"{pack.Id}.{pack.Version}.nupkg"), CreateVisualStudioWorkload.SupportedPlatforms);
            }
        }

        /// <summary>
        /// Creates a workload pack package from the provided NuGet package and workload pack.
        /// </summary>
        /// <param name="pack">The workload pack determines the type of package to create.</param>
        /// <param name="sourcePackage">The NuGet package to use.</param>
        /// <param name="platforms">The platforms that can be targeted by the package.</param>
        /// <param name="destinationBaseDirectory"></param>
        /// <returns>A new <see cref="WorkloadPackPackage"/>.</returns>
        /// <exception cref="ArgumentException"></exception>
        internal static WorkloadPackPackage Create(WorkloadPack pack, string sourcePackage, string[] platforms,
            string destinationBaseDirectory, ITaskItem[]? shortNames, TaskLoggingHelper? log) =>
            pack.Kind switch
            {
                WorkloadPackKind.Sdk => new SdkPackPackage(pack, sourcePackage, platforms, destinationBaseDirectory, shortNames, log),
                WorkloadPackKind.Framework => new FrameworkPackPackage(pack, sourcePackage, platforms, destinationBaseDirectory, shortNames, log),
                WorkloadPackKind.Library => new LibraryPackPackage(pack, sourcePackage, platforms, destinationBaseDirectory, shortNames, log),
                WorkloadPackKind.Template => new TemplatePackPackage(pack, sourcePackage, platforms, destinationBaseDirectory, shortNames, log),
                WorkloadPackKind.Tool => new ToolsPackPackage(pack, sourcePackage, platforms, destinationBaseDirectory, shortNames, log),
                _ => throw new ArgumentException(string.Format(Strings.UnknownWorkloadKind, pack.Kind))
            };
    }
}

#nullable disable

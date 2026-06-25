// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Swix
{
    internal class PackageGroupSwixProject : SwixProjectBase
    {
        private SwixPackageGroup _swixPackageGroup;

        public PackageGroupSwixProject(SwixPackageGroup packageGroup, string baseIntermediateOutputPath, string baseOutputPath, bool outOfSupport = false) :
            base(packageGroup, baseIntermediateOutputPath, baseOutputPath)
        {
            SourcePath = Path.Combine(SourcePath, $"{packageGroup.SdkFeatureBand}",
                $"{Path.GetRandomFileName()}");
            _swixPackageGroup = packageGroup;
            ValidateRelativePackagePath(GetRelativePackagePath());

            if (!packageGroup.HasDependencies)
            {
                throw new ArgumentException(string.Format(Strings.ComponentMustHaveAtLeastOneDependency, packageGroup.Name));
            }
        }

        /// <inheritdoc />
        public override string Create()
        {
            string swixProj = AddFile("packageGroup.swixproj", $"{Id}.{Version.ToString(2)}.swixproj");
            string packageGroupSwr = AddFile("packageGroup.swr");

            // SWIX is indentation sensitive. The dependencies should be written as 
            //
            // vs.dependencies
            //   vs.dependency id=<package Id>
            //                 version=<version range>
            using StreamWriter swrWriter = File.AppendText(packageGroupSwr);

            foreach (SwixDependency dependency in _swixPackageGroup.Dependencies)
            {
                swrWriter.WriteLine($"  vs.dependency id={dependency.Id}");
            }

            return swixProj;
        }

        /// <summary>
        /// Creates a task item with metadata describing the package group SWIX project.
        /// </summary>
        /// <param name="swixPackageGroup">The package group to use when generating the task item.</param>
        /// <param name="baseIntermediateOutputPath">The root intermediate output directory used for generating files.</param>
        /// <param name="baseOutputPath">The base output directory for storing the compiled SWIX project's output (JSON manifest).</param>
        /// <param name="packageGroupType">The metadata value for the package group. This is used for batching and selection during builds.</param>
        /// <returns>A task item describing the SWIX project.</returns>
        public static ITaskItem CreateProjectItem(SwixPackageGroup swixPackageGroup, string baseIntermediateOutputPath, string baseOutputPath,
            string packageGroupType)
        {
            PackageGroupSwixProject swixPackageGroupProject = new(swixPackageGroup, baseIntermediateOutputPath, baseOutputPath);
            ITaskItem swixProjectItem = new TaskItem(swixPackageGroupProject.Create());

            swixProjectItem.SetMetadata(Metadata.SdkFeatureBand, $"{swixPackageGroup.SdkFeatureBand}");
            swixProjectItem.SetMetadata(Metadata.PackageType, packageGroupType);
            swixProjectItem.SetMetadata(Metadata.IsPreview, "false");

            return swixProjectItem;
        }
    }
}

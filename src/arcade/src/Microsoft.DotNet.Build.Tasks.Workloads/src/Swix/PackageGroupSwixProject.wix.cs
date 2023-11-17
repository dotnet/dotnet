// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Swix
{
    internal class PackageGroupSwixProject : SwixProjectBase
    {
        private SwixPackageGroup _swixPackageGroup;

        protected override string ProjectFile
        {
            get;
        }

        protected override string ProjectSourceDirectory
        {
            get;
        }

        public PackageGroupSwixProject(SwixPackageGroup packageGroup, string baseIntermediateOutputPath, string baseOutputPath, bool outOfSupport = false) :
            base(packageGroup, baseIntermediateOutputPath, baseOutputPath)
        {
            _swixPackageGroup = packageGroup;
            ValidateRelativePackagePath(GetRelativePackagePath());

            if (!packageGroup.HasDependencies)
            {
                throw new ArgumentException(string.Format(Strings.ComponentMustHaveAtLeastOneDependency, packageGroup.Name));
            }

            ProjectSourceDirectory = Path.Combine(SwixDirectory, $"{packageGroup.SdkFeatureBand}",
                $"{Path.GetRandomFileName()}");
        }

        /// <inheritdoc />
        public override string Create()
        {
            string swixProj = EmbeddedTemplates.Extract("packageGroup.swixproj", ProjectSourceDirectory, $"{Id}.{Version.ToString(2)}.swixproj");
            string packageGroupSwr = EmbeddedTemplates.Extract("packageGroup.swr", ProjectSourceDirectory);

            ReplaceTokens(swixProj);
            ReplaceTokens(packageGroupSwr);

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

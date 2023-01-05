// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

// Keep in sync with https://raw.githubusercontent.com/NuGet/NuGet.Client/dccbd304b11103e08b97abf4cf4bcc1499d9235a/src/NuGet.Core/NuGet.Frameworks/NuGetFrameworkUtility.cs

using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NuGet.Common;
using NuGet.Frameworks;

namespace Microsoft.DotNet.Build.Tasks.TargetFramework
{
    public class ChooseBestP2PTargetFrameworkTask : BuildTask
    {
        private const string NEAREST_TARGET_FRAMEWORK = "NearestTargetFramework";
        private const string TARGET_FRAMEWORKS = "TargetFrameworks";

        [Required]
        public string? RuntimeGraph { get; set; }

        /// <summary>
        /// The current project's target framework.
        /// </summary>
        [Required]
        public string? CurrentProjectTargetFramework { get; set; }

        /// <summary>
        /// Optional TargetPlatformMoniker
        /// </summary>
        public string? CurrentProjectTargetPlatform { get; set; }

        public bool OmitIncompatibleProjectReferences { get; set; }

        /// <summary>
        /// The project references for property lookup.
        /// </summary>
        public ITaskItem[]? AnnotatedProjectReferences { get; set; }

        /// <summary>
        /// The project references with assigned properties.
        /// </summary>
        [Output]
        public ITaskItem[]? AssignedProjects { get; set; }

        public override bool Execute()
        {
            if (AnnotatedProjectReferences == null)
            {
                return !Log.HasLoggedErrors;
            }

            // validate current project framework
            string errorMessage = string.Format(CultureInfo.CurrentCulture, "The project target framework '{0}' is not a supported target framework.", $"TargetFrameworkMoniker: {CurrentProjectTargetFramework}, TargetPlatformMoniker:{CurrentProjectTargetPlatform}");
            if (!TryParseFramework(CurrentProjectTargetFramework!, CurrentProjectTargetPlatform, errorMessage, Log, out var projectNuGetFramework))
            {
                return false;
            }

            TargetFrameworkResolver targetFrameworkResolver = TargetFrameworkResolver.CreateOrGet(RuntimeGraph!);
            List<ITaskItem> assignedProjects = new(AnnotatedProjectReferences.Length);

            foreach (ITaskItem annotatedProjectReference in AnnotatedProjectReferences)
            {
                ITaskItem? assignedProject = AssignNearestFrameworkForSingleReference(annotatedProjectReference, projectNuGetFramework, targetFrameworkResolver);
                if (assignedProject != null)
                {
                    assignedProjects.Add(assignedProject);
                }
            }

            AssignedProjects = assignedProjects.ToArray();
            return !Log.HasLoggedErrors;
        }

        private ITaskItem? AssignNearestFrameworkForSingleReference(ITaskItem project,
            NuGetFramework projectNuGetFramework,
            TargetFrameworkResolver targetFrameworkResolver)
        {
            TaskItem itemWithProperties = new(project);
            string referencedProjectFrameworkString = project.GetMetadata(TARGET_FRAMEWORKS);

            if (string.IsNullOrEmpty(referencedProjectFrameworkString))
            {
                // No target frameworks set, nothing to do.
                return itemWithProperties;
            }

            string[] referencedProjectFrameworks = MSBuildStringUtility.Split(referencedProjectFrameworkString!);

            // try project framework
            string? nearestNuGetFramework = targetFrameworkResolver.GetNearest(referencedProjectFrameworks, projectNuGetFramework);
            if (nearestNuGetFramework != null)
            {
                itemWithProperties.SetMetadata(NEAREST_TARGET_FRAMEWORK, nearestNuGetFramework);
                return itemWithProperties;
            }

            if (OmitIncompatibleProjectReferences)
            {
                return null;
            }

            // no match found
            Log.LogError(string.Format(CultureInfo.CurrentCulture, "Project '{0}' targets '{1}'. It cannot be referenced by a project that targets '{2}{3}'.", project.ItemSpec, referencedProjectFrameworkString, projectNuGetFramework.DotNetFrameworkName, projectNuGetFramework.HasPlatform ? "-" + projectNuGetFramework.DotNetPlatformName : string.Empty));
            return itemWithProperties;
        }

        private static bool TryParseFramework(string targetFrameworkMoniker, string? targetPlatformMoniker, string errorMessage, Log logger, out NuGetFramework nugetFramework)
        {
            // Check if we have a long name.
#if NETFRAMEWORK || NETSTANDARD
            nugetFramework = targetFrameworkMoniker.Contains(",")
                ? NuGetFramework.ParseComponents(targetFrameworkMoniker, targetPlatformMoniker)
                : NuGetFramework.Parse(targetFrameworkMoniker);
#else
            nugetFramework = targetFrameworkMoniker.Contains(',', System.StringComparison.Ordinal)
               ? NuGetFramework.ParseComponents(targetFrameworkMoniker, targetPlatformMoniker)
               : NuGetFramework.Parse(targetFrameworkMoniker);
#endif

            // validate framework
            if (nugetFramework.IsUnsupported)
            {
                logger.LogError(errorMessage);
                return false;
            }

            return true;
        }
    }
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NuGet.Common;
using NuGet.Frameworks;

namespace NuGet.Build.Tasks
{
    public class GetReferenceNearestTargetFrameworkTask : Microsoft.Build.Utilities.Task
    {
        private const string NEAREST_TARGET_FRAMEWORK = "NearestTargetFramework";
        private const string TARGET_FRAMEWORKS = "TargetFrameworks";
        private const string TARGET_FRAMEWORK_MONIKERS = "TargetFrameworkMonikers";
        private const string TARGET_PLATFORM_MONIKERS = "TargetPlatformMonikers";
        private const string MSBUILD_SOURCE_PROJECT_FILE = "MSBuildSourceProjectFile";

        /// <summary>
        /// The current project's name.
        /// </summary>
        public string CurrentProjectName { get; set; }

        /// <summary>
        /// The current project's target framework.
        /// </summary>
        [Required]
        public string CurrentProjectTargetFramework { get; set; }

        /// <summary>
        /// Optional TargetPlatformMoniker
        /// </summary>
        public string CurrentProjectTargetPlatform { get; set; }

        /// <summary>
        /// Optional list of target frameworks to be used as Fallback target frameworks.
        /// </summary>
        public string[] FallbackTargetFrameworks { get; set; }

        /// <summary>
        /// The project references for property lookup.
        /// </summary>
        public ITaskItem[] AnnotatedProjectReferences { get; set; }

        /// <summary>
        /// The project references with assigned properties.
        /// </summary>
        [Output]
        public ITaskItem[] AssignedProjects { get; set; }

        public override bool Execute()
        {

            var logger = new MSBuildLogger(Log);

            BuildTasksUtility.LogInputParam(logger, nameof(CurrentProjectTargetFramework), CurrentProjectTargetFramework);

            BuildTasksUtility.LogInputParam(logger, nameof(CurrentProjectTargetPlatform), CurrentProjectTargetPlatform);

            BuildTasksUtility.LogInputParam(logger, nameof(FallbackTargetFrameworks),
                FallbackTargetFrameworks == null
                    ? ""
                    : string.Join(";", FallbackTargetFrameworks.Select(p => p)));

            BuildTasksUtility.LogInputParam(logger, nameof(AnnotatedProjectReferences),
                AnnotatedProjectReferences == null
                    ? ""
                    : string.Join(";", AnnotatedProjectReferences.Select(p => p.ItemSpec)));

            if (AnnotatedProjectReferences == null)
            {
                return !Log.HasLoggedErrors;
            }

            var fallbackNuGetFrameworks = new List<NuGetFramework>();

            // validate current project framework
            var errorMessage = string.Format(CultureInfo.CurrentCulture, Strings.UnsupportedTargetFramework, $"TargetFrameworkMoniker: {CurrentProjectTargetFramework}, TargetPlatformMoniker:{CurrentProjectTargetPlatform}");
            if (!TryParseFramework(CurrentProjectTargetFramework, CurrentProjectTargetPlatform, errorMessage, logger, out var projectNuGetFramework))
            {
                return false;
            }

            if (FallbackTargetFrameworks != null &&
                FallbackTargetFrameworks.Length > 0)
            {
                foreach (var fallbackFramework in FallbackTargetFrameworks)
                {
                    // validate ATF project frameworks
                    errorMessage = string.Format(CultureInfo.CurrentCulture, Strings.UnsupportedFallbackFramework, fallbackFramework);
                    if (!TryParseFramework(fallbackFramework, errorMessage, logger, out var nugetFramework))
                    {
                        return false;
                    }
                    else
                    {
                        fallbackNuGetFrameworks.Add(nugetFramework);
                    }
                }
            }

            AssignedProjects = new ITaskItem[AnnotatedProjectReferences.Length];
            for (var index = 0; index < AnnotatedProjectReferences.Length; index++)
            {
                AssignedProjects[index] = AssignNearestFrameworkForSingleReference(AnnotatedProjectReferences[index], projectNuGetFramework, fallbackNuGetFrameworks, logger);
            }

            BuildTasksUtility.LogOutputParam(logger, nameof(AssignedProjects), string.Join(";", AssignedProjects.Select(p => p.ItemSpec)));

            return !Log.HasLoggedErrors;
        }

        private ITaskItem AssignNearestFrameworkForSingleReference(
            ITaskItem project,
            NuGetFramework projectNuGetFramework,
            IList<NuGetFramework> fallbackNuGetFrameworks,
            MSBuildLogger logger)
        {
            var itemWithProperties = new TaskItem(project);
            var referencedProjectFrameworkString = project.GetMetadata(TARGET_FRAMEWORKS);
            var referenceTargetFrameworkMonikers = project.GetMetadata(TARGET_FRAMEWORK_MONIKERS);
            var referencedProjectPlatformString = project.GetMetadata(TARGET_PLATFORM_MONIKERS);

            var referencedProjectFile = project.GetMetadata(MSBUILD_SOURCE_PROJECT_FILE);

            if (string.IsNullOrEmpty(referencedProjectFrameworkString))
            {
                // No target frameworks set, nothing to do.
                return itemWithProperties;
            }

            var referencedProjectFrameworks = MSBuildStringUtility.Split(referencedProjectFrameworkString);
            var referencedProjectTargetFrameworkMonikers = MSBuildStringUtility.Split(referenceTargetFrameworkMonikers);
            var referencedProjectTargetPlatformMonikers = MSBuildStringUtility.Split(referencedProjectPlatformString);

            if (referencedProjectTargetFrameworkMonikers.Length > 0 &&
                (referencedProjectTargetFrameworkMonikers.Length != referencedProjectTargetPlatformMonikers.Length ||
                referencedProjectTargetFrameworkMonikers.Length != referencedProjectFrameworks.Length))
            {
                logger.LogError($"Internal error for {CurrentProjectName}." +
                    $" Expected {TARGET_FRAMEWORKS}:{referencedProjectFrameworks}, " +
                    $"{TARGET_FRAMEWORK_MONIKERS}:{referenceTargetFrameworkMonikers}, " +
                    $"{TARGET_PLATFORM_MONIKERS}:{referencedProjectPlatformString} to have the same number of elements.");
                return itemWithProperties;
            }
            // TargetFrameworks, TargetFrameworkMoniker, TargetPlatforMoniker
            var targetFrameworkInformations = new List<TargetFrameworkInformation>();
            var useTargetMonikers = referencedProjectTargetFrameworkMonikers.Length > 0;
            for (int i = 0; i < referencedProjectFrameworks.Length; i++)
            {

                targetFrameworkInformations.Add(new TargetFrameworkInformation(
                    referencedProjectFrameworks[i],
                    useTargetMonikers ? referencedProjectTargetFrameworkMonikers[i] : null,
                    useTargetMonikers ? referencedProjectTargetPlatformMonikers[i] : null));
            }

            // try project framework
            var nearestNuGetFramework = NuGetFrameworkUtility.GetNearest(targetFrameworkInformations, projectNuGetFramework, GetNuGetFramework);
            if (nearestNuGetFramework != null)
            {
                itemWithProperties.SetMetadata(NEAREST_TARGET_FRAMEWORK, nearestNuGetFramework._targetFrameworkAlias);
                return itemWithProperties;
            }

            // try project fallback frameworks
            foreach (var currentProjectTargetFramework in fallbackNuGetFrameworks)
            {
                nearestNuGetFramework = NuGetFrameworkUtility.GetNearest(targetFrameworkInformations, currentProjectTargetFramework, GetNuGetFramework);

                if (nearestNuGetFramework != null)
                {
                    var message = string.Format(CultureInfo.CurrentCulture,
                        Strings.ImportsFallbackWarning,
                        referencedProjectFile,
                        currentProjectTargetFramework.DotNetFrameworkName,
                        projectNuGetFramework.DotNetFrameworkName);

                    var warning = RestoreLogMessage.CreateWarning(NuGetLogCode.NU1702, message);
                    warning.LibraryId = referencedProjectFile;
                    warning.ProjectPath = CurrentProjectName;

                    // log NU1702 for ATF on project reference
                    logger.Log(warning);

                    itemWithProperties.SetMetadata(NEAREST_TARGET_FRAMEWORK, nearestNuGetFramework._targetFrameworkAlias);
                    return itemWithProperties;
                }
            }

            // no match found
            logger.LogError(string.Format(CultureInfo.CurrentCulture, Strings.NoCompatibleTargetFramework, project.ItemSpec, projectNuGetFramework.DotNetFrameworkName, referencedProjectFrameworkString));
            return itemWithProperties;
        }

        private static bool TryParseFramework(string framework, string errorMessage, MSBuildLogger logger, out NuGetFramework nugetFramework)
        {
            nugetFramework = NuGetFramework.Parse(framework);

            // validate framework
            if (nugetFramework.IsUnsupported)
            {
                logger.LogError(errorMessage);
                return false;
            }

            return true;
        }

        private static bool TryParseFramework(string targetFrameworkMoniker, string targetPlatformMoniker, string errorMessage, MSBuildLogger logger, out NuGetFramework nugetFramework)
        {
            // Check if we have a long name.
#if NETFRAMEWORK || NETSTANDARD
            nugetFramework = targetFrameworkMoniker.Contains(',')
                ? NuGetFramework.ParseComponents(targetFrameworkMoniker, targetPlatformMoniker)
                : NuGetFramework.Parse(targetFrameworkMoniker);
#else
            nugetFramework = targetFrameworkMoniker.Contains(',', StringComparison.Ordinal)
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

        private static NuGetFramework GetNuGetFramework(TargetFrameworkInformation targetFrameworkInformation)
        {
            // Legacy path, process targetFrameworks if empty
            if (string.IsNullOrEmpty(targetFrameworkInformation._targetFrameworkMoniker))
            {
                return NuGetFramework.Parse(targetFrameworkInformation._targetFrameworkAlias);
            }

            // TargetFrameworkMoniker is always expected to be set. TargetPlatformMoniker will have a `None` value when empty, for frameworks like net5.0.
            return NuGetFramework.ParseComponents(targetFrameworkInformation._targetFrameworkMoniker,
                targetFrameworkInformation._targetPlatformMoniker.Equals("None", StringComparison.OrdinalIgnoreCase) ?
                    string.Empty :
                    targetFrameworkInformation._targetPlatformMoniker);
        }

        internal class TargetFrameworkInformation
        {
            internal readonly string _targetFrameworkAlias;
            internal readonly string _targetFrameworkMoniker;
            internal readonly string _targetPlatformMoniker;

            public TargetFrameworkInformation(string targetFrameworkAlias, string targetFrameworkMoniker, string targetPlatformMoniker)
            {
                _targetFrameworkAlias = targetFrameworkAlias;
                _targetFrameworkMoniker = targetFrameworkMoniker;
                _targetPlatformMoniker = targetPlatformMoniker;
            }
        }
    }
}

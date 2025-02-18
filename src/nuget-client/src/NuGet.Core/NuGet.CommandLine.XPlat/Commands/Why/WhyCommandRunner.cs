// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Build.Evaluation;
using NuGet.ProjectModel;

namespace NuGet.CommandLine.XPlat.Commands.Why
{
    internal static class WhyCommandRunner
    {
        private const string ProjectAssetsFile = "ProjectAssetsFile";

        /// <summary>
        /// Executes the 'why' command.
        /// </summary>
        /// <param name="whyCommandArgs">CLI arguments for the 'why' command.</param>
        public static async Task<int> ExecuteCommand(WhyCommandArgs whyCommandArgs)
        {
            bool validArgumentsUsed = ValidatePathArgument(whyCommandArgs.Path, whyCommandArgs.Logger)
                                        && ValidatePackageArgument(whyCommandArgs.Package, whyCommandArgs.Logger);
            if (!validArgumentsUsed)
            {
                return ExitCodes.InvalidArguments;
            }

            string targetPackage = whyCommandArgs.Package;
            IAsyncEnumerable<(string assetsFilePath, string? projectPath)> assetsFiles;
            try
            {
                assetsFiles = FindAssetsFilesAsync(whyCommandArgs.Path, whyCommandArgs.Logger, whyCommandArgs.CancellationToken);
            }
            catch (ArgumentException ex)
            {
                whyCommandArgs.Logger.LogError(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.WhyCommand_Error_ArgumentExceptionThrown,
                        ex.Message));

                return ExitCodes.InvalidArguments;
            }

            bool anyErrors = false;
            await foreach ((string assetsFilePath, string? projectPath) in assetsFiles)
            {
                LockFile? assetsFile = GetProjectAssetsFile(assetsFilePath, projectPath, whyCommandArgs.Logger);

                if (assetsFile != null)
                {
                    ValidateFrameworksOptionsExistInAssetsFile(assetsFile, whyCommandArgs.Frameworks, whyCommandArgs.Logger);

                    Dictionary<string, List<DependencyNode>?>? dependencyGraphPerFramework = DependencyGraphFinder.GetAllDependencyGraphsForTarget(
                        assetsFile,
                        whyCommandArgs.Package,
                        whyCommandArgs.Frameworks);

                    if (dependencyGraphPerFramework != null)
                    {
                        whyCommandArgs.Logger.LogMinimal(
                            string.Format(CultureInfo.CurrentCulture,
                                Strings.WhyCommand_Message_DependencyGraphsFoundInProject,
                                assetsFile.PackageSpec.Name,
                                targetPackage));

                        DependencyGraphPrinter.PrintAllDependencyGraphs(dependencyGraphPerFramework, targetPackage, whyCommandArgs.Logger);
                    }
                    else
                    {
                        whyCommandArgs.Logger.LogMinimal(
                            string.Format(CultureInfo.CurrentCulture,
                                Strings.WhyCommand_Message_NoDependencyGraphsFoundInProject,
                                assetsFile.PackageSpec.Name,
                                targetPackage));
                    }
                }
                else
                {
                    anyErrors = true;
                }
            }

            return anyErrors ? ExitCodes.Error : ExitCodes.Success;
        }

        private static async IAsyncEnumerable<(string assetsFilePath, string? projectPath)> FindAssetsFilesAsync(string path, ILoggerWithColor logger, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            if (XPlatUtility.IsJsonFile(path))
            {
                yield return (path, null);
                yield break;
            }

            var projectPaths = await MSBuildAPIUtility.GetListOfProjectsFromPathArgumentAsync(path, cancellationToken);
            foreach (string projectPath in projectPaths.NoAllocEnumerate())
            {
                Project project = MSBuildAPIUtility.GetProject(projectPath);
                try
                {
                    string usingNetSdk = project.GetPropertyValue("UsingMicrosoftNETSdk");
                    if (bool.TryParse(usingNetSdk, out bool b) && b)
                    {
                        if (!MSBuildAPIUtility.IsPackageReferenceProject(project))
                        {
                            logger.LogError(
                                string.Format(
                                    CultureInfo.CurrentCulture,
                                    Strings.Error_NotPRProject,
                                    project.FullPath));

                            continue;
                        }

                        string? assetsFilePath = project.GetPropertyValue(ProjectAssetsFile);
                        if (string.IsNullOrEmpty(assetsFilePath) || !File.Exists(assetsFilePath))
                        {
                            logger.LogError(
                                string.Format(
                                    CultureInfo.CurrentCulture,
                                    Strings.Error_AssetsFileNotFound,
                                    project.FullPath));
                            continue;
                        }

                        yield return (assetsFilePath, projectPath);
                    }
                    else
                    {
                        logger.LogMinimal(
                                string.Format(
                                    CultureInfo.CurrentCulture,
                                    Strings.WhyCommand_Message_NonSDKStyleProjectsAreNotSupported,
                                    project.GetPropertyValue("MSBuildProjectName")));
                    }
                }
                finally
                {
                    ProjectCollection.GlobalProjectCollection.UnloadProject(project);
                }
            }
        }

        /// <summary>
        /// Validates that the input 'path' argument is a valid path to a directory, solution file or project file.
        /// </summary>
        private static bool ValidatePathArgument(string path, ILoggerWithColor logger)
        {
            if (string.IsNullOrEmpty(path))
            {
                logger.LogError(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.WhyCommand_Error_ArgumentCannotBeEmpty,
                        "PROJECT|SOLUTION"));
                return false;
            }

            // Check that the input is a valid path
            string fullPath;
            try
            {
                fullPath = Path.GetFullPath(path);
            }
            catch (ArgumentException)
            {
                logger.LogError(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.WhyCommand_Error_ArgumentExceptionThrown,
                        string.Format(CultureInfo.CurrentCulture, Strings.Error_PathIsMissingOrInvalid, path)));
                return false;
            }

            // Check that the path is a directory, solution file or project file
            if (Directory.Exists(fullPath)
                || (File.Exists(fullPath)
                    && (XPlatUtility.IsSolutionFile(fullPath) || XPlatUtility.IsProjectFile(fullPath) || XPlatUtility.IsJsonFile(fullPath))))
            {
                return true;
            }
            else
            {
                logger.LogError(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.WhyCommand_Error_ArgumentExceptionThrown,
                        string.Format(CultureInfo.CurrentCulture, Strings.Error_PathIsMissingOrInvalid, path)));
                return false;
            }
        }

        private static bool ValidatePackageArgument(string package, ILoggerWithColor logger)
        {
            if (string.IsNullOrEmpty(package))
            {
                logger.LogError(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.WhyCommand_Error_ArgumentCannotBeEmpty,
                        "PACKAGE"));
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates that the input frameworks options have corresponding targets in the assets file. Outputs a warning message if a framework does not exist.
        /// </summary>
        private static void ValidateFrameworksOptionsExistInAssetsFile(LockFile assetsFile, List<string> inputFrameworks, ILoggerWithColor logger)
        {
            foreach (var frameworkAlias in inputFrameworks)
            {
                if (assetsFile.GetTarget(frameworkAlias, runtimeIdentifier: null) == null)
                {
                    logger.LogWarning(
                        string.Format(
                            CultureInfo.CurrentCulture,
                            Strings.WhyCommand_Warning_AssetsFileDoesNotContainSpecifiedTarget,
                            assetsFile.Path,
                            assetsFile.PackageSpec.Name,
                            frameworkAlias));
                }
            }
        }

        /// <summary>
        /// Returns the path to an assets file for the given project.
        /// </summary>
        /// <param name="assetsFilePath"></param>
        /// <param name="projectPath"></param>
        /// <param name="logger">Logger for the 'why' command</param>
        /// <returns>Assets file for the given project. Returns null if there was any issue finding or parsing the assets file.</returns>
        private static LockFile? GetProjectAssetsFile(string assetsFilePath, string? projectPath, ILoggerWithColor logger)
        {
            if (!File.Exists(assetsFilePath))
            {
                if (!string.IsNullOrEmpty(projectPath))
                {
                    logger.LogError(
                        string.Format(
                            CultureInfo.CurrentCulture,
                            Strings.Error_AssetsFileNotFound,
                            projectPath));
                }
                else
                {
                    logger.LogError(
                        string.Format(
                            CultureInfo.CurrentCulture,
                            Strings.Error_PathIsMissingOrInvalid,
                            assetsFilePath));
                }

                return null;
            }

            var lockFileFormat = new LockFileFormat();
            LockFile assetsFile = lockFileFormat.Read(assetsFilePath);

            // assets file validation
            if (assetsFile.PackageSpec == null
                || assetsFile.Targets == null
                || assetsFile.Targets.Count == 0)
            {
                if (string.IsNullOrEmpty(projectPath))
                {
                    logger.LogError(
                        string.Format(
                            CultureInfo.CurrentCulture,
                            Strings.WhyCommand_Error_InvalidAssetsFile_WithoutProject,
                            assetsFilePath));
                }
                else
                {
                    logger.LogError(
                        string.Format(
                            CultureInfo.CurrentCulture,
                            Strings.WhyCommand_Error_InvalidAssetsFile_WithProject,
                            assetsFilePath,
                            projectPath));
                }

                return null;
            }

            return assetsFile;
        }
    }
}

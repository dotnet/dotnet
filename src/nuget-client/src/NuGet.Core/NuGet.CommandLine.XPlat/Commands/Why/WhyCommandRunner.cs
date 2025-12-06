// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Build.Evaluation;
using NuGet.ProjectModel;
using Spectre.Console;

namespace NuGet.CommandLine.XPlat.Commands.Why
{
    internal static class WhyCommandRunner
    {
        private const string ProjectAssetsFile = "ProjectAssetsFile";

        /// <summary>
        /// Executes the 'why' command.
        /// </summary>
        /// <param name="whyCommandArgs">CLI arguments for the 'why' command.</param>
        public static Task<int> ExecuteCommand(WhyCommandArgs whyCommandArgs)
        {
            bool validArgumentsUsed = ValidatePathArgument(whyCommandArgs.Path, whyCommandArgs.Logger)
                                        && ValidatePackageArgument(whyCommandArgs.Package, whyCommandArgs.Logger);
            if (!validArgumentsUsed)
            {
                return Task.FromResult(ExitCodes.InvalidArguments);
            }

            string targetPackage = whyCommandArgs.Package;
            IEnumerable<(string assetsFilePath, string? projectPath)> assetsFiles;
            try
            {
                assetsFiles = FindAssetsFiles(whyCommandArgs.Path, whyCommandArgs.Logger);
            }
            catch (ArgumentException ex)
            {
                string message = string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.WhyCommand_Error_ArgumentExceptionThrown,
                    ex.Message);
                whyCommandArgs.Logger.MarkupLine($"[red]{message}[/]");

                return Task.FromResult(ExitCodes.InvalidArguments);
            }

            bool anyErrors = false;
            foreach ((string assetsFilePath, string? projectPath) in assetsFiles)
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
                        whyCommandArgs.Logger.WriteLine(
                            string.Format(CultureInfo.CurrentCulture,
                                Strings.WhyCommand_Message_DependencyGraphsFoundInProject,
                                assetsFile.PackageSpec.Name,
                                targetPackage));

                        DependencyGraphPrinter.PrintAllDependencyGraphs(dependencyGraphPerFramework, targetPackage, whyCommandArgs.Logger);
                    }
                    else
                    {
                        whyCommandArgs.Logger.WriteLine(
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

            return Task.FromResult(anyErrors ? ExitCodes.Error : ExitCodes.Success);
        }

        private static IEnumerable<(string assetsFilePath, string? projectPath)> FindAssetsFiles(string path, IAnsiConsole logger)
        {
            if (XPlatUtility.IsJsonFile(path))
            {
                yield return (path, null);
                yield break;
            }

            var projectPaths = MSBuildAPIUtility.GetListOfProjectsFromPathArgument(path);
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
                            string message = string.Format(
                                CultureInfo.CurrentCulture,
                                Strings.Error_NotPRProject,
                                project.FullPath);
                            logger.MarkupLine($"[red]{message}[/]");
                            continue;
                        }

                        string? assetsFilePath = project.GetPropertyValue(ProjectAssetsFile);
                        if (string.IsNullOrEmpty(assetsFilePath) || !File.Exists(assetsFilePath))
                        {
                            string message = string.Format(
                                CultureInfo.CurrentCulture,
                                Strings.Error_AssetsFileNotFound,
                                project.FullPath);
                            logger.MarkupLine($"[red]{message}[/]");
                            continue;
                        }

                        yield return (assetsFilePath, projectPath);
                    }
                    else
                    {
                        logger.WriteLine(
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
        private static bool ValidatePathArgument(string path, IAnsiConsole logger)
        {
            if (string.IsNullOrEmpty(path))
            {
                string message = string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.WhyCommand_Error_ArgumentCannotBeEmpty,
                    "PROJECT|SOLUTION");
                logger.MarkupLine($"[red]{message}[/]");
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
                string message = string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.WhyCommand_Error_ArgumentExceptionThrown,
                    string.Format(CultureInfo.CurrentCulture, Strings.Error_PathIsMissingOrInvalid, path));
                logger.MarkupLine($"[red]{message}[/]");
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
                string message = string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.WhyCommand_Error_ArgumentExceptionThrown,
                    string.Format(CultureInfo.CurrentCulture, Strings.Error_PathIsMissingOrInvalid, path));
                logger.MarkupLine($"[red]{message}[/]");
                return false;
            }
        }

        private static bool ValidatePackageArgument(string package, IAnsiConsole logger)
        {
            if (string.IsNullOrEmpty(package))
            {
                string message = string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.WhyCommand_Error_ArgumentCannotBeEmpty,
                    "PACKAGE");
                logger.MarkupLine($"[red]{message}[/]");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates that the input frameworks options have corresponding targets in the assets file. Outputs a warning message if a framework does not exist.
        /// </summary>
        private static void ValidateFrameworksOptionsExistInAssetsFile(LockFile assetsFile, List<string> inputFrameworks, IAnsiConsole logger)
        {
            foreach (var frameworkAlias in inputFrameworks)
            {
                if (assetsFile.GetTarget(frameworkAlias, runtimeIdentifier: null) == null)
                {
                    string message = string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.WhyCommand_Warning_AssetsFileDoesNotContainSpecifiedTarget,
                        assetsFile.Path,
                        assetsFile.PackageSpec.Name,
                        frameworkAlias);
                    logger.MarkupLine($"[yellow]{message}[/]");
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
        private static LockFile? GetProjectAssetsFile(string assetsFilePath, string? projectPath, IAnsiConsole logger)
        {
            if (!File.Exists(assetsFilePath))
            {
                if (!string.IsNullOrEmpty(projectPath))
                {
                    string message = string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.Error_AssetsFileNotFound,
                        projectPath);
                    logger.MarkupLine($"[red]{message}[/]");
                }
                else
                {
                    string message = string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.Error_PathIsMissingOrInvalid,
                        assetsFilePath);
                    logger.MarkupLine($"[red]{message}[/]");
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
                    string message = string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.WhyCommand_Error_InvalidAssetsFile_WithoutProject,
                        assetsFilePath);
                    logger.MarkupLine($"[red]{message}[/]");
                }
                else
                {
                    string message = string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.WhyCommand_Error_InvalidAssetsFile_WithProject,
                        assetsFilePath,
                        projectPath);
                    logger.MarkupLine($"[red]{message}[/]");
                }

                return null;
            }

            return assetsFile;
        }
    }
}

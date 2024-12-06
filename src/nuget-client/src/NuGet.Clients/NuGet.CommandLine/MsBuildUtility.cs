// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.VisualStudio.Setup.Configuration;
using NuGet.Commands;
using NuGet.Common;
using NuGet.ProjectModel;

namespace NuGet.CommandLine
{
    public static class MsBuildUtility
    {
        internal const int MsBuildWaitTime = 2 * 60 * 1000; // 2 minutes in milliseconds

        private const string NuGetProps = "NuGet.CommandLine.NuGet.props";
        private const string NuGetTargets = "NuGet.CommandLine.NuGet.targets";
        private static readonly XNamespace MSBuildNamespace = XNamespace.Get("http://schemas.microsoft.com/developer/msbuild/2003");

        private readonly static string[] MSBuildVersions = new string[] { "14", "12", "4" };

        private readonly static string[] ArchitectureFolderNames = new string[] { "arm64", "amd64" };
        public static bool IsMsBuildBasedProject(string projectFullPath)
        {
            return projectFullPath.EndsWith("proj", StringComparison.OrdinalIgnoreCase);
        }

        public static int Build(string msbuildDirectory,
                                    string args)
        {
            var msbuildPath = GetMsbuild(msbuildDirectory);

            if (!File.Exists(msbuildPath))
            {
                throw new CommandException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        LocalizedResourceManager.GetString(nameof(NuGetResources.MsBuildDoesNotExistAtPath)),
                        msbuildPath));
            }

            var processStartInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                FileName = msbuildPath,
                Arguments = args,
                RedirectStandardOutput = false,
                RedirectStandardError = false
            };

            using (var process = Process.Start(processStartInfo))
            {
                process.WaitForExit();

                return process.ExitCode;
            }
        }

        /// <summary>
        /// Returns the closure of project references for projects specified in <paramref name="projectPaths"/>.
        /// </summary>
        public static async Task<DependencyGraphSpec> GetProjectReferencesAsync(
            MsBuildToolset msbuildToolset,
            string[] projectPaths,
            int timeOut,
            IConsole console,
            bool recursive,
            string solutionDirectory,
            string solutionName,
            string restoreConfigFile,
            string[] sources,
            string packagesDirectory,
            RestoreLockProperties restoreLockProperties)
        {
            var msbuildPath = GetMsbuild(msbuildToolset.Path);

            if (!File.Exists(msbuildPath))
            {
                throw new CommandException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        LocalizedResourceManager.GetString(nameof(NuGetResources.MsBuildDoesNotExistAtPath)),
                        msbuildPath));
            }

            var nugetExePath = Assembly.GetEntryAssembly().Location;

            // Check for the non-ILMerged path
            var buildTasksPath = Path.Combine(Path.GetDirectoryName(nugetExePath), "NuGet.Build.Tasks.dll");

            if (File.Exists(buildTasksPath))
            {
                nugetExePath = buildTasksPath;
            }

            using (var inputTargetPath = new TempFile(".nugetinputs.targets"))
            using (var nugetPropsPath = new TempFile(".nugetrestore.props"))
            using (var entryPointTargetPath = new TempFile(".nugetrestore.targets"))
            using (var resultsPath = new TempFile(".output.dg"))
            {
                // Read NuGet.props and NuGet.targets from nuget.exe and write it to disk for msbuild.exe
                ExtractResource(NuGetProps, nugetPropsPath);
                ExtractResource(NuGetTargets, entryPointTargetPath);

                // Build a .targets file of all restore inputs, this is needed to avoid going over the limit on command line arguments.
                var properties = new Dictionary<string, string>()
                {
                    { "RestoreUseCustomAfterTargets", "true" },
                    { "RestoreGraphOutputPath", resultsPath },
                    { "RestoreRecursive", recursive.ToString(CultureInfo.CurrentCulture).ToLowerInvariant() },
                    { "RestoreProjectFilterMode", "exclusionlist" },
                };

                var inputTargetXML = GetRestoreInputFile(entryPointTargetPath, properties, projectPaths);

                inputTargetXML.Save(inputTargetPath);

                // Create msbuild parameters and include global properties that cannot be set in the input targets path
                var arguments = GetMSBuildArguments(entryPointTargetPath, nugetPropsPath, inputTargetPath, nugetExePath, solutionDirectory, solutionName, restoreConfigFile, sources, packagesDirectory, msbuildToolset, restoreLockProperties, EnvironmentVariableWrapper.Instance);

                var processStartInfo = new ProcessStartInfo
                {
                    UseShellExecute = false,
                    FileName = msbuildPath,
                    Arguments = arguments,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true
                };

                console.LogDebug($"{processStartInfo.FileName} {processStartInfo.Arguments}");

                using (var process = Process.Start(processStartInfo))
                {
                    var errors = new StringBuilder();
                    var output = new StringBuilder();
                    var excluded = new string[] { "msb4011", entryPointTargetPath };

                    // Read console output
                    var errorTask = ConsumeStreamReaderAsync(process.StandardError, errors, filter: null);
                    var outputTask = ConsumeStreamReaderAsync(process.StandardOutput, output, filter: (line) => IsIgnoredOutput(line, excluded));

                    // Run msbuild
                    var finished = process.WaitForExit(timeOut);

                    // Handle timeouts
                    if (!finished)
                    {
                        try
                        {
                            process.Kill();
                        }
                        catch (Exception ex)
                        {
                            throw new CommandException(
                                LocalizedResourceManager.GetString(nameof(NuGetResources.Error_CannotKillMsBuild)) + " : " +
                                ex.Message,
                                ex);
                        }
                    }

                    // Read all console output from msbuild.
                    await Task.WhenAll(outputTask, errorTask);

                    // By default log msbuild output so that it is only
                    // displayed under -Verbosity detailed
                    var logLevel = LogLevel.Verbose;

                    if (process.ExitCode != 0 || !finished)
                    {
                        // If a problem occurred log all msbuild output as an error
                        // so that the user can see it.
                        // By default this runs with /v:q which means that only
                        // errors and warnings will be in the output.
                        logLevel = LogLevel.Error;
                    }

                    // MSBuild writes errors to the output stream, parsing the console output to find
                    // the errors would be error prone so here we log all output combined with any
                    // errors on the error stream (haven't seen the error stream used to date)
                    // to give the user the complete info.
                    await console.LogAsync(logLevel, output.ToString() + errors.ToString());

                    if (!finished)
                    {
                        // MSBuild timed out
                        throw new CommandException(
                                LocalizedResourceManager.GetString(nameof(NuGetResources.Error_MsBuildTimedOut)));
                    }

                    await outputTask;

                    if (process.ExitCode != 0)
                    {
                        // Do not continue if msbuild failed.
                        throw new ExitCodeException(1);
                    }
                }

                DependencyGraphSpec spec = null;

                if (File.Exists(resultsPath) && new FileInfo(resultsPath).Length != 0)
                {
                    spec = DependencyGraphSpec.Load(resultsPath);
                    File.Delete(resultsPath);
                }
                else
                {
                    spec = new DependencyGraphSpec();
                }

                return spec;
            }
        }

        public static string GetMSBuildArguments(
            string entryPointTargetPath,
            string nugetPropsPath,
            string inputTargetPath,
            string nugetExePath,
            string solutionDirectory,
            string solutionName,
            string restoreConfigFile,
            string[] sources,
            string packagesDirectory,
            MsBuildToolset toolset,
            RestoreLockProperties restoreLockProperties,
            IEnvironmentVariableReader reader)
        {
            // args for MSBuild.exe
            var args = new List<string>()
            {
                EscapeQuoted(inputTargetPath),
                "/t:GenerateRestoreGraphFile",
                "/nologo",
                "/nr:false"
            };

            // Set the msbuild verbosity level if specified
            var msbuildVerbosity = reader.GetEnvironmentVariable("NUGET_RESTORE_MSBUILD_VERBOSITY");

            if (string.IsNullOrEmpty(msbuildVerbosity))
            {
                args.Add("/v:q");
            }
            else
            {
                args.Add($"/v:{msbuildVerbosity} ");
            }

            AddProperty(args, "NuGetPropsFile", nugetPropsPath);

            // Override the target under ImportsAfter with the current NuGet.targets version.
            AddProperty(args, "NuGetRestoreTargets", entryPointTargetPath);
            AddProperty(args, "RestoreUseCustomAfterTargets", bool.TrueString);
            AddProperty(args, "DisableCheckingDuplicateNuGetItems", bool.TrueString);

            // Set path to nuget.exe or the build task
            AddProperty(args, "RestoreTaskAssemblyFile", nugetExePath);

            // Settings
            AddRestoreSources(args, sources);
            AddPropertyIfHasValue(args, "RestoreSolutionDirectory", solutionDirectory);
            AddPropertyIfHasValue(args, "RestoreConfigFile", restoreConfigFile);
            AddPropertyIfHasValue(args, "RestorePackagesPath", packagesDirectory);
            AddPropertyIfHasValue(args, "SolutionDir", solutionDirectory);
            AddPropertyIfHasValue(args, "SolutionName", solutionName);

            // If the MSBuild version used does not support SkipNonextentTargets and BuildInParallel
            // use the performance optimization
            // When BuildInParallel is used with ContinueOnError it does not continue in some scenarios
            if (toolset.ParsedVersion.CompareTo(new Version(15, 5)) < 0)
            {
                AddProperty(args, "RestoreBuildInParallel", bool.FalseString);
                AddProperty(args, "RestoreUseSkipNonexistentTargets", bool.FalseString);
            }

            // Checking the SDK uses MSBuild intrinsic functions that were added in 16.5
            if (toolset.ParsedVersion.CompareTo(new Version(16, 5)) < 0)
            {
                AddProperty(args, "NuGetExeSkipSdkAnalysisLevelCheck", bool.TrueString);
            }

            // Add additional args to msbuild if needed
            var msbuildAdditionalArgs = reader.GetEnvironmentVariable("NUGET_RESTORE_MSBUILD_ARGS");

            if (!string.IsNullOrEmpty(msbuildAdditionalArgs))
            {
                args.Add(msbuildAdditionalArgs);
            }

            AddPropertyIfHasValue(args, "RestorePackagesWithLockFile", restoreLockProperties.RestorePackagesWithLockFile);
            AddPropertyIfHasValue(args, "NuGetLockFilePath", restoreLockProperties.NuGetLockFilePath);
            if (restoreLockProperties.RestoreLockedMode)
            {
                AddProperty(args, "RestoreLockedMode", bool.TrueString);
            }

            return string.Join(" ", args);
        }

        private static void AddRestoreSources(List<string> args, string[] sources)
        {
            if (sources.Length != 0)
            {
                var isMono = RuntimeEnvironmentHelper.IsMono && !RuntimeEnvironmentHelper.IsWindows;

                var sourceBuilder = new StringBuilder();

                if (isMono)
                {
                    sourceBuilder.Append("/p:RestoreSources=\\\"");
                }
                else
                {
                    sourceBuilder.Append("/p:RestoreSources=\"");
                }

                for (var i = 0; i < sources.Length; i++)
                {
                    if (isMono)
                    {
                        sourceBuilder.Append(sources[i])
                            .Append("\\;");
                    }
                    else
                    {
                        sourceBuilder.Append(sources[i])
                            .Append(";");
                    }
                }

                if (isMono)
                {
                    sourceBuilder.Append("\\\" ");
                }
                else
                {
                    sourceBuilder.Append("\" ");
                }

                args.Add(sourceBuilder.ToString());
            }
        }

        public static XDocument GetRestoreInputFile(string restoreTargetPath, Dictionary<string, string> properties, IEnumerable<string> projectPaths)
        {
            return GenerateMSBuildFile(
                new XElement(MSBuildNamespace + "PropertyGroup", properties.Select(e => new XElement(MSBuildNamespace + e.Key, e.Value))),
                new XElement(MSBuildNamespace + "ItemGroup", projectPaths.Select(GetRestoreGraphProjectInputItem)),
                new XElement(MSBuildNamespace + "Import", new XAttribute(XName.Get("Project"), restoreTargetPath)));
        }

        public static XDocument GenerateMSBuildFile(params XElement[] elements)
        {
            return new XDocument(
                new XDeclaration("1.0", "utf-8", "no"),
                new XElement(MSBuildNamespace + "Project",
                    new XAttribute("ToolsVersion", "14.0"),
                    elements));
        }

        private static XElement GetRestoreGraphProjectInputItem(string path)
        {
            return new XElement(MSBuildNamespace + "RestoreGraphProjectInputItems", new XAttribute(XName.Get("Include"), path));
        }

        private static bool IsIgnoredOutput(string line, string[] excluded)
        {
            return excluded.All(p => line.IndexOf(p, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private static async Task ConsumeStreamReaderAsync(StreamReader reader, StringBuilder lines, Func<string, bool> filter)
        {
            await Task.Yield();

            string line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                if (filter == null ||
                    !filter(line))
                {
                    lines.AppendLine(line);
                }
            }
        }

        /// <summary>
        /// Gets the list of project files in a solution, using XBuild's solution parser.
        /// </summary>
        /// <param name="solutionFile">The solution file. </param>
        /// <returns>The list of project files (in full path) in the solution.</returns>
        public static IEnumerable<string> GetAllProjectFileNamesWithXBuild(string solutionFile)
        {
            try
            {
                var assembly = Assembly.Load(
                    "Microsoft.Build.Engine, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
                var solutionParserType = assembly.GetType("Mono.XBuild.CommandLine.SolutionParser");
                if (solutionParserType == null)
                {
                    throw new CommandException(
                        LocalizedResourceManager.GetString("Error_CannotGetXBuildSolutionParser"));
                }

                var getAllProjectFileNamesMethod = solutionParserType.GetMethod(
                    "GetAllProjectFileNames",
                    new Type[] { typeof(string) });
                if (getAllProjectFileNamesMethod == null)
                {
                    throw new CommandException(
                        LocalizedResourceManager.GetString("Error_CannotGetGetAllProjectFileNamesMethod"));
                }

                var names = (IEnumerable<string>)getAllProjectFileNamesMethod.Invoke(
                    null, new object[] { solutionFile });
                return names;
            }
            catch (Exception ex)
            {
                var message = string.Format(
                    CultureInfo.CurrentCulture,
                    LocalizedResourceManager.GetString("Error_SolutionFileParseError"),
                    solutionFile,
                    ex.Message);

                throw new CommandException(message, ex);
            }
        }

        /// <summary>
        /// Gets the list of project files in a solution, using MSBuild API.
        /// </summary>
        /// <param name="solutionFile">The solution file. </param>
        /// <param name="msbuildPath">The directory that contains msbuild.</param>
        /// <returns>The list of project files (in full path) in the solution.</returns>
        public static IEnumerable<string> GetAllProjectFileNamesWithMsBuild(
            string solutionFile,
            string msbuildPath)
        {
            try
            {
                var solution = new Solution(solutionFile, msbuildPath);
                return solution.Projects.Where(project => !project.IsSolutionFolder)
                    .Select(project => project.AbsolutePath);
            }
            catch (Exception ex)
            {
                var exMessage = ex.Message;
                if (ex.InnerException != null)
                    exMessage += "  " + ex.InnerException.Message;
                var message = string.Format(
                    CultureInfo.CurrentCulture,
                    LocalizedResourceManager.GetString("Error_SolutionFileParseError"),
                    solutionFile,
                    exMessage);

                throw new CommandException(message, ex);
            }
        }

        public static IEnumerable<string> GetAllProjectFileNames(
            string solutionFile,
            string pathToMsbuildDir)
        {
            if (RuntimeEnvironmentHelper.IsMono &&
                (pathToMsbuildDir.Contains("xbuild") || GetMsbuild(pathToMsbuildDir).Contains("xbuild")))
            {
                return GetAllProjectFileNamesWithXBuild(solutionFile);
            }

            return GetAllProjectFileNamesWithMsBuild(solutionFile, pathToMsbuildDir);
        }

        /// <summary>
        /// Returns the msbuild directory. If <paramref name="userVersion"/> is "latest", then the directory containing
        /// the highest installed msbuild version is returned. If <paramref name="userVersion"/> is null,
        /// the Env variable has priority over the highest installed version. Otherwise, the directory containing msbuild
        /// whose version matches <paramref name="userVersion"/> is returned. If no match is found,
        /// an exception will be thrown. Note that we use Microsoft.Build types as
        /// </summary>
        /// <param name="userVersion">version string as passed by user (so may be empty)</param>
        /// <param name="console">The console used to output messages.</param>
        /// <returns>The msbuild directory.</returns>
        public static MsBuildToolset GetMsBuildToolset(string userVersion, IConsole console)
        {
            var currentDirectoryCache = Directory.GetCurrentDirectory();
            var installedToolsets = new List<MsBuildToolset>();
            MsBuildToolset toolset = null;

            try
            {
                // If Mono, test well known paths and bail if found
                toolset = GetMsBuildFromMonoPaths(userVersion);
                if (toolset != null)
                {
                    return toolset;
                }

                // If the userVersion is not specified, favor the value in the $Path Env variable
                if (string.IsNullOrEmpty(userVersion))
                {
                    var msbuildExe = GetMSBuild(EnvironmentVariableWrapper.Instance);

                    if (msbuildExe != null)
                    {
                        var msBuildDirectory = GetNonArchitectureDirectory(msbuildExe);
                        var msbuildVersion = FileVersionInfo.GetVersionInfo(msbuildExe)?.FileVersion;
                        return toolset = new MsBuildToolset(msbuildVersion, msBuildDirectory);
                    }
                }

                using (var projectCollection = LoadProjectCollection())
                {
                    var installed = ((dynamic)projectCollection)?.Toolsets;
                    if (installed != null)
                    {
                        foreach (var item in installed)
                        {
                            installedToolsets.Add(new MsBuildToolset(version: item.ToolsVersion, path: item.ToolsPath));
                        }

                        installedToolsets = installedToolsets.ToList();
                    }
                }

                // In a non-Mono environment, we have the potential for SxS installs of MSBuild 15.1+. Let's add these here.
                if (!RuntimeEnvironmentHelper.IsMono)
                {
                    var installedSxsToolsets = GetInstalledSxsToolsets();
                    if (installedToolsets == null)
                    {
                        installedToolsets = installedSxsToolsets;
                    }
                    else if (installedSxsToolsets != null)
                    {
                        installedToolsets.AddRange(installedSxsToolsets);
                    }
                }

                if (!installedToolsets.Any())
                {
                    throw new CommandException(
                        LocalizedResourceManager.GetString(
                            nameof(NuGetResources.Error_CannotFindMsbuild)));
                }

                toolset = GetMsBuildDirectoryInternal(
                    userVersion, console, installedToolsets.OrderByDescending(t => t), (IEnvironmentVariableReader reader) => GetMSBuild(reader));

                Directory.SetCurrentDirectory(currentDirectoryCache);
                return toolset;
            }
            finally
            {
                LogToolsetToConsole(console, toolset);
            }
        }

        internal static string GetNonArchitectureDirectory(string msbuildExe)
        {
            var msbuildFile = Path.GetFileName(msbuildExe);
            var directory = Path.GetDirectoryName(msbuildExe);
            var directoryInfo = new DirectoryInfo(directory);
            var directoryName = directoryInfo.Name;
            var parentDirectory = directoryInfo.Parent.FullName;

            //Given Visual Studio 2022 or later, the PATH environment variable in Developer Command Prompt contains the architecture specific path of msbuild.exe.
            // e.g. C:\Program Files\Microsoft Visual Studio\2022\Preview\\MSBuild\Current\Bin\arm64
            //Using the architecture specific path will cause some runtime error when loading assembly, e.g."Microsoft.Build.Framework.dll".
            //
            //This method is to get the non-architecture specific path of msbuild.exe if the msbuildexe is in the architecture specific folder.
            //     C:\Program Files\Microsoft Visual Studio\2022\Preview\\MSBuild\Current\Bin\arm64
            //  => C:\Program Files\Microsoft Visual Studio\2022\Preview\\MSBuild\Current\Bin
            //If msbuildExe is already in the non-architecture specific folder, just return the directory.
            foreach (var architecture in ArchitectureFolderNames)
            {
                if (directoryName.Equals(architecture, StringComparison.OrdinalIgnoreCase))
                {
                    if (File.Exists(Path.Combine(parentDirectory, msbuildFile)))
                    {
                        return parentDirectory;
                    }
                    else
                    {
                        throw new CommandException(
                            string.Format(
                                CultureInfo.CurrentCulture,
                                LocalizedResourceManager.GetString(nameof(NuGetResources.Error_CannotFindNonArchitectureSpecificMsbuild)),
                                directory));
                    }
                }
            }
            return directory;
        }

        /// <summary>
        /// This method is called by GetMsBuildToolset(). This method is not intended to be called directly.
        /// It's marked public so that it can be called by unit tests.
        /// </summary>
        /// <param name="userVersion">version string as passed by user (so may be empty)</param>
        /// <param name="console">console for status reporting</param>
        /// <param name="installedToolsets">all msbuild toolsets discovered by caller</param>
        /// <param name="getMsBuildPathInPathVar">delegate to provide msbuild exe discovered in path environemtnb var/s
        /// (using a delegate allows for testability)</param>
        /// <returns>directory to use for msbuild exe</returns>
        public static MsBuildToolset GetMsBuildDirectoryInternal(
            string userVersion,
            IConsole console,
            IEnumerable<MsBuildToolset> installedToolsets,
            Func<IEnvironmentVariableReader, string> getMsBuildPathInPathVar)
        {
            MsBuildToolset toolset;

            var toolsetsContainingMSBuild = GetToolsetsContainingValidMSBuildInstallation(installedToolsets);

            if (string.Equals(userVersion, "latest", StringComparison.OrdinalIgnoreCase))
            {
                //If "latest", take the default(highest) path, ignoring $PATH
                toolset = toolsetsContainingMSBuild.FirstOrDefault();
            }
            else if (string.IsNullOrEmpty(userVersion))
            {
                var msbuildPathInPath = getMsBuildPathInPathVar(EnvironmentVariableWrapper.Instance);
                toolset = GetToolsetFromPath(msbuildPathInPath, toolsetsContainingMSBuild);
            }
            else
            {
                toolset = GetToolsetFromUserVersion(userVersion, toolsetsContainingMSBuild);
            }

            return toolset;
        }

        private static IEnumerable<MsBuildToolset> GetToolsetsContainingValidMSBuildInstallation(IEnumerable<MsBuildToolset> installedToolsets)
        {
            return installedToolsets.Where(e => e.IsValid);
        }

        /// <summary>
        /// Fetch project collection type from the GAC--this will service MSBuild 14 (and any toolsets included with 14).
        /// </summary>
        /// <returns>ProjectCollection instance to use for toolset enumeration</returns>
        private static IDisposable LoadProjectCollection()
        {
            foreach (var version in MSBuildVersions)
            {
                try
                {
                    var msBuildTypesAssembly = Assembly.Load($"Microsoft.Build, Version={version}.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
                    var projectCollectionType = msBuildTypesAssembly.GetType("Microsoft.Build.Evaluation.ProjectCollection", throwOnError: true);
                    return Activator.CreateInstance(projectCollectionType) as IDisposable;
                }
                catch (Exception)
                {
                }
            }

            return null;
        }

        /// <summary>
        /// Try to find msbuild for mono from hard code path
        /// </summary>
        /// <param name="userVersion">version string as passed by user (so may be empty)</param>
        /// <returns></returns>
        public static MsBuildToolset GetMsBuildFromMonoPaths(string userVersion)
        {
            // Mono always tell user we are on unix even when user is on Mac.
            if (!RuntimeEnvironmentHelper.IsMono)
            {
                return null;
            }
            //Use mscorlib to find mono and msbuild directory
            var systemLibLocation = Path.GetDirectoryName(typeof(object).Assembly.Location);
            var msbuildBasePathOnMono = Path.GetFullPath(Path.Combine(systemLibLocation, "..", "msbuild"));
            //Combine msbuild version paths
            var msBuildPathOnMono14 = Path.Combine(msbuildBasePathOnMono, "14.1", "bin");
            var msBuildPathOnMono15 = Path.Combine(msbuildBasePathOnMono, "15.0", "bin");
            if (string.IsNullOrEmpty(userVersion) || string.Equals(userVersion, "latest", StringComparison.OrdinalIgnoreCase))
            {
                return new[] {
                        new MsBuildToolset(version: "15.0", path: msBuildPathOnMono15),
                        new MsBuildToolset(version: "14.1", path: msBuildPathOnMono14)}
                    .FirstOrDefault(t => Directory.Exists(t.Path));
            }
            else
            {
                switch (userVersion)
                {
                    case "14.1": return new MsBuildToolset(version: "14.1", path: msBuildPathOnMono14);
                    case "15":
                    case "15.0": return new MsBuildToolset(version: userVersion, path: msBuildPathOnMono15);
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the (first) path of MSBuild to appear in environment variable PATH.
        /// </summary>
        /// <returns>The path of MSBuild in PATH environment variable. Returns null if MSBuild location does not exist
        /// in the variable string.</returns>
        private static string GetMsBuildPathInPathVar(IEnvironmentVariableReader reader)
        {
            var path = reader.GetEnvironmentVariable("PATH");
            var paths = path?.Split(new char[] { ';' });
            return paths?.Select(p =>
            {
                // Strip leading/trailing quotes
                if (p.Length > 0 && p[0] == '\"')
                {
                    p = p.Substring(1);
                }
                if (p.Length > 0 && p[p.Length - 1] == '\"')
                {
                    p = p.Substring(0, p.Length - 1);
                }

                return p;
            }).FirstOrDefault(p =>
            {
                try
                {
                    return File.Exists(Path.Combine(p, "msbuild.exe"));
                }
                catch
                {
                    return false;
                }
            });
        }

        /// <summary>
        /// Gets the msbuild toolset found in/under the path passed.
        /// </summary>
        /// <param name="msBuildPath">The msbuild path as found in PATH env var. Can be null.</param>
        /// <param name="installedToolsets">List of installed toolsets,
        /// ordered by ToolsVersion, from highest to lowest.</param>
        /// <returns>The matching toolset.</returns>
        private static MsBuildToolset GetToolsetFromPath(
            string msBuildPath,
            IEnumerable<MsBuildToolset> installedToolsets)
        {
            MsBuildToolset selectedToolset;
            if (string.IsNullOrEmpty(msBuildPath))
            {
                // We have no path for a specifically requested msbuild. Use the highest installed version.
                selectedToolset = installedToolsets.FirstOrDefault();
            }
            else
            {
                // Search by path. We use a StartsWith match because a toolset's path may have an architecture specialization.
                // e.g.
                //     c:\Program Files (x86)\MSBuild\14.0\Bin
                // is specified in the path (a path which we have validated contains an msbuild.exe) and the toolset is located at
                //     c:\Program Files (x86)\MSBuild\14.0\Bin\amd64
                selectedToolset = installedToolsets.FirstOrDefault(
                    t => t.Path.StartsWith(msBuildPath, StringComparison.OrdinalIgnoreCase));

                if (selectedToolset == null)
                {
                    // No match. Fail silently. Use the highest installed version in this case
                    selectedToolset = installedToolsets.FirstOrDefault();
                }
            }

            if (selectedToolset == null)
            {
                throw new CommandException(
                    LocalizedResourceManager.GetString(
                            nameof(NuGetResources.Error_MSBuildNotInstalled)));
            }

            return selectedToolset;
        }

        private static MsBuildToolset GetToolsetFromUserVersion(
            string userVersion,
            IEnumerable<MsBuildToolset> installedToolsets)
        {
            // Version.TryParse only take decimal string like "14.0", "14" need to be converted.
            var versionParts = userVersion.Split('.');
            var major = versionParts.Length > 0 ? versionParts[0] : "0";
            var minor = versionParts.Length > 1 ? versionParts[1] : "0";

            var userVersionString = string.Join(".", major, minor);

            // First match by string comparison
            var selectedToolset = installedToolsets.FirstOrDefault(
                t => string.Equals(userVersion, t.Version, StringComparison.OrdinalIgnoreCase));

            if (selectedToolset != null)
            {
                return selectedToolset;
            }

            // Then match by Major & Minor version numbers. And we want an actual parsing of t.ToolsVersion,
            // without the safe fallback to 0.0 built into t.ParsedToolsVersion.
            selectedToolset = installedToolsets.FirstOrDefault(t =>
            {
                Version parsedUserVersion;
                Version parsedToolsVersion;
                if (Version.TryParse(userVersionString, out parsedUserVersion) &&
                    Version.TryParse(t.Version, out parsedToolsVersion))
                {
                    return parsedToolsVersion.Major == parsedUserVersion.Major &&
                        parsedToolsVersion.Minor == parsedUserVersion.Minor;
                }

                return false;
            });

            if (selectedToolset == null)
            {
                var message = string.Format(
                    CultureInfo.CurrentCulture,
                    LocalizedResourceManager.GetString(
                        nameof(NuGetResources.Error_CannotFindMsbuild)),
                    userVersion);

                throw new CommandException(message);
            }

            return selectedToolset;
        }

        private static void LogToolsetToConsole(IConsole console, MsBuildToolset toolset)
        {
            if (console == null || toolset == null)
            {
                return;
            }

            if (console.Verbosity == Verbosity.Detailed)
            {
                console.WriteLine(
                    LocalizedResourceManager.GetString(
                        nameof(NuGetResources.MSBuildAutoDetection_Verbose)),
                    toolset.Version,
                    toolset.Path);
            }
            else
            {
                console.WriteLine(
                    LocalizedResourceManager.GetString(
                        nameof(NuGetResources.MSBuildAutoDetection)),
                    toolset.Version,
                    toolset.Path);
            }
        }

        public static Lazy<MsBuildToolset> GetMsBuildDirectoryFromMsBuildPath(string msbuildPath, string msbuildVersion, IConsole console)
        {
            if (msbuildPath != null)
            {
                if (msbuildVersion != null)
                {
                    console?.WriteWarning(LocalizedResourceManager.GetString(
                        nameof(NuGetResources.Warning_MsbuildPath)),
                        msbuildPath, msbuildVersion);
                }

                console?.WriteLine(LocalizedResourceManager.GetString(
                               nameof(NuGetResources.MSbuildFromPath)),
                           msbuildPath);

                if (!Directory.Exists(msbuildPath))
                {
                    var message = string.Format(
                        CultureInfo.CurrentCulture,
                        LocalizedResourceManager.GetString(
                            nameof(NuGetResources.MsbuildPathNotExist)),
                        msbuildPath);

                    throw new CommandException(message);
                }

                return new Lazy<MsBuildToolset>(() => new MsBuildToolset(msbuildVersion, msbuildPath));
            }
            else
            {
                return new Lazy<MsBuildToolset>(() => GetMsBuildToolset(msbuildVersion, console));
            }
        }

        private static void AddProperty(List<string> args, string property, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException(NuGetResources.ArgumentNullOrEmpty, nameof(value));
            }

            AddPropertyIfHasValue(args, property, value);
        }

        private static void AddPropertyIfHasValue(List<string> args, string property, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                args.Add($"/p:{property}={EscapeQuoted(value)}");
            }
        }

        private static void ExtractResource(string resourceName, string targetPath)
        {
            using (var input = typeof(MsBuildUtility).Assembly.GetManifestResourceStream(resourceName))
            {
                using (var output = File.OpenWrite(targetPath))
                {
                    input.CopyTo(output);
                }
            }
        }

        private static List<MsBuildToolset> GetInstalledSxsToolsets()
        {
            ISetupConfiguration configuration;
            try
            {
                configuration = new SetupConfiguration();
            }
            catch (Exception)
            {
                return null; // No COM class
            }

            var enumerator = configuration.EnumInstances();
            if (enumerator == null)
            {
                return null;
            }

            var setupInstances = new List<MsBuildToolset>();
            while (true)
            {
                var fetchedInstances = new ISetupInstance[3];
                int fetched;
                enumerator.Next(fetchedInstances.Length, fetchedInstances, out fetched);
                if (fetched == 0)
                {
                    break;
                }

                // fetched will return the value 3 even if only one instance returned
                var index = 0;
                while (index < fetched)
                {
                    if (fetchedInstances[index] != null)
                    {
                        setupInstances.Add(new MsBuildToolset(fetchedInstances[index]));
                    }

                    index++;
                }
            }

            if (setupInstances.Count == 0)
            {
                return null;
            }

            return setupInstances;
        }

        /// <summary>
        /// Escapes a string so that it can be safely passed as a command line argument when starting a msbuild process.
        /// Source: http://stackoverflow.com/a/12364234
        /// </summary>
        public static string Escape(string argument)
        {
            if (argument == string.Empty)
            {
                return "\"\"";
            }

            var escaped = Regex.Replace(argument, @"(\\*)""", @"$1\$0");

            escaped = Regex.Replace(
                escaped,
                @"^(.*\s.*?)(\\*)$", @"""$1$2$2""",
                RegexOptions.Singleline);

            return escaped;
        }

        public static string EscapeQuoted(string argument)
        {
            if (argument == string.Empty)
            {
                return "\"\"";
            }
            var escaped = Regex.Replace(argument, @"(\\*)" + "\"", @"$1$1\" + "\"");
            escaped = "\"" + Regex.Replace(escaped, @"(\\+)$", @"$1$1") + "\"";
            return escaped;

        }

        private static string GetMsbuild(string msbuildDirectory)
        {
            if (RuntimeEnvironmentHelper.IsMono)
            {
                var msbuildExe = GetMSBuild(EnvironmentVariableWrapper.Instance);

                if (msbuildExe != null)
                {
                    return msbuildExe;
                }

                // Find the first mono path that exists
                msbuildExe = GetMsBuildFromMonoPaths(userVersion: null)?.Path;

                if (msbuildExe != null)
                {
                    return msbuildExe;
                }
            }

            return CombinePathWithVerboseError(msbuildDirectory, "msbuild.exe");
        }

        internal static string GetMSBuild(IEnvironmentVariableReader reader)
        {
            var exeNames = new[] { "msbuild.exe" };

            if (RuntimeEnvironmentHelper.IsMono)
            {
                exeNames = new[] { "msbuild", "xbuild" };
            }

            // Try to find msbuild or xbuild in $Path.
            var pathDirs = reader.GetEnvironmentVariable("PATH")?.Split(new[] { Path.PathSeparator }, StringSplitOptions.RemoveEmptyEntries);

            if (pathDirs?.Length > 0)
            {
                foreach (var exeName in exeNames)
                {
                    var exePath = pathDirs.Select(dir => CombinePathWithVerboseError(dir.Trim('\"'), exeName)).FirstOrDefault(File.Exists);
                    if (exePath != null)
                    {
                        return exePath;
                    }
                }
            }

            return null;
        }

        internal static string CombinePathWithVerboseError(params string[] paths)
        {
            try
            {
                return Path.Combine(paths);
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                        LocalizedResourceManager.GetString("Error_InvalidCharactersInPathSegment"),
                        string.Join("', '", paths)),
                    e);
            }

        }

        /// <summary>
        /// This class is used to create a temp file, which is deleted in Dispose().
        /// </summary>
        private class TempFile : IDisposable
        {
            private readonly string _filePath;

            /// <summary>
            /// Constructor. It creates an empty temp file under the temp directory / NuGet, with
            /// extension <paramref name="extension"/>.
            /// </summary>
            /// <param name="extension">The extension of the temp file.</param>
            public TempFile(string extension)
            {
                if (string.IsNullOrEmpty(extension))
                {
                    throw new ArgumentNullException(nameof(extension));
                }
                var tempDirectory = NuGetEnvironment.GetFolderPath(NuGetFolderPath.Temp);

                Directory.CreateDirectory(tempDirectory);

                var count = 0;
                do
                {
                    _filePath = Path.Combine(tempDirectory, Path.GetRandomFileName() + extension);

                    if (!File.Exists(_filePath))
                    {
                        try
                        {
                            // create an empty file
                            using (var filestream = File.Open(_filePath, FileMode.CreateNew))
                            {
                            }

                            // file is created successfully.
                            return;
                        }
                        catch
                        {
                            // Ignore and try again
                        }
                    }

                    count++;
                }
                while (count < 3);

                throw new InvalidOperationException(
                    LocalizedResourceManager.GetString(nameof(NuGetResources.Error_FailedToCreateRandomFileForP2P)));
            }

            public static implicit operator string(TempFile f)
            {
                return f._filePath;
            }

            public void Dispose()
            {
                try
                {
                    FileUtility.Delete(_filePath);
                }
                catch
                {
                    // Ignore failures
                }
            }
        }
    }
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using FluentAssertions;
using NuGet.Common;
using NuGet.Test.Utility;
using Xunit;

namespace Msbuild.Integration.Test
{
    public class MsbuildIntegrationTestFixture : IDisposable
    {
        internal readonly string _testDir;
        private readonly Dictionary<string, string> _processEnvVars = new Dictionary<string, string>();
        private readonly Lazy<string> _msbuildPath = new Lazy<string>(() =>
            {
                string msbuildPath = FindMsbuildOnPath();
                if (msbuildPath == null)
                {
                    msbuildPath = FindMsbuildWithVsWhere();
                }
                if (msbuildPath == null)
                {
                    throw new Exception("Could not find msbuild.exe");
                }
                return msbuildPath;

                string FindMsbuildOnPath()
                {
                    string msbuild = RuntimeEnvironmentHelper.IsMono
                        ? "mono msbuild.exe"
                        : "msbuild.exe";

                    try
                    {
                        var result = CommandRunner.Run(
                            filename: msbuild,
                            workingDirectory: Environment.CurrentDirectory,
                            arguments: "-help");
                        if (result.Success)
                        {
                            return msbuild;
                        }
                    }
                    catch (Win32Exception)
                    {
                        // can't find program
                    }

                    return null;
                }

                string FindMsbuildWithVsWhere()
                {
                    string vswherePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Microsoft Visual Studio", "Installer", "vswhere.exe");

                    CommandRunnerResult result = CommandRunner.Run(filename: vswherePath,
                        workingDirectory: Environment.CurrentDirectory,
                        arguments: "-latest -prerelease -requires Microsoft.Component.MSBuild -find MSBuild\\**\\Bin\\MSBuild.exe");

                    if (!result.Success)
                    {
                        throw new Exception("vswhere did not return success");
                    }

                    string path = null;
                    using (var stringReader = new StringReader(result.Output))
                    {
                        string line;
                        while ((line = stringReader.ReadLine()) != null)
                        {
                            if (path != null)
                            {
                                throw new Exception("vswhere returned more than 1 line");
                            }
                            path = line;
                        }
                    }

                    return path;
                }
            });

        public MsbuildIntegrationTestFixture()
        {
            _testDir = Directory.GetCurrentDirectory();
            _processEnvVars.Add("UseSharedCompilation", "false");
            _processEnvVars.Add("DOTNET_MULTILEVEL_LOOKUP", "0");
            _processEnvVars.Add("MSBUILDDISABLENODEREUSE ", "true");
        }

        /// <summary>
        /// msbuild.exe args
        /// </summary>
        internal CommandRunnerResult RunMsBuild(string workingDirectory, string args, bool ignoreExitCode = false)
        {
            var restoreDllPath = Path.Combine(_testDir, "NuGet.Build.Tasks.dll");
            var nugetRestoreTargetsPath = Path.Combine(_testDir, "NuGet.targets");
            // Uncomment to debug the msbuild call
            // _processEnvVars.Add("DEBUG_RESTORE_TASK", "true");
            _processEnvVars["UNIT_TEST_RESTORE_TASK"] = bool.TrueString;
            var result = CommandRunner.Run(_msbuildPath.Value,
                workingDirectory,
                $"/p:NuGetRestoreTargets={nugetRestoreTargetsPath} /p:RestoreTaskAssemblyFile={restoreDllPath} /p:ImportNuGetBuildTasksPackTargetsFromSdk=true {args}",
                environmentVariables: _processEnvVars);

            if (!ignoreExitCode)
            {
                result.ExitCode.Should().Be(0, because: $"msbuild.exe {args} command failed with following log information :\n {result.AllOutput}");
            }

            return result;
        }

        public void Dispose()
        {
        }

        /// <summary>
        /// Depth-first recursive delete, with handling for descendant
        /// directories open in Windows Explorer or used by another process
        /// </summary>
        private static void DeleteDirectory(string path)
        {
            foreach (string directory in Directory.GetDirectories(path))
            {
                DeleteDirectory(directory);
            }

            try
            {
                Directory.Delete(path, true);
            }
            catch (IOException)
            {
                Directory.Delete(path, true);
            }
            catch (UnauthorizedAccessException)
            {
                Directory.Delete(path, true);
            }
            catch
            {

            }
        }
    }
}

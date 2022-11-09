// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace NuGet.Common.Migrations
{
    internal static class Migration1
    {
        public static void Run()
        {
            if (RuntimeEnvironmentHelper.IsWindows)
            {
                return;
            }

            // Since these paths have changed, we can't use NuGetEnvironment.GetFolderPath, since that will
            // return us the new path, not the old.
            string localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string nugetPath = Path.Combine(localAppDataPath, "NuGet");
            DeleteMigratedDirectories(nugetBaseDirectory: nugetPath);

            PosixPermissions umask = GetUmask();
            HashSet<string> pathsToCheck = GetPathsToCheck();
            EnsureExpectedPermissions(pathsToCheck: pathsToCheck, umask: umask);

            EnsureConfigFilePermissions();
        }

        internal static void DeleteMigratedDirectories(string nugetBaseDirectory)
        {
            var v3cachePath = Path.Combine(nugetBaseDirectory, "v3-cache");
            if (Directory.Exists(v3cachePath))
            {
                Directory.Delete(v3cachePath, recursive: true);
            }

            var pluginsCachePath = Path.Combine(nugetBaseDirectory, "plugins-cache");
            if (Directory.Exists(pluginsCachePath))
            {
                Directory.Delete(pluginsCachePath, recursive: true);
            }
        }

        internal static void EnsureExpectedPermissions(HashSet<string> pathsToCheck, PosixPermissions umask)
        {
            foreach (var path in pathsToCheck)
            {
                FixPermissions(path, umask);
            }
        }

        private static HashSet<string> GetPathsToCheck()
        {
            HashSet<string> pathsToCheck = new HashSet<string>();
            var homePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            // NuGetEnvironment.GetFolderPath(SpecialFolder.LocalApplicationData) is private, so we'll get the parent of the HTTP cache.
            var httpCachePath = NuGetEnvironment.GetFolderPath(NuGetFolderPath.HttpCacheDirectory);
            var nugetLocalAppDataPath = Path.GetDirectoryName(httpCachePath);
            AddAllParentDirectoriesUpToHome(nugetLocalAppDataPath);

            // We could be running in either mono or .NET (Core), and they use different paths for NuGetFolderPath.UserSettingsDirectory
            // So, we need to duplicate both of their path generation code to check both locations
            var monoConfigHome = GetMonoConfigPath();
            AddAllParentDirectoriesUpToHome(monoConfigHome);

            var dotnetConfigHome = GetDotnetConfigPath();
            AddAllParentDirectoriesUpToHome(dotnetConfigHome);

            return pathsToCheck;

            // Add all the parent directories starting from the path (passed as parameter) up to home directory.
            // If earlier versions of NuGet was the first app to create these directories, it probably created with too many permissions.
            void AddAllParentDirectoriesUpToHome(string path)
            {
                pathsToCheck.Add(path);

                if (!path.StartsWith(homePath + Path.DirectorySeparatorChar, StringComparison.Ordinal))
                {
                    return;
                }

                string parent = Path.GetDirectoryName(path);
                while (parent != homePath)
                {
                    pathsToCheck.Add(parent);
                    parent = Path.GetDirectoryName(parent);
                }
            }
        }

        private static string GetMonoConfigPath()
        {
            string xdgConfigHome = Environment.GetEnvironmentVariable("XDG_CONFIG_HOME");
            if (string.IsNullOrEmpty(xdgConfigHome))
            {
                var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                return Path.Combine(home, ".config", "NuGet");
            }
            else
            {
                return Path.Combine(xdgConfigHome, "NuGet");
            }
        }

        private static string GetDotnetConfigPath()
        {
            var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            return Path.Combine(home, ".nuget", "NuGet");
        }

        private static void EnsureConfigFilePermissions()
        {
            // We want the file to be user readable only
            PosixPermissions umask = PosixPermissions.Parse("077");

            // We could be running in either mono or .NET (Core), and they use different paths for NuGetFolderPath.UserSettingsDirectory
            // So, we need to duplicate both of their path generation code to check both locations
            EnsureConfigFilePermissions(GetMonoConfigPath(), umask);
            EnsureConfigFilePermissions(GetDotnetConfigPath(), umask);
        }

        internal static void EnsureConfigFilePermissions(string directory, PosixPermissions umask)
        {
            if (Directory.Exists(directory))
            {
                // nuget.config can have at least 3 different casing, which is a problem on case-sensitive filesystems.
                // But the config might also be copied for backups, so let's ensure all the files are 
                foreach (string file in Directory.EnumerateFiles(directory, "*", SearchOption.AllDirectories))
                {
                    FixPermissions(file, umask);
                }
            }
        }

        private static PosixPermissions GetUmask()
        {
            var output = Exec("sh", "-c umask");
            PosixPermissions umask = PosixPermissions.Parse(output);
            return umask;
        }

        private static void FixPermissions(string path, PosixPermissions umask)
        {
            PosixPermissions? permissions = GetPermissions(path);
            if (permissions == null)
            {
                return;
            }

            if (!permissions.Value.SatisfiesUmask(umask))
            {
                PosixPermissions correctedPermissions = permissions.Value.WithUmask(umask);
                string correctedPermissionsString = correctedPermissions.ToString();
                Exec("chmod", correctedPermissionsString + " " + path.FormatWithDoubleQuotes());
            }
        }

        internal static PosixPermissions? GetPermissions(string path)
        {
            string output = Exec("ls", "-ld " + path.FormatWithDoubleQuotes());
            if (output == null)
            {
                return null;
            }

            int indexOfSpace = output.IndexOf(" ", StringComparison.Ordinal);
            if (indexOfSpace < 10)
            {
                return null;
            }

            int permissions = 0;
            for (int i = 1; i < 10; i++)
            {
                permissions = (permissions << 1) + (output[i] != '-' ? 1 : 0);
            }

            return new PosixPermissions(permissions);
        }

        internal static string Exec(string command, string args)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(command)
            {
                Arguments = args,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false
            };

            using (var proc = new Process())
            {
                proc.StartInfo = startInfo;
                proc.Start();

                proc.WaitForExit(10000);
                if (proc.ExitCode != 0)
                {
                    // File does not exist
                    return null;
                }

                string output = proc.StandardOutput.ReadLine();
                return output;
            }
        }
    }
}

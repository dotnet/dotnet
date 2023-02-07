// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace NuGet.Build.Tasks.Console
{
    /// <summary>
    /// Represents the main entry point to the console application.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The main entry point to the console application.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        /// <returns><code>0</code> if the application ran successfully with no errors, otherwise <code>1</code>.</returns>
        public static async Task<int> Main(string[] args)
        {
            var debug = IsDebug();

            if (debug)
            {
                Debugger.Launch();
            }

            NuGet.Common.Migrations.MigrationRunner.Run();
            if (args.Length != 2)
            {
                return 1;
            }

            var msbuildFilePath = new FileInfo(args[0]);
            var entryProjectPath = new FileInfo(args[1]);

            // Enable MSBuild feature flags
            MSBuildFeatureFlags.MSBuildExeFilePath = msbuildFilePath.FullName;
            MSBuildFeatureFlags.EnableCacheFileEnumerations = true;
            MSBuildFeatureFlags.LoadAllFilesAsReadonly = true;
            MSBuildFeatureFlags.SkipEagerWildcardEvaluations = true;

#if NETFRAMEWORK
            if (AppDomain.CurrentDomain.IsDefaultAppDomain())
            {
                // MSBuild.exe.config has binding redirects that change from time to time and its very hard to make sure that NuGet.Build.Tasks.Console.exe.config is correct.
                // It also can be different per instance of Visual Studio so when running unit tests it always needs to match that instance of MSBuild
                // The code below runs this EXE in an AppDomain as if its MSBuild.exe so the assembly search location is next to MSBuild.exe and all binding redirects are used
                // allowing this process to evaluate MSBuild projects as if it is MSBuild.exe
                Assembly thisAssembly = Assembly.GetExecutingAssembly();

                AppDomain appDomain = AppDomain.CreateDomain(
                    thisAssembly.FullName,
                    securityInfo: null,
                    info: new AppDomainSetup
                    {
                        ApplicationBase = msbuildFilePath.DirectoryName,
                        ConfigurationFile = Path.Combine(msbuildFilePath.DirectoryName, "MSBuild.exe.config")
                    });

                return appDomain
                    .ExecuteAssembly(
                        thisAssembly.Location,
                        args);
            }
#endif

            // Parse command-line arguments
            if (!TryGetArguments(out StaticGraphRestoreArguments arguments))
            {
                return 1;
            }

            // Check whether the ask is to generate the restore graph file.
            if (MSBuildStaticGraphRestore.IsOptionTrue("GenerateRestoreGraphFile", arguments.Options))
            {
                using (var dependencyGraphSpecGenerator = new MSBuildStaticGraphRestore(debug: debug))
                {
                    return dependencyGraphSpecGenerator.WriteDependencyGraphSpec(entryProjectPath.FullName, arguments.GlobalProperties, arguments.Options) ? 0 : 1;
                }
            }

            // Otherwise run restore!
            using (var dependencyGraphSpecGenerator = new MSBuildStaticGraphRestore(debug: debug))
            {
                return await dependencyGraphSpecGenerator.RestoreAsync(entryProjectPath.FullName, arguments.GlobalProperties, arguments.Options) ? 0 : 1;
            }
        }

        /// <summary>
        /// Determines if a user specified that the current process is being debugged.
        /// </summary>
        /// <returns><code>true</code> if the user specified to debug the current process, otherwise <code>false</code>.</returns>
        private static bool IsDebug()
        {
            return string.Equals(Environment.GetEnvironmentVariable("DEBUG_RESTORE_TASK"), bool.TrueString, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Parses command-line arguments.
        /// </summary>
        /// <param name="staticGraphRestoreArguments">Receives the arguments as a <see cref="StaticGraphRestoreArguments" />.</param>
        /// <returns><code>true</code> if the arguments were successfully parsed, otherwise <code>false</code>.</returns>
        private static bool TryGetArguments(out StaticGraphRestoreArguments staticGraphRestoreArguments)
        {
            staticGraphRestoreArguments = null;

            try
            {
                using Stream stream = System.Console.OpenStandardInput();

                staticGraphRestoreArguments = StaticGraphRestoreArguments.Read(stream, System.Console.InputEncoding);

                return staticGraphRestoreArguments != null;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

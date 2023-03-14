// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
        /// A <see cref="T:char[]" /> containing the equals sign '=' to be used to split key/value pairs that are separated by it.
        /// </summary>
        private static readonly char[] EqualSign = { '=' };

        /// <summary>
        /// A <see cref="T:char[]" /> containing the semicolon ';' to be used to split key/value pairs that are separated by it.
        /// </summary>
        private static readonly char[] Semicolon = { ';' };

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

            // Parse command-line arguments
            if (!TryParseArguments(args, out (Dictionary<string, string> Options, FileInfo MSBuildExeFilePath, string EntryProjectFilePath, Dictionary<string, string> MSBuildGlobalProperties) arguments))
            {
                return 1;
            }

            // Enable MSBuild feature flags
            MSBuildFeatureFlags.MSBuildExeFilePath = arguments.MSBuildExeFilePath.FullName;
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
                var thisAssembly = Assembly.GetExecutingAssembly();

                AppDomain appDomain = AppDomain.CreateDomain(
                    thisAssembly.FullName,
                    securityInfo: null,
                    info: new AppDomainSetup
                    {
                        ApplicationBase = arguments.MSBuildExeFilePath.DirectoryName,
                        ConfigurationFile = Path.Combine(arguments.MSBuildExeFilePath.DirectoryName, "MSBuild.exe.config")
                    });

                return appDomain
                    .ExecuteAssembly(
                        thisAssembly.Location,
                        args);
            }
#endif

            // Check whether the ask is to generate the restore graph file.
            if (MSBuildStaticGraphRestore.IsOptionTrue("GenerateRestoreGraphFile", arguments.Options))
            {
                using (var dependencyGraphSpecGenerator = new MSBuildStaticGraphRestore(debug: debug))
                {
                    return dependencyGraphSpecGenerator.WriteDependencyGraphSpec(arguments.EntryProjectFilePath, arguments.MSBuildGlobalProperties, arguments.Options) ? 0 : 1;
                }
            }

            // Otherwise run restore!
            using (var dependencyGraphSpecGenerator = new MSBuildStaticGraphRestore(debug: debug))
            {
                return await dependencyGraphSpecGenerator.RestoreAsync(arguments.EntryProjectFilePath, arguments.MSBuildGlobalProperties, arguments.Options) ? 0 : 1;
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
        /// Parses a semicolon delimited list of equal sign separated key value pairs.
        /// </summary>
        /// <param name="value">The string containing a semicolon delimited list of key value pairs to parse.</param>
        /// <returns>A <see cref="Dictionary{String,String}" /> containing the list of items as key value pairs.</returns>
        private static Dictionary<string, string> ParseSemicolonDelimitedListOfKeyValuePairs(string value)
        {
            var properties = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (var pair in value
                .Split(Semicolon, StringSplitOptions.RemoveEmptyEntries)
                .Where(i => !string.IsNullOrWhiteSpace(i))
                .Select(i => i.Split(EqualSign, 2))
                .Where(i => i.Length == 2 && !string.IsNullOrWhiteSpace(i[0]) && !string.IsNullOrWhiteSpace(i[1])))
            {
                properties[pair[0].Trim()] = pair[1].Trim();
            }

            return properties;
        }

        /// <summary>
        /// Parses command-line arguments.
        /// </summary>
        /// <param name="args">A <see cref="T:string[]" /> containing the process command-line arguments.</param>
        /// <param name="arguments">A <see cref="T:Tuple&lt;Dictionary&lt;string, string&gt;, FileInfo, string, Dictionary&lt;string, string&gt;&gt;" /> that receives the parsed command-line arguments.</param>
        /// <returns><code>true</code> if the arguments were successfully parsed, otherwise <code>false</code>.</returns>
        private static bool TryParseArguments(string[] args, out (Dictionary<string, string> Options, FileInfo MSBuildExeFilePath, string EntryProjectFilePath, Dictionary<string, string> MSBuildGlobalProperties) arguments)
        {
            if (args.Length != 4)
            {
                arguments = (null, null, null, null);

                return false;
            }

            try
            {
                var options = ParseSemicolonDelimitedListOfKeyValuePairs(args[0]);
                var msbuildExeFilePath = new FileInfo(args[1]);
                var entryProjectFilePath = args[2];
                var globalProperties = ParseSemicolonDelimitedListOfKeyValuePairs(args[3]);

                arguments = (options, msbuildExeFilePath, entryProjectFilePath, globalProperties);

                // Command-line is correct if no exceptions were thrown and the MSBuild path exists and an entry project were specified
                return msbuildExeFilePath.Exists && !string.IsNullOrWhiteSpace(entryProjectFilePath);
            }
            catch (Exception)
            {
                arguments = (null, null, null, null);

                return false;
            }
        }
    }
}

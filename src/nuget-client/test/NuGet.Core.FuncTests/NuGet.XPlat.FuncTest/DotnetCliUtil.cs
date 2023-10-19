// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using NuGet.Frameworks;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.XPlat.FuncTest
{
    public class DotnetCliUtil
    {
        private const string XPlatDll = "NuGet.CommandLine.XPlat.dll";
        private static readonly string[] TestFileNames = new string[] { "file1.txt", "file2.txt" };

        /// <summary>
        /// Adds a few dummy text files at the specified path for testing nuget locals --clear
        /// </summary>
        /// <param name="path">Path which needs to be populated with dummy files</param>
        public static void CreateTestFiles(string path)
        {
            foreach (var fileName in TestFileNames)
            {
                File.Create(Path.Combine(path, fileName)).Dispose();
            }
        }

        /// <summary>
        /// Verifies the dummy text files at the specified path for testing nuget locals --clear
        /// </summary>
        /// <param name="path">Path which needs to be tested for the dummy files</param>
        public static void VerifyClearSuccess(string path)
        {
            Assert.False(Directory.Exists(path));
        }

        /// <summary>
        /// Verifies the dummy text files at the specified path for testing nuget locals --clear
        /// </summary>
        /// <param name="path">Path which needs to be tested for the dummy files</param>
        public static void VerifyNoClear(string path)
        {
            Assert.True(Directory.Exists(path));
            var files = Directory.GetFiles(path)
                                 .Select(filepath => Path.GetFileName(filepath))
                                 .ToArray();
            foreach (var filename in TestFileNames)
            {
                Assert.True(Array.Exists(files, element => element == filename));
            }

            Assert.Equal(files.Count(), TestFileNames.Length);
        }

        /// <summary>
        /// Provides the path to Xplat dll on the test machine.
        /// It traverses in the directory tree going one step up at a time and looks for src folder.
        /// Once in src, it looks for the xplat dll in the location specified by <code>_xplatDll</code>.
        /// </summary>
        /// <returns>
        /// <code>String</code> containing the path to the dotnet cli within the local repository.
        /// Can return <code>null</code> if no src directory or xplat dll is found, in which case the tests can fail.
        /// </returns>
        public static string GetXplatDll()
        {
            var dir = TestFileSystemUtility.ParentDirectoryLookup()
               .FirstOrDefault(d => Directory.Exists(Path.Combine(d.FullName, "src")));

            if (dir != null)
            {
                const string configuration =
#if DEBUG
                "Debug";
#else
                "Release";
#endif
                var configurationDirectory = Path.Combine(dir.FullName, "artifacts", "NuGet.CommandLine.XPlat", "bin", configuration);
                var referenceTfm = new NuGetFramework(FrameworkConstants.FrameworkIdentifiers.NetCoreApp, new Version(int.MaxValue, 0, 0));
                var bestTfm = GetTfmToCopy(configurationDirectory, referenceTfm);
                var filePath = Path.Combine(configurationDirectory, bestTfm, XPlatDll);

                if (File.Exists(filePath))
                {
                    return filePath;
                }
            }

            return null;
        }

        private static string GetTfmToCopy(string projectArtifactsBinFolder, NuGetFramework referenceTfm)
        {
            var compiledTfms =
                Directory.EnumerateDirectories(projectArtifactsBinFolder) // get all directories in bin folder
                .Select(Path.GetFileName) // just the folder name (tfm)
                .ToDictionary(folder => NuGetFramework.Parse(folder));

            var reducer = new FrameworkReducer();
            var selectedTfm = reducer.GetNearest(referenceTfm, compiledTfms.Keys);

            if (selectedTfm == null)
            {
                var message = $@"Could not find suitable assets to copy in {projectArtifactsBinFolder}
TFM being tested: {referenceTfm}
project TFMs found: {string.Join(", ", compiledTfms.Keys.Select(k => k.ToString()))}";

                throw new Exception(message);
            }

            var selectedVersion = compiledTfms[selectedTfm];

            return selectedVersion;
        }



        /// <summary>
        /// Used to verify the success of positive test cases
        /// </summary>
        /// <param name="result">The actual result of the test</param>
        /// <param name="expectedOutputMessage"> The expected result of the test</param>
        public static void VerifyResultSuccess(CommandRunnerResult result, string expectedOutputMessage = null)
        {
            Assert.True(
                result.ExitCode == 0,
                $"Command DID NOT SUCCEED. Ouput is: \"{result.Output}\". Error is: \"{result.Errors}\"");

            if (!string.IsNullOrEmpty(expectedOutputMessage))
            {
                Assert.Contains(
                    expectedOutputMessage,
                    result.Output);
            }
        }

        /// <summary>
        /// Used to verify the failure of negitive test cases
        /// </summary>
        /// <param name="result">The actual result of the test</param>
        /// <param name="expectedOutputMessage"> The expected result of the test</param>
        public static void VerifyResultFailure(CommandRunnerResult result,
                                               string expectedErrorMessage)
        {
            Assert.True(
                result.ExitCode != 0,
                $"Command DID NOT FAIL. Ouput is: \"{result.Output}\". Error is: \"{result.Errors}\"");

            Assert.True(
                result.Output.Contains(expectedErrorMessage),
                $"Expected error is: \"{expectedErrorMessage}\". Actual error is: \"{result.Errors}\". Ouput is: \"{result.Output}\".");
        }
    }
}

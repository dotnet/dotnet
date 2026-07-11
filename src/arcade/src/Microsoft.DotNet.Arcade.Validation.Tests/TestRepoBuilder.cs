// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Utilities.ProjectCreation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Validation.Tests
{
    public class TestRepoUtils
    {
        /// <summary>
        /// Calculates a build argument to build.sh/build.ps1.
        /// Arguments (non msbuild ones) have -- in sh, and - in ps1.
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static string BuildArg(string arg)
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ?
                $"-{arg}" : $"--{arg}";
        }

        public static string CreateUniqueTempDir(string dirName)
        {
            string repoRoot;
            do
            {
                var guidSuffix = Guid.NewGuid().ToString().Substring(0, 8);
                var repoDir = $"{dirName}{guidSuffix}";
                repoRoot = Path.Combine(Path.GetTempPath(), repoDir);
            }
            while (Directory.Exists(repoRoot));

            Directory.CreateDirectory(repoRoot);
            return repoRoot;
        }

        public static string NormalizePath(string path)
        {
            return path.Replace('/', Path.DirectorySeparatorChar)
                       .Replace('\\', Path.DirectorySeparatorChar);
        }

        public static void KillSpecificExecutable(string fileName)
        {
            string justProcessName = Path.GetFileNameWithoutExtension(Path.GetFileName(fileName));
            System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcessesByName(justProcessName);

            // File paths are case-insensitive on Windows/macOS but case-sensitive on Linux.
            var pathComparison = RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                ? StringComparison.Ordinal
                : StringComparison.OrdinalIgnoreCase;

            foreach (var process in processes)
            {
                using (process)
                {
                    try
                    {
                        if (string.Equals(process.MainModule.FileName, fileName, pathComparison))
                        {
                            process.Kill(true);
                        }
                    }
                    catch { } // Ignored
                }
            }
        }

        public static string DotNetHostExecutableName
        {
            get => RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "dotnet.exe" : "dotnet";
        }

        /// <summary>
        /// Best-effort recursive delete of a temporary directory. Synthetic repo builds spawn
        /// persistent build server nodes (MSBuild worker nodes, VBCSCompiler) that can keep assemblies
        /// loaded briefly after the build completes, so a straight <see cref="Directory.Delete(string, bool)"/>
        /// can intermittently fail with <see cref="IOException"/>/<see cref="UnauthorizedAccessException"/>.
        /// Since the directory is throwaway, retry a few times and then give up rather than failing the run.
        /// </summary>
        public static void TryDeleteDirectory(string path)
        {
            if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
            {
                return;
            }

            for (int attempt = 0; ; attempt++)
            {
                try
                {
                    Directory.Delete(path, true);
                    return;
                }
                catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
                {
                    if (attempt >= 4)
                    {
                        // Best effort: leave the temp directory behind rather than failing teardown.
                        return;
                    }

                    Thread.Sleep(500);
                }
            }
        }
    }

    public class RepoResources : IDisposable
    {
        public string DotNetVersion { get; private set; }
        public string ArcadeVersion { get; private set; }
        /// <summary>
        /// ';'-separated list of local package feed directories to inject (with priority) into
        /// each synthetic repo's NuGet.config. Used to make the *newly built* Arcade/Helix SDK
        /// packages available to the test repos. Null/empty when validating against public feeds.
        /// </summary>
        public string LocalPackageFeeds { get; private set; }
        /// <summary>
        /// Common dotnet root, if desired. If not, then null
        /// </summary>
        public string CommonDotnetRoot { get; private set; } = null;
        /// <summary>
        /// Common package restore location, if desired. If not, then null
        /// </summary>
        public string CommonPackagesRoot { get; private set; } = null;

        private string CommonRoot = null;

        private RepoResources() { }

        // Runtime configuration options (set via this project's runtimeconfig.json — see the .csproj)
        // point the validation tests at the newly produced Arcade SDK version and packages instead of
        // the bootstrap versions pinned in global.json. When unset, the tests fall back to the
        // global.json versions and the default public feeds.
        internal const string ArcadeVersionConfigKey = "Microsoft.DotNet.Arcade.Validation.Tests.ArcadeSdkVersion";
        internal const string LocalFeedsConfigKey = "Microsoft.DotNet.Arcade.Validation.Tests.LocalPackageFeeds";

        private static string GetRuntimeConfigValue(string key) => AppContext.GetData(key) as string;

        /// <summary>
        /// Create a set of repo resources.
        /// </summary>
        /// <param name="useIsolatedRoots">
        ///     Should isolated .dotnet and .packages locations
        ///     be used? If false, then to share a dotnet root, a simple repo is created
        ///     and restored. Then the resulting .dotnet and .packages directories are
        ///     </param>
        /// <returns></returns>
        public static async Task<RepoResources> Create(bool useIsolatedRoots)
        {
            string dotnetVersion;
            string arcadeVersion;
            string dotnetRoot = null;
            string packagesRoot = null;
            string commonRoot = null;

            var globalJsonLocation = TestRepoBuilder.GetTestInputPath("global.json");
            using (var reader = new StreamReader(globalJsonLocation))
            using (JsonDocument doc = JsonDocument.Parse(reader.BaseStream))
            {
                dotnetVersion = doc.RootElement.GetProperty("tools").GetProperty("dotnet").GetString();
                arcadeVersion = doc.RootElement.GetProperty("msbuild-sdks").GetProperty("Microsoft.DotNet.Arcade.Sdk").GetString();
            }

            // Override the versions/feeds so the synthetic repos validate the newly produced Arcade SDK
            // (the version in global.json is the bootstrap SDK, not the one this build produces).
            var arcadeVersionOverride = GetRuntimeConfigValue(ArcadeVersionConfigKey);
            if (!string.IsNullOrEmpty(arcadeVersionOverride))
            {
                arcadeVersion = arcadeVersionOverride;
            }

            var localPackageFeeds = GetRuntimeConfigValue(LocalFeedsConfigKey);

            // If not using isolated roots, create a quick test repo builder
            // to restore things.
            if (!useIsolatedRoots)
            {
                // Common repo resources for constructing a simple repo. The
                // repo's test dir is set to not be deleted, and the .dotnet and .packages paths are extracted.
                // During the disposal of the returned set of resources, the common dir is deleted.
                var commonRepoResources = new RepoResources()
                {
                    ArcadeVersion = arcadeVersion,
                    DotNetVersion = dotnetVersion,
                    LocalPackageFeeds = localPackageFeeds
                };
                using (var builder = new TestRepoBuilder("common", commonRepoResources, deleteOnDispose: false))
                {
                    await builder.AddDefaultRepoSetupAsync();

                    // Create a simple project
                    builder.AddProject(ProjectCreator
                            .Templates
                            .SdkCsproj(
                                targetFramework: "net8.0",
                                outputType: "Exe"),
                        "./src/FooPackage/FooPackage.csproj");
                    await builder.AddSimpleCSFile("./src/FooPackage/Program.cs");

                    await builder.Build(
                        TestRepoUtils.BuildArg("restore"),
                        TestRepoUtils.BuildArg("ci"),
                        TestRepoUtils.BuildArg("projects"),
                        Path.Combine(builder.TestRepoRoot, "src/FooPackage/FooPackage.csproj"))();

                    commonRoot = builder.TestRepoRoot;
                    dotnetRoot = Path.Combine(builder.TestRepoRoot, ".dotnet");
                    if (!Directory.Exists(dotnetRoot))
                    {
                        // Coming from the machine
                        dotnetRoot = null;
                    }
                    packagesRoot = Path.Combine(builder.TestRepoRoot, ".packages");
                    if (!Directory.Exists(packagesRoot))
                    {
                        packagesRoot = null;
                    }
                }
            }

            RepoResources repoResources = new RepoResources()
            {
                ArcadeVersion = arcadeVersion,
                DotNetVersion = dotnetVersion,
                LocalPackageFeeds = localPackageFeeds,
                CommonPackagesRoot = packagesRoot,
                CommonDotnetRoot = dotnetRoot,
                CommonRoot = commonRoot
            };

            return repoResources;
        }

        public void Dispose()
        {
            if (!string.IsNullOrEmpty(CommonRoot) && Directory.Exists(CommonRoot))
            {
                if (!string.IsNullOrEmpty(CommonDotnetRoot))
                {
                    TestRepoUtils.KillSpecificExecutable(Path.Combine(CommonDotnetRoot, TestRepoUtils.DotNetHostExecutableName));
                }
                // Delete the common root
                TestRepoUtils.TryDeleteDirectory(CommonRoot);
            }
        }
    }

    public class TestRepoBuilder : IDisposable
    {
        public readonly bool DeleteOnDispose;
        public readonly string TestRepoRoot;
        public readonly string TestName;
        public readonly RepoResources RepoResources;

        public TestRepoBuilder(string testName, RepoResources repoResources, bool deleteOnDispose = true)
        {
            RepoResources = repoResources;
            TestName = testName;
            DeleteOnDispose = deleteOnDispose;

            // Truncate the test name to 8 chars and compute the test repo
            const int testNameMaxLength = 8;
            string truncatedTestName = testName.Length > testNameMaxLength ? testName.Remove(testNameMaxLength) : testName;
            TestRepoRoot = TestRepoUtils.CreateUniqueTempDir(truncatedTestName);
        }

        public static string GetTestInputPath(string relativeTestInputPath)
        {
            return Path.Combine(
                Path.GetDirectoryName(typeof(TestRepoBuilder).Assembly.Location),
                "inputs",
                TestRepoUtils.NormalizePath(relativeTestInputPath));
        }

        /// <summary>
        ///     Adds basic files required for running a test case
        ///         - global.json at root, with msbuild-sdks node with arcade and helix versions and tools node with dotnet node.
        ///         - eng/common folder
        /// </summary>
        public async Task AddDefaultRepoSetupAsync()
        {
            await AddDefaultGlobalJsonAsync();
            await AddDefaultVersionsPropsAsync();
            await AddDefaultNuGetConfigAsync();
            await AddDefaultDirectoryBuildPropsAsync();
            await AddDefaultDirectoryBuildTargetsAsync();
            await AddDefaultLicenseFileAsync();

            CloneSubdirectoryFromTestResources("eng/common");
        }

        private async Task AddDefaultGlobalJsonAsync()
        {
            var options = new JsonWriterOptions { Indented = true };
            using (var stream = new MemoryStream())
            {
                using (var writer = new Utf8JsonWriter(stream, options))
                {
                    writer.WriteStartObject();
                    writer.WriteStartObject("tools");
                    writer.WriteString("dotnet", RepoResources.DotNetVersion);
                    writer.WriteEndObject();
                    writer.WriteStartObject("msbuild-sdks");
                    writer.WriteString("Microsoft.DotNet.Arcade.Sdk", RepoResources.ArcadeVersion);
                    writer.WriteString("Microsoft.DotNet.Helix.Sdk", RepoResources.ArcadeVersion);
                    writer.WriteEndObject();
                    writer.WriteEndObject();
                }

                await AddFileWithContents("global.json", Encoding.UTF8.GetString(stream.ToArray()));
            }
        }

        public void AddProject(ProjectCreator creator, string path)
        {
            string normalizedPath = TestRepoUtils.NormalizePath(path);
            string targetFileName = Path.Combine(TestRepoRoot, normalizedPath);
            creator.Save(targetFileName);
        }

        private async Task AddDefaultLicenseFileAsync()
        {
            const string licenseFile = @"The MIT License (MIT)

Copyright (c) .NET Foundation and Contributors

All rights reserved.

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the ""Software""), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/ or sell
 copies of the Software, and to permit persons to whom the Software is
 furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.";

            await AddFileWithContents("LICENSE.TXT", licenseFile);
        }

        private async Task AddDefaultNuGetConfigAsync()
        {
            // Inject any local package feeds (e.g. the freshly built Arcade/Helix SDK packages) with
            // priority so the synthetic repo restores the newly produced SDK rather than a published one.
            var localFeedEntries = new StringBuilder();
            if (!string.IsNullOrEmpty(RepoResources.LocalPackageFeeds))
            {
                int index = 0;
                foreach (var feed in RepoResources.LocalPackageFeeds.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var trimmedFeed = feed.Trim();
                    if (trimmedFeed.Length == 0)
                    {
                        continue;
                    }

                    // Skip feed directories that don't exist. NuGet fails restore with NU1301 when a
                    // configured local source path is missing, and not every configured feed (e.g. the
                    // Shipping vs NonShipping packages dir) is necessarily materialized for a given build.
                    if (!Directory.Exists(trimmedFeed))
                    {
                        continue;
                    }

                    // Escape the path so paths containing XML-special characters (e.g. '&') produce valid config.
                    localFeedEntries.AppendLine($"    <add key=\"arcade-local-{index}\" value=\"{SecurityElement.Escape(trimmedFeed)}\" />");
                    index++;
                }
            }

            string nugetConfig = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <packageSources>
    <clear />
{localFeedEntries}    <add key=""dotnet-eng"" value=""https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-eng/nuget/v3/index.json"" />
    <add key=""dotnet-tools"" value=""https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-tools/nuget/v3/index.json"" />
    <add key=""dotnet-public"" value=""https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-public/nuget/v3/index.json"" />
  </packageSources>
</configuration>
";
            await AddFileWithContents("NuGet.config", nugetConfig);
        }

        private async Task AddDefaultDirectoryBuildPropsAsync()
        {
            const string dirBuildProps = @"<Project>
  <Import Project=""Sdk.props"" Sdk=""Microsoft.DotNet.Arcade.Sdk"" />

  <PropertyGroup>
    <PackageProjectUrl>https://localhost/</PackageProjectUrl>
    <PackageLicenseExpression>MIT</PackageLicenseExpression>
    <RepositoryUrl>https://localhost/</RepositoryUrl>
    <RepositoryCommit>aaaabbbbccccddddeeeeffffeeeeddddccccbbcc</RepositoryCommit>
    <EnableSourceControlManagerQueries>false</EnableSourceControlManagerQueries>
  </PropertyGroup>
</Project>";

            await AddFileWithContents("Directory.Build.props", dirBuildProps);
        }

        private async Task AddDefaultDirectoryBuildTargetsAsync()
        {
            const string dirBuildTargets = @"<Project>
  <Import Project=""Sdk.targets"" Sdk=""Microsoft.DotNet.Arcade.Sdk"" />
</Project> ";

            await AddFileWithContents("Directory.Build.targets", dirBuildTargets);
        }

        private async Task AddDefaultVersionsPropsAsync()
        {
            const string defaultVersionsProps = @"<Project>
    <PropertyGroup>
      <VersionPrefix>1.0.0</VersionPrefix>
      <PreReleaseVersionLabel>prerelease</PreReleaseVersionLabel>
    </PropertyGroup>
  </Project>";
            await AddFileWithContents("eng/Versions.props", defaultVersionsProps);
        }

        private async Task AddFileWithContents(string path, string contents)
        {
            string normalizedPath = TestRepoUtils.NormalizePath(path);
            string targetFileName = Path.Combine(TestRepoRoot, normalizedPath);

            Directory.CreateDirectory(Path.GetDirectoryName(targetFileName));

            using (StreamWriter writer = new StreamWriter(targetFileName))
            {
                await writer.WriteAsync(contents);
            }
        }
        private void CloneFileFromTestResources(string file)
        {
            string normalizedPath = TestRepoUtils.NormalizePath(file);
            string inputsFileName = GetTestInputPath(normalizedPath);
            string targetFileName = Path.Combine(TestRepoRoot, normalizedPath);

            if (!File.Exists(inputsFileName))
            {
                throw new ArgumentException($"{file} doesn't exist at {inputsFileName}");
            }

            Directory.CreateDirectory(Path.GetDirectoryName(targetFileName));
            File.Copy(inputsFileName, targetFileName);
        }

        /// <summary>
        /// Adds a default hello world
        /// </summary>
        /// <param name="path"></param>
        public async Task AddSimpleCSFile(string path)
        {
            const string helloWorld = @"// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(""Hello World!"");
        }
    }
}";
            await AddFileWithContents(path, helloWorld);
        }

        private void CloneSubdirectoryFromTestResources(string path)
        {
            string normalizedPath = TestRepoUtils.NormalizePath(path);
            string resourcesPath = GetTestInputPath(normalizedPath);
            string testPath = Path.Combine(TestRepoRoot, normalizedPath);

            if (!Directory.Exists(resourcesPath))
            {
                throw new ArgumentException($"{resourcesPath} is not a file or a directory that exists in the test resources");
            }

            var allFiles = Directory.GetFiles(resourcesPath, "*.*", SearchOption.AllDirectories);
            foreach (var file in allFiles)
            {
                string targetFileName = file.Replace(resourcesPath, testPath);
                Directory.CreateDirectory(Path.GetDirectoryName(targetFileName));
                File.Copy(file, targetFileName);
            }
        }

        public Func<Task> Build(params string[] args)
        {
            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            var allArgs = new List<string>();
            string executable;

            if (isWindows)
            {
                executable = "powershell.exe";
                allArgs.AddRange(new[] { "-ExecutionPolicy", "ByPass", "-NoProfile", "-File", "./eng/common/build.ps1" });
            }
            else
            {
                executable = "bash";
                allArgs.Add("./eng/common/build.sh");
            }

            allArgs.AddRange(args);

            // Workaround: a NuGet change (https://github.com/NuGet/NuGet.Client/pull/7020) flowed in via Arcade
            // incorrectly holds file locks when deterministic timestamps are enabled, causing pack/sign tests to
            // fail on Windows during cleanup with "The process cannot access the file ... because it is being used
            // by another process." Disable deterministic timestamps until the underlying issue is resolved.
            // This only manifests on Windows, so scope the workaround there to avoid changing behavior elsewhere.
            // Tracking issue: https://github.com/dotnet/arcade/issues/17065
            if (isWindows)
            {
                allArgs.Add("/p:DeterministicTimestamp=false");
            }

            // Invokes eng/common/build.ps1 with provided options
            return async () =>
            {
                CommandResult result = await Command.Create(executable, allArgs)
                    .EnvironmentVariable("DOTNET_INSTALL_DIR", RepoResources.CommonDotnetRoot)
                    .EnvironmentVariable("NUGET_PACKAGES", RepoResources.CommonPackagesRoot)
                    .EnvironmentVariable("BUILD_REPOSITORY_URI", "https://localhost")
                    .EnvironmentVariable("BUILD_SOURCEBRANCH", "whatsabranch")
                    .EnvironmentVariable("BUILD_BUILDNUMBER", "20200101.1")
                    .EnvironmentVariable("BUILD_SOURCEVERSION", "aaaabbbbccccddddeeeeffffeeeeddddccccbbcc")
                    .EnvironmentVariable("DOTNET_CLI_TELEMETRY_OPTOUT", "1")
                    .WorkingDirectory(TestRepoRoot)
                    .CaptureStdErr()
                    .CaptureStdOut()
                    .ExecuteAsync()
                    .ConfigureAwait(false);

                result.EnsureSuccessful();
            };
        }

        public void Cleanup()
        {
            // Workaround to the fact that the .dotnet cannot be easily
            // placed outside the repo at the current time.
            string potentialDotNetExe = Path.Combine(TestRepoRoot, ".dotnet", TestRepoUtils.DotNetHostExecutableName);
            if (File.Exists(potentialDotNetExe))
            {
                TestRepoUtils.KillSpecificExecutable(potentialDotNetExe);
            }

            if (DeleteOnDispose)
            {
                TestRepoUtils.TryDeleteDirectory(TestRepoRoot);
            }
        }

        public void Dispose()
        {
            Cleanup();
        }
    }
}

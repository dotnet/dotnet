// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml.Linq;

using Microsoft.VisualStudio.TestPlatform.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;

namespace Microsoft.TestPlatform.TestUtilities;

public class IntegrationTestBuild : IntegrationTestBase
{
    private static readonly string Root = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ".."));
    private static readonly string DotnetDir = Path.GetFullPath(Path.Combine(Root, ".dotnet"));
    private static readonly string Dotnet = Path.GetFullPath(Path.Combine(Root, ".dotnet", OSUtils.IsWindows ? "dotnet.exe" : "dotnet"));

    // Session mutex: held for the entire test session (build + tests).
    // The process that creates it (createdNew=true) is the builder.
    private static Mutex? s_sessionMutex;
    private static bool s_isSessionMutexOwner;

    public static void BuildTestAssetsForIntegrationTests(TestContext context)
    {
        // This is common setup that should happen even when build will not run.
        SetDotnetEnvironment();

        var skipBuild = GetTestParameter(context, "SkipIntegrationTestBuild");
        if (skipBuild)
        {
            Debug.WriteLine("SkipIntegrationBuild parameter is true, skipping build.");
            return;
        }

        var buildCompatibility = GetTestParameter(context, "BuildCompatibility");

        // Avoid collisions across different clones of this repo.
        var baseName = "VSTest_AcceptanceTests_" + IntegrationTestEnvironment.RepoRootDirectory!
            .Replace('\\', '-').Replace('/', '-').Replace(':', '-');

        // Session mutex: the process that creates it is the builder.
        // Held for the entire session (build + tests), destroyed in CleanupTestAssets.
        s_sessionMutex = CreateMutex(baseName + "_Session", out bool isBuilder);
        s_isSessionMutexOwner = isBuilder;

        if (isBuilder)
        {
            // We are the builder. Create a build mutex to signal "build in progress"
            // to other processes. It exists while we're building, then we destroy it.
            Debug.WriteLine("Session mutex created — we are the builder.");
            using var buildMutex = CreateMutex(baseName + "_Build", out _);

            var sw = Stopwatch.StartNew();
            var nugetCache = IntegrationTestEnvironment.TestAssetsNuGetCacheDirectory;
            var packagesAreNew = UnzipExecutablePackages();
            if (packagesAreNew)
            {
                CleanNugetCacheAndProjects(nugetCache);
            }
            Debug.WriteLine($"Unzipping packages took: {sw.ElapsedMilliseconds} ms"); sw.Restart();
            BuildTestAssets(nugetCache);
            Debug.WriteLine($"Building test assets took: {sw.ElapsedMilliseconds} ms"); sw.Restart();
            if (buildCompatibility)
            {
                BuildTestAssetsCompatibility(nugetCache);
                Debug.WriteLine($"Building compatibility test assets took: {sw.ElapsedMilliseconds} ms"); sw.Restart();
            }
            else
            {
                Debug.WriteLine("BuildCompatibility parameter is false, skipping build.");
            }
            CopyAndPatchDotnet();
            Debug.WriteLine($"Copying and patching dotnet took: {sw.ElapsedMilliseconds} ms");

            // Build mutex is disposed here (end of using), signaling to other processes
            // that the build is done.
            Debug.WriteLine("Build complete, releasing build mutex.");
        }
        else
        {
            // We are NOT the builder. Check if a build is in progress.
            Debug.WriteLine("Session mutex already exists — another process is the builder.");
            var buildMutex = TryOpenMutex(baseName + "_Build");
            if (buildMutex is not null)
            {
                // Build mutex exists — build is in progress. Wait for it to be destroyed.
                Debug.WriteLine("Build in progress, waiting for it to finish...");
                using (buildMutex)
                {
                    // Wait to acquire it (builder will release/destroy it when done).
                    try
                    {
                        if (!buildMutex.WaitOne(TimeSpan.FromMinutes(15)))
                        {
                            throw new TimeoutException("Timed out waiting for the build to complete.");
                        }
                    }
                    catch (AbandonedMutexException)
                    {
                        // Builder crashed — we got the mutex. Destroy it and retry
                        // as a fresh session (we might become the builder).
                        Debug.WriteLine("Build mutex was abandoned (builder crashed). Retrying.");
                    }
                }
                // Build mutex destroyed. Build is done (or builder crashed and we'll
                // see a stale state — but test assets should still be usable).
                Debug.WriteLine("Build finished, proceeding to tests.");
            }
            else
            {
                // Build mutex doesn't exist — build already completed.
                Debug.WriteLine("No build mutex found — build already done, proceeding to tests.");
            }
        }
    }

    /// <summary>
    /// Create a named mutex. If abandoned by a previous process, destroy and retry.
    /// </summary>
    private static Mutex CreateMutex(string name, out bool createdNew)
    {
        while (true)
        {
            var mutex = new Mutex(initiallyOwned: true, name, out createdNew);
            if (!createdNew)
            {
                try
                {
                    // We didn't create it, try to acquire to check if it's alive.
                    if (mutex.WaitOne(0))
                    {
                        // We acquired it — previous owner released. Release and return.
                        mutex.ReleaseMutex();
                    }
                    // If WaitOne returned false, mutex is held by another process — that's fine.
                }
                catch (AbandonedMutexException)
                {
                    // Previous owner crashed. Destroy and retry.
                    Debug.WriteLine($"Mutex '{name}' was abandoned, destroying and retrying.");
                    mutex.Dispose();
                    continue;
                }
            }
            return mutex;
        }
    }

    /// <summary>
    /// Try to open an existing named mutex. Returns null if it doesn't exist.
    /// </summary>
    private static Mutex? TryOpenMutex(string name)
    {
        var mutex = new Mutex(initiallyOwned: false, name, out bool createdNew);
        if (createdNew)
        {
            // We created it — meaning it didn't exist. Destroy it and return null.
            mutex.Dispose();
            return null;
        }
        return mutex;
    }

    public static void CleanupTestAssets()
    {
        if (s_sessionMutex is not null)
        {
            if (s_isSessionMutexOwner)
            {
                try { s_sessionMutex.ReleaseMutex(); } catch (ApplicationException) { }
            }
            s_sessionMutex.Dispose();
            s_sessionMutex = null;
        }
    }

    private static void BuildTestAssets(string nugetCache)
    {
        var testAssets = Path.GetFullPath(Path.Combine(Root, "test", "TestAssets", "TestAssets.slnx"));
        var nugetFeeds = GetNugetSourceParameters(Root);

        var netTestSdkVersion = IntegrationTestEnvironment.LatestLocallyBuiltNugetVersion;

        ExecuteApplication2(Dotnet, $"""restore --packages {nugetCache} {nugetFeeds} --source "{IntegrationTestEnvironment.LocalPackageSource}" -p:PackageVersion={netTestSdkVersion} "{testAssets}" """);
        ExecuteApplication2(Dotnet, $"""build "{testAssets}" --configuration {IntegrationTestEnvironment.BuildConfiguration} --no-restore""");

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            // Build special project written in IL.
            // This project is used on Windows only Tests. On non-Windows the build fails with: "IlasmToolPath must be set in order to build ilproj's outside of Windows.".
            var cilProject = Path.Combine(Root, "test", "TestAssets", "CILProject", "CILProject.proj");
            var binPath = Path.Combine(Root, "artifacts", "bin", "TestAssets", "CILProject", IntegrationTestEnvironment.BuildConfiguration, "net462");
            ExecuteApplication2(Dotnet, $"""restore --packages {nugetCache} {nugetFeeds} --source "{IntegrationTestEnvironment.LocalPackageSource}" "{cilProject}" """);
            ExecuteApplication2(Dotnet, $"""build "{cilProject}" --configuration {IntegrationTestEnvironment.BuildConfiguration} --no-restore --output {binPath}""");
        }
    }

    private static void SetDotnetEnvironment()
    {
        // We need to set this to point to our dotnet, because we cannot guarantee what is installed on the machine in Program Files,
        // and we install all the required SDKs and runtimes ourselves in Build.cmd.
#pragma warning disable RS0030 // Do not used banned APIs
        Environment.SetEnvironmentVariable("DOTNET_ROOT", DotnetDir);
        Environment.SetEnvironmentVariable("DOTNET_ROOT(x86)", Path.Combine(DotnetDir, "dotnet-sdk-x86"));
        Environment.SetEnvironmentVariable("PATH", $"{DotnetDir};{Environment.GetEnvironmentVariable("PATH")}");
#pragma warning restore RS0030 // Do not used banned APIs
    }

    private static void CopyAndPatchDotnet()
    {
        var patchedDotnetDir = Path.GetFullPath(Path.Combine(Root, "artifacts", "tmp", ".dotnet"));

        var dotnetExe = OSUtils.IsWindows ? "dotnet.exe" : "dotnet";
        var originalDotnetExePath = Path.Combine(DotnetDir, dotnetExe);
        var patchedDotnetExePath = Path.Combine(patchedDotnetDir, dotnetExe);

        // It is not necessary to copy whole dotnet folder before each test run
        // we just need to make sure the build files are updated automatically,
        // so dotnet test tests reflect what is in our local build targets.
        bool skipCopy = File.Exists(originalDotnetExePath)
            && File.Exists(patchedDotnetExePath)
            && File.GetLastWriteTime(originalDotnetExePath) == File.GetLastWriteTime(patchedDotnetExePath);

        if (!skipCopy)
        {
            // Copy .dotnet
            DirectoryUtils.CopyDirectory(new DirectoryInfo(DotnetDir), new DirectoryInfo(patchedDotnetDir));
        }

        // e.g. artifacts\tmp\.dotnet\sdk\
        var sdkDirectory = Path.Combine(patchedDotnetDir, "sdk");
        // e.g. artifacts\tmp\.dotnet\sdk\8.0.100-preview.6.23330.14
        var dotnetSdkDirectories = Directory.GetDirectories(sdkDirectory);
        if (dotnetSdkDirectories.Length == 0)
        {
            throw new InvalidOperationException($"No .NET SDK directories found in '{sdkDirectory}'.");
        }
        if (dotnetSdkDirectories.Length > 1)
        {
            throw new InvalidOperationException($"More than 1 .NET SDK directories found in '{sdkDirectory}': {string.Join(", ", dotnetSdkDirectories)}.");
        }

        var dotnetSdkDirectory = dotnetSdkDirectories.Single();

        // Copy target file and build task dll into it.
        // This updates the definition for running dotnet test from what we have built locally.
        var netTestSdkVersion = IntegrationTestEnvironment.LatestLocallyBuiltNugetVersion;
        var packageName = $"Microsoft.TestPlatform.Build.{netTestSdkVersion}.nupkg";
        var packagePath = Path.GetFullPath(Path.Combine(IntegrationTestEnvironment.PublishDirectory, packageName));

        DirectoryUtils.CopyDirectory(Path.Combine(packagePath, "lib", "netstandard2.0"), dotnetSdkDirectory);
        DirectoryUtils.CopyDirectory(Path.Combine(packagePath, "runtimes", "any", "native"), dotnetSdkDirectory);
    }

    private static void BuildTestAssetsCompatibility(string nugetCache)
    {
        var testAssetsDir = Path.GetFullPath(Path.Combine(Root, "test", "TestAssets"));

        var generated = Path.GetFullPath(Path.Combine(Root, "artifacts", "tmp", "GeneratedTestAssets"));
        var generatedSln = Path.Combine(generated, "CompatibilityTestAssets.slnx");

        var dependenciesPath = Path.Combine(Root, "eng", "Versions.props");
        var dependenciesXml = XDocument.Load(dependenciesPath);
        var propsNode = dependenciesXml!.Element("Project")!
            .Descendants("PropertyGroup")
            .Where(p => p.Attributes().Any(a => a.Name == "Label" && a.Value == "VSTest test settings"))
            .Single()!;

        var cacheId = new OrderedDictionary();

        // Restore previous versions of TestPlatform (for vstest.console.exe), and TestPlatform.CLI (for vstest.console.dll).
        // These properties are coming from TestPlatform.Dependencies.props.
        var vstestConsoleVersionProperties = new[] {
            "VSTestConsoleLatestVersion",
            "VSTestConsoleLatestPreviewVersion",
            "VSTestConsoleLatestStableVersion",
            "VSTestConsoleRecentStableVersion",
            "VSTestConsoleMostDownloadedVersion",
            "VSTestConsolePreviousStableVersion",
            "VSTestConsoleLegacyStableVersion",
        };

        var projects = new[]
        {
            Path.Combine(Root, "test", "TestAssets", "MSTestProject1", "MSTestProject1.csproj"),
            Path.Combine(Root, "test", "TestAssets", "MSTestProject2", "MSTestProject2.csproj"),
        };

        var msTestVersionProperties = new[] {
            "MSTestFrameworkLatestPreviewVersion",
            "MSTestFrameworkLatestStableVersion",
            "MSTestFrameworkRecentStableVersion",
            "MSTestFrameworkMostDownloadedVersion",
            "MSTestFrameworkPreviousStableVersion",
            "MSTestFrameworkLegacyStableVersion",
        };

        var nugetFeeds = GetNugetSourceParameters(Root);

        // We use the same version properties for NET.Test.Sdk as for VSTestConsole, for now.
        foreach (var sdkPropertyName in vstestConsoleVersionProperties)
        {
            string? netTestSdkVersion;
            if (sdkPropertyName == "VSTestConsoleLatestVersion")
            {
                netTestSdkVersion = IntegrationTestEnvironment.LatestLocallyBuiltNugetVersion;
            }
            else
            {
                netTestSdkVersion = propsNode.Element(sdkPropertyName!)!.Value;
            }

            if (netTestSdkVersion.IsNullOrWhiteSpace())
            {
                throw new InvalidOperationException($"{nameof(netTestSdkVersion)} should contain version of the package to restore, but it is empty.");
            }

            cacheId[sdkPropertyName] = netTestSdkVersion;

            var netTestSdkVersionDir = netTestSdkVersion.TrimStart('[').TrimEnd(']');
            if (Directory.Exists(Path.Combine(nugetCache, "microsoft.testplatform", netTestSdkVersionDir)) && Directory.Exists(Path.Combine(nugetCache, "microsoft.testplatform.cli", netTestSdkVersionDir)))
            {
                continue;
            }

            // We restore this project to download TestPlatform and TestPlatform.CLI nugets, into our package cache.
            // Using nuget.exe install errors out in various weird ways.
            var tools = Path.Combine(Root, "test", "TestAssets", "Tools", "Tools.csproj");
            ExecuteApplication2(Dotnet, $"""restore --packages {nugetCache} {nugetFeeds} --source "{IntegrationTestEnvironment.LocalPackageSource}" "{tools}" -p:PackageVersion={netTestSdkVersionDir} """);
        }

        foreach (var propertyName in msTestVersionProperties)
        {
            var mstestVersion = propsNode.Element(propertyName)!.Value;
            cacheId[propertyName] = mstestVersion;
        }

        cacheId["projects"] = projects;

        var cacheIdText = JsonConvert.SerializeObject(cacheId, Formatting.Indented);

        var currentCacheId = File.Exists(Path.Combine(generated, "checksum.json")) ? File.ReadAllText(Path.Combine(generated, "checksum.json")) : null;

        var rebuild = true;
        if (cacheIdText == currentCacheId)
        {
            // Project cache is up-to-date, just rebuilding solution.
            ExecuteApplication2(Dotnet, $"""restore --packages {nugetCache} {nugetFeeds} --source "{IntegrationTestEnvironment.LocalPackageSource}" "{generatedSln}" """);
            ExecuteApplication2(Dotnet, $"build {generatedSln} --no-restore --configuration {IntegrationTestEnvironment.BuildConfiguration} -v:minimal");
            rebuild = false;
        }

        if (rebuild)
        {
            if (Directory.Exists(generated))
            {
                Directory.Delete(generated, recursive: true);
            }

            Directory.CreateDirectory(generated);

            // Fix repo root, we are 1 level deeper than in the test/TestAssets location.
            var buildPropsContent = File.ReadAllText(Path.Combine(testAssetsDir, "Directory.Build.props"));
            buildPropsContent = Regex.Replace(buildPropsContent, "<RepoRoot.*RepoRoot>", "<RepoRoot>$(MSBuildThisFileDirectory)../../../</RepoRoot>");
            File.WriteAllText(Path.Combine(generated, "Directory.Build.props"), buildPropsContent);

            File.Copy(Path.Combine(testAssetsDir, "Directory.Build.targets"), Path.Combine(generated, "Directory.Build.targets"));

            ExecuteApplication2(Dotnet, $"""new sln --name CompatibilityTestAssets --output "{generated}""");

            var projectsToAdd = new List<string>();
            foreach (var project in projects)
            {
                var projectName = Path.GetFileName(project);
                var projectBaseName = Path.GetFileNameWithoutExtension(projectName);
                var projectDir = Path.GetDirectoryName(project)!;
                var projectItems = Directory.GetFiles(projectDir, "*", SearchOption.AllDirectories).Where(p =>
                    !p.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}")
                    && !p.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}")
                    // Is a file, and not a directory.
                    && File.Exists(p)).ToList();

                foreach (var sdkPropertyName in vstestConsoleVersionProperties)
                {
                    string netTestSdkVersion;
                    if (sdkPropertyName == "VSTestConsoleLatestVersion")
                    {
                        netTestSdkVersion = IntegrationTestEnvironment.LatestLocallyBuiltNugetVersion;
                    }
                    else
                    {
                        netTestSdkVersion = propsNode.Element(sdkPropertyName!)!.Value;
                    }

                    var dirNetTestSdkVersion = netTestSdkVersion.TrimStart('[').TrimEnd(']');
                    var dirNetTestSdkPropertyName = sdkPropertyName
                        .Replace("Framework", "")
                        .Replace("Version", "")
                        .Replace("VSTestConsole", "NETTestSdk");

                    foreach (var propertyName in msTestVersionProperties)
                    {
                        var mstestVersion = propsNode.Element(propertyName)!.Value;

                        var dirMSTestVersion = mstestVersion.TrimStart('[').TrimEnd(']');
                        var dirMSTestPropertyName = propertyName
                            .Replace("Framework", "")
                            .Replace("Version", "");

                        // Do not make this a folder structure, it will break the relative reference to scripts\build\TestAssets.props that we have in the project,
                        // because the relative path will be different.
                        // It would be nice to use fully descriptive name but it is too long, hash the versions instead.
                        // $compatibilityProjectDir = "$generated/$projectBaseName--$dirNetTestSdkPropertyName-$dirNetTestSdkVersion--$dirMSTestPropertyName-$dirMSTestVersion"

                        var versions = $"{dirNetTestSdkPropertyName}-{dirNetTestSdkVersion}--{dirMSTestPropertyName}-{dirMSTestVersion}";
                        var hash = IntegrationTestEnvironment.GetPathHash(versions);
                        var projectShortName = $"{projectBaseName}--{hash}";
                        var compatibilityProjectDir = Path.Combine(generated, projectShortName);

                        Directory.CreateDirectory(compatibilityProjectDir);

                        foreach (var projectItem in projectItems)
                        {
                            var relativePath = projectItem.Replace(projectDir, "").TrimStart(Path.DirectorySeparatorChar);
                            var fullPath = Path.Combine(compatibilityProjectDir, relativePath);
                            File.Copy(projectItem, fullPath);
                        }

                        var compatibilityCsproj = Directory.GetFiles(compatibilityProjectDir, "*.csproj", SearchOption.AllDirectories);

                        if (!compatibilityCsproj.Any())
                        {
                            throw new InvalidOperationException($"No .csproj file was found in directory {compatibilityProjectDir}.");
                        }

                        if (compatibilityCsproj.Length > 1)
                        {
                            throw new InvalidOperationException($"More than 1 .csproj file was found in directory {compatibilityProjectDir}.");
                        }

                        var csproj = compatibilityCsproj.Single();

                        var content = File.ReadAllText(csproj);
                        // We replace the content rather than using MSBuild properties, because that allows us to create a solution with
                        // many versions of the package, and let msbuild figure out how to correctly restore and build in parallel. If we did use
                        // MSBuild properties we would have to build each combination one by one in sequence.
                        var newContent = content
                            .Replace("$(MSTestTestFrameworkVersion)", mstestVersion)
                            .Replace("$(MSTestTestAdapterVersion)", mstestVersion)
                            .Replace("$(PackageVersion)", netTestSdkVersion);
                        File.WriteAllText(csproj, newContent);

                        var uniqueCsprojName = Path.Combine(compatibilityProjectDir, $"{projectShortName}.csproj");
                        File.Move(csproj, uniqueCsprojName);
                        projectsToAdd.Add(uniqueCsprojName);
                    }
                }
            }

            ExecuteApplication2(Dotnet, $"""sln {generatedSln} add "{string.Join("\" \"", projectsToAdd)}" """);

            ExecuteApplication2(Dotnet, $"""restore --packages {nugetCache} {nugetFeeds} --source "{IntegrationTestEnvironment.LocalPackageSource}" "{generatedSln}" """);
            ExecuteApplication2(Dotnet, $"""build --no-restore --configuration {IntegrationTestEnvironment.BuildConfiguration} "{generatedSln}" """);

            File.WriteAllText(Path.Combine(generated, "checksum.json"), cacheIdText);
        }
    }

    private static string GetNugetSourceParameters(string root)
    {
        string nugetConfigPath = Path.Combine(root, "NuGet.config");
        var nugetConfig = XDocument.Load(nugetConfigPath);

        var feeds = nugetConfig!
            .Element("configuration")!
            .Element("packageSources")!
            .Descendants("add")
            .Where(p => p.Attributes().Any(a => a.Name == "key"))
            .SelectMany(p => p.Attributes())
            .Where(a => a.Name == "value")
            .Select(a => a.Value)
            .ToList();

        if (feeds.Count == 0)
        {
            throw new InvalidOperationException($"No feeds were loaded from '{nugetConfigPath}'.");
        }

        // --source "value1" --source "value2", including quotes
        var parameters = $"""--source "{string.Join("\" --source \"", feeds)}" """;

        return parameters;
    }

    protected static void ExecuteApplication2(string path, string? args,
        Dictionary<string, string?>? environmentVariables = null, string? workingDirectory = null)
    {

        ExecuteApplication(path, args, out string stdOut, out string stdError, out int exitCode, environmentVariables, workingDirectory);
        if (exitCode != 0)
        {
            throw new InvalidOperationException(
                $"""
                Executing '{path} {args}' failed.
                STDOUT: {stdOut},
                STDERR: {stdError}
                """);
        }
    }

    private static bool UnzipExecutablePackages()
    {
        var netTestSdkVersion = IntegrationTestEnvironment.LatestLocallyBuiltNugetVersion;

        // Extract locally built packages that have our tools (like vstest.console.exe) into tmp directory,
        // so we can use them to run tests.
        var packagesToExtract = new[]
{
            $"Microsoft.TestPlatform.{netTestSdkVersion}.nupkg",
            $"Microsoft.TestPlatform.CLI.{netTestSdkVersion}.nupkg",
            $"Microsoft.TestPlatform.Build.{netTestSdkVersion}.nupkg",
            $"Microsoft.CodeCoverage.{netTestSdkVersion}.nupkg",
            $"Microsoft.TestPlatform.Portable.{netTestSdkVersion}.nupkg",
        };

        var packagesAreNew = false;
        foreach (var packageName in packagesToExtract)
        {
            var packagePath = Path.Combine(IntegrationTestEnvironment.LocalPackageSource, packageName);
            var unzipPath = Path.Combine(IntegrationTestEnvironment.PublishDirectory, packageName);

            var cacheMarkerPath = Path.Combine(unzipPath, packageName + ".cache");
            if (File.Exists(cacheMarkerPath))
            {
                if (File.ReadAllText(cacheMarkerPath) == File.GetLastWriteTimeUtc(packagePath).ToString("o", CultureInfo.InvariantCulture))
                {
                    // Already extracted and using the latest built packages.
                    continue;
                }
            }

            // I any package is new we will clean the package cache before restore and build.
            packagesAreNew |= true;

            if (Directory.Exists(unzipPath))
            {
                Directory.Delete(unzipPath, recursive: true);
            }

            ZipFile.ExtractToDirectory(packagePath, unzipPath);
            File.WriteAllText(cacheMarkerPath, File.GetLastWriteTimeUtc(packagePath).ToString("o", CultureInfo.InvariantCulture));
        }

        return packagesAreNew;
    }

    private static void CleanNugetCacheAndProjects(string nugetCache)
    {
        // dotnet clean needs the packages in place, but here we don't yet know what projects we will build
        // luckily they are all built into artifacts/bin/TestAssets and artifacts/obj/TestAssets so we just need to delete
        // the obj to force re-build in the next steps.

        var objPath = Path.Combine(Root, "artifacts", "obj", "TestAssets");
        if (Directory.Exists(objPath))
        {
            Directory.Delete(objPath, recursive: true);
        }

        // Then clean all -dev and -ci packages from the cache to force updating from local source.
        if (Directory.Exists(nugetCache))
        {
            foreach (var packageDir in Directory.GetDirectories(nugetCache))
            {
                foreach (var versionDir in Directory.GetDirectories(packageDir))
                {
                    if (versionDir.EndsWith("-dev") || versionDir.EndsWith("-ci"))
                    {
                        Directory.Delete(versionDir, recursive: true);
                    }
                }
            }
        }

        // Unzip VSIX so we can test with it on Windows.
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            var vsixPath = IntegrationTestEnvironment.LocalVsixInsertion;
            var vsixUnzipPath = Path.Combine(IntegrationTestEnvironment.PublishDirectory, Path.GetFileName(vsixPath));
            if (Directory.Exists(vsixUnzipPath))
            {
                Directory.Delete(vsixUnzipPath, recursive: true);
            }

            ZipFile.ExtractToDirectory(vsixPath, vsixUnzipPath);
        }
    }

    public static bool GetTestParameter(TestContext testContext, string parameterName)
    {
        return testContext.Properties.TryGetValue(parameterName, out var skipBuildValue)
            ? bool.TryParse((string?)skipBuildValue, out var value)
                ? value
                : false
            : false;
    }

}

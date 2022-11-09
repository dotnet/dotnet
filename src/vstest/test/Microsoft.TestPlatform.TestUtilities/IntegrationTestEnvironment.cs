// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Xml;

using Microsoft.VisualStudio.TestPlatform.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.TestUtilities;

/// <summary>
/// Provider for test environment configuration.
/// Currently reads configuration from environment variables. We may support a
/// different provider later. E.g. run settings.
/// </summary>
public class IntegrationTestEnvironment
{
    public static string TestPlatformRootDirectory { get; private set; } =
        Environment.GetEnvironmentVariable("TP_ROOT_DIR")
        ?? Path.GetFullPath(@"..\..\..\..\..".Replace('\\', Path.DirectorySeparatorChar));

    private static Dictionary<string, string>? s_dependencyVersions;

    private string? _targetRuntime;

    public IntegrationTestEnvironment()
    {
        // If the variables are not set, valid defaults are assumed.
        if (TargetFramework.IsNullOrEmpty())
        {
            // Run integration tests for net462 by default.
            TargetFramework = "net462";
        }

        if (TestPlatformRootDirectory.IsNullOrEmpty())
        {
            // Running in VS/IDE. Use artifacts directory as root.
            // Get root directory from test assembly output directory
            TestPlatformRootDirectory = Path.GetFullPath(@"..\..\..\..\..".Replace('\\', Path.DirectorySeparatorChar));
        }

        TestAssetsPath = Path.Combine(TestPlatformRootDirectory, $@"test{Path.DirectorySeparatorChar}TestAssets");

        // There is an assumption that integration tests will always run from a source enlistment.
        // Need to remove this assumption when we move to a CDP.
        PackageDirectory = Path.Combine(TestPlatformRootDirectory, @"packages");
        ToolsDirectory = Path.Combine(TestPlatformRootDirectory, @"tools");
        TestArtifactsDirectory = Path.Combine(TestPlatformRootDirectory, "artifacts", "testArtifacts");
        RunnerFramework = "net462";
    }

    /// <summary>
    /// Gets the build configuration for the test run.
    /// </summary>
    public static string BuildConfiguration
    {
        get
        {
#if DEBUG
            return "Debug";
#else
            return "Release";
#endif
        }
    }

    public static Dictionary<string, string> DependencyVersions
        => s_dependencyVersions ??= GetDependencies(TestPlatformRootDirectory);

    /// <summary>
    /// Gets the nuget packages directory for enlistment.
    /// </summary>
    public string PackageDirectory { get; private set; }

    /// <summary>
    /// Gets the publish directory for <c>vstest.console</c> package.
    /// </summary>
    public string PublishDirectory
    {
        get
        {
            // this used to switch to src\package\package\bin\based on whether
            // this is running in cli, but that's a bad idea, the console there does not have
            // a runtime config and will fail to start with error testhostpolicy.dll not found
            var publishDirectory = Path.Combine(
                TestPlatformRootDirectory,
                "artifacts",
                BuildConfiguration,
                RunnerFramework,
                TargetRuntime!);

            return !Directory.Exists(publishDirectory)
                ? throw new InvalidOperationException($"Path '{publishDirectory}' does not exist, did you build the solution via build.cmd?")
                : publishDirectory;
        }
    }

    /// <summary>
    /// Gets the extensions directory for <c>vstest.console</c> package.
    /// </summary>
    public string ExtensionsDirectory => Path.Combine(PublishDirectory, "Extensions");

    /// <summary>
    /// Gets the target framework.
    /// Supported values = <c>net462</c>, <c>netcoreapp3.1</c>.
    /// </summary>
    [NotNull]
    public string? TargetFramework { get; set; }

    /// <summary>
    /// Gets the target runtime.
    /// Supported values = <c>win7-x64</c>.
    /// </summary>
    public string? TargetRuntime
    {
        get
        {
            if (RunnerFramework == IntegrationTestBase.DesktopRunnerFramework)
            {
                if (_targetRuntime.IsNullOrEmpty())
                {
                    _targetRuntime = "win7-x64";
                }
            }
            else
            {
                _targetRuntime = "";
            }

            return _targetRuntime;
        }
        set
        {
            _targetRuntime = value;
        }
    }

    /// <summary>
    /// Gets the inIsolation.
    /// Supported values = <c>/InIsolation</c>.
    /// </summary>
    public string? InIsolationValue { get; set; }

    /// <summary>
    /// Gets the root directory for test assets.
    /// </summary>
    public string TestAssetsPath { get; }

    /// <summary>
    /// Gets the tools directory for dependent tools
    /// </summary>
    public string ToolsDirectory { get; private set; }

    /// <summary>
    /// Gets the test artifacts directory.
    /// </summary>
    public string TestArtifactsDirectory { get; private set; }

    /// <summary>
    /// Gets the application type.
    /// Supported values = <c>net462</c>, <c>netcoreapp3.1</c>.
    /// </summary>
    public string RunnerFramework { get; set; }

    // A known AzureDevOps env variable meaning we are running in CI.
    public static bool IsCI { get; } = Environment.GetEnvironmentVariable("TF_BUILD") == "True";
    public DebugInfo? DebugInfo { get; set; }
    public VSTestConsoleInfo? VSTestConsoleInfo { get; set; }
    public List<DllInfo> DllInfos { get; set; } = new();

    /// <summary>
    /// Gets the full path to a test asset.
    /// </summary>
    /// <param name="assetName">Name of the asset with extension. E.g. <c>SimpleUnitTest.dll</c></param>
    /// <returns>Full path to the test asset.</returns>
    /// <remarks>
    /// Test assets follow several conventions:
    /// (a) They are built for supported frameworks. See <see cref="TargetFramework"/>.
    /// (b) They are built for provided build configuration.
    /// (c) Name of the test asset matches the parent directory name. E.g. <c>TestAssets\SimpleUnitTest\SimpleUnitTest.csproj</c> must
    /// produce <c>TestAssets\SimpleUnitTest\bin\Debug\net462\SimpleUnitTest.dll</c>
    /// </remarks>
    public string GetTestAsset(string assetName)
    {
        return GetTestAsset(assetName, TargetFramework!);
    }

    /// <summary>
    /// Gets the full path to a test asset.
    /// </summary>
    /// <param name="assetName">Name of the asset with extension. E.g. <c>SimpleUnitTest.dll</c></param>
    /// <param name="targetFramework">asset project target framework. E.g <c>net462</c></param>
    /// <returns>Full path to the test asset.</returns>
    /// <remarks>
    /// Test assets follow several conventions:
    /// (a) They are built for supported frameworks. See <see cref="TargetFramework"/>.
    /// (b) They are built for provided build configuration.
    /// (c) Name of the test asset matches the parent directory name. E.g. <c>TestAssets\SimpleUnitTest\SimpleUnitTest.csproj</c> must
    /// produce <c>TestAssets\SimpleUnitTest\bin\Debug\net462\SimpleUnitTest.dll</c>
    /// </remarks>
    public string GetTestAsset(string assetName, string targetFramework)
    {
        var simpleAssetName = Path.GetFileNameWithoutExtension(assetName);
        var assetPath = Path.Combine(
            TestAssetsPath,
            simpleAssetName,
            "bin",
            BuildConfiguration,
            targetFramework,
            assetName);

        // Update the path to be taken from the compatibility matrix instead of from the root folder.
        if (DllInfos.Count > 0)
        {
            // The path is really ugly: S:\p\vstest3\test\GeneratedTestAssets\NETTestSdkLegacyStable-15.9.2--MSTestMostDownloaded-2.1.0--MSTestProject2\bin\Debug\net462\MSTestProject2-NETTestSdkLegacyStable-15.9.2--MSTestMostDownloaded-2.1.0.dll
            // And we need to hash the versions in it to get shorter path as well.
            var versions = string.Join("--", DllInfos.Select(d => d.Path));
            var versionsHash = Hash(versions);
            assetPath = Path.Combine(TestAssetsPath, "..", "GeneratedTestAssets", $"{simpleAssetName}--{versionsHash}", "bin", BuildConfiguration, targetFramework, $"{simpleAssetName}--{versionsHash}.dll");
        }

        Assert.IsTrue(File.Exists(assetPath), "GetTestAsset: Path not found: \"{0}\". Most likely you need to build using build.cmd -s PrepareAcceptanceTests.", assetPath);

        // If you are thinking about wrapping the path in double quotes here,
        // then don't. File.Exist cannot handle quoted paths, and we use it in a lot of places.
        return assetPath;

        static string Hash(string value)
        {
            unchecked
            {
                long hash = 23;
                foreach (char ch in value)
                {
                    hash = hash * 31 + ch;
                }

                return $"{hash:X}";
            }
        }
    }

    /// <summary>
    /// Gets the full path to a nuget package.
    /// </summary>
    /// <param name="packageSuffix">Suffix for the nuget package.</param>
    /// <returns>Complete path to a nuget package.</returns>
    /// <remarks>GetNugetPackage("foobar") will return a path to packages\foobar.</remarks>
    public string GetNugetPackage(string packageSuffix)
    {
        var packagePath = Path.Combine(PackageDirectory, packageSuffix);

        Assert.IsTrue(Directory.Exists(packagePath), "GetNugetPackage: Directory not found: {0}.", packagePath);

        return packagePath;
    }

    private static Dictionary<string, string> GetDependencies(string testPlatformRoot)
    {
        var dependencyPropsFile = Path.Combine(testPlatformRoot, @"scripts\build\TestPlatform.Dependencies.props".Replace('\\', Path.DirectorySeparatorChar));
        var dependencyProps = new Dictionary<string, string>();
        if (!File.Exists(dependencyPropsFile))
        {
            throw new FileNotFoundException("Dependency props file not found: " + dependencyPropsFile);
        }

        using (var reader = XmlReader.Create(dependencyPropsFile))
        {
            reader.ReadToFollowing("PropertyGroup");
            using var props = reader.ReadSubtree();
            props.MoveToContent();
            props.Read();   // Read thru the PropertyGroup node
            while (!props.EOF)
            {
                if (props.IsStartElement() && !props.Name.IsNullOrEmpty())
                {
                    if (!dependencyProps.ContainsKey(props.Name))
                    {
                        dependencyProps.Add(props.Name, props.ReadElementContentAsString());
                    }
                    else
                    {
                        dependencyProps[props.Name] = string.Join(", ", dependencyProps[props.Name], props.ReadElementContentAsString());
                    }
                }
                props.Read();
            }
        }

        return dependencyProps;
    }

    /// <summary>
    /// Gets the full path to a test asset.
    /// </summary>
    /// <param name="assetName">Name of the asset with extension. E.g. <c>SimpleUnitTest.csproj</c></param>
    /// <returns>Full path to the test asset.</returns>
    /// <remarks>
    /// Test assets follow several conventions:
    /// (a) They are built for supported frameworks. See <see cref="TargetFramework"/>.
    /// (b) They are built for provided build configuration.
    /// (c) Name of the test asset matches the parent directory name. E.g. <c>TestAssets\SimpleUnitTest\SimpleUnitTest.csproj</c> must
    /// produce <c>TestAssets\SimpleUnitTest\SimpleUnitTest.csproj</c>
    /// </remarks>
    public string GetTestProject(string assetName)
    {
        var simpleAssetName = Path.GetFileNameWithoutExtension(assetName);
        var assetPath = Path.Combine(
            TestAssetsPath,
            simpleAssetName,
            assetName);

        Assert.IsTrue(File.Exists(assetPath), "GetTestAsset: Path not found: \"{0}\".", assetPath);

        return assetPath;
    }
}

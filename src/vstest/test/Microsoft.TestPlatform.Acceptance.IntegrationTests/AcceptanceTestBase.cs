// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Linq;

using Microsoft.TestPlatform.TestUtilities;

namespace Microsoft.TestPlatform.AcceptanceTests;

public class AcceptanceTestBase : IntegrationTestBase
{
    public const string DesktopTargetFramework = Net462TargetFramework;
    public const string Net462TargetFramework = "net462";
    public const string Net47TargetFramework = "net47";
    public const string Net471TargetFramework = "net471";
    public const string Net472TargetFramework = "net472";
    public const string Net48TargetFramework = "net48";
    public const string Core80TargetFramework = "net8.0";
    public const string Core90TargetFramework = "net9.0";
    public const string Core10TargetFramework = "net10.0";

    public const string DesktopFrameworkArgValue = Net462FrameworkArgValue;
    public const string Net462FrameworkArgValue = ".NETFramework,Version=v4.6.2";
    public const string Net47FrameworkArgValue = ".NETFramework,Version=v4.7";
    public const string Net471FrameworkArgValue = ".NETFramework,Version=v4.7.1";
    public const string Net472FrameworkArgValue = ".NETFramework,Version=v4.7.2";
    public const string Net48FrameworkArgValue = ".NETFramework,Version=v4.8";

    public const string Core80FrameworkArgValue = ".NETCoreApp,Version=v8.0";
    public const string Core90FrameworkArgValue = ".NETCoreApp,Version=v9.0";
    public const string Core10FrameworkArgValue = ".NETCoreApp,Version=v10.0";

    public const string DesktopRunnerTargetRuntime = "win7-x64";
    public const string CoreRunnerTargetRuntime = "";
    public const string InIsolation = "/InIsolation";

    public const string NETFX462_48 = "net462;net472;net48";
    public const string NETFX462_NET9 = "net462;net472;net48;net8.0;net9.0";
    public const string DEFAULT_RUNNER_NETFX = Net462TargetFramework;
    public const string DEFAULT_HOST_NETFX = Net462TargetFramework;
    public const string DEFAULT_RUNNER_NETCORE = Core80TargetFramework;
    public const string DEFAULT_HOST_NETCORE = Core80TargetFramework;
    /// <summary>
    /// Our current defaults for .NET and .NET Framework.
    /// </summary>
    public const string DEFAULT_HOST_NETFX_AND_NET = "net462;net8.0";
    public const string LATEST_TO_LEGACY = "Latest;LatestPreview;LatestStable;RecentStable;MostDownloaded;PreviousStable;LegacyStable";
    public const string LATESTPREVIEW_TO_LEGACY = "LatestPreview;LatestStable;RecentStable;MostDownloaded;PreviousStable;LegacyStable";
    public const string LATEST = "Latest";
    public const string LATESTSTABLE = "LatestStable";
    internal const string MSTEST = "MSTest";

    public static string And(string left, string right)
    {
        return string.Join(";", left, right);
    }

    protected string FrameworkArgValue => DeriveFrameworkArgValue(_testEnvironment);

    protected static void SetTestEnvironment(IntegrationTestEnvironment testEnvironment, RunnerInfo runnerInfo)
    {
        testEnvironment.VSTestConsoleInfo = runnerInfo.VSTestConsoleInfo;
        // The order here matters, it changes how the resulting path is built when we resolve test dlls and other assets.
        testEnvironment.DllInfos = new[] { runnerInfo.TestHostInfo, runnerInfo.AdapterInfo }.Where(d => d != null).Select(x => x!).ToList();
        testEnvironment.DebugInfo = runnerInfo.DebugInfo;

        testEnvironment.RunnerFramework = runnerInfo.RunnerFramework!;
        testEnvironment.TargetFramework = runnerInfo.TargetFramework;
        testEnvironment.InIsolationValue = runnerInfo.InIsolationValue;
    }

    protected static string DeriveFrameworkArgValue(IntegrationTestEnvironment testEnvironment)
        => testEnvironment.TargetFramework switch
        {
            Core80TargetFramework => Core80FrameworkArgValue,
            Core90TargetFramework => Core90FrameworkArgValue,
            Core10TargetFramework => Core10FrameworkArgValue,
            Net462TargetFramework => Net462FrameworkArgValue,
            Net47TargetFramework => Net47FrameworkArgValue,
            Net471TargetFramework => Net471FrameworkArgValue,
            Net472TargetFramework => Net472FrameworkArgValue,
            Net48TargetFramework => Net48FrameworkArgValue,
            _ => throw new NotSupportedException($"{testEnvironment.TargetFramework} is not supported TargetFramework value."),
        };

    protected bool IsDesktopTargetFramework()
        => _testEnvironment.TargetFramework == DesktopTargetFramework;

    protected string GetTargetFrameworkForRunsettings()
    {
        string targetFramework = _testEnvironment.TargetFramework == DesktopTargetFramework ? "Framework45" : "FrameworkCore10";

        return targetFramework;
    }

    /// <summary>
    /// Empty runsettings, just with the RunSettings tag that we require.
    /// </summary>
    /// <returns></returns>
    public static string GetEmptyRunsettings()
    {
        return "<RunSettings></RunSettings>";
    }

    /// <summary>
    /// Almost empty runsettings, just specifying the target framework from the currently set test environment.
    /// </summary>
    public string GetDefaultRunSettings()
    {
        return GetRunSettingsWithTargetFramework(FrameworkArgValue);
    }

    /// <summary>
    /// Almost empty runsettings, just specifying the given target framework.
    /// Use the overload without any parameters to get the target framework from the currently set test environment.
    /// </summary>
    /// <returns></returns>
    public static string GetRunSettingsWithTargetFramework(string targetFramework)
    {
        string runSettingsXml =
            $@"<?xml version=""1.0"" encoding=""utf-8""?>
            <RunSettings>
                <RunConfiguration>
                    <TargetFrameworkVersion>{targetFramework}</TargetFrameworkVersion>
                </RunConfiguration>
            </RunSettings>";

        return runSettingsXml;
    }

    protected string GetIsolatedTestAsset(string assetName)
    {
        var projectPath = GetProjectFullPath(assetName);

        foreach (var file in new FileInfo(projectPath).Directory!.EnumerateFiles())
        {
            TempDirectory.CopyFile(file.FullName);

            if (file.Extension.Equals(".csproj", StringComparison.OrdinalIgnoreCase))
            {
                string root = IntegrationTestEnvironment.RepoRootDirectory;
                var testAssetsRoot = Path.GetFullPath(Path.Combine(root, "test", "TestAssets"));

                // Replace $(RepoRoot) with the path to temp directory in which we put the csproj,
                // and write it into Directory.Build.props
                var directoryBuildProps = Path.Combine(testAssetsRoot, "Directory.Build.props");
                var newDirectoryBuildProps = Path.Combine(TempDirectory.Path, "Directory.Build.props");
                var content = File.ReadAllText(directoryBuildProps).Replace("$(RepoRoot)", TempDirectory.Path + "/");
                File.WriteAllText(newDirectoryBuildProps, content);

                // Copy Directory.Build.targets
                TempDirectory.CopyFile(Path.Combine(testAssetsRoot, "Directory.Build.targets"));

                Directory.CreateDirectory(Path.Combine(TempDirectory.Path, "eng"));
                File.Copy(Path.Combine(root, "eng", "Versions.props"),
                     Path.Combine(TempDirectory.Path, "eng", "Versions.props"));

                // Copy NuGet.config
                var nugetContent = File.ReadAllText(Path.Combine(root, "NuGet.config"))
                    // and make packages folder point to vstest packages folder
                    .Replace("\".packages\"", "\"" + Path.Combine(root, ".packages") + "\"")
                    // and add local package source
                    .Replace("</packageSources>", $"""<add key="localy-built-packages" value="{IntegrationTestEnvironment.LocalPackageSource}" /></packageSources>""");
                File.WriteAllText(Path.Combine(TempDirectory.Path, "NuGet.config"), nugetContent);
            }
        }

        return Path.Combine(TempDirectory.Path, assetName);
    }
}

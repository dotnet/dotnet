// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Microsoft.TestPlatform.TestUtilities;

public class AcceptanceTestBase : IntegrationTestBase
{
    // Target framework constants for test assets.
    public const string Net462TargetFramework = "net462";
    public const string Net481TargetFramework = "net481";
    public const string Core11TargetFramework = "net11.0";

    public const string DesktopTargetFramework = Net481TargetFramework;

    // Framework argument values for vstest.console --framework parameter.
    public const string Net462FrameworkArgValue = ".NETFramework,Version=v4.6.2";
    public const string Net481FrameworkArgValue = ".NETFramework,Version=v4.8.1";
    public const string Core11FrameworkArgValue = ".NETCoreApp,Version=v11.0";

    public const string DesktopFrameworkArgValue = Net481FrameworkArgValue;

    public const string DesktopRunnerTargetRuntime = "win7-x64";
    public const string CoreRunnerTargetRuntime = "";
    public const string InIsolation = "/InIsolation";

    // Test asset target framework lists.
    public const string NETFX = Net481TargetFramework;
    public const string NETFX_AND_NET = "net481;net11.0";

    // Runner frameworks.
    public const string RUNNER_NETFX = Net481TargetFramework;
    public const string RUNNER_NET = Core11TargetFramework;
    public const string RUNNER_NETFX_AND_NET = "net481;net11.0";

    // Host frameworks (test asset TFMs).
    public const string HOST_NETFX = Net481TargetFramework;
    public const string HOST_NET = Core11TargetFramework;
    public const string HOST_NETFX_AND_NET = "net481;net11.0";

    public const string LATEST_TO_LEGACY = "Latest;LatestPreview;LatestStable;RecentStable;MostDownloaded;PreviousStable;LegacyStable";
    public const string LATEST_TO_RECENT_STABLE = "Latest;LatestPreview;LatestStable;RecentStable";
    public const string LATESTPREVIEW_TO_LEGACY = "LatestPreview;LatestStable;RecentStable;MostDownloaded;PreviousStable;LegacyStable";
    public const string LATEST = "Latest";
    // "Special" version for runner, to take the latest from VSIX we don't ship any other component that way, so we need separate value to control it.
    public const string LATESTVSIX = "LatestVsix";
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
            Core11TargetFramework => Core11FrameworkArgValue,
            Net462TargetFramework => Net462FrameworkArgValue,
            Net481TargetFramework => Net481FrameworkArgValue,
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

    protected string GetIsolatedTestAsset(string assetName, string targetFramework)
    {
        var projectPath = GetProjectFullPath(assetName);

        foreach (var file in new FileInfo(projectPath).Directory!.EnumerateFiles())
        {
            TempDirectory.CopyFile(file.FullName);

            if (file.Extension.Equals(".csproj", StringComparison.OrdinalIgnoreCase))
            {
                // Build just for the given tfm
                var projFile = Path.Combine(TempDirectory.Path, Path.GetFileName(file.FullName));
                var csprojContent = File.ReadAllText(projFile);
                csprojContent = Regex.Replace(csprojContent, "<TargetFramework>.*?</TargetFramework>", $"<TargetFramework>{targetFramework}</TargetFramework>");
                csprojContent = Regex.Replace(csprojContent, "<TargetFrameworks>.*?</TargetFrameworks>", $"<TargetFramework>{targetFramework}</TargetFramework>");
                File.WriteAllText(projFile, csprojContent);

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
                File.Copy(Path.Combine(root, "eng", "Version.Details.props"),
                    Path.Combine(TempDirectory.Path, "eng", "Version.Details.props"));

                // Copy NuGet.config
                var nugetContent = File.ReadAllText(Path.Combine(root, "NuGet.config"))
                    // Point packages folder to vstest's local .packages cache used by test assets.
                    .Replace("</config>", $"""<add key="globalPackagesFolder" value="{IntegrationTestEnvironment.TestAssetsNuGetCacheDirectory}" /></config>""")
                    // and add local package source
                    .Replace("</packageSources>", $"""<add key="localy-built-packages" value="{IntegrationTestEnvironment.LocalPackageSource}" /></packageSources>""");
                File.WriteAllText(Path.Combine(TempDirectory.Path, "NuGet.config"), nugetContent);
            }
        }

        return Path.Combine(TempDirectory.Path, assetName);
    }
}

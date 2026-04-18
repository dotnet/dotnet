// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using NuGet.Versioning;

namespace Microsoft.TestPlatform.TestUtilities;

public class CompatibilityRowsBuilder
{
    private static XmlDocument? s_depsXml;
    private readonly string[] _runnerFrameworks;
    private readonly string[] _hostFrameworks;
    private readonly string[] _adapters;

    private readonly string[] _runnerVersions;
    private readonly string[] _hostVersions;
    private readonly string[] _adapterVersions;

    public CompatibilityRowsBuilder(string runnerVersions,
        string runnerFrameworks,
        string hostVersions,
        string hostFrameworks,
        string adapterVersions,
        string adapters)
    {
        _runnerFrameworks = runnerFrameworks.Split(';');
        _runnerVersions = runnerVersions.Split(';');
        _hostFrameworks = hostFrameworks.Split(';');
        _hostVersions = hostVersions.Split(';');
        _adapterVersions = adapterVersions.Split(';');
        _adapters = adapters.Split(';');
    }

    /// <summary>
    /// Add run for in-process using the selected .NET Framework runners, and all selected adapters.
    /// </summary>
    public bool WithInProcess { get; set; }
    // Add runner from VSIX to check the shipment we make into VisualStudio.
    public bool WithVSIXRunner { get; set; }

    public string? BeforeRunnerFeature { get; set; }
    public string? AfterRunnerFeature { get; set; }
    public string? BeforeTestHostFeature { get; set; }
    public string? AfterTestHostFeature { get; set; }
    public string? BeforeAdapterFeature { get; set; }
    public string? AfterAdapterFeature { get; set; }

    public bool DebugVSTestConsole { get; set; }
    public bool DebugTestHost { get; set; }
    public bool DebugDataCollector { get; set; }
    public bool DebugStopAtEntrypoint { get; set; }
    public int? JustRow { get; internal set; }

    public List<TestDataRow<RunnerInfo>> CreateData()
    {
        var dataRows = new List<RunnerInfo>();

        AddRows(dataRows);

        if (WithInProcess)
            AddInProcess(dataRows);

        if (WithVSIXRunner)
            AddVsix(dataRows, WithInProcess);

        var minVersion = ParseAndPatchSemanticVersion("0.0.0-alpha.1");
        var maxVersion = ParseAndPatchSemanticVersion("9999.0.0");
        SemanticVersion? beforeRunnerVersion = maxVersion;
        SemanticVersion? afterRunnerVersion = minVersion;
        SemanticVersion? beforeTestHostVersion = maxVersion;
        SemanticVersion? afterTestHostVersion = minVersion;
        SemanticVersion? beforeAdapterVersion = maxVersion;
        SemanticVersion? afterAdapterVersion = minVersion;

        if (BeforeRunnerFeature != null)
        {
            var feature = Features.TestPlatformFeatures[BeforeRunnerFeature];
            beforeRunnerVersion = ParseAndPatchSemanticVersion(feature.Version);
        }

        if (AfterRunnerFeature != null)
        {
            var feature = Features.TestPlatformFeatures[AfterRunnerFeature];
            afterRunnerVersion = ParseAndPatchSemanticVersion(feature.Version);
        }

        if (BeforeTestHostFeature != null)
        {
            var feature = Features.TestPlatformFeatures[BeforeTestHostFeature];
            beforeTestHostVersion = ParseAndPatchSemanticVersion(feature.Version);
        }

        if (AfterTestHostFeature != null)
        {
            var feature = Features.TestPlatformFeatures[AfterTestHostFeature];
            afterTestHostVersion = ParseAndPatchSemanticVersion(feature.Version);
        }

        if (BeforeAdapterFeature != null)
        {
            var feature = Features.AdapterFeatures[BeforeAdapterFeature];
            beforeAdapterVersion = ParseAndPatchSemanticVersion(feature.Version);
        }

        if (AfterAdapterFeature != null)
        {
            var feature = Features.AdapterFeatures[AfterAdapterFeature];
            afterAdapterVersion = ParseAndPatchSemanticVersion(feature.Version);
        }

        var isWindows = Environment.OSVersion.Platform.ToString().StartsWith("Win");
        // Run .NET Framework tests only on Windows.
        Func<string, bool> filter = tfm => isWindows || !tfm.StartsWith("net4");

        // TODO: maybe we should throw if we don't end up generating any data
        // because none of the versions match, or some other way to identify tests that will never run because they are very outdated.
        // We probably don't have that need right now, because legacy version is 15.x.x, which is very old, and we are still keeping
        // compatibility.

        Func<SemanticVersion, SemanticVersion, SemanticVersion, bool> isInRange = (version, before, after) => version < before && after <= version;

        var rows = dataRows.Where(r => r.VSTestConsoleInfo != null
            && isInRange(ParseAndPatchSemanticVersion(r.VSTestConsoleInfo.Version), beforeRunnerVersion, afterRunnerVersion)
            && r.TestHostInfo != null && isInRange(ParseAndPatchSemanticVersion(r.TestHostInfo.Version), beforeTestHostVersion, afterTestHostVersion)
            && r.AdapterInfo != null && isInRange(ParseAndPatchSemanticVersion(r.AdapterInfo.Version), beforeAdapterVersion, afterAdapterVersion)).ToList();

        // Figure out the distinct rows, and shrink the values, so if we have multiple rows that use the same versions and same setup, and only differ in how the version types
        // are called (e.g. Latest and LatestStable have the same version), then we get just single row, with description for both.
        // like RunMultipleTestAssemblies1 (Row: 0, Matrix, Runner = net10.0, TargetFramework = net481, InIsolation,  vstest.console = 18.6.0-dev [Latest],  Testhost = 18.6.0-dev [Latest],  MSTest = 3.9.3 [LatestPreview, LatestStable])
        var distinctRows = new Dictionary<string, RunnerInfo>();
        foreach (var r in rows)
        {
            var key = $"{r.Batch}|{r.RunnerFramework}|{r.VSTestConsoleInfo!.Version}|{r.TargetFramework}|{r.TestHostInfo!.Version}|{r.InIsolationValue}|{r.AdapterInfo!.Name}|{r.AdapterInfo.Version}";
            if (distinctRows.TryGetValue(key, out var value))
            {
                if (value.VSTestConsoleInfo!.Version == r.VSTestConsoleInfo.Version)
                {
                    value.VSTestConsoleInfo.VersionType += $",{r.VSTestConsoleInfo.VersionType}";
                }

                if (value.TestHostInfo!.Version == r.TestHostInfo.Version)
                {
                    value.TestHostInfo.VersionType += $",{r.TestHostInfo.VersionType}";
                }

                if (r.AdapterInfo!.Version == value.AdapterInfo!.Version)
                {
                    value.AdapterInfo.VersionType += $",{r.AdapterInfo.VersionType}";
                }
            }
            else
            {
                distinctRows.Add(key, r);
            }
        }

        // We added all the version types together (would have been better to have this property as array originally, but that would complicate other code), now deduplicate the names to make it less confusing for user.
        // Latest, Latest, Latest, LatestPreview -> Latest, LatestPreview
        foreach (var r in distinctRows.Values)
        {
            r.VSTestConsoleInfo!.VersionType = string.Join(", ", r.VSTestConsoleInfo!.VersionType!.Split(',').Distinct());
            r.TestHostInfo!.VersionType = string.Join(", ", r.TestHostInfo!.VersionType!.Split(',').Distinct());
            r.AdapterInfo!.VersionType = string.Join(", ", r.AdapterInfo!.VersionType!.Split(',').Distinct());
        }

        if (distinctRows.Count == 0)
        {
            throw new InvalidOperationException(
                $"No compatibility rows matched the specified criteria. "
                + $"Runner version range: [{afterRunnerVersion}, {beforeRunnerVersion}), "
                + $"TestHost version range: [{afterTestHostVersion}, {beforeTestHostVersion}), "
                + $"Adapter version range: [{afterAdapterVersion}, {beforeAdapterVersion}). "
                + $"Total candidate rows before filtering: {dataRows.Count}, after version filtering: {rows.Count}, after deduping: {distinctRows.Count}.");
        }

        var allRows = distinctRows.Values.ToList();
        for (var i = 0; i < allRows.Count; i++)
        {
            allRows[i].Index = i;
        }

        foreach (var r in allRows)
        {
            var hasLatest = r.VSTestConsoleInfo!.VersionType!.Split(',').Any(a => a.Trim() == "Latest")
                || r.TestHostInfo!.VersionType!.Split(',').Any(a => a.Trim() == "Latest")
                || r.AdapterInfo!.VersionType!.Split(',').Any(a => a.Trim() == "Latest");
            if (!hasLatest)
            {
                throw new InvalidOperationException($"Row {r.ToString()} does not have any version marked as Latest. You are testing only versions that were already shipped.");
            }
        }

        var selectedRows = JustRow == null ? allRows : [allRows[JustRow.Value]];
        return [.. selectedRows.Select(r => new TestDataRow<RunnerInfo>(r)
        {
            TestCategories = ["Compatibility"],
        })];
    }

    private static SemanticVersion ParseAndPatchSemanticVersion(string? version)
    {
        // Our developer version is 17.2.0-dev, but we release few preview, that are named 17.2.0-preview or 17.2.0-release, yet we still
        // want 17.2.0-dev to be considered the latest version. So we patch it.
        var v = version != null && version.EndsWith("-dev") ? version?.Substring(0, version.Length - 4) + "-ZZZZZZZZZZ" : version;
        return SemanticVersion.Parse(v?.TrimStart('v'));
    }

    private void AddRows(List<RunnerInfo> dataRows)
    {
        foreach (var runnerVersion in _runnerVersions)
        {
            foreach (var runnerFramework in _runnerFrameworks)
            {
                foreach (var hostFramework in _hostFrameworks)
                {
                    var isNetFramework = hostFramework.StartsWith("net4");

                    foreach (var hostVersion in _hostVersions)
                    {
                        if (isNetFramework && runnerVersion != hostVersion)
                        {
                            // For .NET Framework, runner and host versions must be the same,
                            // becase ship them in one package, and we will select the testhost from vstest.console package.
                            continue;
                        }

                        foreach (var adapterVersion in _adapterVersions)
                        {
                            foreach (var adapter in _adapters)
                            {
                                AddRow(dataRows, "Matrix", runnerVersion, runnerFramework, hostVersion, hostFramework, adapterVersion, adapter, inIsolation: true);
                            }
                        }
                    }
                }
            }
        }
    }

    private void AddInProcess(List<RunnerInfo> dataRows)
    {
        foreach (var runnerFramework in _runnerFrameworks)
        {
            if (!runnerFramework.StartsWith("net4"))
            {
                continue;
            }

            foreach (var runnerVersion in _runnerVersions)
            {
                foreach (var adapter in _adapters)
                {
                    foreach (var adapterVersion in _adapterVersions)
                    {
                        AddRow(dataRows, "In process", runnerVersion, runnerFramework, runnerVersion, runnerFramework, adapterVersion, adapter, inIsolation: false);
                    }
                }
            }
        }
    }

    private void AddVsix(List<RunnerInfo> dataRows, bool inProcess)
    {
        // Runs outside of process, test both tfms of testhost.
        foreach (var hostFramework in _hostFrameworks)
        {
            AddRow(dataRows, "VSIX", AcceptanceTestBase.LATESTVSIX, AcceptanceTestBase.RUNNER_NETFX, AcceptanceTestBase.LATEST, hostFramework, AcceptanceTestBase.LATESTSTABLE, AcceptanceTestBase.MSTEST, inIsolation: true);
        }

        if (inProcess)
        {
            // Runs in process. We specify the testhost, but it has no impact.
            AddRow(dataRows, "VSIX", AcceptanceTestBase.LATESTVSIX, AcceptanceTestBase.RUNNER_NETFX, AcceptanceTestBase.LATEST, AcceptanceTestBase.HOST_NETFX, AcceptanceTestBase.LATESTSTABLE, AcceptanceTestBase.MSTEST, inIsolation: false);
        }
    }

    private void AddRow(List<RunnerInfo> dataRows, string batch,
        string runnerVersion, string runnerFramework, string hostVersion, string hostFramework, string adapterVersion, string adapter, bool inIsolation)
    {
        if (adapter != AcceptanceTestBase.MSTEST)
        {
            throw new NotSupportedException($"Adapter {adapter} is not supported. Only {AcceptanceTestBase.MSTEST} is supported.");
        }

        RunnerInfo runnerInfo = GetRunnerInfo(batch, runnerFramework, hostFramework, inIsolation);
        runnerInfo.DebugInfo = GetDebugInfo();
        runnerInfo.VSTestConsoleInfo = GetVSTestConsoleInfo(runnerVersion, runnerInfo);
        runnerInfo.TestHostInfo = GetNetTestSdkInfo(hostVersion);
        runnerInfo.AdapterInfo = GetMSTestInfo(adapterVersion);
        dataRows.Add(runnerInfo);
    }

    private DebugInfo GetDebugInfo()
    {
        return new DebugInfo
        {
            DebugDataCollector = DebugDataCollector,
            DebugTestHost = DebugTestHost,
            DebugVSTestConsole = DebugVSTestConsole,
            DebugStopAtEntrypoint = DebugStopAtEntrypoint
        };
    }

    private static RunnerInfo GetRunnerInfo(string batch, string runnerFramework, string hostFramework, bool inIsolation)
    {
        return new RunnerInfo
        {
            Batch = batch,
            RunnerFramework = runnerFramework,
            TargetFramework = hostFramework,
            InIsolationValue = inIsolation ? AcceptanceTestBase.InIsolation : null
        };
    }

    private static DllInfo GetMSTestInfo(string msTestVersion)
    {
        var depsXml = GetDependenciesXml();

        // It is okay when node is null, we check that Version has value when we update paths by using MSTestInfo, and throw.
        // This way it throws in the body of the test which has better error reporting than throwing in the data source.
        XmlNode? node = depsXml.DocumentElement?.SelectSingleNode($"PropertyGroup/MSTestFramework{msTestVersion}Version");
        var version = node?.InnerText.Replace("[", "").Replace("]", "");
        var versionSpecificPath = $"MSTest{msTestVersion}-{version}";

        return new DllInfo
        {
            Name = "MSTest",
            PropertyName = "MSTest",
            VersionType = msTestVersion,
            Version = version,
            Path = versionSpecificPath,
        };
    }

    private static VSTestConsoleInfo GetVSTestConsoleInfo(string vstestConsoleVersion, RunnerInfo runnerInfo)
    {
        if (vstestConsoleVersion == AcceptanceTestBase.LATESTVSIX)
        {
            return new VSTestConsoleInfo
            {
                VersionType = vstestConsoleVersion,
                Version = IntegrationTestEnvironment.LatestLocallyBuiltNugetVersion,
                Path = Path.Combine(IntegrationTestEnvironment.PublishDirectory, Path.GetFileName(IntegrationTestEnvironment.LocalVsixInsertion), "vstest.console.exe"),
            };
        }

        var depsXml = GetDependenciesXml();
        var packageName = runnerInfo.IsNetFrameworkRunner
            ? "microsoft.testplatform"
            : "microsoft.testplatform.cli";

        string version;

        // When version is Latest, we built it locally, but it gets restored into our nuget cache on build.
        if (vstestConsoleVersion == AcceptanceTestBase.LATEST)
        {
            version = IntegrationTestEnvironment.LatestLocallyBuiltNugetVersion;
        }
        else
        {
            var propertyName = $"VSTestConsole{vstestConsoleVersion}Version";

            // It is okay when node is null, we will fail to find the executable later, and throw.
            // This way it throws in the body of the test which has better error reporting than throwing in the data source.
            // And we can easily find out what is going on because --WRONG-VERSION-- sticks out, and is easy to find in the codebase.
            XmlNode? node = depsXml.DocumentElement?.SelectSingleNode($"PropertyGroup/{propertyName}");
            version = node?.InnerText.Replace("[", "").Replace("]", "") ?? "--WRONG-VERSION--";
        }

        // Target frameworks changed in the package over time as we are moving forward, this table selects the correct one.
        var vstestConsolePath = runnerInfo.IsNetFrameworkRunner switch
        {
            true when NuGetVersion.TryParse(version, out var v)
                && new NuGetVersion(v.Major, v.Minor, v.Patch) < new NuGetVersion("17.3.0") => GetToolsPath("net451"),
            true => GetToolsPath("net462"),
            false when version.StartsWith("15.") => GetContentFilesPath("netcoreapp2.0"),
            false when NuGetVersion.TryParse(version, out var v)
                && new NuGetVersion(v.Major, v.Minor, v.Patch) < new NuGetVersion("17.4.0") => GetContentFilesPath("netcoreapp2.1"),
            false when NuGetVersion.TryParse(version, out var v)
                && new NuGetVersion(v.Major, v.Minor, v.Patch) <= new NuGetVersion("17.12.0") => GetContentFilesPath("netcoreapp3.1"),
            false when NuGetVersion.TryParse(version, out var v)
                && new NuGetVersion(v.Major, v.Minor, v.Patch) <= new NuGetVersion("18.3.0") => GetContentFilesPath("net9.0"),
            false => GetContentFilesPath("net10.0"),
        };

        return new VSTestConsoleInfo
        {
            VersionType = vstestConsoleVersion,
            Version = version,
            Path = vstestConsolePath,
        };

        string GetToolsPath(string fwkVersion) => Path.Combine(IntegrationTestEnvironment.TestAssetsNuGetCacheDirectory,
            packageName, version, "tools", fwkVersion, "Common7", "IDE", "Extensions", "TestPlatform", "vstest.console.exe");

        string GetContentFilesPath(string fwkVersion) => Path.Combine(IntegrationTestEnvironment.TestAssetsNuGetCacheDirectory,
            packageName, version, "contentFiles", "any", fwkVersion, "vstest.console.dll");
    }

    private static NetTestSdkInfo GetNetTestSdkInfo(string testhostVersionType)
    {
        var depsXml = GetDependenciesXml();

        string version;
        if (testhostVersionType == AcceptanceTestBase.LATEST)
        {
            version = IntegrationTestEnvironment.LatestLocallyBuiltNugetVersion;
        }
        else
        {
            var propertyName = $"VSTestConsole{testhostVersionType}Version";

            // It is okay when node is null, we check that Version has value when we update paths by using TesthostInfo, and throw.
            // This way it throws in the body of the test which has better error reporting than throwing in the data source.
            //
            // We use the VSTestConsole properties to figure out testhost version, for now.
            XmlNode? node = depsXml.DocumentElement?.SelectSingleNode($"PropertyGroup/{propertyName}");
            version = node?.InnerText.Replace("[", "").Replace("]", "")!;
        }

        var versionSpecificPath = $"NETTestSdk{testhostVersionType}-{version}";

        return new NetTestSdkInfo
        {
            VersionType = testhostVersionType,
            Version = version,
            Path = versionSpecificPath
        };
    }

    private static XmlDocument GetDependenciesXml()
    {
        if (s_depsXml != null)
            return s_depsXml;

        var depsXmlPath = Path.Combine(IntegrationTestEnvironment.RepoRootDirectory, "eng", "Versions.props");
        var fileStream = File.OpenRead(depsXmlPath);
        var xmlTextReader = new XmlTextReader(fileStream) { Namespaces = false };
        var depsXml = new XmlDocument();
        depsXml.Load(xmlTextReader);

        s_depsXml = depsXml;
        return depsXml;
    }
}


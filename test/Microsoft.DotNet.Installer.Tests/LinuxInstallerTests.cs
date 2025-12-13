// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.Build.Tasks.Installers;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using TestUtilities;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Installer.Tests;

public partial class LinuxInstallerTests : IDisposable
{
    private readonly DockerHelper _dockerHelper;
    private readonly string _tmpDir;
    private readonly string _contextDir;
    private readonly ITestOutputHelper _outputHelper;
    private readonly string _excludeLinuxArch;

    private bool _rpmContextInitialized = false;
    private bool _debContextInitialized = false;
    private bool _sharedContextInitialized = false;

    private readonly Dictionary<string, List<string>> _expectedPackageDependencies = new()
    {
        { DotnetHostPrefix, new List<string> { } },
        { DotnetHostFxrPrefix, new List<string> { $"{DotnetHostPrefix.TrimEnd('-')}" } },
        { DotnetRuntimePrefix, new List<string>
            {
                $"{DotnetHostFxrPrefix}{Config.TargetProductVersion}",
                $"{DotnetRuntimeDepsPrefix}{Config.TargetProductVersion}"
            }
        },
        { DotnetTargetingPackPrefix, new List<string> {  } },
        { AspNetCoreRuntimePrefix, new List<string> { $"{DotnetRuntimePrefix}{Config.TargetProductVersion}" } },
        { AspNetCoreTargetingPackPrefix, new List<string> { $"{DotnetTargetingPackPrefix}{Config.TargetProductVersion}" } },
        { DotnetApphostPackPrefix, new List<string> { } },
        { DotnetSdkPrefix, new List<string>
            {
                $"{DotnetRuntimePrefix}{Config.TargetProductVersion}",
                $"{DotnetTargetingPackPrefix}{Config.TargetProductVersion}",
                $"{DotnetApphostPackPrefix}{Config.TargetProductVersion}",
                $"{AspNetCoreRuntimePrefix}{Config.TargetProductVersion}",
                $"{AspNetCoreTargetingPackPrefix}{Config.TargetProductVersion}"
            }
        }
    };

    // Extract the package prefix from the package name
    // e.g., dotnet-runtime-10.0.0-rc.2.25418.119-x64 -> dotnet-runtime-
    [GeneratedRegex(@"^(.*?-)(?=\d)")]
    private static partial Regex PackagePrefixFromPackageNameRegex { get; }

    // Use multiline + case-insensitive to find the Depends line (same line only)
    // Captures everything after "Depends:" up to the end of that line
    [GeneratedRegex(@"(?im)^[ \t]*Depends:\s*(.+)$")]
    private static partial Regex DependsLineRegex { get; }

    // Remove any version constraint in parentheses: "package (>= 1.0)" -> "package"
    // Handles any interior text until the matching closing parenthesis on that token.
    [GeneratedRegex(@"\s*\([^)]*\)", RegexOptions.CultureInvariant)]
    private static partial Regex RemoveVersionConstraintRegex { get; }

    // Remove version numbers from package names: "dotnet-runtime-10.0.0-rc.1.25480.112-x64.rpm" -> "dotnet-runtime-*-x64.rpm"
    [GeneratedRegex(@"\d+\.\d+\.\d+(?:-(?:alpha|rc|rtm|preview)(?:\.\d+)*)?", RegexOptions.CultureInvariant)]
    private static partial Regex RemoveVersionFromPackageNameRegex { get; }

    private const string RuntimeDepsRepo = "mcr.microsoft.com/dotnet/runtime-deps";
    private const string RuntimeDepsVersion = "10.0";
    private const string DotnetRuntimeDepsPrefix = "dotnet-runtime-deps-";
    private const string DotnetHostPrefix = "dotnet-host-";
    private const string DotnetHostFxrPrefix = "dotnet-hostfxr-";
    private const string DotnetRuntimePrefix = "dotnet-runtime-";
    private const string DotnetTargetingPackPrefix = "dotnet-targeting-pack-";
    private const string AspNetCoreRuntimePrefix = "aspnetcore-runtime-";
    private const string AspNetCoreTargetingPackPrefix = "aspnetcore-targeting-pack-";
    private const string DotnetApphostPackPrefix = "dotnet-apphost-pack-";
    private const string DotnetSdkPrefix = "dotnet-sdk-";
    private const string DowngradeFxVersionsScript = "downgrade-fx-versions.sh";

    public static bool IncludeRpmTests => Config.TestRpmPackages;
    public static bool IncludeDebTests => Config.TestDebPackages;

    private enum PackageType
    {
        Rpm,
        Deb
    }

    public LinuxInstallerTests(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
        _dockerHelper = new DockerHelper(_outputHelper);

        _tmpDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(_tmpDir);
        _contextDir = Path.Combine(_tmpDir, Path.GetRandomFileName());
        Directory.CreateDirectory(_contextDir);

        _excludeLinuxArch = Config.Architecture == Architecture.X64 ?
                                                   Architecture.Arm64.ToString().ToLower() :
                                                   Architecture.X64.ToString().ToLower();
    }

    public void Dispose()
    {
        try
        {
            Directory.Delete(_tmpDir, recursive: true);
        }
        catch
        {
        }
    }

    [ConditionalTheory(typeof(LinuxInstallerTests), nameof(IncludeRpmTests))]
    [InlineData(RuntimeDepsRepo, $"{RuntimeDepsVersion}-azurelinux3.0")]
    public async Task RpmScenarioTest(string repo, string tag)
    {
        if (!tag.Contains("azurelinux"))
        {
            // Only Azure Linux is currently supported for RPM tests
            Assert.Fail("Only Azure Linux is currently supported for RPM tests");
        }

        await InitializeContextAsync(PackageType.Rpm);

        DistroTest($"{repo}:{tag}", PackageType.Rpm);
    }

    [ConditionalTheory(typeof(LinuxInstallerTests), nameof(IncludeDebTests))]
    [InlineData(RuntimeDepsRepo, $"{RuntimeDepsVersion}-noble")]
    public async Task DebScenarioTest(string repo, string tag)
    {
        await InitializeContextAsync(PackageType.Deb);

        DistroTest($"{repo}:{tag}", PackageType.Deb);
    }

    [ConditionalTheory(typeof(LinuxInstallerTests), nameof(IncludeRpmTests))]
    [InlineData(RuntimeDepsRepo, $"{RuntimeDepsVersion}-azurelinux3.0")]
    public async Task RpmPackageMetadataTest(string repo, string tag)
    {
        await InitializeContextAsync(PackageType.Rpm, initializeSharedContext: false);

        ValidatePackageMetadata($"{repo}:{tag}", PackageType.Rpm);
    }

    [ConditionalTheory(typeof(LinuxInstallerTests), nameof(IncludeDebTests))]
    [InlineData(RuntimeDepsRepo, $"{RuntimeDepsVersion}-noble")]
    public async Task DebPackageMetadataTest(string repo, string tag)
    {
        await InitializeContextAsync(PackageType.Deb, initializeSharedContext: false);

        ValidatePackageMetadata($"{repo}:{tag}", PackageType.Deb);
    }

    [ConditionalFact(typeof(LinuxInstallerTests), nameof(IncludeRpmTests))]
    public void ValidateRpmPackageList()
    {
        ValidatePackageList(PackageType.Rpm);
    }

    [ConditionalFact(typeof(LinuxInstallerTests), nameof(IncludeDebTests))]
    public void ValidateDebPackageList()
    {
        ValidatePackageList(PackageType.Deb);
    }

    private async Task InitializeContextAsync(PackageType packageType, bool initializeSharedContext = true)
    {
        string packageArchitecture =
            Config.Architecture == Architecture.X64 ?
                "x64" :
                packageType == PackageType.Rpm ?
                    "aarch64" :
                    "arm64";

        if (packageType == PackageType.Rpm && !_rpmContextInitialized)
        {
            // Copy all applicable RPM packages, excluding Mariner and Azure Linux copies
            List<string> rpmPackages =
                Directory.GetFiles(Config.AssetsDirectory, $"*-{packageArchitecture}*.rpm", SearchOption.AllDirectories)
                .Where(p => !Path.GetFileName(p).Contains("-cm.") &&
                            !Path.GetFileName(p).Contains("-azl-") &&
                            !Path.GetFileName(p).EndsWith("azl.rpm"))
                .ToList();

            foreach (string rpmPackage in rpmPackages)
            {
                File.Copy(rpmPackage, Path.Combine(_contextDir, Path.GetFileName(rpmPackage)));
            }

            await DownloadPackagesAsync(packageArchitecture, packageType);
            _rpmContextInitialized = true;
        }
        else if (!_debContextInitialized)
        {
            // Copy all applicable DEB packages
            foreach (string debPackage in Directory.GetFiles(Config.AssetsDirectory, $"*-{packageArchitecture}*.deb", SearchOption.AllDirectories))
            {
                File.Copy(debPackage, Path.Combine(_contextDir, Path.GetFileName(debPackage)));
            }

            await DownloadPackagesAsync(packageArchitecture, packageType);
            _debContextInitialized = true;
        }

        // Some tests do not need shared context
        if (initializeSharedContext && !_sharedContextInitialized)
        {
            // Copy nuget packages
            string nugetPackagesDir = Path.Combine(_contextDir, "packages");
            Directory.CreateDirectory(nugetPackagesDir);
            foreach (string package in Directory.GetFiles(Config.PackagesDirectory, "*.nupkg", SearchOption.AllDirectories))
            {
                if (ShouldCopyPackage(package.ToLower()))
                {
                    File.Copy(package, Path.Combine(nugetPackagesDir, Path.GetFileName(package)));
                }
            }

            // Copy and update NuGet.config from scenario-tests repo
            string newNuGetConfig = Path.Combine(_contextDir, "NuGet.config");
            File.Copy(Config.ScenarioTestsNuGetConfigPath, newNuGetConfig);
            InsertLocalPackagesPathToNuGetConfig(newNuGetConfig, "/packages");

            // Copy downgrade-fx-versions.sh script
            // This script is used to update the latest known 8.0 and 9.0 framework versions in SDK's
            // Microsoft.NETCoreSdk.BundledVersions.props file to the versions 2 releases prior.
            // This is needed as the SDK automatically picks up servicing versions not yet released, which
            // is either one or two versions higher than publicly available versions, depending on
            // when we run the tests.
            string downgradeScript = Path.Combine(_contextDir, DowngradeFxVersionsScript);
            File.Copy(Path.Combine(GetAssetsDirectory(), DowngradeFxVersionsScript), downgradeScript);

            // Find the scenario-tests package and unpack it to the context dir, subfolder "scenario-tests"
            string? scenarioTestsPackage = Directory.GetFiles(nugetPackagesDir, "Microsoft.DotNet.ScenarioTests.SdkTemplateTests*.nupkg", SearchOption.AllDirectories).FirstOrDefault();
            if (scenarioTestsPackage == null)
            {
                Assert.Fail("Scenario tests package not found");
            }

            ZipFile.ExtractToDirectory(scenarioTestsPackage, Path.Combine(_contextDir, "scenario-tests"));
            _sharedContextInitialized = true;
        }
    }

    private async Task DownloadPackagesAsync(string packageArchitecture, PackageType packageType)
    {
        // Collect URLs and file names for downloading
        var downloadsToProcess = new List<(Uri url, string fileName)>();
        
        // Since this is for a non-1xx branch, we never produced runtime packages. Download these from
        // the referenced 1xx build instead.
        if (!Config.DotNetBuildSharedComponents)
        {
            string distroLabel = packageType == PackageType.Rpm ? "-azl.3" : "";
            AddRuntimePackageForDownload(downloadsToProcess, DotnetRuntimeDepsPrefix, packageArchitecture, packageType, distroLabel: distroLabel);

            string[] runtimePackagePrefixes =
            [
                DotnetHostPrefix,
                DotnetHostFxrPrefix,
                DotnetRuntimePrefix,
                DotnetTargetingPackPrefix,
                DotnetApphostPackPrefix
            ];

            foreach (string prefix in runtimePackagePrefixes)
            {
                AddRuntimePackageForDownload(downloadsToProcess, prefix, packageArchitecture, packageType);
            }

            string[] aspnetcorePackagePrefixes = 
            [
                AspNetCoreRuntimePrefix,
                AspNetCoreTargetingPackPrefix
            ];

            foreach (string prefix in aspnetcorePackagePrefixes)
            {
                AddRuntimePackageForDownload(downloadsToProcess, prefix, packageArchitecture, packageType, "aspnetcore/Runtime");
            }
        }

        // Download all collected files.
        // These are small files. No need to parallelize. The logging of sequential downloads will be easier to follow.
        foreach (var (url, fileName) in downloadsToProcess)
        {
            await DownloadFileAsync(url, Path.Combine(_contextDir, fileName));
        }
    }

    private static void AddRuntimePackageForDownload(
        List<(Uri url, string fileName)> downloadsToProcess,
        string packagePrefix,
        string architecture,
        PackageType packageType,
        string runtimeLocation = "Runtime",
        string distroLabel = "")
    {
        Uri packageUrl = new Uri($"https://ci.dot.net/public/{runtimeLocation}/{Config.MicrosoftNETCorePlatformsVersion1xx}/{packagePrefix}{Config.MicrosoftNETCoreAppRefVersion1xx}{distroLabel}-{architecture}.{packageType.ToString().ToLower()}");
        downloadsToProcess.Add((packageUrl, packageUrl.Segments.Last()));
    }

    private bool ShouldCopyPackage(string package)
    {
        if (package.Contains(".osx-") ||
            package.Contains(".win-") ||
            package.Contains(".linux-musl-") ||
            package.Contains(".linux-bionic-") ||
            package.Contains(".mono.") ||
            package.Contains("symbols") ||
            package.Contains("vs.redist") ||
            package.Contains(".linux-arm.") ||
            package.Contains($".linux-{_excludeLinuxArch}."))
        {
            return false;
        }

        return true;
    }

    private void InsertLocalPackagesPathToNuGetConfig(string nuGetConfig, string localPackagesPath)
    {
        XDocument doc = XDocument.Load(nuGetConfig);
        if (doc.Root != null)
        {
            XElement? packageSourcesElement = doc.Root.Element("packageSources");
            if (packageSourcesElement != null)
            {
                XElement? clearElement = packageSourcesElement.Element("clear");
                if (clearElement != null)
                {
                    XElement newAddElement = new XElement("add",
                        new XAttribute("key", "local-packages"),
                        new XAttribute("value", localPackagesPath));

                    clearElement.AddAfterSelf(newAddElement);
                }
            }

            doc.Save(nuGetConfig);
        }
    }

    private void DistroTest(string baseImage, PackageType packageType)
    {
        List<string> packageList = GetPackageList(baseImage, packageType);
        string dockerfile = GenerateDockerfile(packageList, baseImage, packageType);

        // Define the host and container paths for the log file
        string hostLogDir = Path.Combine(Config.ArtifactsTestResultsDirectory, "scenario-tests");
        Directory.CreateDirectory(hostLogDir);
        string containerLogDir = "/logs";
        string containerLogPath = Path.Combine(containerLogDir, $"scenario-tests-{GetSanitizedImageName(baseImage)}.xml");

        string testCommand = $"dotnet {GetScenarioTestsBinaryPath()} --dotnet-root /usr/share/dotnet/ --xml {containerLogPath} --no-traits Category=RequiresNonTargetRidPackages";

        string tag = $"test-{Path.GetRandomFileName()}";
        string output = "";
        bool buildCompleted = false;

        try
        {
            // Build docker image and run the tests
            _dockerHelper.Build(tag, dockerfile: dockerfile, contextDir: _contextDir);
            buildCompleted = true;

            // Mount the host log directory to the container
            string optionalRunArgs = $"-v {hostLogDir}:{containerLogDir}";
            output = _dockerHelper.Run(tag, tag, testCommand, optionalRunArgs: optionalRunArgs);

            int testResultsSummaryIndex = output.IndexOf("Tests run: ");
            if (testResultsSummaryIndex >= 0)
            {
                string testResultsSummary = output[testResultsSummaryIndex..];
                Assert.False(AnyTestFailures(testResultsSummary), testResultsSummary);
            }
            else
            {
                Assert.Fail("Test summary not found");
            }
        }
        catch (Exception e)
        {
            if (string.IsNullOrEmpty(output))
            {
                output = e.Message;
            }
            Assert.Fail($"{(buildCompleted ? "Build" : "Test")} failed: {output}");
        }
        finally
        {
            if (!Config.KeepDockerImages)
            {
                _dockerHelper.DeleteImage(tag);
            }
        }
    }

    private string GetScenarioTestsBinaryPath()
    {
        // Find scenario-tests binary in context/scenario-tests
        string? scenarioTestsBinary = Directory.GetFiles(Path.Combine(_contextDir, "scenario-tests"), "Microsoft.DotNet.ScenarioTests.SdkTemplateTests.dll", SearchOption.AllDirectories).FirstOrDefault();
        if (scenarioTestsBinary == null)
        {
            throw new Exception("Scenario tests binary not found");
        }

        return scenarioTestsBinary.Replace(_contextDir, "").Replace("\\", "/");
    }

    private List<string> GetPackageList(string baseImage, PackageType packageType)
    {
        // Order of installation is important as we do not want to use "--nodeps"
        // We install in correct order, so package dependencies are present.

        // Prepare the package list in correct install order
        List<string> packageList =
        [
            // Deps package should be installed first
            Path.GetFileName(GetMatchingDepsPackage(baseImage, packageType))
        ];

        // Add all other packages in correct install order
        AddPackage(packageList, DotnetHostPrefix, packageType);
        AddPackage(packageList, DotnetHostFxrPrefix, packageType);
        AddPackage(packageList, DotnetRuntimePrefix, packageType);
        AddPackage(packageList, DotnetTargetingPackPrefix, packageType);
        AddPackage(packageList, AspNetCoreRuntimePrefix, packageType);
        AddPackage(packageList, AspNetCoreTargetingPackPrefix, packageType);
        AddPackage(packageList, DotnetApphostPackPrefix, packageType);
        AddPackage(packageList, DotnetSdkPrefix, packageType);

        return packageList;
    }

    private string GenerateDockerfile(List<string> packageList, string baseImage, PackageType packageType)
    {
        StringBuilder sb = new();
        sb.AppendLine("FROM " + baseImage);
        sb.AppendLine("");
        sb.AppendLine("# Copy NuGet.config");
        sb.AppendLine($"COPY NuGet.config .");

        sb.AppendLine("");
        sb.AppendLine($"# Copy {DowngradeFxVersionsScript}");
        sb.AppendLine($"COPY {DowngradeFxVersionsScript} .");

        sb.AppendLine("");
        sb.AppendLine("# Copy scenario-tests content");
        sb.AppendLine($"COPY scenario-tests scenario-tests");

        sb.AppendLine("");
        sb.AppendLine("# Copy nuget packages");
        sb.AppendLine($"COPY packages packages");

        sb.AppendLine("");
        sb.AppendLine("# Copy installer packages");
        foreach (string package in packageList)
        {
            sb.AppendLine($"COPY {package} {package}");
        }
        sb.AppendLine("");
        sb.AppendLine("# Install the installer packages and Microsoft.DotNet.ScenarioTests.SdkTemplateTests tool");
        sb.Append("RUN");

        string packageInstallationCommand = packageType == PackageType.Deb ? "dpkg -i" : "rpm -i";
        bool useAndOperator = false;
        foreach (string package in packageList)
        {
            string options = "";
            // TODO: remove --force-depends after deps image issue has been resolved - https://github.com/dotnet/dotnet-docker/issues/6271
            if (packageType == PackageType.Deb &&
                package.Contains(DotnetRuntimeDepsPrefix))
            {
                options = " --force-depends";
            }

            sb.AppendLine(" \\");
            sb.Append($"    {(useAndOperator ? "&&" : "")} {packageInstallationCommand}{options} {package}");
            useAndOperator = true;
        }
        sb.AppendLine("");

        sb.AppendLine("");
        sb.AppendLine("# Run the script to downgrade 8.0 and 9.0 framework versions");
        sb.AppendLine("RUN \\");
        sb.AppendLine($"    chmod +x {DowngradeFxVersionsScript} && \\");
        sb.AppendLine($"    ./{DowngradeFxVersionsScript}");

        // Set environment for nuget.config
        sb.AppendLine("");
        sb.AppendLine("# Set custom nuget.config");
        sb.AppendLine("ENV RestoreConfigFile=/NuGet.config");

        string dockerfile = Path.Combine(_contextDir, $"Dockerfile-{Path.GetRandomFileName()}");
        File.WriteAllText(dockerfile, sb.ToString());
        return dockerfile;
    }

    private bool AnyTestFailures(string testResultSummary)
    {
        var parts = testResultSummary.Split(',')
            .Select(part => part.Split(':').Select(p => p.Trim()).ToArray())
            .Where(p => p.Length == 2)
            .ToDictionary(p => p[0], p => int.Parse(p[1]));

        return parts["Errors"] > 0 || parts["Failures"] > 0;
    }

    private void AddPackage(List<string> packageList, string prefix, PackageType packageType)
    {
        packageList.Add(Path.GetFileName(GetContentPackage(prefix, packageType)));
    }

    private string GetContentPackage(string prefix, PackageType packageType)
    {
        string matchPattern = PackageType.Deb == packageType ? "*.deb" : "*.rpm";
        string[] files = Directory.GetFiles(_contextDir, prefix + matchPattern, SearchOption.AllDirectories)
            .Where(p => !Path.GetFileName(p).Contains(DotnetRuntimeDepsPrefix))
            .ToArray();
        if (files.Length == 0)
        {
            throw new Exception($"RPM package with prefix '{prefix}' not found");
        }

        return files.OrderByDescending(f => f).First();
    }

    private string GetMatchingDepsPackage(string baseImage, PackageType packageType)
    {
        string matchPattern = packageType == PackageType.Deb
            ? $"{DotnetRuntimeDepsPrefix}*.deb"
            : $"{DotnetRuntimeDepsPrefix}*azl*.rpm"; // We currently only support Azure Linux deps image

        string[] files = Directory.GetFiles(_contextDir, matchPattern, SearchOption.AllDirectories);
        if (files.Length == 0)
        {
            throw new Exception($"Did not find the DEPS package.");
        }

        return files.OrderByDescending(f => f).First();
    }

    private static string GetSanitizedImageName(string image) =>
        image.Replace("/", "_").Replace(":", "_").Replace(".", "_");

    private async Task DownloadFileAsync(Uri url, string filePath)
    {
        _outputHelper.WriteLine($"Downloading {url} to {filePath}");

        using HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        using FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
        await response.Content.CopyToAsync(fileStream);
    }

    private void ValidatePackageMetadata(string image, PackageType packageType)
    {
        List<string> list = GetPackageList(image, packageType);
        ValidatePackageDependencies(list, packageType);
    }

    private void ValidatePackageDependencies(List<string> list, PackageType packageType)
    {
        foreach (string package in list)
        {
            // Skip runtime-deps packages as they are not expected to have .NET dependencies
            if (package.StartsWith(DotnetRuntimeDepsPrefix))
            {
                continue;
            }

            EnsurePackageContainsExpectedDependencies(package, packageType);
        }
    }

    private void EnsurePackageContainsExpectedDependencies(string package, PackageType packageType)
    {
        List<string> dependencies = GetPackageDependencies(package, packageType);

        string packagePrefix = GetPackagePrefixFromPackageName(package);
        List<string> expectedDependencies = _expectedPackageDependencies.ContainsKey(packagePrefix)
            ? _expectedPackageDependencies[packagePrefix]
            : [];

        Assert.Equal(expectedDependencies.OrderBy(x => x), dependencies.OrderBy(x => x));
    }

    private string GetPackagePrefixFromPackageName(string packageName)
    {
        Match match = PackagePrefixFromPackageNameRegex.Match(packageName);
        if (match.Success)
        {
            return match.Value;
        }

        Assert.Fail($"Could not extract package prefix from package name: {packageName}");
        return string.Empty;
    }

    private List<string> GetPackageDependencies(string package, PackageType packageType)
    {
        string packagePath = Path.Combine(_contextDir, package);
        if (!File.Exists(packagePath))
        {
            _outputHelper.WriteLine($"Package file not found: {packagePath}");
            return [];
        }

        if (packageType == PackageType.Deb)
        {
            return GetDebianPackageDependencies(packagePath);
        }
        else if (packageType == PackageType.Rpm)
        {
            return GetRpmPackageDependencies(packagePath);
        }

        return [];
    }

    private List<string> GetRpmPackageDependencies(string packagePath)
    {
        try
        {
            using FileStream rpmStream = File.OpenRead(packagePath);
            using RpmPackage rpmPackage = RpmPackage.Read(rpmStream);

            string[] requireNames = (string[])rpmPackage.Header.Entries.FirstOrDefault(e => e.Tag == RpmHeaderTag.RequireName).Value;
            if (requireNames == null || requireNames.Length == 0)
            {
                return [];
            }

            return requireNames
                .Where(name => !string.IsNullOrWhiteSpace(name) && !name.StartsWith("rpmlib(", StringComparison.Ordinal))
                .Distinct(StringComparer.Ordinal)
                .ToList();
        }
        catch (Exception ex)
        {
            _outputHelper.WriteLine($"Error parsing RPM package '{packagePath}': {ex}");
            return [];
        }
    }

    private List<string> GetDebianPackageDependencies(string packagePath)
    {
        try
        {
            using FileStream debStream = File.OpenRead(packagePath);
            using ArReader ar = new ArReader(debStream, false);

            // Find control.tar.* entry
            while (ar.GetNextEntry() is ArEntry arEentry)
            {
                if (!arEentry.Name.StartsWith("control.tar", StringComparison.Ordinal))
                {
                    continue;
                }

                using MemoryStream controlArchiveData = new MemoryStream((int)arEentry.DataStream.Length);
                arEentry.DataStream?.CopyTo(controlArchiveData);
                controlArchiveData.Position = 0;

                Stream decompressed =
                    arEentry.Name.EndsWith(".gz", StringComparison.OrdinalIgnoreCase) ?
                        new GZipStream(controlArchiveData, CompressionMode.Decompress, leaveOpen: false) :
                    // Future compressors can be added here (xz, zst). Fallback: treat as plain.
                        controlArchiveData;

                using (decompressed)
                {
                    // Read tar entries to find "control" file
                    using TarReader tarReader = new TarReader(decompressed, leaveOpen: false);
                    TarEntry? entry;
                    while ((entry = tarReader.GetNextEntry()) is not null)
                    {
                        if (entry.Name
                            .TrimStart('.', '/')
                            .Equals("control", StringComparison.Ordinal))
                        {
                            using MemoryStream controlFileData = new MemoryStream();
                            entry.DataStream?.CopyTo(controlFileData);
                            string controlContent = Encoding.UTF8.GetString(controlFileData.ToArray());
                            File.WriteAllText(Path.Combine(Path.GetTempPath(), "control.txt"), controlContent);
                            return ParseDebControlDependencies(controlContent);
                        }
                    }
                }
            }

            _outputHelper.WriteLine($"No control.tar.* entry found in {packagePath}");
            return [];
        }
        catch (Exception ex)
        {
            _outputHelper.WriteLine($"Error parsing DEB package '{packagePath}': {ex}");
            return [];
        }
    }

    private static List<string> ParseDebControlDependencies(string contents)
    {
        Match match = DependsLineRegex.Match(contents);
        if (!match.Success)
        {
            return [];
        }

        string dependsLine = match.Groups[1].Value.Trim();
        if (dependsLine.Length == 0)
        {
            return [];
        }

        var results = new List<string>();

        foreach (string segment in dependsLine.Split(','))
        {
            string part = segment.Trim();
            if (part.Length == 0)
            {
                continue;
            }

            // If there are alternates (pkgA | pkgB), keep only the first one as the dependency
            int pipeIndex = part.IndexOf('|');
            if (pipeIndex >= 0)
            {
                part = part.Substring(0, pipeIndex).Trim();
            }

            part = RemoveVersionConstraintRegex.Replace(part, "").Trim();

            // Skip native lib packages
            if (part.Length > 0 && !part.StartsWith("lib"))
            {
                results.Add(part);
            }
        }

        return results;
    }

    private void ValidatePackageList(PackageType packageType)
    {
        string extension = packageType == PackageType.Rpm ? "*.rpm" : "*.deb";
        List<string> expectedPatterns = GetExpectedPackagePatterns(packageType).OrderBy(p => p).ToList();

        // Find all packages of the specified type and normalize by removing version numbers
        List<string> normalizedActual = Directory.GetFiles(Config.AssetsDirectory, extension, SearchOption.AllDirectories)
            .Select(path => RemoveVersionFromPackageNameRegex.Replace(Path.GetFileName(path), "*"))
            .Distinct()
            .OrderBy(name => name)
            .ToList();

        Assert.True(
            expectedPatterns.SequenceEqual(normalizedActual),
            $"Package list validation failed for {packageType}:\nExpected:\n{string.Join("\n", expectedPatterns)}\nActual:\n{string.Join("\n", normalizedActual)}"
        );
    }

    private List<string> GetExpectedPackagePatterns(PackageType packageType)
    {
        string extension = packageType == PackageType.Rpm ? ".rpm" : ".deb";
        string arch = Config.Architecture == Architecture.X64 ? "x64" :
                     (packageType == PackageType.Rpm ? "aarch64" : "arm64");

        var patterns = new List<string>();

        List<string> basePackages;

        if (Config.DotNetBuildSharedComponents)
        {
            // Base package prefixes (common to both RPM and DEB)
            basePackages =
            [
                "aspnetcore-runtime", "aspnetcore-targeting-pack", "dotnet-apphost-pack",
                "dotnet-host", "dotnet-hostfxr", "dotnet-runtime", "dotnet-sdk", "dotnet-targeting-pack"
            ];

            // Add runtime-deps for DEB only (RPM only has distro-specific variants)
            if (packageType == PackageType.Deb)
            {
                basePackages.Add("dotnet-runtime-deps");
            }

            if (packageType == PackageType.Rpm)
            {
                // Runtime deps distro variants (RPM only)
                string[] distros = new[] { "azl.3", "opensuse.15", "sles.15" };
                foreach (string distro in distros)
                {
                    patterns.Add($"dotnet-runtime-deps-*-{distro}-{arch}{extension}");

                    // `azl` deps packages do not have a -newkey- variant
                    if (distro != "azl.3")
                    {
                        patterns.Add($"dotnet-runtime-deps-*-{distro}-newkey-{arch}{extension}");
                    }
                }
            }
        }
        else
        {
            // When not building shared components, only sdk is expected
            basePackages = [ "dotnet-sdk" ];
        }

        // Standard variants
        foreach (string package in basePackages)
        {
            patterns.Add($"{package}-*-{arch}{extension}");
        }

        // New key variants
        foreach (string package in basePackages)
        {
            patterns.Add($"{package}-*-newkey-{arch}{extension}");
        }

        if (packageType == PackageType.Rpm)
        {
            // Azure Linux variants
            foreach (string package in basePackages)
            {
                patterns.Add($"{package}-*-azl-{arch}{extension}");
            }
        }

        return patterns;
    }

    private static string GetAssetsDirectory() => Path.Combine(Directory.GetCurrentDirectory(), "assets");
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.IO.Enumeration;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Extensions.FileSystemGlobbing;
using NuGet.Packaging;
using TestUtilities;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.DotNet.SourceBuild.Tests;

[Trait("Category", "SdkContent")]
public partial class SdkContentTests : SdkTests
{
    private const string BaselineSubDir = nameof(SdkContentTests);
    private const string MsftSdkType = "msft";
    private const string SourceBuildSdkType = "sb";
    public static bool IncludeSdkContentTests => !string.IsNullOrWhiteSpace(Config.MsftSdkTarballPath) && !string.IsNullOrWhiteSpace(Config.SdkTarballPath);

    [GeneratedRegex(@"(<Left>)/.*?(/msft/)")]
    private static partial Regex LeftMsftRegex { get; }

    [GeneratedRegex(@"(<Right>)/.*?(/sb/)")]
    private static partial Regex RightSbRegex { get; }

    [GeneratedRegex(@"(,\s*Version=)(0|[1-9]\d*)(\.\d+){2,3}")]
    private static partial Regex AssemblyVersionRegex { get; }

    private record VersionInfo(Version? AssemblyVersion, string? FileVersion, Version? FileVersionNumber, string? ProductVersion, Version? ProductVersionNumber);

    public SdkContentTests(ITestOutputHelper outputHelper) : base(outputHelper) { }

    /// <Summary>
    /// Verifies the file layout of the source built sdk tarball to the Microsoft build.
    /// The differences are captured in baselines/MsftToSbSdkDiff.txt.
    /// Version numbers that appear in paths are compared but are stripped from the baseline.
    /// This makes the baseline durable between releases.  This does mean however, entries
    /// in the baseline may appear identical if the diff is version specific.
    /// </Summary>
    [ConditionalFact(typeof(SdkContentTests), nameof(IncludeSdkContentTests))]
    public void CompareMsftToSbFileList()
    {
        const string msftFileListingFileName = "msftSdkFiles.txt";
        const string sbFileListingFileName = "sbSdkFiles.txt";
        ExclusionsHelper exclusionsHelper = new ExclusionsHelper("SdkFileDiffExclusions.txt", Config.LogsDirectory, BaselineSubDir);

        WriteTarballFileList(Config.MsftSdkTarballPath, msftFileListingFileName, isPortable: true, MsftSdkType, exclusionsHelper);
        WriteTarballFileList(Config.SdkTarballPath, sbFileListingFileName, isPortable: false, SourceBuildSdkType, exclusionsHelper);

        exclusionsHelper.GenerateNewBaselineFile("FileList");
        
        string diff = BaselineHelper.DiffFiles(msftFileListingFileName, sbFileListingFileName, OutputHelper);
        diff = RemoveDiffMarkers(diff);
        BaselineHelper.CompareBaselineContents("MsftToSbSdkFiles.diff", diff, OutputHelper, BaselineSubDir);
    }

    [ConditionalFact(typeof(SdkContentTests), nameof(IncludeSdkContentTests))]
    public void CompareMsftToSbAssemblyVersions()
    {
        Assert.NotNull(Config.MsftSdkTarballPath);
        Assert.NotNull(Config.SdkTarballPath);

        DirectoryInfo tempDir = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));
        try
        {
            DirectoryInfo sbSdkDir = Directory.CreateDirectory(Path.Combine(tempDir.FullName, SourceBuildSdkType));
            Utilities.ExtractTarball(Config.SdkTarballPath, sbSdkDir.FullName, OutputHelper);

            DirectoryInfo msftSdkDir = Directory.CreateDirectory(Path.Combine(tempDir.FullName, MsftSdkType));
            Utilities.ExtractTarball(Config.MsftSdkTarballPath, msftSdkDir.FullName, OutputHelper);

            Dictionary<string, VersionInfo> sbSdkAssemblyVersions = GetSbSdkAssemblyVersions(sbSdkDir.FullName);
            Dictionary<string, VersionInfo> msftSdkAssemblyVersions = GetMsftSdkAssemblyVersions(msftSdkDir.FullName, sbSdkAssemblyVersions);

            RemoveExcludedAssemblyVersionPaths(sbSdkAssemblyVersions, msftSdkAssemblyVersions);

            const string SbVersionsFileName = "sb_assemblyversions.txt";
            WriteAssemblyVersionsToFile(sbSdkAssemblyVersions, SbVersionsFileName);

            const string MsftVersionsFileName = "msft_assemblyversions.txt";
            WriteAssemblyVersionsToFile(msftSdkAssemblyVersions, MsftVersionsFileName);

            string diff = BaselineHelper.DiffFiles(MsftVersionsFileName, SbVersionsFileName, OutputHelper);
            diff = RemoveDiffMarkers(diff);
            BaselineHelper.CompareBaselineContents("MsftToSbSdkAssemblyVersions.diff", diff, OutputHelper, BaselineSubDir);
        }
        finally
        {
            tempDir.Delete(recursive: true);
        }
    }

    [ConditionalFact(typeof(SdkContentTests), nameof(IncludeSdkContentTests))]
    public void CompareMsftToSbAPIs()
    {
        DirectoryInfo tempDir = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));
        try
        {
            string baselineSuppressionPath = Path.Combine(BaselineHelper.GetAssetsDirectory(), BaselineSubDir, "ApiDiff.suppression");
            string generatedSuppressionPath = Path.Combine(Config.LogsDirectory, "generated_ApiDiff.suppression");

            Assert.True(File.Exists(generatedSuppressionPath), $"Generated API diff suppression file does not exist at path: {generatedSuppressionPath}");

            string updatedSuppressionPath = Path.Combine(Config.LogsDirectory, "UpdatedApiDiff.suppression");
            File.WriteAllText(updatedSuppressionPath, NormalizeApiDiffSuppressionFileContent(File.ReadAllText(generatedSuppressionPath)));

            BaselineHelper.CompareFiles(baselineSuppressionPath, updatedSuppressionPath, OutputHelper);
        }
        finally
        {
            tempDir.Delete(recursive: true);
        }
    }

    /// <Summary>
    /// Verifies that PackageReference versions in template nupkgs are consistent between
    /// the source-built and Microsoft-built SDKs (e.g. https://github.com/dotnet/source-build/issues/5493).
    /// </Summary>
    [ConditionalFact(typeof(SdkContentTests), nameof(IncludeSdkContentTests))]
    public void CompareMsftToSbTemplatePackageVersions()
    {
        Assert.NotNull(Config.MsftSdkTarballPath);
        Assert.NotNull(Config.SdkTarballPath);

        DirectoryInfo tempDir = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));
        try
        {
            DirectoryInfo sbSdkDir = Directory.CreateDirectory(Path.Combine(tempDir.FullName, SourceBuildSdkType));
            Utilities.ExtractTarball(Config.SdkTarballPath, sbSdkDir.FullName, OutputHelper);

            DirectoryInfo msftSdkDir = Directory.CreateDirectory(Path.Combine(tempDir.FullName, MsftSdkType));
            Utilities.ExtractTarball(Config.MsftSdkTarballPath, msftSdkDir.FullName, OutputHelper);

            ExclusionsHelper exclusionsHelper = new("SdkTemplateVersionDiffExclusions.txt", Config.LogsDirectory, BaselineSubDir);

            Dictionary<string, string> sbVersions = GetTemplatePackageVersions(sbSdkDir.FullName, exclusionsHelper, SourceBuildSdkType);
            Dictionary<string, string> msftVersions = GetTemplatePackageVersions(msftSdkDir.FullName, exclusionsHelper, MsftSdkType);

            exclusionsHelper.GenerateNewBaselineFile("TemplateVersions");

            const string MsftVersionsFileName = "msft_templateversions.txt";
            const string SbVersionsFileName = "sb_templateversions.txt";

            File.WriteAllLines(MsftVersionsFileName, msftVersions.OrderBy(kvp => kvp.Key).Select(kvp => $"{kvp.Key} {kvp.Value}"));
            File.WriteAllLines(SbVersionsFileName, sbVersions.OrderBy(kvp => kvp.Key).Select(kvp => $"{kvp.Key} {kvp.Value}"));

            string diff = BaselineHelper.DiffFiles(MsftVersionsFileName, SbVersionsFileName, OutputHelper);
            diff = RemoveDiffMarkers(diff);
            BaselineHelper.CompareBaselineContents("MsftToSbSdkTemplateVersions.diff", diff, OutputHelper, BaselineSubDir);
        }
        finally
        {
            tempDir.Delete(recursive: true);
        }
    }

    /// <summary>
    /// Scans all template nupkgs in an extracted SDK and returns a flat dictionary of
    /// "nupkgId,projPath,PackageName" → version string, with versions in the key normalized.
    /// </summary>
    private static Dictionary<string, string> GetTemplatePackageVersions(string sdkRootDir, ExclusionsHelper exclusionsHelper, string sdkType)
    {
        Dictionary<string, string> result = [];

        string templatesDir = Path.Combine(sdkRootDir, "templates");
        if (!Directory.Exists(templatesDir))
        {
            return result;
        }

        string[] nupkgFiles = Directory.GetFiles(templatesDir, "*.nupkg", SearchOption.AllDirectories);

        string[] projectExtensions = [".csproj", ".fsproj"];

        foreach (string nupkgPath in nupkgFiles)
        {
            using PackageArchiveReader packageReader = new(nupkgPath);
            string normalizedNupkgName = BaselineHelper.RemoveVersions(packageReader.GetIdentity().Id);

            IEnumerable<string> projectFiles = packageReader.GetFiles()
                .Where(f => projectExtensions.Contains(Path.GetExtension(f), StringComparer.OrdinalIgnoreCase));

            foreach (string projectFile in projectFiles)
            {
                Dictionary<string, string> packageVersions = GetPackageReferencesFromProjectFile(packageReader.GetEntry(projectFile));
                foreach ((string packageName, string version) in packageVersions)
                {
                    string sanitizedEntryName = projectFile.Replace('/', '-');
                    string key = $"{normalizedNupkgName},{sanitizedEntryName},{packageName}";
                    if (!exclusionsHelper.IsFileExcluded(key, sdkType))
                    {
                        result[key] = version;
                    }
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Parses PackageReference elements from a project file (.csproj/.fsproj) zip entry and returns package name to version mappings.
    /// </summary>
    private static Dictionary<string, string> GetPackageReferencesFromProjectFile(ZipArchiveEntry entry)
    {
        Dictionary<string, string> versions = [];

        using Stream stream = entry.Open();
        XDocument doc;
        try
        {
            doc = XDocument.Load(stream);
        }
        catch (XmlException ex)
        {
            throw new Exception($"Failed to parse project file '{entry.FullName}' from package archive.", ex);
        }

        IEnumerable<XElement> packageRefs = doc.Descendants()
            .Where(e => e.Name.LocalName == "PackageReference");

        foreach (XElement packageRef in packageRefs)
        {
            string? packageName = packageRef.Attribute("Include")?.Value
                ?? packageRef.Attribute("Update")?.Value;

            if (string.IsNullOrEmpty(packageName))
            {
                continue;
            }

            // Version can be an attribute or a child element
            string? version = packageRef.Attribute("Version")?.Value
                ?? packageRef.Elements().FirstOrDefault(e => e.Name.LocalName == "Version")?.Value;

            if (!string.IsNullOrEmpty(version))
            {
                versions[packageName] = version;
            }
        }

        return versions;
    }

    private string NormalizeApiDiffSuppressionFileContent(string content)
    {
        content = BaselineHelper.RemoveVersions(content);
        content = LeftMsftRegex.Replace(content, "$1$2");
        content = RightSbRegex.Replace(content, "$1$2");
        content = AssemblyVersionRegex.Replace(content, "$1x.y.z");
        return content;
    }

    private void RemoveExcludedAssemblyVersionPaths(Dictionary<string, VersionInfo> sbSdkAssemblyVersions, Dictionary<string, VersionInfo> msftSdkAssemblyVersions)
    {
        // Remove any excluded files as long as SB SDK's file has the same or greater assembly version compared to the corresponding
        // file in the MSFT SDK. If the version is less, the file will show up in the results as this is not a scenario
        // that is valid for shipping.
        ExclusionsHelper exclusionsHelper = new ExclusionsHelper("SdkAssemblyVersionDiffExclusions.txt", Config.LogsDirectory, BaselineSubDir);
        string[] sbSdkFileArray = sbSdkAssemblyVersions.Keys.ToArray();
        for (int i = sbSdkFileArray.Length - 1; i >= 0; i--)
        {
            string assemblyPath = sbSdkFileArray[i];
            VersionInfo sbVersionInfo = sbSdkAssemblyVersions[assemblyPath];
            if (!msftSdkAssemblyVersions.TryGetValue(assemblyPath, out VersionInfo? msftVersionInfo))
            {
                sbSdkAssemblyVersions.Remove(assemblyPath);
                continue;
            }

            if (sbVersionInfo.AssemblyVersion is not null &&
                msftVersionInfo.AssemblyVersion is not null &&
                sbVersionInfo.AssemblyVersion >= msftVersionInfo.AssemblyVersion &&
                exclusionsHelper.IsFileExcluded(assemblyPath))
            {
                sbSdkAssemblyVersions.Remove(assemblyPath);
                msftSdkAssemblyVersions.Remove(assemblyPath);
            }
        }
        exclusionsHelper.GenerateNewBaselineFile();
    }

    private static void WriteAssemblyVersionsToFile(Dictionary<string, VersionInfo> assemblyVersions, string outputPath)
    {
        string[] lines = assemblyVersions
            .Select(kvp => $"{kvp.Key} - AssemblyVersion:{kvp.Value.AssemblyVersion}, FileVersion:{kvp.Value.FileVersion}, FileVersionNumber:{kvp.Value.FileVersionNumber}, ProductVersion:{kvp.Value.ProductVersion}, ProductVersionNumber:{kvp.Value.ProductVersionNumber}")
            .Order()
            .ToArray();
        File.WriteAllLines(outputPath, lines);
    }

    private Dictionary<string, VersionInfo> GetMsftSdkAssemblyVersions(
        string msftSdkPath, Dictionary<string, VersionInfo> sbSdkAssemblyVersions)
    {
        Dictionary<string, VersionInfo> msftSdkAssemblyVersions = new();
        foreach ((string relativePath, _) in sbSdkAssemblyVersions)
        {
            // Now we want to find the corresponding file that exists in the MSFT SDK.
            // We've already replaced version numbers with placeholders in the path.
            // So we can't directly use the relative path to find the corresponding file. Instead,
            // we need to replace the version placeholders with wildcards and find the path through path matching.
            string file = Path.Combine(msftSdkPath, relativePath);
            Matcher matcher = BaselineHelper.GetFileMatcherFromPath(relativePath);

            file = FindMatchingFilePath(msftSdkPath, matcher, relativePath);

            if (!File.Exists(file))
            {
                continue;
            }

            msftSdkAssemblyVersions.Add(BaselineHelper.RemoveVersions(relativePath), GetVersionInfo(file));
        }
        return msftSdkAssemblyVersions;
    }

    private static VersionInfo GetVersionInfo(string filePath)
    {
        AssemblyName assemblyName = AssemblyName.GetAssemblyName(filePath);
        FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(filePath);

        Version fileVersionNumber = new(fileVersionInfo.FileMajorPart, fileVersionInfo.FileMinorPart, fileVersionInfo.FileBuildPart, fileVersionInfo.FilePrivatePart);
        Version productVersionNumber = new(fileVersionInfo.ProductMajorPart, fileVersionInfo.ProductMinorPart, fileVersionInfo.ProductBuildPart, fileVersionInfo.ProductPrivatePart);

        return new VersionInfo(assemblyName.Version, fileVersionInfo.FileVersion, fileVersionNumber, fileVersionInfo.ProductVersion, productVersionNumber);
    }

    private string FindMatchingFilePath(string rootDir, Matcher matcher, string representativeFile)
    {
        foreach (string file in Directory.EnumerateFiles(rootDir, "*", SearchOption.AllDirectories))
        {
            if (matcher.Match(rootDir, file).HasMatches)
            {
                return file;
            }
        }

        OutputHelper.WriteLine($"Unable to find matching file for '{representativeFile}' in '{rootDir}'.");
        return string.Empty;
    }

    private Dictionary<string, VersionInfo> GetSbSdkAssemblyVersions(string sbSdkPath)
    {
        ExclusionsHelper exclusionsHelper = new("SdkFileDiffExclusions.txt", Config.LogsDirectory, BaselineSubDir);
        Dictionary<string, VersionInfo> sbSdkAssemblyVersions = new();
        foreach (string file in Directory.EnumerateFiles(sbSdkPath, "*", SearchOption.AllDirectories))
        {
            string fileExt = Path.GetExtension(file);
            if (fileExt.Equals(".dll", StringComparison.OrdinalIgnoreCase) ||
                fileExt.Equals(".exe", StringComparison.OrdinalIgnoreCase))
            {
                string relativePath = Path.GetRelativePath(sbSdkPath, file);
                string normalizedPath = BaselineHelper.RemoveRids(relativePath, isPortable: false);
                normalizedPath = BaselineHelper.RemoveVersions(normalizedPath);

                if(!exclusionsHelper.IsFileExcluded(normalizedPath, SourceBuildSdkType))
                {
                    sbSdkAssemblyVersions.Add(normalizedPath, GetVersionInfo(file));
                }
            }
        }
        exclusionsHelper.GenerateNewBaselineFile("AssemblyVersions");
        return sbSdkAssemblyVersions;
    }

    private void WriteTarballFileList(string? tarballPath, string outputFileName, bool isPortable, string sdkType, ExclusionsHelper exclusionsHelper)
    {
        if (!File.Exists(tarballPath))
        {
            throw new InvalidOperationException($"Tarball path '{tarballPath}' does not exist.");
        }

        string fileListing = Utilities.GetTarballContentNames(tarballPath).Aggregate((a, b) => $"{a}{Environment.NewLine}{b}");
        fileListing = BaselineHelper.RemoveRids(fileListing, isPortable);
        fileListing = BaselineHelper.RemoveVersions(fileListing);
        IEnumerable<string> files = fileListing.Split(Environment.NewLine).OrderBy(path => path);
        files = files.Where(item => !exclusionsHelper.IsFileExcluded(item, sdkType));

        File.WriteAllLines(outputFileName, files);
    }

    private static string RemoveDiffMarkers(string source)
    {
        Regex indexRegex = new("^index .*", RegexOptions.Multiline);
        string result = indexRegex.Replace(source, "index ------------");

        Regex diffSegmentRegex = new("^@@ .* @@", RegexOptions.Multiline);
        return diffSegmentRegex.Replace(result, "@@ ------------ @@");
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using static SbrpUtilities.CommonUtilities;

if (args.Length < 1)
{
    Console.Error.WriteLine("Usage: UpdateExternalMetadata <component-name>");
    Console.Error.WriteLine("Example: UpdateExternalMetadata azure-activedirectory-identitymodel-extensions-for-dotnet");
    return 1;
}

string componentName = args[0];

// Find repo root
string repoRoot = RunGit("rev-parse --show-toplevel").Trim();
string projFile = Path.Combine(repoRoot, "src", "externalPackages", "projects", $"{componentName}.proj");
string submoduleDir = Path.Combine(repoRoot, "src", "externalPackages", "src", componentName);

if (!File.Exists(projFile))
{
    Console.Error.WriteLine($"Error: Project file not found: {projFile}");
    return 1;
}

if (!Directory.Exists(submoduleDir))
{
    Console.Error.WriteLine($"Error: Submodule directory not found: {submoduleDir}");
    return 1;
}

string projContent = File.ReadAllText(projFile);

// --- 1. Update SourceRevisionId ---
string commitHash = RunGit($"-C \"{submoduleDir}\" rev-parse HEAD").Trim();

string? oldSourceRevisionId = ReadXmlElement(projContent, "SourceRevisionId");
projContent = UpdateXmlElement(projContent, "SourceRevisionId", commitHash);
if (oldSourceRevisionId is not null)
{
    Console.WriteLine($"Updated SourceRevisionId: {oldSourceRevisionId} -> {commitHash}");
}
else
{
    Console.WriteLine("No SourceRevisionId property found - skipping.");
}

// --- 2. Update package-derived version metadata ---
string? validationPackage = ReadXmlElement(projContent, "FileVersionValidationPackage");
string? existingRevision = ReadXmlElement(projContent, "FileVersionRevision");
string? existingAssemblyVersionOverride = ReadXmlElement(projContent, "AssemblyVersionOverride");
string? existingInformationalVersionOverride = ReadXmlElement(projContent, "InformationalVersionOverride");
bool hasMetadataProperties = existingRevision is not null
    || existingAssemblyVersionOverride is not null
    || existingInformationalVersionOverride is not null;

if (string.IsNullOrEmpty(validationPackage) || !hasMetadataProperties)
{
    Console.WriteLine("No package-derived version metadata configured - skipping metadata update.");
    File.WriteAllText(projFile, projContent);
    Console.WriteLine($"Saved changes to {componentName}.proj");
    return 0;
}

// Find release version from eng/Versions.props
string versionsPropsPath = Path.Combine(repoRoot, "eng", "Versions.props");
string? releaseVersion = FindReleaseVersion(versionsPropsPath, componentName);

if (string.IsNullOrEmpty(releaseVersion))
{
    Console.Error.WriteLine($"Error: Could not find release version for '{componentName}' in eng/Versions.props.");
    return 1;
}

Console.WriteLine($"Found release version: {releaseVersion}");
Console.WriteLine($"Downloading {validationPackage} {releaseVersion}...");

var versionMetadata = await GetPackageVersionMetadataAsync(
    repoRoot, validationPackage, releaseVersion);

if (versionMetadata.Revision is null && existingRevision is not null)
{
    Console.Error.WriteLine($"Error: Unable to get FileVersion from {validationPackage} {releaseVersion}.");
    return 1;
}

if (existingRevision is not null)
{
    string revisionStr = versionMetadata.Revision!.Value.ToString();
    projContent = UpdateXmlElement(projContent, "FileVersionRevision", revisionStr);
    Console.WriteLine($"Updated FileVersionRevision: {existingRevision} -> {revisionStr} " +
        $"(from {validationPackage} {releaseVersion}, FileVersion: {versionMetadata.FileVersion})");
}

if (existingAssemblyVersionOverride is not null)
{
    if (string.IsNullOrEmpty(versionMetadata.AssemblyVersion))
    {
        Console.Error.WriteLine($"Error: Unable to get AssemblyVersion from {validationPackage} {releaseVersion}.");
        return 1;
    }

    projContent = UpdateXmlElement(projContent, "AssemblyVersionOverride", versionMetadata.AssemblyVersion);
    Console.WriteLine($"Updated AssemblyVersionOverride: {existingAssemblyVersionOverride} -> {versionMetadata.AssemblyVersion} " +
        $"(from {validationPackage} {releaseVersion})");
}

if (existingInformationalVersionOverride is not null)
{
    if (string.IsNullOrEmpty(versionMetadata.InformationalVersion))
    {
        Console.Error.WriteLine($"Error: Unable to get InformationalVersion from {validationPackage} {releaseVersion}.");
        return 1;
    }

    projContent = UpdateXmlElement(projContent, "InformationalVersionOverride", versionMetadata.InformationalVersion);
    Console.WriteLine($"Updated InformationalVersionOverride: {existingInformationalVersionOverride} -> {versionMetadata.InformationalVersion} " +
        $"(from {validationPackage} {releaseVersion})");
}

File.WriteAllText(projFile, projContent);
Console.WriteLine($"Saved changes to {componentName}.proj");
return 0;

// --- Helper methods ---

static string RunGit(string arguments)
{
    using Process process = new()
    {
        StartInfo = new ProcessStartInfo
        {
            FileName = "git",
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        }
    };

    process.Start();
    string output = process.StandardOutput.ReadToEnd();
    process.WaitForExit();

    if (process.ExitCode != 0)
    {
        string error = process.StandardError.ReadToEnd();
        throw new InvalidOperationException($"git {arguments} failed (exit {process.ExitCode}): {error}");
    }

    return output;
}

static string? ReadXmlElement(string content, string elementName)
{
    Match match = Regex.Match(content, $@"<{elementName}>([^<]*)</{elementName}>");
    return match.Success ? match.Groups[1].Value : null;
}

static string UpdateXmlElement(string content, string elementName, string newValue)
{
    string pattern = $@"<{elementName}>[^<]*</{elementName}>";
    string replacement = $"<{elementName}>{newValue}</{elementName}>";

    if (Regex.IsMatch(content, pattern))
    {
        string oldValue = Regex.Match(content, pattern).Value;
        return content.Replace(oldValue, replacement);
    }

    return content;
}

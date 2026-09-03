// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Build.Evaluation;
using SbrpUtilities;

if (args.Length < 1)
{
    Console.Error.WriteLine("Usage: UpdateExternalMetadata <component-name>");
    Console.Error.WriteLine("Example: UpdateExternalMetadata azure-activedirectory-identitymodel-extensions-for-dotnet");
    return 1;
}

string componentName = args[0];
string repoRoot = RunGit("rev-parse --show-toplevel").Trim();
string componentPropsPath = Path.Combine(repoRoot, "src", "externalPackages", "projects", $"{componentName}.props");
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

// XDocument + line-buffer view of the .proj is used for surgical in-place updates that
// preserve formatting exactly. MSBuild's XML round-tripping reflows the file in ways we
// do not want (XML decl, attribute layout, whitespace), so writes go through the line
// buffer instead.
FileEditBuffer projBuffer = FileEditBuffer.Load(projFile);

// --- 1. Update SourceRevisionId in the .proj file ---
// This runs unconditionally for every component — even those that don't have a sibling
// <component>.props (no validation overrides). SourceRevisionId is tied to the submodule's
// HEAD commit and is orthogonal to validation overrides.
string commitHash = RunGit($"-C \"{submoduleDir}\" rev-parse HEAD").Trim();

XElement? sourceRevisionIdEl = projBuffer.Document.Root?
    .Elements("PropertyGroup")
    .Elements("SourceRevisionId")
    .LastOrDefault();

if (sourceRevisionIdEl is not null)
{
    string oldValue = sourceRevisionIdEl.Value;
    UpdatePropertyLine(projBuffer.Lines, sourceRevisionIdEl, "SourceRevisionId", commitHash);
    Console.WriteLine($"Updated SourceRevisionId: {oldValue} -> {commitHash}");
}
else
{
    Console.WriteLine("No SourceRevisionId property found in .proj - skipping.");
}

// --- 2. Update package-derived version metadata in <component>.props ---
// Validation overrides are optional per component: only components whose source-built
// assemblies need their AssemblyVersion / FileVersion / InformationalVersion pinned to the
// MSFT-shipped values have a sibling <component>.props. For everything else, save the
// SourceRevisionId update and exit successfully.
if (!File.Exists(componentPropsPath))
{
    projBuffer.Save();
    Console.WriteLine($"No sibling {componentName}.props - skipping metadata update.");
    Console.WriteLine($"Saved changes to {componentName}.proj");
    return 0;
}

// Two views of <component>.props coexist:
//   (1) MSBuild evaluation — authoritative for which validation items exist, which property
//       names they bind to (including ItemDefinitionGroup defaults from the imported shared
//       validation.props), and the evaluated property values.
//   (2) XDocument + line-buffer — used for surgical in-place updates that preserve
//       formatting exactly (same reason as for the .proj above).
Project project = new(componentPropsPath);
FileEditBuffer componentPropsBuffer = FileEditBuffer.Load(componentPropsPath);

// All <PropertyGroup> elements in the per-component .props file are in scope — the file
// is component-scoped so there's no component-discrimination needed at this level.
List<XElement> componentPropertyGroups = componentPropsBuffer.Document.Root?
    .Elements("PropertyGroup")
    .ToList() ?? new List<XElement>();

IReadOnlyList<ValidationPackageItem> validationItems = CommonUtilities.ParseValidationPackageItems(project);

if (validationItems.Count == 0)
{
    Console.WriteLine("No <FileVersionValidationPackage> items configured - skipping metadata update.");
    projBuffer.Save();
    Console.WriteLine($"Saved changes to {componentName}.proj");
    return 0;
}

string versionsPropsPath = Path.Combine(repoRoot, "eng", "Versions.props");
Project versionsProject = new(versionsPropsPath);

// Cache downloads across items so multiple items sharing the same package id+version
// (a legitimate scenario, e.g. when one package contributes several aspects) don't re-fetch.
Dictionary<string, PackageVersionMetadata> metadataCache = new();

foreach (ValidationPackageItem item in validationItems)
{
    string? releaseVersion = item.ReleaseVersionPropertyName is not null
        ? CommonUtilities.FindReleaseVersionByPropertyName(versionsProject, item.ReleaseVersionPropertyName)
        : CommonUtilities.FindReleaseVersion(versionsProject, componentName);

    if (string.IsNullOrEmpty(releaseVersion))
    {
        Console.Error.WriteLine($"Error: Could not find release version for '{item.PackageId}' " +
            $"({(item.ReleaseVersionPropertyName is null ? "auto-derived" : $"property '{item.ReleaseVersionPropertyName}'")}).");
        return 1;
    }

    AspectUpdate? revUpdate = ResolveAspect(item.PackageId, item.FileVersionRevision, componentPropertyGroups, "FileVersionRevisionProperty");
    AspectUpdate? avoUpdate = ResolveAspect(item.PackageId, item.AssemblyVersionOverride, componentPropertyGroups, "AssemblyVersionOverrideProperty");
    AspectUpdate? ivoUpdate = ResolveAspect(item.PackageId, item.InformationalVersionOverride, componentPropertyGroups, "InformationalVersionOverrideProperty");

    if (revUpdate is null && avoUpdate is null && ivoUpdate is null)
    {
        Console.WriteLine($"No override properties to update for {item.PackageId} - skipping.");
        continue;
    }

    string cacheKey = $"{item.PackageId}|{releaseVersion}";
    if (!metadataCache.TryGetValue(cacheKey, out PackageVersionMetadata? versionMetadata))
    {
        Console.WriteLine($"Downloading {item.PackageId} {releaseVersion}...");
        versionMetadata = await CommonUtilities.GetPackageVersionMetadataAsync(repoRoot, item.PackageId, releaseVersion);
        metadataCache[cacheKey] = versionMetadata;
    }

    if (revUpdate is { } rev)
    {
        if (versionMetadata.Revision is null)
        {
            Console.Error.WriteLine($"Error: Unable to get FileVersion from {item.PackageId} {releaseVersion}.");
            return 1;
        }

        string revisionStr = versionMetadata.Revision.Value.ToString();
        string oldValue = rev.Element.Value;
        UpdatePropertyLine(componentPropsBuffer.Lines, rev.Element, rev.PropertyName, revisionStr);
        Console.WriteLine($"Updated {rev.PropertyName}: {oldValue} -> {revisionStr} " +
            $"(from {item.PackageId} {releaseVersion}, FileVersion: {versionMetadata.FileVersion})");
    }

    if (avoUpdate is { } avo)
    {
        if (string.IsNullOrEmpty(versionMetadata.AssemblyVersion))
        {
            Console.Error.WriteLine($"Error: Unable to get AssemblyVersion from {item.PackageId} {releaseVersion}.");
            return 1;
        }

        string oldValue = avo.Element.Value;
        UpdatePropertyLine(componentPropsBuffer.Lines, avo.Element, avo.PropertyName, versionMetadata.AssemblyVersion);
        Console.WriteLine($"Updated {avo.PropertyName}: {oldValue} -> {versionMetadata.AssemblyVersion} " +
            $"(from {item.PackageId} {releaseVersion})");
    }

    if (ivoUpdate is { } ivo)
    {
        if (string.IsNullOrEmpty(versionMetadata.InformationalVersion))
        {
            Console.Error.WriteLine($"Error: Unable to get InformationalVersion from {item.PackageId} {releaseVersion}.");
            return 1;
        }

        string oldValue = ivo.Element.Value;
        UpdatePropertyLine(componentPropsBuffer.Lines, ivo.Element, ivo.PropertyName, versionMetadata.InformationalVersion);
        Console.WriteLine($"Updated {ivo.PropertyName}: {oldValue} -> {versionMetadata.InformationalVersion} " +
            $"(from {item.PackageId} {releaseVersion})");
    }
}

projBuffer.Save();
componentPropsBuffer.Save();
Console.WriteLine($"Saved changes to {componentName}.proj and {componentName}.props");
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

// Looks up the XElement for an aspect's target property in the per-component .props
// file. Returns null when the binding is null (item is not configured for this aspect)
// or when the binding came from an ItemDefinitionGroup default and the named property
// is not actually present (defaulted bindings are "validate if present"). Throws when
// the binding was explicit on the item itself and the named property is missing —
// that always signals a configuration error.
static AspectUpdate? ResolveAspect(string packageId, AspectBinding? binding, List<XElement> componentPropertyGroups, string metadataName)
{
    if (binding is null)
    {
        return null;
    }

    XElement? element = FindPropertyElement(componentPropertyGroups, binding.PropertyName);
    if (element is null)
    {
        if (binding.IsExplicit)
        {
            throw new InvalidOperationException(
                $"<FileVersionValidationPackage Include=\"{packageId}\"> {metadataName} names " +
                $"property '{binding.PropertyName}' but no such property is defined in the component's " +
                $".props file.");
        }

        return null;
    }

    return new AspectUpdate(binding.PropertyName, element);
}

// MSBuild evaluates properties top-to-bottom and last-write-wins. Match that semantics when
// the .props file accidentally contains duplicate declarations: target the last occurrence
// so the rewrite affects the property MSBuild actually consumes.
static XElement? FindPropertyElement(List<XElement> componentPropertyGroups, string propertyName) =>
    componentPropertyGroups
        .Elements(propertyName)
        .LastOrDefault();

// Surgically updates the on-disk line containing a property element to a new value.
// Locates the line via the XElement's IXmlLineInfo, then runs a narrow regex on that
// single line. Comments and other PropertyGroups on different lines are not at risk
// because (a) XDocument navigation already filtered out comments, and (b) the regex
// is scoped to one line. Properties that span multiple lines (rare in .proj files)
// surface as an InvalidOperationException rather than silently no-op.
static void UpdatePropertyLine(List<string> lines, XElement element, string elementName, string newValue)
{
    // Single-line replacement invariant: each edit must not change the file's line count,
    // otherwise the IXmlLineInfo line numbers captured earlier from XDocument would drift
    // for any subsequent UpdatePropertyLine call on the same buffer.
    if (newValue.Contains('\n') || newValue.Contains('\r'))
    {
        throw new InvalidOperationException(
            $"<{elementName}> replacement value contains a newline, which would break " +
            $"line-number tracking for subsequent edits. Value: [{newValue}].");
    }

    IXmlLineInfo info = element;
    int lineIndex = info.LineNumber - 1;
    if (lineIndex < 0 || lineIndex >= lines.Count)
    {
        throw new InvalidOperationException(
            $"<{elementName}> line {info.LineNumber} is out of range (file has {lines.Count} lines).");
    }

    string originalLine = lines[lineIndex];
    string escapedName = Regex.Escape(elementName);
    string pattern = $"<{escapedName}>[^<]*</{escapedName}>";
    string replacement = $"<{elementName}>{new XText(newValue)}</{elementName}>";

    // Use Match.Success rather than string equality to detect "didn't match":
    // a no-op replacement (newValue == oldValue) leaves the line bytewise identical
    // and must not be treated as a failure.
    if (!Regex.IsMatch(originalLine, pattern))
    {
        throw new InvalidOperationException(
            $"Could not update <{elementName}> on line {info.LineNumber}: no inline " +
            $"<{elementName}>...</{elementName}> match found. Line content: [{originalLine}]. " +
            $"The property may span multiple lines.");
    }

    // Use the MatchEvaluator overload (rather than the string-replacement overload) so that
    // any '$' in newValue is treated literally instead of as a Regex substitution token
    // (e.g. $1, $&, ${name}). XText escapes XML entities but not '$', so this matters for
    // values such as InformationalVersion strings that may include SemVer build metadata.
    lines[lineIndex] = Regex.Replace(originalLine, pattern, _ => replacement);
}

internal record AspectUpdate(string PropertyName, XElement Element);

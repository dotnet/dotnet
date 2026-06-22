// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NuGet.Packaging;
using NuGet.Packaging.Core;

namespace Microsoft.DotNet.SourceBuild.Tasks
{
    public class GenerateProject : Task
    {
        // Centralized defaults set by src/referencePackages/Directory.Build.props.
        // When a nuspec value matches one of these (after normalization), the
        // corresponding property is omitted from the generated csproj.
        private const string CentralizedAuthors = "Microsoft";
        private const string CentralizedServiceable = "true";
        private const string CentralizedCopyright = "© Microsoft Corporation. All rights reserved.";

        // License URL rewrites that match the legacy fwlink URLs replaced when the source
        // generator surfaces a license URL into the csproj.
        private const string MicrosoftMitLicenseUrl = "https://microsoft.mit-license.org/";
        private static readonly (string From, string To)[] LicenseUrlRewrites =
        [
            ("http://go.microsoft.com/fwlink/?LinkId=529443", MicrosoftMitLicenseUrl),
            ("http://go.microsoft.com/fwlink/?LinkId=329770", MicrosoftMitLicenseUrl),
        ];

        /// <summary>
        /// The package id.
        /// </summary>
        [Required]
        public required string PackageId { get; set; }

        /// <summary>
        /// The package version.
        /// </summary>
        [Required]
        public required string PackageVersion { get; set; }

        /// <summary>
        /// The path to the project template that is being transformed.
        /// </summary>
        [Required]
        public required string ProjectTemplate { get; set; }

        /// <summary>
        /// The target path that the project file is written to.
        /// </summary>
        [Required]
        public required string TargetPath { get; set; }

        /// <summary>
        /// The root directory that the projects are written into.
        /// </summary>
        [Required]
        public required string ProjectRoot { get; set; }

        /// <summary>
        /// The root directory that contains the reference package projects.
        /// </summary>
        [Required]
        public required string ReferencePackagesRoot { get; set; }

        /// <summary>
        /// The root directory that contains the text-only package projects. Used as a
        /// fallback location when a dependency can't be found under <see cref="ReferencePackagesRoot"/>.
        /// </summary>
        public string? TextOnlyPackagesRoot { get; set; }

        /// <summary>
        /// The package's compile items, including target framework metadata.
        /// </summary>
        public ITaskItem[] CompileItems { get; set; } = Array.Empty<ITaskItem>();

        /// <summary>
        /// The package's dependencies with the PackageId as the identity and the version and target framework as metadata.
        /// </summary>
        public ITaskItem[] PackageDependencies { get; set; } = Array.Empty<ITaskItem>();

        /// <summary>
        /// The package's framework references with the framework reference assembly as the identity and the target framework as metadata.
        /// </summary>
        public ITaskItem[] FrameworkReferences { get; set; } = Array.Empty<ITaskItem>();

        /// <summary>
        /// Placeholder files (e.g. <c>lib/&lt;tfm&gt;/_._</c>) discovered in the source package, with target framework metadata.
        /// Their TFMs are added to TargetFrameworks and a conditional import of <c>placeholderpackaging.targets</c> is emitted
        /// for each so that the produced package retains the empty per-TFM dependency groups and <c>lib/&lt;tfm&gt;/_._</c>
        /// content of the source package.
        /// </summary>
        public ITaskItem[] PlaceholderFiles { get; set; } = Array.Empty<ITaskItem>();

        /// <summary>
        /// The list of dependencies (package id) that should get emitted as PackageReference items.
        /// </summary>
        public string[]? AllowedPackageReference { get; set; }

        /// <summary>
        /// Optional path to the source nuspec. When provided, per-package metadata (Description, license,
        /// repository, etc.) is extracted and emitted into the generated csproj instead of being copied into a
        /// hand-authored nuspec. Used for reference and text-only packages; left empty for targeting packages.
        /// </summary>
        public string? PackageNuspecPath { get; set; }

        /// <summary>
        /// Optional package type ("ref", "text", or "target"). Drives generator behaviors that differ between
        /// reference, text-only, and targeting packages — e.g. text-only packages additionally emit a
        /// packaging item group that picks up files in the project directory and surface
        /// <c>&lt;PackageType&gt;</c>/<c>&lt;contentFiles&gt;</c> values from the source nuspec.
        /// </summary>
        public string? PackageType { get; set; }

        public override bool Execute()
        {
            string referenceIncludes = "";
            StrongNameData strongNameData = default;
            string projectContent = File.ReadAllText(ProjectTemplate);
            string projectDirectory = Path.GetDirectoryName(TargetPath)!;

            // Pick the appropriate MSBuild SDK for the package type:
            //  - reference packages compile generated .cs sources, so they need Microsoft.NET.Sdk.
            //  - text-only and targeting packages don't compile anything; they only pack on-disk
            //    content (and in the targeting-pack case, ilasm output staged via custom targets).
            //    Microsoft.Build.NoTargets is purpose-built for that and avoids the per-layer
            //    workarounds we'd otherwise need (Build no-op, EnableDefaultCompileItems=false,
            //    DisableImplicitFrameworkReferences=true, etc.).
            string sdk = string.Equals(PackageType, "text", StringComparison.OrdinalIgnoreCase) ||
                         string.Equals(PackageType, "target", StringComparison.OrdinalIgnoreCase)
                ? "Microsoft.Build.NoTargets"
                : "Microsoft.NET.Sdk";
            projectContent = projectContent.Replace("$$Sdk$$", sdk);

            // Calculate the target frameworks based on the passed-in items. Placeholder TFMs are
            // included so that they appear in <TargetFrameworks> alongside the real TFMs.
            string[] targetFrameworks = CompileItems.Select(compileItem => compileItem.GetMetadata(SharedMetadata.TargetFrameworkMetadataName))
                .Concat(PlaceholderFiles.Select(placeholderFile => placeholderFile.GetMetadata(SharedMetadata.TargetFrameworkMetadataName)))
                .ToArray();

            if (targetFrameworks.Length == 0)
                targetFrameworks = PackageDependencies.Select(packageDependency => packageDependency.GetMetadata(SharedMetadata.TargetFrameworkMetadataName)).ToArray();

            if (targetFrameworks.Length == 0)
                targetFrameworks = FrameworkReferences.Select(frameworkReference => frameworkReference.GetMetadata(SharedMetadata.TargetFrameworkMetadataName)).ToArray();

            targetFrameworks = targetFrameworks.Distinct()
                .Order()
                .ToArray();

            // If no target framework is supplied, fallback to netstandard2.0.
            projectContent = projectContent.Replace("$$TargetFrameworks$$",
                targetFrameworks.Length > 0 ? string.Join(';', targetFrameworks) : "netstandard2.0");

            projectContent = projectContent.Replace("$$PackageVersion$$", PackageVersion);

            foreach (string targetFramework in targetFrameworks)
            {
                string packageReferences = string.Empty;
                string projectReferences = string.Empty;

                // Add package dependencies
                foreach (ITaskItem packageDependency in PackageDependencies.Where(packageDependency => packageDependency.GetMetadata(SharedMetadata.TargetFrameworkMetadataName) == targetFramework))
                {
                    string dependencyVersion = packageDependency.GetMetadata("Version");

                    // If the dependency is on the package reference allowed list (i.e. for externalPackages like Newtonsoft.Json), emit a package reference. Otherwise, emit a project reference.
                    if (AllowedPackageReference is not null && AllowedPackageReference.Contains(packageDependency.ItemSpec))
                    {
                        packageReferences += $"    <PackageReference Include=\"{packageDependency.ItemSpec}\" Version=\"{dependencyVersion}\" />{Environment.NewLine}";
                    }
                    else
                    {
                        // Default lookup is under the reference packages root, but a few
                        // dependencies (e.g. Microsoft.NETCore.Platforms used by NETStandard.Library
                        // target packs) live under the text-only packages root. Fall back to that
                        // location when the reference-packages copy doesn't exist.
                        string fileName = $"{packageDependency.ItemSpec}.{dependencyVersion}.csproj";
                        string lowerId = packageDependency.ItemSpec.ToLowerInvariant();
                        string dependencyProjectPath = Path.Combine(ReferencePackagesRoot, lowerId, dependencyVersion, fileName);
                        if (!File.Exists(dependencyProjectPath) &&
                            !string.IsNullOrEmpty(TextOnlyPackagesRoot))
                        {
                            string textOnlyPath = Path.Combine(TextOnlyPackagesRoot, lowerId, dependencyVersion, fileName);
                            if (File.Exists(textOnlyPath))
                                dependencyProjectPath = textOnlyPath;
                        }
                        // Make sure that the path always uses forward slashes, even on Windows.
                        string dependencyProjectRelativePath = Path.GetRelativePath(projectDirectory, dependencyProjectPath).Replace('\\', '/');

                        projectReferences += $"    <ProjectReference Include=\"{dependencyProjectRelativePath}\" />{Environment.NewLine}";
                    }
                }

                if (packageReferences != string.Empty || projectReferences != string.Empty)
                {
                    referenceIncludes += $"  <ItemGroup Condition=\"'$(TargetFramework)' == '{targetFramework}'\">{Environment.NewLine}";
                    referenceIncludes += packageReferences + projectReferences;
                    referenceIncludes += $"  </ItemGroup>{Environment.NewLine}{Environment.NewLine}";
                }

                // Retrieve the target framework's strong name data. For historical reasons,
                // we just use the first item that has the data available.
                if (strongNameData == default)
                {
                    foreach (ITaskItem compileItem in CompileItems.Where(compileItem => compileItem.GetMetadata(SharedMetadata.TargetFrameworkMetadataName) == targetFramework))
                    {
                        string strongNameKey = compileItem.GetMetadata(SharedMetadata.StrongNameKeyMetadataName);
                        string strongNameId = compileItem.GetMetadata(SharedMetadata.StrongNameIdMetadataName);
                        string strongNameFilename = compileItem.GetMetadata(SharedMetadata.StrongNameFilenameMetadataName);

                        if (!string.IsNullOrWhiteSpace(strongNameKey) &&
                            !string.IsNullOrWhiteSpace(strongNameId) &&
                            !string.IsNullOrWhiteSpace(strongNameFilename))
                        {
                            strongNameData = new(strongNameKey, strongNameId, strongNameFilename);
                            break;
                        }
                    }
                }
            }

            // Write the gathered package references into the project file.
            projectContent = projectContent.Replace("$$References$$", referenceIncludes);

            // If necessary, write the strong name key into the project file. Don't generate StrongNameKeyId for MSFT key.
            string keyFileTag = (strongNameData != default && strongNameData.Filename != "MSFT") ?
                $"{Environment.NewLine}    <StrongNameKeyId>{strongNameData.Id}</StrongNameKeyId>" :
                string.Empty;
            projectContent = projectContent.Replace("$$KeyFileTag$$", keyFileTag);

            // Calculate the assembly name from the compile items assembly name metadata. If more than one
            // distinct name is found (i.e. multi assembly package), use the PackageId instead.
            string[] assemblyNames = CompileItems
                .Select(compileItem => compileItem.GetMetadata(SharedMetadata.AssemblyNameMetadataName))
                .Distinct()
                .ToArray();
            projectContent = projectContent.Replace("$$AssemblyName$$",
                 assemblyNames.Length == 1 ? assemblyNames[0] : PackageId);

            // Always emit <PackageId> so the package id is independent of <AssemblyName>.
            // Without this, multi-assembly-named packages (e.g. microsoft.codeanalysis.common
            // whose assembly is Microsoft.CodeAnalysis.dll) would pack under the AssemblyName
            // instead of the original package id.
            projectContent = projectContent.Replace("$$PackageId$$", PackageId);

            // Detect whether the source assemblies were originally shipped under `ref/` (instead of
            // `lib/`). Reference-only packages (e.g. Microsoft.Build, Microsoft.Build.Framework) ship
            // their assemblies under `ref/<tfm>/` to indicate they are reference assemblies. MSBuild
            // Pack defaults to `lib/<tfm>/`, which would silently change the package layout.
            string buildOutputTargetFolderTag = string.Empty;
            if (CompileItems.Length > 0 && CompileItems.All(c => c.ItemSpec.Replace('\\', '/').StartsWith("ref/", StringComparison.OrdinalIgnoreCase)))
            {
                buildOutputTargetFolderTag = $"{Environment.NewLine}    <BuildOutputTargetFolder>ref</BuildOutputTargetFolder>";
            }
            projectContent = projectContent.Replace("$$BuildOutputTargetFolderTag$$", buildOutputTargetFolderTag);

            // Embed per-package nuspec metadata into the csproj when a source nuspec is provided.
            // For text-only and targeting packages, no nuspec path is passed so the tokens collapse to empty.
            string extraProperties = BuildNuspecMetadata() + BuildPlaceholderInPropertyGroup();
            projectContent = projectContent.Replace("$$ExtraProperties$$", extraProperties);

            // Text-only packages need an extra <ItemGroup> that picks up the on-disk content
            // (Sdk/, runtime.json, contentFiles/, ...) into the produced .nupkg via <None Pack="true">.
            // Reference and targeting packages don't need this — their content is the produced assembly
            // (handled by MSBuild Pack's default behavior) plus, for the placeholder TFMs, the centralized
            // eng/_._ contributed via eng/placeholderpackaging.targets.
            projectContent = projectContent.Replace("$$Packaging$$", BuildPackagingItems());

            // Final formatting pass: ensure consistent spacing between top-level elements
            //   * exactly one blank line between sibling <PropertyGroup>/<ItemGroup>
            //   * exactly one blank line before the closing </Project>
            //   * exactly one trailing newline at end-of-file
            projectContent = NormalizeProjectWhitespace(projectContent);

            // Generate the project file
            Directory.CreateDirectory(projectDirectory);
            File.WriteAllText(TargetPath, projectContent);

            return true;
        }

        private static string NormalizeProjectWhitespace(string content)
        {
            string nl = Environment.NewLine;
            // Collapse 2+ consecutive blank lines down to a single blank line (i.e. 3+ newlines → 2 newlines).
            // Use a simple loop on the platform newline so we don't accidentally mix CR/LF styles.
            string twoNewlines = nl + nl;
            string threeNewlines = nl + nl + nl;
            while (content.Contains(threeNewlines))
            {
                content = content.Replace(threeNewlines, twoNewlines);
            }
            // Ensure exactly one blank line before </Project>.
            int closingIndex = content.LastIndexOf("</Project>", StringComparison.Ordinal);
            if (closingIndex > 0)
            {
                string before = content.Substring(0, closingIndex).TrimEnd('\r', '\n');
                string after = content.Substring(closingIndex);
                content = before + nl + nl + after;
            }
            // Ensure exactly one trailing newline at end-of-file.
            content = content.TrimEnd('\r', '\n') + nl;
            return content;
        }

        private string BuildPackagingItems()
        {
            // Text-only and target packs both pack on-disk content via explicit <None> items.
            // Reference packages produce their content via the build (assembly) plus optional
            // placeholder targets, so we skip this step for them.
            if (!string.Equals(PackageType, "text", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(PackageType, "target", StringComparison.OrdinalIgnoreCase))
            {
                return string.Empty;
            }

            string projectDirectory = Path.GetDirectoryName(TargetPath)!;
            string nuspecFileName = string.IsNullOrEmpty(PackageNuspecPath) ? string.Empty : Path.GetFileName(PackageNuspecPath);

            // Discover all packageable files on disk. The textOnlyPackages on-disk layout already
            // matches the .nupkg layout (e.g. Sdk/Sdk.targets, contentFiles/cs/<tfm>/...), so a simple
            // include-everything-except-{csproj,nuspec} works for all packages.
            List<string> allFiles = new();
            if (Directory.Exists(projectDirectory))
            {
                foreach (string f in Directory.EnumerateFiles(projectDirectory, "*", SearchOption.AllDirectories))
                {
                    string rel = Path.GetRelativePath(projectDirectory, f).Replace('\\', '/');
                    if (rel.StartsWith("obj/", StringComparison.OrdinalIgnoreCase) ||
                        rel.StartsWith("bin/", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }
                    if (rel.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase))
                        continue;
                    // .il files are the source for ilasm; the assembled .dll output is added by
                    // a separate target in src/targetPacks/Directory.Build.targets and should not
                    // be confused with content to pack.
                    if (rel.EndsWith(".il", StringComparison.OrdinalIgnoreCase))
                        continue;
                    if (!string.IsNullOrEmpty(nuspecFileName) &&
                        rel.Equals(nuspecFileName, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }
                    allFiles.Add(rel);
                }
            }

            if (allFiles.Count == 0)
                return string.Empty;

            // Read <contentFiles> from the source nuspec to know which files need explicit
            // BuildAction/CopyToOutput metadata. NuGet auto-generates <contentFiles><files>
            // entries from the file's PackagePath if BuildAction is left unspecified, so we only
            // need to forward an explicit override when one is present.
            Dictionary<string, ContentFileEntry> contentFileEntries = ReadContentFileEntries();

            StringBuilder builder = new();
            builder.AppendLine();
            builder.AppendLine("  <ItemGroup>");
            foreach (string rel in allFiles.OrderBy(f => f, StringComparer.OrdinalIgnoreCase))
            {
                contentFileEntries.TryGetValue(rel, out ContentFileEntry? entry);
                EmitNoneItem(builder, rel, entry);
            }
            builder.Append("  </ItemGroup>");
            builder.AppendLine();
            return builder.ToString();
        }

        private sealed record ContentFileEntry(string? BuildAction, string? CopyToOutput, string? Flatten);

        private Dictionary<string, ContentFileEntry> ReadContentFileEntries()
        {
            Dictionary<string, ContentFileEntry> map = new(StringComparer.OrdinalIgnoreCase);
            if (string.IsNullOrEmpty(PackageNuspecPath) || !File.Exists(PackageNuspecPath))
                return map;

            try
            {
                System.Xml.Linq.XDocument doc = System.Xml.Linq.XDocument.Load(PackageNuspecPath);
                System.Xml.Linq.XNamespace ns = doc.Root?.GetDefaultNamespace() ?? System.Xml.Linq.XNamespace.None;
                System.Xml.Linq.XElement? contentFiles = doc.Root?
                    .Element(ns + "metadata")?
                    .Element(ns + "contentFiles");
                if (contentFiles is null)
                    return map;

                foreach (System.Xml.Linq.XElement files in contentFiles.Elements(ns + "files"))
                {
                    string include = files.Attribute("include")?.Value ?? string.Empty;
                    if (string.IsNullOrEmpty(include))
                        continue;
                    string normalized = include.Replace('\\', '/');
                    string relPath = "contentFiles/" + normalized;
                    map[relPath] = new ContentFileEntry(
                        BuildAction: files.Attribute("buildAction")?.Value,
                        CopyToOutput: files.Attribute("copyToOutput")?.Value,
                        Flatten: files.Attribute("flatten")?.Value);
                }
            }
            catch
            {
                // If the nuspec can't be parsed for contentFiles, fall back to no overrides; NuGet
                // will use its default heuristics from PackagePath.
            }
            return map;
        }

        private static void EmitNoneItem(StringBuilder builder, string relPath, ContentFileEntry? entry)
        {
            // For PackagePath, use the directory portion only — NuGet places files using
            // <Include's filename> appended to PackagePath. Setting PackagePath to the full file
            // path (e.g. "LICENSE" without extension) causes NuGet to treat it as a folder and
            // emit the file as "LICENSE/LICENSE", which breaks <PackageLicenseFile> validation
            // (NU5030).
            int lastSlash = relPath.LastIndexOf('/');
            string packagePath = lastSlash >= 0 ? relPath.Substring(0, lastSlash) : string.Empty;

            builder.Append("    <None Include=\"")
                   .Append(relPath)
                   .Append("\" Pack=\"true\" PackagePath=\"")
                   .Append(packagePath)
                   .Append('"');
            if (entry is not null)
            {
                if (!string.IsNullOrEmpty(entry.BuildAction))
                    builder.Append(" BuildAction=\"").Append(entry.BuildAction).Append('"');
                if (!string.IsNullOrEmpty(entry.CopyToOutput))
                    builder.Append(" CopyToOutput=\"").Append(entry.CopyToOutput).Append('"');
                if (!string.IsNullOrEmpty(entry.Flatten))
                    builder.Append(" Flatten=\"").Append(entry.Flatten).Append('"');
            }
            builder.AppendLine(" />");
        }

        private string BuildNuspecMetadata()
        {
            if (string.IsNullOrEmpty(PackageNuspecPath) || !File.Exists(PackageNuspecPath))
                return string.Empty;

            NuspecReader nuspecReader = new(PackageNuspecPath);
            StringBuilder builder = new();

            // Authors / Serviceable / Copyright are centralized — only emit overrides when the
            // value differs from the centralized default. Copyright is compared
            // whitespace-insensitively so the four packages with double-space variants are
            // treated as equivalent and not re-emitted per-csproj.
            string authors = nuspecReader.GetAuthors();
            if (!string.IsNullOrEmpty(authors) && authors != CentralizedAuthors)
                AppendProperty(builder, "Authors", authors);

            string? serviceable = ReadMetadataValue(nuspecReader, "serviceable");
            if (!string.IsNullOrEmpty(serviceable))
            {
                // For ref packages, Serviceable=true is centralized; only emit when value differs.
                // For text packages and others, Directory.Build.props doesn't centralize Serviceable,
                // so always emit when the source nuspec declares it.
                bool isRefPackage = string.Equals(PackageType, "ref", StringComparison.OrdinalIgnoreCase);
                if (!isRefPackage ||
                    !string.Equals(serviceable, CentralizedServiceable, StringComparison.OrdinalIgnoreCase))
                {
                    AppendProperty(builder, "Serviceable", serviceable);
                }
            }

            string copyright = nuspecReader.GetCopyright();
            if (!string.IsNullOrEmpty(copyright) &&
                !NormalizeWhitespace(copyright).Equals(NormalizeWhitespace(CentralizedCopyright), StringComparison.Ordinal))
            {
                AppendProperty(builder, "Copyright", copyright);
            }

            // Per-package metadata — emit whatever the source nuspec contains.
            string description = nuspecReader.GetDescription();
            if (!string.IsNullOrEmpty(description))
                AppendProperty(builder, "Description", description);

            // Note: <summary> and <language> are intentionally not emitted — NuGet's
            // PackTask doesn't expose them as MSBuild properties (both are deprecated).

            string title = nuspecReader.GetTitle();
            if (!string.IsNullOrEmpty(title))
                AppendProperty(builder, "Title", title);

            string projectUrl = nuspecReader.GetProjectUrl();
            if (!string.IsNullOrEmpty(projectUrl))
                AppendProperty(builder, "PackageProjectUrl", projectUrl);

            string licenseUrl = nuspecReader.GetLicenseUrl();
            // <license type="expression"> / <license type="file"> / PackageLicenseFile
            // are mutually exclusive with PackageLicenseUrl (NU5035). When the source
            // nuspec carries a structured <license> element, prefer that and drop the
            // legacy <licenseUrl>; MSBuild Pack auto-fills the resulting nuspec's
            // <licenseUrl> from the structured element on its own.
            //
            // For older packages that only ship a <licenseUrl> pointing to corefx's
            // LICENSE.TXT (which is MIT) or one of the deprecated fwlink URLs we know to
            // be MIT, upgrade to a structured MIT expression. Arcade's Workarounds.targets
            // requires every packable project to have a structured license; without this
            // upgrade 9 packages would fail to pack.
            LicenseMetadata? licenseMetadata = nuspecReader.GetLicenseMetadata();
            if (licenseMetadata is not null)
            {
                switch (licenseMetadata.Type)
                {
                    case LicenseType.Expression:
                        AppendProperty(builder, "PackageLicenseExpression", licenseMetadata.License);
                        break;
                    case LicenseType.File:
                        AppendProperty(builder, "PackageLicenseFile", licenseMetadata.License);
                        // Arcade ProjectDefaults.props sets PackageLicenseExpression=MIT by default;
                        // clear it to avoid NU5033 (cannot specify both PackageLicenseExpression and
                        // PackageLicenseFile).
                        AppendProperty(builder, "PackageLicenseExpression", string.Empty);
                        break;
                }
            }
            else if (!string.IsNullOrEmpty(licenseUrl))
            {
                if (IsKnownMitLicenseUrl(licenseUrl))
                {
                    AppendProperty(builder, "PackageLicenseExpression", "MIT");
                }
                else
                {
                    AppendProperty(builder, "PackageLicenseUrl", RewriteLicenseUrl(licenseUrl));
                    AppendProperty(builder, "PackageLicenseExpression", string.Empty);
                }
            }

            // <iconUrl>: NuGet has deprecated this in favor of <icon>; <icon> in turn is dropped
            // for SBRP outputs (see GetPackageItems / PackageContentToCopy). Skip emission so
            // produced packages don't carry a deprecated property + don't trigger NU5048.

            string releaseNotes = nuspecReader.GetReleaseNotes();
            if (!string.IsNullOrEmpty(releaseNotes))
                AppendProperty(builder, "PackageReleaseNotes", releaseNotes);

            string tags = nuspecReader.GetTags();
            if (!string.IsNullOrEmpty(tags))
                AppendProperty(builder, "PackageTags", tags);

            // <requireLicenseAcceptance> is omitted-by-default in the nuspec; only emit when present
            // and only when it differs from the centralized default (false). NuGet's default is also
            // false but Arcade's ProjectDefaults.props overrides to true, so the centralized default in
            // src/referencePackages/Directory.Build.props re-asserts false.
            string? requireLicenseAcceptance = ReadMetadataValue(nuspecReader, "requireLicenseAcceptance");
            if (!string.IsNullOrEmpty(requireLicenseAcceptance) &&
                !string.Equals(requireLicenseAcceptance, "false", StringComparison.OrdinalIgnoreCase))
            {
                AppendProperty(builder, "PackageRequireLicenseAcceptance", requireLicenseAcceptance);
            }

            // <repository type="git" url="..." commit="..." /> is intentionally NOT emitted.
            // Arcade fills in RepositoryUrl/RepositoryType/RepositoryCommit from the current
            // build's git context, which is the desired behavior — the produced reference
            // package's <repository> reflects the SBRP commit that produced it, not the
            // upstream commit of the original NuGet package.

            // Note: <owners> is intentionally not emitted — NuGet has deprecated it and
            // there is no MSBuild Pack property that maps to it.

            // <icon>file</icon> bundles a file in the package. We intentionally do NOT translate
            // it to <PackageIcon>: the historical RewriteNuspec behavior was to strip <icon> for
            // ref/text packages, and unifying on "no PackageIcon" for target packs too keeps the
            // generator simple. The icon file itself is still packaged for target packs because
            // BuildPackagingItems globs all on-disk files.

            // <readme>file</readme> bundles a readme in the package. We intentionally do NOT
            // translate it to <PackageReadmeFile>: this matches the historical RewriteNuspec
            // behavior of stripping <readme> for ref/text packages and keeps the generator
            // uniform across all package types. Any readme file on disk is still bundled in
            // text/target packages because BuildPackagingItems globs all on-disk files.

            // <packageTypes><packageType name="..." /></packageTypes>. NuGet supports a
            // semicolon-separated <PackageType> property: "Name1;Name2/Version2".
            string packageTypesValue = GetPackageTypesProperty(nuspecReader);
            if (!string.IsNullOrEmpty(packageTypesValue))
                AppendProperty(builder, "PackageType", packageTypesValue);

            // minClientVersion attribute on <metadata>.
            string? minClientVersion = TryGetMinClientVersion(nuspecReader);
            if (!string.IsNullOrEmpty(minClientVersion))
                AppendProperty(builder, "MinClientVersion", minClientVersion);

            return builder.ToString();
        }

        private static string GetPackageTypesProperty(NuspecReader nuspecReader)
        {
            // PackageTypes is exposed via NuspecCoreReaderBase.GetPackageTypes() in NuGet.Packaging.
            IReadOnlyList<global::NuGet.Packaging.Core.PackageType> types = nuspecReader.GetPackageTypes();
            if (types is null || types.Count == 0)
                return string.Empty;

            // Skip the default "Dependency" type; NuGet implies it.
            List<string> emitted = new();
            foreach (global::NuGet.Packaging.Core.PackageType type in types)
            {
                if (string.Equals(type.Name, "Dependency", StringComparison.OrdinalIgnoreCase))
                    continue;

                string entry = type.Name;
                if (type.Version is not null && type.Version != global::NuGet.Packaging.Core.PackageType.EmptyVersion)
                    entry += "/" + type.Version;
                emitted.Add(entry);
            }
            return string.Join(";", emitted);
        }

        private static string? TryGetMinClientVersion(NuspecReader nuspecReader)
        {
            try
            {
                global::NuGet.Versioning.NuGetVersion? min = nuspecReader.GetMinClientVersion();
                return min?.ToString();
            }
            catch
            {
                return null;
            }
        }

        private string[] GetPlaceholderTfms() => PlaceholderFiles
            .Select(placeholderFile => placeholderFile.GetMetadata(SharedMetadata.TargetFrameworkMetadataName))
            .Where(tfm => !string.IsNullOrEmpty(tfm))
            .Distinct()
            .Order()
            .ToArray();

        private string BuildPlaceholderInPropertyGroup()
        {
            string[] placeholderTfms = GetPlaceholderTfms();
            if (placeholderTfms.Length == 0)
                return string.Empty;

            // <PlaceholderTargetFrameworks> is read by src/referencePackages/Directory.Build.targets
            // to (a) strip these TFMs from `_TargetFrameworkInfo` in the cross-targeting outer build
            // so referencing projects never resolve a placeholder via FrameworkReducer, and (b) gate
            // the eng/placeholderpackaging.targets import on per-TFM inner builds (which no-ops
            // CoreCompile and contributes `lib/<tfm>/_._` to the produced package).
            StringBuilder builder = new();
            builder.Append(Environment.NewLine)
                   .Append("    <PlaceholderTargetFrameworks>")
                   .Append(string.Join(';', placeholderTfms))
                   .Append("</PlaceholderTargetFrameworks>");
            return builder.ToString();
        }

        private static void AppendProperty(StringBuilder builder, string name, string value)
        {
            builder.Append(Environment.NewLine)
                   .Append("    <")
                   .Append(name)
                   .Append('>')
                   .Append(EscapeXml(value))
                   .Append("</")
                   .Append(name)
                   .Append('>');
        }

        private static string EscapeXml(string value) =>
            new XmlDocument().CreateTextNode(value).OuterXml;

        private static string NormalizeWhitespace(string value)
        {
            StringBuilder normalized = new(value.Length);
            bool lastWasSpace = false;
            foreach (char c in value)
            {
                if (char.IsWhiteSpace(c))
                {
                    if (!lastWasSpace)
                    {
                        normalized.Append(' ');
                        lastWasSpace = true;
                    }
                }
                else
                {
                    normalized.Append(c);
                    lastWasSpace = false;
                }
            }
            return normalized.ToString().Trim();
        }

        private static string RewriteLicenseUrl(string licenseUrl)
        {
            foreach ((string from, string to) in LicenseUrlRewrites)
            {
                if (string.Equals(licenseUrl, from, StringComparison.OrdinalIgnoreCase))
                    return to;
            }
            return licenseUrl;
        }

        // URLs known to point to MIT-licensed projects. Older legacy packages without a
        // structured <license> element get upgraded to <license type="expression">MIT</license>.
        private static readonly string[] s_knownMitLicenseUrls =
        [
            "https://github.com/dotnet/corefx/blob/master/LICENSE.TXT",
            "https://github.com/dotnet/standard/blob/master/LICENSE.TXT",
            "https://github.com/dotnet/core-setup/blob/master/LICENSE.TXT",
            "http://go.microsoft.com/fwlink/?LinkId=529443",
            "http://go.microsoft.com/fwlink/?LinkId=329770",
        ];

        private static bool IsKnownMitLicenseUrl(string licenseUrl) =>
            s_knownMitLicenseUrls.Any(u => string.Equals(u, licenseUrl, StringComparison.OrdinalIgnoreCase));

        // NuspecReader exposes most fields via dedicated accessors but not arbitrary
        // metadata elements. Read them directly from the underlying XDocument.
        private static string? ReadMetadataValue(NuspecReader nuspecReader, string elementName) =>
            nuspecReader.GetMetadataValue(elementName);
    }
}

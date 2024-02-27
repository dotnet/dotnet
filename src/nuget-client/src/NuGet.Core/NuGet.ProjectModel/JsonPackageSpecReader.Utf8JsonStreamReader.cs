// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Packaging.Core;
using NuGet.RuntimeModel;
using NuGet.Versioning;

namespace NuGet.ProjectModel
{
    public partial class JsonPackageSpecReader
    {
        private static readonly byte[] AuthorsPropertyName = Encoding.UTF8.GetBytes("authors");
        private static readonly byte[] BuildOptionsPropertyName = Encoding.UTF8.GetBytes("buildOptions");
        private static readonly byte[] ContentFilesPropertyName = Encoding.UTF8.GetBytes("contentFiles");
        private static readonly byte[] CopyrightPropertyName = Encoding.UTF8.GetBytes("copyright");
        private static readonly byte[] DependenciesPropertyName = Encoding.UTF8.GetBytes("dependencies");
        private static readonly byte[] DescriptionPropertyName = Encoding.UTF8.GetBytes("description");
        private static readonly byte[] LanguagePropertyName = Encoding.UTF8.GetBytes("language");
        private static readonly byte[] PackIncludePropertyName = Encoding.UTF8.GetBytes("packInclude");
        private static readonly byte[] PackOptionsPropertyName = Encoding.UTF8.GetBytes("packOptions");
        private static readonly byte[] ScriptsPropertyName = Encoding.UTF8.GetBytes("scripts");
        private static readonly byte[] FrameworksPropertyName = Encoding.UTF8.GetBytes("frameworks");
        private static readonly byte[] RestorePropertyName = Encoding.UTF8.GetBytes("restore");
        private static readonly byte[] RuntimesPropertyName = Encoding.UTF8.GetBytes("runtimes");
        private static readonly byte[] SupportsPropertyName = Encoding.UTF8.GetBytes("supports");
        private static readonly byte[] TitlePropertyName = Encoding.UTF8.GetBytes("title");
        private static readonly byte[] VersionPropertyName = Encoding.UTF8.GetBytes("version");
        private static readonly byte[] OutputNamePropertyName = Encoding.UTF8.GetBytes("outputName");
        private static readonly byte[] AutoReferencedPropertyName = Encoding.UTF8.GetBytes("autoReferenced");
        private static readonly byte[] ExcludePropertyName = Encoding.UTF8.GetBytes("exclude");
        private static readonly byte[] GeneratePathPropertyPropertyName = Encoding.UTF8.GetBytes("generatePathProperty");
        private static readonly byte[] IncludePropertyName = Encoding.UTF8.GetBytes("include");
        private static readonly byte[] NoWarnPropertyName = Encoding.UTF8.GetBytes("noWarn");
        private static readonly byte[] SuppressParentPropertyName = Encoding.UTF8.GetBytes("suppressParent");
        private static readonly byte[] TargetPropertyName = Encoding.UTF8.GetBytes("target");
        private static readonly byte[] VersionOverridePropertyName = Encoding.UTF8.GetBytes("versionOverride");
        private static readonly byte[] VersionCentrallyManagedPropertyName = Encoding.UTF8.GetBytes("versionCentrallyManaged");
        private static readonly byte[] AliasesPropertyName = Encoding.UTF8.GetBytes("aliases");
        private static readonly byte[] NamePropertyName = Encoding.UTF8.GetBytes("name");
        private static readonly byte[] PrivateAssetsPropertyName = Encoding.UTF8.GetBytes("privateAssets");
        private static readonly byte[] ExcludeFilesPropertyName = Encoding.UTF8.GetBytes("excludeFiles");
        private static readonly byte[] IncludeFilesPropertyName = Encoding.UTF8.GetBytes("includeFiles");
        private static readonly byte[] CentralPackageVersionsManagementEnabledPropertyName = Encoding.UTF8.GetBytes("centralPackageVersionsManagementEnabled");
        private static readonly byte[] CentralPackageVersionOverrideDisabledPropertyName = Encoding.UTF8.GetBytes("centralPackageVersionOverrideDisabled");
        private static readonly byte[] CentralPackageTransitivePinningEnabledPropertyName = Encoding.UTF8.GetBytes("CentralPackageTransitivePinningEnabled");
        private static readonly byte[] ConfigFilePathsPropertyName = Encoding.UTF8.GetBytes("configFilePaths");
        private static readonly byte[] CrossTargetingPropertyName = Encoding.UTF8.GetBytes("crossTargeting");
        private static readonly byte[] FallbackFoldersPropertyName = Encoding.UTF8.GetBytes("fallbackFolders");
        private static readonly byte[] FilesPropertyName = Encoding.UTF8.GetBytes("files");
        private static readonly byte[] LegacyPackagesDirectoryPropertyName = Encoding.UTF8.GetBytes("legacyPackagesDirectory");
        private static readonly byte[] OriginalTargetFrameworksPropertyName = Encoding.UTF8.GetBytes("originalTargetFrameworks");
        private static readonly byte[] OutputPathPropertyName = Encoding.UTF8.GetBytes("outputPath");
        private static readonly byte[] PackagesConfigPathPropertyName = Encoding.UTF8.GetBytes("packagesConfigPath");
        private static readonly byte[] PackagesPathPropertyName = Encoding.UTF8.GetBytes("packagesPath");
        private static readonly byte[] ProjectJsonPathPropertyName = Encoding.UTF8.GetBytes("projectJsonPath");
        private static readonly byte[] ProjectNamePropertyName = Encoding.UTF8.GetBytes("projectName");
        private static readonly byte[] ProjectPathPropertyName = Encoding.UTF8.GetBytes("projectPath");
        private static readonly byte[] ProjectStylePropertyName = Encoding.UTF8.GetBytes("projectStyle");
        private static readonly byte[] ProjectUniqueNamePropertyName = Encoding.UTF8.GetBytes("projectUniqueName");
        private static readonly byte[] RestoreLockPropertiesPropertyName = Encoding.UTF8.GetBytes("restoreLockProperties");
        private static readonly byte[] NuGetLockFilePathPropertyName = Encoding.UTF8.GetBytes("nuGetLockFilePath");
        private static readonly byte[] RestoreLockedModePropertyName = Encoding.UTF8.GetBytes("restoreLockedMode");
        private static readonly byte[] RestorePackagesWithLockFilePropertyName = Encoding.UTF8.GetBytes("restorePackagesWithLockFile");
        private static readonly byte[] RestoreAuditPropertiesPropertyName = Encoding.UTF8.GetBytes("restoreAuditProperties");
        private static readonly byte[] EnableAuditPropertyName = Encoding.UTF8.GetBytes("enableAudit");
        private static readonly byte[] AuditLevelPropertyName = Encoding.UTF8.GetBytes("auditLevel");
        private static readonly byte[] AuditModePropertyName = Encoding.UTF8.GetBytes("auditMode");
        private static readonly byte[] SkipContentFileWritePropertyName = Encoding.UTF8.GetBytes("skipContentFileWrite");
        private static readonly byte[] SourcesPropertyName = Encoding.UTF8.GetBytes("sources");
        private static readonly byte[] ValidateRuntimeAssetsPropertyName = Encoding.UTF8.GetBytes("validateRuntimeAssets");
        private static readonly byte[] WarningPropertiesPropertyName = Encoding.UTF8.GetBytes("warningProperties");
        private static readonly byte[] AllWarningsAsErrorsPropertyName = Encoding.UTF8.GetBytes("allWarningsAsErrors");
        private static readonly byte[] WarnAsErrorPropertyName = Encoding.UTF8.GetBytes("warnAsError");
        private static readonly byte[] WarnNotAsErrorPropertyName = Encoding.UTF8.GetBytes("warnNotAsError");
        private static readonly byte[] ExcludeAssetsPropertyName = Encoding.UTF8.GetBytes("excludeAssets");
        private static readonly byte[] IncludeAssetsPropertyName = Encoding.UTF8.GetBytes("includeAssets");
        private static readonly byte[] TargetAliasPropertyName = Encoding.UTF8.GetBytes("targetAlias");
        private static readonly byte[] AssetTargetFallbackPropertyName = Encoding.UTF8.GetBytes("assetTargetFallback");
        private static readonly byte[] SecondaryFrameworkPropertyName = Encoding.UTF8.GetBytes("secondaryFramework");
        private static readonly byte[] CentralPackageVersionsPropertyName = Encoding.UTF8.GetBytes("centralPackageVersions");
        private static readonly byte[] DownloadDependenciesPropertyName = Encoding.UTF8.GetBytes("downloadDependencies");
        private static readonly byte[] FrameworkAssembliesPropertyName = Encoding.UTF8.GetBytes("frameworkAssemblies");
        private static readonly byte[] FrameworkReferencesPropertyName = Encoding.UTF8.GetBytes("frameworkReferences");
        private static readonly byte[] ImportsPropertyName = Encoding.UTF8.GetBytes("imports");
        private static readonly byte[] RuntimeIdentifierGraphPathPropertyName = Encoding.UTF8.GetBytes("runtimeIdentifierGraphPath");
        private static readonly byte[] WarnPropertyName = Encoding.UTF8.GetBytes("warn");
        private static readonly byte[] IconUrlPropertyName = Encoding.UTF8.GetBytes("iconUrl");
        private static readonly byte[] LicenseUrlPropertyName = Encoding.UTF8.GetBytes("licenseUrl");
        private static readonly byte[] OwnersPropertyName = Encoding.UTF8.GetBytes("owners");
        private static readonly byte[] PackageTypePropertyName = Encoding.UTF8.GetBytes("packageType");
        private static readonly byte[] ProjectUrlPropertyName = Encoding.UTF8.GetBytes("projectUrl");
        private static readonly byte[] ReleaseNotesPropertyName = Encoding.UTF8.GetBytes("releaseNotes");
        private static readonly byte[] RequireLicenseAcceptancePropertyName = Encoding.UTF8.GetBytes("requireLicenseAcceptance");
        private static readonly byte[] SummaryPropertyName = Encoding.UTF8.GetBytes("summary");
        private static readonly byte[] TagsPropertyName = Encoding.UTF8.GetBytes("tags");
        private static readonly byte[] MappingsPropertyName = Encoding.UTF8.GetBytes("mappings");
        private static readonly byte[] HashTagImportPropertyName = Encoding.UTF8.GetBytes("#import");
        private static readonly byte[] ProjectReferencesPropertyName = Encoding.UTF8.GetBytes("projectReferences");
        private static readonly byte[] EmptyStringPropertyName = Encoding.UTF8.GetBytes(string.Empty);

        internal static PackageSpec GetPackageSpecUtf8JsonStreamReader(Stream stream, string name, string packageSpecPath, IEnvironmentVariableReader environmentVariableReader, string snapshotValue = null)
        {
            var reader = new Utf8JsonStreamReader(stream);
            return GetPackageSpec(ref reader, name, packageSpecPath, environmentVariableReader, snapshotValue);
        }

        internal static PackageSpec GetPackageSpec(ref Utf8JsonStreamReader jsonReader, string name, string packageSpecPath, IEnvironmentVariableReader environmentVariableReader, string snapshotValue = null)
        {
            var packageSpec = new PackageSpec();

            List<CompatibilityProfile> compatibilityProfiles = null;
            List<RuntimeDescription> runtimeDescriptions = null;
            var wasPackOptionsSet = false;
            var isMappingsNull = false;
            string filePath = name == null ? null : Path.GetFullPath(packageSpecPath);

            if (jsonReader.TokenType == JsonTokenType.StartObject)
            {
                while (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.PropertyName)
                {
                    if (jsonReader.ValueTextEquals(EmptyStringPropertyName))
                    {
                        jsonReader.Skip();
                    }
#pragma warning disable CS0612 // Type or member is obsolete
                    else if (jsonReader.ValueTextEquals(AuthorsPropertyName))
                    {
                        jsonReader.Read();
                        if (jsonReader.TokenType == JsonTokenType.StartArray)
                        {
                            packageSpec.Authors = jsonReader.ReadStringArrayAsIList()?.ToArray();
                        }
                        packageSpec.Authors ??= [];
                    }
                    else if (jsonReader.ValueTextEquals(BuildOptionsPropertyName))
                    {
                        ReadBuildOptions(ref jsonReader, packageSpec);
                    }
                    else if (jsonReader.ValueTextEquals(ContentFilesPropertyName))
                    {
                        jsonReader.Read();
                        jsonReader.ReadStringArrayAsIList(packageSpec.ContentFiles);
                    }
                    else if (jsonReader.ValueTextEquals(CopyrightPropertyName))
                    {
                        packageSpec.Copyright = jsonReader.ReadNextTokenAsString();
                    }
                    else if (jsonReader.ValueTextEquals(DescriptionPropertyName))
                    {
                        packageSpec.Description = jsonReader.ReadNextTokenAsString();
                    }
                    else if (jsonReader.ValueTextEquals(LanguagePropertyName))
                    {
                        packageSpec.Language = jsonReader.ReadNextTokenAsString();
                    }
                    else if (jsonReader.ValueTextEquals(PackIncludePropertyName))
                    {
                        ReadPackInclude(ref jsonReader, packageSpec);
                    }
                    else if (jsonReader.ValueTextEquals(PackOptionsPropertyName))
                    {
                        ReadPackOptions(ref jsonReader, packageSpec, ref isMappingsNull);
                        wasPackOptionsSet = true;
                    }
                    else if (jsonReader.ValueTextEquals(ScriptsPropertyName))
                    {
                        ReadScripts(ref jsonReader, packageSpec);
                    }
#pragma warning restore CS0612 // Type or member is 
                    else if (jsonReader.ValueTextEquals(DependenciesPropertyName))
                    {
                        ReadDependencies(
                            ref jsonReader,
                            packageSpec.Dependencies,
                            filePath,
                            isGacOrFrameworkReference: false);
                    }
                    else if (jsonReader.ValueTextEquals(FrameworksPropertyName))
                    {
                        ReadFrameworks(ref jsonReader, packageSpec);
                    }
                    else if (jsonReader.ValueTextEquals(RestorePropertyName))
                    {
                        ReadMSBuildMetadata(ref jsonReader, packageSpec, environmentVariableReader);
                    }
                    else if (jsonReader.ValueTextEquals(RuntimesPropertyName))
                    {
                        runtimeDescriptions = ReadRuntimes(ref jsonReader);
                    }
                    else if (jsonReader.ValueTextEquals(SupportsPropertyName))
                    {
                        compatibilityProfiles = ReadSupports(ref jsonReader);
                    }
                    else if (jsonReader.ValueTextEquals(TitlePropertyName))
                    {
                        packageSpec.Title = jsonReader.ReadNextTokenAsString();
                    }
                    else if (jsonReader.ValueTextEquals(VersionPropertyName))
                    {
                        string version = jsonReader.ReadNextTokenAsString();
                        if (version != null)
                        {
                            try
                            {
#pragma warning disable CS0612 // Type or member is obsolete
                                packageSpec.HasVersionSnapshot = PackageSpecUtility.IsSnapshotVersion(version);
#pragma warning restore CS0612 // Type or member is obsolete
                                packageSpec.Version = PackageSpecUtility.SpecifySnapshot(version, snapshotValue);
                            }
                            catch (Exception ex)
                            {
                                throw FileFormatException.Create(ex, version, packageSpec.FilePath);
                            }
                        }
                    }
                    else
                    {
                        jsonReader.Skip();
                    }
                }
            }
            packageSpec.Name = name;
            packageSpec.FilePath = name == null ? null : Path.GetFullPath(packageSpecPath);

#pragma warning disable CS0612 // Type or member is obsolete
            if (!wasPackOptionsSet)
            {
                packageSpec.Owners = [];
                packageSpec.PackOptions = new PackOptions()
                {
                    PackageType = Array.Empty<PackageType>()
                };
                packageSpec.Tags = [];
            }

            if (isMappingsNull)
            {
                packageSpec.PackOptions.Mappings = null;
            }
#pragma warning restore CS0612 // Type or member is obsolete

            packageSpec.RuntimeGraph = new RuntimeGraph(
                runtimeDescriptions ?? Enumerable.Empty<RuntimeDescription>(),
                compatibilityProfiles ?? Enumerable.Empty<CompatibilityProfile>());

            packageSpec.Name ??= packageSpec.RestoreMetadata?.ProjectName;

            // Use the project.json path if one is set, otherwise use the project path
            packageSpec.FilePath ??= packageSpec.RestoreMetadata?.ProjectJsonPath
                    ?? packageSpec.RestoreMetadata?.ProjectPath;

            return packageSpec;
        }

        internal static void ReadCentralTransitiveDependencyGroup(
            ref Utf8JsonStreamReader jsonReader,
            out IList<LibraryDependency> results,
            string packageSpecPath)
        {
            results = null;
            if (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.StartObject)
            {
                while (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.PropertyName)
                {
                    var libraryName = jsonReader.GetString();
                    if (string.IsNullOrEmpty(libraryName))
                    {
                        throw FileFormatException.Create(
                            "Unable to resolve dependency ''.",
                            packageSpecPath);
                    }

                    if (jsonReader.Read())
                    {
                        var libraryDependency = ReadLibraryDependency(ref jsonReader, packageSpecPath, libraryName);
                        results ??= [];
                        results.Add(libraryDependency);
                    }
                }
            }
            results ??= Array.Empty<LibraryDependency>();
        }

        private static LibraryDependency ReadLibraryDependency(ref Utf8JsonStreamReader jsonReader, string packageSpecPath, string libraryName)
        {
            var dependencyIncludeFlagsValue = LibraryIncludeFlags.All;
            var dependencyExcludeFlagsValue = LibraryIncludeFlags.None;
            var suppressParentFlagsValue = LibraryIncludeFlagUtils.DefaultSuppressParent;
            string dependencyVersionValue = null;

            if (jsonReader.TokenType == JsonTokenType.String)
            {
                dependencyVersionValue = jsonReader.GetString();
            }
            else if (jsonReader.TokenType == JsonTokenType.StartObject)
            {
                ReadCentralTransitiveDependencyGroupProperties(
                    ref jsonReader,
                    ref dependencyIncludeFlagsValue,
                    ref dependencyExcludeFlagsValue,
                    ref suppressParentFlagsValue,
                    ref dependencyVersionValue);
            }

            VersionRange dependencyVersionRange = null;

            if (!string.IsNullOrEmpty(dependencyVersionValue))
            {
                try
                {
                    dependencyVersionRange = VersionRange.Parse(dependencyVersionValue);
                }
                catch (Exception ex)
                {
                    throw FileFormatException.Create(
                        ex,
                        packageSpecPath);
                }
            }

            if (dependencyVersionRange == null)
            {
                throw FileFormatException.Create(
                    new ArgumentException(Strings.MissingVersionOnDependency),
                    packageSpecPath);
            }

            // the dependency flags are: Include flags - Exclude flags
            var includeFlags = dependencyIncludeFlagsValue & ~dependencyExcludeFlagsValue;
            var libraryDependency = new LibraryDependency()
            {
                LibraryRange = new LibraryRange()
                {
                    Name = libraryName,
                    TypeConstraint = LibraryDependencyTarget.Package,
                    VersionRange = dependencyVersionRange
                },

                IncludeType = includeFlags,
                SuppressParent = suppressParentFlagsValue,
                VersionCentrallyManaged = true,
                ReferenceType = LibraryDependencyReferenceType.Transitive
            };

            return libraryDependency;
        }

        private static void ReadCentralTransitiveDependencyGroupProperties(
            ref Utf8JsonStreamReader jsonReader,
            ref LibraryIncludeFlags dependencyIncludeFlagsValue,
            ref LibraryIncludeFlags dependencyExcludeFlagsValue,
            ref LibraryIncludeFlags suppressParentFlagsValue,
            ref string dependencyVersionValue)
        {
            while (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.PropertyName)
            {
                if (jsonReader.ValueTextEquals(ExcludePropertyName))
                {
                    var values = jsonReader.ReadDelimitedString();
                    dependencyExcludeFlagsValue = LibraryIncludeFlagUtils.GetFlags(values);
                }
                else if (jsonReader.ValueTextEquals(IncludePropertyName))
                {
                    var values = jsonReader.ReadDelimitedString();
                    dependencyIncludeFlagsValue = LibraryIncludeFlagUtils.GetFlags(values);
                }
                else if (jsonReader.ValueTextEquals(SuppressParentPropertyName))
                {
                    var values = jsonReader.ReadDelimitedString();
                    suppressParentFlagsValue = LibraryIncludeFlagUtils.GetFlags(values);
                }
                else if (jsonReader.ValueTextEquals(VersionPropertyName))
                {
                    if (jsonReader.Read())
                    {
                        dependencyVersionValue = jsonReader.GetString();
                    }
                }
                else
                {
                    jsonReader.Skip();
                }
            }
        }

        private static void ReadDependencies(
            ref Utf8JsonStreamReader jsonReader,
            IList<LibraryDependency> results,
            string packageSpecPath,
            bool isGacOrFrameworkReference)
        {
            if (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.StartObject)
            {
                while (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.PropertyName)
                {
                    var propertyName = jsonReader.GetString();
                    if (string.IsNullOrEmpty(propertyName))
                    {
                        throw FileFormatException.Create("Unable to resolve dependency ''.", packageSpecPath);
                    }

                    // Support
                    // "dependencies" : {
                    //    "Name" : "1.0"
                    // }

                    if (jsonReader.Read())
                    {
                        var dependencyIncludeFlagsValue = LibraryIncludeFlags.All;
                        var dependencyExcludeFlagsValue = LibraryIncludeFlags.None;
                        var suppressParentFlagsValue = LibraryIncludeFlagUtils.DefaultSuppressParent;
                        List<NuGetLogCode> noWarn = null;

                        // This method handles both the dependencies and framework assembly sections.
                        // Framework references should be limited to references.
                        // Dependencies should allow everything but framework references.
                        LibraryDependencyTarget targetFlagsValue = isGacOrFrameworkReference
                            ? LibraryDependencyTarget.Reference
                            : LibraryDependencyTarget.All & ~LibraryDependencyTarget.Reference;

                        var autoReferenced = false;
                        var generatePathProperty = false;
                        var versionCentrallyManaged = false;
                        string aliases = null;
                        string dependencyVersionValue = null;
                        VersionRange versionOverride = null;

                        if (jsonReader.TokenType == JsonTokenType.String)
                        {
                            dependencyVersionValue = jsonReader.GetString();
                        }
                        else if (jsonReader.TokenType == JsonTokenType.StartObject)
                        {
                            while (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.PropertyName)
                            {
                                if (jsonReader.ValueTextEquals(AutoReferencedPropertyName))
                                {
                                    autoReferenced = jsonReader.ReadNextTokenAsBoolOrFalse();
                                }
                                else if (jsonReader.ValueTextEquals(ExcludePropertyName))
                                {
                                    var values = jsonReader.ReadDelimitedString();
                                    dependencyExcludeFlagsValue = LibraryIncludeFlagUtils.GetFlags(values);
                                }
                                else if (jsonReader.ValueTextEquals(GeneratePathPropertyPropertyName))
                                {
                                    generatePathProperty = jsonReader.ReadNextTokenAsBoolOrFalse();
                                }
                                else if (jsonReader.ValueTextEquals(IncludePropertyName))
                                {
                                    var values = jsonReader.ReadDelimitedString();
                                    dependencyIncludeFlagsValue = LibraryIncludeFlagUtils.GetFlags(values);
                                }
                                else if (jsonReader.ValueTextEquals(NoWarnPropertyName))
                                {
                                    noWarn = ReadNuGetLogCodesList(ref jsonReader);
                                }
                                else if (jsonReader.ValueTextEquals(SuppressParentPropertyName))
                                {
                                    var values = jsonReader.ReadDelimitedString();
                                    suppressParentFlagsValue = LibraryIncludeFlagUtils.GetFlags(values);
                                }
                                else if (jsonReader.ValueTextEquals(TargetPropertyName))
                                {
                                    targetFlagsValue = ReadTarget(ref jsonReader, packageSpecPath, targetFlagsValue);
                                }
                                else if (jsonReader.ValueTextEquals(VersionPropertyName))
                                {
                                    if (jsonReader.Read())
                                    {
                                        dependencyVersionValue = jsonReader.GetString();
                                    }
                                }
                                else if (jsonReader.ValueTextEquals(VersionOverridePropertyName))
                                {
                                    if (jsonReader.Read())
                                    {
                                        var versionPropValue = jsonReader.GetString();
                                        try
                                        {
                                            versionOverride = VersionRange.Parse(versionPropValue);
                                        }
                                        catch (Exception ex)
                                        {
                                            throw FileFormatException.Create(ex, packageSpecPath);
                                        }
                                    }
                                }
                                else if (jsonReader.ValueTextEquals(VersionCentrallyManagedPropertyName))
                                {
                                    versionCentrallyManaged = jsonReader.ReadNextTokenAsBoolOrFalse();
                                }
                                else if (jsonReader.ValueTextEquals(AliasesPropertyName))
                                {
                                    aliases = jsonReader.ReadNextTokenAsString();
                                }
                                else
                                {
                                    jsonReader.Skip();
                                }
                            }
                        }

                        VersionRange dependencyVersionRange = null;

                        if (!string.IsNullOrEmpty(dependencyVersionValue))
                        {
                            try
                            {
                                dependencyVersionRange = VersionRange.Parse(dependencyVersionValue);
                            }
                            catch (Exception ex)
                            {
                                throw FileFormatException.Create(
                                    ex,
                                    packageSpecPath);
                            }
                        }

                        // Projects and References may have empty version ranges, Packages may not
                        if (dependencyVersionRange == null)
                        {
                            if ((targetFlagsValue & LibraryDependencyTarget.Package) == LibraryDependencyTarget.Package)
                            {
                                throw FileFormatException.Create(
                                new ArgumentException(Strings.MissingVersionOnDependency),
                                packageSpecPath);
                            }
                            else
                            {
                                // Projects and references with no version property allow all versions
                                dependencyVersionRange = VersionRange.All;
                            }
                        }

                        // the dependency flags are: Include flags - Exclude flags
                        var includeFlags = dependencyIncludeFlagsValue & ~dependencyExcludeFlagsValue;
                        var libraryDependency = new LibraryDependency()
                        {
                            LibraryRange = new LibraryRange()
                            {
                                Name = propertyName,
                                TypeConstraint = targetFlagsValue,
                                VersionRange = dependencyVersionRange
                            },
                            IncludeType = includeFlags,
                            SuppressParent = suppressParentFlagsValue,
                            AutoReferenced = autoReferenced,
                            GeneratePathProperty = generatePathProperty,
                            VersionCentrallyManaged = versionCentrallyManaged,
                            Aliases = aliases,
                            // The ReferenceType is not persisted to the assets file
                            // Default to LibraryDependencyReferenceType.Direct on Read
                            ReferenceType = LibraryDependencyReferenceType.Direct,
                            VersionOverride = versionOverride
                        };

                        if (noWarn != null)
                        {
                            libraryDependency.NoWarn = noWarn;
                        }

                        results.Add(libraryDependency);
                    }
                }
            }
        }

        private static PackageType CreatePackageType(ref Utf8JsonStreamReader jsonReader)
        {
            var name = jsonReader.GetString();

            return new PackageType(name, Packaging.Core.PackageType.EmptyVersion);
        }

        [Obsolete]
        private static void ReadBuildOptions(ref Utf8JsonStreamReader jsonReader, PackageSpec packageSpec)
        {
            packageSpec.BuildOptions = new BuildOptions();

            if (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.StartObject)
            {
                while (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.PropertyName)
                {
                    if (jsonReader.ValueTextEquals(OutputNamePropertyName))
                    {
                        packageSpec.BuildOptions.OutputName = jsonReader.ReadNextTokenAsString();
                    }
                    else
                    {
                        jsonReader.Skip();
                    }
                }
            }
        }

        private static void ReadCentralPackageVersions(
            ref Utf8JsonStreamReader jsonReader,
            IDictionary<string, CentralPackageVersion> centralPackageVersions,
            string filePath)
        {
            if (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.StartObject)
            {
                while (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.PropertyName)
                {
                    var propertyName = jsonReader.GetString();

                    if (string.IsNullOrEmpty(propertyName))
                    {
                        throw FileFormatException.Create("Unable to resolve central version ''.", filePath);
                    }

                    string version = jsonReader.ReadNextTokenAsString();

                    if (string.IsNullOrEmpty(version))
                    {
                        throw FileFormatException.Create("The version cannot be null or empty.", filePath);
                    }

                    centralPackageVersions[propertyName] = new CentralPackageVersion(propertyName, VersionRange.Parse(version));
                }
            }
        }

        private static CompatibilityProfile ReadCompatibilityProfile(ref Utf8JsonStreamReader jsonReader, string profileName)
        {
            List<FrameworkRuntimePair> sets = null;

            if (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.StartObject)
            {
                while (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.PropertyName)
                {
                    var propertyName = jsonReader.GetString();
                    sets ??= [];

                    IReadOnlyList<string> values = jsonReader.ReadNextStringOrArrayOfStringsAsReadOnlyList() ?? Array.Empty<string>();

                    IEnumerable<FrameworkRuntimePair> profiles = ReadCompatibilitySets(values, propertyName);

                    sets.AddRange(profiles);
                }
            }
            return new CompatibilityProfile(profileName, sets ?? Enumerable.Empty<FrameworkRuntimePair>());
        }

        private static IEnumerable<FrameworkRuntimePair> ReadCompatibilitySets(IReadOnlyList<string> values, string compatibilitySetName)
        {
            NuGetFramework framework = NuGetFramework.Parse(compatibilitySetName);

            foreach (string value in values)
            {
                yield return new FrameworkRuntimePair(framework, value);
            }
        }

        private static void ReadDownloadDependencies(
            ref Utf8JsonStreamReader jsonReader,
            IList<DownloadDependency> downloadDependencies,
            string packageSpecPath)
        {
            var seenIds = new HashSet<string>();

            if (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.StartArray)
            {
                do
                {
                    string name = null;
                    string versionValue = null;
                    var isNameDefined = false;

                    if (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.StartObject)
                    {
                        while (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.PropertyName)
                        {
                            if (jsonReader.ValueTextEquals(NamePropertyName))
                            {
                                isNameDefined = true;
                                name = jsonReader.ReadNextTokenAsString();
                            }
                            else if (jsonReader.ValueTextEquals(VersionPropertyName))
                            {
                                versionValue = jsonReader.ReadNextTokenAsString();
                            }
                            else
                            {
                                jsonReader.Skip();
                            }
                        }
                    }

                    if (jsonReader.TokenType == JsonTokenType.EndArray)
                    {
                        break;
                    }

                    if (!isNameDefined)
                    {
                        throw FileFormatException.Create(
                            "Unable to resolve downloadDependency ''.",
                            packageSpecPath);
                    }

                    if (!seenIds.Add(name))
                    {
                        // package ID already seen, only use first definition.
                        continue;
                    }

                    if (string.IsNullOrEmpty(versionValue))
                    {
                        throw FileFormatException.Create(
                            "The version cannot be null or empty",
                            packageSpecPath);
                    }

                    var versions = new LazyStringSplit(versionValue, VersionSeparator);

                    foreach (string singleVersionValue in versions)
                    {
                        if (string.IsNullOrEmpty(singleVersionValue))
                        {
                            continue;
                        }

                        try
                        {
                            VersionRange version = VersionRange.Parse(singleVersionValue);

                            downloadDependencies.Add(new DownloadDependency(name, version));
                        }
                        catch (Exception ex)
                        {
                            throw FileFormatException.Create(
                               ex,
                               packageSpecPath);
                        }
                    }
                } while (jsonReader.TokenType == JsonTokenType.EndObject);
            }
        }

        private static void ReadFrameworkReferences(
            ref Utf8JsonStreamReader jsonReader,
            ISet<FrameworkDependency> frameworkReferences,
            string packageSpecPath)
        {
            if (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.StartObject)
            {
                while (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.PropertyName)
                {
                    var frameworkName = jsonReader.GetString();
                    if (string.IsNullOrEmpty(frameworkName))
                    {
                        throw FileFormatException.Create(
                            "Unable to resolve frameworkReference.",
                            packageSpecPath);
                    }

                    var privateAssets = FrameworkDependencyFlagsUtils.Default;

                    if (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.StartObject)
                    {
                        while (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.PropertyName)
                        {
                            if (jsonReader.ValueTextEquals(PrivateAssetsPropertyName))
                            {
                                IEnumerable<string> strings = jsonReader.ReadDelimitedString();

                                privateAssets = FrameworkDependencyFlagsUtils.GetFlags(strings);
                            }
                            else
                            {
                                jsonReader.Skip();
                            }
                        }
                    }

                    frameworkReferences.Add(new FrameworkDependency(frameworkName, privateAssets));
                }
            }
        }

        private static void ReadFrameworks(ref Utf8JsonStreamReader reader, PackageSpec packageSpec)
        {
            if (reader.Read() && reader.TokenType == JsonTokenType.StartObject)
            {
                while (reader.Read() && reader.TokenType == JsonTokenType.PropertyName)
                {
                    try
                    {
                        ReadTargetFrameworks(packageSpec, ref reader);
                    }
                    catch (Exception ex)
                    {
                        throw FileFormatException.Create(ex, packageSpec.FilePath);
                    }
                }
            }
        }

        private static void ReadImports(PackageSpec packageSpec, ref Utf8JsonStreamReader jsonReader, TargetFrameworkInformation targetFrameworkInformation)
        {
            IReadOnlyList<string> imports = jsonReader.ReadNextStringOrArrayOfStringsAsReadOnlyList();

            if (imports != null && imports.Count > 0)
            {
                foreach (string import in imports.Where(element => !string.IsNullOrEmpty(element)))
                {
                    NuGetFramework framework = NuGetFramework.Parse(import);

                    if (!framework.IsSpecificFramework)
                    {
                        throw FileFormatException.Create(
                            string.Format(
                                CultureInfo.CurrentCulture,
                                Strings.Log_InvalidImportFramework,
                                import,
                                PackageSpec.PackageSpecFileName),
                            packageSpec.FilePath);
                    }

                    targetFrameworkInformation.Imports.Add(framework);
                }
            }
        }

        private static void ReadMappings(ref Utf8JsonStreamReader jsonReader, string mappingKey, IDictionary<string, IncludeExcludeFiles> mappings)
        {
            if (jsonReader.Read())
            {
                switch (jsonReader.TokenType)
                {
                    case JsonTokenType.String:
                        {
                            var files = new IncludeExcludeFiles()
                            {
                                Include = new[] { (string)jsonReader.GetString() }
                            };

                            mappings.Add(mappingKey, files);
                        }
                        break;
                    case JsonTokenType.StartArray:
                        {
                            IReadOnlyList<string> include = jsonReader.ReadStringArrayAsReadOnlyListFromArrayStart();

                            var files = new IncludeExcludeFiles()
                            {
                                Include = include
                            };

                            mappings.Add(mappingKey, files);
                        }
                        break;
                    case JsonTokenType.StartObject:
                        {
                            IReadOnlyList<string> excludeFiles = null;
                            IReadOnlyList<string> exclude = null;
                            IReadOnlyList<string> includeFiles = null;
                            IReadOnlyList<string> include = null;

                            while (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.PropertyName)
                            {
                                if (jsonReader.ValueTextEquals(ExcludeFilesPropertyName))
                                {
                                    excludeFiles = jsonReader.ReadNextStringOrArrayOfStringsAsReadOnlyList();
                                }
                                else if (jsonReader.ValueTextEquals(ExcludePropertyName))
                                {
                                    exclude = jsonReader.ReadNextStringOrArrayOfStringsAsReadOnlyList();
                                }
                                else if (jsonReader.ValueTextEquals(IncludeFilesPropertyName))
                                {
                                    includeFiles = jsonReader.ReadNextStringOrArrayOfStringsAsReadOnlyList();
                                }
                                else if (jsonReader.ValueTextEquals(IncludePropertyName))
                                {
                                    include = jsonReader.ReadNextStringOrArrayOfStringsAsReadOnlyList();
                                }
                                else
                                {
                                    jsonReader.Skip();
                                }
                            }

                            if (include != null || includeFiles != null || exclude != null || excludeFiles != null)
                            {
                                var files = new IncludeExcludeFiles()
                                {
                                    ExcludeFiles = excludeFiles,
                                    Exclude = exclude,
                                    IncludeFiles = includeFiles,
                                    Include = include
                                };

                                mappings.Add(mappingKey, files);
                            }
                        }
                        break;
                }
            }
        }

        private static void ReadMSBuildMetadata(ref Utf8JsonStreamReader jsonReader, PackageSpec packageSpec, IEnvironmentVariableReader environmentVariableReader)
        {
            var centralPackageVersionsManagementEnabled = false;
            var centralPackageVersionOverrideDisabled = false;
            var CentralPackageTransitivePinningEnabled = false;
            List<string> configFilePaths = null;
            var crossTargeting = false;
            List<string> fallbackFolders = null;
            List<ProjectRestoreMetadataFile> files = null;
            var legacyPackagesDirectory = false;
            List<string> originalTargetFrameworks = null;
            string outputPath = null;
            string packagesConfigPath = null;
            string packagesPath = null;
            string projectJsonPath = null;
            string projectName = null;
            string projectPath = null;
            ProjectStyle? projectStyle = null;
            string projectUniqueName = null;
            RestoreLockProperties restoreLockProperties = null;
            var skipContentFileWrite = false;
            List<PackageSource> sources = null;
            IList<ProjectRestoreMetadataFrameworkInfo> targetFrameworks = null;
            var validateRuntimeAssets = false;
            WarningProperties warningProperties = null;
            RestoreAuditProperties auditProperties = null;
            bool useMacros = MSBuildStringUtility.IsTrue(environmentVariableReader.GetEnvironmentVariable(MacroStringsUtility.NUGET_ENABLE_EXPERIMENTAL_MACROS));
            var userSettingsDirectory = NuGetEnvironment.GetFolderPath(NuGetFolderPath.UserSettingsDirectory);

            if (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.StartObject)
            {
                while (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.PropertyName)
                {
                    if (jsonReader.ValueTextEquals(CentralPackageVersionsManagementEnabledPropertyName))
                    {
                        centralPackageVersionsManagementEnabled = jsonReader.ReadNextTokenAsBoolOrFalse();
                    }
                    else if (jsonReader.ValueTextEquals(CentralPackageVersionOverrideDisabledPropertyName))
                    {
                        centralPackageVersionOverrideDisabled = jsonReader.ReadNextTokenAsBoolOrFalse();
                    }
                    else if (jsonReader.ValueTextEquals(CentralPackageTransitivePinningEnabledPropertyName))
                    {
                        CentralPackageTransitivePinningEnabled = jsonReader.ReadNextTokenAsBoolOrFalse();
                    }
                    else if (jsonReader.ValueTextEquals(ConfigFilePathsPropertyName))
                    {
                        jsonReader.Read();
                        configFilePaths = jsonReader.ReadStringArrayAsIList() as List<string>;
                        ExtractMacros(configFilePaths, userSettingsDirectory, useMacros);
                    }
                    else if (jsonReader.ValueTextEquals(CrossTargetingPropertyName))
                    {
                        crossTargeting = jsonReader.ReadNextTokenAsBoolOrFalse();
                    }
                    else if (jsonReader.ValueTextEquals(FallbackFoldersPropertyName))
                    {
                        jsonReader.Read();
                        fallbackFolders = jsonReader.ReadStringArrayAsIList() as List<string>;
                        ExtractMacros(fallbackFolders, userSettingsDirectory, useMacros);
                    }
                    else if (jsonReader.ValueTextEquals(FilesPropertyName))
                    {
                        if (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.StartObject)
                        {
                            while (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.PropertyName)
                            {
                                var filePropertyName = jsonReader.GetString();
                                files ??= [];

                                files.Add(new ProjectRestoreMetadataFile(filePropertyName, jsonReader.ReadNextTokenAsString()));
                            }
                        }
                    }
                    else if (jsonReader.ValueTextEquals(FrameworksPropertyName))
                    {
                        targetFrameworks = ReadTargetFrameworks(ref jsonReader);
                    }
                    else if (jsonReader.ValueTextEquals(LegacyPackagesDirectoryPropertyName))
                    {
                        legacyPackagesDirectory = jsonReader.ReadNextTokenAsBoolOrFalse();
                    }
                    else if (jsonReader.ValueTextEquals(OriginalTargetFrameworksPropertyName))
                    {
                        jsonReader.Read();
                        originalTargetFrameworks = jsonReader.ReadStringArrayAsIList() as List<string>;
                    }
                    else if (jsonReader.ValueTextEquals(OutputPathPropertyName))
                    {
                        outputPath = jsonReader.ReadNextTokenAsString();
                    }
                    else if (jsonReader.ValueTextEquals(PackagesConfigPathPropertyName))
                    {
                        packagesConfigPath = jsonReader.ReadNextTokenAsString();
                    }
                    else if (jsonReader.ValueTextEquals(PackagesPathPropertyName))
                    {
                        packagesPath = ExtractMacro(jsonReader.ReadNextTokenAsString(), userSettingsDirectory, useMacros);
                    }
                    else if (jsonReader.ValueTextEquals(ProjectJsonPathPropertyName))
                    {
                        projectJsonPath = jsonReader.ReadNextTokenAsString();
                    }
                    else if (jsonReader.ValueTextEquals(ProjectNamePropertyName))
                    {
                        projectName = jsonReader.ReadNextTokenAsString();
                    }
                    else if (jsonReader.ValueTextEquals(ProjectPathPropertyName))
                    {
                        projectPath = jsonReader.ReadNextTokenAsString();
                    }
                    else if (jsonReader.ValueTextEquals(ProjectStylePropertyName))
                    {
                        string projectStyleString = jsonReader.ReadNextTokenAsString();

                        if (!string.IsNullOrEmpty(projectStyleString)
                            && Enum.TryParse(projectStyleString, ignoreCase: true, result: out ProjectStyle projectStyleValue))
                        {
                            projectStyle = projectStyleValue;
                        }
                    }
                    else if (jsonReader.ValueTextEquals(ProjectUniqueNamePropertyName))
                    {
                        projectUniqueName = ExtractMacro(jsonReader.ReadNextTokenAsString(), userSettingsDirectory, useMacros);
                    }
                    else if (jsonReader.ValueTextEquals(RestoreLockPropertiesPropertyName))
                    {
                        string nuGetLockFilePath = null;
                        var restoreLockedMode = false;
                        string restorePackagesWithLockFile = null;

                        if (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.StartObject)
                        {
                            while (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.PropertyName)
                            {
                                if (jsonReader.ValueTextEquals(NuGetLockFilePathPropertyName))
                                {
                                    nuGetLockFilePath = jsonReader.ReadNextTokenAsString();
                                }
                                else if (jsonReader.ValueTextEquals(RestoreLockedModePropertyName))
                                {
                                    restoreLockedMode = jsonReader.ReadNextTokenAsBoolOrFalse();
                                }
                                else if (jsonReader.ValueTextEquals(RestorePackagesWithLockFilePropertyName))
                                {
                                    restorePackagesWithLockFile = jsonReader.ReadNextTokenAsString();
                                }
                                else
                                {
                                    jsonReader.Skip();
                                }
                            }
                        }
                        restoreLockProperties = new RestoreLockProperties(restorePackagesWithLockFile, nuGetLockFilePath, restoreLockedMode);
                    }
                    else if (jsonReader.ValueTextEquals(RestoreAuditPropertiesPropertyName))
                    {
                        string enableAudit = null, auditLevel = null, auditMode = null;
                        if (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.StartObject)
                        {
                            while (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.PropertyName)
                            {
                                if (jsonReader.ValueTextEquals(EnableAuditPropertyName))
                                {
                                    enableAudit = jsonReader.ReadNextTokenAsString();
                                }
                                else if (jsonReader.ValueTextEquals(AuditLevelPropertyName))
                                {
                                    auditLevel = jsonReader.ReadNextTokenAsString();
                                }
                                else if (jsonReader.ValueTextEquals(AuditModePropertyName))
                                {
                                    auditMode = jsonReader.ReadNextTokenAsString();
                                }
                                else
                                {
                                    jsonReader.Skip();
                                }
                            }
                        }
                        auditProperties = new RestoreAuditProperties()
                        {
                            EnableAudit = enableAudit,
                            AuditLevel = auditLevel,
                            AuditMode = auditMode,
                        };
                    }
                    else if (jsonReader.ValueTextEquals(SkipContentFileWritePropertyName))
                    {
                        skipContentFileWrite = jsonReader.ReadNextTokenAsBoolOrFalse();
                    }
                    else if (jsonReader.ValueTextEquals(SourcesPropertyName))
                    {
                        if (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.StartObject)
                        {
                            while (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.PropertyName)
                            {
                                var sourcePropertyName = jsonReader.GetString();
                                sources ??= [];

                                sources.Add(new PackageSource(sourcePropertyName));
                                jsonReader.Skip();
                            }
                        }
                    }
                    else if (jsonReader.ValueTextEquals(ValidateRuntimeAssetsPropertyName))
                    {
                        validateRuntimeAssets = jsonReader.ReadNextTokenAsBoolOrFalse();
                    }
                    else if (jsonReader.ValueTextEquals(WarningPropertiesPropertyName))
                    {
                        var allWarningsAsErrors = false;
                        var noWarn = new HashSet<NuGetLogCode>();
                        var warnAsError = new HashSet<NuGetLogCode>();
                        var warningsNotAsErrors = new HashSet<NuGetLogCode>();

                        if (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.StartObject)
                        {
                            while (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.PropertyName)
                            {
                                if (jsonReader.ValueTextEquals(AllWarningsAsErrorsPropertyName))
                                {
                                    allWarningsAsErrors = jsonReader.ReadNextTokenAsBoolOrFalse();
                                }
                                else if (jsonReader.ValueTextEquals(NoWarnPropertyName))
                                {
                                    ReadNuGetLogCodes(ref jsonReader, noWarn);
                                }
                                else if (jsonReader.ValueTextEquals(WarnAsErrorPropertyName))
                                {
                                    ReadNuGetLogCodes(ref jsonReader, warnAsError);
                                }
                                else if (jsonReader.ValueTextEquals(WarnNotAsErrorPropertyName))
                                {
                                    ReadNuGetLogCodes(ref jsonReader, warningsNotAsErrors);
                                }
                                else
                                {
                                    jsonReader.Skip();
                                }
                            }
                        }

                        warningProperties = new WarningProperties(warnAsError, noWarn, allWarningsAsErrors, warningsNotAsErrors);
                    }
                    else
                    {
                        jsonReader.Skip();
                    }
                }
            }

            ProjectRestoreMetadata msbuildMetadata;
            if (projectStyle == ProjectStyle.PackagesConfig)
            {
                msbuildMetadata = new PackagesConfigProjectRestoreMetadata()
                {
                    PackagesConfigPath = packagesConfigPath
                };
            }
            else
            {
                msbuildMetadata = new ProjectRestoreMetadata();
            }

            msbuildMetadata.CentralPackageVersionsEnabled = centralPackageVersionsManagementEnabled;
            msbuildMetadata.CentralPackageVersionOverrideDisabled = centralPackageVersionOverrideDisabled;
            msbuildMetadata.CentralPackageTransitivePinningEnabled = CentralPackageTransitivePinningEnabled;
            msbuildMetadata.RestoreAuditProperties = auditProperties;

            if (configFilePaths != null)
            {
                msbuildMetadata.ConfigFilePaths = configFilePaths;
            }

            msbuildMetadata.CrossTargeting = crossTargeting;

            if (fallbackFolders != null)
            {
                msbuildMetadata.FallbackFolders = fallbackFolders;
            }

            if (files != null)
            {
                msbuildMetadata.Files = files;
            }

            msbuildMetadata.LegacyPackagesDirectory = legacyPackagesDirectory;

            if (originalTargetFrameworks != null)
            {
                msbuildMetadata.OriginalTargetFrameworks = originalTargetFrameworks;
            }

            msbuildMetadata.OutputPath = outputPath;
            msbuildMetadata.PackagesPath = packagesPath;
            msbuildMetadata.ProjectJsonPath = projectJsonPath;
            msbuildMetadata.ProjectName = projectName;
            msbuildMetadata.ProjectPath = projectPath;

            if (projectStyle.HasValue)
            {
                msbuildMetadata.ProjectStyle = projectStyle.Value;
            }

            msbuildMetadata.ProjectUniqueName = projectUniqueName;

            if (restoreLockProperties != null)
            {
                msbuildMetadata.RestoreLockProperties = restoreLockProperties;
            }

            msbuildMetadata.SkipContentFileWrite = skipContentFileWrite;

            if (sources != null)
            {
                msbuildMetadata.Sources = sources;
            }

            if (targetFrameworks != null)
            {
                msbuildMetadata.TargetFrameworks = targetFrameworks;
            }

            msbuildMetadata.ValidateRuntimeAssets = validateRuntimeAssets;

            if (warningProperties != null)
            {
                msbuildMetadata.ProjectWideWarningProperties = warningProperties;
            }

            packageSpec.RestoreMetadata = msbuildMetadata;
        }

        private static void ReadNuGetLogCodes(ref Utf8JsonStreamReader jsonReader, HashSet<NuGetLogCode> hashCodes)
        {
            if (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.StartArray)
            {
                while (jsonReader.Read() && jsonReader.TokenType != JsonTokenType.EndArray)
                {
                    if (jsonReader.TokenType == JsonTokenType.String && Enum.TryParse(jsonReader.GetString(), out NuGetLogCode code))
                    {
                        hashCodes.Add(code);
                    }
                }
            }
        }

        private static List<NuGetLogCode> ReadNuGetLogCodesList(ref Utf8JsonStreamReader jsonReader)
        {
            List<NuGetLogCode> items = null;

            if (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.StartArray)
            {
                while (jsonReader.Read() && jsonReader.TokenType != JsonTokenType.EndArray)
                {
                    if (jsonReader.TokenType == JsonTokenType.String && Enum.TryParse(jsonReader.GetString(), out NuGetLogCode code))
                    {
                        items ??= [];

                        items.Add(code);
                    }
                }
            }
            return items;
        }

        private static void ReadPackageTypes(PackageSpec packageSpec, ref Utf8JsonStreamReader jsonReader)
        {
            IReadOnlyList<PackageType> packageTypes = null;
            try
            {
                if (jsonReader.Read())
                {
                    PackageType packageType;
                    switch (jsonReader.TokenType)
                    {
                        case JsonTokenType.String:
                            packageType = CreatePackageType(ref jsonReader);
                            packageTypes = new[] { packageType };
                            break;
                        case JsonTokenType.StartArray:
                            List<PackageType> types = null;

                            while (jsonReader.Read() && jsonReader.TokenType != JsonTokenType.EndArray)
                            {
                                if (jsonReader.TokenType != JsonTokenType.String)
                                {
                                    throw FileFormatException.Create(
                                        string.Format(
                                            CultureInfo.CurrentCulture,
                                            Strings.InvalidPackageType,
                                            PackageSpec.PackageSpecFileName),
                                        packageSpec.FilePath);
                                }

                                packageType = CreatePackageType(ref jsonReader);
                                types ??= [];
                                types.Add(packageType);
                            }

                            packageTypes = types;
                            break;
                        case JsonTokenType.Null:
                            break;
                        default:
                            throw new InvalidCastException();
                    }

#pragma warning disable CS0612 // Type or member is obsolete
                    if (packageTypes != null)
                    {
                        packageSpec.PackOptions.PackageType = packageTypes;
                    }
#pragma warning restore CS0612 // Type or member is obsolete
                }
            }
            catch (Exception)
            {
                throw FileFormatException.Create(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.InvalidPackageType,
                        PackageSpec.PackageSpecFileName),
                    packageSpec.FilePath);
            }
        }

        [Obsolete]
        private static void ReadPackInclude(ref Utf8JsonStreamReader jsonReader, PackageSpec packageSpec)
        {
            if (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.StartObject)
            {
                while (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.PropertyName)
                {
                    string propertyName = jsonReader.GetString();
                    string propertyValue = jsonReader.ReadNextTokenAsString();

                    packageSpec.PackInclude.Add(new KeyValuePair<string, string>(propertyName, propertyValue));
                }
            }
        }

        [Obsolete]
        private static void ReadPackOptions(ref Utf8JsonStreamReader jsonReader, PackageSpec packageSpec, ref bool isMappingsNull)
        {
            var wasMappingsRead = false;
            bool isPackOptionsValueAnObject = false;

            if (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.StartObject)
            {
                isPackOptionsValueAnObject = true;
                while (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.PropertyName)
                {
                    if (jsonReader.ValueTextEquals(FilesPropertyName))
                    {
                        wasMappingsRead = ReadPackOptionsFiles(packageSpec, ref jsonReader, wasMappingsRead);
                    }
                    else if (jsonReader.ValueTextEquals(IconUrlPropertyName))
                    {
                        packageSpec.IconUrl = jsonReader.ReadNextTokenAsString();
                    }
                    else if (jsonReader.ValueTextEquals(LicenseUrlPropertyName))
                    {
                        packageSpec.LicenseUrl = jsonReader.ReadNextTokenAsString();
                    }
                    else if (jsonReader.ValueTextEquals(OwnersPropertyName))
                    {
                        jsonReader.Read();
                        string[] owners = jsonReader.ReadStringArrayAsIList()?.ToArray();
                        if (owners != null)
                        {
                            packageSpec.Owners = owners;
                        }
                    }
                    else if (jsonReader.ValueTextEquals(PackageTypePropertyName))
                    {
                        ReadPackageTypes(packageSpec, ref jsonReader);
                    }
                    else if (jsonReader.ValueTextEquals(ProjectUrlPropertyName))
                    {
                        packageSpec.ProjectUrl = jsonReader.ReadNextTokenAsString();
                    }
                    else if (jsonReader.ValueTextEquals(ReleaseNotesPropertyName))
                    {
                        packageSpec.ReleaseNotes = jsonReader.ReadNextTokenAsString();
                    }
                    else if (jsonReader.ValueTextEquals(RequireLicenseAcceptancePropertyName))
                    {
                        packageSpec.RequireLicenseAcceptance = jsonReader.ReadNextTokenAsBoolOrFalse();
                    }
                    else if (jsonReader.ValueTextEquals(SummaryPropertyName))
                    {
                        packageSpec.Summary = jsonReader.ReadNextTokenAsString();
                    }
                    else if (jsonReader.ValueTextEquals(TagsPropertyName))
                    {
                        jsonReader.Read();
                        string[] tags = jsonReader.ReadStringArrayAsIList()?.ToArray();

                        if (tags != null)
                        {
                            packageSpec.Tags = tags;
                        }
                    }
                    else
                    {
                        jsonReader.Skip();
                    }
                }
            }
            isMappingsNull = isPackOptionsValueAnObject && !wasMappingsRead;
        }

        [Obsolete]
        private static bool ReadPackOptionsFiles(PackageSpec packageSpec, ref Utf8JsonStreamReader jsonReader, bool wasMappingsRead)
        {
            IReadOnlyList<string> excludeFiles = null;
            IReadOnlyList<string> exclude = null;
            IReadOnlyList<string> includeFiles = null;
            IReadOnlyList<string> include = null;

            if (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.StartObject)
            {
                while (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.PropertyName)
                {
                    if (jsonReader.ValueTextEquals(ExcludeFilesPropertyName))
                    {
                        excludeFiles = jsonReader.ReadNextStringOrArrayOfStringsAsReadOnlyList();
                    }
                    else if (jsonReader.ValueTextEquals(ExcludePropertyName))
                    {
                        exclude = jsonReader.ReadNextStringOrArrayOfStringsAsReadOnlyList();
                    }
                    else if (jsonReader.ValueTextEquals(IncludeFilesPropertyName))
                    {
                        includeFiles = jsonReader.ReadNextStringOrArrayOfStringsAsReadOnlyList();
                    }
                    else if (jsonReader.ValueTextEquals(IncludePropertyName))
                    {
                        include = jsonReader.ReadNextStringOrArrayOfStringsAsReadOnlyList();
                    }
                    else if (jsonReader.ValueTextEquals(MappingsPropertyName))
                    {
                        if (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.StartObject)
                        {
                            while (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.PropertyName)
                            {
                                wasMappingsRead = true;
                                var mappingsPropertyName = jsonReader.GetString();
                                ReadMappings(ref jsonReader, mappingsPropertyName, packageSpec.PackOptions.Mappings);
                            }
                        }
                    }
                    else
                    {
                        jsonReader.Skip();
                    }
                }
            }

            if (include != null || includeFiles != null || exclude != null || excludeFiles != null)
            {
                packageSpec.PackOptions.IncludeExcludeFiles = new IncludeExcludeFiles()
                {
                    ExcludeFiles = excludeFiles,
                    Exclude = exclude,
                    IncludeFiles = includeFiles,
                    Include = include
                };
            }

            return wasMappingsRead;
        }

        private static RuntimeDependencySet ReadRuntimeDependencySet(ref Utf8JsonStreamReader jsonReader, string dependencySetName)
        {
            List<RuntimePackageDependency> dependencies = null;

            if (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.StartObject)
            {
                while (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.PropertyName)
                {
                    var propertyName = jsonReader.GetString();
                    dependencies ??= [];

                    var dependency = new RuntimePackageDependency(propertyName, VersionRange.Parse(jsonReader.ReadNextTokenAsString()));

                    dependencies.Add(dependency);
                }
            }

            return new RuntimeDependencySet(
                dependencySetName,
                dependencies);
        }

        private static RuntimeDescription ReadRuntimeDescription(ref Utf8JsonStreamReader jsonReader, string runtimeName)
        {
            List<string> inheritedRuntimes = null;
            List<RuntimeDependencySet> additionalDependencies = null;

            if (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.StartObject)
            {
                while (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.PropertyName)
                {
                    if (jsonReader.ValueTextEquals(HashTagImportPropertyName))
                    {
                        jsonReader.Read();
                        inheritedRuntimes = jsonReader.ReadStringArrayAsIList() as List<string>;
                    }
                    else
                    {
                        var propertyName = jsonReader.GetString();
                        additionalDependencies ??= [];

                        RuntimeDependencySet dependency = ReadRuntimeDependencySet(ref jsonReader, propertyName);

                        additionalDependencies.Add(dependency);
                    }
                }
            }

            return new RuntimeDescription(
                runtimeName,
                inheritedRuntimes,
                additionalDependencies);
        }

        private static List<RuntimeDescription> ReadRuntimes(ref Utf8JsonStreamReader jsonReader)
        {
            List<RuntimeDescription> runtimeDescriptions = null;

            if (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.StartObject)
            {
                while (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.PropertyName)
                {
                    RuntimeDescription runtimeDescription = ReadRuntimeDescription(ref jsonReader, jsonReader.GetString());
                    runtimeDescriptions ??= [];
                    runtimeDescriptions.Add(runtimeDescription);
                }
            }

            return runtimeDescriptions;
        }

        [Obsolete]
        private static void ReadScripts(ref Utf8JsonStreamReader jsonReader, PackageSpec packageSpec)
        {
            if (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.StartObject)
            {
                while (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.PropertyName)
                {
                    var propertyName = jsonReader.GetString();
                    if (jsonReader.Read())
                    {
                        if (jsonReader.TokenType == JsonTokenType.String)
                        {
                            packageSpec.Scripts[propertyName] = new string[] { (string)jsonReader.GetString() };
                        }
                        else if (jsonReader.TokenType == JsonTokenType.StartArray)
                        {
                            IList<string> list = null;

                            while (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.String)
                            {
                                list ??= [];
                                list.Add(jsonReader.GetString());
                            }

                            packageSpec.Scripts[propertyName] = list ?? Enumerable.Empty<string>();
                        }
                        else
                        {
                            throw FileFormatException.Create(
                            string.Format(CultureInfo.CurrentCulture, "The value of a script in '{0}' can only be a string or an array of strings", PackageSpec.PackageSpecFileName),
                            packageSpec.FilePath);
                        }
                    }
                }
            }
        }

        private static List<CompatibilityProfile> ReadSupports(ref Utf8JsonStreamReader jsonReader)
        {
            List<CompatibilityProfile> compatibilityProfiles = null;

            if (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.StartObject)
            {
                while (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.PropertyName)
                {
                    var propertyName = jsonReader.GetString();
                    CompatibilityProfile compatibilityProfile = ReadCompatibilityProfile(ref jsonReader, propertyName);
                    compatibilityProfiles ??= [];
                    compatibilityProfiles.Add(compatibilityProfile);
                }
            }
            return compatibilityProfiles;
        }

        private static LibraryDependencyTarget ReadTarget(
           ref Utf8JsonStreamReader jsonReader,
           string packageSpecPath,
           LibraryDependencyTarget targetFlagsValue)
        {
            if (jsonReader.Read())
            {
                var targetString = jsonReader.GetString();

                targetFlagsValue = LibraryDependencyTargetUtils.Parse(targetString);

                // Verify that the value specified is package, project, or external project
#pragma warning disable CS0612 // Type or member is obsolete
                if (!ValidateDependencyTarget(targetFlagsValue))
                {
                    string message = string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.InvalidDependencyTarget,
                        targetString);
                    throw FileFormatException.Create(
                      message,
                      packageSpecPath);
                }
#pragma warning restore CS0612 // Type or member is obsolete
            }

            return targetFlagsValue;
        }

        private static List<ProjectRestoreMetadataFrameworkInfo> ReadTargetFrameworks(ref Utf8JsonStreamReader jsonReader)
        {
            List<ProjectRestoreMetadataFrameworkInfo> targetFrameworks = null;

            if (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.StartObject)
            {
                while (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.PropertyName)
                {
                    var frameworkPropertyName = jsonReader.GetString();
                    NuGetFramework framework = NuGetFramework.Parse(frameworkPropertyName);
                    var frameworkGroup = new ProjectRestoreMetadataFrameworkInfo(framework);

                    if (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.StartObject)
                    {
                        while (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.PropertyName)
                        {
                            if (jsonReader.ValueTextEquals(ProjectReferencesPropertyName))
                            {
                                if (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.StartObject)
                                {
                                    while (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.PropertyName)
                                    {
                                        var projectReferencePropertyName = jsonReader.GetString();
                                        string excludeAssets = null;
                                        string includeAssets = null;
                                        string privateAssets = null;
                                        string projectReferenceProjectPath = null;

                                        if (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.StartObject)
                                        {
                                            while (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.PropertyName)
                                            {
                                                if (jsonReader.ValueTextEquals(ExcludeAssetsPropertyName))
                                                {
                                                    excludeAssets = jsonReader.ReadNextTokenAsString();
                                                }
                                                else if (jsonReader.ValueTextEquals(IncludeAssetsPropertyName))
                                                {
                                                    includeAssets = jsonReader.ReadNextTokenAsString();
                                                }
                                                else if (jsonReader.ValueTextEquals(PrivateAssetsPropertyName))
                                                {
                                                    privateAssets = jsonReader.ReadNextTokenAsString();
                                                }
                                                else if (jsonReader.ValueTextEquals(ProjectPathPropertyName))
                                                {
                                                    projectReferenceProjectPath = jsonReader.ReadNextTokenAsString();
                                                }
                                                else
                                                {
                                                    jsonReader.Skip();
                                                }

                                            }
                                        }

                                        frameworkGroup.ProjectReferences.Add(new ProjectRestoreReference()
                                        {
                                            ProjectUniqueName = projectReferencePropertyName,
                                            ProjectPath = projectReferenceProjectPath,

                                            IncludeAssets = LibraryIncludeFlagUtils.GetFlags(
                                                flags: includeAssets,
                                                defaultFlags: LibraryIncludeFlags.All),

                                            ExcludeAssets = LibraryIncludeFlagUtils.GetFlags(
                                                flags: excludeAssets,
                                                defaultFlags: LibraryIncludeFlags.None),

                                            PrivateAssets = LibraryIncludeFlagUtils.GetFlags(
                                                flags: privateAssets,
                                                defaultFlags: LibraryIncludeFlagUtils.DefaultSuppressParent),
                                        });
                                    }
                                }
                            }
                            else if (jsonReader.ValueTextEquals(TargetAliasPropertyName))
                            {
                                frameworkGroup.TargetAlias = jsonReader.ReadNextTokenAsString();
                            }
                            else
                            {
                                jsonReader.Skip();
                            }
                        }
                        targetFrameworks ??= [];
                        targetFrameworks.Add(frameworkGroup);
                    }
                }
            }
            return targetFrameworks;
        }

        private static void ReadTargetFrameworks(PackageSpec packageSpec, ref Utf8JsonStreamReader jsonReader)
        {
            var frameworkName = NuGetFramework.Parse(jsonReader.GetString());

            var targetFrameworkInformation = new TargetFrameworkInformation();
            NuGetFramework secondaryFramework = default;

            if (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.StartObject)
            {
                while (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.PropertyName)
                {
                    if (jsonReader.ValueTextEquals(AssetTargetFallbackPropertyName))
                    {
                        targetFrameworkInformation.AssetTargetFallback = jsonReader.ReadNextTokenAsBoolOrFalse();
                    }
                    else if (jsonReader.ValueTextEquals(SecondaryFrameworkPropertyName))
                    {
                        var secondaryFrameworkString = jsonReader.ReadNextTokenAsString();
                        if (!string.IsNullOrEmpty(secondaryFrameworkString))
                        {
                            secondaryFramework = NuGetFramework.Parse(secondaryFrameworkString);
                        }
                    }
                    else if (jsonReader.ValueTextEquals(CentralPackageVersionsPropertyName))
                    {
                        ReadCentralPackageVersions(
                            ref jsonReader,
                            targetFrameworkInformation.CentralPackageVersions,
                            packageSpec.FilePath);
                    }
                    else if (jsonReader.ValueTextEquals(DependenciesPropertyName))
                    {
                        ReadDependencies(
                            ref jsonReader,
                            targetFrameworkInformation.Dependencies,
                            packageSpec.FilePath,
                            isGacOrFrameworkReference: false);
                    }
                    else if (jsonReader.ValueTextEquals(DownloadDependenciesPropertyName))
                    {
                        ReadDownloadDependencies(
                            ref jsonReader,
                            targetFrameworkInformation.DownloadDependencies,
                            packageSpec.FilePath);
                    }
                    else if (jsonReader.ValueTextEquals(FrameworkAssembliesPropertyName))
                    {
                        ReadDependencies(
                            ref jsonReader,
                            targetFrameworkInformation.Dependencies,
                            packageSpec.FilePath,
                            isGacOrFrameworkReference: true);
                    }
                    else if (jsonReader.ValueTextEquals(FrameworkReferencesPropertyName))
                    {
                        ReadFrameworkReferences(
                            ref jsonReader,
                            targetFrameworkInformation.FrameworkReferences,
                            packageSpec.FilePath);
                    }
                    else if (jsonReader.ValueTextEquals(ImportsPropertyName))
                    {
                        ReadImports(packageSpec, ref jsonReader, targetFrameworkInformation);
                    }
                    else if (jsonReader.ValueTextEquals(RuntimeIdentifierGraphPathPropertyName))
                    {
                        targetFrameworkInformation.RuntimeIdentifierGraphPath = jsonReader.ReadNextTokenAsString();
                    }
                    else if (jsonReader.ValueTextEquals(TargetAliasPropertyName))
                    {
                        targetFrameworkInformation.TargetAlias = jsonReader.ReadNextTokenAsString();
                    }
                    else if (jsonReader.ValueTextEquals(WarnPropertyName))
                    {
                        targetFrameworkInformation.Warn = jsonReader.ReadNextTokenAsBoolOrFalse();
                    }
                    else
                    {
                        jsonReader.Skip();
                    }
                }
            }

#pragma warning disable CS0612 // Type or member is obsolete
            AddTargetFramework(packageSpec, frameworkName, secondaryFramework, targetFrameworkInformation);
#pragma warning restore CS0612 // Type or member is obsolete
        }
    }
}

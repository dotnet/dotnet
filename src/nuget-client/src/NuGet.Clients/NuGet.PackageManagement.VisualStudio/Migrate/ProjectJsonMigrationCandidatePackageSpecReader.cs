// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.ProjectModel;
using NuGet.RuntimeModel;
using NuGet.Shared;
using NuGet.Versioning;
using FileFormatException = NuGet.ProjectModel.FileFormatException;

namespace NuGet.PackageManagement.VisualStudio.Migrate
{
    internal class ProjectJsonMigrationCandidatePackageSpecReader
    {
        private static readonly byte[] DependenciesPropertyName = Encoding.UTF8.GetBytes("dependencies");
        private static readonly byte[] FrameworksPropertyName = Encoding.UTF8.GetBytes("frameworks");
        private static readonly byte[] RuntimesPropertyName = Encoding.UTF8.GetBytes("runtimes");
        private static readonly byte[] SupportsPropertyName = Encoding.UTF8.GetBytes("supports");
        private static readonly byte[] VersionPropertyName = Encoding.UTF8.GetBytes("version");
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
        private static readonly byte[] HashTagImportPropertyName = Encoding.UTF8.GetBytes("#import");
        private static readonly byte[] EmptyStringPropertyName = Encoding.UTF8.GetBytes(string.Empty);

        /// <summary>
        /// Load and parse a project.json file
        /// </summary>
        /// <param name="name">project name</param>
        /// <param name="packageSpecPath">file path</param>
        public static PackageSpecProjectJsonMigrationCandidate GetPackageSpec(string name, string packageSpecPath)
        {
            PackageSpecProjectJsonMigrationCandidate packageSpecProjectJsonMigrationCandidate =
                FileUtility.SafeRead(filePath: packageSpecPath, read: (stream, filePath) => GetPackageSpec(stream, name, filePath, null));

            return packageSpecProjectJsonMigrationCandidate;
        }

        private static PackageSpecProjectJsonMigrationCandidate GetPackageSpec(Stream stream, string name, string packageSpecPath, string snapshotValue)
        {
            return GetPackageSpec(stream, name, packageSpecPath, snapshotValue, EnvironmentVariableWrapper.Instance);
        }

        private static PackageSpecProjectJsonMigrationCandidate GetPackageSpec(Stream stream, string name, string packageSpecPath, string snapshotValue, IEnvironmentVariableReader environmentVariableReader)
        {
            return GetPackageSpecUtf8JsonStreamReader(stream, name, packageSpecPath, environmentVariableReader, snapshotValue);
        }

        internal static PackageSpecProjectJsonMigrationCandidate GetPackageSpecUtf8JsonStreamReader(Stream stream, string name, string packageSpecPath, IEnvironmentVariableReader environmentVariableReader, string snapshotValue = null)
        {
            var reader = new Utf8JsonStreamReader(stream);
            var packageSpec = GetPackageSpec(ref reader, name, packageSpecPath, environmentVariableReader, snapshotValue);

            reader.Dispose();

            return packageSpec;
        }

        internal static PackageSpecProjectJsonMigrationCandidate GetPackageSpec(ref Utf8JsonStreamReader jsonReader, string name, string packageSpecPath, IEnvironmentVariableReader environmentVariableReader, string snapshotValue = null)
        {
            List<LibraryDependency> dependencies = new List<LibraryDependency>();
            List<TargetFrameworkInformation> targetFrameworks = new List<TargetFrameworkInformation>();
            List<CompatibilityProfile> compatibilityProfiles = null;
            List<RuntimeDescription> runtimeDescriptions = null;
            string filePath = name == null ? null : Path.GetFullPath(packageSpecPath);

            if (jsonReader.TokenType == JsonTokenType.StartObject)
            {
                while (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.PropertyName)
                {
                    if (jsonReader.ValueTextEquals(EmptyStringPropertyName))
                    {
                        jsonReader.Skip();
                    }
                    else if (jsonReader.ValueTextEquals(DependenciesPropertyName))
                    {
                        ReadDependencies(
                            ref jsonReader,
                            dependencies,
                            filePath,
                            isGacOrFrameworkReference: false);
                    }
                    else if (jsonReader.ValueTextEquals(FrameworksPropertyName))
                    {
                        ReadFrameworks(ref jsonReader, targetFrameworks, filePath);
                    }
                    else if (jsonReader.ValueTextEquals(RuntimesPropertyName))
                    {
                        runtimeDescriptions = ReadRuntimes(ref jsonReader);
                    }
                    else if (jsonReader.ValueTextEquals(SupportsPropertyName))
                    {
                        compatibilityProfiles = ReadSupports(ref jsonReader);
                    }
                    else
                    {
                        jsonReader.Skip();
                    }
                }
            }

            RuntimeGraph runtimeGraph = new RuntimeGraph(
                runtimeDescriptions ?? Enumerable.Empty<RuntimeDescription>(),
                compatibilityProfiles ?? Enumerable.Empty<CompatibilityProfile>());

            PackageSpecProjectJsonMigrationCandidate packageSpec = new(
                dependencies,
                targetFrameworks,
                runtimeGraph);

            return packageSpec;
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

        private static void ReadFrameworks(ref Utf8JsonStreamReader reader, List<TargetFrameworkInformation> targetFrameworks, string filePath)
        {
            if (reader.Read() && reader.TokenType == JsonTokenType.StartObject)
            {
                while (reader.Read() && reader.TokenType == JsonTokenType.PropertyName)
                {
                    try
                    {
                        ReadTargetFrameworks(targetFrameworks, filePath, ref reader);
                    }
                    catch (Exception ex)
                    {
                        throw new FileFormatException(filePath, ex);
                    }
                }
            }
        }

        private static void ReadTargetFrameworks(List<TargetFrameworkInformation> targetFrameworks, string filePath, ref Utf8JsonStreamReader jsonReader)
        {
            var frameworkName = NuGetFramework.Parse(jsonReader.GetString());
            List<LibraryDependency> dependencies = null;

            if (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.StartObject)
            {
                while (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.PropertyName)
                {
                    if (jsonReader.ValueTextEquals(DependenciesPropertyName))
                    {
                        dependencies ??= new List<LibraryDependency>();
                        ReadDependencies(
                            ref jsonReader,
                            dependencies,
                            filePath,
                            isGacOrFrameworkReference: false);
                    }
                    else
                    {
                        jsonReader.Skip();
                    }
                }
            }

            var targetFrameworkInformation = new TargetFrameworkInformation()
            {
                Dependencies = dependencies != null ? dependencies.ToImmutableArray() : [],
            };

            AddTargetFramework(targetFrameworks, frameworkName, secondaryFramework: default, targetFrameworkInformation);
        }

        private static void AddTargetFramework(List<TargetFrameworkInformation> targetFrameworks, NuGetFramework frameworkName, NuGetFramework secondaryFramework, TargetFrameworkInformation targetFrameworkInformation)
        {
            NuGetFramework updatedFramework = frameworkName;

            if (targetFrameworkInformation.Imports.Length > 0)
            {
                NuGetFramework[] imports = targetFrameworkInformation.Imports.ToArray();

                if (targetFrameworkInformation.AssetTargetFallback)
                {
                    updatedFramework = new AssetTargetFallbackFramework(GetDualCompatibilityFrameworkIfNeeded(frameworkName, secondaryFramework), imports);
                }
                else
                {
                    updatedFramework = new FallbackFramework(GetDualCompatibilityFrameworkIfNeeded(frameworkName, secondaryFramework), imports);
                }
            }
            else
            {
                updatedFramework = GetDualCompatibilityFrameworkIfNeeded(frameworkName, secondaryFramework);
            }

            targetFrameworkInformation = new TargetFrameworkInformation(targetFrameworkInformation) { FrameworkName = updatedFramework };

            targetFrameworks.Add(targetFrameworkInformation);
        }

        private static NuGetFramework GetDualCompatibilityFrameworkIfNeeded(NuGetFramework frameworkName, NuGetFramework secondaryFramework)
        {
            if (secondaryFramework != default)
            {
                return new DualCompatibilityFramework(frameworkName, secondaryFramework);
            }

            return frameworkName;
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
                        throw new FileFormatException(packageSpecPath, new ArgumentException("Unable to resolve dependency."));
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
                        ImmutableArray<NuGetLogCode> noWarn = [];

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
                                            throw new FileFormatException(packageSpecPath, ex);
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
                                throw new FileFormatException(packageSpecPath, ex);
                            }
                        }

                        // Projects and References may have empty version ranges, Packages may not
                        if (dependencyVersionRange == null)
                        {
                            if ((targetFlagsValue & LibraryDependencyTarget.Package) == LibraryDependencyTarget.Package)
                            {
                                throw new FileFormatException(packageSpecPath, new ArgumentException("Dependency version"));
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
                            VersionOverride = versionOverride,
                            NoWarn = noWarn,
                        };

                        results.Add(libraryDependency);
                    }
                }
            }
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
                if (!ValidateDependencyTarget(targetFlagsValue))
                {
                    throw new FileFormatException(packageSpecPath, new ArgumentException("Invalid dependency target"));
                }
            }

            return targetFlagsValue;
        }

        private static ImmutableArray<NuGetLogCode> ReadNuGetLogCodesList(ref Utf8JsonStreamReader jsonReader)
        {
            NuGetLogCode[] items = null;
            var index = 0;

            if (jsonReader.Read() && jsonReader.TokenType == JsonTokenType.StartArray)
            {
                while (jsonReader.Read() && jsonReader.TokenType != JsonTokenType.EndArray)
                {
                    if (jsonReader.TokenType == JsonTokenType.String && Enum.TryParse(jsonReader.GetString(), out NuGetLogCode code))
                    {
                        if (items == null)
                        {
                            items = ArrayPool<NuGetLogCode>.Shared.Rent(16);
                        }
                        else if (items.Length == index)
                        {
                            var oldItems = items;

                            items = ArrayPool<NuGetLogCode>.Shared.Rent(items.Length * 2);
                            oldItems.CopyTo(items, index: 0);

                            ArrayPool<NuGetLogCode>.Shared.Return(oldItems);
                        }

                        items[index++] = code;
                    }
                }
            }

            if (items == null)
            {
                return [];
            }

            var retVal = items.AsSpan(0, index).ToImmutableArray();
            ArrayPool<NuGetLogCode>.Shared.Return(items);

            return retVal;
        }

        private static bool ValidateDependencyTarget(LibraryDependencyTarget targetValue)
        {
            var isValid = false;

            switch (targetValue)
            {
                case LibraryDependencyTarget.Package:
                case LibraryDependencyTarget.Project:
                case LibraryDependencyTarget.ExternalProject:
                    isValid = true;
                    break;
            }

            return isValid;
        }
    }
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Packaging.Rules;
using NuGet.ProjectModel;
using NuGet.Versioning;

namespace NuGet.Commands
{
    public class PackCommandRunner
    {
        public delegate IProjectFactory CreateProjectFactory(PackArgs packArgs, string path);

        private readonly PackArgs _packArgs;
        private readonly PackageBuilder _packageBuilder;
        private readonly CreateProjectFactory _createProjectFactory;

        private static readonly HashSet<string> AllowedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            NuGetConstants.ManifestExtension,
            ".csproj",
            ".vbproj",
            ".fsproj",
            ".nproj",
            ".btproj",
            ".dxjsproj",
            ".json"
        };

        // Target file paths to exclude when building the lib package for symbol server scenario
        private static readonly string[] LibPackageExcludes = new[]
        {
            @"**\*.pdb".Replace('\\', Path.DirectorySeparatorChar),
            @"src\**\*".Replace('\\', Path.DirectorySeparatorChar)
        };

        // Target file paths to exclude when building the symbols package for symbol server scenario
        private static readonly string[] SymbolPackageExcludes = new[]
        {
            @"content\**\*".Replace('\\', Path.DirectorySeparatorChar),
            @"tools\**\*.ps1".Replace('\\', Path.DirectorySeparatorChar)
        };

        private readonly HashSet<string> _excludes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public bool GenerateNugetPackage { get; set; }

        public IEnumerable<IPackageRule> Rules { get; set; }

        public PackCommandRunner(
            PackArgs packArgs,
            CreateProjectFactory createProjectFactory,
            PackageBuilder packageBuilder)
            : this(packArgs, createProjectFactory)
        {
            _packageBuilder = packageBuilder;
        }

        public PackCommandRunner(PackArgs packArgs, CreateProjectFactory createProjectFactory)
        {
            _createProjectFactory = createProjectFactory;
            _packArgs = packArgs;
            Rules = RuleSet.PackageCreationRuleSet;
            GenerateNugetPackage = true;
        }

        /// <summary>
        /// Runs a package build for the args provided in the command runner.
        /// </summary>
        /// <returns><see langword="true"/> if the package creation completed succesfully. <see langword="false"/> otherwise.</returns>
        /// <exception cref="PackagingException">If a core packaging validation fails.</exception>
        public bool RunPackageBuild()
        {
            var result = BuildPackage(Path.GetFullPath(Path.Combine(_packArgs.CurrentDirectory, _packArgs.Path)));
            return result;
        }

        [Obsolete("Do not use this. Use RunPackageBuild() instead as it accounts for the effects of package analysis to the complete operation status.")]
        public void BuildPackage()
        {
            BuildPackage(Path.GetFullPath(Path.Combine(_packArgs.CurrentDirectory, _packArgs.Path)));
        }

        private bool BuildPackage(string path)
        {
            string extension = Path.GetExtension(path);
            if (extension.Equals(NuGetConstants.ManifestExtension, StringComparison.OrdinalIgnoreCase))
            {
                return BuildFromNuspec(path);
            }
            else
            {
                return BuildFromProjectFile(path);
            }
        }

        /// <summary>
        /// Builds and validates the package.
        /// If a core validation fails, this method will throw a <see cref="PackagingException"/>.
        /// If for any other reason the package creation fails (like for example, a validation rule got bumped from warning to an error, this will return <see langword="null"/>.
        /// </summary>
        /// <param name="builder">The package builder to use.</param>
        /// <param name="outputPath">The package output path.</param>
        /// <returns>A <see cref="PackageArchiveReader"/> if everything completed succesfully. Throws if a core package validation fails. Returns <see langword="null"/> if a validation rule got elevated from a warning to an error.</returns>
        /// <exception cref="PackagingException">If a core packaging validation fails.</exception>
        [Obsolete("Do not use this. Use RunPackageBuild() instead as it accounts for the effects of package analysis to the complete operation status.")]
        public PackageArchiveReader BuildPackage(PackageBuilder builder, string outputPath = null)
        {
            outputPath = outputPath ?? GetOutputPath(builder, _packArgs, false, builder.Version);
            var successful = BuildPackage(builder, outputPath, symbolsPackage: false);
            PackageArchiveReader packageArchiveReader = null;
            if (successful && File.Exists(outputPath))
            {
                packageArchiveReader = new PackageArchiveReader(outputPath);
            }
            return packageArchiveReader;
        }

        /// <summary>
        /// Builds and validates the package.
        /// If a core validation fails, this method will throw a <see cref="PackagingException"/>.
        /// If for any other reason the package creation fails (like for example, a validation rule got bumped from warning to an error, this will return <see langword="null"/>.
        /// </summary>
        /// <param name="builder">The package builder to use.</param>
        /// <param name="outputPath">The package output path.</param>
        /// <param name="symbolsPackage">Whether this package is a symbols package. Symbols packages do not undergo validations.</param>
        /// <returns>A <see cref="PackageArchiveReader"/> if everything completed succesfully. Throws if a core package validation fails. Returns <see langword="null"/> if a validation rule got elevated from a warning to an error.</returns>
        /// <exception cref="PackagingException">If a core packaging validation fails.</exception>
        private bool BuildPackage(PackageBuilder builder, string outputPath = null, bool symbolsPackage = false)
        {
            outputPath = outputPath ?? GetOutputPath(builder, _packArgs, false, builder.Version);
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath));

            // Track if the package file was already present on disk
            bool isExistingPackage = File.Exists(outputPath);
            try
            {
                using (Stream stream = File.Create(outputPath))
                {
                    builder.Save(stream);
                }
            }
            catch
            {
                if (!isExistingPackage && File.Exists(outputPath))
                {
                    File.Delete(outputPath);
                }
                throw;
            }

            if (_packArgs.LogLevel == LogLevel.Verbose)
            {
                PrintVerbose(outputPath, builder);
            }

            using var package = new PackageArchiveReader(outputPath);

            if (package != null && !_packArgs.NoPackageAnalysis && !symbolsPackage)
            {
                AnalyzePackage(package);
                if (_packArgs.Logger is PackCollectorLogger collectorLogger)
                {
                    if (collectorLogger.Errors.Any(e => e.Level == LogLevel.Error))
                    {
                        package.Dispose();
                        if (!isExistingPackage && File.Exists(outputPath))
                        {
                            File.Delete(outputPath);
                        }
                        return false;
                    }
                }
            }

            if (_packArgs.InstallPackageToOutputPath)
            {
                _packArgs.Logger.Log(
                    PackagingLogMessage.CreateMessage(
                        string.Format(
                            CultureInfo.CurrentCulture,
                            Strings.Log_PackageCommandInstallPackageToOutputPath, "Package", outputPath),
                        LogLevel.Minimal));

                WriteResolvedNuSpecToPackageOutputDirectory(builder);
                WriteSHA512PackageHash(builder);
            }

            _packArgs.Logger.Log(
                PackagingLogMessage.CreateMessage(
                    string.Format(CultureInfo.CurrentCulture, Strings.Log_PackageCommandSuccess, outputPath),
                    LogLevel.Minimal));


            return true;
        }

        /// <summary>
        /// Writes the resolved NuSpec file to the package output directory.
        /// </summary>
        /// <param name="builder">The package builder</param>
        private void WriteResolvedNuSpecToPackageOutputDirectory(PackageBuilder builder)
        {
            string outputPath = GetOutputPath(builder, _packArgs, false, builder.Version);
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath));

            string resolvedNuSpecOutputPath = Path.Combine(
                Path.GetDirectoryName(outputPath),
                new VersionFolderPathResolver(outputPath).GetManifestFileName(builder.Id, builder.Version));

            _packArgs.Logger.Log(
                PackagingLogMessage.CreateMessage(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.Log_PackageCommandInstallPackageToOutputPath,
                        "NuSpec",
                        resolvedNuSpecOutputPath),
                    LogLevel.Minimal));

            if (string.Equals(_packArgs.Path, resolvedNuSpecOutputPath, StringComparison.OrdinalIgnoreCase))
            {
                throw new PackagingException(
                    NuGetLogCode.NU5001,
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.Error_WriteResolvedNuSpecOverwriteOriginal,
                        _packArgs.Path));
            }

            // We must use the Path.GetTempPath() which NuGetFolderPath.Temp uses as a root because writing temp files
            // to the package directory with a guid would break some build tools caching
            var manifest = new Manifest(new ManifestMetadata(builder), files: null);
            string tempOutputPath = Path.Combine(
                NuGetEnvironment.GetFolderPath(NuGetFolderPath.Temp),
                Path.GetRandomFileName());

            using (var stream = new FileStream(tempOutputPath, FileMode.Create))
            {
                manifest.Save(stream);
            }

            FileUtility.Replace(tempOutputPath, resolvedNuSpecOutputPath);
        }

        /// <summary>
        /// Writes the sha512 package hash file to the package output directory
        /// </summary>
        /// <param name="builder">The package builder</param>
        private void WriteSHA512PackageHash(PackageBuilder builder)
        {
            string outputPath = GetOutputPath(builder, _packArgs, symbols: false, nugetVersion: builder.Version);

            Directory.CreateDirectory(Path.GetDirectoryName(outputPath));

            string sha512OutputPath = Path.Combine(outputPath + ".sha512");

            // We must use the Path.GetTempPath() which NuGetFolderPath.Temp uses as a root because writing temp files
            // to the package directory with a guid would break some build tools caching
            string tempOutputPath = Path.Combine(
                NuGetEnvironment.GetFolderPath(NuGetFolderPath.Temp),
                Path.GetRandomFileName());

            _packArgs.Logger.Log(
                PackagingLogMessage.CreateMessage(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.Log_PackageCommandInstallPackageToOutputPath,
                        "SHA512",
                        sha512OutputPath),
                    LogLevel.Minimal));

            byte[] sha512hash;
            var cryptoHashProvider = new CryptoHashProvider("SHA512");

            using (var fileStream = new FileStream(outputPath, FileMode.Open, FileAccess.Read))
            {
                sha512hash = cryptoHashProvider.CalculateHash(fileStream);
            }

            File.WriteAllText(tempOutputPath, Convert.ToBase64String(sha512hash));
            FileUtility.Replace(tempOutputPath, sha512OutputPath);
        }

        private void InitCommonPackageBuilderProperties(PackageBuilder builder)
        {
            if (!string.IsNullOrEmpty(_packArgs.Version))
            {
                builder.Version = new NuGetVersion(_packArgs.Version);
                builder.HasSnapshotVersion = false;
            }

            if (!string.IsNullOrEmpty(_packArgs.Suffix) && !builder.HasSnapshotVersion)
            {
                string version = VersionFormatter.Instance.Format("V", builder.Version, VersionFormatter.Instance);
                builder.Version = new NuGetVersion($"{version}-{_packArgs.Suffix}");
            }

            if (_packArgs.Serviceable)
            {
                builder.Serviceable = true;
            }

            if (_packArgs.MinClientVersion != null)
            {
                builder.MinClientVersion = _packArgs.MinClientVersion;
            }

            CheckForUnsupportedFrameworks(builder);

            ExcludeFiles(builder.Files);
        }

        [Obsolete]
        public static bool ProcessProjectJsonFile(
            PackageBuilder builder,
            string basePath,
            string id,
            NuGetVersion version,
            string suffix,
            Func<string, string> propertyProvider)
        {
            if (basePath == null)
            {
                return false;
            }

            string path = ProjectJsonPathUtilities.GetProjectConfigPath(basePath, Path.GetFileName(basePath));
            if (File.Exists(path))
            {
                using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    LoadProjectJsonFile(builder, path, basePath, id, stream, version, suffix);
                }
                return true;
            }

            return false;
        }

        [Obsolete]
        private static void LoadProjectJsonFile(
            PackageBuilder builder,
            string path,
            string basePath,
            string id,
            Stream stream,
            NuGetVersion version,
            string suffix)
        {
            PackageSpec spec = JsonPackageSpecReader.GetPackageSpec(stream, id, path, suffix);

            if (id == null)
            {
                builder.Id = Path.GetFileName(basePath);
            }
            else
            {
                builder.Id = id;
            }
            if (version != null)
            {
                builder.Version = version;
                builder.HasSnapshotVersion = false;
            }
            else if (!spec.IsDefaultVersion)
            {
                builder.Version = spec.Version;
                builder.HasSnapshotVersion = spec.HasVersionSnapshot;

                if (suffix != null && !spec.HasVersionSnapshot)
                {
                    builder.Version = new NuGetVersion(
                        builder.Version.Major,
                        builder.Version.Minor,
                        builder.Version.Patch,
                        builder.Version.Revision,
                        suffix,
                        metadata: null);
                }
            }
            if (spec.Title != null)
            {
                builder.Title = spec.Title;
            }
            if (spec.Description != null)
            {
                builder.Description = spec.Description;
            }
            if (spec.Copyright != null)
            {
                builder.Copyright = spec.Copyright;
            }
            if (spec.Authors.Any())
            {
                builder.Authors.AddRange(spec.Authors);
            }
            if (spec.Owners.Any())
            {
                builder.Owners.AddRange(spec.Owners);
            }
            Uri tempUri;
            if (Uri.TryCreate(spec.LicenseUrl, UriKind.Absolute, out tempUri))
            {
                builder.LicenseUrl = tempUri;
            }
            if (Uri.TryCreate(spec.ProjectUrl, UriKind.Absolute, out tempUri))
            {
                builder.ProjectUrl = tempUri;
            }
            if (Uri.TryCreate(spec.IconUrl, UriKind.Absolute, out tempUri))
            {
                builder.IconUrl = tempUri;
            }
            builder.RequireLicenseAcceptance = spec.RequireLicenseAcceptance;
            if (spec.Summary != null)
            {
                builder.Summary = spec.Summary;
            }
            if (spec.ReleaseNotes != null)
            {
                builder.ReleaseNotes = spec.ReleaseNotes;
            }
            if (spec.Language != null)
            {
                builder.Language = spec.Language;
            }
            if (spec.BuildOptions != null && spec.BuildOptions.OutputName != null)
            {
                builder.OutputName = spec.BuildOptions.OutputName;
            }

            foreach (KeyValuePair<string, string> include in spec.PackInclude)
            {
                builder.AddFiles(basePath, include.Value, include.Key);
            }

            if (spec.PackOptions != null)
            {
                if (spec.PackOptions.IncludeExcludeFiles != null)
                {
                    string fullExclude;
                    string filesExclude;
                    CalculateExcludes(spec.PackOptions.IncludeExcludeFiles, out fullExclude, out filesExclude);

                    if (spec.PackOptions.IncludeExcludeFiles.Include != null)
                    {
                        foreach (string include in spec.PackOptions.IncludeExcludeFiles.Include)
                        {
                            builder.AddFiles(basePath, include, string.Empty, fullExclude);
                        }
                    }

                    if (spec.PackOptions.IncludeExcludeFiles.IncludeFiles != null)
                    {
                        foreach (string includeFile in spec.PackOptions.IncludeExcludeFiles.IncludeFiles)
                        {
                            string resolvedPath = ResolvePath(new PhysicalPackageFile() { SourcePath = includeFile }, basePath);

                            builder.AddFiles(basePath, includeFile, resolvedPath, filesExclude);
                        }
                    }
                }

                if (spec.PackOptions.Mappings != null)
                {
                    foreach (KeyValuePair<string, IncludeExcludeFiles> map in spec.PackOptions.Mappings)
                    {
                        string fullExclude;
                        string filesExclude;
                        CalculateExcludes(map.Value, out fullExclude, out filesExclude);

                        if (map.Value.Include != null)
                        {
                            // Include paths from project.json are glob matching strings.
                            // Calling AddFiles for "path/**" with an output target of "newpath/"
                            // should go to "newpath/filename" but instead goes to "newpath/path/filename".
                            // To get around this, do a WildcardSearch ahead of the AddFiles to get full paths.
                            // Passing in the target path will then what we want.
                            foreach (string include in map.Value.Include)
                            {
                                IEnumerable<string> matchedFiles = PathResolver.PerformWildcardSearch(basePath, include);
                                foreach (var matchedFile in matchedFiles)
                                {
                                    builder.AddFiles(basePath, matchedFile, map.Key, fullExclude);
                                }
                            }
                        }

                        if (map.Value.IncludeFiles != null)
                        {
                            foreach (string include in map.Value.IncludeFiles)
                            {
                                builder.AddFiles(basePath, include, map.Key, filesExclude);
                            }
                        }
                    }
                }
            }

            if (spec.Tags.Any())
            {
                builder.Tags.AddRange(spec.Tags);
            }

            if (spec.TargetFrameworks.Any())
            {
                foreach (TargetFrameworkInformation framework in spec.TargetFrameworks)
                {
                    if (framework.FrameworkName.IsUnsupported)
                    {
                        throw new PackagingException(
                            NuGetLogCode.NU5003,
                            string.Format(
                                CultureInfo.CurrentCulture,
                                Strings.Error_InvalidTargetFramework,
                                framework.FrameworkName));
                    }

                    builder.TargetFrameworks.Add(framework.FrameworkName);
                    AddDependencyGroups(framework.Dependencies.Concat(spec.Dependencies), framework.FrameworkName, builder);
                }
            }
            else
            {
                if (spec.Dependencies.Any())
                {
                    AddDependencyGroups(spec.Dependencies, NuGetFramework.AnyFramework, builder);
                }
            }

            builder.PackageTypes = new Collection<PackageType>(spec.PackOptions?.PackageType?.ToList() ?? new List<PackageType>());
        }

        private static void CalculateExcludes(IncludeExcludeFiles files, out string fullExclude, out string filesExclude)
        {
            fullExclude = string.Empty;
            filesExclude = string.Empty;
            if (files.Exclude != null &&
                files.Exclude.Any())
            {
                fullExclude = string.Join(";", files.Exclude);
            }

            if (files.ExcludeFiles != null &&
                files.ExcludeFiles.Any())
            {
                if (!string.IsNullOrEmpty(fullExclude))
                {
                    fullExclude += ";";
                }
                filesExclude += string.Join(";", files.ExcludeFiles);
                fullExclude += filesExclude;
            }
        }

        public static void AddDependencyGroups(
            IEnumerable<LibraryDependency> dependencies,
            NuGetFramework framework,
            PackageBuilder builder)
        {
            ISet<PackageDependency> packageDependencies = new HashSet<PackageDependency>();

            foreach (LibraryDependency dependency in dependencies)
            {
                LibraryIncludeFlags effectiveInclude = dependency.IncludeType & ~dependency.SuppressParent;

                if (dependency.IncludeType == LibraryIncludeFlags.None || dependency.SuppressParent == LibraryIncludeFlags.All)
                {
                    continue;
                }

                if (dependency.LibraryRange.TypeConstraint == LibraryDependencyTarget.Reference)
                {
                    FrameworkAssemblyReference reference = builder.FrameworkReferences.FirstOrDefault(r => r.AssemblyName == dependency.Name);
                    if (reference == null)
                    {
                        builder.FrameworkReferences.Add(
                            new FrameworkAssemblyReference(dependency.Name, new NuGetFramework[] { framework }));
                    }
                    else
                    {
                        if (!reference.SupportedFrameworks.Contains(framework))
                        {
                            // Add another framework reference by replacing the existing reference
                            var newReference = new FrameworkAssemblyReference(
                                reference.AssemblyName,
                                reference.SupportedFrameworks.Concat(new NuGetFramework[] { framework }));
                            int index = builder.FrameworkReferences.IndexOf(reference);
                            builder.FrameworkReferences.Remove(reference);
                            builder.FrameworkReferences.Insert(index, newReference);
                        }
                    }
                }
                else
                {
                    var includes = new List<string>();
                    var excludes = new List<string>();
                    if (effectiveInclude == LibraryIncludeFlags.All)
                    {
                        includes.Add(LibraryIncludeFlags.All.ToString());
                    }
                    else if ((effectiveInclude & LibraryIncludeFlags.ContentFiles) == LibraryIncludeFlags.ContentFiles)
                    {
                        includes.AddRange(
                            effectiveInclude.ToString().Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries));
                    }
                    else
                    {
                        if ((LibraryIncludeFlagUtils.NoContent & ~effectiveInclude) != LibraryIncludeFlags.None)
                        {
                            excludes.AddRange(
                                (LibraryIncludeFlagUtils.NoContent & ~effectiveInclude).ToString()
                                    .Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries));
                        }
                    }

                    VersionRange version = dependency.LibraryRange.VersionRange;
                    if (!version.HasLowerBound && !version.HasUpperBound)
                    {
                        version = new VersionRange(builder.Version);
                    }

                    packageDependencies.Add(new PackageDependency(dependency.Name, version, includes, excludes));
                }
            }

            PackageDependencyGroup dependencyGroup = builder.DependencyGroups.FirstOrDefault(r => r.TargetFramework.Equals(framework));
            if (dependencyGroup != null)
            {
                var existingDependencies = new HashSet<PackageDependency>(dependencyGroup.Packages);
                foreach (PackageDependency packageDependency in packageDependencies)
                {
                    AddPackageDependency(packageDependency, existingDependencies);
                }
                var newDependencyGroup = new PackageDependencyGroup(framework, existingDependencies);
                builder.DependencyGroups.Remove(dependencyGroup);
                builder.DependencyGroups.Add(newDependencyGroup);
            }
            else
            {
                builder.DependencyGroups.Add(new PackageDependencyGroup(framework, packageDependencies));
            }
        }

        private bool BuildFromNuspec(string path)
        {
            PackageBuilder packageBuilder = CreatePackageBuilderFromNuspec(path);

            bool successful;

            InitCommonPackageBuilderProperties(packageBuilder);

            if (_packArgs.InstallPackageToOutputPath)
            {
                string outputPath = GetOutputPath(packageBuilder, _packArgs);
                successful = BuildPackage(packageBuilder, outputPath: outputPath, symbolsPackage: false);
            }
            else
            {
                if (_packArgs.Symbols && packageBuilder.Files.Any())
                {
                    // remove source related files when building the lib package
                    ExcludeFilesForLibPackage(packageBuilder.Files);

                    if (!packageBuilder.Files.Any())
                    {
                        throw new PackagingException(
                            NuGetLogCode.NU5004,
                            string.Format(
                                CultureInfo.CurrentCulture,
                                Strings.Error_PackageCommandNoFilesForLibPackage,
                                path,
                                Strings.NuGetDocs));
                    }
                }

                successful = BuildPackage(packageBuilder, symbolsPackage: false);

                if (_packArgs.Symbols)
                {
                    successful = successful && BuildSymbolsPackage(path);
                }
            }

            return successful;
        }

        private PackageBuilder CreatePackageBuilderFromNuspec(string path)
        {
            // Set the version property if the flag is set
            if (!string.IsNullOrEmpty(_packArgs.Version))
            {
                _packArgs.Properties["version"] = _packArgs.Version;
            }

            // If a nuspec file is being set via dotnet.exe then the warning properties and logger has already been initialized via PackTask.
            if (_packArgs.WarningProperties == null)
            {
                _packArgs.WarningProperties = WarningProperties.GetWarningProperties(
                treatWarningsAsErrors: _packArgs.GetPropertyValue("TreatWarningsAsErrors") ?? string.Empty,
                warningsAsErrors: _packArgs.GetPropertyValue("WarningsAsErrors") ?? string.Empty,
                noWarn: _packArgs.GetPropertyValue("NoWarn") ?? string.Empty,
                warningsNotAsErrors: _packArgs.GetPropertyValue("WarningsNotAsErrors") ?? string.Empty);
                _packArgs.Logger = new PackCollectorLogger(_packArgs.Logger, _packArgs.WarningProperties);
            }

            if (string.IsNullOrEmpty(_packArgs.BasePath))
            {
                return new PackageBuilder(
                    path,
                    _packArgs.GetPropertyValue,
                    !_packArgs.ExcludeEmptyDirectories,
                    _packArgs.Deterministic,
                    _packArgs.Logger);
            }

            return new PackageBuilder(
                path,
                _packArgs.BasePath,
                _packArgs.GetPropertyValue,
                !_packArgs.ExcludeEmptyDirectories,
                _packArgs.Deterministic,
                _packArgs.Logger);
        }

        private bool BuildFromProjectFile(string path)
        {
            // PackTargetArgs is only set for dotnet.exe pack code path, hence the check.
            if ((string.IsNullOrEmpty(_packArgs.MsBuildDirectory?.Value)
                || _createProjectFactory == null) && _packArgs.PackTargetArgs == null)
            {
                throw new PackagingException(
                    NuGetLogCode.NU5009, string.Format(CultureInfo.CurrentCulture, Strings.Error_CannotFindMsbuild));
            }

            IProjectFactory factory = _createProjectFactory.Invoke(_packArgs, path);
            if (_packArgs.WarningProperties == null && _packArgs.PackTargetArgs == null)
            {
                _packArgs.WarningProperties = factory.GetWarningPropertiesForProject();
                // Reinitialize the logger with Console as the inner logger and the obtained warning properties
                _packArgs.Logger = new PackCollectorLogger(_packArgs.Logger, _packArgs.WarningProperties);
                factory.Logger = _packArgs.Logger;
            }

            // Add the additional Properties to the properties of the Project Factory
            foreach (KeyValuePair<string, string> property in _packArgs.Properties)
            {
                if (factory.GetProjectProperties().ContainsKey(property.Key))
                {
                    _packArgs.Logger.Log(PackagingLogMessage.CreateWarning(
                        string.Format(CultureInfo.CurrentCulture, Strings.Warning_DuplicatePropertyKey, property.Key),
                        NuGetLogCode.NU5114));
                }
                factory.GetProjectProperties()[property.Key] = property.Value;
            }

            NuGetVersion version = null;
            if (_packArgs.Version != null)
            {
                version = new NuGetVersion(_packArgs.Version);
            }

            // Create a builder for the main package as well as the sources/symbols package
            PackageBuilder mainPackageBuilder = factory.CreateBuilder(
                _packArgs.BasePath,
                version,
                _packArgs.Suffix,
                buildIfNeeded: true,
                builder: _packageBuilder);

            if (mainPackageBuilder == null)
            {
                throw new PackagingException(string.Format(CultureInfo.CurrentCulture, Strings.Error_PackFailed, path));
            }

            InitCommonPackageBuilderProperties(mainPackageBuilder);

            mainPackageBuilder.EmitRequireLicenseAcceptance = mainPackageBuilder.RequireLicenseAcceptance;

            bool successful = true;
            // Build the main package
            if (GenerateNugetPackage)
            {
                if (_packArgs.InstallPackageToOutputPath)
                {
                    string outputPath = GetOutputPath(mainPackageBuilder, _packArgs);
                    successful = BuildPackage(mainPackageBuilder, outputPath: outputPath, symbolsPackage: false);
                }
                else
                {
                    successful = BuildPackage(mainPackageBuilder, symbolsPackage: false);
                }

                // If we're excluding symbols then do nothing else
                if (!_packArgs.Symbols || _packArgs.InstallPackageToOutputPath)
                {
                    return successful;
                }
            }

            if (_packArgs.Symbols)
            {
                WriteLine(string.Empty);
                WriteLine(Strings.Log_PackageCommandAttemptingToBuildSymbolsPackage, Path.GetFileName(path));
                NuGetVersion argsVersion = null;
                if (_packArgs.Version != null)
                {
                    argsVersion = new NuGetVersion(_packArgs.Version);
                }

                factory.SetIncludeSymbols(includeSymbols: true);
                PackageBuilder symbolsBuilder = factory.CreateBuilder(
                    _packArgs.BasePath,
                    argsVersion,
                    _packArgs.Suffix,
                    buildIfNeeded: false,
                    builder: mainPackageBuilder);
                symbolsBuilder.Version = mainPackageBuilder.Version;
                symbolsBuilder.HasSnapshotVersion = mainPackageBuilder.HasSnapshotVersion;
                if (_packArgs.SymbolPackageFormat == SymbolPackageFormat.Snupkg) // Snupkgs can only have 1 PackageType.
                {
                    symbolsBuilder.PackageTypes.Clear();
                    symbolsBuilder.PackageTypes.Add(PackageType.SymbolsPackage);
                }

                // Get the file name for the sources package and build it
                string outputPath = GetOutputPath(symbolsBuilder, _packArgs, symbols: true);

                InitCommonPackageBuilderProperties(symbolsBuilder);

                if (GenerateNugetPackage)
                {
                    successful = successful && BuildPackage(symbolsBuilder, outputPath, symbolsPackage: true);
                }
            }

            return successful;
        }

        private void CheckForUnsupportedFrameworks(PackageBuilder builder)
        {
            foreach (FrameworkAssemblyReference reference in builder.FrameworkReferences)
            {
                foreach (NuGetFramework framework in reference.SupportedFrameworks)
                {
                    if (framework.IsUnsupported)
                    {
                        throw new PackagingException(
                            NuGetLogCode.NU5003,
                            string.Format(CultureInfo.CurrentCulture, Strings.Error_InvalidTargetFramework, reference.AssemblyName));
                    }
                }
            }
        }

        private void PrintVerbose(string outputPath, PackageBuilder builder)
        {
            WriteLine(string.Empty);

            using var package = new PackageArchiveReader(outputPath);

            WriteLine("Id: {0}", builder.Id);
            WriteLine("Version: {0}", builder.Version);
            WriteLine("Authors: {0}", string.Join(", ", builder.Authors));
            WriteLine("Description: {0}", builder.Description);
            if (builder.LicenseUrl != null)
            {
                WriteLine("License Url: {0}", builder.LicenseUrl);
            }
            if (builder.ProjectUrl != null)
            {
                WriteLine("Project Url: {0}", builder.ProjectUrl);
            }
            if (builder.Tags.Any())
            {
                WriteLine("Tags: {0}", string.Join(", ", builder.Tags));
            }
            if (builder.DependencyGroups.Any())
            {
                WriteLine("Dependencies: {0}", string.Join(", ", builder.DependencyGroups.SelectMany(d => d.Packages).Select(d => d.ToString())));
            }
            else
            {
                WriteLine("Dependencies: None");
            }

            WriteLine(string.Empty);

            foreach (string file in package.GetFiles().OrderBy(p => p))
            {
                WriteLine(Strings.Log_PackageCommandAddedFile, file);
            }

            WriteLine(string.Empty);
        }

        internal void ExcludeFiles(ICollection<IPackageFile> packageFiles)
        {
            // Always exclude the nuspec file
            // Review: This exclusion should be done by the package builder because it knows which file would collide with the auto-generated
            // manifest file.
            IEnumerable<string> wildCards = _excludes.Concat(new[] { "**" + NuGetConstants.ManifestExtension });

            if (!_packArgs.NoDefaultExcludes)
            {
                // The user has not explicitly disabled default filtering.
                IEnumerable<IPackageFile> excludedFiles = RemoveDefaultExclusions(packageFiles);
                if (excludedFiles != null)
                {
                    foreach (IPackageFile file in excludedFiles)
                    {
                        if (file is PhysicalPackageFile)
                        {
                            var physicalPackageFile = file as PhysicalPackageFile;
                            _packArgs.Logger.Log(
                                PackagingLogMessage.CreateWarning(
                                    string.Format(
                                        CultureInfo.CurrentCulture,
                                        Strings.Warning_FileExcludedByDefault,
                                        physicalPackageFile.SourcePath),
                                    NuGetLogCode.NU5119));
                        }
                    }
                }
            }

            wildCards = wildCards.Concat(_packArgs.Exclude);

            PathResolver.FilterPackageFiles(packageFiles, ResolvePath, wildCards);
        }

        private IEnumerable<IPackageFile> RemoveDefaultExclusions(ICollection<IPackageFile> packageFiles)
        {
            string basePath = string.IsNullOrEmpty(_packArgs.BasePath) ? _packArgs.CurrentDirectory : _packArgs.BasePath;

            var matches = packageFiles.Where(packageFile =>
            {
                var filePath = ResolvePath(packageFile, basePath);
                var fileName = Path.GetFileName(filePath);

                return fileName.EndsWith(".nupkg", StringComparison.OrdinalIgnoreCase)
                    || (fileName.StartsWith(".", StringComparison.Ordinal) && fileName.IndexOf(".", startIndex: 1, StringComparison.Ordinal) == -1);
            });

            var matchedFiles = new HashSet<IPackageFile>(matches);
            List<IPackageFile> toRemove = packageFiles.Where(matchedFiles.Contains).ToList();

            foreach (IPackageFile item in toRemove)
            {
                packageFiles.Remove(item);
            }

            return toRemove;
        }

        private string ResolvePath(IPackageFile packageFile)
        {
            string basePath = string.IsNullOrEmpty(_packArgs.BasePath) ? _packArgs.CurrentDirectory : _packArgs.BasePath;

            return ResolvePath(packageFile, basePath);
        }

        private static string ResolvePath(IPackageFile packageFile, string basePath)
        {
            var physicalPackageFile = packageFile as PhysicalPackageFile;

            // For PhysicalPackageFiles, we want to filter by SourcePaths, the path on disk. The Path value maps to the TargetPath
            if (physicalPackageFile == null)
            {
                return packageFile.Path;
            }

            string path = physicalPackageFile.SourcePath;

            // Make sure that the basepath has a directory separator
            int index = path.IndexOf(PathUtility.EnsureTrailingSlash(basePath), StringComparison.OrdinalIgnoreCase);
            if (index != -1)
            {
                // Since wildcards are going to be relative to the base path, remove the BasePath portion of the file's source path.
                // Also remove any leading path separator slashes
                path = path.Substring(index + basePath.Length).TrimStart(Path.DirectorySeparatorChar);
            }

            return path;
        }

        private bool BuildSymbolsPackage(string path)
        {
            PackageBuilder symbolsBuilder = CreatePackageBuilderFromNuspec(path);
            if (_packArgs.SymbolPackageFormat == SymbolPackageFormat.Snupkg) // Snupkgs can only have 1 PackageType. 
            {
                symbolsBuilder.PackageTypes.Clear();
                symbolsBuilder.PackageTypes.Add(PackageType.SymbolsPackage);
                // Remove the references when building the symbols package.
                // They are not relevant for the symbols packages (snupkgs specifically).
                symbolsBuilder.PackageAssemblyReferences.Clear();
            }

            // remove unnecessary files when building the symbols package
            ExcludeFilesForSymbolPackage(symbolsBuilder.Files, _packArgs.SymbolPackageFormat);

            if (!symbolsBuilder.Files.Any())
            {
                throw new PackagingException(
                    NuGetLogCode.NU5005,
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.Error_PackageCommandNoFilesForSymbolsPackage,
                        path,
                        Strings.NuGetDocs));
            }

            string outputPath = GetOutputPath(symbolsBuilder, _packArgs, symbols: true);

            InitCommonPackageBuilderProperties(symbolsBuilder);
            return BuildPackage(symbolsBuilder, outputPath, symbolsPackage: false);
        }

        internal void AnalyzePackage(PackageArchiveReader package)
        {
            IEnumerable<IPackageRule> packageRules = Rules;
            IList<PackagingLogMessage> logMessages = new List<PackagingLogMessage>();

            foreach (IPackageRule rule in packageRules)
            {
                logMessages.AddRange(rule.Validate(package).OrderBy(p => p.Code.ToString(), StringComparer.CurrentCulture));
            }

            if (logMessages.Count > 0)
            {
                foreach (PackagingLogMessage logMessage in logMessages)
                {
                    PrintPackageLogMessage(logMessage);
                }
            }
        }

        private void PrintPackageLogMessage(PackagingLogMessage message)
        {
            if (!string.IsNullOrEmpty(message.Message))
            {
                _packArgs.Logger.Log(message);
            }
        }

        internal static void ExcludeFilesForLibPackage(ICollection<IPackageFile> files)
        {
            PathResolver.FilterPackageFiles(files, file => file.Path, LibPackageExcludes);
        }

        internal static void ExcludeFilesForSymbolPackage(ICollection<IPackageFile> files, SymbolPackageFormat symbolPackageFormat)
        {
            PathResolver.FilterPackageFiles(files, file => file.Path, SymbolPackageExcludes);
            if (symbolPackageFormat == SymbolPackageFormat.Snupkg)
            {
                List<IPackageFile> toRemove = files.Where(t => !string.Equals(Path.GetExtension(t.Path), ".pdb", StringComparison.OrdinalIgnoreCase)).ToList();
                foreach (IPackageFile fileToRemove in toRemove)
                {
                    files.Remove(fileToRemove);
                }
            }
        }

        // Gets the full path of the resulting nuget package including the file name
        public static string GetOutputPath(
            PackageBuilder builder,
            PackArgs packArgs,
            bool symbols = false,
            NuGetVersion nugetVersion = null,
            string outputDirectory = null,
            bool isNupkg = true)
        {
            NuGetVersion versionToUse;
            if (nugetVersion != null)
            {
                versionToUse = nugetVersion;
            }
            else
            {
                if (string.IsNullOrEmpty(packArgs.Version))
                {
                    if (builder.Version == null)
                    {
                        // If the version is null, the user will get an error later saying that a version
                        // is required. Specifying a version here just keeps it from throwing until
                        // it gets to the better error message. It won't actually get used.
                        versionToUse = NuGetVersion.Parse("1.0.0");
                    }
                    else
                    {
                        versionToUse = builder.Version;
                    }
                }
                else
                {
                    versionToUse = NuGetVersion.Parse(packArgs.Version);
                }
            }

            string outputFile = GetOutputFileName(builder.Id,
                versionToUse,
                isNupkg: isNupkg,
                symbols: symbols,
                symbolPackageFormat: packArgs.SymbolPackageFormat,
                excludeVersion: packArgs.OutputFileNamesWithoutVersion);

            string finalOutputDirectory = packArgs.OutputDirectory ?? packArgs.CurrentDirectory;
            finalOutputDirectory = outputDirectory ?? finalOutputDirectory;
            return Path.Combine(finalOutputDirectory, outputFile);
        }

        public static string GetOutputFileName(
            string packageId,
            NuGetVersion version,
            bool isNupkg,
            bool symbols,
            SymbolPackageFormat symbolPackageFormat,
            bool excludeVersion = false)
        {
            // Output file is {id}.{version}
            string normalizedVersion = version.ToNormalizedString();
            string outputFile = excludeVersion ? packageId : packageId + "." + normalizedVersion;

            string extension = isNupkg ? NuGetConstants.PackageExtension : NuGetConstants.ManifestExtension;
            string symbolsExtension = isNupkg
                ? (symbolPackageFormat == SymbolPackageFormat.Snupkg ? NuGetConstants.SnupkgExtension : NuGetConstants.SymbolsExtension)
                : NuGetConstants.ManifestSymbolsExtension;

            // If this is a source package then add .symbols.nupkg to the package file name
            if (symbols)
            {
                outputFile += symbolsExtension;
            }
            else
            {
                outputFile += extension;
            }

            return outputFile;
        }

        public static void SetupCurrentDirectory(PackArgs packArgs)
        {
            string directory = Path.GetDirectoryName(packArgs.Path);

            if (!directory.Equals(packArgs.CurrentDirectory, StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrEmpty(packArgs.OutputDirectory))
                {
                    packArgs.OutputDirectory = packArgs.CurrentDirectory;
                }
                packArgs.OutputDirectory = Path.GetFullPath(packArgs.OutputDirectory);

                if (!string.IsNullOrEmpty(packArgs.BasePath))
                {
                    // Make sure base path is not relative before changing the current directory
                    packArgs.BasePath = Path.GetFullPath(packArgs.BasePath);
                }

                packArgs.CurrentDirectory = directory;
                Directory.SetCurrentDirectory(packArgs.CurrentDirectory);
            }
        }

        public static string GetInputFile(PackArgs packArgs)
        {
            IEnumerable<string> files = packArgs.Arguments != null && packArgs.Arguments.Any()
                ? packArgs.Arguments : Directory.GetFiles(packArgs.CurrentDirectory);

            return GetInputFile(packArgs, files);
        }

        internal static string GetInputFile(PackArgs packArgs, IEnumerable<string> files)
        {
            if (files.Count() == 1 && Directory.Exists(files.First()))
            {
                string first = files.First();
                files = Directory.GetFiles(first);
            }

            List<string> candidates = files.Where(file => AllowedExtensions.Contains(Path.GetExtension(file))).ToList();
            string result;

            candidates.RemoveAll(ext => ext.EndsWith(".lock.json", StringComparison.OrdinalIgnoreCase) ||
                                    (ext.EndsWith(".json", StringComparison.OrdinalIgnoreCase) &&
                                    !Path.GetFileName(ext).Equals(ProjectJsonPathUtilities.ProjectConfigFileName, StringComparison.OrdinalIgnoreCase) &&
                                    !ext.EndsWith(ProjectJsonPathUtilities.ProjectConfigFileEnding, StringComparison.OrdinalIgnoreCase)));

            if (!candidates.Any())
            {
                throw new PackagingException(NuGetLogCode.NU5002, Strings.Error_InputFileNotSpecified);
            }
            if (candidates.Count == 1)
            {
                result = candidates[0];
            }
            else
            {
                // Remove all nuspec files
                candidates.RemoveAll(file => Path.GetExtension(file).Equals(NuGetConstants.ManifestExtension, StringComparison.OrdinalIgnoreCase));
                if (candidates.Count == 1)
                {
                    result = candidates[0];
                }
                else
                {
                    // Remove all json files
                    candidates.RemoveAll(file => Path.GetExtension(file).Equals(".json", StringComparison.OrdinalIgnoreCase));
                    if (candidates.Count == 1)
                    {
                        result = candidates[0];
                    }
                    else
                    {
                        throw new PackagingException(NuGetLogCode.NU5002, Strings.Error_InputFileNotSpecified);
                    }
                }
            }

            return Path.Combine(packArgs.CurrentDirectory, result);
        }

        private void WriteLine(string message, object arg = null)
        {
            _packArgs.Logger.Log(
                PackagingLogMessage.CreateMessage(
                    string.Format(CultureInfo.CurrentCulture, message, arg?.ToString()),
                    LogLevel.Information));
        }

        public static void AddLibraryDependency(LibraryDependency dependency, ISet<LibraryDependency> list)
        {
            if (list.Any(r => r.Name == dependency.Name))
            {
                LibraryDependency matchingDependency = list.Single(r => r.Name == dependency.Name);
                VersionRange newVersionRange = VersionRange.CommonSubSet(new VersionRange[]
                {
                    matchingDependency.LibraryRange.VersionRange, dependency.LibraryRange.VersionRange
                });
                if (!newVersionRange.Equals(VersionRange.None))
                {
                    list.Remove(matchingDependency);
                    list.Add(new LibraryDependency()
                    {
                        LibraryRange = new LibraryRange(matchingDependency.Name, newVersionRange, LibraryDependencyTarget.All),
                        IncludeType = dependency.IncludeType & matchingDependency.IncludeType,
                        SuppressParent = dependency.SuppressParent & matchingDependency.SuppressParent
                    });
                }
                else
                {
                    throw new PackagingException(
                        NuGetLogCode.NU5016,
                        string.Format(
                            CultureInfo.CurrentCulture,
                            Strings.Error_InvalidDependencyVersionConstraints,
                            dependency.Name));
                }
            }
            else
            {
                list.Add(dependency);
            }
        }

        public static void AddPackageDependency(PackageDependency dependency, ISet<PackageDependency> set)
        {
            PackageDependency matchingDependency = set.SingleOrDefault(r => r.Id == dependency.Id);
            if (matchingDependency != null)
            {
                VersionRange newVersionRange = VersionRange.CommonSubSet(new VersionRange[]
                {
                    matchingDependency.VersionRange, dependency.VersionRange
                });
                if (!newVersionRange.Equals(VersionRange.None))
                {
                    set.Remove(matchingDependency);
                    set.Add(new PackageDependency(dependency.Id, newVersionRange, dependency.Include, dependency.Exclude));
                }
                else
                {
                    throw new PackagingException(
                        NuGetLogCode.NU5016,
                        string.Format(
                            CultureInfo.CurrentCulture,
                            Strings.Error_InvalidDependencyVersionConstraints,
                            dependency.Id));
                }
            }
            else
            {
                set.Add(dependency);
            }
        }
    }
}

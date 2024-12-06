// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Packaging.Licenses;
using NuGet.ProjectModel;
using NuGet.Versioning;
using PackageSpecificWarningProperties = NuGet.Commands.PackCommand.PackageSpecificWarningProperties;

namespace NuGet.Build.Tasks.Pack
{
    public class PackTaskLogic : IPackTaskLogic
    {
        private const string IdentityProperty = "Identity";
        private PackageSpecificWarningProperties _packageSpecificWarningProperties;

        public PackArgs GetPackArgs(IPackTaskRequest<IMSBuildItem> request)
        {
            var packArgs = new PackArgs
            {
                InstallPackageToOutputPath = request.InstallPackageToOutputPath,
                OutputFileNamesWithoutVersion = request.OutputFileNamesWithoutVersion,
                OutputDirectory = request.PackageOutputPath,
                Serviceable = request.Serviceable,
                Tool = request.IsTool,
                Symbols = request.IncludeSymbols,
                SymbolPackageFormat = PackArgs.GetSymbolPackageFormat(request.SymbolPackageFormat),
                BasePath = request.NuspecBasePath,
                NoPackageAnalysis = request.NoPackageAnalysis,
                NoDefaultExcludes = request.NoDefaultExcludes,
                WarningProperties = WarningProperties.GetWarningProperties(request.TreatWarningsAsErrors, request.WarningsAsErrors, request.NoWarn, request.WarningsNotAsErrors),
                PackTargetArgs = new MSBuildPackTargetArgs()
            };

            packArgs.Logger = new PackCollectorLogger(request.Logger, packArgs.WarningProperties, _packageSpecificWarningProperties);

            if (request.MinClientVersion != null)
            {
                Version version;
                if (!Version.TryParse(request.MinClientVersion, out version))
                {
                    throw new PackagingException(NuGetLogCode.NU5022, string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.InvalidMinClientVersion,
                        request.MinClientVersion));
                }

                packArgs.MinClientVersion = version;
            }

            LockFile assetsFile = GetAssetsFile(request);
            var aliases = new Dictionary<string, string>();
            foreach (var tfm in assetsFile.PackageSpec.TargetFrameworks)
            {
                aliases[tfm.TargetAlias] = tfm.FrameworkName.GetShortFolderName();
            }

            InitCurrentDirectoryAndFileName(request, packArgs);
            InitNuspecOutputPath(request, packArgs);
            PackCommandRunner.SetupCurrentDirectory(packArgs);

            if (!string.IsNullOrEmpty(request.NuspecFile))
            {
                if (request.NuspecProperties != null && request.NuspecProperties.Any())
                {
                    packArgs.Properties.AddRange(ParsePropertiesAsDictionary(request.NuspecProperties));
                    if (packArgs.Properties.TryGetValue("version", out var version))
                    {
                        packArgs.Version = version;
                    }
                }
            }
            else
            {
                // This only needs to happen when packing via csproj, not nuspec.
                packArgs.PackTargetArgs.AllowedOutputExtensionsInPackageBuildOutputFolder = InitOutputExtensions(request.AllowedOutputExtensionsInPackageBuildOutputFolder);
                packArgs.PackTargetArgs.AllowedOutputExtensionsInSymbolsPackageBuildOutputFolder = InitOutputExtensions(request.AllowedOutputExtensionsInSymbolsPackageBuildOutputFolder);
                packArgs.PackTargetArgs.TargetPathsToAssemblies = InitLibFiles(request.BuildOutputInPackage, aliases);
                packArgs.PackTargetArgs.TargetPathsToSymbols = InitLibFiles(request.TargetPathsToSymbols, aliases);
                packArgs.PackTargetArgs.AssemblyName = request.AssemblyName;
                packArgs.PackTargetArgs.IncludeBuildOutput = request.IncludeBuildOutput;
                packArgs.PackTargetArgs.BuildOutputFolder = request.BuildOutputFolders;
                packArgs.PackTargetArgs.TargetFrameworks = ParseFrameworks(request, aliases);

                if (request.IncludeSource)
                {
                    packArgs.PackTargetArgs.SourceFiles = GetSourceFiles(request, packArgs.CurrentDirectory);
                    packArgs.Symbols = request.IncludeSource;
                }

                var contentFiles = ProcessContentToIncludeInPackage(request, packArgs);
                packArgs.PackTargetArgs.ContentFiles = contentFiles;
            }

            return packArgs;
        }

        public PackageBuilder GetPackageBuilder(IPackTaskRequest<IMSBuildItem> request)
        {
            // Load the assets JSON file produced by restore.
            var assetsFilePath = Path.Combine(request.RestoreOutputPath, LockFileFormat.AssetsFileName);
            if (!File.Exists(assetsFilePath))
            {
                throw new PackagingException(NuGetLogCode.NU5023, string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.AssetsFileNotFound,
                    assetsFilePath));
            }

            var builder = new PackageBuilder(request.Deterministic, request.Logger)
            {
                Id = request.PackageId,
                Description = request.Description,
                Title = request.Title,
                Copyright = request.Copyright,
                ReleaseNotes = request.ReleaseNotes,
                RequireLicenseAcceptance = request.RequireLicenseAcceptance,
                EmitRequireLicenseAcceptance = request.RequireLicenseAcceptance,
                PackageTypes = ParsePackageTypes(request)
            };

            if (request.DevelopmentDependency)
            {
                builder.DevelopmentDependency = true;
            }

            if (request.PackageVersion != null)
            {
                NuGetVersion version;
                if (!NuGetVersion.TryParse(request.PackageVersion, out version))
                {
                    throw new PackagingException(NuGetLogCode.NU5024, string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.InvalidPackageVersion,
                        request.PackageVersion));
                }
                builder.Version = version;
            }
            else
            {
                builder.Version = new NuGetVersion("1.0.0");
            }

            if (request.Authors != null)
            {
                builder.Authors.AddRange(request.Authors);
            }

            if (request.Tags != null)
            {
                builder.Tags.AddRange(request.Tags);
            }

            Uri tempUri;
            if (Uri.TryCreate(request.LicenseUrl, UriKind.Absolute, out tempUri))
            {
                builder.LicenseUrl = tempUri;
            }
            if (Uri.TryCreate(request.ProjectUrl, UriKind.Absolute, out tempUri))
            {
                builder.ProjectUrl = tempUri;
            }
            if (Uri.TryCreate(request.IconUrl, UriKind.Absolute, out tempUri))
            {
                builder.IconUrl = tempUri;
            }
            if (!string.IsNullOrEmpty(request.RepositoryUrl) || !string.IsNullOrEmpty(request.RepositoryType))
            {
                builder.Repository = new RepositoryMetadata(
                    request.RepositoryType,
                    request.RepositoryUrl,
                    request.RepositoryBranch,
                    request.RepositoryCommit);
            }

            builder.LicenseMetadata = BuildLicenseMetadata(request);

            builder.Icon = request.PackageIcon;

            builder.Readme = request.Readme;

            if (request.MinClientVersion != null)
            {
                Version version;
                if (!Version.TryParse(request.MinClientVersion, out version))
                {
                    throw new PackagingException(NuGetLogCode.NU5022, string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.InvalidMinClientVersion,
                        request.MinClientVersion));
                }

                builder.MinClientVersion = version;
            }

            // The assets file is necessary for project and package references. Pack should not do any traversal,
            // so we leave that work up to restore (which produces the assets file).
            var lockFileFormat = new LockFileFormat();
            var assetsFile = lockFileFormat.Read(assetsFilePath);

            if (assetsFile.PackageSpec == null)
            {
                throw new PackagingException(NuGetLogCode.NU5025, string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.AssetsFileDoesNotHaveValidPackageSpec,
                    assetsFilePath));
            }

            var projectRefToVersionMap = new Dictionary<string, string>(PathUtility.GetStringComparerBasedOnOS());

            if (request.ProjectReferencesWithVersions != null && request.ProjectReferencesWithVersions.Any())
            {
                projectRefToVersionMap = request
                    .ProjectReferencesWithVersions
                    .ToDictionary(msbuildItem => msbuildItem.Identity,
                    msbuildItem => msbuildItem.GetProperty("ProjectVersion"), PathUtility.GetStringComparerBasedOnOS());
            }

            var aliases = new Dictionary<string, string>();
            foreach (var tfm in assetsFile.PackageSpec.TargetFrameworks)
            {
                aliases[tfm.TargetAlias] = tfm.FrameworkName.GetShortFolderName();
            }

            var nuGetFrameworkComparer = NuGetFrameworkFullComparer.Instance;
            var frameworksWithSuppressedDependencies = new HashSet<NuGetFramework>(nuGetFrameworkComparer);
            if (request.FrameworksWithSuppressedDependencies != null && request.FrameworksWithSuppressedDependencies.Any())
            {
                frameworksWithSuppressedDependencies =
                    new HashSet<NuGetFramework>(request.FrameworksWithSuppressedDependencies
                    .Select(t =>
                    {
                        if (aliases.TryGetValue(t.Identity, out string translated))
                        {
                            return NuGetFramework.Parse(translated);
                        }
                        else
                        {
                            return NuGetFramework.Parse(t.Identity);
                        }
                    }).ToList(), nuGetFrameworkComparer);
            }

            PopulateProjectAndPackageReferences(builder,
                assetsFile,
                projectRefToVersionMap,
                frameworksWithSuppressedDependencies);

            PopulateFrameworkAssemblyReferences(builder, request);
            PopulateFrameworkReferences(builder, assetsFile);

            return builder;
        }

        private LicenseMetadata BuildLicenseMetadata(IPackTaskRequest<IMSBuildItem> request)
        {
            var hasLicenseExpression = !string.IsNullOrEmpty(request.PackageLicenseExpression);
            var hasLicenseFile = !string.IsNullOrEmpty(request.PackageLicenseFile);
            if (hasLicenseExpression || hasLicenseFile)
            {
                if (!string.IsNullOrEmpty(request.LicenseUrl))
                {
                    throw new PackagingException(NuGetLogCode.NU5035, string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.NuGetLicenses_LicenseUrlCannotBeUsedInConjuctionWithLicense));
                }

                if (hasLicenseExpression && hasLicenseFile)
                {
                    throw new PackagingException(NuGetLogCode.NU5033, string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.InvalidLicenseCombination,
                        request.PackageLicenseExpression));
                }

                var version = GetLicenseExpressionVersion(request);

                if (hasLicenseExpression)
                {
                    if (version.CompareTo(LicenseMetadata.CurrentVersion) <= 0)
                    {
                        try
                        {
                            var expression = NuGetLicenseExpression.Parse(request.PackageLicenseExpression);
                            return new LicenseMetadata(
                                type: LicenseType.Expression,
                                license: request.PackageLicenseExpression,
                                expression: expression,
                                warningsAndErrors: null,
                                version: version);
                        }
                        catch (NuGetLicenseExpressionParsingException e)
                        {
                            throw new PackagingException(NuGetLogCode.NU5032, string.Format(
                                   CultureInfo.CurrentCulture,
                                   Strings.InvalidLicenseExpression,
                                   request.PackageLicenseExpression, e.Message),
                                   e);
                        }
                    }
                    else
                    {
                        throw new PackagingException(NuGetLogCode.NU5034, string.Format(
                                   CultureInfo.CurrentCulture,
                                   Strings.InvalidLicenseExppressionVersion_VersionTooHigh,
                                   request.PackageLicenseExpressionVersion,
                                   LicenseMetadata.CurrentVersion));
                    }
                }
                if (hasLicenseFile)
                {
                    return new LicenseMetadata(
                        type: LicenseType.File,
                        license: request.PackageLicenseFile,
                        expression: null,
                        warningsAndErrors: null,
                        version: version);
                }
            }
            return null;
        }

        private static Version GetLicenseExpressionVersion(IPackTaskRequest<IMSBuildItem> request)
        {
            Version version;
            if (string.IsNullOrEmpty(request.PackageLicenseExpressionVersion))
            {
                version = LicenseMetadata.EmptyVersion;
            }
            else
            {
                if (!Version.TryParse(request.PackageLicenseExpressionVersion, out version))
                {
                    throw new PackagingException(NuGetLogCode.NU5034, string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.InvalidLicenseExpressionVersion,
                        request.PackageLicenseExpressionVersion));
                }
            }

            return version;
        }


        private LockFile GetAssetsFile(IPackTaskRequest<IMSBuildItem> request)
        {
            if (request.PackItem == null)
            {
                throw new PackagingException(NuGetLogCode.NU5028, Strings.NoPackItemProvided);
            }

            string assetsFilePath = Path.Combine(request.RestoreOutputPath, LockFileFormat.AssetsFileName);

            if (!File.Exists(assetsFilePath))
            {
                throw new InvalidOperationException(string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.AssetsFileNotFound,
                    assetsFilePath));
            }
            // The assets file is necessary for project and package references. Pack should not do any traversal,
            // so we leave that work up to restore (which produces the assets file).
            var lockFileFormat = new LockFileFormat();
            return lockFileFormat.Read(assetsFilePath);
        }
        private void PopulateFrameworkAssemblyReferences(PackageBuilder builder, IPackTaskRequest<IMSBuildItem> request)
        {
            // First add all the assembly references which are not specific to a certain TFM.
            var tfmSpecificRefs = new Dictionary<string, IList<string>>(StringComparer.OrdinalIgnoreCase);
            // Then add the TFM specific framework assembly references, and ignore any which have already been added above.
            foreach (var tfmRef in request.FrameworkAssemblyReferences)
            {
                var targetFramework = tfmRef.GetProperty("TargetFramework");

                if (tfmSpecificRefs.ContainsKey(tfmRef.Identity))
                {
                    tfmSpecificRefs[tfmRef.Identity].Add(targetFramework);
                }
                else
                {
                    tfmSpecificRefs.Add(tfmRef.Identity, new List<string>() { targetFramework });
                }
            }

            builder.FrameworkReferences.AddRange(
                tfmSpecificRefs.Select(
                    t => new FrameworkAssemblyReference(
                        t.Key, t.Value.Select(
                            k => NuGetFramework.Parse(k))
                            )));
        }

        private void PopulateFrameworkReferences(PackageBuilder builder, LockFile assetsFile)
        {
            var tfmSpecificRefs = new Dictionary<string, ISet<string>>();
            bool hasAnyRefs = false;

            foreach (var framework in assetsFile.PackageSpec.TargetFrameworks)
            {
                var frameworkShortFolderName = framework.FrameworkName.GetShortFolderName();
                tfmSpecificRefs.Add(frameworkShortFolderName, new HashSet<string>(ComparisonUtility.FrameworkReferenceNameComparer));
                foreach (var frameworkRef in framework.FrameworkReferences.Where(e => e.PrivateAssets != FrameworkDependencyFlags.All))
                {
                    var frameworkRefNames = tfmSpecificRefs[frameworkShortFolderName];
                    frameworkRefNames.Add(frameworkRef.Name);
                    hasAnyRefs = true;
                }
            }

            if (hasAnyRefs)
            {
                builder.FrameworkReferenceGroups.AddRange(
                    tfmSpecificRefs.Select(e =>
                        new FrameworkReferenceGroup(
                            NuGetFramework.Parse(e.Key),
                            e.Value.Select(fr => new FrameworkReference(fr)))));
            }
        }

        public PackCommandRunner GetPackCommandRunner(
            IPackTaskRequest<IMSBuildItem> request,
            PackArgs packArgs,
            PackageBuilder packageBuilder)
        {
            var runner = new PackCommandRunner(
                packArgs,
                MSBuildProjectFactory.ProjectCreator,
                packageBuilder);

            runner.GenerateNugetPackage = request.ContinuePackingAfterGeneratingNuspec;

            return runner;
        }

        public bool BuildPackage(PackCommandRunner runner)
        {
            return runner.RunPackageBuild();
        }

        private IEnumerable<OutputLibFile> InitLibFiles(IMSBuildItem[] libFiles, IDictionary<string, string> aliases)
        {
            var assemblies = new List<OutputLibFile>();
            if (libFiles == null)
            {
                return assemblies;
            }


            foreach (var assembly in libFiles)
            {
                var finalOutputPath = assembly.GetProperty("FinalOutputPath");

                // Fallback to using Identity if FinalOutputPath is not set.
                // See bug https://github.com/NuGet/Home/issues/5408
                if (string.IsNullOrEmpty(finalOutputPath))
                {
                    finalOutputPath = assembly.GetProperty(IdentityProperty);
                }

                var targetPath = assembly.GetProperty("TargetPath");
                var targetFramework = assembly.GetProperty("TargetFramework");

                if (!File.Exists(finalOutputPath))
                {
                    throw new PackagingException(NuGetLogCode.NU5026, string.Format(CultureInfo.CurrentCulture, Strings.Error_FileNotFound, finalOutputPath));
                }

                string translated = null;
                var succeeded = aliases.TryGetValue(targetFramework, out translated);
                if (succeeded)
                {
                    targetFramework = translated;
                }

                // If target path is not set, default it to the file name. Only satellite DLLs have a special target path
                // where culture is part of the target path. This condition holds true for files like runtimeconfig.json file
                // in netcore projects.
                if (targetPath == null)
                {
                    targetPath = Path.GetFileName(finalOutputPath);
                }

                if (string.IsNullOrEmpty(targetFramework) || NuGetFramework.Parse(targetFramework).IsSpecificFramework == false)
                {
                    throw new PackagingException(NuGetLogCode.NU5027, string.Format(CultureInfo.CurrentCulture, Strings.InvalidTargetFramework, finalOutputPath));
                }

                assemblies.Add(new OutputLibFile()
                {
                    FinalOutputPath = finalOutputPath,
                    TargetPath = targetPath,
                    TargetFramework = targetFramework
                });
            }

            return assemblies;
        }

        private ISet<NuGetFramework> ParseFrameworks(IPackTaskRequest<IMSBuildItem> request, IDictionary<string, string> aliases)
        {
            var nugetFrameworks = new HashSet<NuGetFramework>();
            if (request.TargetFrameworks != null)
            {
                nugetFrameworks = new HashSet<NuGetFramework>(request.TargetFrameworks.Select(targetFramework =>
                {
                    string translated = null;
                    var succeeded = aliases.TryGetValue(targetFramework, out translated);
                    if (succeeded)
                    {
                        targetFramework = translated;
                    }
                    return NuGetFramework.Parse(targetFramework);
                }));
            }

            return nugetFrameworks;
        }

        private ICollection<PackageType> ParsePackageTypes(IPackTaskRequest<IMSBuildItem> request)
        {
            var listOfPackageTypes = new List<PackageType>();
            if (request.PackageTypes != null)
            {
                foreach (var packageType in request.PackageTypes)
                {
                    var packageTypeSplitInPart = packageType.Split(new char[] { ',' });
                    var packageTypeName = packageTypeSplitInPart[0].Trim();
                    var version = PackageType.EmptyVersion;
                    if (packageTypeSplitInPart.Length > 1)
                    {
                        var versionString = packageTypeSplitInPart[1];
                        _ = Version.TryParse(versionString, out version);
                    }
                    listOfPackageTypes.Add(new PackageType(packageTypeName, version));
                }
            }
            return listOfPackageTypes;
        }

        private void InitCurrentDirectoryAndFileName(IPackTaskRequest<IMSBuildItem> request, PackArgs packArgs)
        {
            if (request.PackItem == null)
            {
                throw new PackagingException(NuGetLogCode.NU5028, Strings.NoPackItemProvided);
            }

            packArgs.CurrentDirectory = Path.Combine(
                request.PackItem.GetProperty("RootDir"),
                request.PackItem.GetProperty("Directory")).TrimEnd(Path.DirectorySeparatorChar);

            packArgs.Arguments = new string[]
            {
                !string.IsNullOrEmpty(request.NuspecFile)
                ? request.NuspecFile
                : string.Concat(request.PackItem.GetProperty("FileName"), request.PackItem.GetProperty("Extension"))
            };

            packArgs.Path = !string.IsNullOrEmpty(request.NuspecFile)
                ? request.NuspecFile
                : request.PackItem.GetProperty("FullPath");
            packArgs.Exclude = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        }

        private void InitNuspecOutputPath(IPackTaskRequest<IMSBuildItem> request, PackArgs packArgs)
        {
            if (Path.IsPathRooted(request.NuspecOutputPath))
            {
                packArgs.PackTargetArgs.NuspecOutputPath = request.NuspecOutputPath;
            }
            else
            {
                packArgs.PackTargetArgs.NuspecOutputPath = Path.Combine(
                    packArgs.CurrentDirectory,
                    request.NuspecOutputPath);
            }
        }

        private Dictionary<string, IEnumerable<ContentMetadata>> ProcessContentToIncludeInPackage(
            IPackTaskRequest<IMSBuildItem> request,
            PackArgs packArgs)
        {
            // This maps from source path on disk to target path inside the nupkg.
            var fileModel = new Dictionary<string, IEnumerable<ContentMetadata>>();
            if (request.PackageFiles != null)
            {
                var excludeFiles = CalculateFilesToExcludeInPack(request);
                foreach (var packageFile in request.PackageFiles)
                {
                    var sourcePath = GetSourcePath(packageFile);
                    if (excludeFiles.Contains(sourcePath))
                    {
                        continue;
                    }

                    var totalContentMetadata = GetContentMetadata(packageFile, sourcePath, packArgs, request.ContentTargetFolders);

                    if (fileModel.ContainsKey(sourcePath))
                    {
                        var existingContentMetadata = fileModel[sourcePath];
                        fileModel[sourcePath] = existingContentMetadata.Concat(totalContentMetadata);
                    }
                    else
                    {
                        var existingContentMetadata = new List<ContentMetadata>();
                        existingContentMetadata.AddRange(totalContentMetadata);
                        fileModel.Add(sourcePath, existingContentMetadata);
                    }
                }
            }

            return fileModel;
        }

        // The targetpaths returned from this function contain the directory in the nuget package where the file would go to. The filename is added later on to the target path.
        // whether or not the filename is added later on is dependent upon the fact that does the targetpath resolved here ends with a directory separator char or not.
        private IEnumerable<ContentMetadata> GetContentMetadata(IMSBuildItem packageFile, string sourcePath,
            PackArgs packArgs, string[] contentTargetFolders)
        {
            var targetPaths = contentTargetFolders
                .Select(PathUtility.EnsureTrailingSlash)
                .ToList();

            var isPackagePathSpecified = packageFile.Properties.Contains("PackagePath");
            // if user specified a PackagePath, then use that. Look for any ** which are indicated by the RecrusiveDir metadata in msbuild.
            if (isPackagePathSpecified)
            {
                // The rule here is that if the PackagePath is an empty string, then we add the file to the root of the package.
                // Instead if it is a ';' delimited string, then the user needs to specify a '\' to indicate that the file should go to the root of the package.

                var packagePathString = packageFile.GetProperty("PackagePath");
                targetPaths = packagePathString == null
                    ? new string[] { string.Empty }.ToList()
                    : MSBuildStringUtility.Split(packagePathString)
                    .Distinct()
                    .ToList();

                var recursiveDir = packageFile.GetProperty("RecursiveDir");
                // The below NuGetRecursiveDir workaround needs to be done due to msbuild bug https://github.com/Microsoft/msbuild/issues/3121
                recursiveDir = string.IsNullOrEmpty(recursiveDir) ? packageFile.GetProperty("NuGetRecursiveDir") : recursiveDir;
                if (!string.IsNullOrEmpty(recursiveDir))
                {
                    var newTargetPaths = new List<string>();
                    var fileName = Path.GetFileName(sourcePath);
                    foreach (var targetPath in targetPaths)
                    {
                        newTargetPaths.Add(PathUtility.GetStringComparerBasedOnOS().
                            Compare(Path.GetExtension(fileName),
                            Path.GetExtension(targetPath)) == 0
                            && !string.IsNullOrEmpty(Path.GetExtension(fileName))
                                ? targetPath
                                : Path.Combine(targetPath, recursiveDir));
                    }

                    targetPaths = newTargetPaths;
                }
            }

            var buildActionString = packageFile.GetProperty("BuildAction");
            var buildAction = BuildAction.Parse(string.IsNullOrEmpty(buildActionString) ? "None" : buildActionString);

            // TODO: Do the work to get the right language of the project, tracked via https://github.com/NuGet/Home/issues/4100
            var language = buildAction.Equals(BuildAction.Compile) ? "cs" : "any";


            var setOfTargetPaths = new HashSet<string>(targetPaths, PathUtility.GetStringComparerBasedOnOS());

            // If package path wasn't specified, then we expand the "contentFiles" value we
            // got from ContentTargetFolders and expand it to contentFiles/any/<TFM>/
            if (!isPackagePathSpecified)
            {
                if (setOfTargetPaths.Remove("contentFiles" + Path.DirectorySeparatorChar)
                || setOfTargetPaths.Remove("contentFiles"))
                {
                    foreach (var framework in packArgs.PackTargetArgs.TargetFrameworks)
                    {
                        setOfTargetPaths.Add(PathUtility.EnsureTrailingSlash(
                            Path.Combine("contentFiles", language, framework.GetShortFolderName()
                            )));
                    }
                }
            }

            // this  if condition means there is no package path provided, file is within the project directory
            // and the target path should preserve this relative directory structure.
            // This case would be something like :
            // <Content Include= "folderA\folderB\abc.txt">
            // Since the package path wasn't specified, we will add this to the package paths obtained via ContentTargetFolders and preserve
            // relative directory structure
            if (!isPackagePathSpecified &&
                sourcePath.StartsWith(packArgs.CurrentDirectory, StringComparison.CurrentCultureIgnoreCase) &&
                     !Path.GetFileName(sourcePath)
                         .Equals(packageFile.GetProperty(IdentityProperty), StringComparison.CurrentCultureIgnoreCase))
            {
                var newTargetPaths = new List<string>();
                var identity = packageFile.GetProperty(IdentityProperty);

                // Identity can be a rooted absolute path too, in which case find the path relative to the current directory
                if (Path.IsPathRooted(identity))
                {
                    identity = PathUtility.GetRelativePath(PathUtility.EnsureTrailingSlash(packArgs.CurrentDirectory), identity);
                    identity = Path.GetDirectoryName(identity);
                }

                // If identity is not a rooted path, then it is a relative path to the project directory
                else if (identity.EndsWith(Path.GetFileName(sourcePath), StringComparison.CurrentCultureIgnoreCase))
                {
                    identity = Path.GetDirectoryName(identity);
                }

                foreach (var targetPath in setOfTargetPaths)
                {
                    var newTargetPath = Path.Combine(targetPath, identity);
                    // We need to do this because evaluated identity in the above line of code can be an empty string
                    // in the case when the original identity string was the absolute path to a file in project directory, and is in
                    // the same directory as the csproj file.
                    newTargetPath = PathUtility.EnsureTrailingSlash(newTargetPath);
                    newTargetPaths.Add(newTargetPath);
                }
                setOfTargetPaths = new HashSet<string>(newTargetPaths, PathUtility.GetStringComparerBasedOnOS());
            }

            // we take the final set of evaluated target paths and append the file name to it if not
            // already done. we check whether the extension of the target path is the same as the extension
            // of the source path and add the filename accordingly.
            var totalSetOfTargetPaths = new List<string>();
            foreach (var targetPath in setOfTargetPaths)
            {
                var currentPath = targetPath;
                var fileName = Path.GetFileName(sourcePath);
                if (string.IsNullOrEmpty(Path.GetExtension(fileName)) ||
                    !Path.GetExtension(fileName)
                    .Equals(Path.GetExtension(targetPath), StringComparison.OrdinalIgnoreCase))
                {
                    currentPath = Path.Combine(targetPath, fileName);
                }
                totalSetOfTargetPaths.Add(currentPath);
            }

            return totalSetOfTargetPaths.Select(target => new ContentMetadata()
            {
                BuildAction = buildAction.Value,
                Source = sourcePath,
                Target = target,
                CopyToOutput = packageFile.GetProperty("PackageCopyToOutput"),
                Flatten = packageFile.GetProperty("PackageFlatten")
            });
        }

        private string GetSourcePath(IMSBuildItem packageFile)
        {
            var sourcePath = packageFile.GetProperty("FullPath");
            if (packageFile.Properties.Contains("MSBuildSourceProjectFile"))
            {
                var sourceProjectFile = packageFile.GetProperty("MSBuildSourceProjectFile");
                var identity = packageFile.GetProperty(IdentityProperty);
                sourcePath = Path.Combine(sourceProjectFile.Replace(Path.GetFileName(sourceProjectFile), string.Empty), identity);
            }
            return Path.GetFullPath(sourcePath);
        }

        private ISet<string> CalculateFilesToExcludeInPack(IPackTaskRequest<IMSBuildItem> request)
        {
            var excludeFiles = new HashSet<string>();
            if (request.PackageFilesToExclude != null)
            {
                foreach (var file in request.PackageFilesToExclude)
                {
                    var sourcePath = GetSourcePath(file);
                    excludeFiles.Add(sourcePath);
                }
            }
            return excludeFiles;
        }

        private IDictionary<string, string> GetSourceFiles(IPackTaskRequest<IMSBuildItem> request, string currentProjectDirectory)
        {
            var sourceFiles = new Dictionary<string, string>();
            if (request.SourceFiles != null)
            {
                foreach (var src in request.SourceFiles)
                {
                    var sourcePath = GetSourcePath(src);
                    var sourceProjectFile = currentProjectDirectory;
                    if (src.Properties.Contains("MSBuildSourceProjectFile"))
                    {
                        sourceProjectFile = src.GetProperty("MSBuildSourceProjectFile");
                        sourceProjectFile = Path.GetDirectoryName(sourceProjectFile);
                    }

                    sourceFiles[sourcePath] = sourceProjectFile;
                }
            }
            return sourceFiles;
        }

        private void PopulateProjectAndPackageReferences(PackageBuilder packageBuilder, LockFile assetsFile,
            IDictionary<string, string> projectRefToVersionMap,
            ISet<NuGetFramework> frameworksWithSuppressedDependencies)
        {
            var dependenciesByFramework = new Dictionary<NuGetFramework, HashSet<LibraryDependency>>();

            InitializeProjectDependencies(assetsFile, dependenciesByFramework, projectRefToVersionMap, frameworksWithSuppressedDependencies);
            InitializePackageDependencies(assetsFile, dependenciesByFramework, frameworksWithSuppressedDependencies);

            foreach (var pair in dependenciesByFramework)
            {
                PackCommandRunner.AddDependencyGroups(pair.Value, pair.Key, packageBuilder);
            }
        }

        private static void InitializeProjectDependencies(
            LockFile assetsFile,
            IDictionary<NuGetFramework, HashSet<LibraryDependency>> dependenciesByFramework,
            IDictionary<string, string> projectRefToVersionMap,
            ISet<NuGetFramework> frameworkWithSuppressedDependencies)
        {
            // From the package spec, all we know is each absolute path to the project reference the the target
            // framework that project reference applies to.

            if (assetsFile.PackageSpec.RestoreMetadata == null)
            {
                return;
            }

            // Using the libraries section of the assets file, the library name and version for the project path.
            var projectPathToLibraryIdentities = assetsFile
                .Libraries
                .Where(library => library.MSBuildProject != null)
                .ToLookup(
                    library => Path.GetFullPath(Path.Combine(
                        Path.GetDirectoryName(assetsFile.PackageSpec.RestoreMetadata.ProjectPath),
                        PathUtility.GetPathWithDirectorySeparator(library.MSBuildProject))),
                    library => new PackageIdentity(library.Name, library.Version),
                    PathUtility.GetStringComparerBasedOnOS());

            // Consider all of the project references, grouped by target framework.
            foreach (var framework in assetsFile.PackageSpec.RestoreMetadata.TargetFrameworks)
            {
                var target = assetsFile.GetTarget(framework.FrameworkName, runtimeIdentifier: null);
                if (target == null || frameworkWithSuppressedDependencies.Contains(framework.FrameworkName))
                {
                    continue;
                }

                HashSet<LibraryDependency> dependencies;
                if (!dependenciesByFramework.TryGetValue(framework.FrameworkName, out dependencies))
                {
                    dependencies = new HashSet<LibraryDependency>();
                    dependenciesByFramework[framework.FrameworkName] = dependencies;
                }

                // For the current target framework, create a map from library identity to library model. This allows
                // us to be sure we have picked the correct library (name and version) for this target framework.
                var libraryIdentityToTargetLibrary = target
                    .Libraries
                    .ToLookup(library => new PackageIdentity(library.Name, library.Version));

                foreach (var projectReference in framework.ProjectReferences)
                {
                    var libraryIdentities = projectPathToLibraryIdentities[projectReference.ProjectPath];

                    var targetLibrary = libraryIdentities
                       .Select(identity => libraryIdentityToTargetLibrary[identity].FirstOrDefault())
                       .FirstOrDefault(library => library != null);

                    if (targetLibrary == null)
                    {
                        continue;
                    }

                    var versionToUse = new VersionRange(targetLibrary.Version);

                    // Use the project reference version obtained at build time if it exists, otherwise fallback to the one in assets file.
                    if (projectRefToVersionMap.TryGetValue(projectReference.ProjectPath, out var projectRefVersion))
                    {
                        versionToUse = VersionRange.Parse(projectRefVersion, allowFloating: false);
                    }
                    // TODO: Implement <TreatAsPackageReference>false</TreatAsPackageReference>
                    //   https://github.com/NuGet/Home/issues/3891
                    //
                    // For now, assume the project reference is a package dependency.
                    var projectDependency = new LibraryDependency()
                    {
                        LibraryRange = new LibraryRange(
                            targetLibrary.Name,
                            versionToUse,
                            LibraryDependencyTarget.All),
                        IncludeType = projectReference.IncludeAssets & ~projectReference.ExcludeAssets,
                        SuppressParent = projectReference.PrivateAssets
                    };

                    PackCommandRunner.AddLibraryDependency(projectDependency, dependencies);
                }
            }
        }

        private void InitializePackageDependencies(
            LockFile assetsFile,
            Dictionary<NuGetFramework, HashSet<LibraryDependency>> dependenciesByFramework,
            ISet<NuGetFramework> frameworkWithSuppressedDependencies)
        {
            var packageSpecificNoWarnProperties = new Dictionary<string, HashSet<(NuGetLogCode, NuGetFramework)>>(StringComparer.OrdinalIgnoreCase);
            var frameworks = assetsFile.PackageSpec.TargetFrameworks;

            // From the package spec, we know the direct package dependencies of this project.
            for (var i = 0; i < frameworks.Count; i++)
            {
                var framework = frameworks[i];

                if (frameworkWithSuppressedDependencies.Contains(framework.FrameworkName))
                {
                    continue;
                }

                // First, add each of the generic package dependencies
                AddDependencies(assetsFile.PackageSpec.Dependencies, dependenciesByFramework, framework, assetsFile, packageSpecificNoWarnProperties);

                // Next, the framework-specific dependencies
                var newFrameworkDependencies = AddDependencies(framework.Dependencies, dependenciesByFramework, framework, assetsFile, packageSpecificNoWarnProperties);
                framework = new TargetFrameworkInformation(framework) { Dependencies = newFrameworkDependencies };

                // Next, the central transitive dependencies
                foreach (var centralTDG in assetsFile.CentralTransitiveDependencyGroups)
                {
                    if (centralTDG.FrameworkName.Equals(framework.FrameworkName.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        AddDependencies(centralTDG.TransitiveDependencies.ToList(), dependenciesByFramework, framework, assetsFile, packageSpecificNoWarnProperties);
                    }
                }

                frameworks[i] = framework;
            }

            if (packageSpecificNoWarnProperties.Keys.Count > 0)
            {
                _packageSpecificWarningProperties = PackageSpecificWarningProperties.CreatePackageSpecificWarningProperties(packageSpecificNoWarnProperties);
            }
        }

        private static void AddDependencies(
            IList<LibraryDependency> packageDependencies,
            Dictionary<NuGetFramework, HashSet<LibraryDependency>> dependenciesByFramework,
            TargetFrameworkInformation framework,
            LockFile assetsFile,
            Dictionary<string, HashSet<(NuGetLogCode, NuGetFramework)>> packageSpecificNoWarnProperties)
        {
            HashSet<LibraryDependency> dependencies;
            if (!dependenciesByFramework.TryGetValue(framework.FrameworkName, out dependencies))
            {
                dependencies = new HashSet<LibraryDependency>();
                dependenciesByFramework[framework.FrameworkName] = dependencies;
            }

            // Add each package dependency.
            for (int i = 0; i < packageDependencies.Count; i++)
            {
                var updatedPackageDependency = GetUpdatedPackageDependency(packageDependencies[i], assetsFile, framework, packageSpecificNoWarnProperties, dependencies);
                packageDependencies[i] = updatedPackageDependency;
            }
        }

        private static ImmutableArray<LibraryDependency> AddDependencies(
            ImmutableArray<LibraryDependency> packageDependencies,
            Dictionary<NuGetFramework, HashSet<LibraryDependency>> dependenciesByFramework,
            TargetFrameworkInformation framework,
            LockFile assetsFile,
            Dictionary<string, HashSet<(NuGetLogCode, NuGetFramework)>> packageSpecificNoWarnProperties)
        {
            HashSet<LibraryDependency> dependencies;
            if (!dependenciesByFramework.TryGetValue(framework.FrameworkName, out dependencies))
            {
                dependencies = new HashSet<LibraryDependency>();
                dependenciesByFramework[framework.FrameworkName] = dependencies;
            }

            LibraryDependency[] updatedDependencies = new LibraryDependency[packageDependencies.Length];

            // Add each package dependency.
            for (var i = 0; i < packageDependencies.Length; i++)
            {
                var updatedPackageDependency = GetUpdatedPackageDependency(packageDependencies[i], assetsFile, framework, packageSpecificNoWarnProperties, dependencies);
                updatedDependencies[i] = updatedPackageDependency;
            }

            return ImmutableCollectionsMarshal.AsImmutableArray(updatedDependencies);
        }

        private static LibraryDependency GetUpdatedPackageDependency(
            LibraryDependency packageDependency,
            LockFile assetsFile,
            TargetFrameworkInformation framework,
            Dictionary<string, HashSet<(NuGetLogCode, NuGetFramework)>> packageSpecificNoWarnProperties,
            HashSet<LibraryDependency> dependencies)
        {
            // If we have a floating package dependency like 1.2.3-xyz-*, we
            // use the version of the package that restore resolved it to.
            if (packageDependency.LibraryRange.VersionRange.IsFloating)
            {
                var lockFileTarget = assetsFile.GetTarget(framework.FrameworkName, runtimeIdentifier: null);
                var package = lockFileTarget.Libraries.First(
                    library =>
                        string.Equals(library.Name, packageDependency.Name, StringComparison.OrdinalIgnoreCase));

                VersionRange versionRange;
                if (packageDependency.LibraryRange.VersionRange.HasUpperBound)
                {
                    versionRange = new VersionRange(
                        minVersion: package.Version,
                        includeMinVersion: packageDependency.LibraryRange.VersionRange.IsMinInclusive,
                        maxVersion: packageDependency.LibraryRange.VersionRange.MaxVersion,
                        includeMaxVersion: packageDependency.LibraryRange.VersionRange.IsMaxInclusive);
                }
                else
                {
                    versionRange = new VersionRange(
                        minVersion: package.Version,
                        includeMinVersion: packageDependency.LibraryRange.VersionRange.IsMinInclusive);
                }

                var libraryRange = new LibraryRange(packageDependency.LibraryRange) { VersionRange = versionRange };
                packageDependency = new LibraryDependency(packageDependency) { LibraryRange = libraryRange };
            }

            if (packageDependency.NoWarn.Length > 0)
            {
                HashSet<(NuGetLogCode, NuGetFramework)> nowarnProperties = null;

                if (!packageSpecificNoWarnProperties.TryGetValue(packageDependency.Name, out nowarnProperties))
                {
                    nowarnProperties = new HashSet<(NuGetLogCode, NuGetFramework)>();
                }

                nowarnProperties.AddRange(packageDependency.NoWarn.Select(n => (n, framework.FrameworkName)));
                packageSpecificNoWarnProperties[packageDependency.Name] = nowarnProperties;
            }

            PackCommandRunner.AddLibraryDependency(packageDependency, dependencies);

            return packageDependency;
        }

        private static IDictionary<string, string> ParsePropertiesAsDictionary(string[] properties)
        {
            var dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var item in properties)
            {
                var index = item.IndexOf("=", StringComparison.Ordinal);
                // Make sure '=' is not the first or the last character of the string
                if (index > 0 && index < item.Length - 1)
                {
                    var key = item.Substring(0, index);
                    var value = item.Substring(index + 1);
                    dictionary[key] = value;
                }
                // if value is empty string, set it to string.Empty instead of erroring out
                else if (index == item.Length - 1)
                {
                    var key = item.Substring(0, index);
                    var value = string.Empty;
                    dictionary[key] = value;
                }
                else
                {
                    throw new PackagingException(NuGetLogCode.NU5029, Strings.InvalidNuspecProperties);
                }
            }

            return dictionary;
        }

        private HashSet<string> InitOutputExtensions(IEnumerable<string> outputExtensions)
        {
            return new HashSet<string>(outputExtensions.Distinct(StringComparer.OrdinalIgnoreCase));
        }
    }
}

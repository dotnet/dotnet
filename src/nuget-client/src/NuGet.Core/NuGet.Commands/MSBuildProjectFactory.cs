// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectModel;
using NuGet.Versioning;

namespace NuGet.Commands
{
    public class MSBuildProjectFactory : IProjectFactory
    {
        private ILogger _logger;

        // Packaging folders
        private static readonly string SourcesFolder = PackagingConstants.Folders.Source;

        private MSBuildPackTargetArgs PackTargetArgs { get; set; }
        private PackArgs PackArgs { get; set; }

        public void SetIncludeSymbols(bool includeSymbols)
        {
            IncludeSymbols = includeSymbols;
        }
        public bool IncludeSymbols { get; set; }

        public bool Build { get; set; }

        public Dictionary<string, string> GetProjectProperties()
        {
            return ProjectProperties;
        }
        public Dictionary<string, string> ProjectProperties { get; private set; }

        public bool IsTool { get; set; }
        public ICollection<ManifestFile> Files { get; set; }

        public ILogger Logger
        {
            get
            {
                return _logger ?? NullLogger.Instance;
            }
            set
            {
                _logger = value;
            }
        }

        public IMachineWideSettings MachineWideSettings { get; set; }

        public static IProjectFactory ProjectCreator(PackArgs packArgs, string path)
        {
            return new MSBuildProjectFactory()
            {
                PackArgs = packArgs,
                IsTool = packArgs.Tool,
                Logger = packArgs.Logger,
                MachineWideSettings = packArgs.MachineWideSettings,
                Build = false,
                PackTargetArgs = packArgs.PackTargetArgs,
                Files = new HashSet<ManifestFile>(),
                ProjectProperties = new Dictionary<string, string>()
            };
        }

        public PackageBuilder CreateBuilder(string basePath, NuGetVersion version, string suffix, bool buildIfNeeded, PackageBuilder builder)
        {
            // Add output files
            Files.Clear();
            builder.Files.Clear();

            AddOutputFiles();

            // Add content files if there are any. They could come from a project or nuspec file
            AddContentFiles(builder);

            // Add sources if this is a symbol package
            if (IncludeSymbols)
            {
                AddSourceFiles();
            }

            var manifest = new Manifest(new ManifestMetadata(builder), Files);
            var manifestPath = PackCommandRunner.GetOutputPath(
                builder,
                PackArgs,
                IncludeSymbols,
                builder.Version,
                PackTargetArgs.NuspecOutputPath,
                isNupkg: false);

            var manifestDirectory = Path.GetDirectoryName(manifestPath);
            if (!Directory.Exists(manifestDirectory))
            {
                Directory.CreateDirectory(manifestDirectory);
            }

            using (Stream stream = new FileStream(manifestPath, FileMode.Create))
            {
                manifest.Save(stream);
            }

            builder.PopulateFiles(string.Empty, Files);

            return builder;
        }

        private void AddOutputFiles()
        {
            if (PackTargetArgs.IncludeBuildOutput)
            {
                AddOutputLibFiles(PackTargetArgs.TargetPathsToSymbols, IncludeSymbols ? PackTargetArgs.AllowedOutputExtensionsInSymbolsPackageBuildOutputFolder : PackTargetArgs.AllowedOutputExtensionsInPackageBuildOutputFolder);

                AddOutputLibFiles(PackTargetArgs.TargetPathsToAssemblies, IncludeSymbols ? PackTargetArgs.AllowedOutputExtensionsInSymbolsPackageBuildOutputFolder : PackTargetArgs.AllowedOutputExtensionsInPackageBuildOutputFolder);
            }
        }

        private void AddOutputLibFiles(IEnumerable<OutputLibFile> libFiles, HashSet<string> allowedExtensions)
        {
            var targetFolders = PackTargetArgs.BuildOutputFolder;
            foreach (var file in libFiles)
            {
                var extension = Path.GetExtension(file.FinalOutputPath);

                // Only look at files we care about
                if (!allowedExtensions.Contains(extension))
                {
                    continue;
                }
                var tfm = NuGetFramework.Parse(file.TargetFramework).GetShortFolderName();
                var targetPath = file.TargetPath;
                for (var i = 0; i < targetFolders.Length; i++)
                {
                    var packageFile = new ManifestFile()
                    {
                        Source = file.FinalOutputPath,
                        Target = IsTool ? Path.Combine(targetFolders[i], targetPath) : Path.Combine(targetFolders[i], tfm, targetPath)
                    };

                    AddFileToBuilder(packageFile);
                }
            }
        }

        private bool AddFileToBuilder(ManifestFile packageFile)
        {
            if (!Files.Any(p => packageFile.Target.Equals(p.Target, StringComparison.CurrentCultureIgnoreCase)))
            {
                var fileExtension = Path.GetExtension(packageFile.Source);

                if (IncludeSymbols &&
                    PackArgs.SymbolPackageFormat == SymbolPackageFormat.Snupkg &&
                    !string.Equals(fileExtension, ".pdb", StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
                else
                {
                    Files.Add(packageFile);
                    return true;
                }
            }
            else
            {
                Logger.Log(PackagingLogMessage.CreateWarning(string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.FileNotAddedToPackage,
                        packageFile.Source,
                        packageFile.Target), NuGetLogCode.NU5118));
                return false;
            }
        }

        private void AddContentFiles(PackageBuilder builder)
        {
            foreach (var sourcePath in PackTargetArgs.ContentFiles.Keys)
            {
                var listOfContentMetadata = PackTargetArgs.ContentFiles[sourcePath];
                foreach (var contentMetadata in listOfContentMetadata)
                {
                    var target = contentMetadata.Target;
                    var packageFile = new ManifestFile()
                    {
                        Source = sourcePath,
                        Target = target.EndsWith(Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture)) || string.IsNullOrEmpty(target)
                        ? Path.Combine(target, Path.GetFileName(sourcePath))
                        : target
                    };
                    var added = AddFileToBuilder(packageFile);

                    // Add contentFiles entry to the nuspec if applicable
                    if (added && IsContentFile(contentMetadata.Target))
                    {
                        var includePath = PathUtility.GetRelativePath("contentFiles" + Path.DirectorySeparatorChar, packageFile.Target, '/');
                        // This is just a check to see if the filename has already been appended to the target path. 
                        // We do this by comparing extensions of the file
                        if (!Path.GetExtension(includePath)
                                .Equals(Path.GetExtension(sourcePath), StringComparison.OrdinalIgnoreCase))
                        {
                            includePath = Path.Combine(includePath, Path.GetFileName(sourcePath));
                        }

                        var manifestContentFile = new ManifestContentFiles()
                        {
                            BuildAction = contentMetadata.BuildAction,
                            Include = includePath,
                            CopyToOutput = contentMetadata.CopyToOutput,
                            Flatten = contentMetadata.Flatten
                        };

                        builder.ContentFiles.Add(manifestContentFile);
                    }
                }
            }
        }

        private void AddSourceFiles()
        {
            foreach (var sourcePath in PackTargetArgs.SourceFiles.Keys)
            {
                var projectDirectory = PackTargetArgs.SourceFiles[sourcePath];
                var finalTargetPath = GetTargetPathForSourceFile(sourcePath, projectDirectory);

                var packageFile = new ManifestFile()
                {
                    Source = sourcePath,
                    Target = finalTargetPath
                };
                AddFileToBuilder(packageFile);
            }
        }

        public static string GetTargetPathForSourceFile(string sourcePath, string projectDirectory)
        {
            if (string.IsNullOrEmpty(sourcePath))
            {
                throw new PackagingException(NuGetLogCode.NU5020, string.Format(CultureInfo.CurrentCulture, Strings.Error_EmptySourceFilePath));
            }

            if (string.IsNullOrEmpty(projectDirectory))
            {
                throw new PackagingException(NuGetLogCode.NU5021, string.Format(CultureInfo.CurrentCulture, Strings.Error_EmptySourceFileProjectDirectory, sourcePath));
            }

            if (PathUtility.HasTrailingDirectorySeparator(projectDirectory))
            {
                projectDirectory = projectDirectory.Substring(0, projectDirectory.Length - 1);
            }
            var projectName = Path.GetFileName(projectDirectory);
            var targetPath = Path.Combine(SourcesFolder, projectName);
#if NETCOREAPP
            if (sourcePath.Contains(projectDirectory, StringComparison.Ordinal))
#else
            if (sourcePath.Contains(projectDirectory))
#endif
            {
                // This is needed because Path.GetDirectoryName returns a path with Path.DirectorySepartorChar
                var projectDirectoryWithSeparatorChar = PathUtility.GetPathWithDirectorySeparator(projectDirectory);

#if NETCOREAPP
                var relativePath = Path.GetDirectoryName(sourcePath).Replace(projectDirectoryWithSeparatorChar, string.Empty, StringComparison.Ordinal);
#else
                var relativePath = Path.GetDirectoryName(sourcePath).Replace(projectDirectoryWithSeparatorChar, string.Empty);
#endif
                if (!string.IsNullOrEmpty(relativePath) && PathUtility.IsDirectorySeparatorChar(relativePath[0]))
                {
                    relativePath = relativePath.Substring(1, relativePath.Length - 1);
                }
                if (PathUtility.HasTrailingDirectorySeparator(relativePath))
                {
                    relativePath = relativePath.Substring(0, relativePath.Length - 1);
                }
                targetPath = Path.Combine(targetPath, relativePath);
            }

            var finalTargetPath = Path.Combine(targetPath, Path.GetFileName(sourcePath));
            return finalTargetPath;
        }

        private static bool IsContentFile(string contentFileTargetPath)
        {
            return contentFileTargetPath != null && contentFileTargetPath.StartsWith("contentFiles", StringComparison.Ordinal);
        }

        public WarningProperties GetWarningPropertiesForProject()
        {
            return PackArgs.WarningProperties;
        }
    }
}

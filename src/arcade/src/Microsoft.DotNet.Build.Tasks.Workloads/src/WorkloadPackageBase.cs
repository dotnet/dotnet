// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.Deployment.DotNet.Releases;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Versioning;

namespace Microsoft.DotNet.Build.Tasks.Workloads
{
    /// <summary>
    /// Serves as a base class for implementing different types of workload packages. The class captures some common
    /// elements related to the underlying NuGet package.
    /// </summary>
    public abstract class WorkloadPackageBase
    {
        /// <summary>
        /// The package authors.
        /// </summary>
        public string Authors
        {
            get;
        }

        /// <summary>
        /// The NuGet package copyright.
        /// </summary>
        public string Copyright
        {
            get;
        }

        /// <summary>
        /// The NuGet package description.
        /// </summary>
        public string Description
        {
            get;
        }

        /// <summary>
        /// Determines on the contents of the package is managed.
        /// </summary>
        public abstract PackageExtractionMethod ExtractionMethod
        {
            get;
        }

        /// <summary>
        /// The NuGet package identifier.
        /// </summary>
        public string Id => Identity.Id;

        /// <summary>
        /// The identity of the NuGet package.
        /// </summary>
        public PackageIdentity Identity
        {
            get;
        }

        /// <summary>
        /// Gets whether the package has been extracted.
        /// </summary>
        public bool HasBeenExtracted
        {
            get;
            private set;
        }

        public LicenseMetadata LicenseData
        {
            get;
        }

        public string LicenseUrl
        {
            get;
        }

        /// <summary>
        /// Gets the version to use for the generated MSI's ProductVersion property.
        /// </summary>
        public abstract Version MsiVersion
        {
            get;
        }

        public string PackagePath
        {
            get;
        }

        public string PackageFileName
        {
            get;
        }

        public string ShortName
        {
            get;
        }

        /// <summary>
        /// A string containing the major, minor and patch version of the package.
        /// </summary>
        public string ProductVersion => $"{PackageVersion.Major}.{PackageVersion.Minor}.{PackageVersion.Patch}";

        public string Title
        {
            get;
        }

        public string ProjectUrl
        {
            get;
        }

        /// <summary>
        /// The version of the NuGet package.
        /// </summary>
        public NuGetVersion PackageVersion => Identity.Version;

        public ITaskItem[]? ShortNames
        {
            get;
        }

        /// <summary>
        /// The SWIX identifier for the package in VS.
        /// </summary>
        public string SwixPackageId
        {
            get;
            protected set;
        }

        /// <summary>
        /// The SWIX identifier for a package group that references this SWIX package.
        /// </summary>
        public string SwixPackageGroupId
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets an instance of a <see cref="TaskLoggingHelper"/> class containing task logging methods.
        /// </summary>
        protected TaskLoggingHelper? Log
        {
            get;
        }

        /// <summary>
        /// A <see cref="System.Version"/> containing the major, minor, and patch version of the underlying NuGet package.
        /// </summary>
        public Version Version => Identity.Version.Version;

        /// <summary>
        /// The destination directory where the package will be extracted.
        /// </summary>
        public string DestinationDirectory
        {
            get;
        }

        /// <summary>
        /// Creates a new instance of a <see cref="WorkloadPackageBase"/> class.
        /// </summary>
        /// <param name="packagePath">The path of the NuGet package.</param>
        /// <param name="destinationBaseDirectory">The root directory where packages will be extracted.</param>
        /// <param name="shortNames">A set of items used to shorten the names and identifiers of setup packages.</param>
        /// <param name="log">A <see cref="TaskLoggingHelper"/> class containing task logging methods.</param>
        public WorkloadPackageBase(string packagePath, string destinationBaseDirectory, ITaskItem[]? shortNames = null, TaskLoggingHelper? log = null)
        {
            // Very important: If the underlying stream isn't closed, it will cause
            // sharing violations when the package content is being extracted later.
            using FileStream fs = new(packagePath, FileMode.Open);
            using PackageArchiveReader reader = new(fs);
            NuspecReader nuspec = reader.NuspecReader;

            Authors = nuspec.GetAuthors();
            Copyright = nuspec.GetCopyright();
            Description = nuspec.GetDescription();
            Identity = nuspec.GetIdentity();
            LicenseData = nuspec.GetLicenseMetadata();
            LicenseUrl = nuspec.GetLicenseUrl();
            ProjectUrl = nuspec.GetProjectUrl();
            Title = nuspec.GetTitle();

            PackagePath = packagePath;
            DestinationDirectory = Path.Combine(destinationBaseDirectory, $"{Identity}");
            ShortNames = shortNames;

            PackageFileName = Path.GetFileNameWithoutExtension(packagePath);
            ShortName = PackageFileName.Replace(shortNames);
            SwixPackageId = $"{Id.Replace(shortNames)}.{Identity.Version}";
            SwixPackageGroupId = $"{DefaultValues.PackageGroupPrefix}.{SwixPackageId}";
            Log = log;
        }

        /// <summary>
        /// Extracts the contents of the package.
        /// </summary>
        public void Extract()
        {
            Extract(Enumerable.Empty<string>());
        }

        /// <summary>
        /// Extract the contents of the package and optionally delete files that match
        /// the set of exclusions.
        /// </summary>
        /// <param name="exclusionPatterns">A set of regular expression patterns used to determine if a
        /// file should be excluded. Excluded files will be deleted after the package has been extracted.</param>
        public virtual void Extract(IEnumerable<string> exclusionPatterns)
        {
            if (HasBeenExtracted)
            {
                return;
            }

            Utils.DeleteDirectory(DestinationDirectory);
            Directory.CreateDirectory(DestinationDirectory);

            if (ExtractionMethod == PackageExtractionMethod.Copy)
            {
                File.Copy(PackagePath, Path.Combine(DestinationDirectory, Path.GetFileName(PackagePath)), overwrite: true);
                HasBeenExtracted = true;
            }
            else if (ExtractionMethod == PackageExtractionMethod.Unzip)
            {
                ZipFile.ExtractToDirectory(PackagePath, DestinationDirectory);

                // Remove unnecessary files and directories that we never want to ship. These are always present in a NuGet package.
                Utils.DeleteDirectory(Path.Combine(DestinationDirectory, "_rels"));
                Utils.DeleteDirectory(Path.Combine(DestinationDirectory, "package"));

                Utils.DeleteFile(Path.Combine(DestinationDirectory, ".signature.p7s"));
                Utils.DeleteFile(Path.Combine(DestinationDirectory, "[Content_Types].xml"));
                Utils.DeleteFile(Path.Combine(DestinationDirectory, $"{Id}.nuspec"));

                if (exclusionPatterns.Any())
                {
                    foreach (string file in Directory.EnumerateFiles(DestinationDirectory, "*.*", SearchOption.AllDirectories))
                    {
                        if (exclusionPatterns.Any(pattern => Regex.IsMatch(file, pattern)))
                        {
                            Log?.LogMessage(MessageImportance.Low, string.Format(Strings.WorkloadPackageDeleteExclusion, file));
                            File.Delete(file);
                        }
                    }
                }

                HasBeenExtracted = true;
            }
        }

        /// <summary>
        /// Converts a string containing an SDK version to a semantic version that normalizes the patch level and 
        /// optionally includes the first two prerelease labels. For example, if the specified version is 6.0.105, then
        /// 6.0.100 would be returned. If the version is 6.0.301-preview.2.1234, the result would be 6.0.300-preview.1.
        /// </summary>
        /// <param name="sdkVersion">A string containing an SDK version.</param>
        /// <returns>An SDK feature band version.</returns>
        internal static ReleaseVersion GetSdkFeatureBandVersion(string sdkVersion)
        {
            ReleaseVersion version = new(sdkVersion);

            // Ignore CI and dev builds.
            if (string.IsNullOrEmpty(version.Prerelease) || version.Prerelease.Split('.').Any(s => string.Equals("ci", s) || string.Equals("dev", s)))
            {
                return new ReleaseVersion(version.Major, version.Minor, version.SdkFeatureBand);
            }

            string[] preleaseParts = version.Prerelease.Split('.');

            // Only the first two prerelease identifiers are used to support side-by-side previews.
            string prerelease = (preleaseParts.Length > 1) ?
                $"{preleaseParts[0]}.{preleaseParts[1]}" :
                preleaseParts[0];

            return new ReleaseVersion(version.Major, version.Minor, version.SdkFeatureBand, prerelease);
        }

        /// <summary>
        /// Extracts the SDK version from the package ID.
        /// </summary>
        /// <param name="packageId">The package ID from which to extract the SDK version.</param>
        /// <param name="separator">A string used to determine where the SDK version should start.</param>
        /// <returns>SDK version part of the package ID.</returns>
        /// <exception cref="FormatException" />
        internal static string GetSdkVersion(string packageId, string separator) =>
            !string.IsNullOrWhiteSpace(packageId) && packageId.IndexOf(separator) > -1 ?
                packageId.Substring(packageId.IndexOf(separator) + separator.Length) :
                throw new FormatException(string.Format(Strings.CannotExtractSdkVersionFromPackageId, packageId));

        /// <summary>
        /// Gets the MSI ProductVersion to use for the given packagage. The task item metadata is used first, before falling
        /// back to using the version parameter on the task. If neither exist an exception is thrown.
        /// </summary>
        /// <param name="package">The package item to convert into an MSI.</param>
        /// <param name="msiVersion">The default MSI version 1</param>
        /// <param name="taskName"></param>
        /// <param name="taskParameterName"></param>
        /// <param name="taskItemName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        internal static Version GetMsiVersion(ITaskItem package, Version msiVersion, string taskName,
            string taskParameterName, string taskItemName)
        {
            if (!string.IsNullOrWhiteSpace(package.GetMetadata(Metadata.MsiVersion)))
            {
                // We prefer version metadata information on the package item.
                return new(package.GetMetadata(Metadata.MsiVersion));
            }
            else if (msiVersion != null)
            {
                // Fall back to the version provided by the task parameter.
                return msiVersion;
            }

            // While we could use the major.minor.patch part of the package, it won't work for upgradable MSIs (manifests) and
            // unlike packs, we want users to be explicit about the MSI versionsand
            // the user to be aware of this and explicitly tell us the value.
            throw new Exception(string.Format(Strings.NoInstallerVersion, taskName,
                taskParameterName, taskItemName, Metadata.MsiVersion));
        }
    }
}

#nullable disable

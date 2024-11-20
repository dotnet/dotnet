// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol
{
    public class LocalPackageSearchMetadata : IPackageSearchMetadata
    {
        private readonly NuspecReader _nuspec;
        private readonly LocalPackageInfo _package;

        public LocalPackageSearchMetadata(LocalPackageInfo package)
        {
            _package = package ?? throw new ArgumentNullException(nameof(package));
            _nuspec = package.Nuspec;
        }

        public string Authors => _nuspec.GetAuthors();

        public IEnumerable<PackageDependencyGroup> DependencySets => _nuspec.GetDependencyGroups().ToArray();

        public string Description => _nuspec.GetDescription();

        /// <remarks>
        /// Local package sources never provide a download count.
        /// </remarks>
        public long? DownloadCount => null;

        public Uri IconUrl => GetIconUri();

        public string ReadmeFileUrl => GetReadmeUri();

        public PackageIdentity Identity => _nuspec.GetIdentity();

        public Uri LicenseUrl => Convert(_nuspec.GetLicenseUrl());

        private IReadOnlyList<string> _ownersList;

        public IReadOnlyList<string> OwnersList
        {
            get
            {
                if (_ownersList is null)
                {
                    _ownersList = Owners != null ? Owners.Split(',').Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()).ToList() : null;
                }

                return _ownersList;
            }
        }

        public string Owners => _nuspec.GetOwners();

        public Uri ProjectUrl => Convert(_nuspec.GetProjectUrl());

        public DateTimeOffset? Published => _package.LastWriteTimeUtc;

        /// <remarks>
        /// There is no readme url for local packages. Later releases may display the readme file in a VS window.
        /// </remarks>
        public Uri ReadmeUrl => null;

        /// <remarks>
        /// There is no report abuse url for local packages.
        /// </remarks>
        public Uri ReportAbuseUrl => null;

        public Uri PackageDetailsUrl => null;

        public bool RequireLicenseAcceptance => _nuspec.GetRequireLicenseAcceptance();

        public string Summary => !string.IsNullOrEmpty(_nuspec.GetSummary()) ? _nuspec.GetSummary() : Description;

        public string Tags
        {
            get
            {
                var tags = _nuspec.GetTags()?.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
                return string.Join(" ", tags);
            }
        }

        public string Title => !string.IsNullOrEmpty(_nuspec.GetTitle()) ? _nuspec.GetTitle() : _nuspec.GetId();

        /// <summary>
        /// Gets a function that provides <c>PackageReaderBase</c>-like objects, for reading package content
        /// </summary>
        /// <remarks>
        /// This property depends from the reader contained in the <c>LocalPackageInfo</c> specified at object creation time
        /// </remarks>
        public Func<PackageReaderBase> PackageReader
        {
            get
            {
                return new Func<PackageReaderBase>(() => _package.GetReader());
            }
        }

        /// <inheritdoc cref="IPackageSearchMetadata.GetVersionsAsync" />
        public Task<IEnumerable<VersionInfo>> GetVersionsAsync() => TaskResult.EmptyEnumerable<VersionInfo>();

        /// <summary>
        /// Convert a string to a URI safely. This will return null if there are errors.
        /// </summary>
        private static Uri Convert(string uri)
        {
            Uri fullUri = null;

            if (!string.IsNullOrEmpty(uri))
            {
                Uri.TryCreate(uri, UriKind.Absolute, out fullUri);
            }

            return fullUri;
        }

        public bool IsListed => true;

        /// <remarks>
        /// The prefix reservation is not applicable to local packages
        /// </remarks>
        public bool PrefixReserved => false;

        public LicenseMetadata LicenseMetadata => _nuspec.GetLicenseMetadata();

        /// <inheritdoc cref="IPackageSearchMetadata.GetDeprecationMetadataAsync" />
        public Task<PackageDeprecationMetadata> GetDeprecationMetadataAsync() => TaskResult.Null<PackageDeprecationMetadata>();

        /// <inheritdoc cref="IPackageSearchMetadata.Vulnerabilities" />
        public IEnumerable<PackageVulnerabilityMetadata> Vulnerabilities => null;

        public string PackagePath => _package.Path;

        private const int FiveMegabytes = 5242880; // 1024 * 1024 * 5, 5MB

        public string LoadFileAsText(string path)
        {
            string fileContent = null;
            try
            {
                if (_package.GetReader() is PackageArchiveReader reader) // This will never be anything else in reality. The search resource always uses a PAR
                {
                    var entry = reader.GetEntry(PathUtility.StripLeadingDirectorySeparators(path));
                    if (entry != null)
                    {
                        if (entry.Length >= FiveMegabytes)
                        {
                            fileContent = string.Format(CultureInfo.CurrentCulture, Strings.LoadFileFromNupkg_FileTooLarge, path, "5");
                        }
                        else
                        {
                            using (var licenseStream = entry.Open())
                            using (TextReader textReader = new StreamReader(licenseStream))
                            {
                                fileContent = textReader.ReadToEnd();
                            }
                        }
                    }
                    else
                    {
                        fileContent = string.Format(CultureInfo.CurrentCulture, Strings.LoadFileFromNupkg_FileNotFound, path);
                    }
                }
            }
            catch
            {
                fileContent = string.Format(CultureInfo.CurrentCulture, Strings.LoadFileFromNupkg_UnknownProblemLoadingTheFile, path);
            }
            finally
            {
                if (fileContent == null)
                {
                    fileContent = string.Format(CultureInfo.CurrentCulture, Strings.LoadFileFromNupkg_UnknownProblemLoadingTheFile, path);
                }
            }
            return fileContent;
        }

        /// <summary>
        /// Points to an Icon, either Embedded Icon or IconUrl
        /// </summary>
        private Uri GetIconUri()
        {
            string embeddedIcon = _nuspec.GetIcon();

            if (embeddedIcon == null)
            {
                return Convert(_nuspec.GetIconUrl());
            }

            var baseUri = Convert(_package.Path);

            UriBuilder builder = new UriBuilder(baseUri)
            {
                Fragment = embeddedIcon
            };

            // get the special icon url
            return builder.Uri;
        }

        private string GetReadmeUri()
        {
            string embeddedReadme = _nuspec.GetReadme();
            if (embeddedReadme == null)
            {
                return null;
            }

            var baseUri = Convert(_package.Path);
            UriBuilder builder = new UriBuilder(baseUri)
            {
                Fragment = embeddedReadme
            };

            // get the readme url
            return builder.Uri.AbsoluteUri;
        }
    }
}

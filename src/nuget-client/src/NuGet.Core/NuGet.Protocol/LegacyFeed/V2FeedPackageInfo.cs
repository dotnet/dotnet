// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Versioning;

namespace NuGet.Protocol
{
    /// <summary>
    /// Represents a V2 package entry from the OData feed. This object primarily just holds the strings parsed from XML, all parsing 
    /// and converting should be done after based on the scenario.
    /// </summary>
    public class V2FeedPackageInfo : PackageIdentity
    {
        private readonly string _title;
        private readonly string _summary;
        private readonly string[] _authors;
        private readonly string _description;
        private readonly string[] _owners;
        private readonly string _iconUrl;
        private readonly string _licenseUrl;
        private readonly string _projectUrl;
        private readonly string _reportAbuseUrl;
        private readonly string _galleryDetailsUrl;
        private readonly string _tags;
        private readonly string _downloadCount;
        private readonly bool _requireLicenseAcceptance;
        private readonly DateTimeOffset? _created;
        private readonly DateTimeOffset? _lastEdited;
        private readonly DateTimeOffset? _published;
        private readonly string _dependencies;
        private readonly string _downloadUrl;
        private readonly string _packageHash;
        private readonly string _packageHashAlgorithm;
        private readonly NuGetVersion _minClientVersion;
        private const string NullString = "null";

        public V2FeedPackageInfo(PackageIdentity identity, string title, string summary, string description, IEnumerable<string> authors, IEnumerable<string> owners,
            string iconUrl, string licenseUrl, string projectUrl, string reportAbuseUrl, string galleryDetailsUrl,
            string tags, DateTimeOffset? created, DateTimeOffset? lastEdited, DateTimeOffset? published, string dependencies, bool requireLicenseAccept, string downloadUrl, string downloadCount,
            string packageHash, string packageHashAlgorithm, NuGetVersion minClientVersion)
            : base(identity.Id, identity.Version)
        {
            _summary = summary;
            _description = description;
            _authors = authors == null ? Array.Empty<string>() : authors.ToArray();
            _owners = owners == null ? Array.Empty<string>() : owners.ToArray();
            _iconUrl = iconUrl;
            _licenseUrl = licenseUrl;
            _projectUrl = projectUrl;
            _reportAbuseUrl = reportAbuseUrl;
            _galleryDetailsUrl = galleryDetailsUrl;
            _description = description;
            _summary = summary;
            _tags = tags;
            _dependencies = dependencies;
            _requireLicenseAcceptance = requireLicenseAccept;
            _title = title;
            _downloadUrl = downloadUrl;
            _downloadCount = downloadCount;
            _created = created;
            _lastEdited = lastEdited;
            _published = published;
            _packageHash = packageHash;
            _packageHashAlgorithm = packageHashAlgorithm;
            _minClientVersion = minClientVersion;
        }

        public string Title
        {
            get
            {
                return _title;
            }
        }

        public string Summary
        {
            get
            {
                return _summary;
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
        }

        public IEnumerable<string> Authors
        {
            get
            {
                return _authors;
            }
        }

        public IEnumerable<string> Owners
        {
            get
            {
                return _owners;
            }
        }

        public string IconUrl
        {
            get
            {
                return _iconUrl;
            }
        }

        public string LicenseUrl
        {
            get
            {
                return _licenseUrl;
            }
        }

        public string ProjectUrl
        {
            get
            {
                return _projectUrl;
            }
        }

        public string DownloadUrl
        {
            get
            {
                return _downloadUrl;
            }
        }

        public string ReportAbuseUrl
        {
            get
            {
                return _reportAbuseUrl;
            }
        }

        public string GalleryDetailsUrl
        {
            get
            {
                return _galleryDetailsUrl;
            }
        }

        public string Tags
        {
            get
            {
                return _tags;
            }
        }

        public string DownloadCount
        {
            get
            {
                return _downloadCount;
            }
        }

        /// <summary>
        /// Parse DownloadCount into an integer
        /// </summary>
        public int DownloadCountAsInt
        {
            get
            {
                int x = 0;
                _ = int.TryParse(_downloadCount, out x);
                return x;
            }
        }

        public DateTimeOffset? Created
        {
            get
            {
                return _created;
            }
        }

        public DateTimeOffset? LastEdited
        {
            get
            {
                return _lastEdited;
            }
        }

        public DateTimeOffset? Published
        {
            get
            {
                return _published;
            }
        }

        /// <summary>
        /// Checks the published date
        /// </summary>
        public bool IsListed
        {
            get
            {
                return !Published.HasValue || Published.Value.Year > 1901;
            }
        }

        public string Dependencies
        {
            get
            {
                return _dependencies;
            }
        }

        /// <summary>
        /// Parses Dependencies into actual groups
        /// </summary>
        public IReadOnlyList<PackageDependencyGroup> DependencySets
        {
            get
            {
                // Ex: Microsoft.Data.OData:5.6.3:aspnetcore50|Microsoft.Data.Services.Client:5.6.3:aspnetcore50|System.Spatial:5.6.3:aspnetcore50|System.Collections:4.0.10-beta-22231:aspnetcore50|System.Collections.Concurrent:4.0.0-beta-22231:aspnetcore50|System.Collections.Specialized:4.0.0-beta-22231:aspnetcore50|System.Diagnostics.Debug:4.0.10-beta-22231:aspnetcore50|System.Diagnostics.Tools:4.0.0-beta-22231:aspnetcore50|System.Diagnostics.TraceSource:4.0.0-beta-22231:aspnetcore50|System.Diagnostics.Tracing:4.0.10-beta-22231:aspnetcore50|System.Dynamic.Runtime:4.0.0-beta-22231:aspnetcore50|System.Globalization:4.0.10-beta-22231:aspnetcore50|System.IO:4.0.10-beta-22231:aspnetcore50|System.IO.FileSystem:4.0.0-beta-22231:aspnetcore50|System.IO.FileSystem.Primitives:4.0.0-beta-22231:aspnetcore50|System.Linq:4.0.0-beta-22231:aspnetcore50|System.Linq.Expressions:4.0.0-beta-22231:aspnetcore50|System.Linq.Queryable:4.0.0-beta-22231:aspnetcore50|System.Net.Http:4.0.0-beta-22231:aspnetcore50|System.Net.Primitives:4.0.10-beta-22231:aspnetcore50|System.Reflection:4.0.10-beta-22231:aspnetcore50|System.Reflection.Extensions:4.0.0-beta-22231:aspnetcore50|System.Reflection.TypeExtensions:4.0.0-beta-22231:aspnetcore50|System.Runtime:4.0.20-beta-22231:aspnetcore50|System.Runtime.Extensions:4.0.10-beta-22231:aspnetcore50|System.Runtime.InteropServices:4.0.20-beta-22231:aspnetcore50|System.Runtime.Serialization.Primitives:4.0.0-beta-22231:aspnetcore50|System.Runtime.Serialization.Xml:4.0.10-beta-22231:aspnetcore50|System.Security.Cryptography.Encoding:4.0.0-beta-22231:aspnetcore50|System.Security.Cryptography.Encryption:4.0.0-beta-22231:aspnetcore50|System.Security.Cryptography.Hashing:4.0.0-beta-22231:aspnetcore50|System.Security.Cryptography.Hashing.Algorithms:4.0.0-beta-22231:aspnetcore50|System.Text.Encoding:4.0.10-beta-22231:aspnetcore50|System.Text.Encoding.Extensions:4.0.10-beta-22231:aspnetcore50|System.Text.RegularExpressions:4.0.10-beta-22231:aspnetcore50|System.Threading:4.0.0-beta-22231:aspnetcore50|System.Threading.Tasks:4.0.10-beta-22231:aspnetcore50|System.Threading.Thread:4.0.0-beta-22231:aspnetcore50|System.Threading.ThreadPool:4.0.10-beta-22231:aspnetcore50|System.Threading.Timer:4.0.0-beta-22231:aspnetcore50|System.Xml.ReaderWriter:4.0.10-beta-22231:aspnetcore50|System.Xml.XDocument:4.0.0-beta-22231:aspnetcore50|System.Xml.XmlSerializer:4.0.0-beta-22231:aspnetcore50|Microsoft.Data.OData:5.6.3:aspnet50|Microsoft.Data.Services.Client:5.6.3:aspnet50|System.Spatial:5.6.3:aspnet50|Microsoft.Data.OData:5.6.2:net40-Client|Newtonsoft.Json:5.0.8:net40-Client|Microsoft.Data.Services.Client:5.6.2:net40-Client|Microsoft.WindowsAzure.ConfigurationManager:1.8.0.0:net40-Client|Microsoft.Data.OData:5.6.2:win80|Microsoft.Data.OData:5.6.2:wpa|Microsoft.Data.OData:5.6.2:wp80|Newtonsoft.Json:5.0.8:wp80

                if (string.IsNullOrEmpty(Dependencies))
                {
                    return new List<PackageDependencyGroup>();
                }
                else
                {
                    var results = new Dictionary<NuGetFramework, List<PackageDependency>>(NuGetFrameworkFullComparer.Instance);

                    foreach (var set in Dependencies.Split('|'))
                    {
                        var parts = set.Trim().Split(new[] { ':' }, StringSplitOptions.None);

                        if (parts.Length != 0)
                        {
                            // Defaults
                            var dependencyId = parts[0];
                            var versionRange = VersionRange.All;
                            var framework = NuGetFramework.AnyFramework;

                            if (parts.Length > 1)
                            {
                                var versionRangeString = parts[1];

                                // Nexus will write "null" when there is no depenency version range.
                                // Parse the optional version range
                                if (!string.IsNullOrWhiteSpace(versionRangeString)
                                    && !string.Equals(NullString, versionRangeString, StringComparison.OrdinalIgnoreCase))
                                {
                                    // Attempt to parse the version
                                    versionRange = VersionRange.Parse(versionRangeString);
                                }

                                // Parse the optional framework string
                                if (parts.Length > 2)
                                {
                                    var frameworkString = parts[2];

                                    if (!string.IsNullOrWhiteSpace(frameworkString)
                                        && !string.Equals(NullString, frameworkString, StringComparison.OrdinalIgnoreCase))
                                    {
                                        framework = NuGetFramework.Parse(frameworkString);
                                    }
                                }
                            }

                            // Group dependencies by target framework
                            List<PackageDependency> deps = null;
                            if (!results.TryGetValue(framework, out deps))
                            {
                                deps = new List<PackageDependency>();
                                results.Add(framework, deps);
                            }

                            // Validate - this should never be empty
                            if (!string.IsNullOrEmpty(dependencyId))
                            {
                                // Make sure there are no duplicate dependencies, this could happen with Unsupported
                                if (!deps.Any(p => string.Equals(p.Id, dependencyId, StringComparison.OrdinalIgnoreCase)))
                                {
                                    var packageDependency = new PackageDependency(dependencyId, versionRange);

                                    deps.Add(packageDependency);
                                }
                            }
                        }
                        else
                        {
                            Debug.Fail("Unknown dependency format: " + set);
                        }
                    }

                    return results.Select(pair => new PackageDependencyGroup(pair.Key, pair.Value)).ToList();
                }
            }
        }

        public bool RequireLicenseAcceptance
        {
            get
            {
                return _requireLicenseAcceptance;
            }
        }

        public string PackageHash
        {
            get
            {
                return _packageHash;
            }
        }

        public string PackageHashAlgorithm
        {
            get
            {
                return _packageHashAlgorithm;
            }
        }

        public NuGetVersion MinClientVersion
        {
            get
            {
                return _minClientVersion;
            }
        }
    }
}

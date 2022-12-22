// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.Packaging.Signing;
using NuGet.Versioning;

namespace NuGet.Test.Utility
{
    public class SimpleTestPackageContext
    {
        public SimpleTestPackageContext(string packageId)
            : this()
        {
            Id = packageId;
        }

        public SimpleTestPackageContext(string packageId, string version)
            : this(packageId)
        {
            Version = version;
        }

        public SimpleTestPackageContext(PackageIdentity identity)
            : this(identity.Id, identity.Version.ToString())
        {
        }

        public SimpleTestPackageContext()
        {
        }

        public string Id { get; set; } = "packageA";
        public string Version { get; set; } = "1.0.0";
        public string VersionOverride { get; set; }
        public string MinClientVersion { get; set; }
        public List<SimpleTestPackageContext> Dependencies { get; set; } = new List<SimpleTestPackageContext>();
        public Dictionary<NuGetFramework, List<SimpleTestPackageContext>> PerFrameworkDependencies { get; set; } = new();
        public Dictionary<NuGetFramework, string[]> FrameworkReferences { get; set; } = new Dictionary<NuGetFramework, string[]>();
        public string Include { get; set; } = string.Empty;
        public string Exclude { get; set; } = string.Empty;
        // Used by the parent project
        public string PrivateAssets { get; set; } = string.Empty;
        public string Aliases { get; set; } = string.Empty;
        public List<KeyValuePair<string, byte[]>> Files { get; set; } = new List<KeyValuePair<string, byte[]>>();
        public XDocument Nuspec { get; set; }
        public List<PackageType> PackageTypes { get; set; } = new List<PackageType>();
        public PackageType PackageType { get; set; }
        public string NoWarn { get; set; }

        public bool UseDefaultRuntimeAssemblies { get; set; } = true;

        public ITimestampProvider PrimaryTimestampProvider { get; set; }
        public ITimestampProvider CounterTimestampProvider { get; set; }
        public bool IsPrimarySigned { get; set; }
        public bool IsRepositoryCounterSigned { get; set; }
        public X509Certificate2 PrimarySignatureCertificate { get; set; }
        public X509Certificate2 RepositoryCountersignatureCertificate { get; set; }
        public Uri V3ServiceIndexUrl { get; set; }
        public IReadOnlyList<string> PackageOwners { get; set; }

        /// <summary>
        /// runtime.json
        /// </summary>
        public string RuntimeJson { get; set; }

        public bool IsSymbolPackage { get; set; }
        public bool IsSnupkgPackage { get; set; }

        public PackageIdentity Identity => new PackageIdentity(Id, NuGetVersion.Parse(Version));

        public string PackageName
        {
            get
            {
                string dotExtension = !IsSymbolPackage ? NuGetConstants.PackageExtension : (IsSnupkgPackage ? NuGetConstants.SnupkgExtension : NuGetConstants.SymbolsExtension);
                return $"{Id}.{Version}{dotExtension}";
            }
        }

        /// <summary>
        /// Add a file to the zip. Ex: lib/net45/a.dll
        /// </summary>
        public void AddFile(string path)
        {
            AddFile(path, new byte[] { 0 });
        }

        public void AddFile(string path, string content)
        {
            AddFile(path, Encoding.UTF8.GetBytes(content));
        }

        public void AddFile(string path, byte[] bytes)
        {
            if (string.IsNullOrEmpty(path) || path.StartsWith("/") || path.IndexOf('\\') > -1)
            {
                throw new ArgumentException(nameof(path));
            }

            Files.Add(new KeyValuePair<string, byte[]>(path, bytes));
        }

        /// <summary>
        /// Creates a ZipArchive and writes it to a stream.
        /// </summary>
        public async Task<MemoryStream> CreateAsStreamAsync()
        {
            var stream = new MemoryStream();
            await SimpleTestPackageUtility.CreatePackageAsync(stream, this);
            return stream;
        }

        /// <summary>
        /// Creates a file and writes to it.
        /// </summary>
        /// <param name="testDirectory">The directory for the new file.</param>
        /// <param name="fileName">The file name.</param>
        /// <returns>A <see cref="FileInfo" /> object.</returns>
        public async Task<FileInfo> CreateAsFileAsync(TestDirectory testDirectory, string fileName)
        {
            var packageFile = new FileInfo(Path.Combine(testDirectory, fileName));

            using (var readStream = await CreateAsStreamAsync())
            using (var writeStream = packageFile.OpenWrite())
            {
                readStream.CopyTo(writeStream);
            }

            return packageFile;
        }

        public new string ToString()
        {
            return $"{Id}.{Version}";
        }
    }
}

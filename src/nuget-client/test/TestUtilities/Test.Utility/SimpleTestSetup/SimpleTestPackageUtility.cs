// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Versioning;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Packaging.PackageExtraction;
using NuGet.Packaging.Signing;
using NuGet.Versioning;

namespace NuGet.Test.Utility
{
    public static class SimpleTestPackageUtility
    {
        public static async Task CreateFullPackagesAsync(string repositoryDir, IDictionary<string, IEnumerable<string>> packages)
        {
            if (packages == null)
            {
                throw new ArgumentNullException(nameof(packages));
            }
            if (repositoryDir == null)
            {
                throw new ArgumentNullException(nameof(repositoryDir));
            }

            foreach (KeyValuePair<string, IEnumerable<string>> package in packages)
            {
                foreach (string pkgVersion in package.Value)
                {
                    await CreateFullPackageAsync(repositoryDir, package.Key, pkgVersion);
                }
            }
        }

        /// <summary>
        /// Creates a net45 package containing lib, build, native, tools, and contentFiles
        /// </summary>
        public static async Task<FileInfo> CreateFullPackageAsync(
           string repositoryDir,
           string id,
           string version)
        {
            return await CreateFullPackageAsync(repositoryDir, id, version, new PackageDependency[0]);
        }

        /// <summary>
        /// Creates a net45 package containing lib, build, native, tools, and contentFiles
        /// </summary>
        public static async Task<FileInfo> CreateFullPackageAsync(
           string repositoryDir,
           string id,
           string version,
           IEnumerable<PackageDependency> dependencies)
        {
            var package = new SimpleTestPackageContext()
            {
                Id = id,
                Version = version
            };

            package.Dependencies.AddRange(dependencies.Select(d => new SimpleTestPackageContext()
            {
                Id = d.Id,
                Version = d.VersionRange.MinVersion.ToString() ?? "1.0.0",
                Include = string.Join(",", d.Include),
                Exclude = string.Join(",", d.Include),
            }));

            return await CreateFullPackageAsync(repositoryDir, package);
        }

        public static async Task<FileInfo> CreateSymbolPackageAsync(
           string repositoryDir,
           string id,
           string version,
           bool isSnupkg = false)
        {
            var package = new SimpleTestPackageContext()
            {
                Id = id,
                Version = version,
                IsSymbolPackage = true,
                IsSnupkgPackage = isSnupkg
            };

            return await CreateFullPackageAsync(repositoryDir, package);
        }

        /// <summary>
        /// Creates a net45 package containing lib, build, native, tools, and contentFiles
        /// </summary>
        public static async Task<FileInfo> CreateFullPackageAsync(
           string repositoryDir,
           SimpleTestPackageContext packageContext)
        {
            var packageName = packageContext.PackageName;

            var packagePath = Path.Combine(repositoryDir, packageName);
            var file = new FileInfo(packagePath);

            file.Directory.Create();

            using (var stream = file.Open(FileMode.CreateNew, FileAccess.ReadWrite))
            {
                await CreatePackageAsync(stream, packageContext);
            }

            return file;
        }

        /// <summary>
        /// Write a zip file to a stream.
        /// </summary>
        public static async Task CreatePackageAsync(Stream stream, SimpleTestPackageContext packageContext)
        {
            var id = packageContext.Id;
            var version = packageContext.Version;
            var runtimeJson = packageContext.RuntimeJson;
            var pathResolver = new VersionFolderPathResolver(null);
            var testLogger = new TestLogger();
            var tempStream = stream;
            var isUsingTempStream = false;

            if (packageContext.IsPrimarySigned)
            {
                tempStream = new MemoryStream();
                isUsingTempStream = true;
            }

            using (var zip = new ZipArchive(tempStream, ZipArchiveMode.Create, leaveOpen: true))
            {
                if (packageContext.Files.Any())
                {
                    foreach (var entryFile in packageContext.Files)
                    {
                        zip.AddEntry(entryFile.Key, entryFile.Value);
                    }
                }
                else
                {
                    zip.AddEntry("contentFiles/any/any/config.xml", new byte[] { 0 });
                    zip.AddEntry("contentFiles/cs/net45/code.cs", new byte[] { 0 });
                    zip.AddEntry("lib/net45/a.dll", new byte[] { 0 });
                    zip.AddEntry("lib/netstandard1.0/a.dll", new byte[] { 0 });
                    zip.AddEntry($"build/net45/{id}.targets", @"<Project />", Encoding.UTF8);
                    zip.AddEntry("runtimes/any/native/a.dll", new byte[] { 0 });
                    zip.AddEntry("tools/a.exe", new byte[] { 0 });
                }

                if (!string.IsNullOrEmpty(runtimeJson))
                {
                    zip.AddEntry("runtime.json", runtimeJson, Encoding.UTF8);
                }

                var frameworkAssembliesAndContentFiles = packageContext.UseDefaultRuntimeAssemblies ?
                          $@"<frameworkAssemblies>
                                <frameworkAssembly assemblyName=""System.Runtime""/>
                            </frameworkAssemblies>
                           <contentFiles>
                               <files include=""cs/net45/config/config.xml"" buildAction=""none"" />
                               <files include=""cs/net45/config/config.xml"" copyToOutput=""true"" flatten=""false"" />
                               <files include=""cs/net45/images/image.jpg"" buildAction=""embeddedresource"" />
                           </contentFiles>" :
                           string.Empty;

                var nuspecXml = packageContext.Nuspec?.ToString() ??
                    $@"<?xml version=""1.0"" encoding=""utf-8""?>
                        <package>
                        <metadata>
                            <id>{id}</id>
                            <version>{version.ToString()}</version>
                            <title />
                            {frameworkAssembliesAndContentFiles}
                        </metadata>
                        </package>";

                var xml = XDocument.Parse(nuspecXml);

                // Add the min client version if it exists
                if (!string.IsNullOrEmpty(packageContext.MinClientVersion))
                {
                    xml.Root.Element(XName.Get("metadata"))
                        .Add(new XAttribute(XName.Get("minClientVersion"), packageContext.MinClientVersion));
                }

                List<(string, List<PackageDependency>)> dependenciesPerFramework = GetPackageDependencies(packageContext);

                if (dependenciesPerFramework.Any())
                {
                    var metadata = xml.Element(XName.Get("package")).Element(XName.Get("metadata"));
                    var dependenciesNode = new XElement(XName.Get("dependencies"));

                    foreach (var deps in dependenciesPerFramework)
                    {
                        var groupNode = new XElement(XName.Get("group"));
                        if (!string.IsNullOrEmpty(deps.Item1))
                        {
                            groupNode.SetAttributeValue("targetFramework", deps.Item1);
                        }
                        dependenciesNode.Add(groupNode);
                        metadata.Add(dependenciesNode);

                        foreach (var dependency in deps.Item2)
                        {
                            var node = new XElement(XName.Get("dependency"));
                            groupNode.Add(node);
                            node.Add(new XAttribute(XName.Get("id"), dependency.Id));
                            node.Add(new XAttribute(XName.Get("version"), dependency.VersionRange.ToNormalizedString()));

                            if (dependency.Include.Count > 0)
                            {
                                node.Add(new XAttribute(XName.Get("include"), string.Join(",", dependency.Include)));
                            }

                            if (dependency.Exclude.Count > 0)
                            {
                                node.Add(new XAttribute(XName.Get("exclude"), string.Join(",", dependency.Exclude)));
                            }
                        }
                    }
                }

                if (packageContext.FrameworkReferences.Any())
                {
                    var metadata = xml.Element(XName.Get("package")).Element(XName.Get("metadata"));
                    var frameworkReferencesNode = new XElement(XName.Get("frameworkReferences"));

                    foreach (var kvp in packageContext.FrameworkReferences)
                    {
                        var groupNode = new XElement(XName.Get("group"));
                        groupNode.SetAttributeValue("targetFramework", kvp.Key.GetFrameworkString());
                        frameworkReferencesNode.Add(groupNode);
                        metadata.Add(frameworkReferencesNode);

                        foreach (var frameworkReference in kvp.Value)
                        {
                            var node = new XElement(XName.Get("frameworkReference"));
                            groupNode.Add(node);
                            node.Add(new XAttribute(XName.Get("name"), frameworkReference));
                        }
                    }
                }

                if (packageContext.PackageTypes.Count > 0)
                {
                    var metadata = xml.Element("package").Element("metadata");
                    var packageTypes = new XElement("packageTypes");
                    metadata.Add(packageTypes);

                    foreach (var packageType in packageContext.PackageTypes)
                    {
                        var packageTypeElement = new XElement("packageType");
                        packageTypeElement.Add(new XAttribute("name", packageType.Name));
                        if (packageType.Version != PackageType.EmptyVersion)
                        {
                            packageTypeElement.Add(new XAttribute("version", packageType.Version));
                        }

                        packageTypes.Add(packageTypeElement);
                    }
                }

                zip.AddEntry($"{id}.nuspec", xml.ToString(), Encoding.UTF8);
            }

            if (isUsingTempStream)
            {
                using (tempStream)
#if IS_SIGNING_SUPPORTED
                using (var signPackage = new SignedPackageArchive(tempStream, stream))
#endif
                {
#if IS_SIGNING_SUPPORTED
                    using (var request = GetPrimarySignRequest(packageContext))
                    {
                        await AddSignatureToPackageAsync(packageContext, signPackage, request, testLogger);
                    }

                    if (packageContext.IsRepositoryCounterSigned)
                    {
                        using (var request = new RepositorySignPackageRequest(new X509Certificate2(packageContext.RepositoryCountersignatureCertificate),
                                                                                HashAlgorithmName.SHA256,
                                                                                HashAlgorithmName.SHA256,
                                                                                packageContext.V3ServiceIndexUrl,
                                                                                packageContext.PackageOwners))
                        {
                            await AddRepositoryCountersignatureToSignedPackageAsync(packageContext, signPackage, request, testLogger);
                        }
                    }
#endif
                }
            }

            // Reset position
            stream.Position = 0;
        }

        private static List<(string, List<PackageDependency>)> GetPackageDependencies(SimpleTestPackageContext package)
        {
            if (package.PerFrameworkDependencies.Count > 0 && package.Dependencies.Count > 0)
            {
                throw new ArgumentException("A package context can't have dependencies with and without a group. Please use only one.");
            }
            var packageDependencies = new List<(string, List<PackageDependency>)>();

            if (package.PerFrameworkDependencies.Count > 0)
            {
                foreach (var dependencies in package.PerFrameworkDependencies)
                {
                    packageDependencies.Add((dependencies.Key.GetFrameworkString(), GetPackageDependencyList(dependencies.Value)));
                }
            }

            if (package.Dependencies.Count > 0)
            {
                packageDependencies.Add((string.Empty, GetPackageDependencyList(package.Dependencies)));
            }

            return packageDependencies;
        }

        private static List<PackageDependency> GetPackageDependencyList(List<SimpleTestPackageContext> packages)
        {
            return packages.Select(e =>
                new PackageDependency(
                    e.Id,
                    VersionRange.Parse(e.Version),
                    string.IsNullOrEmpty(e.Include)
                        ? new List<string>()
                        : e.Include.Split(',').ToList(),
                    string.IsNullOrEmpty(e.Exclude)
                        ? new List<string>()
                        : e.Exclude.Split(',').ToList())).ToList();
        }

#if IS_SIGNING_SUPPORTED
        private static SignPackageRequest GetPrimarySignRequest(SimpleTestPackageContext packageContext)
        {
            if (packageContext.V3ServiceIndexUrl != null)
            {
                return new RepositorySignPackageRequest(
                    new X509Certificate2(packageContext.PrimarySignatureCertificate),
                    HashAlgorithmName.SHA256,
                    HashAlgorithmName.SHA256,
                    packageContext.V3ServiceIndexUrl,
                    packageContext.PackageOwners);
            }

            return new AuthorSignPackageRequest(
                new X509Certificate2(packageContext.PrimarySignatureCertificate),
                HashAlgorithmName.SHA256);
        }

        private static async Task AddSignatureToPackageAsync(SimpleTestPackageContext packageContext, ISignedPackage package, SignPackageRequest request, ILogger logger)
        {
            var testSignatureProvider = new X509SignatureProvider(packageContext.PrimaryTimestampProvider);

            var zipArchiveHash = await package.GetArchiveHashAsync(request.SignatureHashAlgorithm, CancellationToken.None);
            var base64ZipArchiveHash = Convert.ToBase64String(zipArchiveHash);
            var signatureContent = new SignatureContent(SigningSpecifications.V1, request.SignatureHashAlgorithm, base64ZipArchiveHash);

            var signature = await testSignatureProvider.CreatePrimarySignatureAsync(request, signatureContent, logger, CancellationToken.None);

            using (var stream = new MemoryStream(signature.GetBytes()))
            {
                await package.AddSignatureAsync(stream, CancellationToken.None);
            }
        }

        private static async Task AddRepositoryCountersignatureToSignedPackageAsync(SimpleTestPackageContext packageContext, ISignedPackage package, RepositorySignPackageRequest request, ILogger logger)
        {
            var primarySignature = await package.GetPrimarySignatureAsync(CancellationToken.None);

            if (primarySignature != null)
            {
                var testSignatureProvider = new X509SignatureProvider(packageContext.CounterTimestampProvider);

                var signature = await testSignatureProvider.CreateRepositoryCountersignatureAsync(request, primarySignature, logger, CancellationToken.None);

                using (var stream = new MemoryStream(signature.GetBytes()))
                {

                    await package.AddSignatureAsync(stream, CancellationToken.None);
                }
            }
        }
#endif

        /// <summary>
        /// Create packages.
        /// </summary>
        public static async Task CreatePackagesAsync(string repositoryPath, params SimpleTestPackageContext[] package)
        {
            await CreatePackagesAsync(package.ToList(), repositoryPath);
        }

        /// <summary>
        /// Create all packages in the list, including dependencies.
        /// </summary>
        public static async Task CreatePackagesAsync(List<SimpleTestPackageContext> packages, string repositoryPath)
        {
            var done = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var toCreate = new Stack<SimpleTestPackageContext>(packages);

            while (toCreate.Count > 0)
            {
                var package = toCreate.Pop();

                if (done.Add($"{package.Id}|{package.Version.ToString()}"))
                {
                    await CreateFullPackageAsync(
                        repositoryPath,
                        package);

                    foreach (var dep in package.Dependencies)
                    {
                        toCreate.Push(dep);
                    }
                }
            }
        }

        /// <summary>
        /// Create an unzipped repository folder of nupkgs
        /// </summary>
        public static async Task CreateFolderFeedUnzipAsync(string root, params PackageIdentity[] packages)
        {
            var contexts = packages.Select(package => new SimpleTestPackageContext(package)).ToList();

            foreach (var context in contexts)
            {
                var name = $"{context.Id}.{context.Version.ToString()}";

                var nupkgPath = Path.Combine(root, name + ".nupkg");
                var folder = Path.Combine(root, name);
                var nuspecPath = Path.Combine(root, name, name + ".nuspec");

                Directory.CreateDirectory(folder);

                using (var tempRoot = TestDirectory.Create())
                {
                    await CreatePackagesAsync(tempRoot, context);

                    var input = Directory.GetFiles(tempRoot).Single();

                    using (var zip = new ZipArchive(File.OpenRead(input)))
                    {
                        zip.ExtractAll(folder);
                    }

                    foreach (var file in Directory.GetFiles(folder))
                    {
                        if (file.EndsWith(".nuspec"))
                        {
                            File.Move(file, nuspecPath);
                        }

                        // Delete the rest
                        File.Delete(file);
                    }

                    // move the nupkg
                    File.Move(input, nupkgPath);
                }
            }
        }

        /// <summary>
        /// Create a v2 folder of nupkgs
        /// </summary>
        public static async Task CreateFolderFeedV2Async(string root, params PackageIdentity[] packages)
        {
            var contexts = packages.Select(package => new SimpleTestPackageContext(package)).ToList();

            await CreatePackagesAsync(contexts, root);
        }

        /// <summary>
        /// Create a v3 folder of nupkgs
        /// </summary>
        public static async Task CreateFolderFeedV3Async(string root, params PackageIdentity[] packages)
        {
            await CreateFolderFeedV3Async(root, PackageSaveMode.Nupkg | PackageSaveMode.Nuspec, packages);
        }

        /// <summary>
        /// Create a v3 folder of nupkgs
        /// </summary>
        public static async Task CreateFolderFeedV3Async(string root, PackageSaveMode saveMode, params PackageIdentity[] packages)
        {
            var contexts = packages.Select(p => new SimpleTestPackageContext(p)).ToArray();

            await CreateFolderFeedV3Async(root, saveMode, contexts);
        }

        /// <summary>
        /// Create a v3 folder of nupkgs.
        /// Does not write out extracted files.
        /// </summary>
        public static async Task CreateFolderFeedV3Async(string root, params SimpleTestPackageContext[] contexts)
        {
            using (var tempRoot = TestDirectory.Create())
            {
                await CreatePackagesAsync(tempRoot, contexts);

                var saveMode = PackageSaveMode.Nupkg | PackageSaveMode.Nuspec;

                await CreateFolderFeedV3Async(root, saveMode, Directory.GetFiles(tempRoot));
            }
        }

        /// <summary>
        /// Create a v3 folder of nupkgs
        /// </summary>
        public static async Task CreateFolderFeedV3Async(string root, PackageSaveMode saveMode, params SimpleTestPackageContext[] contexts)
        {
            using (var tempRoot = TestDirectory.Create())
            {
                await CreatePackagesAsync(tempRoot, contexts);

                await CreateFolderFeedV3Async(root, saveMode, Directory.GetFiles(tempRoot));
            }
        }

        /// <summary>
        /// Create a v3 folder of nupkgs
        /// </summary>
        public static async Task CreateFolderFeedV3Async(string root, PackageSaveMode saveMode, params string[] nupkgPaths)
        {
            var pathResolver = new VersionFolderPathResolver(root);

            foreach (var file in nupkgPaths)
            {
                PackageIdentity identity = null;

                using (var reader = new PackageArchiveReader(File.OpenRead(file)))
                {
                    identity = reader.GetIdentity();
                }

                if (!File.Exists(pathResolver.GetHashPath(identity.Id, identity.Version)))
                {
                    using (var fileStream = File.OpenRead(file))
                    {
                        await PackageExtractor.InstallFromSourceAsync(
                            null,
                            identity,
                            (stream) => fileStream.CopyToAsync(stream, 4096, CancellationToken.None),
                            new VersionFolderPathResolver(root),
                            new PackageExtractionContext(
                                saveMode,
                                XmlDocFileSaveMode.None,
                                clientPolicyContext: null,
                                logger: NullLogger.Instance),
                            CancellationToken.None);
                    }
                }
            }
        }

        /// <summary>
        /// Create a packagets.config folder of nupkgs
        /// </summary>
        public static async Task CreateFolderFeedPackagesConfigAsync(string root, params PackageIdentity[] packages)
        {
            var contexts = packages.Select(p => new SimpleTestPackageContext(p)).ToArray();

            await CreateFolderFeedPackagesConfigAsync(root, contexts);
        }

        /// <summary>
        /// Create a packagets.config folder of nupkgs
        /// </summary>
        public static async Task CreateFolderFeedPackagesConfigAsync(string root, params SimpleTestPackageContext[] contexts)
        {
            using (var tempRoot = TestDirectory.Create())
            {
                await CreatePackagesAsync(tempRoot, contexts);

                await CreateFolderFeedPackagesConfigAsync(root, Directory.GetFiles(tempRoot));
            }
        }

        /// <summary>
        /// Create a packagets.config folder of nupkgs
        /// </summary>
        public static async Task CreateFolderFeedPackagesConfigAsync(string root, params string[] nupkgPaths)
        {
            var resolver = new PackagePathResolver(root);
            var context = new PackageExtractionContext(
                        PackageSaveMode.Defaultv2,
                        PackageExtractionBehavior.XmlDocFileSaveMode,
                        clientPolicyContext: null,
                        logger: NullLogger.Instance);

            foreach (var path in nupkgPaths)
            {
                using (var stream = File.OpenRead(path))
                {
                    await PackageExtractor.ExtractPackageAsync(string.Empty, stream, resolver, context, CancellationToken.None);
                }
            }
        }

        /// <summary>
        /// Create packages with PackageBuilder, this includes OPC support.
        /// </summary>
        public static void CreateOPCPackage(SimpleTestPackageContext package, string repositoryPath)
        {
            CreateOPCPackages(new List<SimpleTestPackageContext>() { package }, repositoryPath);
        }

        /// <summary>
        /// Create packages with PackageBuilder, this includes OPC support.
        /// </summary>
        public static void CreateOPCPackages(List<SimpleTestPackageContext> packages, string repositoryPath, bool developmentDependency = false)
        {
            foreach (var package in packages)
            {
                var builder = new PackageBuilder()
                {
                    Id = package.Id,
                    Version = NuGetVersion.Parse(package.Version),
                    Description = "Description.",
                    DevelopmentDependency = developmentDependency
                };

                builder.Authors.Add("testAuthor");

                foreach (var file in package.Files)
                {
                    builder.Files.Add(CreatePackageFile(file.Key));
                }

                using (var stream = File.OpenWrite(Path.Combine(repositoryPath, $"{package.Identity.Id}.{package.Identity.Version.ToString()}.nupkg")))
                {
                    builder.Save(stream);
                }
            }
        }

        /// <summary>
        /// Delete nuspec file from the package
        /// </summary>
        /// <param name="nupkgPath">Path to package file</param>
        public static Task DeleteNuspecFileFromPackageAsync(string nupkgPath)
        {
            return Task.Run(() =>
            {
                using (FileStream zipToOpen = new FileStream(nupkgPath, FileMode.Open))
                {
                    using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                    {
                        var nuspec = archive.Entries.Where(entry => entry.Name.EndsWith(NuGetConstants.ManifestExtension)).SingleOrDefault();
                        nuspec?.Delete();
                    }
                }
            }, CancellationToken.None);

        }
        private static IPackageFile CreatePackageFile(string name)
        {
            var file = new InMemoryFile
            {
                Path = name,
                Stream = new MemoryStream()
            };

            string effectivePath;
            var fx = FrameworkNameUtility.ParseNuGetFrameworkFromFilePath(name, out effectivePath);
            file.EffectivePath = effectivePath;
            if (fx != null)
            {
                file.NuGetFramework = fx;
                if (fx.Version.Major < 5)
                {
                    file.TargetFramework = new FrameworkName(fx.DotNetFrameworkName);
                }
            }

            return file;
        }

        private class InMemoryFile : IPackageFile
        {
            private DateTimeOffset _lastWriteTime;

            public string EffectivePath { get; set; }

            public string Path { get; set; }

            public FrameworkName TargetFramework { get; set; }

            public NuGetFramework NuGetFramework { get; set; }

            public MemoryStream Stream { get; set; }

            public Stream GetStream()
            {
                _lastWriteTime = DateTimeOffset.UtcNow;
                return Stream;
            }

            public DateTimeOffset LastWriteTime => _lastWriteTime;
        }
    }
}

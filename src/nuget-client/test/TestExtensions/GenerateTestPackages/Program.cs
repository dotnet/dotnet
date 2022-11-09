using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.CSharp;
using NuGet;

namespace GenerateTestPackages
{
    class Program
    {
        const string KeyFileName = "TestPackageKey.snk";

        static Dictionary<string, PackageInfo> Packages = new Dictionary<string, PackageInfo>();

        static void Main(string[] args)
        {
            string path = args[0];
            var extension = Path.GetExtension(path);

            if (extension.Equals(".nuspec", StringComparison.OrdinalIgnoreCase))
            {
                BuildPackage(path);
            }
            else
            {
                BuildDependency(path);
            }
        }

        private static void BuildPackage(string nuspecPath)
        {
            var repositoryPath = Path.GetDirectoryName(nuspecPath);
            var basePath = Path.Combine(repositoryPath, "files", Path.GetFileNameWithoutExtension(nuspecPath));
            Directory.CreateDirectory(basePath);

            var createdFiles = new List<string>();
            bool deleteDir = true;
            using (var fileStream = File.OpenRead(nuspecPath))
            {
                var manifest = Manifest.ReadFrom(fileStream, validateSchema: true);
                var packageBuilder = new PackageBuilder();
                packageBuilder.Populate(manifest.Metadata);
                if (!manifest.Files.IsEmpty())
                {
                    foreach (var file in manifest.Files)
                    {
                        string outputPath = Path.Combine(basePath, file.Source);
                        if (File.Exists(outputPath))
                        {
                            deleteDir = false;
                            // A user created file exists. Continue to next file.
                            continue;
                        }

                        createdFiles.Add(outputPath);
                        string outputDir = Path.GetDirectoryName(outputPath);
                        if (!Directory.Exists(outputDir))
                        {
                            Directory.CreateDirectory(outputDir);
                        }

                        if (file.Source.StartsWith(@"lib" + Path.DirectorySeparatorChar) &&
                            (file.Source.EndsWith(".dll") && !file.Source.EndsWith("resources.dll")))
                        {
                            var name = Path.GetFileNameWithoutExtension(file.Source);
                            CreateAssembly(new PackageInfo(manifest.Metadata.Id + ":" + manifest.Metadata.Version),
                                           outputPath: outputPath);
                        }
                        else
                        {
                            File.WriteAllBytes(outputPath, new byte[0]);
                        }
                    }

                    packageBuilder.PopulateFiles(basePath, manifest.Files);
                }

                string nupkgDirectory = Path.GetFullPath("packages");
                Directory.CreateDirectory(nupkgDirectory);
                string nupkgPath = Path.Combine(nupkgDirectory, Path.GetFileNameWithoutExtension(nuspecPath)) + ".nupkg";
                using (var nupkgStream = File.Create(nupkgPath))
                {
                    packageBuilder.Save(nupkgStream);
                }
            }
            try
            {
                if (deleteDir)
                {
                    Directory.Delete(basePath, recursive: true);
                }
                else
                {
                    // Delete files that we created.
                    createdFiles.ForEach(File.Delete);
                }
            }
            catch { }
        }

        private static void BuildDependency(string path)
        {
            var document = XDocument.Load(path);

            XNamespace ns = "http://schemas.microsoft.com/vs/2009/dgml";

            // Parse through the dgml file and group things by Source
            Packages = document.Descendants(ns + "Link")
                .ToLookup(l => l.Attribute("Source").Value)
                .Select(group => new PackageInfo(group.Key, group.Select(GetDependencyInfoFromLinkTag)))
                .ToDictionary(p => p.FullName.ToString());

            // Add all the packages that only exist as targets to the dictionary
            var allPackageNames = Packages.Values.SelectMany(p => p.Dependencies).Select(dep => dep.FullName.ToString()).Distinct().ToList();
            foreach (var dependency in allPackageNames)
            {
                if (!Packages.ContainsKey(dependency))
                {
                    Packages.Add(dependency, new PackageInfo(dependency));
                }
            }

            // Process all the packages
            foreach (var p in Packages.Values)
            {
                EnsurePackageProcessed(p);
            }

            // Add all packages that are simply listed as Nodes.
            var remainders = document.Root.Elements(ns + "Nodes")
                                          .Elements()
                                          .Select(s => new PackageInfo(s.Attribute("Id").Value));

            foreach (var package in remainders)
            {
                if (!Packages.ContainsKey(package.FullName.ToString()))
                {
                    EnsurePackageProcessed(package);
                }
            }
        }

        static DependencyInfo GetDependencyInfoFromLinkTag(XElement linkTag)
        {
            var label = linkTag.Attribute("Label");

            return new DependencyInfo(
                new FullPackageName(linkTag.Attribute("Target").Value),
                label != null ? VersionUtility.ParseVersionSpec(label.Value) : null);
        }

        static void EnsurePackageProcessed(string fullName)
        {
            EnsurePackageProcessed(Packages[fullName]);
        }

        static void EnsurePackageProcessed(PackageInfo package)
        {
            if (!package.Processed)
            {
                ProcessPackage(package);
                package.Processed = true;
            }
        }

        static void ProcessPackage(PackageInfo package)
        {
            // Make sure all its dependencies are processed first
            foreach (var dependency in package.Dependencies)
            {
                EnsurePackageProcessed(dependency.FullName.ToString());
            }

            Console.WriteLine("Creating package {0}", package.FullName);
            CreateAssembly(package);
            CreatePackage(package);
        }

        static void CreateAssembly(PackageInfo package, string outputPath = null)
        {

            // Save the snk file from the embedded resource to the disk so we can use it when we compile
            using (var resStream = typeof(Program).Assembly.GetManifestResourceStream("GenerateTestPackages." + KeyFileName))
            {
                using (var snkStream = File.Create(KeyFileName))
                {
                    resStream.CopyTo(snkStream);
                }
            }


            var codeProvider = new CSharpCodeProvider();
            var compilerParams = new CompilerParameters()
            {
                OutputAssembly = outputPath ?? Path.GetFullPath(GetAssemblyFullPath(package.FullName)),
                CompilerOptions = "/keyfile:" + KeyFileName
            };

            // Add all the dependencies as referenced assemblies
            foreach (DependencyInfo dependency in package.Dependencies)
            {
                compilerParams.ReferencedAssemblies.Add(GetAssemblyFullPath(dependency.FullName));
            }

            // Create the source code and compile it using CodeDom
            var generator = new AssemblySourceFileGenerator() { Package = package };
            CompilerResults results = codeProvider.CompileAssemblyFromSource(compilerParams, generator.TransformText());

            if (results.Errors.HasErrors)
            {
                Console.WriteLine(results.Errors[0]);
            }

            File.Delete(KeyFileName);
        }

        static void CreatePackage(PackageInfo package)
        {
            var packageBuilder = new PackageBuilder()
            {
                Id = package.Id,
                Version = package.Version,
                Description = "Some test package"
            };

            packageBuilder.Authors.Add(".NET Foundation");

            string assemblySourcePath = GetAssemblyFullPath(package.FullName);
            packageBuilder.Files.Add(new PhysicalPackageFile()
            {
                SourcePath = assemblySourcePath,
                TargetPath = @"lib\" + Path.GetFileName(assemblySourcePath)
            });

            var set = new PackageDependencySet(VersionUtility.DefaultTargetFramework,
                package.Dependencies.Select(dependency => new PackageDependency(dependency.Id, dependency.VersionSpec)));
            packageBuilder.DependencySets.Add(set);

            using (var stream = File.Create(GetPackageFileFullPath(package)))
            {
                packageBuilder.Save(stream);
            }
        }

        static string GetAssemblyFullPath(FullPackageName fullName)
        {
            string relativeDir = string.Format(@"Assemblies\{0}\{1}", fullName.Id, fullName.Version);
            string fullDir = Path.GetFullPath(relativeDir);
            Directory.CreateDirectory(fullDir);
            return Path.Combine(fullDir, fullName.Id + ".dll");
        }

        static string GetPackageFileFullPath(PackageInfo package)
        {
            string packagesFolder = Path.GetFullPath("Packages");
            Directory.CreateDirectory(packagesFolder);
            string packageFileName = String.Format("{0}.{1}.nupkg", package.Id, package.Version);
            return Path.Combine(packagesFolder, packageFileName);
        }
    }

    class PackageInfo
    {
        public PackageInfo(string nameAndVersion, IEnumerable<DependencyInfo> dependencies = null)
        {
            FullName = new FullPackageName(nameAndVersion);

            Dependencies = dependencies != null ? dependencies : Enumerable.Empty<DependencyInfo>();
        }

        public FullPackageName FullName { get; private set; }
        public string Id { get { return FullName.Id; } }
        public SemanticVersion Version { get { return FullName.Version; } }
        public IEnumerable<DependencyInfo> Dependencies { get; private set; }
        public bool Processed { get; set; }

        public override string ToString()
        {
            return FullName.ToString();
        }
    }

    // Contains at least an exact id:version, and optionally a fuller version spec
    class DependencyInfo
    {
        public DependencyInfo(FullPackageName fullName, IVersionSpec versionSpec)
        {
            FullName = fullName;

            // Default to the simple version (which means min-version)
            VersionSpec = versionSpec ?? VersionUtility.ParseVersionSpec(FullName.Version.ToString());
        }

        public FullPackageName FullName { get; private set; }
        public IVersionSpec VersionSpec { get; private set; }
        public string Id { get { return FullName.Id; } }
    }

    class FullPackageName
    {
        public FullPackageName(string nameAndVersion)
        {
            var parts = nameAndVersion.Split(':');
            Id = parts[0];
            Version = new SemanticVersion(parts[1]);
        }

        public string Id { get; private set; }
        public SemanticVersion Version { get; private set; }

        public override string ToString()
        {
            return Id + ":" + Version;
        }
    }
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.CommandLine.XPlat;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Packaging.Core;
using NuGet.ProjectModel;
using NuGet.Test.Utility;
using NuGet.Versioning;

namespace NuGet.XPlat.FuncTest
{
    public static class XPlatTestUtils
    {
        /// <summary>
        /// Add a dependency to project.json.
        /// </summary>
        public static void AddDependency(JObject json, string id, string version)
        {
            var deps = (JObject)json["dependencies"];

            deps.Add(new JProperty(id, version));
        }

        /// <summary>
        /// Basic netcoreapp1.0 config
        /// </summary>
        public static JObject BasicConfigNetCoreApp
        {
            get
            {
                var json = new JObject();

                var frameworks = new JObject
                {
                    ["netcoreapp1.0"] = new JObject()
                };

                json["dependencies"] = new JObject();

                json["frameworks"] = frameworks;

                return json;
            }
        }

        /// <summary>
        /// Write a json file to disk.
        /// </summary>
        public static void WriteJson(JObject json, string outputPath)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath));

            using (var fs = File.Open(outputPath, FileMode.CreateNew))
            using (var sw = new StreamWriter(fs))
            using (var writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Newtonsoft.Json.Formatting.Indented;

                var serializer = new JsonSerializer();
                serializer.Serialize(writer, json);
            }
        }

        /// <summary>
        /// Copies test sources configuration to a test folder
        /// </summary>
        public static string CopyFuncTestConfig(string destinationFolder)
        {
            var testSettingsFolder = TestSources.GetConfigFileRoot();
            var funcTestConfigPath = Path.Combine(testSettingsFolder, TestSources.ConfigFile);

            var destConfigFile = Path.Combine(destinationFolder, "NuGet.Config");
            File.Copy(funcTestConfigPath, destConfigFile);
            return destConfigFile;
        }

        private const string ProtocolConfigFileName = "NuGet.Protocol.FuncTest.config";

        public static string ReadApiKey(string feedName)
        {
            var testSettingsFolder = TestSources.GetConfigFileRoot();
            var protocolConfigPath = Path.Combine(testSettingsFolder, ProtocolConfigFileName);

            var fullPath = NuGetEnvironment.GetFolderPath(NuGetFolderPath.UserSettingsDirectory);
            using (Stream configStream = File.OpenRead(protocolConfigPath))
            {
                var doc = XDocument.Load(XmlReader.Create(configStream));
                var element = doc.Root.Element(feedName);

                return element?.Element("ApiKey")?.Value;
            }
        }

        public static void WaitForDebugger()
        {
            Console.WriteLine("Waiting for debugger to attach.");
            Console.WriteLine($"Process ID: {Process.GetCurrentProcess().Id}");

            while (!Debugger.IsAttached)
            {
                System.Threading.Thread.Sleep(100);
            }
            Debugger.Break();
        }

        public static XDocument LoadSafe(string filePath)
        {
            var settings = CreateSafeSettings();
            using (var reader = XmlReader.Create(filePath, settings))
            {
                return XDocument.Load(reader);
            }
        }

        public static XmlReaderSettings CreateSafeSettings(bool ignoreWhiteSpace = false)
        {
            var safeSettings = new XmlReaderSettings
            {
#if !IS_CORECLR
                XmlResolver = null,
#endif
                DtdProcessing = DtdProcessing.Prohibit,
                IgnoreWhitespace = ignoreWhiteSpace
            };

            return safeSettings;
        }

        public static SimpleTestProjectContext CreateProject(string projectName,
            SimpleTestPathContext pathContext,
            SimpleTestPackageContext package,
            string projectFrameworks,
            string packageFramework = null)
        {
            var project = SimpleTestProjectContext.CreateNETCoreWithSDK(
                    projectName: projectName,
                    solutionRoot: pathContext.SolutionRoot,
                    frameworks: MSBuildStringUtility.Split(projectFrameworks));

            if (packageFramework == null)
            {
                project.AddPackageToAllFrameworks(package);
            }
            else
            {
                project.AddPackageToFramework(packageFramework, package);
            }
            project.Save();
            return project;
        }

        public static SimpleTestProjectContext CreateProject(string projectName,
            SimpleTestPathContext pathContext,
            string projectFrameworks)
        {
            var settings = Settings.LoadDefaultSettings(Path.GetDirectoryName(pathContext.NuGetConfig), Path.GetFileName(pathContext.NuGetConfig), null);
            var project = SimpleTestProjectContext.CreateNETCoreWithSDK(
                    projectName: projectName,
                    solutionRoot: pathContext.SolutionRoot,
                    frameworks: MSBuildStringUtility.Split(projectFrameworks));

            project.FallbackFolders = (IList<string>)SettingsUtility.GetFallbackPackageFolders(settings);
            project.GlobalPackagesFolder = SettingsUtility.GetGlobalPackagesFolder(settings);
            var packageSourceProvider = new PackageSourceProvider(settings);
            project.Sources = packageSourceProvider.LoadPackageSources();

            project.Save();
            return project;
        }

        public static SimpleTestPackageContext CreatePackage(string packageId = "packageX",
            string packageVersion = "1.0.0",
            string frameworkString = null,
            PackageType packageType = null,
            bool developmentDependency = false)
        {
            var package = new SimpleTestPackageContext()
            {
                Id = packageId,
                Version = packageVersion
            };
            var frameworks = MSBuildStringUtility.Split(frameworkString);

            if (packageType != null)
            {
                package.PackageType = packageType;
            }

            // Make the package Compatible with specific frameworks
            frameworks?
                .ToList()
                .ForEach(f => package.AddFile($"lib/{f}/a.dll"));

            // To ensure that the nuspec does not have System.Runtime.dll
            package.Nuspec = GetNetCoreNuspec(packageId, packageVersion, developmentDependency);

            return package;
        }

        internal static PackageReferenceArgs GetPackageReferenceArgs(string packageId, SimpleTestProjectContext project)
        {
            var logger = new TestCommandOutputLogger();
            return new PackageReferenceArgs(project.ProjectPath, logger)
            {
                PackageId = packageId
            };
        }

        internal static PackageReferenceArgs GetPackageReferenceArgs(string packageId, string packageVersion, SimpleTestProjectContext project,
            string frameworks = "", string packageDirectory = "", string sources = "", bool noRestore = false, bool noVersion = false, bool prerelease = false)
        {
            var logger = new TestCommandOutputLogger();
            var dgFilePath = string.Empty;
            if (!noRestore)
            {
                dgFilePath = CreateDGFileForProject(project);
            }
            return new PackageReferenceArgs(project.ProjectPath, logger)
            {
                Frameworks = MSBuildStringUtility.Split(frameworks),
                Sources = MSBuildStringUtility.Split(sources),
                PackageDirectory = packageDirectory,
                NoRestore = noRestore,
                NoVersion = noVersion,
                DgFilePath = dgFilePath,
                Prerelease = prerelease,
                PackageVersion = packageVersion,
                PackageId = packageId
            };
        }

        public static XDocument GetNetCoreNuspec(string package, string packageVersion, bool developmentDependency = false)
        {
            if (developmentDependency)
            {
                return XDocument.Parse($@"<?xml version=""1.0"" encoding=""utf-8""?>
                        <package>
                        <metadata>
                            <id>{package}</id>
                            <version>{packageVersion}</version>
                            <developmentDependency>true</developmentDependency>
                            <title />
                        </metadata>
                        </package>");
            }

            return XDocument.Parse($@"<?xml version=""1.0"" encoding=""utf-8""?>
                        <package>
                        <metadata>
                            <id>{package}</id>
                            <version>{packageVersion}</version>
                            <title />
                        </metadata>
                        </package>");
        }

        // Assert Helper Methods

        public static bool ValidateReference(XElement root, string packageId, string version, PackageType packageType = null, bool developmentDependency = false)
        {

            var packageReferences = root
                    .Descendants(GetReferenceType(packageType))
                    .Where(d => d.FirstAttribute.Value.Equals(packageId, StringComparison.OrdinalIgnoreCase));

            if (packageReferences.Count() != 1)
            {
                return false;
            }

            var versionAttribute = packageReferences
                .First()
                .Attribute("Version");

            if (versionAttribute == null ||
                !versionAttribute.Value.Equals(version, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (developmentDependency)
            {
                var privateAssets = packageReferences.First().Element("PrivateAssets");

                if (privateAssets == null ||
                    !privateAssets.Value.Equals("all", StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                var includeAssets = packageReferences.First().Element("IncludeAssets");

                if (includeAssets == null ||
                    !includeAssets.Value.Equals("runtime; build; native; contentfiles; analyzers; buildtransitive", StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool ValidateAssetsFile(SimpleTestProjectContext project, string packageId)
        {
            if (!File.Exists(project.AssetsFileOutputPath))
            {
                return false;
            }

            return project.AssetsFile.Targets.Any(t => t.Libraries.Any(library => string.Equals(library.Name, packageId, StringComparison.OrdinalIgnoreCase)));
        }

        public static bool ValidateNoReference(XElement root, string packageId, PackageType packageType = null)
        {
            var packageReferences = root
                    .Descendants(GetReferenceType(packageType))
                    .Where(d => d.FirstAttribute.Value.Equals(packageId, StringComparison.OrdinalIgnoreCase));

            return !(packageReferences.Any());
        }

        public static XElement GetItemGroupForFramework(XElement root, string framework, PackageType packageType = null)
        {
            var itemGroups = root.Descendants("ItemGroup");
            return itemGroups
                    .Where(i => i.Descendants(GetReferenceType(packageType)).Any() &&
                                i.FirstAttribute != null &&
                                i.FirstAttribute.Name.LocalName.Equals("Condition", StringComparison.OrdinalIgnoreCase) &&
                                i.FirstAttribute.Value.Trim().Equals(GetTargetFrameworkCondition(framework), StringComparison.OrdinalIgnoreCase))
                     .First();
        }

        public static XElement GetItemGroupForAllFrameworks(XElement root, PackageType packageType = null)
        {
            var itemGroups = root.Descendants("ItemGroup");
            var referenceType = GetReferenceType(packageType);
            foreach (var i in itemGroups)
            {
                var x = i.Descendants(referenceType);
            }
            return itemGroups
                    .Where(i => i.Descendants(referenceType).Any() &&
                                i.FirstAttribute == null)
                     .First();
        }

        public static bool ValidateTwoReferences(XElement root, SimpleTestPackageContext packageX, SimpleTestPackageContext packageY)
        {
            return ValidateReference(root, packageX.Id, packageX.Version) &&
                ValidateReference(root, packageY.Id, packageY.Version);
        }

        public static bool ValidatePackageDownload(string packageDirectoryPath, SimpleTestPackageContext package)
        {
            return Directory.Exists(packageDirectoryPath) &&
                Directory.Exists(Path.Combine(packageDirectoryPath, package.Id.ToLower())) &&
                Directory.Exists(Path.Combine(packageDirectoryPath, package.Id.ToLower(), package.Version.ToLower())) &&
                Directory.EnumerateFiles(Path.Combine(packageDirectoryPath, package.Id.ToLower(), package.Version.ToLower())).Any();
        }

        public static string CreateDGFileForProject(SimpleTestProjectContext project)
        {
            var dgSpec = new DependencyGraphSpec();
            var dgFilePath = Path.Combine(Directory.GetParent(project.ProjectPath).FullName, "temp.dg");
            dgSpec.AddRestore(project.ProjectName);
            dgSpec.AddProject(project.PackageSpec);
            dgSpec.Save(dgFilePath);
            return dgFilePath;
        }

        public static string GetCommonFramework(string frameworkStringA, string frameworkStringB, string frameworkStringC)
        {
            var frameworksA = MSBuildStringUtility.Split(frameworkStringA);
            var frameworksB = MSBuildStringUtility.Split(frameworkStringB);
            var frameworksC = MSBuildStringUtility.Split(frameworkStringC);
            return frameworksA.ToList()
                .Intersect(frameworksB.ToList())
                .Intersect(frameworksC.ToList())
                .First();
        }

        public static string GetCommonFramework(string frameworkStringA, string frameworkStringB)
        {
            var frameworksA = MSBuildStringUtility.Split(frameworkStringA);
            var frameworksB = MSBuildStringUtility.Split(frameworkStringB);
            return frameworksA.ToList()
                .Intersect(frameworksB.ToList())
                .First();
        }

        public static XDocument LoadCSProj(string path)
        {
            return LoadSafe(path);
        }

        public static string GetTargetFrameworkCondition(string targetFramework)
        {
            return string.Format("'$(TargetFramework)' == '{0}'", targetFramework);
        }

        public static void DisposeTemporaryFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public static string GetReferenceType(PackageType packageType)
        {
            var referenceType = "PackageReference";

            if (packageType == PackageType.DotnetCliTool)
            {
                referenceType = "DotNetCliToolReference";
            }

            return referenceType;
        }
    }
}

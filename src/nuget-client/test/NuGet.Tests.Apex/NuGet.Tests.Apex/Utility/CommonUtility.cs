// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using EnvDTE;
using FluentAssertions;
using Microsoft.Test.Apex.Services;
using Microsoft.Test.Apex.VisualStudio;
using Microsoft.Test.Apex.VisualStudio.Solution;
using NuGet.Common;
using NuGet.LibraryModel;
using NuGet.Packaging.Signing;
using NuGet.ProjectModel;
using NuGet.Test.Utility;
using NuGet.Versioning;
using NuGet.VisualStudio;
using Thread = System.Threading.Thread;

namespace NuGet.Tests.Apex
{
    public class CommonUtility
    {
        private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(10);
        private static readonly TimeSpan Interval = TimeSpan.FromSeconds(2);

        public static async Task CreatePackageInSourceAsync(string packageSource, string packageName, string packageVersion)
        {
            var package = CreatePackage(packageName, packageVersion);
            await SimpleTestPackageUtility.CreatePackagesAsync(packageSource, package);
        }

        public static async Task CreateDependenciesPackageInSourceAsync(string packageSource, string packageName, string packageVersion, string transitivePackageName, string transitivePackageVersion)
        {
            var packageA = CreatePackage(packageName, packageVersion);
            var packageB = CreatePackage(transitivePackageName, transitivePackageVersion);
            packageA.Dependencies.Add(packageB);
            await SimpleTestPackageUtility.CreatePackagesAsync(packageSource, packageA);
        }

        public static async Task CreateAuthorSignedPackageInSourceAsync(
            string packageSource,
            string packageName,
            string packageVersion,
            X509Certificate2 testCertificate,
            Uri timestampProviderUrl = null)
        {
            var package = CreateAuthorSignedPackage(packageName, packageVersion, testCertificate, timestampProviderUrl);
            await SimpleTestPackageUtility.CreatePackagesAsync(packageSource, package);
        }

        public static SimpleTestPackageContext CreateAuthorSignedPackage(
            string packageName,
            string packageVersion,
            X509Certificate2 testCertificate,
            Uri timestampProviderUrl = null)
        {
            var package = CreatePackage(packageName, packageVersion);
            return AuthorSignPackage(package, testCertificate, timestampProviderUrl);
        }

        public static SimpleTestPackageContext CreateRepositorySignedPackage(
            string packageName,
            string packageVersion,
            X509Certificate2 testCertificate,
            Uri v3ServiceIndexUrl,
            IReadOnlyList<string> packageOwners = null,
            Uri timestampProviderUrl = null)
        {
            var package = CreatePackage(packageName, packageVersion);
            return RepositorySignPackage(package, testCertificate, v3ServiceIndexUrl, packageOwners, timestampProviderUrl);
        }

        public static SimpleTestPackageContext CreateRepositoryCountersignedPackage(
            string packageName,
            string packageVersion,
            X509Certificate2 authorCertificate,
            X509Certificate2 repoCertificate,
            Uri v3ServiceIndexUrl,
            IReadOnlyList<string> packageOwners = null,
            Uri timestampProviderUrl = null)
        {
            var package = CreatePackage(packageName, packageVersion);
            var authorSignedPackage = AuthorSignPackage(package, authorCertificate, timestampProviderUrl);
            return RepositoryCountersignPackage(authorSignedPackage, repoCertificate, v3ServiceIndexUrl, packageOwners, timestampProviderUrl);
        }

        public static async Task CreateNetFrameworkPackageInSourceAsync(string packageSource, string packageName, string packageVersion, string requestAdditionalContent = null)
        {
            var package = CreatePackage(packageName, packageVersion, requestAdditionalContent);
            await SimpleTestPackageUtility.CreatePackagesAsync(packageSource, package);
        }

        public static SimpleTestPackageContext AuthorSignPackage(
            SimpleTestPackageContext package,
            X509Certificate2 authorCertificate,
            Uri timestampProviderUrl = null)
        {
            package.IsPrimarySigned = true;
            package.PrimarySignatureCertificate = authorCertificate;

            if (package.PrimaryTimestampProvider == null && timestampProviderUrl != null)
            {
                package.PrimaryTimestampProvider = new Rfc3161TimestampProvider(timestampProviderUrl);
            }

            return package;
        }

        public static SimpleTestPackageContext RepositorySignPackage(
            SimpleTestPackageContext package,
            X509Certificate2 repoCertificate,
            Uri v3ServiceIndexUrl,
            IReadOnlyList<string> packageOwners = null,
            Uri timestampProviderUrl = null)
        {
            package.IsPrimarySigned = true;
            package.PrimarySignatureCertificate = repoCertificate;
            package.V3ServiceIndexUrl = v3ServiceIndexUrl;
            package.PackageOwners = packageOwners;

            if (package.PrimaryTimestampProvider == null && timestampProviderUrl != null)
            {
                package.PrimaryTimestampProvider = new Rfc3161TimestampProvider(timestampProviderUrl);
            }

            return package;
        }

        public static SimpleTestPackageContext RepositoryCountersignPackage(
            SimpleTestPackageContext package,
            X509Certificate2 repoCertificate,
            Uri v3ServiceIndexUrl,
            IReadOnlyList<string> packageOwners = null,
            Uri timestampProviderUrl = null)
        {
            if (!package.IsPrimarySigned)
            {
                throw new InvalidOperationException("Package has to be primary signed");
            }

            package.IsRepositoryCounterSigned = true;
            package.RepositoryCountersignatureCertificate = repoCertificate;
            package.V3ServiceIndexUrl = v3ServiceIndexUrl;
            package.PackageOwners = packageOwners;

            if (package.CounterTimestampProvider == null && timestampProviderUrl != null)
            {
                package.CounterTimestampProvider = new Rfc3161TimestampProvider(timestampProviderUrl);
            }

            return package;
        }

        public static SimpleTestPackageContext CreatePackage(string packageName, string packageVersion, string requestAdditionalContent = null)
        {
            var package = new SimpleTestPackageContext(packageName, packageVersion);
            package.Files.Clear();
            package.AddFile("lib/net45/_._");
            package.AddFile("lib/netstandard1.0/_._");

            if (!string.IsNullOrWhiteSpace(requestAdditionalContent))
            {
                package.AddFile("lib/net45/" + requestAdditionalContent);
            }

            return package;
        }

        public static void AssertPackageReferenceExists(VisualStudioHost visualStudio, ProjectTestExtension project, string packageName, string packageVersion, ITestLogger logger)
        {
            logger.WriteMessage($"Checking for PackageReference {packageName} {packageVersion}");

            var matches = GetPackageReferences(project)
                .Where(e => e.Name.Equals(packageName, StringComparison.OrdinalIgnoreCase)
                        && e.LibraryRange.VersionRange.MinVersion.Equals(NuGetVersion.Parse(packageVersion)))
                .ToList();

            logger.WriteMessage($"Matches: {matches.Count}");

            matches.Any().Should().BeTrue($"A PackageReference with {packageName}/{packageVersion} was not found in {project.FullPath}");
        }

        public static void AssertPackageReferenceDoesNotExist(VisualStudioHost visualStudio, ProjectTestExtension project, string packageName, string packageVersion, ITestLogger logger)
        {
            logger.WriteMessage($"Checking for PackageReference {packageName} {packageVersion}");

            var matches = GetPackageReferences(project)
                .Where(e => e.Name.Equals(packageName, StringComparison.OrdinalIgnoreCase)
                        && e.LibraryRange.VersionRange.MinVersion.Equals(NuGetVersion.Parse(packageVersion)))
                .ToList();

            logger.WriteMessage($"Matches: {matches.Count}");

            matches.Any().Should().BeFalse($"A PackageReference with {packageName}/{packageVersion} was found in {project.FullPath}");
        }

        public static void AssertPackageReferenceDoesNotExist(VisualStudioHost visualStudio, ProjectTestExtension project, string packageName, ITestLogger logger)
        {
            logger.WriteMessage($"Checking for PackageReference {packageName}");

            var matches = GetPackageReferences(project)
                .Where(e => e.Name.Equals(packageName, StringComparison.OrdinalIgnoreCase))
                .ToList();

            logger.WriteMessage($"Matches: {matches.Count}");

            matches.Any().Should().BeFalse($"A PackageReference for {packageName} was found in {project.FullPath}");
        }

        public static List<LibraryDependency> GetPackageReferences(ProjectTestExtension project)
        {
            project.Save();
            var doc = XDocument.Load(project.FullPath);

            return doc.Root.Descendants()
                .Where(e => e.Name.LocalName.Equals("PackageReference", StringComparison.OrdinalIgnoreCase))
                .Select(e => new LibraryDependency()
                {
                    LibraryRange = new LibraryRange(e.Attribute(XName.Get("Include")).Value, VersionRange.Parse(e.Attribute(XName.Get("Version")).Value), LibraryDependencyTarget.Package),
                    IncludeType = LibraryIncludeFlags.All,
                    SuppressParent = LibraryIncludeFlags.None,
                    NoWarn = new List<NuGetLogCode>(),
                    AutoReferenced = false,
                    GeneratePathProperty = false
                })
                .ToList();

        }

        public static void AssertPackageInAssetsFile(VisualStudioHost visualStudio, ProjectTestExtension project, string packageName, string packageVersion, ITestLogger logger)
        {
            logger.WriteMessage($"Checking assets file for {packageName}");

            var testService = visualStudio.Get<NuGetApexTestService>();
            testService.WaitForAutoRestore();

            var assetsFilePath = GetAssetsFilePath(project.FullPath);

            // Project has an assets file, let's look there to assert
            var inAssetsFile = IsPackageInstalledInAssetsFile(assetsFilePath, packageName, packageVersion, true);

            logger.WriteMessage($"Exists: {inAssetsFile}");

            inAssetsFile.Should().BeTrue(AppendErrors($"{packageName}/{packageVersion} should be installed in {project.Name}", visualStudio));
        }

        public static void AssertPackageInPackagesConfig(VisualStudioHost visualStudio, ProjectTestExtension project, string packageName, string packageVersion, ITestLogger logger)
        {
            logger.WriteMessage($"Checking project {packageName}");
            var testService = visualStudio.Get<NuGetApexTestService>();

            // Check using the IVs APIs
            var exists = testService.IsPackageInstalled(project.UniqueName, packageName, packageVersion);

            logger.WriteMessage($"Exists: {exists}");

            exists.Should().BeTrue(AppendErrors($"{packageName}/{packageVersion} should be installed in {project.Name}", visualStudio));
        }

        public static void AssertPackageInPackagesConfig(VisualStudioHost visualStudio, ProjectTestExtension project, string packageName, ITestLogger logger)
        {
            logger.WriteMessage($"Checking project {packageName}");
            var testService = visualStudio.Get<NuGetApexTestService>();

            // Check using the IVs APIs
            var exists = testService.IsPackageInstalled(project.UniqueName, packageName);
            logger.WriteMessage($"Exists: {exists}");

            exists.Should().BeTrue(AppendErrors($"{packageName} should be installed in {project.Name}", visualStudio));
        }

        public static void AssertPackageNotInAssetsFile(VisualStudioHost visualStudio, ProjectTestExtension project, string packageName, string packageVersion, ITestLogger logger)
        {
            logger.WriteMessage($"Checking assets file for {packageName}");
            var testService = visualStudio.Get<NuGetApexTestService>();
            testService.WaitForAutoRestore();

            var assetsFilePath = GetAssetsFilePath(project.FullPath);

            // Project has an assets file, let's look there to assert
            var inAssetsFile = IsPackageInstalledInAssetsFile(assetsFilePath, packageName, packageVersion, false);
            logger.WriteMessage($"Exists: {inAssetsFile}");

            inAssetsFile.Should().BeFalse(AppendErrors($"{packageName}/{packageVersion} should not be installed in {project.Name}", visualStudio));
        }

        public static void AssertPackageNotInPackagesConfig(VisualStudioHost visualStudio, ProjectTestExtension project, string packageName, string packageVersion, ITestLogger logger)
        {
            logger.WriteMessage($"Checking project for {packageName}");
            var testService = visualStudio.Get<NuGetApexTestService>();

            // Check using the IVs APIs
            var exists = testService.IsPackageInstalled(project.UniqueName, packageName, packageVersion);
            logger.WriteMessage($"Exists: {exists}");

            exists.Should().BeFalse(AppendErrors($"{packageName}/{packageVersion} should NOT be in {project.Name}", visualStudio));
        }

        public static void AssertPackageNotInPackagesConfig(VisualStudioHost visualStudio, ProjectTestExtension project, string packageName, ITestLogger logger)
        {
            logger.WriteMessage($"Checking project for {packageName}");
            var testService = visualStudio.Get<NuGetApexTestService>();

            // Check using the IVs APIs
            var exists = testService.IsPackageInstalled(project.UniqueName, packageName);
            logger.WriteMessage($"Exists: {exists}");

            exists.Should().BeFalse(AppendErrors($"{packageName} should NOT be in {project.Name}", visualStudio));
        }

        public static void CreateConfigurationFile(string configurationPath, string configurationContent)
        {
            using (var file = File.Create(configurationPath))
            {
                var info = Encoding.UTF8.GetBytes(configurationContent);
                file.Write(info, 0, info.Count());
            }
        }

        public static void OpenNuGetPackageManagerWithDte(VisualStudioHost visualStudio, ITestLogger logger)
        {
            visualStudio.ObjectModel.Solution.WaitForOperationsInProgress(TimeSpan.FromMinutes(3));
            WaitForCommandAvailable(visualStudio, "Project.ManageNuGetPackages", TimeSpan.FromMinutes(1), logger);
            visualStudio.Dte.ExecuteCommand("Project.ManageNuGetPackages");
        }

        public static void RestoreNuGetPackages(VisualStudioHost visualStudio, ITestLogger logger)
        {
            visualStudio.ObjectModel.Solution.WaitForOperationsInProgress(TimeSpan.FromMinutes(3));
            WaitForCommandAvailable(visualStudio, "ProjectAndSolutionContextMenus.Solution.RestoreNuGetPackages", TimeSpan.FromMinutes(1), logger);
            visualStudio.Dte.ExecuteCommand("ProjectAndSolutionContextMenus.Solution.RestoreNuGetPackages");
        }

        private static void WaitForCommandAvailable(VisualStudioHost visualStudio, string commandName, TimeSpan timeout, ITestLogger logger)
        {
            WaitForCommandAvailable(visualStudio.Dte.Commands.Item(commandName), timeout, logger);
        }

        private static void WaitForCommandAvailable(Command cmd, TimeSpan timeout, ITestLogger logger)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            while (stopWatch.Elapsed < timeout)
            {
                if (cmd.IsAvailable)
                {
                    return;
                }
                Thread.Sleep(250);
            }

            logger.WriteWarning($"Timed out waiting for {cmd.Name} to be available");
        }

        private static bool IsPackageInstalledInAssetsFile(string assetsFilePath, string packageName, string packageVersion, bool expected)
        {
            return PackageExistsInLockFile(assetsFilePath, packageName, packageVersion, expected);
        }

        // return true if package exists, but retry logic is based on what value is expected so there is enough time for assets file to be updated.
        private static bool PackageExistsInLockFile(string pathToAssetsFile, string packageName, string packageVersion, bool expected)
        {
            var numAttempts = 0;
            LockFileLibrary lockFileLibrary = null;
            while (numAttempts++ < 3)
            {
                var version = NuGetVersion.Parse(packageVersion);
                var lockFile = GetAssetsFileWithRetry(pathToAssetsFile);
                lockFileLibrary = lockFile.Libraries
                    .SingleOrDefault(p => StringComparer.OrdinalIgnoreCase.Equals(p.Name, packageName)
                                        && p.Version.Equals(version));
                if (expected && lockFileLibrary != null)
                {
                    return true;
                }
                if (!expected && lockFileLibrary == null)
                {
                    return false;
                }

                Thread.Sleep(2000);
            }

            return lockFileLibrary != null;
        }

        private static LockFile GetAssetsFileWithRetry(string path)
        {
            var timeout = TimeSpan.FromSeconds(20);
            var timer = Stopwatch.StartNew();

            do
            {
                Thread.Sleep(100);
                if (File.Exists(path))
                {
                    try
                    {
                        var format = new LockFileFormat();
                        return format.Read(path);
                    }
                    catch
                    {
                        // Ignore errors from conflicting writes.
                    }
                }
            }
            while (timer.Elapsed < timeout);

            // File cannot be read
            if (File.Exists(path))
            {
                throw new InvalidOperationException("Unable to read: " + path);
            }
            else
            {
                throw new FileNotFoundException("Not found: " + path);
            }
        }

        private static string GetAssetsFilePath(string projectPath)
        {
            var projectDirectory = Path.GetDirectoryName(projectPath);
            return Path.Combine(projectDirectory, "obj", "project.assets.json");
        }

        public static FileInfo GetCacheFilePath(string projectPath)
        {
            var projectDirectory = Path.GetDirectoryName(projectPath);
            return new FileInfo(Path.Combine(projectDirectory, "obj", "project.nuget.cache"));
        }

        public static void WaitForFileExists(FileInfo file)
        {
            Omni.Common.WaitFor.IsTrue(
                () => File.Exists(file.FullName),
                Timeout,
                Interval,
                $"{file.FullName} did not exist within {Timeout}.");
        }

        public static void WaitForFileNotExists(FileInfo file)
        {
            Omni.Common.WaitFor.IsTrue(
                () => !File.Exists(file.FullName),
                Timeout,
                Interval,
                $"{file.FullName} still existed after {Timeout}.");
        }

        public static void WaitForDirectoryExists(string directoryPath)
        {
            Omni.Common.WaitFor.IsTrue(
                () => Directory.Exists(directoryPath),
                Timeout,
                Interval,
                $"{directoryPath} did not exist within {Timeout}.");
        }

        public static void WaitForDirectoryNotExists(string directoryPath)
        {
            Omni.Common.WaitFor.IsTrue(
                () => !Directory.Exists(directoryPath),
                Timeout,
                Interval,
                $"{directoryPath} still existed after {Timeout}.");
        }

        public static void UIInvoke(Action action)
        {
            var jtf = NuGetUIThreadHelper.JoinableTaskFactory;

            if (jtf != null)
            {
                jtf.Run(async () =>
                {
                    await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                    action();
                });
            }
            else
            {
                // Run directly
                action();
            }
        }

        internal static ProjectTestExtension CreateAndInitProject(ProjectTemplate projectTemplate, SimpleTestPathContext pathContext, SolutionService solutionService, ITestLogger logger)
        {
            logger.WriteMessage("Creating solution");
            solutionService.CreateEmptySolution("TestSolution", pathContext.SolutionRoot);

            logger.WriteMessage("Adding project");
            var project = solutionService.AddProject(ProjectLanguage.CSharp, projectTemplate, ProjectTargetFramework.V46, "TestProject");

            logger.WriteMessage("Saving solution");
            solutionService.Save();

            logger.WriteMessage("Building solution");
            project.Build();

            return project;
        }

        public static string AppendErrors(string s, VisualStudioHost visualStudio)
        {
            var errors = visualStudio.GetErrorsInOutputWindows();

            if (errors.Any())
            {
                s += Environment.NewLine + string.Join("\n\t error: ", errors);
            }

            return s;
        }

        public static void AssertInstalledPackageByProjectType(VisualStudioHost visualStudio, ProjectTemplate projectTemplate, ProjectTestExtension project, string packageName, string packageVersion, ITestLogger logger)
        {
            if (projectTemplate.Equals(ProjectTemplate.ClassLibrary))
            {
                AssertPackageInPackagesConfig(visualStudio, project, packageName, packageVersion, logger);
            }
            else
            {
                AssertPackageReferenceExists(visualStudio, project, packageName, packageVersion, logger);
            }
        }

        public static void AssertUninstalledPackageByProjectType(VisualStudioHost visualStudio, ProjectTemplate projectTemplate, ProjectTestExtension project, string packageName, ITestLogger logger)
        {
            if (projectTemplate.Equals(ProjectTemplate.ClassLibrary))
            {
                CommonUtility.AssertPackageNotInPackagesConfig(visualStudio, project, packageName, logger);
            }
            else
            {
                CommonUtility.AssertPackageReferenceDoesNotExist(visualStudio, project, packageName, logger);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Internal.NuGet.Testing.SignedPackages.ChildProcess;
using Microsoft.Test.Apex.VisualStudio.Shell;
using Microsoft.Test.Apex.VisualStudio.Solution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuGet.Common;
using NuGet.ProjectModel;
using NuGet.Test.Utility;

namespace NuGet.Tests.Apex
{
    [TestClass]
    public class NetCoreProjectTestCase : SharedVisualStudioHostTestClass
    {
        // basic create for .net core template
        [DataTestMethod]
        [DynamicData(nameof(GetNetCoreTemplates), DynamicDataSourceType.Method)]
        [Timeout(DefaultTimeout)]
        public void CreateNetCoreProject_RestoresNewProject(ProjectTemplate projectTemplate)
        {
            using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger, addNetStandardFeeds: true))
            {
                VisualStudio.AssertNoErrors();
            }
        }

        // basic create for .net core template
        [DataTestMethod]
        [DynamicData(nameof(GetNetCoreTemplates), DynamicDataSourceType.Method)]
        [Timeout(DefaultTimeout)]
        public void CreateNetCoreProject_AddProjectReference(ProjectTemplate projectTemplate)
        {
            using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger, addNetStandardFeeds: true))
            {
                var project2 = testContext.SolutionService.AddProject(ProjectLanguage.CSharp, projectTemplate, CommonUtility.DefaultTargetFramework, "TestProject2");
                project2.Build();

                testContext.Project.References.Dte.AddProjectReference(project2);
                testContext.SolutionService.SaveAll();

                testContext.SolutionService.Build();
                testContext.NuGetApexTestService.WaitForAutoRestore();

                VisualStudio.AssertNoErrors();
                CommonUtility.AssertPackageInAssetsFile(VisualStudio, testContext.Project, "TestProject2", "1.0.0", Logger);
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(GetNetCoreTemplates), DynamicDataSourceType.Method)]
        [Timeout(DefaultTimeout)]
        public async Task WithSourceMappingEnabled_InstallPackageFromPMUIFromExpectedSource_Succeeds(ProjectTemplate projectTemplate)
        {
            // Arrange
            using var simpleTestPathContext = new SimpleTestPathContext();
            var privateRepositoryPath = Path.Combine(simpleTestPathContext.SolutionRoot, "PrivateRepository");
            Directory.CreateDirectory(privateRepositoryPath);

            var packageName = "Contoso.a";
            var packageVersion = "1.0.0";

            await CommonUtility.CreatePackageInSourceAsync(simpleTestPathContext.PackageSource, packageName, packageVersion);
            await CommonUtility.CreatePackageInSourceAsync(privateRepositoryPath, packageName, packageVersion);

            // Configure Package source mapping filtering rules before project is created.
            simpleTestPathContext.Settings.AddSource("PrivateRepository", privateRepositoryPath);
            simpleTestPathContext.Settings.AddPackageSourceMapping("source", "External.*", "Others.*");
            simpleTestPathContext.Settings.AddPackageSourceMapping("PrivateRepository", "contoso.*", "Test.*");
            simpleTestPathContext.Settings.AddPackageSourceMapping("nuget", "Microsoft.*", "NetStandard*");

            using var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger, addNetStandardFeeds: true, simpleTestPathContext: simpleTestPathContext);

            VisualStudio.AssertNoErrors();

            // Act
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, Logger);
            var nugetTestService = GetNuGetTestService();
            var uiwindow = nugetTestService.GetUIWindowfromProject(testContext.SolutionService.Projects[0]);

            // The Install action will automatically create a package source mapping to the selected package source if it's missing,
            // so select the source which already has a mapping.
            uiwindow.SetPackageSourceOptionToSource("PrivateRepository");
            uiwindow.InstallPackageFromUI(packageName, packageVersion);

            // Assert
            VisualStudio.AssertNuGetOutputDoesNotHaveErrors();
            CommonUtility.AssertPackageReferenceExists(testContext.SolutionService.Projects[0], packageName, packageVersion, Logger);
            StringAssert.Contains(GetPackageManagerOutputWindowPaneText(), $"Installed {packageName} {packageVersion} from {privateRepositoryPath}");
        }

        [DataTestMethod]
        [DynamicData(nameof(GetNetCoreTemplates), DynamicDataSourceType.Method)]
        [Timeout(DefaultTimeout)]
        public async Task WithSourceMappingEnabled_InstallAndUpdatePackageFromPMUIFromExpectedSource_Succeeds(ProjectTemplate projectTemplate)
        {
            // Arrange
            using var simpleTestPathContext = new SimpleTestPathContext();
            var privateRepositoryPath = Path.Combine(simpleTestPathContext.SolutionRoot, "PrivateRepository");
            Directory.CreateDirectory(privateRepositoryPath);

            var packageName = "Contoso.a";
            var packageVersion1 = "1.0.0";
            var packageVersion2 = "2.0.0";

            await CommonUtility.CreatePackageInSourceAsync(simpleTestPathContext.PackageSource, packageName, packageVersion1);
            await CommonUtility.CreatePackageInSourceAsync(simpleTestPathContext.PackageSource, packageName, packageVersion2);
            await CommonUtility.CreatePackageInSourceAsync(privateRepositoryPath, packageName, packageVersion1);
            await CommonUtility.CreatePackageInSourceAsync(privateRepositoryPath, packageName, packageVersion2);

            // Configure Package source mapping filtering rules before project is created.
            simpleTestPathContext.Settings.AddSource("PrivateRepository", privateRepositoryPath);
            simpleTestPathContext.Settings.AddPackageSourceMapping("source", "External.*", "Others.*");
            simpleTestPathContext.Settings.AddPackageSourceMapping("PrivateRepository", "contoso.*", "Test.*");
            simpleTestPathContext.Settings.AddPackageSourceMapping("nuget", "Microsoft.*", "NetStandard*");

            using var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger, addNetStandardFeeds: true, simpleTestPathContext: simpleTestPathContext);

            VisualStudio.AssertNoErrors();

            // Install v1 (arrange for update test)
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, Logger);
            var nugetTestService = GetNuGetTestService();
            var uiwindow = nugetTestService.GetUIWindowfromProject(testContext.SolutionService.Projects[0]);
            // The Install action will automatically create a package source mapping to the selected package source if it's missing,
            // so select the source which already has a mapping.
            uiwindow.SetPackageSourceOptionToSource("PrivateRepository");
            uiwindow.InstallPackageFromUI(packageName, packageVersion1);
            testContext.SolutionService.SaveAll();
            VisualStudio.AssertNuGetOutputDoesNotHaveErrors();
            VisualStudio.ClearWindows();

            // Act
            uiwindow.UpdatePackageFromUI(packageName, packageVersion2);

            // Assert
            VisualStudio.AssertNuGetOutputDoesNotHaveErrors();
            CommonUtility.AssertPackageReferenceExists(testContext.SolutionService.Projects[0], packageName, packageVersion2, Logger);
            StringAssert.Contains(GetPackageManagerOutputWindowPaneText(), $"Installed {packageName} {packageVersion2} from {privateRepositoryPath}");
        }

        [DataTestMethod]
        [DynamicData(nameof(GetNetCoreTemplates), DynamicDataSourceType.Method)]
        [Timeout(DefaultTimeout)]
        public async Task WithSourceMappingEnabled_InstallPackageFromPMUIAndNoSourcesFound_Fails(ProjectTemplate projectTemplate)
        {
            // Arrange
            using var simpleTestPathContext = new SimpleTestPathContext();
            var privateRepositoryPath = Path.Combine(simpleTestPathContext.SolutionRoot, "PrivateRepository");
            Directory.CreateDirectory(privateRepositoryPath);

            var packageName = "Contoso.a";
            var packageVersion = "1.0.0";

            // Package only exists in the external (default) source, not in PrivateRepository.
            await CommonUtility.CreatePackageInSourceAsync(simpleTestPathContext.PackageSource, packageName, packageVersion);

            // Configure Package source mapping filtering rules before project is created.
            simpleTestPathContext.Settings.AddSource("PrivateRepository", privateRepositoryPath);
            simpleTestPathContext.Settings.AddPackageSourceMapping("source", "External.*", "Others.*");
            simpleTestPathContext.Settings.AddPackageSourceMapping("PrivateRepository", "contoso.*", "Test.*");
            simpleTestPathContext.Settings.AddPackageSourceMapping("nuget", "Microsoft.*", "NetStandard*");

            using var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger, addNetStandardFeeds: true, simpleTestPathContext: simpleTestPathContext);

            VisualStudio.AssertNoErrors();

            // Act
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, Logger);
            var nugetTestService = GetNuGetTestService();
            var uiwindow = nugetTestService.GetUIWindowfromProject(testContext.SolutionService.Projects[0]);

            // The Install action will automatically create a package source mapping to the selected package source if it's missing,
            // so select the source which already has a mapping.
            uiwindow.SetPackageSourceOptionToSource("PrivateRepository");
            uiwindow.InstallPackageFromUI(packageName, packageVersion);

            // Assert
            CommonUtility.AssertPackageReferenceDoesNotExist(testContext.SolutionService.Projects[0], packageName, packageVersion, Logger);
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        [TestCategory("StagingGate")]
        [TestCategory("CanaryGate")]
        public async Task InstallAndUpdatePackageFromUI_NetCoreProject_Succeeds()
        {
            // Arrange
            using var suppressNuGetUI = NuGetUISuppression.Suppress();
            using var testContext = new ApexTestContext(VisualStudio, ProjectTemplate.NetCoreConsoleApp, Logger);
            var packageName = "NetCoreUpdateTestPackage";
            var packageVersion1 = "1.0.0";
            var packageVersion2 = "2.0.0";

            await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, packageName, packageVersion1);
            await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, packageName, packageVersion2);

            VisualStudio.AssertNoErrors();

            // Act
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, Logger);
            var nugetTestService = GetNuGetTestService();
            var uiwindow = nugetTestService.GetUIWindowfromProject(testContext.Project);
            uiwindow.InstallPackageFromUI(packageName, packageVersion1);
            testContext.SolutionService.Build();
            testContext.NuGetApexTestService.WaitForAutoRestore();
            CommonUtility.AssertPackageReferenceExists(testContext.Project, packageName, packageVersion1, Logger);

            uiwindow.UpdatePackageFromUI(packageName, packageVersion2);
            testContext.SolutionService.Build();
            testContext.NuGetApexTestService.WaitForAutoRestore();

            // Assert
            VisualStudio.AssertNuGetOutputDoesNotHaveErrors();
            CommonUtility.AssertPackageReferenceExists(testContext.Project, packageName, packageVersion2, Logger);
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public async Task RestoreWpfProjectWithUnversionedPackageReference_ReportsNU1604Async()
        {
            // Arrange
            string? msbuildPath = VisualStudioMSBuildLocator.GetMSBuildPath();
            if (msbuildPath is null)
            {
                Assert.Inconclusive(
                    "Could not locate MSBuild from the Visual Studio installation under test using vswhere. " +
                    "Ensure a Visual Studio instance with MSBuild is installed.");
                return;
            }

            using var simpleTestPathContext = new SimpleTestPathContext();
            const string PackageName = "TestUpdatePackage";
            const string PackageVersion = "1.0.0";
            const string ExpectedWarning =
                "Project dependency TestUpdatePackage does not contain an inclusive lower bound. " +
                "Include a lower bound in the dependency version to ensure consistent restore results.";

            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(
                simpleTestPathContext.PackageSource,
                PackageName,
                PackageVersion);

            var directoryBuildPropsPath = Path.Combine(simpleTestPathContext.SolutionRoot, "Directory.Build.props");
            File.WriteAllText(
                directoryBuildPropsPath,
                $"""
                <Project>
                  <PropertyGroup>
                    <SdkAnalysisLevel>9.0.100</SdkAnalysisLevel>
                  </PropertyGroup>
                  <ItemGroup>
                    <PackageReference Include="{PackageName}" />
                  </ItemGroup>
                </Project>
                """);

            using var testContext = new ApexTestContext(
                VisualStudio,
                ProjectTemplate.WPFApplication,
                Logger,
                simpleTestPathContext: simpleTestPathContext);

            var errorListService = VisualStudio.Get<ErrorListService>();
            errorListService.ShowWarnings();

            // Act
            var assetsFilePath = CommonUtility.GetAssetsFilePath(testContext.Project.FullPath);
            File.Delete(assetsFilePath);
            CommonUtility.RestoreNuGetPackages(VisualStudio, Logger);
            testContext.NuGetApexTestService.WaitForAutoRestore();

            // Assert
            Omni.Common.WaitFor.IsTrue(
                () => VisualStudio.ObjectModel.Shell.ToolWindows.ErrorList.AllItems
                    .Any(item => item.Description == ExpectedWarning),
                TimeSpan.FromMinutes(1),
                TimeSpan.FromMilliseconds(500),
                $"Expected the Error List to contain '{ExpectedWarning}'.");

            AssertAssetsFileContainsWarning(assetsFilePath, NuGetLogCode.NU1604, "Visual Studio restore");

            File.Delete(assetsFilePath);
            CommandRunnerResult result = CommandRunner.Run(
                filename: msbuildPath,
                workingDirectory: simpleTestPathContext.SolutionRoot,
                arguments: $"-t:restore \"{testContext.Project.FullPath}\"",
                timeOutInMilliseconds: DefaultTimeout);
            Assert.AreEqual(0, result.ExitCode, result.AllOutput);
            Assert.AreEqual(0, result.ExitCode, result.AllOutput);
            AssertAssetsFileContainsWarning(assetsFilePath, NuGetLogCode.NU1604, result.AllOutput);
        }

        private static void AssertAssetsFileContainsWarning(
            string assetsFilePath,
            NuGetLogCode warningCode,
            string diagnosticOutput)
        {
            CommonUtility.WaitForFileExists(new FileInfo(assetsFilePath));

            TestLogger logger = new();
            LockFile? assetsFile = new LockFileFormat().Read(assetsFilePath, logger);
            Assert.AreEqual(0, logger.Messages.Count, string.Join(Environment.NewLine, logger.Messages));
            Assert.IsNotNull(
                assetsFile,
                $"Expected restore to generate a valid assets file at '{assetsFilePath}'.{Environment.NewLine}{diagnosticOutput}");
            Assert.IsTrue(
                assetsFile.LogMessages.Any(message => message.Code == warningCode),
                $"Expected assets file '{assetsFilePath}' to contain {warningCode}.{Environment.NewLine}{diagnosticOutput}");
        }

        // There  is a bug with VS or Apex where NetCoreConsoleApp and NetCoreClassLib create netcore 2.1 projects that are not supported by the sdk
        // Commenting out any NetCoreConsoleApp or NetCoreClassLib template and swapping it for NetStandardClassLib as both are package ref.

        public static IEnumerable<object[]> GetNetCoreTemplates()
        {
            yield return new object[] { ProjectTemplate.NetStandardClassLib };
        }
    }
}

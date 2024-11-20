using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Test.Apex.VisualStudio.Solution;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
                var project2 = testContext.SolutionService.AddProject(ProjectLanguage.CSharp, projectTemplate, ProjectTargetFramework.V46, "TestProject2");
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
            using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger, addNetStandardFeeds: true))
            {
                var privateRepositoryPath = Path.Combine(testContext.SolutionRoot, "PrivateRepository");
                Directory.CreateDirectory(privateRepositoryPath);
                var externalRepositoryPath = Path.Combine(testContext.SolutionRoot, "ExternalRepository");
                Directory.CreateDirectory(externalRepositoryPath);

                var packageName = "Contoso.a";
                var packageVersion = "1.0.0";

                await CommonUtility.CreatePackageInSourceAsync(privateRepositoryPath, packageName, packageVersion);
                await CommonUtility.CreatePackageInSourceAsync(externalRepositoryPath, packageName, packageVersion);


                // Create nuget.config with Package source mapping filtering rules before project is created.
                CommonUtility.CreateConfigurationFile(Path.Combine(testContext.SolutionRoot, "NuGet.Config"), $@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
        <add key=""ExternalRepository"" value=""{externalRepositoryPath}"" />
        <add key=""PrivateRepository"" value=""{privateRepositoryPath}"" />
    </packageSources>
    <packageSourceMapping>
        <packageSource key=""externalRepository"">
            <package pattern=""External.*"" />
            <package pattern=""Others.*"" />
        </packageSource>
        <packageSource key=""PrivateRepository"">
            <package pattern=""contoso.*"" />
            <package pattern=""Test.*"" />
        </packageSource>
        <packageSource key=""nuget"">
            <package pattern=""Microsoft.*"" />
            <package pattern=""NetStandard*"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");


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
                CommonUtility.AssertPackageReferenceExists(VisualStudio, testContext.SolutionService.Projects[0], packageName, packageVersion, Logger);
                StringAssert.Contains(GetPackageManagerOutputWindowPaneText(), $"Installed {packageName} {packageVersion} from {privateRepositoryPath}");
            }
        }

        [Ignore("https://github.com/NuGet/Home/issues/12898")]
        [DataTestMethod]
        [DynamicData(nameof(GetNetCoreTemplates), DynamicDataSourceType.Method)]
        [Timeout(DefaultTimeout)]
        public async Task WithSourceMappingEnabled_InstallAndUpdatePackageFromPMUIFromExpectedSource_Succeeds(ProjectTemplate projectTemplate)
        {
            // Arrange
            using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger, addNetStandardFeeds: true))
            {
                var privateRepositoryPath = Path.Combine(testContext.SolutionRoot, "PrivateRepository");
                Directory.CreateDirectory(privateRepositoryPath);
                var externalRepositoryPath = Path.Combine(testContext.SolutionRoot, "ExternalRepository");
                Directory.CreateDirectory(externalRepositoryPath);

                var packageName = "Contoso.a";
                var packageVersion1 = "1.0.0";
                var packageVersion2 = "2.0.0";

                await CommonUtility.CreatePackageInSourceAsync(privateRepositoryPath, packageName, packageVersion1);
                await CommonUtility.CreatePackageInSourceAsync(externalRepositoryPath, packageName, packageVersion1);

                await CommonUtility.CreatePackageInSourceAsync(privateRepositoryPath, packageName, packageVersion2);
                await CommonUtility.CreatePackageInSourceAsync(externalRepositoryPath, packageName, packageVersion2);

                // Create nuget.config with Package source mapping filtering rules before project is created.
                CommonUtility.CreateConfigurationFile(Path.Combine(testContext.SolutionRoot, "NuGet.Config"), $@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
        <add key=""ExternalRepository"" value=""{externalRepositoryPath}"" />
        <add key=""PrivateRepository"" value=""{privateRepositoryPath}"" />
    </packageSources>
    <packageSourceMapping>
        <packageSource key=""externalRepository"">
            <package pattern=""External.*"" />
            <package pattern=""Others.*"" />
        </packageSource>
        <packageSource key=""PrivateRepository"">
            <package pattern=""contoso.*"" />
            <package pattern=""Test.*"" />
        </packageSource>
        <packageSource key=""nuget"">
            <package pattern=""Microsoft.*"" />
            <package pattern=""NetStandard*"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");


                VisualStudio.AssertNoErrors();

                // Arrange
                CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, Logger);
                var nugetTestService = GetNuGetTestService();
                var uiwindow = nugetTestService.GetUIWindowfromProject(testContext.SolutionService.Projects[0]);
                uiwindow.InstallPackageFromUI(packageName, packageVersion1);
                testContext.SolutionService.SaveAll();
                VisualStudio.AssertNuGetOutputDoesNotHaveErrors();
                VisualStudio.ClearWindows();

                // Act
                uiwindow.UpdatePackageFromUI(packageName, packageVersion2);

                // Assert
                VisualStudio.AssertNuGetOutputDoesNotHaveErrors();
                CommonUtility.AssertPackageReferenceExists(VisualStudio, testContext.SolutionService.Projects[0], packageName, packageVersion2, Logger);
                StringAssert.Contains(GetPackageManagerOutputWindowPaneText(), $"Installed {packageName} {packageVersion2} from {privateRepositoryPath}");
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(GetNetCoreTemplates), DynamicDataSourceType.Method)]
        [Timeout(DefaultTimeout)]
        public async Task WithSourceMappingEnabled_InstallPackageFromPMUIAndNoSourcesFound_Fails(ProjectTemplate projectTemplate)
        {
            // Arrange
            using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger, addNetStandardFeeds: true))
            {
                var privateRepositoryPath = Path.Combine(testContext.SolutionRoot, "PrivateRepository");
                Directory.CreateDirectory(privateRepositoryPath);
                var externalRepositoryPath = Path.Combine(testContext.SolutionRoot, "ExternalRepository");
                Directory.CreateDirectory(externalRepositoryPath);

                var packageName = "Contoso.a";
                var packageVersion = "1.0.0";

                await CommonUtility.CreatePackageInSourceAsync(externalRepositoryPath, packageName, packageVersion);

                // Create nuget.config with Package source mapping filtering rules before project is created.
                CommonUtility.CreateConfigurationFile(Path.Combine(testContext.SolutionRoot, "NuGet.Config"), $@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
        <add key=""ExternalRepository"" value=""{externalRepositoryPath}"" />
        <add key=""PrivateRepository"" value=""{privateRepositoryPath}"" />
    </packageSources>
    <packageSourceMapping>
        <packageSource key=""externalRepository"">
            <package pattern=""External.*"" />
            <package pattern=""Others.*"" />
        </packageSource>
        <packageSource key=""PrivateRepository"">
            <package pattern=""contoso.*"" />
            <package pattern=""Test.*"" />
        </packageSource>
        <packageSource key=""nuget"">
            <package pattern=""Microsoft.*"" />
            <package pattern=""NetStandard*"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");

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
                CommonUtility.AssertPackageReferenceDoesNotExist(VisualStudio, testContext.SolutionService.Projects[0], packageName, packageVersion, Logger);
            }
        }

        // There  is a bug with VS or Apex where NetCoreConsoleApp and NetCoreClassLib create netcore 2.1 projects that are not supported by the sdk
        // Commenting out any NetCoreConsoleApp or NetCoreClassLib template and swapping it for NetStandardClassLib as both are package ref.

        public static IEnumerable<object[]> GetNetCoreTemplates()
        {
            yield return new object[] { ProjectTemplate.NetStandardClassLib };
        }
    }
}

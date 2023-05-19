using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Test.Apex.VisualStudio.Solution;
using NuGet.StaFact;
using NuGet.Test.Utility;
using Xunit;
using Xunit.Abstractions;

namespace NuGet.Tests.Apex
{
    public class NetCoreProjectTestCase : SharedVisualStudioHostTestClass, IClassFixture<VisualStudioHostFixtureFactory>
    {
        public NetCoreProjectTestCase(VisualStudioHostFixtureFactory visualStudioHostFixtureFactory, ITestOutputHelper output)
            : base(visualStudioHostFixtureFactory, output)
        {
        }

        // basic create for .net core template
        [NuGetWpfTheory(Skip = "https://github.com/NuGet/Home/issues/11308")]
        [MemberData(nameof(GetNetCoreTemplates))]
        public void CreateNetCoreProject_RestoresNewProject(ProjectTemplate projectTemplate)
        {
            // Arrange
            EnsureVisualStudioHost();

            using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, XunitLogger, addNetStandardFeeds: true))
            {
                VisualStudio.AssertNoErrors();
            }
        }

        // basic create for .net core template
        [NuGetWpfTheory(Skip = "https://github.com/NuGet/Home/issues/9410")]
        [MemberData(nameof(GetNetCoreTemplates))]
        public void CreateNetCoreProject_AddProjectReference(ProjectTemplate projectTemplate)
        {
            // Arrange
            EnsureVisualStudioHost();

            using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, XunitLogger, addNetStandardFeeds: true))
            {
                var project2 = testContext.SolutionService.AddProject(ProjectLanguage.CSharp, projectTemplate, ProjectTargetFramework.V46, "TestProject2");
                project2.Build();

                testContext.Project.References.Dte.AddProjectReference(project2);
                testContext.SolutionService.SaveAll();

                testContext.SolutionService.Build();
                testContext.NuGetApexTestService.WaitForAutoRestore();

                VisualStudio.AssertNoErrors();
                CommonUtility.AssertPackageInAssetsFile(VisualStudio, testContext.Project, "TestProject2", "1.0.0", XunitLogger);
            }
        }

        [NuGetWpfTheory]
        [MemberData(nameof(GetNetCoreTemplates))]
        public async Task WithSourceMappingEnabled_InstallPackageFromPMUIFromExpectedSource_Succeeds(ProjectTemplate projectTemplate)
        {
            // Arrange
            EnsureVisualStudioHost();

            using (var simpleTestPathContext = new SimpleTestPathContext())
            {
                string solutionDirectory = simpleTestPathContext.SolutionRoot;
                var privateRepositoryPath = Path.Combine(solutionDirectory, "PrivateRepository");
                Directory.CreateDirectory(privateRepositoryPath);
                var externalRepositoryPath = Path.Combine(solutionDirectory, "ExternalRepository");
                Directory.CreateDirectory(externalRepositoryPath);

                var packageName = "Contoso.a";
                var packageVersion = "1.0.0";

                await CommonUtility.CreatePackageInSourceAsync(privateRepositoryPath, packageName, packageVersion);
                await CommonUtility.CreatePackageInSourceAsync(externalRepositoryPath, packageName, packageVersion);


                // Create nuget.config with Package source mapping filtering rules before project is created.
                CommonUtility.CreateConfigurationFile(Path.Combine(solutionDirectory, "NuGet.Config"), $@"<?xml version=""1.0"" encoding=""utf-8""?>
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

                using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, XunitLogger, addNetStandardFeeds: true, simpleTestPathContext: simpleTestPathContext))
                {
                    VisualStudio.AssertNoErrors();

                    // Act
                    CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, XunitLogger);
                    var nugetTestService = GetNuGetTestService();
                    var uiwindow = nugetTestService.GetUIWindowfromProject(testContext.SolutionService.Projects[0]);
                    uiwindow.InstallPackageFromUI(packageName, packageVersion);

                    // Assert
                    VisualStudio.AssertNuGetOutputDoesNotHaveErrors();
                    CommonUtility.AssertPackageReferenceExists(VisualStudio, testContext.SolutionService.Projects[0], packageName, packageVersion, XunitLogger);
                    Assert.Contains($"Installed {packageName} {packageVersion} from {privateRepositoryPath}", GetPackageManagerOutputWindowPaneText());
                }
            }
        }

        [NuGetWpfTheory(Skip = "https://github.com/NuGet/Home/issues/11308")]
        [MemberData(nameof(GetNetCoreTemplates))]
        public async Task WithSourceMappingEnabled_InstallAndUpdatePackageFromPMUIFromExpectedSource_Succeeds(ProjectTemplate projectTemplate)
        {
            // Arrange
            EnsureVisualStudioHost();

            using (var simpleTestPathContext = new SimpleTestPathContext())
            {
                string solutionDirectory = simpleTestPathContext.SolutionRoot;
                var privateRepositoryPath = Path.Combine(solutionDirectory, "PrivateRepository");
                Directory.CreateDirectory(privateRepositoryPath);
                var externalRepositoryPath = Path.Combine(solutionDirectory, "ExternalRepository");
                Directory.CreateDirectory(externalRepositoryPath);

                var packageName = "Contoso.a";
                var packageVersion1 = "1.0.0";
                var packageVersion2 = "2.0.0";

                await CommonUtility.CreatePackageInSourceAsync(privateRepositoryPath, packageName, packageVersion1);
                await CommonUtility.CreatePackageInSourceAsync(externalRepositoryPath, packageName, packageVersion1);

                await CommonUtility.CreatePackageInSourceAsync(privateRepositoryPath, packageName, packageVersion2);
                await CommonUtility.CreatePackageInSourceAsync(externalRepositoryPath, packageName, packageVersion2);

                // Create nuget.config with Package source mapping filtering rules before project is created.
                CommonUtility.CreateConfigurationFile(Path.Combine(solutionDirectory, "NuGet.Config"), $@"<?xml version=""1.0"" encoding=""utf-8""?>
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

                using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, XunitLogger, addNetStandardFeeds: true, simpleTestPathContext: simpleTestPathContext))
                {
                    VisualStudio.AssertNoErrors();

                    // Arrange
                    CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, XunitLogger);
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
                    CommonUtility.AssertPackageReferenceExists(VisualStudio, testContext.SolutionService.Projects[0], packageName, packageVersion2, XunitLogger);
                    Assert.Contains($"Installed {packageName} {packageVersion2} from {privateRepositoryPath}", GetPackageManagerOutputWindowPaneText());
                }
            }
        }

        [NuGetWpfTheory]
        [MemberData(nameof(GetNetCoreTemplates))]
        public async Task WithSourceMappingEnabled_InstallPackageFromPMUIAndNoSourcesFound_Fails(ProjectTemplate projectTemplate)
        {
            // Arrange
            EnsureVisualStudioHost();

            using (var simpleTestPathContext = new SimpleTestPathContext())
            {
                string solutionDirectory = simpleTestPathContext.SolutionRoot;
                var privateRepositoryPath = Path.Combine(solutionDirectory, "PrivateRepository");
                Directory.CreateDirectory(privateRepositoryPath);
                var externalRepositoryPath = Path.Combine(solutionDirectory, "ExternalRepository");
                Directory.CreateDirectory(externalRepositoryPath);

                var packageName = "Contoso.a";
                var packageVersion = "1.0.0";

                await CommonUtility.CreatePackageInSourceAsync(externalRepositoryPath, packageName, packageVersion);

                // Create nuget.config with Package source mapping filtering rules before project is created.
                CommonUtility.CreateConfigurationFile(Path.Combine(solutionDirectory, "NuGet.Config"), $@"<?xml version=""1.0"" encoding=""utf-8""?>
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

                using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, XunitLogger, addNetStandardFeeds: true, simpleTestPathContext: simpleTestPathContext))
                {
                    VisualStudio.AssertNoErrors();

                    // Act
                    CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, XunitLogger);
                    var nugetTestService = GetNuGetTestService();
                    var uiwindow = nugetTestService.GetUIWindowfromProject(testContext.SolutionService.Projects[0]);
                    uiwindow.InstallPackageFromUI(packageName, packageVersion);

                    // Assert                    
                    CommonUtility.AssertPackageReferenceDoesNotExist(VisualStudio, testContext.SolutionService.Projects[0], packageName, packageVersion, XunitLogger);
                }
            }
        }

        [NuGetWpfTheory]
        [MemberData(nameof(GetNetCoreTemplates))]
        public async Task InstallPackageToNetCoreProjectFromUI(ProjectTemplate projectTemplate)
        {
            EnsureVisualStudioHost();

            using (var simpleTestPathContext = new SimpleTestPathContext())
            {
                // Arrange
                var packageName = "NetCoreInstallTestPackage";
                var packageVersion = "1.0.0";
                await CommonUtility.CreatePackageInSourceAsync(simpleTestPathContext.PackageSource, packageName, packageVersion);

                using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, XunitLogger, addNetStandardFeeds: true, simpleTestPathContext: simpleTestPathContext))
                {
                    VisualStudio.AssertNoErrors();

                    // Act
                    CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, XunitLogger);
                    var nugetTestService = GetNuGetTestService();
                    var uiwindow = nugetTestService.GetUIWindowfromProject(testContext.Project);
                    uiwindow.InstallPackageFromUI(packageName, packageVersion);

                    // Assert
                    VisualStudio.AssertNuGetOutputDoesNotHaveErrors();
                    CommonUtility.AssertPackageReferenceExists(VisualStudio, testContext.Project, packageName, packageVersion, XunitLogger);
                }
            }
        }

        [NuGetWpfTheory]
        [MemberData(nameof(GetNetCoreTemplates))]
        public async Task UpdatePackageToNetCoreProjectFromUI(ProjectTemplate projectTemplate)
        {
            EnsureVisualStudioHost();

            using (var simpleTestPathContext = new SimpleTestPathContext())
            {
                // Arrange
                var packageName = "NetCoreUpdateTestPackage";
                var packageVersion1 = "1.0.0";
                var packageVersion2 = "2.0.0";

                await CommonUtility.CreatePackageInSourceAsync(simpleTestPathContext.PackageSource, packageName, packageVersion1);
                await CommonUtility.CreatePackageInSourceAsync(simpleTestPathContext.PackageSource, packageName, packageVersion2);

                using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, XunitLogger, addNetStandardFeeds: true, simpleTestPathContext: simpleTestPathContext))
                {
                    VisualStudio.AssertNoErrors();

                    // Act
                    CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, XunitLogger);
                    var nugetTestService = GetNuGetTestService();
                    var uiwindow = nugetTestService.GetUIWindowfromProject(testContext.Project);
                    uiwindow.InstallPackageFromUI(packageName, packageVersion1);
                    testContext.SolutionService.Build();
                    testContext.NuGetApexTestService.WaitForAutoRestore();

                    uiwindow.UpdatePackageFromUI(packageName, packageVersion2);
                    testContext.SolutionService.Build();
                    testContext.NuGetApexTestService.WaitForAutoRestore();

                    // Assert
                    VisualStudio.AssertNuGetOutputDoesNotHaveErrors();
                    CommonUtility.AssertPackageReferenceExists(VisualStudio, testContext.Project, packageName, packageVersion2, XunitLogger);
                }
            }
        }

        [NuGetWpfTheory]
        [MemberData(nameof(GetNetCoreTemplates))]
        public async Task UninstallPackageFromNetCoreProjectFromUI(ProjectTemplate projectTemplate)
        {
            EnsureVisualStudioHost();

            using (var simpleTestPathContext = new SimpleTestPathContext())
            {
                // Arrange
                var packageName = "NetCoreUninstallTestPackage";
                var packageVersion = "1.0.0";

                await CommonUtility.CreatePackageInSourceAsync(simpleTestPathContext.PackageSource, packageName, packageVersion);

                using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, XunitLogger, addNetStandardFeeds: true, simpleTestPathContext: simpleTestPathContext))
                {
                    VisualStudio.AssertNoErrors();

                    // Act
                    CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, XunitLogger);
                    var nugetTestService = GetNuGetTestService();
                    var uiwindow = nugetTestService.GetUIWindowfromProject(testContext.Project);
                    uiwindow.InstallPackageFromUI(packageName, packageVersion);
                    testContext.SolutionService.Build();
                    testContext.NuGetApexTestService.WaitForAutoRestore();

                    uiwindow.UninstallPackageFromUI(packageName);
                    testContext.SolutionService.Build();
                    testContext.NuGetApexTestService.WaitForAutoRestore();

                    // Assert
                    VisualStudio.AssertNuGetOutputDoesNotHaveErrors();
                    CommonUtility.AssertPackageReferenceDoesNotExist(VisualStudio, testContext.Project, packageName, XunitLogger);
                }
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

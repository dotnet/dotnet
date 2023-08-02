using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Microsoft.Test.Apex;
using Microsoft.Test.Apex.Hosts.Services;
using Microsoft.Test.Apex.VisualStudio;
using Microsoft.Test.Apex.VisualStudio.Shell.ToolWindows;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NuGet
{
    [TestClass]
    public class OptProfV2Tests : ApexTest
    {
        private static readonly Guid _packageManagerOutputWindowPaneGuid = Guid.Parse("CEC55EC8-CC51-40E7-9243-57B87A6F6BEB");
        private const string _testCategory = "OptProf";
        private const int _timeoutInMilliseconds = 15 * 60 * 1000; // 15 minutes

        // Must be lazy because TestContext.DeploymentDirectory only returns a valid value within a test method.
        private readonly Lazy<DirectoryInfo> _lazyAssetsDirectory;

        public OptProfV2Tests()
        {
            _lazyAssetsDirectory = new Lazy<DirectoryInfo>(() => new DirectoryInfo(Path.Combine(TestContext.DeploymentDirectory, "Assets")));
        }

        [TestMethod]
        [TestCategory(_testCategory)]
        [Timeout(_timeoutInMilliseconds)]
        [DeploymentItem(@"Assets\PackageReferenceSdk", @"Assets\PackageReferenceSdk")]
        public void OpenSolutionAndBuild_PackageReferenceSdk()
        {
            FileInfo solutionFile = GetSolutionFile("PackageReferenceSdk");

            OpenSolutionAndBuild(solutionFile);
        }

        [TestMethod]
        [TestCategory(_testCategory)]
        [Timeout(_timeoutInMilliseconds)]
        [DeploymentItem(@"Assets\PackageReferenceSdk", @"Assets\PackageReferenceSdk")]
        public void IVsPackageSourceProvider_GetSources()
        {
            FileInfo solutionFile = GetSolutionFile("PackageReferenceSdk");

            UseService<TestIVsPackageSourceProvider>(solutionFile, service =>
            {
                IEnumerable<KeyValuePair<string, string>> sources = service.GetSources(includeUnOfficial: true, includeDisabled: true);

                Assert.AreNotEqual(0, sources.Count());
            });
        }

        private void BuildSolution(VisualStudioHost visualStudio)
        {
            using (Scope.Enter("Build solution."))
            {
                // Rebuild to ensure a clean build.
                visualStudio.ObjectModel.Solution.BuildManager.Rebuild();
                visualStudio.ObjectModel.Solution.BuildManager.Verify.Succeeded();
            }
        }

        private VisualStudioHostConfiguration CreateVisualStudioHostConfiguration()
        {
            return new VisualStudioHostConfiguration()
            {
                RestoreUserSettings = false,
                // Required in order to allow modules mapped to the test to generate IBC files.
                InheritProcessEnvironment = true
            };
        }

        private FileInfo GetSolutionFile(string solutionName)
        {
            var solutionFile = new FileInfo(Path.Combine(_lazyAssetsDirectory.Value.FullName, solutionName, $"{solutionName}.sln"));

            Verify.IsTrue(solutionFile.Exists, $"Unable to find solution file {solutionFile.FullName}");

            return solutionFile;
        }

        private void HandleException(VisualStudioHost visualStudio, Exception ex)
        {
            using (Scope.Enter("Handle exception."))
            {
                if (visualStudio != null && visualStudio.IsRunning)
                {
                    visualStudio.CaptureHostProcessDumpIfRunning(MiniDumpType.WithFullMemory);
                    visualStudio.HostProcess.Kill();
                }

                Logger.WriteException(EntryType.Error, ex);
                Verify.Fail("Exception encountered during test execution");
                Assert.Fail(ex.Message);
            }
        }

        private VisualStudioHost LaunchVisualStudio(VisualStudioHostConfiguration configuration)
        {
            VisualStudioHost visualStudio = Operations.CreateHost<VisualStudioHost>(configuration);

            using (Scope.Enter("Launch Visual Studio."))
            {
                visualStudio.Start();
            }

            return visualStudio;
        }

        private void LoadSolution(VisualStudioHost visualStudio, FileInfo solutionFile)
        {
            using (Scope.Enter("Load solution."))
            {
                visualStudio.ObjectModel.Solution.WaitForFullyLoadedOnOpen = true;
                visualStudio.ObjectModel.Solution.Open(solutionFile.FullName);
                visualStudio.ObjectModel.Solution.Verify.HasProject();
            }
        }

        private void OpenSolutionAndBuild(FileInfo solutionFile)
        {
            VisualStudioHost visualStudio = null;

            try
            {
                VisualStudioHostConfiguration configuration = CreateVisualStudioHostConfiguration();

                visualStudio = LaunchVisualStudio(configuration);

                LoadSolution(visualStudio, solutionFile);
                WaitForAutoRestoreToComplete(visualStudio, solutionFile);
                BuildSolution(visualStudio);
                ShutDownVisualStudio(visualStudio);
            }
            catch (Exception ex)
            {
                HandleException(visualStudio, ex);
            }
        }

        private void ShutDownVisualStudio(VisualStudioHost visualStudio)
        {
            using (Scope.Enter("Close solution and shut down Visual Studio."))
            {
                visualStudio.ObjectModel.Solution.Close();
                visualStudio.Stop();
            }
        }

        private void UseService<T>(FileInfo solutionFile, Action<T> action)
            where T : class
        {
            VisualStudioHost visualStudio = null;

            try
            {
                VisualStudioHostConfiguration configuration = CreateVisualStudioHostConfiguration();

                configuration.AddCompositionAssembly(Assembly.GetExecutingAssembly().Location);

                visualStudio = LaunchVisualStudio(configuration);

                LoadSolution(visualStudio, solutionFile);
                WaitForAutoRestoreToComplete(visualStudio, solutionFile);

                using (Scope.Enter("Get service."))
                {
                    var service = visualStudio.Get<T>();

                    using (Scope.Enter("Verify service."))
                    {
                        action(service);
                    }
                }

                ShutDownVisualStudio(visualStudio);
            }
            catch (Exception ex)
            {
                HandleException(visualStudio, ex);
            }
        }

        private void WaitForAutoRestoreToComplete(VisualStudioHost visualStudio, FileInfo solutionFile)
        {
            using (Scope.Enter("Wait for auto restore completion."))
            {
                var assetsFile = new FileInfo(Path.Combine(solutionFile.DirectoryName, solutionFile.Directory.Name, "obj", "project.assets.json"));
                var timeout = TimeSpan.FromMinutes(1);
                var interval = TimeSpan.FromSeconds(5);

                const string RestoreOutputCompletionMarker = "========== Finished ==========";

                visualStudio.ObjectModel.Solution.WaitForIntellisenseStage(TimeSpan.FromMinutes(5));
                visualStudio.ObjectModel.Shell.ToolWindows.OutputWindow.ToolWindow.Show();

                // Wait for the assets file to be created.
                Omni.Common.WaitFor.IsTrue(
                    () => File.Exists(assetsFile.FullName),
                    timeout,
                    interval,
                    $"An assets file was not created at '{assetsFile.FullName}'.");

                // Wait for the solution restore to complete.
                Omni.Common.WaitFor.IsTrue(
                     () =>
                     {
                         if (TryGetPackageManagerOutputWindowPane(visualStudio, out OutputWindowPaneTestExtension packageManagerOutputWindowPane))
                         {
                             var output = packageManagerOutputWindowPane.Text;

                             if (!string.IsNullOrEmpty(output))
                             {
                                 output = output.TrimEnd('\r', '\n');

                                 return output.Contains(RestoreOutputCompletionMarker);
                             }
                         }

                         return false;
                     },
                     timeout,
                     interval,
                     "Solution restore did not complete according to the Package Manager output window pane.");
            }
        }

        private static bool TryGetPackageManagerOutputWindowPane(VisualStudioHost visualStudio, out OutputWindowPaneTestExtension packageManagerOutputWindowPane)
        {
            packageManagerOutputWindowPane = visualStudio.ObjectModel.Shell.ToolWindows.OutputWindow.GetOutputPane(_packageManagerOutputWindowPaneGuid);

            return packageManagerOutputWindowPane != null;
        }
    }
}

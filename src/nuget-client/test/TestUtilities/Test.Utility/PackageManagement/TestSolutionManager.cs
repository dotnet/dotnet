// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NuGet.Frameworks;
using NuGet.PackageManagement;
using NuGet.ProjectManagement;
using NuGet.ProjectManagement.Projects;
using NuGet.Test.Utility;
#if NETFRAMEWORK
using NuGet.ProjectModel;
using NuGet.VisualStudio;
#endif

namespace Test.Utility
{
    public class TestSolutionManager : ISolutionManager, IDisposable
    {
        private static readonly string GLOBAL_PACKAGES_ENV_KEY = "NUGET_PACKAGES";

        public List<NuGetProject> NuGetProjects { get; set; } = new List<NuGetProject>();

        public INuGetProjectContext NuGetProjectContext { get; set; } = new TestNuGetProjectContext();

        public string NuGetConfigPath { get; set; }

        public string GlobalPackagesFolder { get; set; }

        public string PackagesFolder { get; set; }

        public string SolutionDirectory { get; }

        public TestDirectory TestDirectory { get; private set; }

        private readonly string _configContent = @"<?xml version='1.0' encoding='utf-8'?>
<configuration>
  <packageSources>
    <add key='NuGet.org' value='https://api.nuget.org/v3/index.json' />
  </packageSources>
  <config>
     <add key='globalPackagesFolder' value='{0}' />
  </config>
</configuration>";

        public TestSolutionManager()
        {
            TestDirectory = TestDirectory.Create();
            SolutionDirectory = TestDirectory;
            NuGetConfigPath = Path.Combine(SolutionDirectory, "NuGet.Config");
            PackagesFolder = Path.Combine(SolutionDirectory, "packages");
            GlobalPackagesFolder = Path.Combine(SolutionDirectory, "globalpackages");

            // create nuget config in solution root
            File.WriteAllText(NuGetConfigPath, string.Format(CultureInfo.CurrentCulture, _configContent, GlobalPackagesFolder));
        }

        public TestSolutionManager(SimpleTestPathContext pathContext)
        {
            TestDirectory = pathContext.WorkingDirectory;
            SolutionDirectory = pathContext.SolutionRoot;
            NuGetConfigPath = pathContext.NuGetConfig;
            PackagesFolder = pathContext.PackagesV2;
            GlobalPackagesFolder = pathContext.UserPackagesFolder;
        }

        public TestSolutionManager(string solutionDirectory)
        {
            if (solutionDirectory == null)
            {
                throw new ArgumentNullException(nameof(solutionDirectory));
            }

            SolutionDirectory = solutionDirectory;
        }

        public MSBuildNuGetProject AddNewMSBuildProject(string projectName = null, NuGetFramework projectTargetFramework = null, string projectPath = null, bool validateExistingProject = true)
        {
            if (validateExistingProject)
            {
                var existingProject = Task.Run(async () => await GetNuGetProjectAsync(projectName));
                existingProject.Wait();
                if (existingProject.IsCompleted && existingProject.Result != null)
                {
                    throw new ArgumentException("Project with " + projectName + " already exists");
                }
            }

            projectName = string.IsNullOrEmpty(projectName) ? Guid.NewGuid().ToString() : projectName;
            var projectFullPath = projectPath ?? Path.Combine(SolutionDirectory, projectName);
            Directory.CreateDirectory(projectFullPath);

            projectTargetFramework = projectTargetFramework ?? NuGetFramework.Parse("net45");

            var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(projectTargetFramework, new TestNuGetProjectContext(),
                projectFullPath, projectName);
            var msBuildNuGetProject = new MSBuildNuGetProject(msBuildNuGetProjectSystem, PackagesFolder, projectFullPath);
            NuGetProjects.Add(msBuildNuGetProject);
            return msBuildNuGetProject;
        }

        public NuGetProject AddBuildIntegratedProject(string projectName = null, NuGetFramework projectTargetFramework = null, JObject json = null)
        {
            var existingProject = Task.Run(async () => await GetNuGetProjectAsync(projectName));
            existingProject.Wait();
            if (existingProject.IsCompleted && existingProject.Result != null)
            {
                throw new ArgumentException("Project with " + projectName + " already exists");
            }

            projectName = string.IsNullOrEmpty(projectName) ? Guid.NewGuid().ToString() : projectName;
            var projectFullPath = Path.Combine(SolutionDirectory, projectName);
            Directory.CreateDirectory(projectFullPath);

            var projectJsonPath = Path.Combine(projectFullPath, "project.json");
            CreateConfigJson(projectJsonPath, json?.ToString() ?? BasicConfig.ToString());

            projectTargetFramework = projectTargetFramework ?? NuGetFramework.Parse("net46");
            var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(projectTargetFramework, new TestNuGetProjectContext(),
                projectFullPath, projectName);

            var projectFilePath = Path.Combine(projectFullPath, $"{msBuildNuGetProjectSystem.ProjectName}.csproj");
            NuGetProject nuGetProject = new ProjectJsonNuGetProject(projectJsonPath, projectFilePath);
            NuGetProjects.Add(nuGetProject);

            return nuGetProject;
        }
#if IS_DESKTOP
        public NuGetProject AddCPSPackageReferenceBasedProject(IProjectSystemCache projectSystemCache, PackageSpec packageSpec)
        {
            if (packageSpec == null) throw new ArgumentNullException(nameof(packageSpec));

            var cpsPackageReferenceProject = TestCpsPackageReferenceProject.CreateTestCpsPackageReferenceProject(
            packageSpec.Name, packageSpec.FilePath, projectSystemCache, assetsFilePath: packageSpec.RestoreMetadata.OutputPath, packageSpec: packageSpec);
            Directory.CreateDirectory(packageSpec.FilePath);
            NuGetProjects.Add(cpsPackageReferenceProject);
            return cpsPackageReferenceProject;
        }
#endif

        private static void CreateConfigJson(string path, string config)
        {
            using (var writer = new StreamWriter(path))
            {
                writer.Write(config);
            }
        }

        private static JObject BasicConfig => new JObject
        {
            ["dependencies"] = new JObject
            {
                new JProperty("entityframework", "7.0.0-beta-*")
            },

            ["frameworks"] = new JObject
            {
                ["net46"] = new JObject()
            },

            ["runtimes"] = JObject.Parse("{ \"win-anycpu\": { } }")
        };

        public Task<NuGetProject> GetNuGetProjectAsync(string nuGetProjectSafeName)
        {
            return Task.FromResult(NuGetProjects.FirstOrDefault(p => string.Equals(nuGetProjectSafeName, p.GetMetadata<string>(NuGetProjectMetadataKeys.Name), StringComparison.OrdinalIgnoreCase)));
        }

        public Task<string> GetNuGetProjectSafeNameAsync(NuGetProject nuGetProject)
        {
            return Task.FromResult(nuGetProject.GetMetadata<string>(NuGetProjectMetadataKeys.Name));
        }

        public Task<IEnumerable<NuGetProject>> GetNuGetProjectsAsync()
        {
            return Task.FromResult(NuGetProjects.AsEnumerable());
        }

        public bool IsSolutionOpen
        {
            get { return NuGetProjects.Count > 0; }
        }

        public Task<bool> IsSolutionAvailableAsync()
        {
            return Task.FromResult(IsSolutionOpen);
        }

        public void EnsureSolutionIsLoaded()
        {
            // do nothing
        }

        public Task<bool> DoesNuGetSupportsAnyProjectAsync()
        {
            return Task.FromResult(true);
        }

#pragma warning disable 0067

        public event EventHandler<NuGetProjectEventArgs> NuGetProjectAdded;

        public event EventHandler<NuGetProjectEventArgs> NuGetProjectRemoved;

        public event EventHandler<NuGetProjectEventArgs> NuGetProjectRenamed;

        public event EventHandler<NuGetProjectEventArgs> NuGetProjectUpdated;

        public event EventHandler<NuGetProjectEventArgs> AfterNuGetProjectRenamed;

        public event EventHandler SolutionClosed;

        public event EventHandler SolutionClosing;

        public event EventHandler SolutionOpened;

        public event EventHandler SolutionOpening;

        public event EventHandler<NuGetEventArgs<string>> AfterNuGetCacheUpdated;

        public event EventHandler<ActionsExecutedEventArgs> ActionsExecuted;

        public void OnActionsExecuted(IEnumerable<ResolvedAction> actions)
        {
            if (ActionsExecuted != null)
            {
                ActionsExecuted(this, new ActionsExecutedEventArgs(actions));
            }
        }

        public void OnSolutionClosed()
        {
            SolutionClosed?.Invoke(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            var testDirectory = TestDirectory;
            if (testDirectory != null)
            {
                testDirectory.Dispose();
                TestDirectory = null;
            }

            // reset environment variable
            Environment.SetEnvironmentVariable(GLOBAL_PACKAGES_ENV_KEY, null);
        }

#pragma warning restore 0067
    }
}

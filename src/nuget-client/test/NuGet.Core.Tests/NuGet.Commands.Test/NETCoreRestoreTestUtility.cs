// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.ProjectModel;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;

namespace NuGet.Commands.Test
{
    public static class NETCoreRestoreTestUtility
    {
        public static List<SimpleTestProjectContext> CreateProjectsFromSpecs(SimpleTestPathContext pathContext, params PackageSpec[] specs)
        {
            var projects = new List<SimpleTestProjectContext>();

            foreach (var spec in specs)
            {
                var project = new SimpleTestProjectContext(spec.Name, ProjectStyle.PackageReference, pathContext.SolutionRoot);

                // Set proj properties
                spec.FilePath = project.ProjectPath;
                spec.RestoreMetadata.OutputPath = project.ProjectExtensionsPath;
                spec.RestoreMetadata.ProjectPath = project.ProjectPath;

                projects.Add(project);
            }

            var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot, projects.ToArray());
            solution.Create(pathContext.SolutionRoot);

            return projects;
        }

        public static async Task<IReadOnlyList<RestoreSummary>> RunRestore(
            SimpleTestPathContext pathContext,
            TestLogger logger,
            List<PackageSource> sources,
            DependencyGraphSpec dgFile,
            SourceCacheContext cacheContext)
        {
            var restoreContext = new RestoreArgs()
            {
                CacheContext = cacheContext,
                DisableParallel = true,
                GlobalPackagesFolder = pathContext.UserPackagesFolder,
                Sources = new List<string>() { pathContext.PackageSource },
                Log = logger,
                CachingSourceProvider = new CachingSourceProvider(new TestPackageSourceProvider(sources)),
                PreLoadedRequestProviders = new List<IPreLoadedRestoreRequestProvider>()
                {
                    new DependencyGraphSpecRequestProvider(new RestoreCommandProvidersCache(), dgFile)
                }
            };

            return await RestoreRunner.RunAsync(restoreContext);
        }

        public static PackageSpec GetProject(string projectName, string framework)
        {
            var targetFrameworkInfo = new TargetFrameworkInformation();
            targetFrameworkInfo.FrameworkName = NuGetFramework.Parse(framework);
            var frameworks = new[] { targetFrameworkInfo };

            // Create two net45 projects
            var spec = new PackageSpec(frameworks);
            spec.RestoreMetadata = new ProjectRestoreMetadata();
            spec.RestoreMetadata.ProjectUniqueName = $"{projectName}-UNIQUENAME";
            spec.RestoreMetadata.ProjectName = projectName;
            spec.RestoreMetadata.ProjectStyle = ProjectStyle.PackageReference;
            spec.RestoreMetadata.OriginalTargetFrameworks.Add(framework);
            spec.Name = projectName;
            spec.RestoreMetadata.TargetFrameworks.Add(new ProjectRestoreMetadataFrameworkInfo(targetFrameworkInfo.FrameworkName) { TargetAlias = framework });

            return spec;
        }
    }
}

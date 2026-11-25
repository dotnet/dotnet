// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.ProjectModel;
using NuGet.Test.Utility;

namespace NuGet.CommandLine.XPlat.Tests
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
            solution.Create();

            return projects;
        }

        public static PackageSpec GetProject(string projectName, string framework, LibraryDependency dependency)
        {
            var parsedFramework = NuGetFramework.Parse(framework);
            var targetFrameworkInfo = dependency == null ?
                new TargetFrameworkInformation()
                {
                    FrameworkName = parsedFramework
                } :
                new TargetFrameworkInformation()
                {
                    FrameworkName = parsedFramework,
                    Dependencies = [dependency]
                };

            var frameworks = new[] { targetFrameworkInfo };

            // Create two net45 projects
            var spec = new PackageSpec(frameworks);
            spec.RestoreMetadata = new ProjectRestoreMetadata();
            spec.RestoreMetadata.ProjectUniqueName = $"{projectName}-UNIQUENAME";
            spec.RestoreMetadata.ProjectName = projectName;
            spec.RestoreMetadata.ProjectStyle = ProjectStyle.PackageReference;
            spec.RestoreMetadata.OriginalTargetFrameworks.Add(framework);
            spec.Name = projectName;
            spec.RestoreMetadata.TargetFrameworks.Add(new ProjectRestoreMetadataFrameworkInfo(parsedFramework) { TargetAlias = framework });

            return spec;
        }
    }
}

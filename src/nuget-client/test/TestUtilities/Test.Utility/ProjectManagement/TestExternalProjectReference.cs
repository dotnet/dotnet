// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.ProjectManagement;

namespace Test.Utility
{
    public class TestExternalProjectReference
    {
        public IDependencyGraphProject Project { get; set; }

        public IDependencyGraphProject[] Children { get; set; }

        public TestExternalProjectReference(
            IDependencyGraphProject project,
            params IDependencyGraphProject[] children)
        {
            Project = project;
            Children = children;
            MSBuildProjectPath = project.MSBuildProjectPath;
        }

        public string MSBuildProjectPath { get; set; }
    }
}

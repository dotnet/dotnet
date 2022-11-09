// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using Microsoft.VisualStudio.ProjectSystem;

namespace NuGet.VisualStudio.SolutionExplorer
{
    internal static class DependencyTreeFlags
    {
        public static ProjectTreeFlags TargetNode { get; } = ProjectTreeFlags.Create("TargetNode");

        public static ProjectTreeFlags PackageDependencyGroup { get; } = ProjectTreeFlags.Create("PackageDependencyGroup");
        public static ProjectTreeFlags PackageDependency { get; } = ProjectTreeFlags.Create("PackageDependency");

        public static ProjectTreeFlags ProjectDependencyGroup { get; } = ProjectTreeFlags.Create("ProjectDependencyGroup");
        public static ProjectTreeFlags ProjectDependency { get; } = ProjectTreeFlags.Create("ProjectDependency");
    }
}

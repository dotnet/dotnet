// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.ProjectModel;

namespace NuGet.CommandLine.XPlat.Commands.Package.Update
{
    /// <summary>
    /// Mockable interface for generating DGSpecs.
    /// </summary>
    internal interface IDGSpecFactory
    {
        /// <summary>Create a DGSpec for the requested project or solution.</summary>
        /// <param name="project">Path to a solution or project file.</param>
        /// <returns>A DGSpec representing the project or solution's restore inputs.</returns>
        DependencyGraphSpec GetDependencyGraphSpec(string project);
    }
}

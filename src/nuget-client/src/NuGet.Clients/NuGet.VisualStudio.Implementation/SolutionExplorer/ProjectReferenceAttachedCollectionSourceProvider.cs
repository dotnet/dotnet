// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using EnvDTE;
using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Utilities;

using NuGet.VisualStudio.SolutionExplorer.Models;

namespace NuGet.VisualStudio.SolutionExplorer
{
    [AppliesToProject(ProjectCapability.DependenciesTree)]
    [Export(typeof(IAttachedCollectionSourceProvider))]
    [Name(nameof(ProjectReferenceAttachedCollectionSourceProvider))]
    [Order(Before = HierarchyItemsProviderNames.Contains)]
    internal sealed class ProjectReferenceAttachedCollectionSourceProvider : AssetsFileTopLevelDependenciesCollectionSourceProvider<ProjectReferenceItem>
    {
        public ProjectReferenceAttachedCollectionSourceProvider()
            : base(DependencyTreeFlags.ProjectDependency)
        {
        }

        protected override bool TryGetLibraryName(Properties properties, [NotNullWhen(returnValue: true)] out string libraryName)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            try
            {
                if (properties.Item("Identity")?.Value is string identity)
                {
                    libraryName = identity;
                    return true;
                }
            }
            catch (Microsoft.VisualStudio.ProjectSystem.ProjectException)
            {
                // Work around https://github.com/dotnet/project-system/issues/6311
                // "Could not find project item with item type 'ProjectReference' and include value '...'.
            }

            libraryName = null!;
            return false;
        }

        protected override bool TryGetLibrary(AssetsFileTarget target, string libraryName, [NotNullWhen(returnValue: true)] out AssetsFileTargetLibrary? library)
        {
            return target.TryGetProject(libraryName, out library);
        }

        protected override ProjectReferenceItem CreateItem(AssetsFileTarget targetData, AssetsFileTargetLibrary library)
        {
            return new ProjectReferenceItem(targetData, library);
        }

        protected override bool TryUpdateItem(ProjectReferenceItem item, AssetsFileTarget targetData, AssetsFileTargetLibrary library)
        {
            return item.TryUpdateState(targetData, library);
        }
    }
}

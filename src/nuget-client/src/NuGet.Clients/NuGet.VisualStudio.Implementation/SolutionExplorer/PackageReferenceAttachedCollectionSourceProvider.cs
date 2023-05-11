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
    [Name(nameof(PackageReferenceAttachedCollectionSourceProvider))]
    [Order(Before = HierarchyItemsProviderNames.Contains)]
    internal sealed class PackageReferenceAttachedCollectionSourceProvider : AssetsFileTopLevelDependenciesCollectionSourceProvider<PackageReferenceItem>
    {
        public PackageReferenceAttachedCollectionSourceProvider()
            : base(DependencyTreeFlags.PackageDependency)
        {
        }

        protected override bool TryGetLibraryName(Properties properties, [NotNullWhen(returnValue: true)] out string? libraryName)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            try
            {
                if (properties.Item("Name")?.Value is string name &&
                    !string.IsNullOrEmpty(name))
                {
                    libraryName = name;
                    return true;
                }
            }
            catch (Microsoft.VisualStudio.ProjectSystem.ProjectException)
            {
                // Work around https://github.com/dotnet/project-system/issues/6311
                // "Could not find project item with item type 'PackageReference' and include value '...'.
            }

            libraryName = null;
            return false;
        }

        protected override bool TryGetLibrary(AssetsFileTarget target, string libraryName, [NotNullWhen(returnValue: true)] out AssetsFileTargetLibrary? library)
        {
            return target.TryGetPackage(libraryName, out library);
        }

        protected override PackageReferenceItem CreateItem(AssetsFileTarget targetData, AssetsFileTargetLibrary library)
        {
            return new PackageReferenceItem(targetData, library);
        }

        protected override bool TryUpdateItem(PackageReferenceItem item, AssetsFileTarget targetData, AssetsFileTargetLibrary library)
        {
            return item.TryUpdateState(targetData, library);
        }
    }
}

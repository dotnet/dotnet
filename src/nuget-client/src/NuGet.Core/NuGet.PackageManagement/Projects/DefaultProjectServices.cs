// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Packaging.Core;
using NuGet.ProjectModel;

namespace NuGet.ProjectManagement
{
    /// <summary>
    /// Null-object with no-op implementation of project services.
    /// </summary>
    internal sealed class DefaultProjectServices
        : INuGetProjectServices
#pragma warning disable CS0618 // Type or member is obsolete
        , IProjectBuildProperties
#pragma warning restore CS0618 // Type or member is obsolete
        , IProjectScriptHostService
        , IProjectSystemCapabilities
        , IProjectSystemReferencesReader
        , IProjectSystemReferencesService
        , IProjectSystemService
    {
        public static INuGetProjectServices Instance { get; } = new DefaultProjectServices();

        [Obsolete]
        public IProjectBuildProperties BuildProperties => this;
        public IProjectSystemCapabilities Capabilities => this;
        public IProjectSystemReferencesReader ReferencesReader => this;
        public IProjectSystemService ProjectSystem => this;
        public IProjectSystemReferencesService References => this;
        public IProjectScriptHostService ScriptService => this;

        public bool SupportsPackageReferences => false;

        public bool NominatesOnSolutionLoad => false;

        public Task AddOrUpdatePackageReferenceAsync(
            LibraryDependency packageReference,
            CancellationToken _)
        {
            throw new NotSupportedException();
        }

        public Task<IEnumerable<LibraryDependency>> GetPackageReferencesAsync(
            NuGetFramework targetFramework,
            CancellationToken _)
        {
            throw new NotSupportedException();
        }

        public Task<IEnumerable<ProjectRestoreReference>> GetProjectReferencesAsync(
            Common.ILogger _,
            CancellationToken __)
        {
            return TaskResult.EmptyEnumerable<ProjectRestoreReference>();
        }

        public Task<IReadOnlyList<(string id, string[] metadata)>> GetItemsAsync(string itemTypeName, params string[] metadataNames)
        {
            IReadOnlyList<(string, string[])> items = Array.Empty<(string, string[])>();
            return Task.FromResult(items);
        }

        public string GetPropertyValue(string propertyName)
        {
            return null;
        }

        public Task<string> GetPropertyValueAsync(string propertyName)
        {
            return TaskResult.Null<string>();
        }

        public T GetGlobalService<T>() where T : class
        {
            return null;
        }

        public Task RemovePackageReferenceAsync(string packageName)
        {
            throw new NotSupportedException();
        }

        public Task SaveProjectAsync(CancellationToken _)
        {
            // do nothing
            return Task.CompletedTask;
        }

        public Task ExecutePackageScriptAsync(
            PackageIdentity packageIdentity,
            string packageInstallPath,
            string scriptRelativePath,
            INuGetProjectContext projectContext,
            bool throwOnFailure,
            CancellationToken _)
        {
            // No-op
            return Task.CompletedTask;
        }

        public Task<bool> ExecutePackageInitScriptAsync(
            PackageIdentity packageIdentity,
            string packageInstallPath,
            INuGetProjectContext projectContext,
            bool throwOnFailure,
            CancellationToken _)
        {
            // No-op
            return TaskResult.False;
        }
    }
}

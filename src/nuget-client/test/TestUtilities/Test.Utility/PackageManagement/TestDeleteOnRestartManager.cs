// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NuGet.PackageManagement;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;

namespace Test.Utility
{
    public class TestDeleteOnRestartManager : IDeleteOnRestartManager
    {
        public event EventHandler<PackagesMarkedForDeletionEventArgs> PackagesMarkedForDeletionFound;

        public IReadOnlyList<string> GetPackageDirectoriesMarkedForDeletion()
        {
            return new List<string>();
        }

        public void CheckAndRaisePackageDirectoriesMarkedForDeletion()
        {
            PackagesMarkedForDeletionFound?.Invoke(this, null);
        }

        public void MarkPackageDirectoryForDeletion(
            PackageIdentity package,
            string packageDirectory,
            INuGetProjectContext projectContext)
        {
        }

        public Task DeleteMarkedPackageDirectoriesAsync(INuGetProjectContext projectContext)
        {
            return Task.CompletedTask;
        }
    }
}

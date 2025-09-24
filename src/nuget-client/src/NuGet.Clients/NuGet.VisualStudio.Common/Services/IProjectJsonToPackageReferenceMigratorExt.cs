// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using NuGet.ProjectManagement;

namespace NuGet.VisualStudio.Services
{
    public interface IProjectJsonToPackageReferenceMigratorExt : IVsProjectJsonToPackageReferenceMigrator
    {
        Task<object> MigrateProjectJsonToPackageReferenceAsync(NuGetProject nuGetProject, IVsProjectAdapter projectAdapter);
    }
}

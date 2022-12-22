// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using NuGet.Packaging.Core;

namespace NuGet.ProjectManagement
{
    public abstract class ExecutionContext
    {
        // HACK: TODO: OpenFile is likely never called from ProjectManagement
        // Should only be in PackageManagement
        public abstract Task OpenFile(string fullPath);

        public PackageIdentity DirectInstall { get; protected set; }
    }
}

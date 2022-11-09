// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace NuGet.PackageManagement.VisualStudio
{
    public interface IScriptPackage
    {
        string Id { get; }

        string Version { get; }

        IEnumerable<IPackageAssemblyReference> AssemblyReferences { get; }

        IEnumerable<IScriptPackageFile> GetFiles();
    }
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Reflection;

namespace NuGet.PackageManagement.VisualStudio
{
    internal sealed class FrameworkAssembly
    {
        internal AssemblyName AssemblyName { get; }
        internal bool IsFacade { get; }

        internal FrameworkAssembly(AssemblyName assemblyName, bool isFacade)
        {
            AssemblyName = assemblyName;
            IsFacade = isFacade;
        }
    }
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.LibraryModel;

namespace NuGet.DependencyResolver
{
    public class RemoteResolveResult
    {
        internal static readonly List<LibraryDependency> EmptyDependencies = new List<LibraryDependency>(0);

        public RemoteMatch Match { get; set; }
        public List<LibraryDependency> Dependencies { get; set; }
    }
}

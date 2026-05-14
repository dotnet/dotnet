// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace NuGet.VisualStudio
{
    public interface IAssembly
    {
        string Name { get; }
        Version Version { get; }
        string PublicKeyToken { get; }
        string Culture { get; }
        IEnumerable<IAssembly> ReferencedAssemblies { get; }
    }
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.LibraryModel
{
    [Flags]
    public enum FrameworkDependencyFlags : ushort
    {
        None = 0,
        All = ushort.MaxValue,
    }
}

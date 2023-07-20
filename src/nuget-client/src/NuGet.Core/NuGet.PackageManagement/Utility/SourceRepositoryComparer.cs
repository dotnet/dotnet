// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Protocol.Core.Types;

namespace NuGet.PackageManagement
{
    public class SourceRepositoryComparer : IEqualityComparer<SourceRepository>
    {
#pragma warning disable CS0618 // Type or member is obsolete
        public static SourceRepositoryComparer Instance { get; } = new();
#pragma warning restore CS0618 // Type or member is obsolete

        [Obsolete("Use singleton SourceRepositoryComparer.Instance instead")]
        public SourceRepositoryComparer()
        {
        }

        public bool Equals(SourceRepository x, SourceRepository y)
        {
            return x.PackageSource.Equals(y.PackageSource);
        }

        public int GetHashCode(SourceRepository obj)
        {
            return obj.PackageSource.GetHashCode();
        }
    }
}

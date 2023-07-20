// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace NuGet.Protocol.Core.Types
{
    /// <summary>
    /// An imperfect sort for provider before/after
    /// </summary>
    internal class ProviderComparer : IComparer<INuGetResourceProvider>
    {
        public static ProviderComparer Instance { get; } = new();

        private ProviderComparer()
        {
        }

        // higher goes last
        public int Compare(INuGetResourceProvider providerA, INuGetResourceProvider providerB)
        {
            if (StringComparer.Ordinal.Equals(providerA.Name, providerB.Name))
            {
                return 0;
            }

            // empty names go last
            if (String.IsNullOrEmpty(providerA.Name))
            {
                return 1;
            }

            if (String.IsNullOrEmpty(providerB.Name))
            {
                return -1;
            }

            // check x 
            if (providerA.Before.Contains(providerB.Name, StringComparer.Ordinal))
            {
                return -1;
            }

            if (providerA.After.Contains(providerB.Name, StringComparer.Ordinal))
            {
                return 1;
            }

            // check y
            if (providerB.Before.Contains(providerA.Name, StringComparer.Ordinal))
            {
                return 1;
            }

            if (providerB.After.Contains(providerA.Name, StringComparer.Ordinal))
            {
                return -1;
            }

            // compare with the known names
            if ((providerA.Before.Contains(NuGetResourceProviderPositions.Last, StringComparer.Ordinal) || (providerA.After.Contains(NuGetResourceProviderPositions.Last, StringComparer.Ordinal)))
                && !(providerB.Before.Contains(NuGetResourceProviderPositions.Last, StringComparer.Ordinal) || (providerB.After.Contains(NuGetResourceProviderPositions.Last, StringComparer.Ordinal))))
            {
                return 1;
            }

            if ((providerB.Before.Contains(NuGetResourceProviderPositions.Last, StringComparer.Ordinal) || (providerB.After.Contains(NuGetResourceProviderPositions.Last, StringComparer.Ordinal)))
                && !(providerA.Before.Contains(NuGetResourceProviderPositions.Last, StringComparer.Ordinal) || (providerA.After.Contains(NuGetResourceProviderPositions.Last, StringComparer.Ordinal))))
            {
                return -1;
            }

            if ((providerA.Before.Contains(NuGetResourceProviderPositions.First, StringComparer.Ordinal) || (providerA.After.Contains(NuGetResourceProviderPositions.First, StringComparer.Ordinal)))
                && !(providerB.Before.Contains(NuGetResourceProviderPositions.First, StringComparer.Ordinal) || (providerB.After.Contains(NuGetResourceProviderPositions.First, StringComparer.Ordinal))))
            {
                return -1;
            }

            if ((providerB.Before.Contains(NuGetResourceProviderPositions.First, StringComparer.Ordinal) || (providerB.After.Contains(NuGetResourceProviderPositions.First, StringComparer.Ordinal)))
                && !(providerA.Before.Contains(NuGetResourceProviderPositions.First, StringComparer.Ordinal) || (providerA.After.Contains(NuGetResourceProviderPositions.First, StringComparer.Ordinal))))
            {
                return 1;
            }

            // give up and sort based on the name
            return StringComparer.Ordinal.Compare(providerA.Name, providerB.Name);
        }
    }
}

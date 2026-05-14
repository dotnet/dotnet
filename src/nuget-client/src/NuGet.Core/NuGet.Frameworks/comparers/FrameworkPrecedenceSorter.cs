// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace NuGet.Frameworks
{
    /// <summary>
    /// Sorts frameworks according to the framework mappings
    /// </summary>
    public class FrameworkPrecedenceSorter : IComparer<NuGetFramework>
    {
        private readonly IFrameworkNameProvider _mappings;
        private readonly bool _allEquivalent;

        public FrameworkPrecedenceSorter(IFrameworkNameProvider mappings, bool allEquivalent)
        {
            _mappings = mappings ?? throw new ArgumentNullException(nameof(mappings));
            _allEquivalent = allEquivalent;
        }

        public int Compare(NuGetFramework? x, NuGetFramework? y)
        {
            return _allEquivalent ? _mappings.CompareEquivalentFrameworks(x, y) : _mappings.CompareFrameworks(x, y);
        }
    }
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Shared;

namespace NuGet.ProjectModel
{
    public class CentralTransitiveDependencyGroup : IEquatable<CentralTransitiveDependencyGroup>
    {
        public CentralTransitiveDependencyGroup(NuGetFramework framework, IEnumerable<LibraryDependency> transitiveDependencies)
        {
            if (framework == null)
            {
                throw new ArgumentNullException(nameof(framework));
            }
            if (transitiveDependencies == null)
            {
                throw new ArgumentNullException(nameof(transitiveDependencies));
            }

            FrameworkName = framework.ToString();
            TransitiveDependencies = transitiveDependencies;
        }

        public string FrameworkName { get; }

        public IEnumerable<LibraryDependency> TransitiveDependencies { get; }

        public bool Equals(CentralTransitiveDependencyGroup other)
        {
            if (other == null)
            {
                return false;
            }

            if (Object.ReferenceEquals(this, other))
            {
                return true;
            }

            if (string.Equals(FrameworkName, other.FrameworkName, StringComparison.OrdinalIgnoreCase))
            {
                return EqualityUtility.OrderedEquals(TransitiveDependencies, other.TransitiveDependencies, dep => dep.Name, StringComparer.OrdinalIgnoreCase);
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as CentralTransitiveDependencyGroup);
        }

        public override int GetHashCode()
        {
            var combiner = new HashCodeCombiner();
            combiner.AddStringIgnoreCase(FrameworkName);
            combiner.AddUnorderedSequence(TransitiveDependencies);
            return combiner.CombinedHash;
        }
    }
}

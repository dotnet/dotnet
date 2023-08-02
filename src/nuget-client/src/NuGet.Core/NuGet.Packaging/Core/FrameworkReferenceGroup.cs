// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Frameworks;
using NuGet.Shared;

namespace NuGet.Packaging
{
    /// <summary>
    /// A group of FrameworkReference with the same target framework.
    /// </summary>
    public class FrameworkReferenceGroup : IEquatable<FrameworkReferenceGroup>, IFrameworkSpecific
    {
        /// <summary>
        /// FrameworkReference group
        /// </summary>
        /// <param name="targetFramework">group target framework</param>
        /// <param name="frameworkReferences">FrameworkReferences</param>
        public FrameworkReferenceGroup(NuGetFramework targetFramework, IEnumerable<FrameworkReference> frameworkReferences)
        {
            TargetFramework = targetFramework ?? throw new ArgumentNullException(nameof(targetFramework));
            FrameworkReferences = frameworkReferences ?? throw new ArgumentNullException(nameof(frameworkReferences));
        }

        /// <summary>
        /// Group target framework
        /// </summary>
        public NuGetFramework TargetFramework { get; }

        /// <summary>
        /// FrameworkReferences
        /// </summary>
        public IEnumerable<FrameworkReference> FrameworkReferences { get; }

        public bool Equals(FrameworkReferenceGroup other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return TargetFramework.Equals(other.TargetFramework) &&
                   FrameworkReferences.OrderedEquals(other.FrameworkReferences, dependency => dependency);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as FrameworkReferenceGroup);
        }

        public override int GetHashCode()
        {
            var combiner = new HashCodeCombiner();

            combiner.AddObject(TargetFramework);
            combiner.AddUnorderedSequence(FrameworkReferences);

            return combiner.CombinedHash;
        }
    }
}

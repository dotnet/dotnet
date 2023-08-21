// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.Shared;

namespace NuGet.Packaging
{
    /// <summary>
    /// A group of items/files from a nupkg with the same target framework.
    /// </summary>
    public class FrameworkSpecificGroup : IEquatable<FrameworkSpecificGroup>, IFrameworkSpecific
    {
        private readonly NuGetFramework _targetFramework;
        private readonly string[] _items;

        /// <summary>
        /// Framework specific group
        /// </summary>
        /// <param name="targetFramework">group target framework</param>
        /// <param name="items">group items</param>
        public FrameworkSpecificGroup(NuGetFramework targetFramework, IEnumerable<string> items)
        {
            if (targetFramework == null)
            {
                throw new ArgumentNullException(nameof(targetFramework));
            }

            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            _targetFramework = targetFramework;

            HasEmptyFolder = items.Any(item => item.EndsWith(PackagingCoreConstants.ForwardSlashEmptyFolder,
                StringComparison.Ordinal));

            // Remove empty folder markers here
            _items = items.Where(item => !item.EndsWith(PackagingCoreConstants.ForwardSlashEmptyFolder,
                StringComparison.Ordinal))
                    .ToArray();
        }

        /// <summary>
        /// Group target framework
        /// </summary>
        public NuGetFramework TargetFramework
        {
            get { return _targetFramework; }
        }

        /// <summary>
        /// Item relative paths
        /// </summary>
        public IEnumerable<string> Items
        {
            get { return _items; }
        }

        public bool HasEmptyFolder { get; }

        public bool Equals(FrameworkSpecificGroup other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (ReferenceEquals(other, null))
            {
                return false;
            }

            return GetHashCode() == other.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as FrameworkSpecificGroup;

            if (other != null)
            {
                return Equals(other);
            }

            return false;
        }

        public override int GetHashCode()
        {
            if (ReferenceEquals(this, null))
            {
                return 0;
            }

            var combiner = new HashCodeCombiner();

            combiner.AddObject(TargetFramework);
            combiner.AddUnorderedSequence(Items, StringComparer.Ordinal);

            return combiner.CombinedHash;
        }
    }
}

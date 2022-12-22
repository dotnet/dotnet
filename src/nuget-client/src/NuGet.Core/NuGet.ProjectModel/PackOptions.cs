// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Packaging.Core;
using NuGet.Shared;

namespace NuGet.ProjectModel
{
    public class PackOptions : IEquatable<PackOptions>
    {
        public IReadOnlyList<PackageType> PackageType { get; set; } = new List<PackageType>();
        public IncludeExcludeFiles IncludeExcludeFiles { get; set; }
        public IDictionary<string, IncludeExcludeFiles> Mappings { get; set; } = new Dictionary<string, IncludeExcludeFiles>();

        public override int GetHashCode()
        {
            var hashCode = new HashCodeCombiner();

            hashCode.AddSequence(PackageType);
            hashCode.AddObject(IncludeExcludeFiles);
            hashCode.AddDictionary(Mappings);

            return hashCode.CombinedHash;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as PackOptions);
        }

        public bool Equals(PackOptions other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return PackageType.SequenceEqualWithNullCheck(other.PackageType) &&
                   Mappings.SequenceEqualWithNullCheck(other.Mappings) &&
                   EqualityUtility.EqualsWithNullCheck(IncludeExcludeFiles, other.IncludeExcludeFiles);
        }
        public PackOptions Clone()
        {
            var clonedObject = new PackOptions();
            clonedObject.PackageType = new List<PackageType>(PackageType);
            clonedObject.IncludeExcludeFiles = IncludeExcludeFiles?.Clone();
            if (Mappings != null)
            {
                clonedObject.Mappings = new Dictionary<string, IncludeExcludeFiles>();
                foreach (var kvp in Mappings)
                {
                    clonedObject.Mappings.Add(kvp.Key, kvp.Value.Clone());
                }
            }
            return clonedObject;
        }
    }
}

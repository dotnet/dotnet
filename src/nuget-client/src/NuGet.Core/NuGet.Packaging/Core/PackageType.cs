// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Shared;

namespace NuGet.Packaging.Core
{
    /**
     * It is important that this type remains immutable due to the cloning of package specs
     **/
    public class PackageType : IEquatable<PackageType>, IComparable<PackageType>
    {
        public static readonly Version EmptyVersion = new Version(0, 0);
        public static readonly PackageType Legacy = new PackageType("Legacy", version: EmptyVersion);
        public static readonly PackageType DotnetCliTool = new PackageType("DotnetCliTool", version: EmptyVersion);
        public static readonly PackageType Dependency = new PackageType("Dependency", version: EmptyVersion);
        public static readonly PackageType DotnetTool = new PackageType("DotnetTool", version: EmptyVersion);
        public static readonly PackageType SymbolsPackage = new PackageType("SymbolsPackage", version: EmptyVersion);
        public static readonly PackageType DotnetPlatform = new PackageType("DotnetPlatform", version: EmptyVersion);

        public static readonly StringComparer PackageTypeNameComparer = StringComparer.OrdinalIgnoreCase;

        public PackageType(string name, Version version)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException(Strings.StringCannotBeNullOrEmpty, nameof(name));
            }

            Name = name;
            Version = version ?? throw new ArgumentNullException(nameof(version));
        }


        public string Name { get; }

        public Version Version { get; }

        public override bool Equals(object obj)
        {
            return Equals(obj as PackageType);
        }

        public static bool operator ==(PackageType a, PackageType b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(PackageType a, PackageType b)
        {
            return !(a == b);
        }

        public bool Equals(PackageType other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return
                Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase) &&
                Version == other.Version;
        }

        public override int GetHashCode()
        {
            var combiner = new HashCodeCombiner();

            combiner.AddObject(Name, StringComparer.OrdinalIgnoreCase);
            combiner.AddObject(Version);

            return combiner.CombinedHash;
        }

        public int CompareTo(PackageType other)
        {

            if (other == null)
            {
                return 1;
            }

            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            var res = PackageTypeNameComparer.Compare(Name, other.Name);

            if (res != 0)
            {
                return res;
            }

            return Version.CompareTo(other.Version);
        }
    }
}

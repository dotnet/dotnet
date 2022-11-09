// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Frameworks;
using NuGet.PackageManagement.VisualStudio.Utility;
using NuGet.Shared;

namespace NuGet.PackageManagement.VisualStudio
{
    public class FrameworkInstalledPackages : IEquatable<FrameworkInstalledPackages>
    {
        public NuGetFramework TargetFramework { get; set; }
        public Dictionary<string, ProjectInstalledPackage> Packages { get; internal set; }

        public bool Equals(FrameworkInstalledPackages other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            bool equalsFramework;
            if (TargetFramework != null)
            {
                equalsFramework = TargetFramework.Equals(other.TargetFramework);
            }
            else
            {
                equalsFramework = other.TargetFramework == null;
            }

            bool equalsDict = false;
            if (Packages != null)
            {
                if (other.Packages != null)
                {
                    equalsDict = Packages.Count == other.Packages.Count && !Packages.Except(other.Packages).Any();
                }
            }
            else
            {
                equalsDict = other.Packages == null;
            }

            return equalsFramework && equalsDict;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as FrameworkInstalledPackages);
        }

        public override int GetHashCode()
        {
            return HashCodeCombiner.GetHashCode(TargetFramework, Packages);
        }
    }
}

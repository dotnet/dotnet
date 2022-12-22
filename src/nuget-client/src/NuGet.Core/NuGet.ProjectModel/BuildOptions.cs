// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Shared;

namespace NuGet.ProjectModel
{
    public class BuildOptions : IEquatable<BuildOptions>
    {
        public string OutputName { get; set; }

        public override int GetHashCode()
        {
            var hashCode = new HashCodeCombiner();

            hashCode.AddObject(OutputName);

            return hashCode.CombinedHash;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as BuildOptions);
        }

        public bool Equals(BuildOptions other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return OutputName == other.OutputName;
        }

        public BuildOptions Clone()
        {
            var clone = new BuildOptions();
            clone.OutputName = OutputName;
            return clone;
        }
    }
}

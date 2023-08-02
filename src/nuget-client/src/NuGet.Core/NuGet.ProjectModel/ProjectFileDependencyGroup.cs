// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Shared;

namespace NuGet.ProjectModel
{
    public class ProjectFileDependencyGroup : IEquatable<ProjectFileDependencyGroup>
    {
        public ProjectFileDependencyGroup(string frameworkName, IEnumerable<string> dependencies)
        {
            FrameworkName = frameworkName;
            Dependencies = dependencies;
        }

        public string FrameworkName { get; }

        public IEnumerable<string> Dependencies { get; }

        public bool Equals(ProjectFileDependencyGroup other)
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
                if (Dependencies == null || other.Dependencies == null)
                {
                    return Dependencies == other.Dependencies;
                }

                return Dependencies.OrderedEquals(other.Dependencies, s => s, StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase);
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ProjectFileDependencyGroup);
        }

        public override int GetHashCode()
        {
            var combiner = new HashCodeCombiner();

            combiner.AddStringIgnoreCase(FrameworkName);
            combiner.AddUnorderedSequence(Dependencies, StringComparer.OrdinalIgnoreCase);

            return combiner.CombinedHash;
        }
    }
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.ProjectModel;

namespace NuGet.SolutionRestoreManager
{
    internal class ProjectRestoreReferenceComparer : IEqualityComparer<ProjectRestoreReference>
    {
        public static ProjectRestoreReferenceComparer Default { get; } = new ProjectRestoreReferenceComparer();

        public bool Equals(ProjectRestoreReference x, ProjectRestoreReference y)
        {
            if (x == null) { throw new ArgumentNullException(nameof(x)); }
            if (y == null) { throw new ArgumentNullException(nameof(y)); }

            return string.Equals(x.ProjectUniqueName, y.ProjectUniqueName, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(ProjectRestoreReference obj)
        {
            if (obj == null) { throw new ArgumentNullException(nameof(obj)); }

            return obj.ProjectUniqueName.ToUpperInvariant().GetHashCode();
        }
    }
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.Build.Execution;
using NuGet.Commands;

namespace NuGet.Build.Tasks.Console
{
    /// <summary>
    /// Represents a comparer of MSBuild items which considers any item with the same Include value (case-insensitive) to be identical, regardless of its metadata.
    /// This is used to ignore duplicate items specified by users since NuGet needs to only consider the first one specified.
    /// </summary>
    internal sealed class ProjectItemInstanceEvaluatedIncludeComparer : IEqualityComparer<IMSBuildItem>
    {
        /// <summary>
        /// A singleton to be used by callers.
        /// </summary>
        public static readonly ProjectItemInstanceEvaluatedIncludeComparer Instance = new ProjectItemInstanceEvaluatedIncludeComparer();

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectItemInstanceEvaluatedIncludeComparer" /> class.  This is private because callers should use
        /// the <see cref="Instance" /> singleton rather than create new instances of this object.
        /// </summary>
        private ProjectItemInstanceEvaluatedIncludeComparer()
        {
        }

        /// <summary>
        /// Determines whether or not the two <see cref="ProjectItemInstance" /> objects are considered equal.
        /// </summary>
        /// <param name="x">The first <see cref="ProjectItemInstance" /> to compare, or <code>null</code>.</param>
        /// <param name="y">The second <see cref="ProjectItemInstance" /> to compare, or <code>null</code>.</param>
        /// <returns><code>true</code> if the specified <see cref="ProjectItemInstance" /> objects have the same Include value, otherwise <code>false</code>.</returns>
        public bool Equals(IMSBuildItem x, IMSBuildItem y)
        {
            return string.Equals(x?.Identity, y?.Identity, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(IMSBuildItem obj) => StringComparer.OrdinalIgnoreCase.GetHashCode(obj.Identity);
    }
}

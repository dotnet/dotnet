// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using Microsoft.VisualStudio.ProjectSystem;

namespace NuGet.VisualStudio.SolutionExplorer
{
    internal static class IProjectTreeExtensions
    {
        /// <summary>
        /// Finds the first child node having <paramref name="flags"/>, or <see langword="null"/> if no child matches.
        /// </summary>
        internal static IProjectTree? FindChildWithFlags(this IProjectTree self, ProjectTreeFlags flags)
        {
            foreach (IProjectTree child in self.Children)
            {
                if (child.Flags.Contains(flags))
                {
                    return child;
                }
            }

            return null;
        }
    }
}

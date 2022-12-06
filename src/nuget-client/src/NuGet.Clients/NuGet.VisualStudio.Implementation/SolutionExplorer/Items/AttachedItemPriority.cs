// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using Microsoft.Internal.VisualStudio.PlatformUI;

namespace NuGet.VisualStudio.SolutionExplorer
{
    /// <summary>
    /// Specifies the order of attached items in the dependencies tree.
    /// </summary>
    /// <remarks>
    /// Used in conjunction with <see cref="IPrioritizedComparable"/>.
    /// </remarks>
    internal static class AttachedItemPriority
    {
        // Not all of these can be siblings.

        public const int Diagnostic = 100;
        public const int Package = 200;
        public const int Project = 300;
        public const int DocumentGroup = 350;
        public const int CompileTimeAssemblyGroup = 400;
        public const int FrameworkAssemblyGroup = 500;
        public const int ContentFilesGroup = 600;
        public const int PackageBuildFileGroup = 700;
        public const int PackageBuildMultiTargetingFileGroup = 800;
    }
}

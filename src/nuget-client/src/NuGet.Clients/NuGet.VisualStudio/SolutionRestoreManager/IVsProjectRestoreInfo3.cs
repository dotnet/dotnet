// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System.Collections.Generic;

namespace NuGet.SolutionRestoreManager
{
    /// <summary>
    /// Contains project metadata needed for project restore operation.
    /// </summary>
    public interface IVsProjectRestoreInfo3
    {
        /// <summary>
        /// The MSBuildProjectExtensionsPath of the project, where NuGet will write outputs, such as the assets file.
        /// </summary>
        string MSBuildProjectExtensionsPath { get; }

        /// <summary>
        /// A collection of all the target frameworks that the project defines.
        /// </summary>
        IReadOnlyList<IVsTargetFrameworkInfo4> TargetFrameworks { get; }

        /// <summary>
        /// Collection of DotnetCliToolReference items.
        /// </summary>
        /// <remarks>
        /// This was a feature added to .NET Core 1.0 and removed in .NET Core 3.0. But VS and the .NET SDK still support building these projects.
        /// </remarks>
        IReadOnlyList<IVsReferenceItem2>? ToolReferences { get; }

        /// <summary>
        /// Original raw value of TargetFrameworks property as set in a project file.
        /// Should be null if the project uses singular TargetFramework rather than plural TargetFrameworks.
        /// </summary>
        string? OriginalTargetFrameworks { get; }
    }
}

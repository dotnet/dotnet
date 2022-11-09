// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.InteropServices;

namespace NuGet.SolutionRestoreManager
{
    /// <summary>
    /// Contains project metadata needed for project restore operation.
    /// </summary>
    [ComImport]
    [Guid("ab43992d-b977-436d-84c2-e76aeed20de2")]
    public interface IVsProjectRestoreInfo
    {
        /// <summary>
        /// The MSBuildProjectExtensionsPath of the project (originally BaseIntermediateOutputPath was used,
        /// but rather than create a new interface, we changed the meaning of this property).
        /// </summary>
        string BaseIntermediatePath { get; }

        /// <summary>
        /// Target frameworks metadata.
        /// </summary>
        IVsTargetFrameworks TargetFrameworks { get; }

        /// <summary>
        /// Collection of tool references.
        /// </summary>
        IVsReferenceItems ToolReferences { get; }

        /// <summary>
        /// Original raw value of TargetFrameworks property as set in a project file.
        /// </summary>
        string OriginalTargetFrameworks { get; }
    }
}

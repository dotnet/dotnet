// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace NuGet.SolutionRestoreManager
{
    /// <summary>
    /// Represents a collection of target framework metadata items
    /// </summary>
    [ComImport]
    [Guid("65f2e8c0-4e5d-4d50-945a-334987decc75")]
    public interface IVsTargetFrameworks : IEnumerable
    {
        /// <summary>
        /// Total count of references in container.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Retrieves a reference by name or index.
        /// </summary>
        /// <param name="index">Reference name or index.</param>
        /// <returns>Reference item matching index.</returns>
        IVsTargetFrameworkInfo? Item(object index);
    }
}

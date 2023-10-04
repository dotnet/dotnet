// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace NuGet.SolutionRestoreManager
{
    /// <summary>
    /// Represents a collection of target framework metadata items
    /// </summary>
    [ComImport]
    [Guid("0C9117CB-828D-4E16-B73F-FEEA9BD6A027")]
    public interface IVsTargetFrameworks2 : IEnumerable
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
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="index" /> is <see langword="null" />.</exception>
        IVsTargetFrameworkInfo2 Item(object index);
    }
}

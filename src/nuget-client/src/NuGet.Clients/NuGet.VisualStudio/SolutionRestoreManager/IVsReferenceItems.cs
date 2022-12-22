// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace NuGet.SolutionRestoreManager
{
    /// <summary>
    /// Collection of restore reference items.
    /// </summary>
    [ComImport]
    [Guid("d012bd06-67b0-47ce-bdfd-bcfca90741eb")]
    public interface IVsReferenceItems : IEnumerable
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
        IVsReferenceItem Item(object index);
    }
}

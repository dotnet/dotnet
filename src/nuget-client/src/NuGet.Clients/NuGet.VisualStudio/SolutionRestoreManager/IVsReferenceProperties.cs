// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace NuGet.SolutionRestoreManager
{
    /// <summary>
    /// Represents a collection of reference properties.
    /// </summary>
    [ComImport]
    [Guid("29f7a567-9957-43fe-b45d-6ef69049742a")]
    public interface IVsReferenceProperties : IEnumerable
    {
        /// <summary>
        /// Total count of properties in container.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Retrieves a property by name or index.
        /// </summary>
        /// <param name="index">Property name or index.</param>
        /// <returns>Property matching index.</returns>
        IVsReferenceProperty Item(object index);
    }
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace NuGet.SolutionRestoreManager
{
    /// <summary>
    /// Represents a collection of project properties.
    /// </summary>
    [ComImport]
    [Guid("8ba829f1-0271-40a7-a098-21c518b8148b")]
    public interface IVsProjectProperties : IEnumerable
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
        IVsProjectProperty? Item(object index);
    }
}

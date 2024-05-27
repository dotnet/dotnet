// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Runtime.InteropServices;

namespace NuGet.SolutionRestoreManager
{
    /// <summary>
    /// Represents a property as a key-value pair
    /// </summary>
    [ComImport]
    [Guid("1513778e-10b7-411e-a4d4-58dcbe51a9a5")]
    public interface IVsReferenceProperty
    {
        /// <summary>
        /// Property name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Property value.
        /// </summary>
        string? Value { get; }
    }
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.InteropServices;

namespace NuGet.SolutionRestoreManager
{
    /// <summary>
    /// Represents a property as a key-value pair
    /// </summary>
    [ComImport]
    [Guid("28954114-b5b5-40c4-8ca3-c983e1429960")]
    public interface IVsProjectProperty
    {
        /// <summary>
        /// Property name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Property value.
        /// </summary>
        string Value { get; }
    }
}

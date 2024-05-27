// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Runtime.InteropServices;

namespace NuGet.SolutionRestoreManager
{
    /// <summary>
    /// Represents metadata associated with a single reference item, e.g. project or package.
    /// </summary>
    [ComImport]
    [Guid("77ab3429-3e64-40ed-9082-10dc01668de9")]
    public interface IVsReferenceItem
    {
        /// <summary>
        /// Unique reference item name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Collection of reference properties.
        /// </summary>
        IVsReferenceProperties? Properties { get; }
    }
}

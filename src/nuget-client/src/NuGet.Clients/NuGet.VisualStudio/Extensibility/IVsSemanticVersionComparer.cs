// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.InteropServices;

namespace NuGet.VisualStudio
{
    /// <summary>
    /// An interface for comparing two opaque version strings by treating them as NuGet semantic
    /// versions.
    /// </summary>
    [ComImport]
    [Guid("1AE338E2-9120-4D1A-A779-012FEBEF77FA")]
    public interface IVsSemanticVersionComparer
    {
        /// <summary>
        /// Compares two version strings as if they were NuGet semantic version
        /// strings. Returns a number less than zero if <paramref name="versionA"/>
        /// is less than <paramref name="versionB"/>. Returns zero if the two versions 
        /// are equivalent. Returns a number greater than zero if <paramref name="versionA"/>
        /// is greater than <paramref name="versionB"/>.
        /// </summary>
        /// <remarks>This API is free-threaded.</remarks>
        /// <param name="versionA">The first version string.</param>
        /// <param name="versionB">The second version string.</param>
        /// <exception cref="ArgumentNullException">If either version string is null.</exception>
        /// <exception cref="ArgumentException">If either string cannot be parsed.</exception>
        /// <returns>
        /// A standard comparison integer based on the relationship between the
        /// two provided versions.
        /// </returns>
        int Compare(string versionA, string versionB);
    }
}

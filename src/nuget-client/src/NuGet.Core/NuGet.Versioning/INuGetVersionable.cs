// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Versioning
{
    /// <summary>
    /// An item that exposes a NuGetVersion
    /// </summary>
    public interface INuGetVersionable
    {
        /// <summary>
        /// NuGet semantic version
        /// </summary>
        NuGetVersion Version { get; }
    }
}

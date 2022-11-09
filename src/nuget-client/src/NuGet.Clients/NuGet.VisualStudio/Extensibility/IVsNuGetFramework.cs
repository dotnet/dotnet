// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Runtime.InteropServices;

namespace NuGet.VisualStudio
{
    /// <summary>A type that represents the components of a .NET Target Framework Moniker.</summary>
    /// <remarks><see cref="System.Runtime.Versioning.FrameworkName"/> does not support .NET 5 Target Framework Monikers with a platform, but this type does.</remarks>
    [ComImport]
    [Guid("E57318D0-9A4D-443C-87F6-631D7F6B14CF")]
    public interface IVsNuGetFramework
    {
        /// <summary>The framework moniker.</summary>
        string TargetFrameworkMoniker { get; }

        /// <summary>The platform moniker.</summary>
        string TargetPlatformMoniker { get; }

        /// <summary>The platform minimum version.</summary>
        /// <remarks>This property is read by <see cref="IVsFrameworkCompatibility3" />, but will always have a null value when returned from <see cref="IVsFrameworkParser2"/>.</remarks>
        string TargetPlatformMinVersion { get; }
    }
}

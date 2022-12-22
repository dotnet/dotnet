// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.PackageManagement.UI
{
    /// <summary>
    /// Represents possible states of <see cref="PackageItemDeprecationLabel"/> control
    /// </summary>
    public enum PackageItemDeprecationLabelState
    {
        /// <summary>
        /// Implies no Deprecation information
        /// </summary>
        Invisible,

        /// <summary>
        /// There exists Deprecation but no alternative package
        /// </summary>
        Deprecation,

        /// <summary>
        /// Deprecation with alternative package available
        /// </summary>
        AlternativeAvailable,
    }
}

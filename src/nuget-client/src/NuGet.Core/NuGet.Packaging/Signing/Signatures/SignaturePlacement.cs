// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.Packaging.Signing
{
    /// <summary>
    /// Indicates signature placement.
    /// </summary>
    [Flags]
    public enum SignaturePlacement
    {
        /// <summary>
        /// The primary signature.
        /// </summary>
        PrimarySignature = 1 << 0,

        /// <summary>
        /// A countersignature on the primary signature.
        /// </summary>
        Countersignature = 1 << 1,

        /// <summary>
        /// A primary signature or a countersignature.
        /// </summary>
        Any = PrimarySignature | Countersignature
    }
}

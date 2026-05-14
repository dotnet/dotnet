// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.Packaging.Signing
{
    /// <summary>
    /// Indicates the type of signature a verification is targeting
    /// </summary>
    /// <remarks>This target makes no assumption about the placement of the signature.
    /// It only refers to author or repository type of signature.
    /// If a specific placement is needed use the <see cref="SignaturePlacement" /> enum.</remarks>
    [Flags]
    public enum VerificationTarget
    {
        /// <summary>
        /// Don't target any signatures.
        /// </summary>
        None = 0,

        /// <summary>
        /// Target unknown primary signatures.
        /// </summary>
        Unknown = 1 << 1,

        /// <summary>
        /// Target author signatures
        /// </summary>
        Author = 1 << 2,

        /// <summary>
        /// Target repository signatures
        /// </summary>
        Repository = 1 << 3,

        /// <summary>
        /// Target all available signatures.
        /// </summary>
        All = Unknown | Author | Repository
    }
}

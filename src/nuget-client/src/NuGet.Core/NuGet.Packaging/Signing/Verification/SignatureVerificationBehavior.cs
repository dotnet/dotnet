// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Packaging.Signing
{
    public enum SignatureVerificationBehavior
    {
        /// <summary>
        /// Do not verify a signature.
        /// </summary>
        Never,

        /// <summary>
        /// Verify a signature if and only if it exists.
        /// </summary>
        IfExists,

        /// <summary>
        /// Verify a signature if and only if it exists and is necessary
        /// (e.g.:  trust evaluation of another signature depends on it).
        /// </summary>
        IfExistsAndIsNecessary,

        /// <summary>
        /// Verify a signature always.  If the signature is not present, it is an error.
        /// </summary>
        Always
    }
}

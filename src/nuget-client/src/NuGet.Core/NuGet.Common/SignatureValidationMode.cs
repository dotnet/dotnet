// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Common
{
    public enum SignatureValidationMode
    {
        /// <summary>
        /// Relaxed signature verification mode.
        /// Allows unsigned packages, and issues are treated as warnings.
        /// </summary>
        Accept,

        /// <summary>
        /// Strict signature verification mode.
        /// All packages most be signed. Any issue is an error and
        /// allow list of trusted signers must be provided.
        /// </summary>
        Require
    }
}

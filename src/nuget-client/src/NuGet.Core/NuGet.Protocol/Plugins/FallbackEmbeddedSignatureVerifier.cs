// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// A fallback embedded signature verifier for unsupported platforms.
    /// </summary>
    public sealed class FallbackEmbeddedSignatureVerifier : EmbeddedSignatureVerifier
    {
        /// <summary>
        /// Checks if a file has a valid embedded signature.
        /// </summary>
        /// <param name="filePath">The path of a file to be checked.</param>
        /// <returns><see langword="true" /> if the file has a valid signature; otherwise, <see langword="false" />.</returns>
        /// <exception cref="PlatformNotSupportedException">Thrown always.</exception>
        public override bool IsValid(string filePath)
        {
            throw new PlatformNotSupportedException();
        }
    }
}

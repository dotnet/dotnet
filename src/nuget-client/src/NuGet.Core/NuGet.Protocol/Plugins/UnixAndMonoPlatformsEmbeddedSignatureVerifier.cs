// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// Embedded Signature Verifier for the MacOS, Linux and Mono platforms.
    /// </summary>
    public class UnixAndMonoPlatformsEmbeddedSignatureVerifier : EmbeddedSignatureVerifier
    {
        /// <summary>
        /// Checks if a file has a valid embedded signature.
        /// </summary>
        /// <param name="filePath">The path of a file to be checked.</param>
        /// <returns><see langword="true" /> if the file has a valid signature; otherwise, <see langword="false" />.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="filePath" />
        /// is either <see langword="null" /> or an empty string.</exception>
        /// <exception cref="PlatformNotSupportedException">Thrown if the current platform is unsupported.</exception>
        public override bool IsValid(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException(Strings.Argument_Cannot_Be_Null_Or_Empty, nameof(filePath));
            }
            // There's no embedded signature verification on Linux, MacOS and Mono platforms
            return true;
        }
    }
}

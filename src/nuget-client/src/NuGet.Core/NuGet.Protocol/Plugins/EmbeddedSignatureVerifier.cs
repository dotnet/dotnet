// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Common;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// Base class for embedded signature verifiers.
    /// </summary>
    public abstract class EmbeddedSignatureVerifier
    {
        /// <summary>
        /// Checks if a file has a valid embedded signature.
        /// </summary>
        /// <param name="filePath">The path of a file to be checked.</param>
        /// <returns><c>true</c> if the file has a valid signature; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="filePath" />
        /// is either <c>null</c> or an empty string.</exception>
        /// <exception cref="PlatformNotSupportedException">Thrown if the current platform is unsupported.</exception>
        public abstract bool IsValid(string filePath);

        /// <summary>
        /// Creates an embedded signature verifier for the current platform.
        /// </summary>
        /// <returns>An embedded signature verifier.</returns>
        public static EmbeddedSignatureVerifier Create()
        {
            if (RuntimeEnvironmentHelper.IsWindows)
            {
                return new WindowsEmbeddedSignatureVerifier();
            }

            if (RuntimeEnvironmentHelper.IsLinux || RuntimeEnvironmentHelper.IsMacOSX || RuntimeEnvironmentHelper.IsMono)
            {
                return new UnixAndMonoPlatformsEmbeddedSignatureVerifier();
            }

            return new FallbackEmbeddedSignatureVerifier();
        }
    }
}

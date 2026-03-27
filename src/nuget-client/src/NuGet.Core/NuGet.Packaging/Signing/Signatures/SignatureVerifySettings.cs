// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Common;

namespace NuGet.Packaging.Signing
{
    /// <summary>
    /// Settings to customize Signature.Verify behavior.
    /// </summary>
    public sealed class SignatureVerifySettings
    {
        /// <summary>
        /// Allow packages with signatures that do not conform to the specification.
        /// </summary>
        public bool AllowIllegal { get; }

        /// <summary>
        /// Specifies that a signing certificate's chain that chains to an untrusted root is allowed
        /// </summary>
        public bool AllowUntrusted { get; }

        /// <summary>
        /// Specifies that a signing certificate's chain with unknown revocation is allowed.
        /// If set to true, offline revocation is allowed.
        /// </summary>
        public bool AllowUnknownRevocation { get; }

        /// <summary>
        /// Indicates if unknown revocation status should be reported.
        /// </summary>
        public bool ReportUnknownRevocation { get; }

        /// <summary>
        /// Indicates if a signing certificate that chains to an untrusted root should be reported.
        /// </summary>
        public bool ReportUntrustedRoot { get; }

        /// <summary>
        /// Gets how the revocation verification should be performed.
        /// </summary>
        public RevocationMode RevocationMode { get; }

        public SignatureVerifySettings(
            bool allowIllegal,
            bool allowUntrusted,
            bool allowUnknownRevocation,
            bool reportUnknownRevocation,
            bool reportUntrustedRoot,
            RevocationMode revocationMode)
        {
            AllowIllegal = allowIllegal;
            AllowUntrusted = allowUntrusted;
            AllowUnknownRevocation = allowUnknownRevocation;
            ReportUnknownRevocation = reportUnknownRevocation;
            ReportUntrustedRoot = reportUntrustedRoot;
            RevocationMode = revocationMode;
        }

        /// <summary>
        /// Get default settings values for relaxed verification on a signature
        /// </summary>
        public static SignatureVerifySettings Default { get; } = new SignatureVerifySettings(
            allowIllegal: false,
            allowUntrusted: true,
            allowUnknownRevocation: true,
            reportUnknownRevocation: true,
            reportUntrustedRoot: true,
            revocationMode: RevocationMode.Online);
    }
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
#if IS_SIGNING_SUPPORTED
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
#endif
using NuGet.Common;

namespace NuGet.Packaging.Signing
{
    public sealed class AuthorPrimarySignature : PrimarySignature
    {
#if IS_SIGNING_SUPPORTED
        public AuthorPrimarySignature(SignedCms signedCms)
            : base(signedCms, SignatureType.Author)
        {
        }

        public override string FriendlyName => Strings.AuthorPrimarySignatureFriendlyName;

        public override SignatureVerificationSummary Verify(
            Timestamp timestamp,
            SignatureVerifySettings settings,
            HashAlgorithmName fingerprintAlgorithm,
            X509Certificate2Collection certificateExtraStore)
        {
            var issues = new List<SignatureLog>();
            settings = settings ?? SignatureVerifySettings.Default;

            issues.Add(SignatureLog.MinimalLog(Environment.NewLine +
                        string.Format(CultureInfo.CurrentCulture, Strings.SignatureType, Type.ToString())));

            var summary = base.Verify(timestamp, settings, fingerprintAlgorithm, certificateExtraStore);

            return new SignatureVerificationSummary(
                summary.SignatureType,
                summary.Status,
                summary.Flags,
                summary.Timestamp,
                summary.ExpirationTime,
                issues.Concat(summary.Issues));
        }
#endif
    }
}

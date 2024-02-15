// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Packaging.Signing;
using Xunit;

namespace NuGet.Packaging.Test
{
    public class SignatureVerificationStatusFlagsTests
    {
        // This test helps ensure that changes to the SignatureVerificationStatusFlags enum are
        // implemented as expected.  All simple enum members (like NoSignature, NoCertificate,
        // and IntegrityCheckFailed) are included in compound enum members (like Suspect, Illegal,
        // and Untrusted).  Except for simple member NoErrors, no simple enum member should be
        // defined without also being in one of these compound enum members.  This is because
        // SignatureVerificationStatus is derived from SignatureVerificationStatusFlags values.
        [Fact]
        public void EnumDefintion_HasNotChangedUnexpectedly()
        {
            var expectedMembers = new Dictionary<string, int>()
            {
                { "NoErrors", 0 },
                { "NoSignature", 1 << 0 },
                { "NoCertificate", 1 << 1 },
                { "MultipleSignatures", 1 << 2 },
                { "SignatureCheckFailed", 1 << 3 },
                { "SignatureAlgorithmUnsupported", 1 << 4 },
                { "CertificatePublicKeyInvalid", 1 << 5 },
                { "HasLifetimeSigningEku", 1 << 6 },
                { "CertificateValidityInTheFuture", 1 << 7 },
                { "CertificateExpired", 1 << 8 },
                { "HashAlgorithmUnsupported", 1 << 9 },
                { "MessageImprintUnsupportedAlgorithm", 1 << 10 },
                { "IntegrityCheckFailed", 1 << 11 },
                { "ChainBuildingFailure", 1 << 12 },
                { "UnknownRevocation", 1 << 13 },
                { "CertificateRevoked", 1 << 14 },
                { "UntrustedRoot", 1 << 15 },
                { "GeneralizedTimeOutsideValidity", 1 << 16 },
                { "NoValidTimestamp", 1 << 17 },
                { "MultipleTimestamps", 1 << 18 },
                { "UnknownBuildStatus", 1 << 19 },
                { "Suspect", (int)(SignatureVerificationStatusFlags.IntegrityCheckFailed |
                    SignatureVerificationStatusFlags.CertificateRevoked) },
                { "Illegal", (int)(SignatureVerificationStatusFlags.NoCertificate |
                    SignatureVerificationStatusFlags.MultipleSignatures |
                    SignatureVerificationStatusFlags.SignatureCheckFailed |
                    SignatureVerificationStatusFlags.SignatureAlgorithmUnsupported |
                    SignatureVerificationStatusFlags.CertificatePublicKeyInvalid |
                    SignatureVerificationStatusFlags.HasLifetimeSigningEku |
                    SignatureVerificationStatusFlags.CertificateValidityInTheFuture |
                    SignatureVerificationStatusFlags.HashAlgorithmUnsupported |
                    SignatureVerificationStatusFlags.MessageImprintUnsupportedAlgorithm) },
                { "Untrusted", (int)(SignatureVerificationStatusFlags.NoSignature |
                    SignatureVerificationStatusFlags.CertificateExpired |
                    SignatureVerificationStatusFlags.ChainBuildingFailure |
                    SignatureVerificationStatusFlags.UnknownRevocation |
                    SignatureVerificationStatusFlags.UntrustedRoot |
                    SignatureVerificationStatusFlags.GeneralizedTimeOutsideValidity |
                    SignatureVerificationStatusFlags.UnknownBuildStatus) }
            };

            var actualNames = Enum.GetNames(typeof(SignatureVerificationStatusFlags));

            Assert.Equal(expectedMembers.Count, actualNames.Length);

            var actualNotInExpected = actualNames.Except(expectedMembers.Keys);
            var expectedNotInActual = expectedMembers.Keys.Except(actualNames);

            var commonMessage = $"The {nameof(SignatureVerificationStatusFlags)} enum definition has changed unexpectedly.";

            Assert.False(actualNotInExpected.Any(), $"{commonMessage}  Unexpected members found:  {string.Join(", ", actualNotInExpected)}");
            Assert.False(expectedNotInActual.Any(), $"{commonMessage}  Expected members not found:  {string.Join(", ", expectedNotInActual)}");

            foreach (var member in expectedMembers)
            {
                Assert.Equal(member.Value, (int)Enum.Parse(typeof(SignatureVerificationStatusFlags), member.Key));
            }
        }
    }
}

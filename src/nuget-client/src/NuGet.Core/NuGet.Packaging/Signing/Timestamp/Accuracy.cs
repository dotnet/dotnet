// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Security.Cryptography;
using NuGet.Packaging.Signing.DerEncoding;

namespace NuGet.Packaging.Signing
{
    /*
        From RFC 3161 (https://tools.ietf.org/html/rfc3161):

           Accuracy ::= SEQUENCE {
                 seconds        INTEGER              OPTIONAL,
                 millis     [0] INTEGER  (1..999)    OPTIONAL,
                 micros     [1] INTEGER  (1..999)    OPTIONAL  }
    */
    /// <remarks>This is public only to facilitate testing.</remarks>
    public sealed class Accuracy
    {
        public int? Seconds { get; }
        public int? Milliseconds { get; }
        public int? Microseconds { get; }

        private Accuracy(
            int? seconds,
            int? milliseconds,
            int? microseconds)
        {
            Seconds = seconds;
            Milliseconds = milliseconds;
            Microseconds = microseconds;
        }

        public static Accuracy Read(byte[] bytes)
        {
            var reader = DerSequenceReader.CreateForPayload(bytes);

            return Read(reader);
        }

        internal static Accuracy Read(DerSequenceReader reader)
        {
            var accuracyReader = reader.ReadSequence();
            int? seconds = null;
            int? milliseconds = null;
            int? microseconds = null;

            if (accuracyReader.HasTag(DerSequenceReader.DerTag.Integer))
            {
                seconds = accuracyReader.ReadInteger();

                if (seconds < 0)
                {
                    // The ASN.1 definition does not disallow negative numbers
                    // but it doesn't make sense to allow negative numbers.
                    throw new CryptographicException(Strings.InvalidAsn1);
                }
            }

            if (accuracyReader.HasTag(DerSequenceReader.ContextSpecificTagFlag))
            {
                milliseconds = accuracyReader.ReadInteger();

                if (milliseconds < 1 || milliseconds > 999)
                {
                    throw new CryptographicException(Strings.InvalidAsn1);
                }
            }

            if (accuracyReader.HasTag(DerSequenceReader.ContextSpecificTagFlag | 1))
            {
                microseconds = accuracyReader.ReadInteger();

                if (microseconds < 1 || microseconds > 999)
                {
                    throw new CryptographicException(Strings.InvalidAsn1);
                }
            }

            if (accuracyReader.HasData)
            {
                throw new CryptographicException(Strings.InvalidAsn1);
            }

            return new Accuracy(seconds, milliseconds, microseconds);
        }

        public long? GetTotalMicroseconds()
        {
            return Seconds.GetValueOrDefault() * 1_000_000L +
                   Milliseconds.GetValueOrDefault() * 1_000L +
                   Microseconds.GetValueOrDefault();
        }
    }
}

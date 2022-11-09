// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using NuGet.Packaging.Signing.DerEncoding;

namespace NuGet.Packaging.Signing
{
    /*
        NuGetV3ServiceIndexUrl ::= IA5String
    */
    /// <remarks>This is public only to facilitate testing.</remarks>
    public sealed class NuGetV3ServiceIndexUrl
    {
        public Uri V3ServiceIndexUrl { get; }

        public NuGetV3ServiceIndexUrl(Uri v3ServiceIndexUrl)
        {
            if (v3ServiceIndexUrl == null)
            {
                throw new ArgumentNullException(nameof(v3ServiceIndexUrl));
            }

            if (!v3ServiceIndexUrl.IsAbsoluteUri)
            {
                throw new ArgumentException(Strings.InvalidUrl, nameof(v3ServiceIndexUrl));
            }

            if (!string.Equals(v3ServiceIndexUrl.Scheme, "https", StringComparison.Ordinal))
            {
                throw new ArgumentException(Strings.InvalidUrl, nameof(v3ServiceIndexUrl));
            }

            V3ServiceIndexUrl = v3ServiceIndexUrl;
        }

        public static NuGetV3ServiceIndexUrl Read(byte[] bytes)
        {
            var reader = DerSequenceReader.CreateForPayload(bytes);

            return Read(reader);
        }

        internal static NuGetV3ServiceIndexUrl Read(DerSequenceReader reader)
        {
            var urlString = reader.ReadIA5String();

            if (reader.HasData)
            {
                throw new SignatureException(Strings.NuGetV3ServiceIndexUrlInvalid);
            }

            Uri url;

            if (!Uri.TryCreate(urlString, UriKind.Absolute, out url))
            {
                throw new SignatureException(Strings.NuGetV3ServiceIndexUrlInvalid);
            }

            if (!string.Equals(url.Scheme, "https", StringComparison.Ordinal))
            {
                throw new SignatureException(Strings.NuGetV3ServiceIndexUrlInvalid);
            }

            return new NuGetV3ServiceIndexUrl(url);
        }

        public byte[] Encode()
        {
            return DerEncoder.SegmentedEncodeIA5String(V3ServiceIndexUrl.OriginalString.ToCharArray())
                .SelectMany(x => x)
                .ToArray();
        }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Buffers;
using System.Diagnostics;
using System.Text;
using Internal.Cryptography;

namespace System.Security.Cryptography.X509Certificates
{
    internal sealed partial class AppleCertificatePal : ICertificatePal
    {
        internal delegate bool DerCallback(ReadOnlySpan<byte> derData, X509ContentType contentType);

        internal static bool TryDecodePem(ReadOnlySpan<byte> rawData, DerCallback derCallback)
        {
            // If the character is a control character that isn't whitespace, then we're probably using a DER encoding
            // and not using a PEM encoding in ASCII.
            if (char.IsControl((char)rawData[0]) && !char.IsWhiteSpace((char)rawData[0]))
            {
                return false;
            }

            // Look for the PEM marker. This doesn't guarantee it will be a valid PEM since we don't check whether
            // the marker is at the beginning of line or whether the line is a complete marker. It's just a quick
            // check to avoid conversion from bytes to characters if the content is DER encoded.
            if (rawData.IndexOf("-----BEGIN "u8) < 0)
            {
                return false;
            }

            char[] certPem = ArrayPool<char>.Shared.Rent(rawData.Length);
            byte[]? certBytes = null;

            try
            {
                Encoding.ASCII.GetChars(rawData, certPem);

                foreach ((ReadOnlySpan<char> contents, PemFields fields) in new PemEnumerator(certPem.AsSpan(0, rawData.Length)))
                {
                    ReadOnlySpan<char> label = contents[fields.Label];

                    if (label.SequenceEqual(PemLabels.X509Certificate) || label.SequenceEqual(PemLabels.Pkcs7Certificate))
                    {
                        certBytes = CryptoPool.Rent(fields.DecodedDataLength);

                        if (!Convert.TryFromBase64Chars(contents[fields.Base64Data], certBytes, out int bytesWritten)
                            || bytesWritten != fields.DecodedDataLength)
                        {
                            Debug.Fail("The contents should have already been validated by the PEM reader.");
                            throw new CryptographicException(SR.Cryptography_X509_NoPemCertificate);
                        }

                        X509ContentType contentType =
                            label.SequenceEqual(PemLabels.X509Certificate) ?
                            X509ContentType.Cert :
                            X509ContentType.Pkcs7;
                        bool cont = derCallback(certBytes.AsSpan(0, bytesWritten), contentType);

                        byte[] toReturn = certBytes;
                        certBytes = null;
                        CryptoPool.Return(toReturn, clearSize: 0);

                        if (!cont)
                        {
                            return true;
                        }
                    }
                }
            }
            finally
            {
                ArrayPool<char>.Shared.Return(certPem, clearArray: true);

                if (certBytes != null)
                {
                    CryptoPool.Return(certBytes, clearSize: 0);
                }
            }

            return true;
        }
    }
}

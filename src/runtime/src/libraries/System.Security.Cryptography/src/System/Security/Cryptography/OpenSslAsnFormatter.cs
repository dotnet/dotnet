// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text;

using Microsoft.Win32.SafeHandles;

namespace System.Security.Cryptography
{
    internal sealed class OpenSslAsnFormatter : AsnFormatter
    {
        protected override string? FormatNative(Oid? oid, byte[] rawData, bool multiLine)
        {
            if (oid == null || string.IsNullOrEmpty(oid.Value))
            {
                return EncodeSpaceSeparatedHexString(rawData);
            }

            // The established behavior for this method is to return the native answer, if possible,
            // or to return null and let rawData get hex-encoded.  CryptographicException should not
            // be raised.

            bool clearErrors = true;
            try
            {
                using (SafeAsn1ObjectHandle asnOid = Interop.Crypto.ObjTxt2Obj(oid.Value))
                using (SafeAsn1OctetStringHandle octetString = Interop.Crypto.Asn1OctetStringNew())
                {
                    if (asnOid.IsInvalid || octetString.IsInvalid)
                    {
                        return null;
                    }

                    if (!Interop.Crypto.Asn1OctetStringSet(octetString, rawData, rawData.Length))
                    {
                        return null;
                    }

                    using (SafeBioHandle bio = Interop.Crypto.CreateMemoryBio())
                    using (SafeX509ExtensionHandle x509Ext = Interop.Crypto.X509ExtensionCreateByObj(asnOid, false, octetString))
                    {
                        if (bio.IsInvalid || x509Ext.IsInvalid)
                        {
                            return null;
                        }

                        if (!Interop.Crypto.X509V3ExtPrint(bio, x509Ext))
                        {
                            return null;
                        }

                        // X509V3ExtPrint might contaminate the error queue on success, always clear now.
                        Interop.Crypto.ErrClearError();

                        // Errors past here are handled by throws, don't need to double-lock
                        // the success path.
                        clearErrors = false;

                        int printLen = Interop.Crypto.GetMemoryBioSize(bio);

                        // Account for the null terminator that it'll want to write.
                        Span<byte> buffer = new byte[printLen + 1];
                        Span<byte> current = buffer;
                        int total = 0;
                        int read;

                        do
                        {
                            read = Interop.Crypto.BioGets(bio, current);

                            if (read < 0)
                            {
                                throw Interop.Crypto.CreateOpenSslCryptographicException();
                            }

                            current = current.Slice(read);
                            total += read;
                        }
                        while (read > 0);

                        return Encoding.UTF8.GetString(buffer.Slice(0, total));
                    }
                }
            }
            finally
            {
                // All of the return null paths might have errors that we are ignoring.
                if (clearErrors)
                {
                    Interop.Crypto.ErrClearError();
                }
            }
        }
    }
}

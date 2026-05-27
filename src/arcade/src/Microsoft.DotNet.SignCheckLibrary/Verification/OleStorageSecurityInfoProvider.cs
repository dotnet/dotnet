// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security.Cryptography.Pkcs;
using Microsoft.SignCheck.Interop;
using Microsoft.VisualStudio.OLE.Interop;

#pragma warning disable CA1416 // Validate platform compatibility

namespace Microsoft.SignCheck.Verification
{
    /// <summary>
    /// Reads digital signature information from OLE Compound Document files (MSI, MSP).
    /// These files store their Authenticode signature in a stream named "\u0005DigitalSignature".
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class OleStorageSecurityInfoProvider : ISecurityInfoProvider
    {
        // Use \u0005 (not \x05) because \x greedily consumes hex digits,
        // turning \x05D into U+005D (']') instead of U+0005 followed by 'D'.
        private const string DigitalSignatureStreamName = "\u0005DigitalSignature";

        public SignedCms ReadSecurityInfo(string path)
        {
            Guid iidStorage = typeof(IStorage).GUID;
            int hr = Ole32.StgOpenStorageEx(path, STGM.STGM_READ | STGM.STGM_SHARE_EXCLUSIVE,
                Ole32.STGFMT_STORAGE, 0, IntPtr.Zero, IntPtr.Zero, ref iidStorage, out object storageObj);
            IStorage storage = storageObj as IStorage;

            if (hr != StructuredStorage.S_OK || storage == null)
            {
                return null;
            }

            try
            {
                Microsoft.VisualStudio.OLE.Interop.IStream stream;
                storage.OpenStream(DigitalSignatureStreamName, IntPtr.Zero, STGM.STGM_READ | STGM.STGM_SHARE_EXCLUSIVE, 0, out stream);

                if (stream == null)
                {
                    return null;
                }

                try
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        byte[] buffer = new byte[4096];
                        while (true)
                        {
                            stream.Read(buffer, (uint)buffer.Length, out uint bytesRead);
                            if (bytesRead == 0)
                            {
                                break;
                            }
                            memoryStream.Write(buffer, 0, (int)bytesRead);
                        }

                        byte[] signatureBytes = memoryStream.ToArray();
                        if (signatureBytes.Length == 0)
                        {
                            return null;
                        }

                        var signedCms = new SignedCms();
                        signedCms.Decode(signatureBytes);
                        return signedCms;
                    }
                }
                finally
                {
                    Marshal.ReleaseComObject(stream);
                }
            }
            catch (COMException)
            {
                // Stream doesn't exist - file is not signed
                return null;
            }
            finally
            {
                if (storage != null)
                {
                    Marshal.ReleaseComObject(storage);
                }
            }
        }
    }
}

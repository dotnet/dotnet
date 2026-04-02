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
    /// These files store their Authenticode signature in a stream named "\x05DigitalSignature".
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class OleStorageSecurityInfoProvider : ISecurityInfoProvider
    {
        private const string DigitalSignatureStreamName = "\x05DigitalSignature";

        public SignedCms ReadSecurityInfo(string path)
        {
            IStorage storage = null;
            int hr = Ole32.StgOpenStorage(path, null, STGM.STGM_READ | STGM.STGM_SHARE_EXCLUSIVE, IntPtr.Zero, 0, out storage);

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

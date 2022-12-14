// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Threading;
using Internal.Cryptography;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Cryptography.X509Certificates
{
    internal sealed partial class StorePal
    {
        private sealed class AndroidCertLoader : ILoaderPal
        {
            private ICertificatePal[]? _certs;

            public AndroidCertLoader(SafeX509Handle[] certHandles)
            {
                _certs = new ICertificatePal[certHandles.Length];
                for (int i = 0; i < certHandles.Length; i++)
                {
                    SafeX509Handle handle = certHandles[i];
                    Debug.Assert(!handle.IsInvalid);
                    _certs[i] = AndroidCertificatePal.FromHandle(handle.DangerousGetHandle());
                }
            }

            public AndroidCertLoader(ICertificatePal[] certs)
            {
                _certs = certs;
            }

            public void Dispose()
            {
                _certs?.DisposeAll();
            }

            public void MoveTo(X509Certificate2Collection collection)
            {
                ICertificatePal[]? certs = Interlocked.Exchange(ref _certs, null);
                Debug.Assert(certs != null);

                foreach (ICertificatePal cert in certs)
                {
                    collection.Add(new X509Certificate2(cert));
                }
            }
        }
    }
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;

namespace NuGet.Packaging.Signing
{
    internal static class CmsFactory
    {
        internal static ICms Create(byte[] cmsBytes)
        {
            if (cmsBytes == null)
            {
                throw new ArgumentNullException(nameof(cmsBytes));
            }
#if IS_SIGNING_SUPPORTED
            ICms cms = null;
#if IS_DESKTOP
            NativeCms nativeCms = NativeCms.Decode(cmsBytes);
            cms = new NativeCmsWrapper(nativeCms);
#else
            SignedCms signedCms = new SignedCms();
            signedCms.Decode(cmsBytes);
            cms = new ManagedCmsWrapper(signedCms);
#endif
            return cms;
#else
            throw new NotSupportedException();
#endif
        }
    }
}

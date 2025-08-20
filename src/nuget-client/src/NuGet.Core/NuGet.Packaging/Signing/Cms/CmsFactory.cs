// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

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
#if IS_DESKTOP
            NativeCms nativeCms = NativeCms.Decode(cmsBytes);
            return new NativeCmsWrapper(nativeCms);
#else
            System.Security.Cryptography.Pkcs.SignedCms signedCms = new System.Security.Cryptography.Pkcs.SignedCms();
            signedCms.Decode(cmsBytes);
            return new ManagedCmsWrapper(signedCms);
#endif
        }
    }
}

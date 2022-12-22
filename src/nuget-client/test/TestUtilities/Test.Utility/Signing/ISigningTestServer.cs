// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Test.Utility.Signing
{
    public interface ISigningTestServer
    {
        Uri Url { get; }

#if IS_SIGNING_SUPPORTED
        IDisposable RegisterResponder(IHttpResponder responder);
#endif
    }
}

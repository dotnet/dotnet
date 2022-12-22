// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Net;

namespace Test.Utility.Signing
{
    public interface IHttpResponder
    {
        Uri Url { get; }

#if IS_SIGNING_SUPPORTED
        void Respond(HttpListenerContext context);
#endif
    }
}

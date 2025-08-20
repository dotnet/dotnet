// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#pragma warning disable CS1591

using System;

using System.Net;

namespace Microsoft.Internal.NuGet.Testing.SignedPackages
{
    public interface IHttpResponder
    {
        Uri Url { get; }

        void Respond(HttpListenerContext context);
    }
}

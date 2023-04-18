// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Security.Cryptography.X509Certificates;
using NuGet.Common;

namespace NuGet.Packaging.Signing
{
    internal interface IX509Chain : IDisposable
    {
        ILogMessage AdditionalContext { get; }

        /// <summary>
        /// This exists purely to avoid breaking existing public API's which require an <see cref="X509Chain" /> instance.
        /// Internally, we should be cautious about using <see cref="PrivateReference" />.
        /// Calling X509Chain.Build(...) directly (vs. IX509Chain.Build(...)) will break <see cref="AdditionalContext" />.
        /// Calling any other X509Chain member is safe.
        /// </summary>
        X509Chain PrivateReference { get; }
        X509ChainElementCollection ChainElements { get; }
        X509ChainPolicy ChainPolicy { get; }
        X509ChainStatus[] ChainStatus { get; }

        bool Build(X509Certificate2 certificate);
    }
}

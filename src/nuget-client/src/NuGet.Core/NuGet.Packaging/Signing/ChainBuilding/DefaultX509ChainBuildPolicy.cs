// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Security.Cryptography.X509Certificates;

namespace NuGet.Packaging.Signing
{
    internal sealed class DefaultX509ChainBuildPolicy : IX509ChainBuildPolicy
    {
        internal static IX509ChainBuildPolicy Instance { get; } = new DefaultX509ChainBuildPolicy();

        private DefaultX509ChainBuildPolicy() { }

        public bool Build(IX509Chain chain, X509Certificate2 certificate)
        {
            if (chain is null)
            {
                throw new ArgumentNullException(nameof(chain));
            }

            if (certificate is null)
            {
                throw new ArgumentNullException(nameof(certificate));
            }

            return chain.Build(certificate);
        }
    }
}

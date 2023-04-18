// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Security.Cryptography.X509Certificates;
using NuGet.Common;

namespace NuGet.Packaging.Signing
{
    internal sealed class X509ChainWrapper : IX509Chain
    {
        private readonly X509Chain _chain;
        private readonly Func<X509Chain, ILogMessage> _getAdditionalContext;

        public ILogMessage AdditionalContext { get; private set; }
        public X509ChainElementCollection ChainElements => _chain.ChainElements;
        public X509ChainPolicy ChainPolicy => _chain.ChainPolicy;
        public X509ChainStatus[] ChainStatus => _chain.ChainStatus;
        public X509Chain PrivateReference => _chain;

        internal X509ChainWrapper(X509Chain chain)
            : this(chain, getAdditionalContext: null)
        {
        }

        internal X509ChainWrapper(X509Chain chain, Func<X509Chain, ILogMessage> getAdditionalContext)
        {
            if (chain is null)
            {
                throw new ArgumentNullException(nameof(chain));
            }

            _chain = chain;
            _getAdditionalContext = getAdditionalContext;
        }

        public bool Build(X509Certificate2 certificate)
        {
            if (certificate is null)
            {
                throw new ArgumentNullException(nameof(certificate));
            }

            bool result = _chain.Build(certificate);

            if (!result && _getAdditionalContext is not null)
            {
                AdditionalContext = _getAdditionalContext(_chain);
            }

            return result;
        }

        public void Dispose()
        {
            _chain.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}

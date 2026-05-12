// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.Configuration;

namespace NuGet.Packaging.Signing
{
    public interface ITrustedSignersProvider
    {
        /// <summary>
        /// Get a list of all the trusted signer entries under the computer trusted signers section.
        /// </summary>
        IReadOnlyList<TrustedSignerItem> GetTrustedSigners();

        /// <summary>
        /// Adds a new trusted signer or updates an existing one in the settings.
        /// </summary>
        /// <param name="trustedSigners">Trusted signers to be added or updated</param>
        void AddOrUpdateTrustedSigner(TrustedSignerItem trustedSigner);

        /// <summary>
        /// Removes trusted signers from the settings.
        /// </summary>
        /// <param name="trustedSigners">Trusted signers to be removed</param>
        void Remove(IReadOnlyList<TrustedSignerItem> trustedSigners);
    }
}

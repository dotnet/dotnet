// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Security.Cryptography;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Provides extensibility for cryptographic operators.
    /// If custom operators are needed for then <see cref="CryptoProviderFactory.CustomCryptoProvider"/> can be set to
    /// return these operators. <see cref="CryptoProviderFactory.CustomCryptoProvider"/> will be before each creation.
    /// </summary>
    public interface ICryptoProvider
    {
        /// <summary>
        /// Called to determine if a cryptographic operation is supported.
        /// </summary>
        /// <param name="algorithm">the algorithm that defines the cryptographic operator.</param>
        /// <param name="args">the arguments required by the cryptographic operator. May be null.</param>
        /// <returns>true if supported</returns>
        bool IsSupportedAlgorithm(string algorithm, params object[] args);

        /// <summary>
        /// returns a cryptographic operator that supports the algorithm.
        /// </summary>
        /// <param name="algorithm">the algorithm that defines the cryptographic operator.</param>
        /// <param name="args">the arguments required by the cryptographic operator. May be null.</param>
        /// <remarks>call <see cref="ICryptoProvider.Release(object)"/> when finished with the object.</remarks>
        object Create(string algorithm, params object[] args);

        /// <summary>
        /// called to release the object returned from <see cref="ICryptoProvider.Create(string, object[])"/>
        /// </summary>
        /// <param name="cryptoInstance">the object returned from <see cref="ICryptoProvider.Create(string, object[])"/>.</param>
        void Release(object cryptoInstance);
    }
}

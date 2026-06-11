// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Provides extensibility for cryptographic operators.
    /// If custom operators are needed, <see cref="CryptoProviderFactory.CustomCryptoProvider"/> can be set to return these operators. 
    /// This property will be checked before each creation.
    /// </summary>
    public interface ICryptoProvider
    {
        /// <summary>
        /// Determines if a cryptographic operation is supported.
        /// </summary>
        /// <param name="algorithm">The algorithm that defines the cryptographic operator.</param>
        /// <param name="args">The arguments required by the cryptographic operator. May be null.</param>
        /// <returns><see langword="true"/> if the algorithm is supported; otherwise, <see langword="false"/>.</returns>
        bool IsSupportedAlgorithm(string algorithm, params object[] args);

        /// <summary>
        /// Returns a cryptographic operator that supports the specified algorithm.
        /// </summary>
        /// <param name="algorithm">The algorithm that defines the cryptographic operator.</param>
        /// <param name="args">The arguments required by the cryptographic operator. May be null.</param>
        /// <returns>An object representing the cryptographic operator.</returns>
        /// <remarks>Call <see cref="ICryptoProvider.Release(object)"/> when finished with the object.</remarks>
        object Create(string algorithm, params object[] args);

        /// <summary>
        /// Releases the object returned from <see cref="ICryptoProvider.Create(string, object[])"/>.
        /// </summary>
        /// <param name="cryptoInstance">The object returned from <see cref="ICryptoProvider.Create(string, object[])"/>.</param>
        void Release(object cryptoInstance);
    }
}

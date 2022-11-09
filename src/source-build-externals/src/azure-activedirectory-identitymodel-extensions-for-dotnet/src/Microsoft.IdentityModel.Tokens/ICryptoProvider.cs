//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

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

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

using System;
using System.IO;
using Microsoft.IdentityModel.Tokens;
using static Microsoft.IdentityModel.Logging.LogHelper;
using static Microsoft.IdentityModel.Xml.XmlUtil;

namespace Microsoft.IdentityModel.Xml
{
    /// <summary>
    /// Represents a XmlDsig Signature element as per: https://www.w3.org/TR/2001/PR-xmldsig-core-20010820/#sec-Signature
    /// </summary>
    public class Signature : DSigElement
    {
        private string _signatureValue;
        private SignedInfo _signedInfo;

        /// <summary>
        /// Initializes a <see cref="Signature"/> instance.
        /// </summary>
        public Signature()
        {
        }

        /// <summary>
        /// Initializes a <see cref="Signature"/> instance.
        /// </summary>
        /// <param name="signedInfo">associated with this Signature.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="signedInfo"/> if null.</exception>
        public Signature(SignedInfo signedInfo)
        {
            SignedInfo = signedInfo;
        }

        /// <summary>
        /// Gets or sets the KeyInfo
        /// </summary>
        public KeyInfo KeyInfo
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the SignatureValue
        /// </summary>
        /// <exception cref="ArgumentNullException">if 'value' is null or empty.</exception>
        public string SignatureValue
        {
            get => _signatureValue;
            set => _signatureValue = string.IsNullOrEmpty(value) ? throw LogArgumentNullException(nameof(value)) : value;
        }

        /// <summary>
        /// Gets or sets the <see cref="SignedInfo"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException">if 'value' is null.</exception>
        public SignedInfo SignedInfo
        {
            get => _signedInfo;
            set => _signedInfo = value ?? throw LogArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Verifies the signature over the SignedInfo.
        /// </summary>
        /// <param name="key">the <see cref="SecurityKey"/> to use for cryptographic operations.</param>
        /// <exception cref="ArgumentNullException"> if <paramref name="key"/> is null.</exception>
        /// <exception cref="XmlValidationException"> if <see cref="SignedInfo"/> null.</exception>
        /// <exception cref="XmlValidationException"> if <see cref="SignedInfo.SignatureMethod"/> is not supported.</exception>
        /// <exception cref="XmlValidationException"> if signature does not validate.</exception>
        public void Verify(SecurityKey key)
        {
            if (key == null)
                throw LogArgumentNullException(nameof(key));

            Verify(key, key.CryptoProviderFactory);
        }

        /// <summary>
        /// Verifies the signature over the SignedInfo.
        /// </summary>
        /// <param name="key">the <see cref="SecurityKey"/> to use for cryptographic operations.</param>
        /// <param name="cryptoProviderFactory">the <see cref="CryptoProviderFactory"/> to obtain cryptographic operators.</param>
        /// <exception cref="ArgumentNullException"> if <paramref name="key"/> is null.</exception>
        /// <exception cref="ArgumentNullException"> if <paramref name="cryptoProviderFactory"/> is null.</exception>
        /// <exception cref="XmlValidationException"> if <see cref="SignedInfo"/> null.</exception>
        /// <exception cref="XmlValidationException"> if <see cref="SignedInfo.SignatureMethod"/> is not supported.</exception>
        /// <exception cref="XmlValidationException"> if signature does not validate.</exception>
        public void Verify(SecurityKey key, CryptoProviderFactory cryptoProviderFactory)
        {
            if (key == null)
                throw LogArgumentNullException(nameof(key));

            if (cryptoProviderFactory == null)
                throw LogArgumentNullException(nameof(cryptoProviderFactory));

            if (SignedInfo == null)
                throw LogValidationException(LogMessages.IDX30212);

            if (!cryptoProviderFactory.IsSupportedAlgorithm(SignedInfo.SignatureMethod, key))
                throw LogValidationException(LogMessages.IDX30207, SignedInfo.SignatureMethod, cryptoProviderFactory.GetType());

            var signatureProvider = cryptoProviderFactory.CreateForVerifying(key, SignedInfo.SignatureMethod);
            if (signatureProvider == null)
                throw LogValidationException(LogMessages.IDX30203, cryptoProviderFactory, key, SignedInfo.SignatureMethod);

            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    SignedInfo.GetCanonicalBytes(memoryStream);
                    if (!signatureProvider.Verify(memoryStream.ToArray(), Convert.FromBase64String(SignatureValue)))
                        throw LogValidationException(LogMessages.IDX30200, cryptoProviderFactory, key);
                }

                SignedInfo.Verify(cryptoProviderFactory);
            }
            finally
            {
                if (signatureProvider != null)
                    cryptoProviderFactory.ReleaseSignatureProvider(signatureProvider);
            }
        }
    }
}

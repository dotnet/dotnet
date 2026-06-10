// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// An <see cref="X509EncryptingCredentials"/> designed to construct <see cref="EncryptingCredentials"/> based on a x509 certificate.
    /// </summary>
    public class X509EncryptingCredentials : EncryptingCredentials
    {
        /// <summary>
        /// Designed to construct <see cref="EncryptingCredentials"/> based on a x509 certificate.
        /// </summary>
        /// <param name="certificate">A <see cref="X509Certificate2"/></param>
        /// <remarks>
        /// <see cref="SecurityAlgorithms.DefaultAsymmetricKeyWrapAlgorithm"/> will be used as the key wrap algorithm
        /// <see cref="SecurityAlgorithms.DefaultSymmetricEncryptionAlgorithm"/> will be used as the data encryption algorithm
        /// </remarks>
        /// <exception cref="ArgumentNullException">if 'certificate' is null.</exception>
        public X509EncryptingCredentials(X509Certificate2 certificate)
            : this(certificate, GetEncryptionAlgorithm(), SecurityAlgorithms.DefaultSymmetricEncryptionAlgorithm)
        {
        }

        /// <summary>
        /// Designed to construct <see cref="EncryptingCredentials"/> based on the x509 certificate, a key wrap algorithm, and data encryption algorithm.
        /// </summary>
        /// <param name="certificate">A <see cref="X509Certificate2"/></param>
        /// <param name="keyWrapAlgorithm">A key wrap algorithm</param>
        /// <param name="dataEncryptionAlgorithm">Data encryption algorithm</param>
        /// <exception cref="ArgumentNullException">if 'certificate' is null.</exception>
        /// <exception cref="ArgumentNullException">if 'keyWrapAlgorithm' is null or empty.</exception>
        /// <exception cref="ArgumentNullException">if 'dataEncryptionAlgorithm' is null or empty.</exception>
        public X509EncryptingCredentials(X509Certificate2 certificate, string keyWrapAlgorithm, string dataEncryptionAlgorithm)
            : base(certificate, keyWrapAlgorithm, dataEncryptionAlgorithm)
        {
            Certificate = certificate;
        }

        /// <summary>
        /// Gets the <see cref="X509Certificate2"/> used by this instance.
        /// </summary>
        public X509Certificate2 Certificate
        {
            get;
            private set;
        }

        private static string GetEncryptionAlgorithm()
        {
            return AppContextSwitches.ShouldUseShortNameForRsaOaepKey ? SecurityAlgorithms.RsaOAEP : SecurityAlgorithms.DefaultAsymmetricKeyWrapAlgorithm;
        }
    }
}

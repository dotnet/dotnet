// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Xml
{
    /// <summary>
    /// Represents a XmlDsig X509Data element as per:  https://www.w3.org/TR/2001/PR-xmldsig-core-20010820/#sec-X509Data
    /// </summary>
    /// <remarks> Supports multiple certificates. </remarks>
    public class X509Data
    {
        /// <summary>
        /// Initializes an instance of <see cref="X509Data"/>.
        /// </summary>
        public X509Data()
        {
        }

        /// <summary>
        /// Initializes an instance of <see cref="X509Data"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException">If 'certificate' is null</exception>
        public X509Data(X509Certificate2 certificate)
        {
            if (certificate != null)
                Certificates.Add(Convert.ToBase64String(certificate.RawData));
            else
                throw LogHelper.LogExceptionMessage(new ArgumentNullException(nameof(certificate)));
        }

        /// <summary>
        /// Initializes an instance of <see cref="X509Data"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException">If 'certificates' is null</exception>
        public X509Data(IEnumerable<X509Certificate2> certificates)
        {
            if (certificates != null)
            {
                foreach (var certificate in certificates)
                {
                    if (certificate != null)
                        Certificates.Add(Convert.ToBase64String(certificate.RawData));
                }
            }
            else
            {
                throw LogHelper.LogExceptionMessage(new ArgumentNullException(nameof(certificates)));
            }
        }

        /// <summary>
        /// Gets or sets the 'X509IssuerSerial' that is part of a 'X509Data'.
        /// </summary>
        public IssuerSerial IssuerSerial
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the 'X509SKI' value that is a part of 'X509Data'.
        /// </summary>
        public string SKI
        {
            get;
            set;
        }

        /// <summary>
        /// Get or sets the 'X509SubjectName' value that is a part of 'X509Data'.
        /// </summary>
        public string SubjectName
        {
            get;
            set;
        }

        /// <summary>
        /// Get the collection of X509Certificates that is associated with 'X509Data'.
        /// </summary>
        public ICollection<string> Certificates { get; } = new Collection<string>();

        /// <summary>
        /// Get or sets the 'CRL' value that is a part of 'X509Data'.
        /// </summary>
        public string CRL
        {
            get;
            set;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is X509Data data &&
                EqualityComparer<IssuerSerial>.Default.Equals(IssuerSerial, data.IssuerSerial) &&
                string.Equals(SKI, data.SKI, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(SubjectName, data.SubjectName, StringComparison.OrdinalIgnoreCase) &&
                Enumerable.SequenceEqual(Certificates.OrderBy(t => t), data.Certificates.OrderBy(t => t)) &&
                string.Equals(CRL, data.CRL, StringComparison.OrdinalIgnoreCase);
        }
        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                // Certificates is the only immutable property
                return 794516417 + EqualityComparer<ICollection<string>>.Default.GetHashCode(Certificates);
            }
        }
    }
}

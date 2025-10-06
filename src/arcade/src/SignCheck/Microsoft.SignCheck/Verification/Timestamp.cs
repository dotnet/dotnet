// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Microsoft.SignCheck.Verification
{
    public class Timestamp
    {
        public DateTime EffectiveDate
        {
            get;
            set;
        }

        public DateTime ExpiryDate
        {
            get;
            set;
        }

        /// <summary>
        /// True if the file was signed on or after the <see cref="EffectiveDate"/> and on or prior to the <see cref="ExpiryDate"./>
        /// </summary>
        public bool IsValid
        {
            get
            {
                return (SignedOn >= EffectiveDate) && (SignedOn <= ExpiryDate);
            }
        }

        /// <summary>
        /// The algorithm of the signature, e.g. SHA1
        /// </summary>
        public string SignatureAlgorithm
        {
            get;
            set;
        }

        /// <summary>
        /// The local date and time of the signature.
        /// </summary>
        public DateTime SignedOn
        {
            get;
            set;
        }

        /// <summary>
        /// Adds a timestamp detail to the <see cref="SignatureVerificationResult"/>.
        /// </summary>
        public void AddToSignatureVerificationResult(SignatureVerificationResult svr)
        {
            if (IsValid)
            {
                svr.AddDetail(DetailKeys.Misc, SignCheckResources.DetailTimestamp, SignedOn, SignatureAlgorithm);
            }
            else
            {
                if (SignedOn == DateTime.MaxValue || ExpiryDate == DateTime.MinValue || EffectiveDate == DateTime.MaxValue)
                {
                    svr.AddDetail(DetailKeys.Error, SignCheckResources.ErrorInvalidOrMissingTimestamp);
                }
                else
                {
                    svr.AddDetail(DetailKeys.Error, SignCheckResources.DetailTimestampOutisdeCertValidity, SignedOn, EffectiveDate, ExpiryDate);
                }
            }
        }
    }
}

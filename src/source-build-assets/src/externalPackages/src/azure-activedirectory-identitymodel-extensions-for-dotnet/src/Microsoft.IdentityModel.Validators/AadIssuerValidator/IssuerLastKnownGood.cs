// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Validators
{
    /// <summary>
    /// Class representing the last known good for issuer.
    /// </summary>
    internal class IssuerLastKnownGood
    {
        private string _issuer;
        private TimeSpan _lastKnownGoodLifetime;
        private DateTime? _lastKnownGoodConfigFirstUse;

        /// <summary>
        /// Gets or sets the issuer value.
        /// </summary>
        public string Issuer
        {
            get
            {
                return _issuer;
            }
            set
            {
                if (value == null)
                    throw LogHelper.LogArgumentNullException(nameof(value));

                _lastKnownGoodConfigFirstUse = DateTime.UtcNow;
                _issuer = value;
            }
        }

        /// <summary>
        /// Gets or sets the last known good lifetime.
        /// </summary>
        public TimeSpan LastKnownGoodLifetime
        {
            get { return _lastKnownGoodLifetime; }
            set
            {
                if (value < TimeSpan.Zero)
                    throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException(nameof(value), LogHelper.FormatInvariant(LogMessages.IDX40008, value)));

                _lastKnownGoodLifetime = value;
            }
        }

        /// <summary>
        /// Gets an indicator whether the value is still within its lifetime and is valid.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return _lastKnownGoodConfigFirstUse + LastKnownGoodLifetime > DateTime.UtcNow;
            }
        }

    }
}

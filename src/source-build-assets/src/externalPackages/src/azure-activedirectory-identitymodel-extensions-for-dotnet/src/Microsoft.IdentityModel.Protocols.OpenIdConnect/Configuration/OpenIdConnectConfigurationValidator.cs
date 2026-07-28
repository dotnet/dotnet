// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Linq;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect.Configuration
{
    /// <summary>
    /// Defines a class for validating the OpenIdConnectConfiguration by using default policy.
    /// </summary>
    public class OpenIdConnectConfigurationValidator : IConfigurationValidator<OpenIdConnectConfiguration>
    {
        private int _minimumNumberOfKeys = DefaultMinimumNumberOfKeys;

        /// <summary>
        /// 1 is the default minimum number of keys.
        /// </summary>
        private const int DefaultMinimumNumberOfKeys = 1;

        /// <summary>
        /// Validates a OpenIdConnectConfiguration by using current policy.
        /// </summary>
        /// <param name="openIdConnectConfiguration">The OpenIdConnectConfiguration to validate.</param>
        /// <returns>A <see cref="ConfigurationValidationResult"/> that contains validation result.</returns>
        public ConfigurationValidationResult Validate(OpenIdConnectConfiguration openIdConnectConfiguration)
        {
            if (openIdConnectConfiguration == null)
                throw new ArgumentNullException(nameof(openIdConnectConfiguration));

            if (openIdConnectConfiguration.JsonWebKeySet == null || openIdConnectConfiguration.JsonWebKeySet.Keys.Count == 0)
            {
                return new ConfigurationValidationResult
                {
                    ErrorMessage = LogMessages.IDX21817,
                    Succeeded = false
                };
            }

            int numberOfValidKeys = 0;
            for (int i = 0; i < openIdConnectConfiguration.JsonWebKeySet.Keys.Count; i++)
                if (openIdConnectConfiguration.JsonWebKeySet.Keys[i].ConvertedSecurityKey != null)
                    numberOfValidKeys++;

            if (numberOfValidKeys < MinimumNumberOfKeys)
            {
                string convertKeyInfos = string.Join(
                    "\n",
                    openIdConnectConfiguration.JsonWebKeySet.Keys.Where(
                        key => !string.IsNullOrEmpty(key.ConvertKeyInfo))
                    .Select(key => key.Kid.ToString() + ": " + key.ConvertKeyInfo));

                return new ConfigurationValidationResult
                {
                    ErrorMessage = LogHelper.FormatInvariant(
                        LogMessages.IDX21818,
                        LogHelper.MarkAsNonPII(MinimumNumberOfKeys),
                        LogHelper.MarkAsNonPII(numberOfValidKeys),
                        string.IsNullOrEmpty(convertKeyInfos) ? "None" : LogHelper.MarkAsNonPII(convertKeyInfos)),
                    Succeeded = false
                };
            }

            return new ConfigurationValidationResult
            {
                Succeeded = true
            };
        }

        /// <summary>
        /// The minimum number of keys.
        /// </summary>
        public int MinimumNumberOfKeys
        {
            get { return _minimumNumberOfKeys; }
            set
            {
                if (value < DefaultMinimumNumberOfKeys)
                    throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException(nameof(value), LogHelper.FormatInvariant(LogMessages.IDX21816, LogHelper.MarkAsNonPII(DefaultMinimumNumberOfKeys), LogHelper.MarkAsNonPII(value))));

                _minimumNumberOfKeys = value;
            }
        }
    }
}

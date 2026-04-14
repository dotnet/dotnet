// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Linq;
using System.Web;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

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
            var numberOfValidKeys = openIdConnectConfiguration.JsonWebKeySet.Keys.Where(key => key.ConvertedSecurityKey != null).Count();

            if (numberOfValidKeys < MinimumNumberOfKeys)
            {
                var convertKeyInfos = string.Join("\n", openIdConnectConfiguration.JsonWebKeySet.Keys.Where(key => !string.IsNullOrEmpty(key.ConvertKeyInfo)).Select(key => key.Kid.ToString() + ": " + key.ConvertKeyInfo));
                return new ConfigurationValidationResult
                {
                    ErrorMessage = LogHelper.FormatInvariant(
                        LogMessages.IDX21818,
                        LogHelper.MarkAsNonPII(MinimumNumberOfKeys),
                        LogHelper.MarkAsNonPII(numberOfValidKeys),
                        string.IsNullOrEmpty(convertKeyInfos) ? "None" : convertKeyInfos),
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

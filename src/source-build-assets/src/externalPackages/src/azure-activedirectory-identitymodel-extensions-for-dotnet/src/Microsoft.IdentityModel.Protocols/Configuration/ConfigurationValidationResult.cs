// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.ComponentModel;

namespace Microsoft.IdentityModel.Protocols
{
    /// <summary>
    /// Represents the result of validation a <see cref="IConfigurationValidator{T}"/>.
    /// </summary>
    public class ConfigurationValidationResult
    {
        /// <summary>
        /// Gets or sets the error message that occurred during validation of the configuration.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets a bool indicating if the configuration validation was successful.
        /// </summary>
        [DefaultValue(false)]
        public bool Succeeded { get; set; } = false;
    }
}

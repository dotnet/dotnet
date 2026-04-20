// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Protocols
{
    /// <summary>
    /// Interface that defines a policy for validating configuration data.
    /// </summary>
    /// <typeparam name="T">The type of the configuration metadata.</typeparam>
    public interface IConfigurationValidator<T>
    {
        /// <summary>
        /// Validate the retrieved configuration.
        /// </summary>
        /// <param name="configuration">Configuration of type T.</param>
        /// <returns><see cref="ConfigurationValidationResult"/>.</returns>
        ConfigurationValidationResult Validate(T configuration);
    }
}

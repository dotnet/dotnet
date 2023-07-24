// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Protocols
{
    /// <summary>
    /// This type is for users that want a fixed and static Configuration.
    /// In this case, the configuration is obtained and passed to the constructor.
    /// </summary>
    /// <typeparam name="T">must be a class.</typeparam>
    public class StaticConfigurationManager<T> : BaseConfigurationManager, IConfigurationManager<T> where T : class
    { 
        private T _configuration;

        /// <summary>
        /// Initializes an new instance of <see cref="StaticConfigurationManager{T}"/> with a Configuration instance.
        /// </summary>
        /// <param name="configuration">Configuration of type OpenIdConnectConfiguration or OpenIdConnectConfiguration.</param>
        public StaticConfigurationManager(T configuration) : base()
        {
            if (configuration == null)
                throw LogHelper.LogExceptionMessage(new ArgumentNullException(nameof(configuration), LogHelper.FormatInvariant(LogMessages.IDX20000, LogHelper.MarkAsNonPII(nameof(configuration)))));

            _configuration = configuration;
        }

        /// <summary>
        /// Obtains an updated version of Configuration.
        /// </summary>
        /// <param name="cancel"><see cref="CancellationToken"/>.</param>
        /// <returns>Configuration of type T.</returns>
        public Task<T> GetConfigurationAsync(CancellationToken cancel)
        {
            return Task.FromResult(_configuration);
        }

        /// <summary>
        /// Obtains an updated version of Configuration.
        /// </summary>
        /// <param name="cancel"><see cref="CancellationToken"/>.</param>
        /// <returns>Configuration of type T.</returns>
        public override Task<BaseConfiguration> GetBaseConfigurationAsync(CancellationToken cancel)
        {
            return Task.FromResult(_configuration as BaseConfiguration);
        }

        /// <summary>
        /// For the this type, this is a no-op
        /// </summary>
        public override void RequestRefresh()
        {
        }
    }
}

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

using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Threading;
using System.Threading.Tasks;

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
        public StaticConfigurationManager(T configuration)
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

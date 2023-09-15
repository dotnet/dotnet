// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// A plugin's creation result.
    /// </summary>
    public sealed class PluginCreationResult
    {
        /// <summary>
        /// Gets the plugin's operation claims.
        /// </summary>
        public IReadOnlyList<OperationClaim> Claims { get; }

        /// <summary>
        /// Gets a message if <see cref="Plugin" /> is <see langword="null" />; otherwise, <see langword="null" />.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Gets the exception caught.  May be <see langword="null" />.
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Gets a plugin.
        /// </summary>
        public IPlugin Plugin { get; }

        /// <summary>
        /// Gets a plugin multiclient utilities.
        /// </summary>
        public IPluginMulticlientUtilities PluginMulticlientUtilities { get; }

        /// <summary>
        /// Instantiates a new <see cref="PluginCreationResult" /> class.
        /// </summary>
        /// <param name="plugin">A plugin.</param>
        /// <param name="utilities">A plugin multiclient utilities.</param>
        /// <param name="claims">The plugin's operation claims.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="plugin" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="utilities" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="claims" /> is <see langword="null" />.</exception>
        public PluginCreationResult(IPlugin plugin, IPluginMulticlientUtilities utilities, IReadOnlyList<OperationClaim> claims)
        {
            if (plugin == null)
            {
                throw new ArgumentNullException(nameof(plugin));
            }

            if (utilities == null)
            {
                throw new ArgumentNullException(nameof(utilities));
            }

            if (claims == null)
            {
                throw new ArgumentNullException(nameof(claims));
            }

            Plugin = plugin;
            PluginMulticlientUtilities = utilities;
            Claims = claims;
        }

        /// <summary>
        /// Instantiates a new <see cref="PluginCreationResult" /> class.
        /// </summary>
        /// <param name="message">A message why a plugin could not be created.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="message" />
        /// is either <see langword="null" /> or an empty string.</exception>
        public PluginCreationResult(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(message));
            }

            Message = message;
        }

        /// <summary>
        /// Instantiates a new <see cref="PluginCreationResult" /> class.
        /// </summary>
        /// <param name="message">A message why a plugin could not be created.</param>
        /// <param name="exception">An exception.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="message" />
        /// is either <see langword="null" /> or an empty string.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="exception" /> is <see langword="null" />.</exception>
        public PluginCreationResult(string message, Exception exception)
            : this(message)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            Exception = exception;
        }
    }
}

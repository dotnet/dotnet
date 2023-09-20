// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// A plugin discovery result.
    /// </summary>
    public sealed class PluginDiscoveryResult
    {
        /// <summary>
        /// Gets the plugin file.
        /// </summary>
        public PluginFile PluginFile { get; }

        private string _message;

        /// <summary>
        /// Gets a message if <see cref="PluginFile.State" /> is not <see cref="PluginFileState.Valid" />;
        /// otherwise, <see langword="null" />.
        /// </summary>
        public string Message
        {
            get
            {
                if (_message == null)
                {
                    switch (PluginFile.State.Value)
                    {
                        case PluginFileState.Valid:
                            break;

                        case PluginFileState.NotFound:
                            _message = string.Format(
                                CultureInfo.CurrentCulture,
                                Strings.Plugin_FileNotFound,
                                PluginFile.Path);
                            break;

                        case PluginFileState.InvalidFilePath:
                            _message = string.Format(
                                CultureInfo.CurrentCulture,
                                Strings.Plugin_InvalidPluginFilePath,
                                PluginFile.Path);
                            break;

                        case PluginFileState.InvalidEmbeddedSignature:
                            _message = string.Format(
                                CultureInfo.CurrentCulture,
                                Strings.Plugin_InvalidEmbeddedSignature,
                                PluginFile.Path);
                            break;

                        default:
                            throw new NotImplementedException();
                    }
                }
                return _message;
            }
        }

        /// <summary>
        /// Instantiates a new <see cref="PluginDiscoveryResult" /> class.
        /// </summary>
        /// <param name="pluginFile">A plugin file.</param>
        /// <see cref="PluginFileState.Valid" />; otherwise, <see langword="null" />
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="pluginFile" />
        /// is <see langword="null" />.</exception>
        public PluginDiscoveryResult(PluginFile pluginFile)
        {
            PluginFile = pluginFile ?? throw new ArgumentNullException(nameof(pluginFile));
        }
    }
}

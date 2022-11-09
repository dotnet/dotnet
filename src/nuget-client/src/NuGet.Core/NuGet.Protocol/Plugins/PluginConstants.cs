// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// Plugin constants.
    /// </summary>
    public static class PluginConstants
    {
        /// <summary>
        /// Default close timeout for plugins.
        /// </summary>
        public static readonly TimeSpan CloseTimeout = TimeSpan.FromSeconds(2);

        /// <summary>
        /// Default idle timeout for plugins.
        /// </summary>
        public static readonly TimeSpan IdleTimeout = TimeSpan.FromMinutes(1);

        /// <summary>
        /// Default command-line arguments for plugins.
        /// </summary>
        public static readonly IEnumerable<string> PluginArguments = new[] { "-Plugin" };

        /// <summary>
        /// The progress notification interval.
        /// </summary>
        /// <remarks>This value must be less than half of <see cref="RequestTimeout" />.</remarks>
        public static readonly TimeSpan ProgressInterval = TimeSpan.FromSeconds(10);

        /// <summary>
        /// The default request timeout set by an initialize request after handshaking.
        /// </summary>
        public static readonly TimeSpan RequestTimeout = TimeSpan.FromSeconds(30);
    }
}

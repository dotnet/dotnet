// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using Newtonsoft.Json;
using NuGet.Common;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// A request to set the log level.
    /// </summary>
    public sealed class SetLogLevelRequest
    {
        /// <summary>
        /// Gets the log level.
        /// </summary>
        [JsonRequired]
        public LogLevel LogLevel { get; }

        /// <summary>
        /// Initializes a new <see cref="SetLogLevelRequest" /> class.
        /// </summary>
        /// <param name="logLevel">The log level.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="logLevel" /> is an undefined
        /// <see cref="LogLevel" /> value.</exception>
        [JsonConstructor]
        public SetLogLevelRequest(LogLevel logLevel)
        {
            if (!Enum.IsDefined(typeof(LogLevel), logLevel))
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.Plugin_UnrecognizedEnumValue,
                        logLevel),
                    nameof(logLevel));
            }

            LogLevel = logLevel;
        }
    }
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using Newtonsoft.Json;
using NuGet.Common;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// A logging request.
    /// </summary>
    public sealed class LogRequest
    {
        /// <summary>
        /// Gets the logging level for the message.
        /// </summary>
        [JsonRequired]
        public LogLevel LogLevel { get; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        [JsonRequired]
        public string Message { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogRequest" /> class.
        /// </summary>
        /// <param name="logLevel">The logging level for the message.</param>
        /// <param name="message">The message to be logged.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="logLevel" /> is an undefined
        /// <see cref="LogLevel" /> value.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="message" /> is either <see langword="null" />
        /// or an empty string.</exception>
        [JsonConstructor]
        public LogRequest(LogLevel logLevel, string message)
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

            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(message));
            }

            LogLevel = logLevel;
            Message = message;
        }
    }
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Newtonsoft.Json.Linq;
using NuGet.Common;

namespace NuGet.Protocol.Plugins
{
    internal sealed class EnvironmentVariablesLogMessage : PluginLogMessage
    {
        private readonly int? _handshakeTimeout;
        private readonly int? _idleTimeout;
        private readonly int? _requestTimeout;

        internal EnvironmentVariablesLogMessage(DateTimeOffset now, IEnvironmentVariableReader environmentVariableReader = null)
            : base(now)
        {
            var reader = environmentVariableReader ?? EnvironmentVariableWrapper.Instance;

            _handshakeTimeout = Read(reader, EnvironmentVariableConstants.HandshakeTimeout);
            _idleTimeout = Read(reader, EnvironmentVariableConstants.IdleTimeout);
            _requestTimeout = Read(reader, EnvironmentVariableConstants.RequestTimeout);

            // Some variables are not logged:
            //
            //  * EnvironmentVariableConstants.EnableLog:  it will always be true when logs are generated.
            //  * EnvironmentVariableConstants.LogDirectoryPath:  the value may contain PII.
            //  * EnvironmentVariableConstants.PluginPaths:  the value may contain PII.
        }

        public override string ToString()
        {
            var message = new JObject();

            if (_handshakeTimeout.HasValue)
            {
                message.Add("handshake timeout in seconds", _handshakeTimeout.Value);
            }

            if (_idleTimeout.HasValue)
            {
                message.Add("idle timeout in seconds", _idleTimeout.Value);
            }

            if (_requestTimeout.HasValue)
            {
                message.Add("request timeout in seconds", _requestTimeout.Value);
            }

            return ToString("environment variables", message);
        }

        private static int? Read(IEnvironmentVariableReader reader, string variableName)
        {
            var variableValue = reader.GetEnvironmentVariable(variableName);

            if (int.TryParse(variableValue, out var value))
            {
                return value;
            }

            return null;
        }
    }
}

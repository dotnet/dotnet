// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text.Json.Nodes;

namespace NuGet.Protocol.Plugins
{
    internal sealed class CommunicationLogMessage : PluginLogMessage
    {
        private readonly MessageMethod _method;
        private readonly string _requestId;
        private readonly MessageState _state;
        private readonly MessageType _type;

        internal CommunicationLogMessage(DateTimeOffset now, string requestId, MessageMethod method, MessageType type, MessageState state)
            : base(now)
        {
            _requestId = requestId;
            _method = method;
            _type = type;
            _state = state;
        }

        public override string ToString()
        {
            var message = new JsonObject
            {
                ["request ID"] = _requestId,
                ["method"] = _method.ToString(),
                ["type"] = _type.ToString(),
                ["state"] = _state.ToString(),
            };

            return ToString("communication", message);
        }
    }
}

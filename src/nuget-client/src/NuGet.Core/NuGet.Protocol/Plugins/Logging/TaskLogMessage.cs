// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace NuGet.Protocol.Plugins
{
    internal sealed class TaskLogMessage : PluginLogMessage
    {
        private readonly int? _currentTaskId;
        private readonly MessageMethod _method;
        private readonly string _requestId;
        private readonly TaskState _state;
        private readonly MessageType _type;

        internal TaskLogMessage(DateTimeOffset now, string requestId, MessageMethod method, MessageType type, TaskState state)
            : base(now)
        {
            _requestId = requestId;
            _method = method;
            _type = type;
            _state = state;
            _currentTaskId = Task.CurrentId;
        }

        public override string ToString()
        {
            var message = new JObject(
                new JProperty("request ID", _requestId),
                new JProperty("method", _method),
                new JProperty("type", _type),
                new JProperty("state", _state));

            if (_currentTaskId.HasValue)
            {
                message.Add(new JProperty("current task ID", _currentTaskId.Value));
            }

            return ToString("task", message);
        }
    }
}

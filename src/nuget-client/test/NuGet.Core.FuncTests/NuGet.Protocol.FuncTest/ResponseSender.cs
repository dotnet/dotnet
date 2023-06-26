// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Protocol.Plugins;

namespace NuGet.Protocol.FuncTest
{
    internal sealed class ResponseSender : IDisposable
    {
        private bool _isDisposed;
        private readonly ushort _portNumber;
        private readonly BlockingCollection<Response> _responses;

        internal ResponseSender(ushort portNumber)
        {
            _portNumber = portNumber;
            _responses = new BlockingCollection<Response>();
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _responses.Dispose();

                GC.SuppressFinalize(this);

                _isDisposed = true;
            }
        }

        internal Task StartSendingAsync(CancellationToken cancellationToken)
        {
            var localEndPoint = new IPEndPoint(IPAddress.Loopback, _portNumber);

            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                socket.Connect(localEndPoint);

                using (var stream = new NetworkStream(socket, ownsSocket: true))
                using (var streamWriter = new StreamWriter(stream))
                {
                    streamWriter.AutoFlush = true;

                    foreach (var response in _responses.GetConsumingEnumerable(cancellationToken))
                    {
                        var json = Serialize(response);

                        streamWriter.WriteLine(json);
                    }
                }
            }

            return Task.CompletedTask;
        }

        internal Task SendAsync<TPayload>(MessageType type, MessageMethod method, TPayload payload)
            where TPayload : class
        {
            var response = new Response()
            {
                Type = type,
                Method = method,
                Payload = payload == null ? null : JObject.FromObject(payload)
            };

            _responses.Add(response);

            return Task.CompletedTask;
        }

        private static string Serialize(object value)
        {
            using (var stringWriter = new StringWriter())
            using (var jsonWriter = new JsonTextWriter(stringWriter))
            {
                JsonSerializationUtilities.Serialize(jsonWriter, value);

                return stringWriter.ToString();
            }
        }

        private sealed class Response
        {
            [JsonRequired]
            public MessageType Type { get; set; }

            [JsonRequired]
            public MessageMethod Method { get; set; }

            public JObject Payload { get; set; }
        }
    }
}

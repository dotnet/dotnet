// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Protocol.Plugins;

namespace NuGet.Test.TestExtensions.TestablePlugin
{
    internal sealed class ResponseReceiver
    {
        private readonly ushort _portNumber;
        private readonly BlockingCollection<Response> _responses;

        internal ResponseReceiver(ushort portNumber, BlockingCollection<Response> responses)
        {
            _portNumber = portNumber;
            _responses = responses;
        }

        internal Task StartListeningAsync(CancellationToken cancellationToken)
        {
            var localEndPoint = new IPEndPoint(IPAddress.Loopback, _portNumber);

            using (var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);

                var socket = listener.Accept();

                using (var stream = new NetworkStream(socket, ownsSocket: true))
                using (var streamReader = new StreamReader(stream))
                {
                    string text;

                    while ((text = streamReader.ReadLine()) != null)
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        var response = JsonSerializationUtilities.Deserialize<Response>(text);

                        _responses.Add(response, cancellationToken);
                    }
                }
            }

            return Task.CompletedTask;
        }
    }
}

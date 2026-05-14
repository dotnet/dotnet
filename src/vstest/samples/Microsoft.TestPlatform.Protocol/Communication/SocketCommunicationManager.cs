// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.TestPlatform.Protocol
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Facilitates communication using sockets
    /// </summary>
    public class SocketCommunicationManager
    {
        /// <summary>
        /// TCP Listener to host TCP channel and listen
        /// </summary>
        private TcpListener tcpListener;

        /// <summary>
        /// Binary Writer to write to channel stream
        /// </summary>
        private BinaryWriter binaryWriter;

        /// <summary>
        /// Binary reader to read from channel stream
        /// </summary>
        private BinaryReader binaryReader;

        /// <summary>
        /// Serializer for the data objects
        /// </summary>
        private JsonDataSerializer dataSerializer;

        /// <summary>
        /// Event used to maintain client connection state
        /// </summary>
        private ManualResetEvent clientConnectedEvent = new ManualResetEvent(false);

        /// <summary>
        /// Sync object for sending messages
        /// SendMessage over socket channel is NOT thread-safe
        /// </summary>
        private object sendSyncObject = new object();

        /// <summary>
        /// Stream to use read timeout
        /// </summary>
        private NetworkStream stream;

        private Socket socket;

        /// <summary>
        /// The server stream read timeout constant (in microseconds).
        /// </summary>
        private const int StreamReadTimeout = 1000 * 1000;

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketCommunicationManager"/> class.
        /// </summary>
        public SocketCommunicationManager() : this(JsonDataSerializer.Instance)
        {
        }

        internal SocketCommunicationManager(JsonDataSerializer dataSerializer)
        {
            this.dataSerializer = dataSerializer;
        }

        #region ServerMethods

        /// <summary>
        /// Host TCP Socket Server and start listening
        /// </summary>
        /// <returns></returns>
        public int HostServer()
        {
            var endpoint = new IPEndPoint(IPAddress.Loopback, 0);
            this.tcpListener = new TcpListener(endpoint);

            this.tcpListener.Start();
            var portNumber = ((IPEndPoint)this.tcpListener.LocalEndpoint).Port;
            Console.WriteLine("Server started. Listening at port : {0}", portNumber);
            return portNumber;
        }

        /// <summary>
        /// Accepts client async
        /// </summary>
        public async Task AcceptClientAsync()
        {
            if (this.tcpListener != null)
            {
                this.clientConnectedEvent.Reset();

                var client = await this.tcpListener.AcceptTcpClientAsync();
                this.socket = client.Client;
                this.stream = client.GetStream();
                this.binaryReader = new BinaryReader(this.stream);
                this.binaryWriter = new BinaryWriter(this.stream);

                this.clientConnectedEvent.Set();

                Console.WriteLine("Accepted Client request and set the flag");
            }
        }

        /// <summary>
        /// Waits for Client Connection
        /// </summary>
        /// <param name="clientConnectionTimeout">Time to Wait for the connection</param>
        /// <returns>True if Client is connected, false otherwise</returns>
        public bool WaitForClientConnection(int clientConnectionTimeout)
        {
            return this.clientConnectedEvent.WaitOne(clientConnectionTimeout);
        }

        /// <summary>
        /// Stop Listener
        /// </summary>
        public void StopServer()
        {
            this.tcpListener?.Stop();
            this.tcpListener = null;
            this.binaryReader?.Dispose();
            this.binaryWriter?.Dispose();
        }

        #endregion

        /// <summary>
        /// Writes message to the binary writer.
        /// </summary>
        /// <param name="messageType">Type of Message to be sent, for instance TestSessionStart</param>
        public void SendMessage(string messageType)
        {
            var serializedObject = this.dataSerializer.SerializeMessage(messageType);
            this.WriteAndFlushToChannel(serializedObject);
        }

        /// <summary>
        /// Reads message from the binary reader
        /// </summary>
        /// <returns>Returns message read from the binary reader</returns>
        public Message ReceiveMessage()
        {
            var rawMessage = this.ReceiveRawMessage();
            return this.dataSerializer.DeserializeMessage(rawMessage);
        }

        /// <summary>
        ///  Writes message to the binary writer with payload
        /// </summary>
        /// <param name="messageType">Type of Message to be sent, for instance TestSessionStart</param>
        /// <param name="payload">payload to be sent</param>
        public void SendMessage(string messageType, object payload)
        {
            var rawMessage = this.dataSerializer.SerializePayload(messageType, payload);
            this.WriteAndFlushToChannel(rawMessage);
        }

        /// <summary>
        /// The send hand shake message.
        /// </summary>
        public void SendHandShakeMessage()
        {
            this.SendMessage(MessageType.SessionStart);
        }

        /// <summary>
        /// Reads message from the binary reader
        /// </summary>
        /// <returns> Raw message string </returns>
        public string ReceiveRawMessage()
        {
            var rawMessage =  this.binaryReader.ReadString();
            Console.WriteLine("\n=========== Receiving Message ===========");
            Console.WriteLine(rawMessage);
            return rawMessage;
        }

        /// <summary>
        /// Deserializes the Message into actual TestPlatform objects
        /// </summary>
        /// <typeparam name="T"> The type of object to deserialize to. </typeparam>
        /// <param name="message"> Message object </param>
        /// <returns> TestPlatform object </returns>
        public T DeserializePayload<T>(Message message)
        {
            return this.dataSerializer.DeserializePayload<T>(message);
        }

        /// <summary>
        /// Writes the data on socket and flushes the buffer
        /// </summary>
        /// <param name="rawMessage">message to write</param>
        private void WriteAndFlushToChannel(string rawMessage)
        {
            // Writing Message on binarywriter is not Thread-Safe
            // Need to sync one by one to avoid buffer corruption
            lock (this.sendSyncObject)
            {
                Console.WriteLine("\n=========== Sending Message ===========");
                Console.WriteLine(rawMessage);
                this.binaryWriter?.Write(rawMessage);
                this.binaryWriter?.Flush();
            }
        }
    }
}

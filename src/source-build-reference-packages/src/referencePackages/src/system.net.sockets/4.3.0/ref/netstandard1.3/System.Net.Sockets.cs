// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Security.AllowPartiallyTrustedCallers]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyTitle("System.Net.Sockets")]
[assembly: System.Reflection.AssemblyDescription("System.Net.Sockets")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Net.Sockets")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("1.0.24212.01")]
[assembly: System.Reflection.AssemblyInformationalVersion("1.0.24212.01. Commit Hash: 9688ddbb62c04189cac4c4a06e31e93377dccd41")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.1.0.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Net.Sockets
{
    public enum IOControlCode : long
    {
        AsyncIO = 2147772029L,
        NonBlockingIO = 2147772030L,
        AssociateHandle = 2281701377L,
        MultipointLoopback = 2281701385L,
        MulticastScope = 2281701386L,
        SetQos = 2281701387L,
        SetGroupQos = 2281701388L,
        RoutingInterfaceChange = 2281701397L,
        NamespaceChange = 2281701401L,
        ReceiveAll = 2550136833L,
        ReceiveAllMulticast = 2550136834L,
        ReceiveAllIgmpMulticast = 2550136835L,
        KeepAliveValues = 2550136836L,
        AbsorbRouterAlert = 2550136837L,
        UnicastInterface = 2550136838L,
        LimitBroadcasts = 2550136839L,
        BindToInterface = 2550136840L,
        MulticastInterface = 2550136841L,
        AddMulticastGroupOnInterface = 2550136842L,
        DeleteMulticastGroupFromInterface = 2550136843L,
        GetExtensionFunctionPointer = 3355443206L,
        GetQos = 3355443207L,
        GetGroupQos = 3355443208L,
        TranslateHandle = 3355443213L,
        RoutingInterfaceQuery = 3355443220L,
        AddressListSort = 3355443225L,
        EnableCircularQueuing = 671088642L,
        Flush = 671088644L,
        AddressListChange = 671088663L,
        DataToRead = 1074030207L,
        OobDataRead = 1074033415L,
        GetBroadcastAddress = 1207959557L,
        AddressListQuery = 1207959574L,
        QueryTargetPnpHandle = 1207959576L
    }

    public partial struct IPPacketInformation
    {
        public IPAddress Address { get { throw null; } }

        public int Interface { get { throw null; } }

        public override bool Equals(object comparand) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(IPPacketInformation packetInformation1, IPPacketInformation packetInformation2) { throw null; }

        public static bool operator !=(IPPacketInformation packetInformation1, IPPacketInformation packetInformation2) { throw null; }
    }

    public enum IPProtectionLevel
    {
        Unspecified = -1,
        Unrestricted = 10,
        EdgeRestricted = 20,
        Restricted = 30
    }

    public partial class IPv6MulticastOption
    {
        public IPv6MulticastOption(IPAddress group, long ifindex) { }

        public IPv6MulticastOption(IPAddress group) { }

        public IPAddress Group { get { throw null; } set { } }

        public long InterfaceIndex { get { throw null; } set { } }
    }

    public partial class LingerOption
    {
        public LingerOption(bool enable, int seconds) { }

        public bool Enabled { get { throw null; } set { } }

        public int LingerTime { get { throw null; } set { } }
    }

    public partial class MulticastOption
    {
        public MulticastOption(IPAddress group, int interfaceIndex) { }

        public MulticastOption(IPAddress group, IPAddress mcint) { }

        public MulticastOption(IPAddress group) { }

        public IPAddress Group { get { throw null; } set { } }

        public int InterfaceIndex { get { throw null; } set { } }

        public IPAddress LocalAddress { get { throw null; } set { } }
    }

    public partial class NetworkStream : IO.Stream
    {
        public NetworkStream(Socket socket, bool ownsSocket) { }

        public NetworkStream(Socket socket) { }

        public override bool CanRead { get { throw null; } }

        public override bool CanSeek { get { throw null; } }

        public override bool CanTimeout { get { throw null; } }

        public override bool CanWrite { get { throw null; } }

        public virtual bool DataAvailable { get { throw null; } }

        public override long Length { get { throw null; } }

        public override long Position { get { throw null; } set { } }

        public override int ReadTimeout { get { throw null; } set { } }

        public override int WriteTimeout { get { throw null; } set { } }

        protected override void Dispose(bool disposing) { }

        ~NetworkStream() {
        }

        public override void Flush() { }

        public override Threading.Tasks.Task FlushAsync(Threading.CancellationToken cancellationToken) { throw null; }

        public override int Read(byte[] buffer, int offset, int size) { throw null; }

        public override Threading.Tasks.Task<int> ReadAsync(byte[] buffer, int offset, int size, Threading.CancellationToken cancellationToken) { throw null; }

        public override long Seek(long offset, IO.SeekOrigin origin) { throw null; }

        public override void SetLength(long value) { }

        public override void Write(byte[] buffer, int offset, int size) { }

        public override Threading.Tasks.Task WriteAsync(byte[] buffer, int offset, int size, Threading.CancellationToken cancellationToken) { throw null; }
    }

    public enum ProtocolType
    {
        Unknown = -1,
        IP = 0,
        IPv6HopByHopOptions = 0,
        Unspecified = 0,
        Icmp = 1,
        Igmp = 2,
        Ggp = 3,
        IPv4 = 4,
        Tcp = 6,
        Pup = 12,
        Udp = 17,
        Idp = 22,
        IPv6 = 41,
        IPv6RoutingHeader = 43,
        IPv6FragmentHeader = 44,
        IPSecEncapsulatingSecurityPayload = 50,
        IPSecAuthenticationHeader = 51,
        IcmpV6 = 58,
        IPv6NoNextHeader = 59,
        IPv6DestinationOptions = 60,
        ND = 77,
        Raw = 255,
        Ipx = 1000,
        Spx = 1256,
        SpxII = 1257
    }

    public enum SelectMode
    {
        SelectRead = 0,
        SelectWrite = 1,
        SelectError = 2
    }

    public partial class SendPacketsElement
    {
        public SendPacketsElement(byte[] buffer, int offset, int count, bool endOfPacket) { }

        public SendPacketsElement(byte[] buffer, int offset, int count) { }

        public SendPacketsElement(byte[] buffer) { }

        public SendPacketsElement(string filepath, int offset, int count, bool endOfPacket) { }

        public SendPacketsElement(string filepath, int offset, int count) { }

        public SendPacketsElement(string filepath) { }

        public byte[] Buffer { get { throw null; } }

        public int Count { get { throw null; } }

        public bool EndOfPacket { get { throw null; } }

        public string FilePath { get { throw null; } }

        public int Offset { get { throw null; } }
    }

    public partial class Socket : IDisposable
    {
        public Socket(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType) { }

        public Socket(SocketType socketType, ProtocolType protocolType) { }

        public AddressFamily AddressFamily { get { throw null; } }

        public int Available { get { throw null; } }

        public bool Blocking { get { throw null; } set { } }

        public bool Connected { get { throw null; } }

        public bool DontFragment { get { throw null; } set { } }

        public bool DualMode { get { throw null; } set { } }

        public bool EnableBroadcast { get { throw null; } set { } }

        public bool ExclusiveAddressUse { get { throw null; } set { } }

        public bool IsBound { get { throw null; } }

        public LingerOption LingerState { get { throw null; } set { } }

        public EndPoint LocalEndPoint { get { throw null; } }

        public bool MulticastLoopback { get { throw null; } set { } }

        public bool NoDelay { get { throw null; } set { } }

        public static bool OSSupportsIPv4 { get { throw null; } }

        public static bool OSSupportsIPv6 { get { throw null; } }

        public ProtocolType ProtocolType { get { throw null; } }

        public int ReceiveBufferSize { get { throw null; } set { } }

        public int ReceiveTimeout { get { throw null; } set { } }

        public EndPoint RemoteEndPoint { get { throw null; } }

        public int SendBufferSize { get { throw null; } set { } }

        public int SendTimeout { get { throw null; } set { } }

        public SocketType SocketType { get { throw null; } }

        public short Ttl { get { throw null; } set { } }

        public Socket Accept() { throw null; }

        public bool AcceptAsync(SocketAsyncEventArgs e) { throw null; }

        public void Bind(EndPoint localEP) { }

        public static void CancelConnectAsync(SocketAsyncEventArgs e) { }

        public void Connect(EndPoint remoteEP) { }

        public void Connect(IPAddress address, int port) { }

        public void Connect(IPAddress[] addresses, int port) { }

        public void Connect(string host, int port) { }

        public bool ConnectAsync(SocketAsyncEventArgs e) { throw null; }

        public static bool ConnectAsync(SocketType socketType, ProtocolType protocolType, SocketAsyncEventArgs e) { throw null; }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        ~Socket() {
        }

        public void GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, byte[] optionValue) { }

        public byte[] GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, int optionLength) { throw null; }

        public object GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName) { throw null; }

        public int IOControl(int ioControlCode, byte[] optionInValue, byte[] optionOutValue) { throw null; }

        public int IOControl(IOControlCode ioControlCode, byte[] optionInValue, byte[] optionOutValue) { throw null; }

        public void Listen(int backlog) { }

        public bool Poll(int microSeconds, SelectMode mode) { throw null; }

        public int Receive(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode) { throw null; }

        public int Receive(byte[] buffer, int offset, int size, SocketFlags socketFlags) { throw null; }

        public int Receive(byte[] buffer, int size, SocketFlags socketFlags) { throw null; }

        public int Receive(byte[] buffer, SocketFlags socketFlags) { throw null; }

        public int Receive(byte[] buffer) { throw null; }

        public int Receive(Collections.Generic.IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode) { throw null; }

        public int Receive(Collections.Generic.IList<ArraySegment<byte>> buffers, SocketFlags socketFlags) { throw null; }

        public int Receive(Collections.Generic.IList<ArraySegment<byte>> buffers) { throw null; }

        public bool ReceiveAsync(SocketAsyncEventArgs e) { throw null; }

        public int ReceiveFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags, ref EndPoint remoteEP) { throw null; }

        public int ReceiveFrom(byte[] buffer, int size, SocketFlags socketFlags, ref EndPoint remoteEP) { throw null; }

        public int ReceiveFrom(byte[] buffer, ref EndPoint remoteEP) { throw null; }

        public int ReceiveFrom(byte[] buffer, SocketFlags socketFlags, ref EndPoint remoteEP) { throw null; }

        public bool ReceiveFromAsync(SocketAsyncEventArgs e) { throw null; }

        public int ReceiveMessageFrom(byte[] buffer, int offset, int size, ref SocketFlags socketFlags, ref EndPoint remoteEP, out IPPacketInformation ipPacketInformation) { throw null; }

        public bool ReceiveMessageFromAsync(SocketAsyncEventArgs e) { throw null; }

        public static void Select(Collections.IList checkRead, Collections.IList checkWrite, Collections.IList checkError, int microSeconds) { }

        public int Send(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode) { throw null; }

        public int Send(byte[] buffer, int offset, int size, SocketFlags socketFlags) { throw null; }

        public int Send(byte[] buffer, int size, SocketFlags socketFlags) { throw null; }

        public int Send(byte[] buffer, SocketFlags socketFlags) { throw null; }

        public int Send(byte[] buffer) { throw null; }

        public int Send(Collections.Generic.IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode) { throw null; }

        public int Send(Collections.Generic.IList<ArraySegment<byte>> buffers, SocketFlags socketFlags) { throw null; }

        public int Send(Collections.Generic.IList<ArraySegment<byte>> buffers) { throw null; }

        public bool SendAsync(SocketAsyncEventArgs e) { throw null; }

        public bool SendPacketsAsync(SocketAsyncEventArgs e) { throw null; }

        public int SendTo(byte[] buffer, int offset, int size, SocketFlags socketFlags, EndPoint remoteEP) { throw null; }

        public int SendTo(byte[] buffer, int size, SocketFlags socketFlags, EndPoint remoteEP) { throw null; }

        public int SendTo(byte[] buffer, EndPoint remoteEP) { throw null; }

        public int SendTo(byte[] buffer, SocketFlags socketFlags, EndPoint remoteEP) { throw null; }

        public bool SendToAsync(SocketAsyncEventArgs e) { throw null; }

        public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, bool optionValue) { }

        public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, byte[] optionValue) { }

        public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, int optionValue) { }

        public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, object optionValue) { }

        public void Shutdown(SocketShutdown how) { }
    }

    public partial class SocketAsyncEventArgs : EventArgs, IDisposable
    {
        public Socket AcceptSocket { get { throw null; } set { } }

        public byte[] Buffer { get { throw null; } }

        public Collections.Generic.IList<ArraySegment<byte>> BufferList { get { throw null; } set { } }

        public int BytesTransferred { get { throw null; } }

        public Exception ConnectByNameError { get { throw null; } }

        public Socket ConnectSocket { get { throw null; } }

        public int Count { get { throw null; } }

        public SocketAsyncOperation LastOperation { get { throw null; } }

        public int Offset { get { throw null; } }

        public IPPacketInformation ReceiveMessageFromPacketInfo { get { throw null; } }

        public EndPoint RemoteEndPoint { get { throw null; } set { } }

        public SendPacketsElement[] SendPacketsElements { get { throw null; } set { } }

        public int SendPacketsSendSize { get { throw null; } set { } }

        public SocketError SocketError { get { throw null; } set { } }

        public SocketFlags SocketFlags { get { throw null; } set { } }

        public object UserToken { get { throw null; } set { } }

        public event EventHandler<SocketAsyncEventArgs> Completed { add { } remove { } }

        public void Dispose() { }

        ~SocketAsyncEventArgs() {
        }

        protected virtual void OnCompleted(SocketAsyncEventArgs e) { }

        public void SetBuffer(byte[] buffer, int offset, int count) { }

        public void SetBuffer(int offset, int count) { }
    }

    public enum SocketAsyncOperation
    {
        None = 0,
        Accept = 1,
        Connect = 2,
        Disconnect = 3,
        Receive = 4,
        ReceiveFrom = 5,
        ReceiveMessageFrom = 6,
        Send = 7,
        SendPackets = 8,
        SendTo = 9
    }

    [Flags]
    public enum SocketFlags
    {
        None = 0,
        OutOfBand = 1,
        Peek = 2,
        DontRoute = 4,
        Truncated = 256,
        ControlDataTruncated = 512,
        Broadcast = 1024,
        Multicast = 2048,
        Partial = 32768
    }

    public enum SocketOptionLevel
    {
        IP = 0,
        Tcp = 6,
        Udp = 17,
        IPv6 = 41,
        Socket = 65535
    }

    public enum SocketOptionName
    {
        DontLinger = -129,
        ExclusiveAddressUse = -5,
        Debug = 1,
        IPOptions = 1,
        NoChecksum = 1,
        NoDelay = 1,
        AcceptConnection = 2,
        BsdUrgent = 2,
        Expedited = 2,
        HeaderIncluded = 2,
        TypeOfService = 3,
        IpTimeToLive = 4,
        ReuseAddress = 4,
        KeepAlive = 8,
        MulticastInterface = 9,
        MulticastTimeToLive = 10,
        MulticastLoopback = 11,
        AddMembership = 12,
        DropMembership = 13,
        DontFragment = 14,
        AddSourceMembership = 15,
        DontRoute = 16,
        DropSourceMembership = 16,
        BlockSource = 17,
        UnblockSource = 18,
        PacketInformation = 19,
        ChecksumCoverage = 20,
        HopLimit = 21,
        IPProtectionLevel = 23,
        IPv6Only = 27,
        Broadcast = 32,
        UseLoopback = 64,
        Linger = 128,
        OutOfBandInline = 256,
        SendBuffer = 4097,
        ReceiveBuffer = 4098,
        SendLowWater = 4099,
        ReceiveLowWater = 4100,
        SendTimeout = 4101,
        ReceiveTimeout = 4102,
        Error = 4103,
        Type = 4104,
        ReuseUnicastPort = 12295,
        UpdateAcceptContext = 28683,
        UpdateConnectContext = 28688,
        MaxConnections = int.MaxValue
    }

    public partial struct SocketReceiveFromResult
    {
        public int ReceivedBytes;
        public EndPoint RemoteEndPoint;
    }

    public partial struct SocketReceiveMessageFromResult
    {
        public IPPacketInformation PacketInformation;
        public int ReceivedBytes;
        public EndPoint RemoteEndPoint;
        public SocketFlags SocketFlags;
    }

    public enum SocketShutdown
    {
        Receive = 0,
        Send = 1,
        Both = 2
    }

    public static partial class SocketTaskExtensions
    {
        public static Threading.Tasks.Task<Socket> AcceptAsync(this Socket socket, Socket acceptSocket) { throw null; }

        public static Threading.Tasks.Task<Socket> AcceptAsync(this Socket socket) { throw null; }

        public static Threading.Tasks.Task ConnectAsync(this Socket socket, EndPoint remoteEP) { throw null; }

        public static Threading.Tasks.Task ConnectAsync(this Socket socket, IPAddress address, int port) { throw null; }

        public static Threading.Tasks.Task ConnectAsync(this Socket socket, IPAddress[] addresses, int port) { throw null; }

        public static Threading.Tasks.Task ConnectAsync(this Socket socket, string host, int port) { throw null; }

        public static Threading.Tasks.Task<int> ReceiveAsync(this Socket socket, ArraySegment<byte> buffer, SocketFlags socketFlags) { throw null; }

        public static Threading.Tasks.Task<int> ReceiveAsync(this Socket socket, Collections.Generic.IList<ArraySegment<byte>> buffers, SocketFlags socketFlags) { throw null; }

        public static Threading.Tasks.Task<SocketReceiveFromResult> ReceiveFromAsync(this Socket socket, ArraySegment<byte> buffer, SocketFlags socketFlags, EndPoint remoteEndPoint) { throw null; }

        public static Threading.Tasks.Task<SocketReceiveMessageFromResult> ReceiveMessageFromAsync(this Socket socket, ArraySegment<byte> buffer, SocketFlags socketFlags, EndPoint remoteEndPoint) { throw null; }

        public static Threading.Tasks.Task<int> SendAsync(this Socket socket, ArraySegment<byte> buffer, SocketFlags socketFlags) { throw null; }

        public static Threading.Tasks.Task<int> SendAsync(this Socket socket, Collections.Generic.IList<ArraySegment<byte>> buffers, SocketFlags socketFlags) { throw null; }

        public static Threading.Tasks.Task<int> SendToAsync(this Socket socket, ArraySegment<byte> buffer, SocketFlags socketFlags, EndPoint remoteEP) { throw null; }
    }

    public enum SocketType
    {
        Unknown = -1,
        Stream = 1,
        Dgram = 2,
        Raw = 3,
        Rdm = 4,
        Seqpacket = 5
    }

    public partial class TcpClient : IDisposable
    {
        public TcpClient() { }

        public TcpClient(AddressFamily family) { }

        protected bool Active { get { throw null; } set { } }

        public int Available { get { throw null; } }

        public Socket Client { get { throw null; } set { } }

        public bool Connected { get { throw null; } }

        public bool ExclusiveAddressUse { get { throw null; } set { } }

        public LingerOption LingerState { get { throw null; } set { } }

        public bool NoDelay { get { throw null; } set { } }

        public int ReceiveBufferSize { get { throw null; } set { } }

        public int ReceiveTimeout { get { throw null; } set { } }

        public int SendBufferSize { get { throw null; } set { } }

        public int SendTimeout { get { throw null; } set { } }

        public Threading.Tasks.Task ConnectAsync(IPAddress address, int port) { throw null; }

        public Threading.Tasks.Task ConnectAsync(IPAddress[] addresses, int port) { throw null; }

        public Threading.Tasks.Task ConnectAsync(string host, int port) { throw null; }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        ~TcpClient() {
        }

        public NetworkStream GetStream() { throw null; }
    }

    public partial class TcpListener
    {
        public TcpListener(IPAddress localaddr, int port) { }

        public TcpListener(IPEndPoint localEP) { }

        protected bool Active { get { throw null; } }

        public bool ExclusiveAddressUse { get { throw null; } set { } }

        public EndPoint LocalEndpoint { get { throw null; } }

        public Socket Server { get { throw null; } }

        public Threading.Tasks.Task<Socket> AcceptSocketAsync() { throw null; }

        public Threading.Tasks.Task<TcpClient> AcceptTcpClientAsync() { throw null; }

        public bool Pending() { throw null; }

        public void Start() { }

        public void Start(int backlog) { }

        public void Stop() { }
    }

    public partial class UdpClient : IDisposable
    {
        public UdpClient() { }

        public UdpClient(int port, AddressFamily family) { }

        public UdpClient(int port) { }

        public UdpClient(IPEndPoint localEP) { }

        public UdpClient(AddressFamily family) { }

        protected bool Active { get { throw null; } set { } }

        public int Available { get { throw null; } }

        public Socket Client { get { throw null; } set { } }

        public bool DontFragment { get { throw null; } set { } }

        public bool EnableBroadcast { get { throw null; } set { } }

        public bool ExclusiveAddressUse { get { throw null; } set { } }

        public bool MulticastLoopback { get { throw null; } set { } }

        public short Ttl { get { throw null; } set { } }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        public void DropMulticastGroup(IPAddress multicastAddr, int ifindex) { }

        public void DropMulticastGroup(IPAddress multicastAddr) { }

        public void JoinMulticastGroup(int ifindex, IPAddress multicastAddr) { }

        public void JoinMulticastGroup(IPAddress multicastAddr, int timeToLive) { }

        public void JoinMulticastGroup(IPAddress multicastAddr, IPAddress localAddress) { }

        public void JoinMulticastGroup(IPAddress multicastAddr) { }

        public Threading.Tasks.Task<UdpReceiveResult> ReceiveAsync() { throw null; }

        public Threading.Tasks.Task<int> SendAsync(byte[] datagram, int bytes, IPEndPoint endPoint) { throw null; }

        public Threading.Tasks.Task<int> SendAsync(byte[] datagram, int bytes, string hostname, int port) { throw null; }
    }

    public partial struct UdpReceiveResult : IEquatable<UdpReceiveResult>
    {
        public UdpReceiveResult(byte[] buffer, IPEndPoint remoteEndPoint) { }

        public byte[] Buffer { get { throw null; } }

        public IPEndPoint RemoteEndPoint { get { throw null; } }

        public bool Equals(UdpReceiveResult other) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(UdpReceiveResult left, UdpReceiveResult right) { throw null; }

        public static bool operator !=(UdpReceiveResult left, UdpReceiveResult right) { throw null; }
    }
}
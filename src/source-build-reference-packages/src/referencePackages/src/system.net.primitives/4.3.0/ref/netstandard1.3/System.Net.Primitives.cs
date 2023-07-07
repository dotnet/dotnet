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
[assembly: System.Reflection.AssemblyTitle("System.Net.Primitives")]
[assembly: System.Reflection.AssemblyDescription("System.Net.Primitives")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Net.Primitives")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("1.0.24212.01")]
[assembly: System.Reflection.AssemblyInformationalVersion("1.0.24212.01. Commit Hash: 9688ddbb62c04189cac4c4a06e31e93377dccd41")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.10.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Net
{
    [Flags]
    public enum AuthenticationSchemes
    {
        None = 0,
        Digest = 1,
        Negotiate = 2,
        Ntlm = 4,
        IntegratedWindowsAuthentication = 6,
        Basic = 8,
        Anonymous = 32768
    }

    public sealed partial class Cookie
    {
        public Cookie() { }

        public Cookie(string name, string value, string path, string domain) { }

        public Cookie(string name, string value, string path) { }

        public Cookie(string name, string value) { }

        public string Comment { get { throw null; } set { } }

        public Uri CommentUri { get { throw null; } set { } }

        public bool Discard { get { throw null; } set { } }

        public string Domain { get { throw null; } set { } }

        public bool Expired { get { throw null; } set { } }

        public DateTime Expires { get { throw null; } set { } }

        public bool HttpOnly { get { throw null; } set { } }

        public string Name { get { throw null; } set { } }

        public string Path { get { throw null; } set { } }

        public string Port { get { throw null; } set { } }

        public bool Secure { get { throw null; } set { } }

        public DateTime TimeStamp { get { throw null; } }

        public string Value { get { throw null; } set { } }

        public int Version { get { throw null; } set { } }

        public override bool Equals(object comparand) { throw null; }

        public override int GetHashCode() { throw null; }

        public override string ToString() { throw null; }
    }

    public partial class CookieCollection : Collections.ICollection, Collections.IEnumerable
    {
        public int Count { get { throw null; } }

        public Cookie this[string name] { get { throw null; } }

        bool Collections.ICollection.IsSynchronized { get { throw null; } }

        object Collections.ICollection.SyncRoot { get { throw null; } }

        public void Add(Cookie cookie) { }

        public void Add(CookieCollection cookies) { }

        public Collections.IEnumerator GetEnumerator() { throw null; }

        void Collections.ICollection.CopyTo(Array array, int index) { }
    }

    public partial class CookieContainer
    {
        public const int DefaultCookieLengthLimit = 4096;
        public const int DefaultCookieLimit = 300;
        public const int DefaultPerDomainCookieLimit = 20;
        public int Capacity { get { throw null; } set { } }

        public int Count { get { throw null; } }

        public int MaxCookieSize { get { throw null; } set { } }

        public int PerDomainCapacity { get { throw null; } set { } }

        public void Add(Uri uri, Cookie cookie) { }

        public void Add(Uri uri, CookieCollection cookies) { }

        public string GetCookieHeader(Uri uri) { throw null; }

        public CookieCollection GetCookies(Uri uri) { throw null; }

        public void SetCookies(Uri uri, string cookieHeader) { }
    }

    public partial class CookieException : FormatException
    {
    }

    public partial class CredentialCache : Collections.IEnumerable, ICredentials, ICredentialsByHost
    {
        public static ICredentials DefaultCredentials { get { throw null; } }

        public static NetworkCredential DefaultNetworkCredentials { get { throw null; } }

        public void Add(string host, int port, string authenticationType, NetworkCredential credential) { }

        public void Add(Uri uriPrefix, string authType, NetworkCredential cred) { }

        public NetworkCredential GetCredential(string host, int port, string authenticationType) { throw null; }

        public NetworkCredential GetCredential(Uri uriPrefix, string authType) { throw null; }

        public Collections.IEnumerator GetEnumerator() { throw null; }

        public void Remove(string host, int port, string authenticationType) { }

        public void Remove(Uri uriPrefix, string authType) { }
    }

    [Flags]
    public enum DecompressionMethods
    {
        None = 0,
        GZip = 1,
        Deflate = 2
    }

    public partial class DnsEndPoint : EndPoint
    {
        public DnsEndPoint(string host, int port, Sockets.AddressFamily addressFamily) { }

        public DnsEndPoint(string host, int port) { }

        public override Sockets.AddressFamily AddressFamily { get { throw null; } }

        public string Host { get { throw null; } }

        public int Port { get { throw null; } }

        public override bool Equals(object comparand) { throw null; }

        public override int GetHashCode() { throw null; }

        public override string ToString() { throw null; }
    }

    public abstract partial class EndPoint
    {
        public virtual Sockets.AddressFamily AddressFamily { get { throw null; } }

        public virtual EndPoint Create(SocketAddress socketAddress) { throw null; }

        public virtual SocketAddress Serialize() { throw null; }
    }

    public enum HttpStatusCode
    {
        Continue = 100,
        SwitchingProtocols = 101,
        OK = 200,
        Created = 201,
        Accepted = 202,
        NonAuthoritativeInformation = 203,
        NoContent = 204,
        ResetContent = 205,
        PartialContent = 206,
        Ambiguous = 300,
        MultipleChoices = 300,
        Moved = 301,
        MovedPermanently = 301,
        Found = 302,
        Redirect = 302,
        RedirectMethod = 303,
        SeeOther = 303,
        NotModified = 304,
        UseProxy = 305,
        Unused = 306,
        RedirectKeepVerb = 307,
        TemporaryRedirect = 307,
        BadRequest = 400,
        Unauthorized = 401,
        PaymentRequired = 402,
        Forbidden = 403,
        NotFound = 404,
        MethodNotAllowed = 405,
        NotAcceptable = 406,
        ProxyAuthenticationRequired = 407,
        RequestTimeout = 408,
        Conflict = 409,
        Gone = 410,
        LengthRequired = 411,
        PreconditionFailed = 412,
        RequestEntityTooLarge = 413,
        RequestUriTooLong = 414,
        UnsupportedMediaType = 415,
        RequestedRangeNotSatisfiable = 416,
        ExpectationFailed = 417,
        UpgradeRequired = 426,
        InternalServerError = 500,
        NotImplemented = 501,
        BadGateway = 502,
        ServiceUnavailable = 503,
        GatewayTimeout = 504,
        HttpVersionNotSupported = 505
    }

    public partial interface ICredentials
    {
        NetworkCredential GetCredential(Uri uri, string authType);
    }

    public partial interface ICredentialsByHost
    {
        NetworkCredential GetCredential(string host, int port, string authenticationType);
    }

    public partial class IPAddress
    {
        public static readonly IPAddress Any;
        public static readonly IPAddress Broadcast;
        public static readonly IPAddress IPv6Any;
        public static readonly IPAddress IPv6Loopback;
        public static readonly IPAddress IPv6None;
        public static readonly IPAddress Loopback;
        public static readonly IPAddress None;
        public IPAddress(byte[] address, long scopeid) { }

        public IPAddress(byte[] address) { }

        public IPAddress(long newAddress) { }

        public Sockets.AddressFamily AddressFamily { get { throw null; } }

        public bool IsIPv4MappedToIPv6 { get { throw null; } }

        public bool IsIPv6LinkLocal { get { throw null; } }

        public bool IsIPv6Multicast { get { throw null; } }

        public bool IsIPv6SiteLocal { get { throw null; } }

        public bool IsIPv6Teredo { get { throw null; } }

        public long ScopeId { get { throw null; } set { } }

        public override bool Equals(object comparand) { throw null; }

        public byte[] GetAddressBytes() { throw null; }

        public override int GetHashCode() { throw null; }

        public static short HostToNetworkOrder(short host) { throw null; }

        public static int HostToNetworkOrder(int host) { throw null; }

        public static long HostToNetworkOrder(long host) { throw null; }

        public static bool IsLoopback(IPAddress address) { throw null; }

        public IPAddress MapToIPv4() { throw null; }

        public IPAddress MapToIPv6() { throw null; }

        public static short NetworkToHostOrder(short network) { throw null; }

        public static int NetworkToHostOrder(int network) { throw null; }

        public static long NetworkToHostOrder(long network) { throw null; }

        public static IPAddress Parse(string ipString) { throw null; }

        public override string ToString() { throw null; }

        public static bool TryParse(string ipString, out IPAddress address) { throw null; }
    }

    public partial class IPEndPoint : EndPoint
    {
        public const int MaxPort = 65535;
        public const int MinPort = 0;
        public IPEndPoint(long address, int port) { }

        public IPEndPoint(IPAddress address, int port) { }

        public IPAddress Address { get { throw null; } set { } }

        public override Sockets.AddressFamily AddressFamily { get { throw null; } }

        public int Port { get { throw null; } set { } }

        public override EndPoint Create(SocketAddress socketAddress) { throw null; }

        public override bool Equals(object comparand) { throw null; }

        public override int GetHashCode() { throw null; }

        public override SocketAddress Serialize() { throw null; }

        public override string ToString() { throw null; }
    }

    public partial interface IWebProxy
    {
        ICredentials Credentials { get; set; }

        Uri GetProxy(Uri destination);
        bool IsBypassed(Uri host);
    }

    public partial class NetworkCredential : ICredentials, ICredentialsByHost
    {
        public NetworkCredential() { }

        public NetworkCredential(string userName, string password, string domain) { }

        public NetworkCredential(string userName, string password) { }

        public string Domain { get { throw null; } set { } }

        public string Password { get { throw null; } set { } }

        public string UserName { get { throw null; } set { } }

        public NetworkCredential GetCredential(string host, int port, string authenticationType) { throw null; }

        public NetworkCredential GetCredential(Uri uri, string authType) { throw null; }
    }

    public partial class SocketAddress
    {
        public SocketAddress(Sockets.AddressFamily family, int size) { }

        public SocketAddress(Sockets.AddressFamily family) { }

        public Sockets.AddressFamily Family { get { throw null; } }

        public byte this[int offset] { get { throw null; } set { } }

        public int Size { get { throw null; } }

        public override bool Equals(object comparand) { throw null; }

        public override int GetHashCode() { throw null; }

        public override string ToString() { throw null; }
    }

    public abstract partial class TransportContext
    {
        public abstract System.Security.Authentication.ExtendedProtection.ChannelBinding GetChannelBinding(System.Security.Authentication.ExtendedProtection.ChannelBindingKind kind);
    }
}

namespace System.Net.NetworkInformation
{
    public partial class IPAddressCollection : Collections.Generic.ICollection<IPAddress>, Collections.Generic.IEnumerable<IPAddress>, Collections.IEnumerable
    {
        protected internal IPAddressCollection() { }

        public virtual int Count { get { throw null; } }

        public virtual bool IsReadOnly { get { throw null; } }

        public virtual IPAddress this[int index] { get { throw null; } }

        public virtual void Add(IPAddress address) { }

        public virtual void Clear() { }

        public virtual bool Contains(IPAddress address) { throw null; }

        public virtual void CopyTo(IPAddress[] array, int offset) { }

        public virtual Collections.Generic.IEnumerator<IPAddress> GetEnumerator() { throw null; }

        public virtual bool Remove(IPAddress address) { throw null; }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }
    }
}

namespace System.Net.Security
{
    public enum AuthenticationLevel
    {
        None = 0,
        MutualAuthRequested = 1,
        MutualAuthRequired = 2
    }

    [Flags]
    public enum SslPolicyErrors
    {
        None = 0,
        RemoteCertificateNotAvailable = 1,
        RemoteCertificateNameMismatch = 2,
        RemoteCertificateChainErrors = 4
    }
}

namespace System.Net.Sockets
{
    public enum AddressFamily
    {
        Unknown = -1,
        Unspecified = 0,
        Unix = 1,
        InterNetwork = 2,
        ImpLink = 3,
        Pup = 4,
        Chaos = 5,
        Ipx = 6,
        NS = 6,
        Iso = 7,
        Osi = 7,
        Ecma = 8,
        DataKit = 9,
        Ccitt = 10,
        Sna = 11,
        DecNet = 12,
        DataLink = 13,
        Lat = 14,
        HyperChannel = 15,
        AppleTalk = 16,
        NetBios = 17,
        VoiceView = 18,
        FireFox = 19,
        Banyan = 21,
        Atm = 22,
        InterNetworkV6 = 23,
        Cluster = 24,
        Ieee12844 = 25,
        Irda = 26,
        NetworkDesigners = 28
    }

    public enum SocketError
    {
        SocketError = -1,
        Success = 0,
        OperationAborted = 995,
        IOPending = 997,
        Interrupted = 10004,
        AccessDenied = 10013,
        Fault = 10014,
        InvalidArgument = 10022,
        TooManyOpenSockets = 10024,
        WouldBlock = 10035,
        InProgress = 10036,
        AlreadyInProgress = 10037,
        NotSocket = 10038,
        DestinationAddressRequired = 10039,
        MessageSize = 10040,
        ProtocolType = 10041,
        ProtocolOption = 10042,
        ProtocolNotSupported = 10043,
        SocketNotSupported = 10044,
        OperationNotSupported = 10045,
        ProtocolFamilyNotSupported = 10046,
        AddressFamilyNotSupported = 10047,
        AddressAlreadyInUse = 10048,
        AddressNotAvailable = 10049,
        NetworkDown = 10050,
        NetworkUnreachable = 10051,
        NetworkReset = 10052,
        ConnectionAborted = 10053,
        ConnectionReset = 10054,
        NoBufferSpaceAvailable = 10055,
        IsConnected = 10056,
        NotConnected = 10057,
        Shutdown = 10058,
        TimedOut = 10060,
        ConnectionRefused = 10061,
        HostDown = 10064,
        HostUnreachable = 10065,
        ProcessLimit = 10067,
        SystemNotReady = 10091,
        VersionNotSupported = 10092,
        NotInitialized = 10093,
        Disconnecting = 10101,
        TypeNotFound = 10109,
        HostNotFound = 11001,
        TryAgain = 11002,
        NoRecovery = 11003,
        NoData = 11004
    }

    public partial class SocketException : Exception
    {
        public SocketException() { }

        public SocketException(int errorCode) { }

        public override string Message { get { throw null; } }

        public SocketError SocketErrorCode { get { throw null; } }
    }
}

namespace System.Security.Authentication
{
    public enum CipherAlgorithmType
    {
        None = 0,
        Null = 24576,
        Des = 26113,
        Rc2 = 26114,
        TripleDes = 26115,
        Aes128 = 26126,
        Aes192 = 26127,
        Aes256 = 26128,
        Aes = 26129,
        Rc4 = 26625
    }

    public enum ExchangeAlgorithmType
    {
        None = 0,
        RsaSign = 9216,
        RsaKeyX = 41984,
        DiffieHellman = 43522
    }

    public enum HashAlgorithmType
    {
        None = 0,
        Md5 = 32771,
        Sha1 = 32772
    }

    [Flags]
    public enum SslProtocols
    {
        None = 0,
        Ssl2 = 12,
        Ssl3 = 48,
        Tls = 192,
        Tls11 = 768,
        Tls12 = 3072
    }
}

namespace System.Security.Authentication.ExtendedProtection
{
    public abstract partial class ChannelBinding : Runtime.InteropServices.SafeHandle
    {
        protected ChannelBinding() : base(default, default) { }

        protected ChannelBinding(bool ownsHandle) : base(default, default) { }

        public abstract int Size { get; }
    }

    public enum ChannelBindingKind
    {
        Unknown = 0,
        Unique = 25,
        Endpoint = 26
    }
}
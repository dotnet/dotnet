// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("4.0.30319.17929")]
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Reflection.AssemblyTitle("System.Net.Primitives.dll")]
[assembly: System.Reflection.AssemblyDescription("System.Net.Primitives.dll")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Net.Primitives.dll")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Reflection.AssemblyInformationalVersion("4.0.30319.17929")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Security.AllowPartiallyTrustedCallers]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.0.0")]
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

    public abstract partial class TransportContext
    {
        internal TransportContext() { }
    }
}
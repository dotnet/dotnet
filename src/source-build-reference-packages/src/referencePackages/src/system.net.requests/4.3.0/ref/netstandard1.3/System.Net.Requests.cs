// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;

[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: AllowPartiallyTrustedCallers]
[assembly: ReferenceAssembly]
[assembly: AssemblyTitle("System.Net.Requests")]
[assembly: AssemblyDescription("System.Net.Requests")]
[assembly: AssemblyDefaultAlias("System.Net.Requests")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("1.0.24212.01")]
[assembly: AssemblyInformationalVersion("1.0.24212.01 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("4.0.10.0")]




namespace System.Net
{
    public partial class HttpWebRequest : System.Net.WebRequest
    {
        internal HttpWebRequest() { }
        public string Accept { get { throw null; } set { } }
        public virtual bool AllowReadStreamBuffering { get { throw null; } set { } }
        public override string ContentType { get { throw null; } set { } }
        public int ContinueTimeout { get { throw null; } set { } }
        public virtual System.Net.CookieContainer CookieContainer { get { throw null; } set { } }
        public override System.Net.ICredentials Credentials { get { throw null; } set { } }
        public virtual bool HaveResponse { get { throw null; } }
        public override System.Net.WebHeaderCollection Headers { get { throw null; } set { } }
        public override string Method { get { throw null; } set { } }
        public override System.Uri RequestUri { get { throw null; } }
        public virtual bool SupportsCookieContainer { get { throw null; } }
        public override bool UseDefaultCredentials { get { throw null; } set { } }
        public override void Abort() { }
        public override System.IAsyncResult BeginGetRequestStream(System.AsyncCallback callback, object state) { throw null; }
        public override System.IAsyncResult BeginGetResponse(System.AsyncCallback callback, object state) { throw null; }
        public override System.IO.Stream EndGetRequestStream(System.IAsyncResult asyncResult) { throw null; }
        public override System.Net.WebResponse EndGetResponse(System.IAsyncResult asyncResult) { throw null; }
    }
    public partial class HttpWebResponse : System.Net.WebResponse
    {
        internal HttpWebResponse() { }
        public override long ContentLength { get { throw null; } }
        public override string ContentType { get { throw null; } }
        public virtual System.Net.CookieCollection Cookies { get { throw null; } }
        public override System.Net.WebHeaderCollection Headers { get { throw null; } }
        public virtual string Method { get { throw null; } }
        public override System.Uri ResponseUri { get { throw null; } }
        public virtual System.Net.HttpStatusCode StatusCode { get { throw null; } }
        public virtual string StatusDescription { get { throw null; } }
        public override bool SupportsHeaders { get { throw null; } }
        protected override void Dispose(bool disposing) { }
        public override System.IO.Stream GetResponseStream() { throw null; }
    }
    public partial interface IWebRequestCreate
    {
        System.Net.WebRequest Create(System.Uri uri);
    }
    public partial class ProtocolViolationException : System.InvalidOperationException
    {
        public ProtocolViolationException() { }
        public ProtocolViolationException(string message) { }
    }
    public partial class WebException : System.InvalidOperationException
    {
        public WebException() { }
        public WebException(string message) { }
        public WebException(string message, System.Exception innerException) { }
        public WebException(string message, System.Exception innerException, System.Net.WebExceptionStatus status, System.Net.WebResponse response) { }
        public WebException(string message, System.Net.WebExceptionStatus status) { }
        public System.Net.WebResponse Response { get { throw null; } }
        public System.Net.WebExceptionStatus Status { get { throw null; } }
    }
    public enum WebExceptionStatus
    {
        CacheEntryNotFound = 18,
        ConnectFailure = 2,
        ConnectionClosed = 8,
        KeepAliveFailure = 12,
        MessageLengthLimitExceeded = 17,
        NameResolutionFailure = 1,
        Pending = 13,
        PipelineFailure = 5,
        ProtocolError = 7,
        ProxyNameResolutionFailure = 15,
        ReceiveFailure = 3,
        RequestCanceled = 6,
        RequestProhibitedByCachePolicy = 19,
        RequestProhibitedByProxy = 20,
        SecureChannelFailure = 10,
        SendFailure = 4,
        ServerProtocolViolation = 11,
        Success = 0,
        Timeout = 14,
        TrustFailure = 9,
        UnknownError = 16,
    }
    public abstract partial class WebRequest
    {
        protected WebRequest() { }
        public abstract string ContentType { get; set; }
        public virtual System.Net.ICredentials Credentials { get { throw null; } set { } }
        public static System.Net.IWebProxy DefaultWebProxy { get { throw null; } set { } }
        public abstract System.Net.WebHeaderCollection Headers { get; set; }
        public abstract string Method { get; set; }
        public virtual System.Net.IWebProxy Proxy { get { throw null; } set { } }
        public abstract System.Uri RequestUri { get; }
        public virtual bool UseDefaultCredentials { get { throw null; } set { } }
        public abstract void Abort();
        public abstract System.IAsyncResult BeginGetRequestStream(System.AsyncCallback callback, object state);
        public abstract System.IAsyncResult BeginGetResponse(System.AsyncCallback callback, object state);
        public static System.Net.WebRequest Create(string requestUriString) { throw null; }
        public static System.Net.WebRequest Create(System.Uri requestUri) { throw null; }
        public static System.Net.HttpWebRequest CreateHttp(string requestUriString) { throw null; }
        public static System.Net.HttpWebRequest CreateHttp(System.Uri requestUri) { throw null; }
        public abstract System.IO.Stream EndGetRequestStream(System.IAsyncResult asyncResult);
        public abstract System.Net.WebResponse EndGetResponse(System.IAsyncResult asyncResult);
        public virtual System.Threading.Tasks.Task<System.IO.Stream> GetRequestStreamAsync() { throw null; }
        public virtual System.Threading.Tasks.Task<System.Net.WebResponse> GetResponseAsync() { throw null; }
        public static bool RegisterPrefix(string prefix, System.Net.IWebRequestCreate creator) { throw null; }
    }
    public abstract partial class WebResponse : System.IDisposable
    {
        protected WebResponse() { }
        public abstract long ContentLength { get; }
        public abstract string ContentType { get; }
        public virtual System.Net.WebHeaderCollection Headers { get { throw null; } }
        public abstract System.Uri ResponseUri { get; }
        public virtual bool SupportsHeaders { get { throw null; } }
        public void Dispose() { }
        protected virtual void Dispose(bool disposing) { }
        public abstract System.IO.Stream GetResponseStream();
    }
}

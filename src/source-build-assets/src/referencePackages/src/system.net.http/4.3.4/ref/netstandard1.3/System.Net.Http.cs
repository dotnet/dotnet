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
[assembly: System.Reflection.AssemblyTitle("System.Net.Http")]
[assembly: System.Reflection.AssemblyDescription("System.Net.Http")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Net.Http")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("4.6.26907.01")]
[assembly: System.Reflection.AssemblyInformationalVersion("4.6.26907.01. Commit Hash: 80e67874a4e16027634daa7fa6872a049bf77808")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.1.1.3")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Net.Http
{
    public partial class ByteArrayContent : HttpContent
    {
        public ByteArrayContent(byte[] content, int offset, int count) { }

        public ByteArrayContent(byte[] content) { }

        protected override Threading.Tasks.Task<IO.Stream> CreateContentReadStreamAsync() { throw null; }

        protected override Threading.Tasks.Task SerializeToStreamAsync(IO.Stream stream, TransportContext context) { throw null; }

        protected internal override bool TryComputeLength(out long length) { throw null; }
    }

    public enum ClientCertificateOption
    {
        Manual = 0,
        Automatic = 1
    }

    public abstract partial class DelegatingHandler : HttpMessageHandler
    {
        protected DelegatingHandler() { }

        protected DelegatingHandler(HttpMessageHandler innerHandler) { }

        public HttpMessageHandler InnerHandler { get { throw null; } set { } }

        protected override void Dispose(bool disposing) { }

        protected internal override Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, Threading.CancellationToken cancellationToken) { throw null; }
    }

    public partial class FormUrlEncodedContent : ByteArrayContent
    {
        public FormUrlEncodedContent(Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, string>> nameValueCollection) : base(default!) { }
    }

    public partial class HttpClient : HttpMessageInvoker
    {
        public HttpClient() : base(default!) { }

        public HttpClient(HttpMessageHandler handler, bool disposeHandler) : base(default!) { }

        public HttpClient(HttpMessageHandler handler) : base(default!) { }

        public Uri BaseAddress { get { throw null; } set { } }

        public Headers.HttpRequestHeaders DefaultRequestHeaders { get { throw null; } }

        public long MaxResponseContentBufferSize { get { throw null; } set { } }

        public TimeSpan Timeout { get { throw null; } set { } }

        public void CancelPendingRequests() { }

        public Threading.Tasks.Task<HttpResponseMessage> DeleteAsync(string requestUri, Threading.CancellationToken cancellationToken) { throw null; }

        public Threading.Tasks.Task<HttpResponseMessage> DeleteAsync(string requestUri) { throw null; }

        public Threading.Tasks.Task<HttpResponseMessage> DeleteAsync(Uri requestUri, Threading.CancellationToken cancellationToken) { throw null; }

        public Threading.Tasks.Task<HttpResponseMessage> DeleteAsync(Uri requestUri) { throw null; }

        protected override void Dispose(bool disposing) { }

        public Threading.Tasks.Task<HttpResponseMessage> GetAsync(string requestUri, HttpCompletionOption completionOption, Threading.CancellationToken cancellationToken) { throw null; }

        public Threading.Tasks.Task<HttpResponseMessage> GetAsync(string requestUri, HttpCompletionOption completionOption) { throw null; }

        public Threading.Tasks.Task<HttpResponseMessage> GetAsync(string requestUri, Threading.CancellationToken cancellationToken) { throw null; }

        public Threading.Tasks.Task<HttpResponseMessage> GetAsync(string requestUri) { throw null; }

        public Threading.Tasks.Task<HttpResponseMessage> GetAsync(Uri requestUri, HttpCompletionOption completionOption, Threading.CancellationToken cancellationToken) { throw null; }

        public Threading.Tasks.Task<HttpResponseMessage> GetAsync(Uri requestUri, HttpCompletionOption completionOption) { throw null; }

        public Threading.Tasks.Task<HttpResponseMessage> GetAsync(Uri requestUri, Threading.CancellationToken cancellationToken) { throw null; }

        public Threading.Tasks.Task<HttpResponseMessage> GetAsync(Uri requestUri) { throw null; }

        public Threading.Tasks.Task<byte[]> GetByteArrayAsync(string requestUri) { throw null; }

        public Threading.Tasks.Task<byte[]> GetByteArrayAsync(Uri requestUri) { throw null; }

        public Threading.Tasks.Task<IO.Stream> GetStreamAsync(string requestUri) { throw null; }

        public Threading.Tasks.Task<IO.Stream> GetStreamAsync(Uri requestUri) { throw null; }

        public Threading.Tasks.Task<string> GetStringAsync(string requestUri) { throw null; }

        public Threading.Tasks.Task<string> GetStringAsync(Uri requestUri) { throw null; }

        public Threading.Tasks.Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content, Threading.CancellationToken cancellationToken) { throw null; }

        public Threading.Tasks.Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content) { throw null; }

        public Threading.Tasks.Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content, Threading.CancellationToken cancellationToken) { throw null; }

        public Threading.Tasks.Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content) { throw null; }

        public Threading.Tasks.Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content, Threading.CancellationToken cancellationToken) { throw null; }

        public Threading.Tasks.Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content) { throw null; }

        public Threading.Tasks.Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content, Threading.CancellationToken cancellationToken) { throw null; }

        public Threading.Tasks.Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content) { throw null; }

        public Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption, Threading.CancellationToken cancellationToken) { throw null; }

        public Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption) { throw null; }

        public override Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, Threading.CancellationToken cancellationToken) { throw null; }

        public Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request) { throw null; }
    }

    public partial class HttpClientHandler : HttpMessageHandler
    {
        public bool AllowAutoRedirect { get { throw null; } set { } }

        public DecompressionMethods AutomaticDecompression { get { throw null; } set { } }

        public bool CheckCertificateRevocationList { get { throw null; } set { } }

        public ClientCertificateOption ClientCertificateOptions { get { throw null; } set { } }

        public System.Security.Cryptography.X509Certificates.X509CertificateCollection ClientCertificates { get { throw null; } }

        public CookieContainer CookieContainer { get { throw null; } set { } }

        public ICredentials Credentials { get { throw null; } set { } }

        public ICredentials DefaultProxyCredentials { get { throw null; } set { } }

        public int MaxAutomaticRedirections { get { throw null; } set { } }

        public int MaxConnectionsPerServer { get { throw null; } set { } }

        public long MaxRequestContentBufferSize { get { throw null; } set { } }

        public int MaxResponseHeadersLength { get { throw null; } set { } }

        public bool PreAuthenticate { get { throw null; } set { } }

        public Collections.Generic.IDictionary<string, object> Properties { get { throw null; } }

        public IWebProxy Proxy { get { throw null; } set { } }

        public Func<HttpRequestMessage, System.Security.Cryptography.X509Certificates.X509Certificate2, System.Security.Cryptography.X509Certificates.X509Chain, Security.SslPolicyErrors, bool> ServerCertificateCustomValidationCallback { get { throw null; } set { } }

        public System.Security.Authentication.SslProtocols SslProtocols { get { throw null; } set { } }

        public virtual bool SupportsAutomaticDecompression { get { throw null; } }

        public virtual bool SupportsProxy { get { throw null; } }

        public virtual bool SupportsRedirectConfiguration { get { throw null; } }

        public bool UseCookies { get { throw null; } set { } }

        public bool UseDefaultCredentials { get { throw null; } set { } }

        public bool UseProxy { get { throw null; } set { } }

        protected override void Dispose(bool disposing) { }

        protected internal override Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, Threading.CancellationToken cancellationToken) { throw null; }
    }

    public enum HttpCompletionOption
    {
        ResponseContentRead = 0,
        ResponseHeadersRead = 1
    }

    public abstract partial class HttpContent : IDisposable
    {
        public Headers.HttpContentHeaders Headers { get { throw null; } }

        public Threading.Tasks.Task CopyToAsync(IO.Stream stream, TransportContext context) { throw null; }

        public Threading.Tasks.Task CopyToAsync(IO.Stream stream) { throw null; }

        protected virtual Threading.Tasks.Task<IO.Stream> CreateContentReadStreamAsync() { throw null; }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        public Threading.Tasks.Task LoadIntoBufferAsync() { throw null; }

        public Threading.Tasks.Task LoadIntoBufferAsync(long maxBufferSize) { throw null; }

        public Threading.Tasks.Task<byte[]> ReadAsByteArrayAsync() { throw null; }

        public Threading.Tasks.Task<IO.Stream> ReadAsStreamAsync() { throw null; }

        public Threading.Tasks.Task<string> ReadAsStringAsync() { throw null; }

        protected abstract Threading.Tasks.Task SerializeToStreamAsync(IO.Stream stream, TransportContext context);
        protected internal abstract bool TryComputeLength(out long length);
    }

    public abstract partial class HttpMessageHandler : IDisposable
    {
        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        protected internal abstract Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, Threading.CancellationToken cancellationToken);
    }

    public partial class HttpMessageInvoker : IDisposable
    {
        public HttpMessageInvoker(HttpMessageHandler handler, bool disposeHandler) { }

        public HttpMessageInvoker(HttpMessageHandler handler) { }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        public virtual Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, Threading.CancellationToken cancellationToken) { throw null; }
    }

    public partial class HttpMethod : IEquatable<HttpMethod>
    {
        public HttpMethod(string method) { }

        public static HttpMethod Delete { get { throw null; } }

        public static HttpMethod Get { get { throw null; } }

        public static HttpMethod Head { get { throw null; } }

        public string Method { get { throw null; } }

        public static HttpMethod Options { get { throw null; } }

        public static HttpMethod Post { get { throw null; } }

        public static HttpMethod Put { get { throw null; } }

        public static HttpMethod Trace { get { throw null; } }

        public bool Equals(HttpMethod other) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(HttpMethod left, HttpMethod right) { throw null; }

        public static bool operator !=(HttpMethod left, HttpMethod right) { throw null; }

        public override string ToString() { throw null; }
    }

    public partial class HttpRequestException : Exception
    {
        public HttpRequestException() { }

        public HttpRequestException(string message, Exception inner) { }

        public HttpRequestException(string message) { }
    }

    public partial class HttpRequestMessage : IDisposable
    {
        public HttpRequestMessage() { }

        public HttpRequestMessage(HttpMethod method, string requestUri) { }

        public HttpRequestMessage(HttpMethod method, Uri requestUri) { }

        public HttpContent Content { get { throw null; } set { } }

        public Headers.HttpRequestHeaders Headers { get { throw null; } }

        public HttpMethod Method { get { throw null; } set { } }

        public Collections.Generic.IDictionary<string, object> Properties { get { throw null; } }

        public Uri RequestUri { get { throw null; } set { } }

        public Version Version { get { throw null; } set { } }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        public override string ToString() { throw null; }
    }

    public partial class HttpResponseMessage : IDisposable
    {
        public HttpResponseMessage() { }

        public HttpResponseMessage(HttpStatusCode statusCode) { }

        public HttpContent Content { get { throw null; } set { } }

        public Headers.HttpResponseHeaders Headers { get { throw null; } }

        public bool IsSuccessStatusCode { get { throw null; } }

        public string ReasonPhrase { get { throw null; } set { } }

        public HttpRequestMessage RequestMessage { get { throw null; } set { } }

        public HttpStatusCode StatusCode { get { throw null; } set { } }

        public Version Version { get { throw null; } set { } }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        public HttpResponseMessage EnsureSuccessStatusCode() { throw null; }

        public override string ToString() { throw null; }
    }

    public abstract partial class MessageProcessingHandler : DelegatingHandler
    {
        protected MessageProcessingHandler() { }

        protected MessageProcessingHandler(HttpMessageHandler innerHandler) { }

        protected abstract HttpRequestMessage ProcessRequest(HttpRequestMessage request, Threading.CancellationToken cancellationToken);
        protected abstract HttpResponseMessage ProcessResponse(HttpResponseMessage response, Threading.CancellationToken cancellationToken);
        protected internal sealed override Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, Threading.CancellationToken cancellationToken) { throw null; }
    }

    public partial class MultipartContent : HttpContent, Collections.Generic.IEnumerable<HttpContent>, Collections.IEnumerable
    {
        public MultipartContent() { }

        public MultipartContent(string subtype, string boundary) { }

        public MultipartContent(string subtype) { }

        public virtual void Add(HttpContent content) { }

        protected override void Dispose(bool disposing) { }

        public Collections.Generic.IEnumerator<HttpContent> GetEnumerator() { throw null; }

        protected override Threading.Tasks.Task SerializeToStreamAsync(IO.Stream stream, TransportContext context) { throw null; }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

        protected internal override bool TryComputeLength(out long length) { throw null; }
    }

    public partial class MultipartFormDataContent : MultipartContent
    {
        public MultipartFormDataContent() { }

        public MultipartFormDataContent(string boundary) { }

        public void Add(HttpContent content, string name, string fileName) { }

        public void Add(HttpContent content, string name) { }

        public override void Add(HttpContent content) { }
    }

    public partial class StreamContent : HttpContent
    {
        public StreamContent(IO.Stream content, int bufferSize) { }

        public StreamContent(IO.Stream content) { }

        protected override Threading.Tasks.Task<IO.Stream> CreateContentReadStreamAsync() { throw null; }

        protected override void Dispose(bool disposing) { }

        protected override Threading.Tasks.Task SerializeToStreamAsync(IO.Stream stream, TransportContext context) { throw null; }

        protected internal override bool TryComputeLength(out long length) { throw null; }
    }

    public partial class StringContent : ByteArrayContent
    {
        public StringContent(string content, Text.Encoding encoding, string mediaType) : base(default!) { }

        public StringContent(string content, Text.Encoding encoding) : base(default!) { }

        public StringContent(string content) : base(default!) { }
    }
}

namespace System.Net.Http.Headers
{
    public partial class AuthenticationHeaderValue
    {
        public AuthenticationHeaderValue(string scheme, string parameter) { }

        public AuthenticationHeaderValue(string scheme) { }

        public string Parameter { get { throw null; } }

        public string Scheme { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public static AuthenticationHeaderValue Parse(string input) { throw null; }

        public override string ToString() { throw null; }

        public static bool TryParse(string input, out AuthenticationHeaderValue parsedValue) { throw null; }
    }

    public partial class CacheControlHeaderValue
    {
        public Collections.Generic.ICollection<NameValueHeaderValue> Extensions { get { throw null; } }

        public TimeSpan? MaxAge { get { throw null; } set { } }

        public bool MaxStale { get { throw null; } set { } }

        public TimeSpan? MaxStaleLimit { get { throw null; } set { } }

        public TimeSpan? MinFresh { get { throw null; } set { } }

        public bool MustRevalidate { get { throw null; } set { } }

        public bool NoCache { get { throw null; } set { } }

        public Collections.Generic.ICollection<string> NoCacheHeaders { get { throw null; } }

        public bool NoStore { get { throw null; } set { } }

        public bool NoTransform { get { throw null; } set { } }

        public bool OnlyIfCached { get { throw null; } set { } }

        public bool Private { get { throw null; } set { } }

        public Collections.Generic.ICollection<string> PrivateHeaders { get { throw null; } }

        public bool ProxyRevalidate { get { throw null; } set { } }

        public bool Public { get { throw null; } set { } }

        public TimeSpan? SharedMaxAge { get { throw null; } set { } }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public static CacheControlHeaderValue Parse(string input) { throw null; }

        public override string ToString() { throw null; }

        public static bool TryParse(string input, out CacheControlHeaderValue parsedValue) { throw null; }
    }

    public partial class ContentDispositionHeaderValue
    {
        protected ContentDispositionHeaderValue(ContentDispositionHeaderValue source) { }

        public ContentDispositionHeaderValue(string dispositionType) { }

        public DateTimeOffset? CreationDate { get { throw null; } set { } }

        public string DispositionType { get { throw null; } set { } }

        public string FileName { get { throw null; } set { } }

        public string FileNameStar { get { throw null; } set { } }

        public DateTimeOffset? ModificationDate { get { throw null; } set { } }

        public string Name { get { throw null; } set { } }

        public Collections.Generic.ICollection<NameValueHeaderValue> Parameters { get { throw null; } }

        public DateTimeOffset? ReadDate { get { throw null; } set { } }

        public long? Size { get { throw null; } set { } }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public static ContentDispositionHeaderValue Parse(string input) { throw null; }

        public override string ToString() { throw null; }

        public static bool TryParse(string input, out ContentDispositionHeaderValue parsedValue) { throw null; }
    }

    public partial class ContentRangeHeaderValue
    {
        public ContentRangeHeaderValue(long from, long to, long length) { }

        public ContentRangeHeaderValue(long from, long to) { }

        public ContentRangeHeaderValue(long length) { }

        public long? From { get { throw null; } }

        public bool HasLength { get { throw null; } }

        public bool HasRange { get { throw null; } }

        public long? Length { get { throw null; } }

        public long? To { get { throw null; } }

        public string Unit { get { throw null; } set { } }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public static ContentRangeHeaderValue Parse(string input) { throw null; }

        public override string ToString() { throw null; }

        public static bool TryParse(string input, out ContentRangeHeaderValue parsedValue) { throw null; }
    }

    public partial class EntityTagHeaderValue
    {
        public EntityTagHeaderValue(string tag, bool isWeak) { }

        public EntityTagHeaderValue(string tag) { }

        public static EntityTagHeaderValue Any { get { throw null; } }

        public bool IsWeak { get { throw null; } }

        public string Tag { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public static EntityTagHeaderValue Parse(string input) { throw null; }

        public override string ToString() { throw null; }

        public static bool TryParse(string input, out EntityTagHeaderValue parsedValue) { throw null; }
    }

    public sealed partial class HttpContentHeaders : HttpHeaders
    {
        internal HttpContentHeaders() { }

        public Collections.Generic.ICollection<string> Allow { get { throw null; } }

        public ContentDispositionHeaderValue ContentDisposition { get { throw null; } set { } }

        public Collections.Generic.ICollection<string> ContentEncoding { get { throw null; } }

        public Collections.Generic.ICollection<string> ContentLanguage { get { throw null; } }

        public long? ContentLength { get { throw null; } set { } }

        public Uri ContentLocation { get { throw null; } set { } }

        public byte[] ContentMD5 { get { throw null; } set { } }

        public ContentRangeHeaderValue ContentRange { get { throw null; } set { } }

        public MediaTypeHeaderValue ContentType { get { throw null; } set { } }

        public DateTimeOffset? Expires { get { throw null; } set { } }

        public DateTimeOffset? LastModified { get { throw null; } set { } }
    }

    public abstract partial class HttpHeaders : Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, Collections.Generic.IEnumerable<string>>>, Collections.IEnumerable
    {
        public void Add(string name, Collections.Generic.IEnumerable<string> values) { }

        public void Add(string name, string value) { }

        public void Clear() { }

        public bool Contains(string name) { throw null; }

        public Collections.Generic.IEnumerator<Collections.Generic.KeyValuePair<string, Collections.Generic.IEnumerable<string>>> GetEnumerator() { throw null; }

        public Collections.Generic.IEnumerable<string> GetValues(string name) { throw null; }

        public bool Remove(string name) { throw null; }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

        public override string ToString() { throw null; }

        public bool TryAddWithoutValidation(string name, Collections.Generic.IEnumerable<string> values) { throw null; }

        public bool TryAddWithoutValidation(string name, string value) { throw null; }

        public bool TryGetValues(string name, out Collections.Generic.IEnumerable<string> values) { throw null; }
    }

    public sealed partial class HttpHeaderValueCollection<T> : Collections.Generic.ICollection<T>, Collections.Generic.IEnumerable<T>, Collections.IEnumerable where T : class
    {
        internal HttpHeaderValueCollection() { }

        public int Count { get { throw null; } }

        public bool IsReadOnly { get { throw null; } }

        public void Add(T item) { }

        public void Clear() { }

        public bool Contains(T item) { throw null; }

        public void CopyTo(T[] array, int arrayIndex) { }

        public Collections.Generic.IEnumerator<T> GetEnumerator() { throw null; }

        public void ParseAdd(string input) { }

        public bool Remove(T item) { throw null; }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

        public override string ToString() { throw null; }

        public bool TryParseAdd(string input) { throw null; }
    }

    public sealed partial class HttpRequestHeaders : HttpHeaders
    {
        internal HttpRequestHeaders() { }

        public HttpHeaderValueCollection<MediaTypeWithQualityHeaderValue> Accept { get { throw null; } }

        public HttpHeaderValueCollection<StringWithQualityHeaderValue> AcceptCharset { get { throw null; } }

        public HttpHeaderValueCollection<StringWithQualityHeaderValue> AcceptEncoding { get { throw null; } }

        public HttpHeaderValueCollection<StringWithQualityHeaderValue> AcceptLanguage { get { throw null; } }

        public AuthenticationHeaderValue Authorization { get { throw null; } set { } }

        public CacheControlHeaderValue CacheControl { get { throw null; } set { } }

        public HttpHeaderValueCollection<string> Connection { get { throw null; } }

        public bool? ConnectionClose { get { throw null; } set { } }

        public DateTimeOffset? Date { get { throw null; } set { } }

        public HttpHeaderValueCollection<NameValueWithParametersHeaderValue> Expect { get { throw null; } }

        public bool? ExpectContinue { get { throw null; } set { } }

        public string From { get { throw null; } set { } }

        public string Host { get { throw null; } set { } }

        public HttpHeaderValueCollection<EntityTagHeaderValue> IfMatch { get { throw null; } }

        public DateTimeOffset? IfModifiedSince { get { throw null; } set { } }

        public HttpHeaderValueCollection<EntityTagHeaderValue> IfNoneMatch { get { throw null; } }

        public RangeConditionHeaderValue IfRange { get { throw null; } set { } }

        public DateTimeOffset? IfUnmodifiedSince { get { throw null; } set { } }

        public int? MaxForwards { get { throw null; } set { } }

        public HttpHeaderValueCollection<NameValueHeaderValue> Pragma { get { throw null; } }

        public AuthenticationHeaderValue ProxyAuthorization { get { throw null; } set { } }

        public RangeHeaderValue Range { get { throw null; } set { } }

        public Uri Referrer { get { throw null; } set { } }

        public HttpHeaderValueCollection<TransferCodingWithQualityHeaderValue> TE { get { throw null; } }

        public HttpHeaderValueCollection<string> Trailer { get { throw null; } }

        public HttpHeaderValueCollection<TransferCodingHeaderValue> TransferEncoding { get { throw null; } }

        public bool? TransferEncodingChunked { get { throw null; } set { } }

        public HttpHeaderValueCollection<ProductHeaderValue> Upgrade { get { throw null; } }

        public HttpHeaderValueCollection<ProductInfoHeaderValue> UserAgent { get { throw null; } }

        public HttpHeaderValueCollection<ViaHeaderValue> Via { get { throw null; } }

        public HttpHeaderValueCollection<WarningHeaderValue> Warning { get { throw null; } }
    }

    public sealed partial class HttpResponseHeaders : HttpHeaders
    {
        internal HttpResponseHeaders() { }

        public HttpHeaderValueCollection<string> AcceptRanges { get { throw null; } }

        public TimeSpan? Age { get { throw null; } set { } }

        public CacheControlHeaderValue CacheControl { get { throw null; } set { } }

        public HttpHeaderValueCollection<string> Connection { get { throw null; } }

        public bool? ConnectionClose { get { throw null; } set { } }

        public DateTimeOffset? Date { get { throw null; } set { } }

        public EntityTagHeaderValue ETag { get { throw null; } set { } }

        public Uri Location { get { throw null; } set { } }

        public HttpHeaderValueCollection<NameValueHeaderValue> Pragma { get { throw null; } }

        public HttpHeaderValueCollection<AuthenticationHeaderValue> ProxyAuthenticate { get { throw null; } }

        public RetryConditionHeaderValue RetryAfter { get { throw null; } set { } }

        public HttpHeaderValueCollection<ProductInfoHeaderValue> Server { get { throw null; } }

        public HttpHeaderValueCollection<string> Trailer { get { throw null; } }

        public HttpHeaderValueCollection<TransferCodingHeaderValue> TransferEncoding { get { throw null; } }

        public bool? TransferEncodingChunked { get { throw null; } set { } }

        public HttpHeaderValueCollection<ProductHeaderValue> Upgrade { get { throw null; } }

        public HttpHeaderValueCollection<string> Vary { get { throw null; } }

        public HttpHeaderValueCollection<ViaHeaderValue> Via { get { throw null; } }

        public HttpHeaderValueCollection<WarningHeaderValue> Warning { get { throw null; } }

        public HttpHeaderValueCollection<AuthenticationHeaderValue> WwwAuthenticate { get { throw null; } }
    }

    public partial class MediaTypeHeaderValue
    {
        protected MediaTypeHeaderValue(MediaTypeHeaderValue source) { }

        public MediaTypeHeaderValue(string mediaType) { }

        public string CharSet { get { throw null; } set { } }

        public string MediaType { get { throw null; } set { } }

        public Collections.Generic.ICollection<NameValueHeaderValue> Parameters { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public static MediaTypeHeaderValue Parse(string input) { throw null; }

        public override string ToString() { throw null; }

        public static bool TryParse(string input, out MediaTypeHeaderValue parsedValue) { throw null; }
    }

    public sealed partial class MediaTypeWithQualityHeaderValue : MediaTypeHeaderValue
    {
        public MediaTypeWithQualityHeaderValue(string mediaType, double quality) : base(default(MediaTypeHeaderValue)!) { }

        public MediaTypeWithQualityHeaderValue(string mediaType) : base(default(MediaTypeHeaderValue)!) { }

        public double? Quality { get { throw null; } set { } }

        public new static MediaTypeWithQualityHeaderValue Parse(string input) { throw null; }

        public static bool TryParse(string input, out MediaTypeWithQualityHeaderValue parsedValue) { throw null; }
    }

    public partial class NameValueHeaderValue
    {
        protected NameValueHeaderValue(NameValueHeaderValue source) { }

        public NameValueHeaderValue(string name, string value) { }

        public NameValueHeaderValue(string name) { }

        public string Name { get { throw null; } }

        public string Value { get { throw null; } set { } }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public static NameValueHeaderValue Parse(string input) { throw null; }

        public override string ToString() { throw null; }

        public static bool TryParse(string input, out NameValueHeaderValue parsedValue) { throw null; }
    }

    public partial class NameValueWithParametersHeaderValue : NameValueHeaderValue
    {
        protected NameValueWithParametersHeaderValue(NameValueWithParametersHeaderValue source) : base(default(NameValueHeaderValue)!) { }

        public NameValueWithParametersHeaderValue(string name, string value) : base(default(NameValueHeaderValue)!) { }

        public NameValueWithParametersHeaderValue(string name) : base(default(NameValueHeaderValue)!) { }

        public Collections.Generic.ICollection<NameValueHeaderValue> Parameters { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public new static NameValueWithParametersHeaderValue Parse(string input) { throw null; }

        public override string ToString() { throw null; }

        public static bool TryParse(string input, out NameValueWithParametersHeaderValue parsedValue) { throw null; }
    }

    public partial class ProductHeaderValue
    {
        public ProductHeaderValue(string name, string version) { }

        public ProductHeaderValue(string name) { }

        public string Name { get { throw null; } }

        public string Version { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public static ProductHeaderValue Parse(string input) { throw null; }

        public override string ToString() { throw null; }

        public static bool TryParse(string input, out ProductHeaderValue parsedValue) { throw null; }
    }

    public partial class ProductInfoHeaderValue
    {
        public ProductInfoHeaderValue(ProductHeaderValue product) { }

        public ProductInfoHeaderValue(string productName, string productVersion) { }

        public ProductInfoHeaderValue(string comment) { }

        public string Comment { get { throw null; } }

        public ProductHeaderValue Product { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public static ProductInfoHeaderValue Parse(string input) { throw null; }

        public override string ToString() { throw null; }

        public static bool TryParse(string input, out ProductInfoHeaderValue parsedValue) { throw null; }
    }

    public partial class RangeConditionHeaderValue
    {
        public RangeConditionHeaderValue(DateTimeOffset date) { }

        public RangeConditionHeaderValue(EntityTagHeaderValue entityTag) { }

        public RangeConditionHeaderValue(string entityTag) { }

        public DateTimeOffset? Date { get { throw null; } }

        public EntityTagHeaderValue EntityTag { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public static RangeConditionHeaderValue Parse(string input) { throw null; }

        public override string ToString() { throw null; }

        public static bool TryParse(string input, out RangeConditionHeaderValue parsedValue) { throw null; }
    }

    public partial class RangeHeaderValue
    {
        public RangeHeaderValue() { }

        public RangeHeaderValue(long? from, long? to) { }

        public Collections.Generic.ICollection<RangeItemHeaderValue> Ranges { get { throw null; } }

        public string Unit { get { throw null; } set { } }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public static RangeHeaderValue Parse(string input) { throw null; }

        public override string ToString() { throw null; }

        public static bool TryParse(string input, out RangeHeaderValue parsedValue) { throw null; }
    }

    public partial class RangeItemHeaderValue
    {
        public RangeItemHeaderValue(long? from, long? to) { }

        public long? From { get { throw null; } }

        public long? To { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public override string ToString() { throw null; }
    }

    public partial class RetryConditionHeaderValue
    {
        public RetryConditionHeaderValue(DateTimeOffset date) { }

        public RetryConditionHeaderValue(TimeSpan delta) { }

        public DateTimeOffset? Date { get { throw null; } }

        public TimeSpan? Delta { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public static RetryConditionHeaderValue Parse(string input) { throw null; }

        public override string ToString() { throw null; }

        public static bool TryParse(string input, out RetryConditionHeaderValue parsedValue) { throw null; }
    }

    public partial class StringWithQualityHeaderValue
    {
        public StringWithQualityHeaderValue(string value, double quality) { }

        public StringWithQualityHeaderValue(string value) { }

        public double? Quality { get { throw null; } }

        public string Value { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public static StringWithQualityHeaderValue Parse(string input) { throw null; }

        public override string ToString() { throw null; }

        public static bool TryParse(string input, out StringWithQualityHeaderValue parsedValue) { throw null; }
    }

    public partial class TransferCodingHeaderValue
    {
        protected TransferCodingHeaderValue(TransferCodingHeaderValue source) { }

        public TransferCodingHeaderValue(string value) { }

        public Collections.Generic.ICollection<NameValueHeaderValue> Parameters { get { throw null; } }

        public string Value { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public static TransferCodingHeaderValue Parse(string input) { throw null; }

        public override string ToString() { throw null; }

        public static bool TryParse(string input, out TransferCodingHeaderValue parsedValue) { throw null; }
    }

    public sealed partial class TransferCodingWithQualityHeaderValue : TransferCodingHeaderValue
    {
        public TransferCodingWithQualityHeaderValue(string value, double quality) : base(default(TransferCodingHeaderValue)!) { }

        public TransferCodingWithQualityHeaderValue(string value) : base(default(TransferCodingHeaderValue)!) { }

        public double? Quality { get { throw null; } set { } }

        public new static TransferCodingWithQualityHeaderValue Parse(string input) { throw null; }

        public static bool TryParse(string input, out TransferCodingWithQualityHeaderValue parsedValue) { throw null; }
    }

    public partial class ViaHeaderValue
    {
        public ViaHeaderValue(string protocolVersion, string receivedBy, string protocolName, string comment) { }

        public ViaHeaderValue(string protocolVersion, string receivedBy, string protocolName) { }

        public ViaHeaderValue(string protocolVersion, string receivedBy) { }

        public string Comment { get { throw null; } }

        public string ProtocolName { get { throw null; } }

        public string ProtocolVersion { get { throw null; } }

        public string ReceivedBy { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public static ViaHeaderValue Parse(string input) { throw null; }

        public override string ToString() { throw null; }

        public static bool TryParse(string input, out ViaHeaderValue parsedValue) { throw null; }
    }

    public partial class WarningHeaderValue
    {
        public WarningHeaderValue(int code, string agent, string text, DateTimeOffset date) { }

        public WarningHeaderValue(int code, string agent, string text) { }

        public string Agent { get { throw null; } }

        public int Code { get { throw null; } }

        public DateTimeOffset? Date { get { throw null; } }

        public string Text { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public static WarningHeaderValue Parse(string input) { throw null; }

        public override string ToString() { throw null; }

        public static bool TryParse(string input, out WarningHeaderValue parsedValue) { throw null; }
    }
}
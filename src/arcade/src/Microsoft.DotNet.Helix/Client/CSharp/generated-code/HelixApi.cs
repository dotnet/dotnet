using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Core;
using Azure.Core.Pipeline;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Microsoft.DotNet.Helix.Client
{
    public partial interface IHelixApi
    {
        HelixApiOptions Options { get; set; }

        IAggregate Aggregate { get; }
        IAnalysis Analysis { get; }
        IInformation Information { get; }
        IJob Job { get; }
        ILogSearch LogSearch { get; }
        IMachine Machine { get; }
        IScaleSets ScaleSets { get; }
        IStorage Storage { get; }
        ITelemetry Telemetry { get; }
        IWorkItem WorkItem { get; }
    }

    public partial interface IServiceOperations<T>
    {
        T Client { get; }
    }

    public partial class HelixApiOptions : ClientOptions
    {
        public HelixApiOptions()
            : this(new Uri("https://helix.dot.net/"))
        {
        }

        public HelixApiOptions(Uri baseUri)
            : this(baseUri, null)
        {
        }

        public HelixApiOptions(TokenCredential credentials)
            : this(new Uri("https://helix.dot.net/"), credentials)
        {
        }

        public HelixApiOptions(Uri baseUri, TokenCredential credentials)
        {
            BaseUri = baseUri;
            Credentials = credentials;
            InitializeOptions();
        }

        partial void InitializeOptions();

        /// <summary>
        ///   The base URI of the service.
        /// </summary>
        public Uri BaseUri { get; }

        /// <summary>
        ///   Credentials to authenticate requests.
        /// </summary>
        public TokenCredential Credentials { get; }
    }

    internal partial class HelixApiResponseClassifier : ResponseClassifier
    {
    }

    public partial class HelixApi : IHelixApi
    {
        private HelixApiOptions _options = null;

        public HelixApiOptions Options
        {
            get => _options;
            set
            {
                _options = value;
                Pipeline = CreatePipeline(value);
            }
        }

        private static HttpPipeline CreatePipeline(HelixApiOptions options)
        {
            return HttpPipelineBuilder.Build(options, Array.Empty<HttpPipelinePolicy>(), Array.Empty<HttpPipelinePolicy>(), new HelixApiResponseClassifier());
        }

        public HttpPipeline Pipeline
        {
            get;
            private set;
        }

        public JsonSerializerSettings SerializerSettings { get; }

        public IAggregate Aggregate { get; }

        public IAnalysis Analysis { get; }

        public IInformation Information { get; }

        public IJob Job { get; }

        public ILogSearch LogSearch { get; }

        public IMachine Machine { get; }

        public IScaleSets ScaleSets { get; }

        public IStorage Storage { get; }

        public ITelemetry Telemetry { get; }

        public IWorkItem WorkItem { get; }


        public HelixApi()
            :this(new HelixApiOptions())
        {
        }

        public HelixApi(HelixApiOptions options)
        {
            Options = options;
            Aggregate = new Aggregate(this);
            Analysis = new Analysis(this);
            Information = new Information(this);
            Job = new Job(this);
            LogSearch = new LogSearch(this);
            Machine = new Machine(this);
            ScaleSets = new ScaleSets(this);
            Storage = new Storage(this);
            Telemetry = new Telemetry(this);
            WorkItem = new WorkItem(this);
            SerializerSettings = new JsonSerializerSettings
            {
                Converters =
                {
                    new StringEnumConverter()
                },
                NullValueHandling = NullValueHandling.Ignore,
            };

            Init();
        }

        /// <summary>
        ///    Optional initialization defined outside of auto-gen code
        /// </summary>
        partial void Init();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void OnFailedRequest(RestApiException ex)
        {
            HandleFailedRequest(ex);
        }

        partial void HandleFailedRequest(RestApiException ex);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string Serialize(string value)
        {
            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string Serialize(bool value)
        {
            return value ? "true" : "false";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string Serialize(int value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string Serialize(long value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string Serialize(float value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string Serialize(double value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string Serialize(Guid value)
        {
            return value.ToString("D", CultureInfo.InvariantCulture);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string Serialize<T>(T value)
        {
            string result = JsonConvert.SerializeObject(value, SerializerSettings);

            if (value is Enum)
            {
                return result.Substring(1, result.Length - 2);
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Deserialize<T>(string value)
        {
            if (typeof(T) == typeof(string))
            {
                return (T)(object)value;
            }

            return JsonConvert.DeserializeObject<T>(value, SerializerSettings);
        }

        public virtual ValueTask<Response> SendAsync(Request request, CancellationToken cancellationToken)
        {
            return Pipeline.SendRequestAsync(request, cancellationToken);
        }
    }

    public class AllPropertiesContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(
            MemberInfo member,
            MemberSerialization memberSerialization)
        {
            var prop = base.CreateProperty(member, memberSerialization);

            if (!prop.Writable)
            {
                var property = member as PropertyInfo;
                if (property != null)
                {
                    var hasPrivateSetter = property.GetSetMethod(true) != null;
                    prop.Writable = hasPrivateSetter;
                }
            }

            return prop;
        }
    }

    public partial class RequestWrapper
    {
        public RequestWrapper(Request request)
        {
            Uri = request.Uri.ToUri();
            Method = request.Method;
            Headers = request.Headers.ToDictionary(h => h.Name, h => h.Value);
        }

        public Uri Uri { get; }
        public RequestMethod Method { get; }
        public IReadOnlyDictionary<string, string> Headers { get; }
    }

    public partial class ResponseWrapper
    {
        public ResponseWrapper(Response response, string responseContent)
        {
            Status = response.Status;
            ReasonPhrase = response.ReasonPhrase;
            Headers = response.Headers;
            Content = responseContent;
        }

        public string Content { get; }

        public ResponseHeaders Headers { get; }

        public string ReasonPhrase { get; }

        public int Status { get; }
    }

    [Serializable]
    public partial class RestApiException : Exception
    {
        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new AllPropertiesContractResolver(),
        };

        private static string FormatMessage(Response response, string responseContent)
        {
            var result = $"The response contained an invalid status code {response.Status} {response.ReasonPhrase}";
            if (responseContent != null)
            {
                result += "\n\nBody: ";
                result += responseContent.Length < 300 ? responseContent : responseContent.Substring(0, 300);
            }
            return result;
        }

        public RequestWrapper Request { get; }

        public ResponseWrapper Response { get; }

        public RestApiException(Request request, Response response, string responseContent)
            : base(FormatMessage(response, responseContent))
        {
            Request = new RequestWrapper(request);
            Response = new ResponseWrapper(response, responseContent);
        }

        protected RestApiException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            var requestString = info.GetString("Request");
            var responseString = info.GetString("Response");
            Request = JsonConvert.DeserializeObject<RequestWrapper>(requestString, SerializerSettings);
            Response = JsonConvert.DeserializeObject<ResponseWrapper>(responseString, SerializerSettings);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            var requestString = JsonConvert.SerializeObject(Request, SerializerSettings);
            var responseString = JsonConvert.SerializeObject(Response, SerializerSettings);

            info.AddValue("Request", requestString);
            info.AddValue("Response", responseString);
            base.GetObjectData(info, context);
        }
    }

    [Serializable]
    public partial class RestApiException<T> : RestApiException
    {
        public T Body { get; }

        public RestApiException(Request request, Response response, string responseContent, T body)
           : base(request, response, responseContent)
        {
            Body = body;
        }

        protected RestApiException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Body = JsonConvert.DeserializeObject<T>(info.GetString("Body"));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            info.AddValue("Body", JsonConvert.SerializeObject(Body));
            base.GetObjectData(info, context);
        }
    }

    public partial class QueryBuilder : List<KeyValuePair<string, string>>
    {
        public QueryBuilder()
        {
        }

        public QueryBuilder(IEnumerable<KeyValuePair<string, string>> parameters)
            :base(parameters)
        {
        }

        public void Add(string key, IEnumerable<string> values)
        {
            foreach (string str in values)
                Add(new KeyValuePair<string, string>(key, str));
        }

        public void Add(string key, string value)
        {
            Add(new KeyValuePair<string, string>(key, value));
        }

        public override string ToString()
        {
          var builder = new StringBuilder();
          for (int index = 0; index < Count; ++index)
          {
            KeyValuePair<string, string> keyValuePair = this[index];
            if (index != 0)
            {
                builder.Append("&");
            }
            builder.Append(UrlEncoder.Default.Encode(keyValuePair.Key));
            builder.Append("=");
            builder.Append(UrlEncoder.Default.Encode(keyValuePair.Value));
          }
          return builder.ToString();
        }
    }

    
    public class ResponseStream : Stream
    {
        private readonly Stream _inner;
        private readonly Response _response;

        public ResponseStream(Stream inner, Response response)
        {
            _inner = inner;
            _response = response;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                _inner.Dispose();
                _response.Dispose();
            }
        }

        #region Forwarding Members

        public override void Flush()
        {
            _inner.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _inner.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _inner.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _inner.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _inner.Write(buffer, offset, count);
        }

        public override string ToString()
        {
            return _inner.ToString();
        }

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return _inner.BeginRead(buffer, offset, count, callback, state);
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return _inner.BeginWrite(buffer, offset, count, callback, state);
        }

        public override void Close()
        {
            _inner.Close();
        }

        public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
        {
            return _inner.CopyToAsync(destination, bufferSize, cancellationToken);
        }

        public override int EndRead(IAsyncResult asyncResult)
        {
            return _inner.EndRead(asyncResult);
        }

        public override void EndWrite(IAsyncResult asyncResult)
        {
            _inner.EndWrite(asyncResult);
        }

        public override Task FlushAsync(CancellationToken cancellationToken)
        {
            return _inner.FlushAsync(cancellationToken);
        }

        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            return _inner.ReadAsync(buffer, offset, count, cancellationToken);
        }

        public override int ReadByte()
        {
            return _inner.ReadByte();
        }

        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            return _inner.WriteAsync(buffer, offset, count, cancellationToken);
        }

        public override void WriteByte(byte value)
        {
            _inner.WriteByte(value);
        }

        public override bool CanRead => _inner.CanRead;

        public override bool CanSeek => _inner.CanSeek;

        public override bool CanWrite => _inner.CanWrite;

        public override long Length => _inner.Length;

        public override long Position
        {
            get => _inner.Position;
            set => _inner.Position = value;
        }

        public override bool CanTimeout => _inner.CanTimeout;

        public override int ReadTimeout { get => _inner.ReadTimeout; set => _inner.ReadTimeout = value; }
        public override int WriteTimeout { get => _inner.WriteTimeout; set => _inner.WriteTimeout = value; }

        #endregion
    }
}

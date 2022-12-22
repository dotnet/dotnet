using System;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Core.Pipeline;

namespace Microsoft.DotNet.Helix.Client
{
    public class HelixApiTokenAuthenticationPolicy : HttpPipelinePolicy
    {
        private readonly TokenCredential _credential;
        private string _headerValue;

        public HelixApiTokenAuthenticationPolicy(TokenCredential credential)
        {
            if (credential == null) throw new ArgumentNullException(nameof(credential));
            _credential = credential;
        }

        /// <inheritdoc />
        public override async ValueTask ProcessAsync(
            HttpMessage message,
            ReadOnlyMemory<HttpPipelinePolicy> pipeline)
        {
            if (_headerValue == null)
            {
                var token = await _credential.GetTokenAsync(new TokenRequestContext(Array.Empty<string>(), message.Request.ClientRequestId), message.CancellationToken).ConfigureAwait(false);
                _headerValue = "token " + token.Token;
            }
            if (_headerValue != null)
            {
                message.Request.Headers.Remove(HttpHeader.Names.Authorization);
                message.Request.Headers.Add(HttpHeader.Names.Authorization, _headerValue);
            }
            await ProcessNextAsync(message, pipeline).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public override void Process(HttpMessage message, ReadOnlyMemory<HttpPipelinePolicy> pipeline)
        {
            throw new NotSupportedException("Sync method not supported");
        }
    }
}

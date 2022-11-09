using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;

namespace Microsoft.DotNet.Helix.Client
{
    public class HelixApiTokenCredential : TokenCredential
    {
        public HelixApiTokenCredential(string token)
        {
            Token = token;
        }

        public string Token { get; }

        public override AccessToken GetToken(TokenRequestContext requestContext, CancellationToken cancellationToken)
        {
            return new AccessToken(Token, DateTimeOffset.MaxValue);
        }

        public override ValueTask<AccessToken> GetTokenAsync(TokenRequestContext requestContext, CancellationToken cancellationToken)
        {
            return new ValueTask<AccessToken>(new AccessToken(Token, DateTimeOffset.MaxValue));
        }
    }
}

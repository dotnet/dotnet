#if !NET452 && !NET46
namespace Microsoft.ApplicationInsights.TestFramework.Helpers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Azure.Core;


    /// <remarks>
    /// Copied from (https://github.com/Azure/azure-sdk-for-net/blob/master/sdk/core/Azure.Core.TestFramework/src/MockCredential.cs).
    /// </remarks>
    public class MockCredential : TokenCredential
    {
        public override ValueTask<AccessToken> GetTokenAsync(TokenRequestContext requestContext, CancellationToken cancellationToken)
        {
            return new ValueTask<AccessToken>(GetToken(requestContext, cancellationToken));
        }

        public override AccessToken GetToken(TokenRequestContext requestContext, CancellationToken cancellationToken)
        {
            return new AccessToken("TEST TOKEN " + string.Join(" ", requestContext.Scopes), DateTimeOffset.MaxValue);
        }
    }
}
#endif

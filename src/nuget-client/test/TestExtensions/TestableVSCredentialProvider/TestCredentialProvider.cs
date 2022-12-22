using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using NuGet.VisualStudio;

namespace NuGet.Test.TestExtensions.TestableVSCredentialProvider
{

    [Export(typeof(IVsCredentialProvider))]
    public class TestCredentialProvider
        : IVsCredentialProvider
    {
        /// <summary>
        /// Get credentials for the supplied package source Uri.
        /// </summary>
        /// <param name="uri">The NuGet package source Uri for which credentials are being requested. Implementors are
        /// expected to first determine if this is a package source for which they can supply credentials.
        /// If not, then Null should be returned.</param>
        /// <param name="proxy">Web proxy to use when comunicating on the network.  Null if there is no proxy
        /// authentication configured.</param>
        /// <param name="isProxyRequest">True if if this request is to get proxy authentication
        /// credentials. If the implementation is not valid for acquiring proxy credentials, then
        /// null should be returned.</param>
        /// <param name="isRetry">True if credentials were previously acquired for this uri, but
        /// the supplied credentials did not allow authorized access.</param>
        /// <param name="nonInteractive">If true, then interactive prompts must not be allowed.</param>
        /// <param name="cancellationToken">This cancellation token should be checked to determine if the
        /// operation requesting credentials has been cancelled.</param>
        /// <returns>Credentials acquired by this provider for the given package source uri.
        /// If the provider does not handle requests for the input parameter set, then null should be returned.
        /// If the provider does handle the request, but cannot supply credentials, an exception should be thrown.</returns>
        public Task<ICredentials> GetCredentialsAsync(Uri uri, IWebProxy proxy, bool isProxyRequest, bool isRetry, bool nonInteractive, CancellationToken cancellationToken)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            var className = this.GetType().Name;

            Trace.TraceInformation($"{className}.GetCredentialsAsync was called for uri {uri}.");

            var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
            var responseUser = query[$"{className}-responseUser"];
            var responsePassword = query[$"{className}-responsePassword"];

            if (responsePassword != null)
            {
                try
                {
                    var token = new SecureString();
                    responsePassword.ToList<char>().ForEach(x => token.AppendChar(x));

                    return Task.FromResult<ICredentials>(new TestCredentials(username: responseUser ?? "username", token: token));
                }
                catch (TaskCanceledException)
                {
                    Trace.TraceError($"{className}.GetCredentialsAsync Credentials acquisition for server {uri} was cancelled by the user.");
                    throw;
                }
                catch (Exception ex)
                {
                    Trace.TraceError($"{className}.GetCredentialsAsync Credentials acquisition for server {uri} failed with error: {ex.Message}");
                }
            }

            // By default, this provider is not applicable.
            Trace.TraceInformation($"{className}.GetCredentialsAsync Provider not applicable.");
            return Task.FromResult<ICredentials>(null);
        }
    }
}

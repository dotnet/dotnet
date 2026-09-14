#nullable disable

using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Configuration;
using NuGet.Credentials;

namespace NuGet.CommandLine
{
    public class SettingsCredentialProvider : ICredentialProvider
    {
        private readonly Configuration.IPackageSourceProvider _packageSourceProvider;
        private readonly Common.ILogger _logger;

        public SettingsCredentialProvider(Configuration.IPackageSourceProvider packageSourceProvider)
            : this(packageSourceProvider, Common.NullLogger.Instance)
        {
        }

        public SettingsCredentialProvider(Configuration.IPackageSourceProvider packageSourceProvider, Common.ILogger logger)
        {
            if (packageSourceProvider == null)
            {
                throw new ArgumentNullException(nameof(packageSourceProvider));
            }

            _packageSourceProvider = packageSourceProvider;
            _logger = logger;
            Id = $"{typeof(SettingsCredentialProvider).Name}_{Guid.NewGuid()}";
        }

        public string Id { get; }

        public Task<CredentialResponse> GetAsync(
            Uri uri,
            IWebProxy proxy,
            CredentialRequestType type,
            string message,
            bool isRetry,
            bool nonInteractive,
            CancellationToken cancellationToken)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            cancellationToken.ThrowIfCancellationRequested();

            ICredentials credentials = GetCredentials(
                uri,
                isRequestCredentials: type != CredentialRequestType.Proxy,
                isRetry);

            CredentialResponse response = credentials == null
                ? new CredentialResponse(CredentialStatus.ProviderNotApplicable)
                : new CredentialResponse(credentials);

            return Task.FromResult(response);
        }

        private ICredentials GetCredentials(Uri uri, bool isRequestCredentials, bool retrying)
        {
            // If we are retrying, the stored credentials must be invalid.
            if (!retrying && isRequestCredentials && TryGetCredentials(uri, out var credentials, out var username))
            {
                _logger.LogMinimal(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        LocalizedResourceManager.GetString(nameof(NuGetResources.SettingsCredentials_UsingSavedCredentials)),
                        username));
                return credentials;
            }

            return null;
        }

        private bool TryGetCredentials(Uri uri, out ICredentials configurationCredentials, out string username)
        {
            var source = _packageSourceProvider.LoadPackageSources().FirstOrDefault(p =>
            {
                Uri sourceUri;
                return p.Credentials != null
                    && p.Credentials.IsValid()
                    && Uri.TryCreate(p.Source, UriKind.Absolute, out sourceUri)
                    && UriEquals(sourceUri, uri);
            });
            if (source == null)
            {
                // The source is not in the config file
                configurationCredentials = null;
                username = null;
                return false;
            }

            configurationCredentials = source.Credentials.ToICredentials();
            username = source.Credentials.Username;
            return true;
        }

        /// <summary>
        /// Determines if the scheme, server and path of two Uris are identical.
        /// </summary>
        private static bool UriEquals(Uri uri1, Uri uri2)
        {
            uri1 = CreateODataAgnosticUri(uri1.OriginalString.TrimEnd('/'));
            uri2 = CreateODataAgnosticUri(uri2.OriginalString.TrimEnd('/'));

            return Uri.Compare(uri1, uri2, UriComponents.SchemeAndServer | UriComponents.Path, UriFormat.SafeUnescaped, StringComparison.OrdinalIgnoreCase) == 0;
        }

        // Bug 2379: SettingsCredentialProvider does not work
        private static Uri CreateODataAgnosticUri(string uri)
        {
            if (uri.EndsWith("$metadata", StringComparison.OrdinalIgnoreCase))
            {
                uri = uri.Substring(0, uri.Length - 9).TrimEnd('/');
            }
            return new Uri(uri);
        }
    }
}

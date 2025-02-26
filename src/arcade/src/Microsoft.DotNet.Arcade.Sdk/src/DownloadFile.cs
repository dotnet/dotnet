// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Tasks = System.Threading.Tasks;

namespace Microsoft.DotNet.Arcade.Sdk
{
    public class DownloadFile : Microsoft.Build.Utilities.Task, ICancelableTask
    {
        /// <summary>
        /// List of URls to attempt download from. Accepted metadata are:
        ///     - Token: Base64 encoded token to be appended to base URL for accessing private locations.
        /// </summary>
        public ITaskItem[] Uris { get; set; }

        public string Uri { get; set; }

        [Required]
        public string DestinationPath { get; set; }

        public bool Overwrite { get; set; }

        /// <summary>
        /// Delay between any necessary retries.
        /// </summary>
        public int RetryDelayMilliseconds { get; set; } = 1000;

        public int Retries { get; set; } = 3;

        public int TimeoutInSeconds { get; set; } = 100;

        private readonly CancellationTokenSource _cancellationSource = new CancellationTokenSource();

        public void Cancel() => _cancellationSource.Cancel();

        private readonly string FileUriProtocol = "file://";


        public override bool Execute()
        {
            if (Retries < 0)
            {
                Log.LogError($"Invalid task parameter value: Retries={Retries}");
                return false;
            }

            if (RetryDelayMilliseconds < 0)
            {
                Log.LogError($"Invalid task parameter value: RetryDelayMilliseconds={RetryDelayMilliseconds}");
                return false;
            }

            if (File.Exists(DestinationPath) && !Overwrite)
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(Uri) && (Uris == null || Uris.Count() == 0)) {
                Log.LogError($"Invalid task parameter value: {nameof(Uri)} and {nameof(Uris)} are empty.");
                return false;
            }

            Directory.CreateDirectory(Path.GetDirectoryName(DestinationPath));

            if (!string.IsNullOrWhiteSpace(Uri)) {
                return DownloadFromUriAsync(Uri).Result;
            }

            if (Uris != null) {
                foreach (var uriConfig in Uris)
                {
                    var uri = uriConfig.ItemSpec;
                    var encodedToken = uriConfig.GetMetadata("token");

                    if (!string.IsNullOrWhiteSpace(encodedToken))
                    {
                        var encodedTokenBytes = System.Convert.FromBase64String(encodedToken);
                        var decodedToken = System.Text.Encoding.UTF8.GetString(encodedTokenBytes);
                        // It's possible that the decoded SAS does not begin with the query string parameter.
                        // Handle cleanly before constructing the final URL
                        if (!decodedToken.StartsWith("?"))
                        {
                            decodedToken = $"?{decodedToken}";
                        }
                        uri = $"{uri}{decodedToken}";
                    }

                    if (DownloadFromUriAsync(uri).Result) {
                        return true;
                    }
                }

                Log.LogError($"Download from all targets failed. List of attempted targets: {string.Join(", ", Uris.Select(m => m.ItemSpec))}");
            }

            Log.LogError($"Failed to download file using addresses in {nameof(Uri)} and/or {nameof(Uris)}.");

            return false;
        }

        private async Tasks.Task<bool> DownloadFromUriAsync(string uri) {
            if (uri.StartsWith(FileUriProtocol, StringComparison.Ordinal))
            {
                var filePath = uri.Substring(FileUriProtocol.Length);

                if (File.Exists(filePath)) {
                    Log.LogMessage($"Copying '{filePath}' to '{DestinationPath}'");
                    File.Copy(filePath, DestinationPath, overwrite: true);
                    return true;
                } else {
                    Log.LogMessage($"'{filePath}' does not exist.");
                    return false;
                }
            }

            Log.LogMessage($"Downloading '{uri}' to '{DestinationPath}'");

            // Configure the cert revocation check in a fail-open state to avoid intermittent failures
            // on Mac if the endpoint is not available. This is only available on .NET Core, but has only been
            // observed on Mac anyway.

#if NET
            using SocketsHttpHandler handler = new SocketsHttpHandler();
            handler.SslOptions.CertificateChainPolicy = new X509ChainPolicy
            {
                // Yes, check revocation.
                // Yes, allow it to be downloaded if needed.
                // Online is the default, but it doesn't hurt to be explicit.
                RevocationMode = X509RevocationMode.Online,
                // Roots never bother with revocation.
                // ExcludeRoot is the default, but it doesn't hurt to be explicit.
                RevocationFlag = X509RevocationFlag.ExcludeRoot,
                // RevocationStatusUnknown at the EndEntity/Leaf certificate will not fail the chain build.
                // RevocationStatusUnknown for any intermediate CA will not fail the chain build.
                // IgnoreRootRevocationUnknown could also be specified, but it won't apply given ExcludeRoot above.
                // The default is that all status codes are bad, this is not the default.
                VerificationFlags =
                    X509VerificationFlags.IgnoreCertificateAuthorityRevocationUnknown |
                    X509VerificationFlags.IgnoreEndRevocationUnknown,
                // Always use the "now" when building the chain, rather than the "now" of when this policy object was constructed.
                VerificationTimeIgnored = true,
            };

            using (var httpClient = new HttpClient(handler))
#else
            using (var httpClient = new HttpClient(new HttpClientHandler { CheckCertificateRevocationList = true }))
#endif
            {
                httpClient.Timeout = TimeSpan.FromSeconds(TimeoutInSeconds);
                try
                {
                    return await DownloadWithRetriesAsync(httpClient, uri);
                }
                catch (AggregateException e)
                {
                    if (e.InnerException is OperationCanceledException)
                    {
                        Log.LogMessage($"Download of '{uri}' to '{DestinationPath}' has been cancelled.");
                        return false;
                    }

                    throw e.InnerException;
                }
            }
        }

        private async Tasks.Task<bool> DownloadWithRetriesAsync(HttpClient httpClient, string uri)
        {            
            int attempt = 0;

            while (true)
            {
                try
                {
                    var httpResponse = await httpClient.GetAsync(uri, _cancellationSource.Token).ConfigureAwait(false);

                    // The Azure Storage REST API returns '400 - Bad Request' in some cases
                    // where the resource is not found on the storage.
                    // https://docs.microsoft.com/en-us/rest/api/storageservices/common-rest-api-error-codes
                    if (httpResponse.StatusCode == HttpStatusCode.NotFound ||
                        httpResponse.ReasonPhrase.StartsWith("The requested URI does not represent any resource on the server.", StringComparison.OrdinalIgnoreCase))
                    {
                        Log.LogMessage($"Problems downloading file from '{uri}'. Does the resource exist on the storage? {httpResponse.StatusCode} : {httpResponse.ReasonPhrase}");
                        return false;
                    }

                    httpResponse.EnsureSuccessStatusCode();

                    using (var outStream = File.Create(DestinationPath))
                    {
                        await httpResponse.Content.CopyToAsync(outStream).ConfigureAwait(false);
                    }

                    return true;
                }
                // Retry cases:
                // 1. Plain Http error
                // 2. IOExceptions that aren't definitely deterministic (such as antivirus was scanning the file)
                // 3. HttpClient Timeouts - these surface as TaskCanceledExceptions that don't match our cancellation token source
                catch (Exception e) when (e is HttpRequestException ||  
                                          e is IOException && !(e is DirectoryNotFoundException || e is PathTooLongException) ||
                                          e is Tasks.TaskCanceledException && ((Tasks.TaskCanceledException)e).CancellationToken != _cancellationSource.Token)
                {
                    attempt++;

                    if (attempt > Retries)
                    {
                        Log.LogMessage($"Failed to download '{uri}' to '{DestinationPath}': {e.Message}");
                        return false;
                    }

                    Log.LogMessage($"Retrying download of '{uri}' to '{DestinationPath}' due to failure: '{e.Message}' ({attempt}/{Retries})");
                    Log.LogErrorFromException(e, true, true, null);

                    await Tasks.Task.Delay(RetryDelayMilliseconds).ConfigureAwait(false);
                    continue;
                }
            }
        }
    }
}


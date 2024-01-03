// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Microsoft.DotNet.Helix.Client;
using Microsoft.DotNet.Helix.Sdk;
using Newtonsoft.Json.Linq;

namespace Microsoft.DotNet.Helix.AzureDevOps
{
    public abstract class AzureDevOpsTask : BaseTask
    {
        private bool InAzurePipeline => !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("BUILD_BUILDNUMBER"));

        protected string GetEnvironmentVariable(string name)
        {
            var result = Environment.GetEnvironmentVariable(name);
            if (string.IsNullOrEmpty(result))
            {
                throw new InvalidOperationException($"Required environment variable {name} not set.");
            }

            return result;
        }

        protected string AccessToken => GetEnvironmentVariable("SYSTEM_ACCESSTOKEN");

        protected string CollectionUri => GetEnvironmentVariable("SYSTEM_TEAMFOUNDATIONCOLLECTIONURI");

        protected string TeamProject => GetEnvironmentVariable("SYSTEM_TEAMPROJECT");

        protected string BuildId => GetEnvironmentVariable("BUILD_BUILDID");

        protected abstract Task ExecuteCoreAsync(HttpClient client);

        public override bool Execute()
        {
            try
            {
                if (!InAzurePipeline)
                {
                    Log.LogError(FailureCategory.Build, "This task must be run inside an Azure Pipelines Build");
                }
                else
                {
                    using (var client = new HttpClient(new HttpClientHandler
                    {
                        AllowAutoRedirect = false,
                        CheckCertificateRevocationList = true,
                    })
                    {
                        DefaultRequestHeaders =
                        {
                            Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes("unused:" + AccessToken))),
                            UserAgent =
                            {
                                Helpers.UserAgentHeaderValue
                            },
                        },
                    })
                    {
                        ExecuteCoreAsync(client).GetAwaiter().GetResult();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogErrorFromException(FailureCategory.Helix, ex, true);
            }

            return !Log.HasLoggedErrors;
        }

        protected async Task RetryAsync(Func<Task> function)
        {
            try
            {
                // Grab the retry logic from the helix api client
                await RetryAsync(
                        async () =>
                        {
                            await function();
                            return false; // the retry function requires a return, give it one
                        },
                        ex => Log.LogMessage(MessageImportance.Low, $"Azure Dev Ops Operation failed: {ex}\nRetrying..."),
                        CancellationToken.None);
            }
            catch (HttpRequestException ex)
            {
                Log.LogError(FailureCategory.Helix, ex.ToString());
            }
        }

        protected async Task<T> RetryAsync<T>(Func<Task<T>> function)
        {
            // Grab the retry logic from the helix api client
            try
            {
                return await RetryAsync(
                        async () => await function(),
                        ex => Log.LogMessage(MessageImportance.Normal, $"Azure Dev Ops Operation failed: {ex}\nRetrying..."),
                        CancellationToken.None);
            }
            catch (HttpRequestException ex)
            {
                Log.LogError(FailureCategory.Helix, ex.ToString());
                return default;
            }
        }

        protected async Task HandleFailedRequest(HttpRequestMessage req, HttpResponseMessage res)
        {
            if (res.StatusCode == HttpStatusCode.Unauthorized || res.StatusCode == HttpStatusCode.Found)
            {
                Log.LogError(
                    FailureCategory.Build,
                    $"A call to an Azure DevOps api returned {(int)res.StatusCode}, which may indicate a bad 'System.AccessToken' value.\n\nPlease Check the 'Make secrets available to builds of forks' in the pipeline pull request validation trigger settings.\nWe have evaluated the security considerations of this setting and have determined that it is fine to use for our public PR validation builds.");
                return;
            }

            var statusCodeValue = (int)res.StatusCode;
            var message = $"Request to {req.RequestUri} returned failed status {statusCodeValue} {res.ReasonPhrase}\n\n{(res.Content != null ? await res.Content.ReadAsStringAsync() : "")}";

            if (statusCodeValue >= 400 && statusCodeValue < 500)
            {
                Log.LogError(FailureCategory.Build, message);
            }
            else
            {
                // we want to engage retry logic from HelixApi.RetryAsync in this case
                Log.LogMessage(MessageImportance.Normal, message);
                throw new HttpRequestException(message);
            }
        }

        protected async Task<JObject> ParseResponseAsync(HttpRequestMessage req, HttpResponseMessage res)
        {
            if (!res.IsSuccessStatusCode)
            {
                await HandleFailedRequest(req, res);
                return null;
            }

            var responseContent = await res.Content.ReadAsStringAsync();

            try
            {
                return JObject.Parse(responseContent);
            }
            catch (Exception)
            {
                Log.LogError(FailureCategory.Helix, $"Request to {req.RequestUri} returned unexpected response: {responseContent}");
            }

            return null;
        }

        private static readonly Random s_rand = new Random();

        public int RetryCount { get; set; } = 15;

        public double RetryBackOffFactor { get; set; } = 1.3;

        protected virtual int GetRetryDelay(int attempt)
        {
            var factor = RetryBackOffFactor;
            var min = (int)(Math.Pow(factor, attempt) * 1000);
            var max = (int)(Math.Pow(factor, attempt + 1) * 1000);
            return s_rand.Next(min, max);
        }

        public static bool IsRetryableHttpException(Exception ex)
        {
            return ex is TaskCanceledException ||
                   ex is OperationCanceledException ||
                   ex is HttpRequestException ||
                   ex is IOException ||
                   ex is SocketException
                ;
        }

        public Task<T> RetryAsync<T>(Func<Task<T>> function, Action<Exception> logRetry,
            CancellationToken cancellationToken)
        {
            return RetryAsync<T>(function, logRetry, IsRetryableHttpException, cancellationToken);
        }

        public async Task<T> RetryAsync<T>(Func<Task<T>> function, Action<Exception> logRetry,
            Func<Exception, bool> isRetryable, CancellationToken cancellationToken)
        {
            var attempt = 0;
            var maxAttempt = RetryCount;
            cancellationToken.ThrowIfCancellationRequested();
            while (true)
            {
                try
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    return await function().ConfigureAwait(false);
                }
                catch (OperationCanceledException ocex) when (ocex.CancellationToken == cancellationToken)
                {
                    throw;
                }
                catch (Exception ex) when (isRetryable(ex))
                {
                    if (attempt >= maxAttempt)
                    {
                        throw;
                    }

                    logRetry(ex);
                }
                cancellationToken.ThrowIfCancellationRequested();
                await Task.Delay(GetRetryDelay(attempt)).ConfigureAwait(false);
                cancellationToken.ThrowIfCancellationRequested();
                attempt++;
            }
        }
    }
}

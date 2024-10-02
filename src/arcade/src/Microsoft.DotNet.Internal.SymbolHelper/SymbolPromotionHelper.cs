// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable enable

using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

using Azure.Core;
using Microsoft.SymbolStore;
using Polly.Retry;
using Polly;
using System.Text.Json;
using System.IO;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace Microsoft.DotNet.Internal.SymbolHelper;

/// <summary>
/// This class implements the symbol request processes described in https://www.osgwiki.com/wiki/Symbols_Publishing_Pipeline_to_SymWeb_and_MSDL
/// Generally publishing workflows will just call RegisterAndPublishRequest
/// - If the request doesn't exist in the symbolrequest service, it will get registered and symbols will be published with the expected TTL and to the internal/public servers as requested.
/// - If the request is registered, the method will update the servers it's published to and the TTL.
/// - If the request is registered and is available in all target servers, only the TTL will be updated.
/// </summary>
public static class SymbolPromotionHelper
{
    private static readonly HttpClient s_client = new();

    private static readonly JsonSerializerOptions s_options = new() { PropertyNameCaseInsensitive = true };
    
    public static readonly ResiliencePropertyKey<ITracer> s_loggerKey = new("logger");

    private static readonly ResiliencePipeline s_retryPipeline = new ResiliencePipelineBuilder()
        .AddRetry(new RetryStrategyOptions
        {
            ShouldHandle = static args =>
            {
                if (args.Outcome.Exception is null) { return ValueTask.FromResult(false); }
                if (args.Outcome.Exception is HttpRequestException httpException)
                {
                    bool isRetryable = (httpException.StatusCode == HttpStatusCode.Unauthorized && args.AttemptNumber == 0) // In case the token was grabbed from cache and died shortly. Retry only once in this case.
                        || httpException.StatusCode == HttpStatusCode.RequestTimeout
                        || httpException.StatusCode == HttpStatusCode.TooManyRequests
                        || httpException.StatusCode == HttpStatusCode.BadGateway
                        || httpException.StatusCode == HttpStatusCode.ServiceUnavailable
                        || httpException.StatusCode == HttpStatusCode.GatewayTimeout;
                    return ValueTask.FromResult(isRetryable);
                }
                return ValueTask.FromResult(false);
            },
            Delay = TimeSpan.FromSeconds(5),
            MaxRetryAttempts = 3,
            BackoffType = DelayBackoffType.Exponential,
            UseJitter = true,
            MaxDelay = TimeSpan.FromMinutes(1),
            OnRetry = args =>
            {
                _ = args.Context.Properties.TryGetValue(s_loggerKey, out ITracer? logger);
                if (args.Outcome.Exception is HttpRequestException httpException)
                {
                    logger?.Information("Try {0} failed with '{1}', delaying {2}", args.AttemptNumber + 1, httpException.Message, args.RetryDelay);
                }
                else
                {
                    logger?.Information("Try {0} failed, delaying {1}", args.AttemptNumber, args.RetryDelay);
                }
                return default;
            }
        })
        .Build();

    public static async Task<bool> RegisterAndPublishRequest(ITracer logger, TokenCredential credential, Environment env,
        string symbolRequestProject, string requestName, uint symbolExpirationInDays, Visibility visibility, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(symbolRequestProject);
        SymbolRequestHelpers.ValidateRequestName(requestName, logger);

        if (!Enum.IsDefined(visibility))
        {
            logger.Error("Invalid visibility requested {0}", visibility);
            return false;
        }

        (string? requestRegistrationEndpoint, string? requestSpecificEndpoint, string? tokenResource) = GetEnvironmentResources(logger, env, symbolRequestProject, requestName);
        if (tokenResource is null || requestSpecificEndpoint is null || requestRegistrationEndpoint is null)
        {
            return false;
        }

        // This does mean an extra request. But this is common for cases where there's promotion of a build into different channels if we decide to optimize for
        // already published requests.
        SymbolRequestStatus? registration = await CheckRequestRegistration(logger, credential, env, symbolRequestProject, requestName, ct);

        if (registration is null)
        {
            DateTime expirationDate = DateTime.UtcNow.AddDays(symbolExpirationInDays);
            JsonObject registrationPayload = new()
            {
                ["requestName"] = requestName,
                ["expirationTime"] = expirationDate
            };

            logger.WriteLine("Requesting request '{0}' registration to '{1}' with expiration {2}", requestName, requestRegistrationEndpoint, expirationDate);
            if (!await SendPostRequestWithRetries(requestRegistrationEndpoint, registrationPayload))
            {
                return false;
            }
        }
        else if (RegistrationIsRequestedInTargetServers(registration, visibility))
        {
            logger.WriteLine("Registration published to all servers already. Requesting expiration in {0} days.", symbolExpirationInDays);
            // if we are in all target servers, then we need to patch the servers with the new TTL which is not a post request. Call the appropriate logic.
            return await UpdateRequestExpiration(logger, credential, env, symbolRequestProject, requestName, symbolExpirationInDays, ct);
        }

        // We get here if we had to register, or if we are not in all target servers. Post the request as usual.
        JsonObject visibilityPayload = new()
        {
            ["publishToInternalServer"] = (visibility >= Visibility.Internal),
            ["publishToPublicServer"] = (visibility >= Visibility.Public)
        };
        
        logger.WriteLine("Requesting '{0}' to be visible in as follows: '{1}'", requestName, visibilityPayload);
        if (!await SendPostRequestWithRetries(requestSpecificEndpoint, visibilityPayload))
        {
            return false;
        }

        logger.WriteLine("Successfully added request to all requested symbol servers.");
        return true;

        async Task<bool> SendPostRequestWithRetries(string url, JsonObject payload)
        {
            ResilienceContext context = ResilienceContextPool.Shared.Get(ct);
            try
            {
                context.Properties.Set(s_loggerKey, logger);
                await s_retryPipeline.ExecuteAsync(async _ =>
                {
                    using HttpRequestMessage registerRequest = new(HttpMethod.Post, url)
                    {
                        Headers =
                        {
                            Authorization = await GetSymbolRequestAuthHeader(credential, tokenResource, ct),
                        },
                        Content = new StringContent(payload.ToString(), Encoding.UTF8, "application/json")
                    };

                    using HttpResponseMessage regResponse = await s_client.SendAsync(registerRequest, ct);
                    regResponse.EnsureSuccessStatusCode();
                }, context);
            }
            catch (Exception ex)
            {
                logger.Error("Request failed: {0}", ex);
                if (ex is HttpRequestException httpEx &&  httpEx.StatusCode == HttpStatusCode.BadRequest)
                {
                    logger.Warning("This request returned BadRequest. Make sure the request '{0}' exists in the temporary server and is finalized.", requestName);
                }
                return false;
            }
            finally
            {
                ResilienceContextPool.Shared.Return(context);
            }

            return true;
        }

        static bool RegistrationIsRequestedInTargetServers(SymbolRequestStatus registration, Visibility visibility) =>
            ((visibility >= Visibility.Public) == registration.PublishToPublicServer)
            && ((visibility >= Visibility.Internal) == registration.PublishToInternalServer);
    }


    public static async Task<SymbolRequestStatus?> CheckRequestRegistration(ITracer logger, TokenCredential credential,
        Environment env, string symbolRequestProject, string requestName, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(symbolRequestProject);

        (_, string? requestSpecificEndpoint, string? tokenResource) = GetEnvironmentResources(logger, env, symbolRequestProject, requestName);
        if (tokenResource is null || requestSpecificEndpoint is null)
        {
            logger.Error("Can't get token resource/registration url for env {0} and project {1}", env, symbolRequestProject);
            return default;
        }

        logger.WriteLine("Requesting status of '{0}' from {1}", requestName, requestSpecificEndpoint);
        ResilienceContext context = ResilienceContextPool.Shared.Get(ct);
        try
        {
            return await s_retryPipeline.ExecuteAsync(async _ =>
            {
                using HttpRequestMessage statusRequest = new(HttpMethod.Get, requestSpecificEndpoint)
                {
                    Headers =
                    {
                        Authorization = await GetSymbolRequestAuthHeader(credential, tokenResource, ct),
                        Accept = { new("application/json") }
                    }
                };
                using HttpResponseMessage statusResponse = await s_client.SendAsync(statusRequest, ct);
                
                if (statusResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    logger.WriteLine("Request '{0}' hasn't been registered", requestName);
                    return null;
                }

                statusResponse.EnsureSuccessStatusCode();
                Stream result = await statusResponse.Content.ReadAsStreamAsync(ct);
                return await JsonSerializer.DeserializeAsync<SymbolRequestStatus>(result, s_options, cancellationToken: ct);
            }, context);
        }
        catch (Exception ex)
        {
            logger.Error("Unable to get status of request: {0}", ex);
            return null;
        }
        finally
        {
            ResilienceContextPool.Shared.Return(context);
        }
    }

    private async static Task<AuthenticationHeaderValue> GetSymbolRequestAuthHeader(TokenCredential credential, string tokenResource, CancellationToken ct)
    {
        AccessToken token = await credential.GetTokenAsync(new TokenRequestContext([tokenResource]), ct);
        return new AuthenticationHeaderValue("Bearer", token.Token);
    }

    public static async Task<bool> UpdateRequestExpiration(ITracer logger, TokenCredential credential,
        Environment env, string symbolRequestProject, string requestName, uint symbolExpirationInDays, CancellationToken ct = default)
    {
        SymbolRequestHelpers.ValidateRequestName(requestName, logger);
        ArgumentException.ThrowIfNullOrWhiteSpace(symbolRequestProject);

        (_, string? requestSpecificEndpoint, string? tokenResource) = GetEnvironmentResources(logger, env, symbolRequestProject, requestName);
        if (tokenResource is null || requestSpecificEndpoint is null)
        {
            logger.Error("Can't get token resource/urls for env {0} and project {1}", env, symbolRequestProject);
            return default;
        }

        DateTime expirationDate = DateTime.UtcNow.AddDays(symbolExpirationInDays);
        JsonObject extensionPayload = new()
        {
            ["expirationTime"] = expirationDate
        };
        logger.WriteLine("Requesting '{0}' to expire at '{1}'", requestName, expirationDate);
        ResilienceContext context = ResilienceContextPool.Shared.Get(ct);
        try
        {

            return await s_retryPipeline.ExecuteAsync(async _ =>
            {
                using HttpRequestMessage statusRequest = new(HttpMethod.Patch, requestSpecificEndpoint)
                {
                    Headers =
                    {
                        Authorization = await GetSymbolRequestAuthHeader(credential, tokenResource, ct),
                    },
                    Content = new StringContent(extensionPayload.ToString(), Encoding.UTF8, "application/json")
                };
                using HttpResponseMessage statusResponse = await s_client.SendAsync(statusRequest, ct);
                statusResponse.EnsureSuccessStatusCode();
                return true;
            }, context);
        }
        catch (Exception ex)
        {
            logger.Error("Unable to extend request lifetime: {0}", ex);
            return false;
        }
        finally
        {
            ResilienceContextPool.Shared.Return(context);
        }
    }

    private static (string? RequestRegistrationEndpoint, string? RequestSpecificEndpoint, string? TokenResource) GetEnvironmentResources(ITracer logger, Environment env, string project, string requestName)
    {

        string? tokenResource = env switch
        {
            Environment.PPE => "api://2748228d-54c2-4c34-a8ed-c4ae31661b39",
            Environment.Prod => "api://30471ccf-0966-45b9-a979-065dbedb24c1",
            _ => default
        };

        if (tokenResource is null)
        {
            logger.Error("Can't get token resource for env {0}", env);
            return default;
        }

        string? requestRegistrationEndpoint = env switch
        {
            Environment.PPE => $"https://symbolrequestppe.trafficmanager.net/projects/{project}/requests",
            Environment.Prod => $"https://symbolrequestprod.trafficmanager.net/projects/{project}/requests",
            _ => default
        };

        if (requestRegistrationEndpoint is null)
        {
            logger.Error("Can't get registration endpoint for env {0}", env);
            return default;
        }

        SymbolRequestHelpers.ValidateRequestName(requestName, logger);
        string requestSpecificEndpoint = $"{requestRegistrationEndpoint}/{requestName}";

        return (requestRegistrationEndpoint, requestSpecificEndpoint, tokenResource);
    }

    public enum Environment
    {
        PPE,
        Prod
    }

    public enum Visibility
    {
        Internal,
        Public
    }

    public enum Status
    {
        NotRequested = 0,
        Submitted,
        Processing,
        Completed
    }

    public enum Result
    {
        Pending = 0,
        Succeeded,
        Failed,
        Cancelled
    }

    public sealed record class SymbolRequestStatus(
        string? RequestName,
        DateTime? ExpirationTime,
        bool PublishToInternalServer,
        Status PublishToInternalServerStatus,
        Result PublishToInternalServerResult,
        string? PublishToInternalServerFailureMessage,
        bool PublishToPublicServer,
        Status PublishToPublicServerStatus,
        Result PublishToPublicServerResult,
        string? PublishToPublicServerFailureMessage,
        string[]? FilesPublishedAsPrivateSymbolsToPublicServer,
        string[]? FilesBlockedFromPublicServer);
}

// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NETFRAMEWORK
using System.Net.Http;
#endif
using System.Text;
using System.Text.Json;

namespace OpenTelemetry.Sampler.AWS;

internal class AWSXRaySamplerClient : IDisposable
{
    private readonly string getSamplingRulesEndpoint;
    private readonly string getSamplingTargetsEndpoint;

    private readonly HttpClient httpClient;
    private readonly string jsonContentType = "application/json";

    public AWSXRaySamplerClient(string host)
    {
        this.getSamplingRulesEndpoint = host + "/GetSamplingRules";
        this.getSamplingTargetsEndpoint = host + "/SamplingTargets";
        this.httpClient = new HttpClient();
    }

    public async Task<List<SamplingRule>> GetSamplingRules()
    {
        List<SamplingRule> samplingRules = [];

        using (var request = new HttpRequestMessage(HttpMethod.Post, this.getSamplingRulesEndpoint)
        {
            Content = new StringContent(string.Empty, Encoding.UTF8, this.jsonContentType),
        })
        {
            var responseJson = await this.DoRequestAsync(this.getSamplingRulesEndpoint, request).ConfigureAwait(false);

            try
            {
                var getSamplingRulesResponse = JsonSerializer
#if NET
                    .Deserialize(responseJson, SourceGenerationContext.Default.GetSamplingRulesResponse);
#else
                    .Deserialize<GetSamplingRulesResponse>(responseJson);
#endif

                if (getSamplingRulesResponse is not null)
                {
                    if (getSamplingRulesResponse.SamplingRuleRecords is not null)
                    {
                        foreach (var samplingRuleRecord in getSamplingRulesResponse.SamplingRuleRecords)
                        {
                            if (samplingRuleRecord.SamplingRule is not null)
                            {
                                samplingRules.Add(samplingRuleRecord.SamplingRule);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AWSSamplerEventSource.Log.FailedToDeserializeResponse(
                    nameof(this.GetSamplingRules),
                    ex.Message);
            }
        }

        return samplingRules;
    }

    public async Task<GetSamplingTargetsResponse?> GetSamplingTargets(GetSamplingTargetsRequest getSamplingTargetsRequest)
    {
        var json = JsonSerializer
#if NET
            .Serialize(getSamplingTargetsRequest, SourceGenerationContext.Default.GetSamplingTargetsRequest);
#else
            .Serialize(getSamplingTargetsRequest);
#endif

        var content = new StringContent(json, Encoding.UTF8, this.jsonContentType);

        using var request = new HttpRequestMessage(HttpMethod.Post, this.getSamplingTargetsEndpoint)
        {
            Content = content,
        };

        var responseJson = await this.DoRequestAsync(this.getSamplingTargetsEndpoint, request).ConfigureAwait(false);

        try
        {
            var getSamplingTargetsResponse = JsonSerializer
#if NET
                .Deserialize(responseJson, SourceGenerationContext.Default.GetSamplingTargetsResponse);
#else
                .Deserialize<GetSamplingTargetsResponse>(responseJson);
#endif

            return getSamplingTargetsResponse;
        }
        catch (Exception ex)
        {
            AWSSamplerEventSource.Log.FailedToDeserializeResponse(
                    nameof(this.GetSamplingTargets),
                    ex.Message);
        }

        return null;
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (disposing)
        {
            this.httpClient?.Dispose();
        }
    }

    private async Task<string> DoRequestAsync(string endpoint, HttpRequestMessage request)
    {
        try
        {
            var response = await this.httpClient.SendAsync(request).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                AWSSamplerEventSource.Log.FailedToGetSuccessResponse(endpoint, response.StatusCode.ToString());
                return string.Empty;
            }

            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return responseString;
        }
        catch (Exception ex)
        {
            AWSSamplerEventSource.Log.ExceptionFromSampler(ex.Message);
            return string.Empty;
        }
    }
}

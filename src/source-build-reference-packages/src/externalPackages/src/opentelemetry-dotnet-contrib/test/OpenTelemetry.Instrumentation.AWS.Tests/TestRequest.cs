// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.Endpoints;
using Amazon.Runtime.EventStreams;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Auth;
using Amazon.Runtime.Internal.Util;

namespace OpenTelemetry.Instrumentation.AWS.Tests;

internal class TestRequest : IRequest
{
    public TestRequest(ParameterCollection? parameters = null)
    {
        this.ParameterCollection = parameters ?? [];
    }

    public string RequestName => throw new NotImplementedException();

    public IDictionary<string, string> Headers => throw new NotImplementedException();

    public bool UseQueryString { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public IDictionary<string, string> Parameters => throw new NotImplementedException();

    public ParameterCollection ParameterCollection { get; }

    public IDictionary<string, string> SubResources => throw new NotImplementedException();

    public string HttpMethod { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public Uri Endpoint { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public string ResourcePath { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public IDictionary<string, string> PathResources => throw new NotImplementedException();

    public int MarshallerVersion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public byte[] Content { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public bool SetContentFromParameters { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public Stream ContentStream { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public long OriginalStreamPosition { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public string OverrideSigningServiceName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public string ServiceName => throw new NotImplementedException();

    public AmazonWebServiceRequest OriginalRequest => throw new NotImplementedException();

    public RegionEndpoint AlternateEndpoint { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public string HostPrefix { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public bool Suppress404Exceptions { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public AWS4SigningResult AWS4SignerResult { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public bool? DisablePayloadSigning { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public AWS4aSigningResult AWS4aSignerResult { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public bool UseChunkEncoding { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public string CanonicalResourcePrefix { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public SignatureVersion SignatureVersion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public string AuthenticationRegion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public string DeterminedSigningRegion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public CoreChecksumAlgorithm SelectedChecksum { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public IDictionary<string, string> TrailingHeaders => throw new NotImplementedException();

    public bool UseDoubleEncoding { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public IPropertyBag EndpointAttributes { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public CompressionEncodingAlgorithm CompressionAlgorithm { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public ChecksumData ChecksumData { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public IEventStreamPublisher EventStreamPublisher { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public IHttpRequestStreamPublisher HttpRequestStreamPublisher { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public DateTime? SignedAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public Version HttpProtocolVersion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public IAuthSchemeOption ChosenAuthScheme { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public void AddPathResource(string key, string value)
    {
        throw new NotImplementedException();
    }

    public void AddSubResource(string subResource)
    {
        throw new NotImplementedException();
    }

    public void AddSubResource(string subResource, string value)
    {
        throw new NotImplementedException();
    }

    public string ComputeContentStreamHash()
    {
        throw new NotImplementedException();
    }

    public string GetHeaderValue(string headerName)
    {
        throw new NotImplementedException();
    }

    public bool HasRequestBody()
    {
        throw new NotImplementedException();
    }

    public bool IsRequestStreamRewindable()
    {
        throw new NotImplementedException();
    }

    public bool MayContainRequestBody()
    {
        throw new NotImplementedException();
    }
}

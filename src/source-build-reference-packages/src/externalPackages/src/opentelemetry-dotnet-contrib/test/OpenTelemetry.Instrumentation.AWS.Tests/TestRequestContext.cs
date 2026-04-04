// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Amazon.Runtime;
using Amazon.Runtime.Identity;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Auth;
using Amazon.Runtime.Internal.Transform;
using Amazon.Runtime.Internal.UserAgent;
using Amazon.Runtime.Internal.Util;

namespace OpenTelemetry.Instrumentation.AWS.Tests;

internal class TestRequestContext(AmazonWebServiceRequest originalRequest, IRequest request) : IRequestContext
{
    private IRequest request = request;

    public AmazonWebServiceRequest OriginalRequest { get; set; } = originalRequest;

    public string RequestName => throw new NotImplementedException();

    public IMarshaller<IRequest, AmazonWebServiceRequest> Marshaller => throw new NotImplementedException();

    public ResponseUnmarshaller Unmarshaller => throw new NotImplementedException();

    public InvokeOptionsBase Options => throw new NotImplementedException();

    public RequestMetrics Metrics => throw new NotImplementedException();

    public AbstractAWSSigner Signer => throw new NotImplementedException();

    public IClientConfig ClientConfig => throw new NotImplementedException();

    public ImmutableCredentials ImmutableCredentials { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public IRequest Request { get => this.request; set => this.request = value; }

    public bool IsSigned { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public bool IsAsync => throw new NotImplementedException();

    public int Retries { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public CapacityManager.CapacityType LastCapacityType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public int EndpointDiscoveryRetries { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public CancellationToken CancellationToken => throw new NotImplementedException();

    public MonitoringAPICallAttempt CSMCallAttempt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public MonitoringAPICallEvent CSMCallEvent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public IServiceMetadata ServiceMetaData => throw new NotImplementedException();

    public bool CSMEnabled => throw new NotImplementedException();

    public bool IsLastExceptionRetryable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public Guid InvocationId => throw new NotImplementedException();

    public IDictionary<string, object> ContextAttributes { get; } = new Dictionary<string, object>();

    public BaseIdentity Identity { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public IHttpRequestStreamHandle RequestStreamHandle { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public UserAgentDetails UserAgentDetails => throw new NotImplementedException();

    public AWSCredentials ExplicitAWSCredentials => throw new NotImplementedException();

    ISigner IRequestContext.Signer { get => this.Signer; set => throw new NotImplementedException(); }
}

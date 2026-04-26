// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Fabric;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.V2.Client;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Client;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.V2.Runtime;

namespace OpenTelemetry.Instrumentation.ServiceFabricRemoting;

/// <summary>
/// This attributes allows to set Fabric TCP transport as the default service remoting transport provider in the assembly and customization of it.
/// </summary>
[AttributeUsage(AttributeTargets.Assembly)]
public sealed class TraceContextEnrichedServiceRemotingProviderAttribute : FabricTransportServiceRemotingProviderAttribute
{
    private const string DefaultV2listenerName = "V2Listener";

    /// <summary>
    /// Initializes a new instance of the <see cref="TraceContextEnrichedServiceRemotingProviderAttribute"/> class.
    /// </summary>
    public TraceContextEnrichedServiceRemotingProviderAttribute()
    {
        this.RemotingClientVersion = RemotingClientVersion.V2;
        this.RemotingListenerVersion = RemotingListenerVersion.V2;
    }

    /// <summary>
    /// Creates a V2 service remoting listener for remoting the service interface.
    /// </summary>
    /// <returns>
    ///  A Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Runtime.FabricTransportServiceRemotingListener
    ///  as Microsoft.ServiceFabric.Services.Remoting.Runtime.IServiceRemotingListener for the specified service implementation.
    /// </returns>
    public override Dictionary<string, Func<ServiceContext, IService, IServiceRemotingListener>> CreateServiceRemotingListeners()
    {
        Dictionary<string, Func<ServiceContext, IService, IServiceRemotingListener>> dictionary = new Dictionary<string, Func<ServiceContext, IService, IServiceRemotingListener>>();

        dictionary.Add(DefaultV2listenerName, (ServiceContext serviceContext, IService serviceImplementation) =>
        {
            FabricTransportRemotingListenerSettings listenerSettings = this.GetListenerSettings(serviceContext);
            ServiceRemotingMessageDispatcher serviceRemotingMessageDispatcher = new ServiceRemotingMessageDispatcher(serviceContext, serviceImplementation);
            ServiceRemotingMessageDispatcherAdapter dispatcherAdapter = new ServiceRemotingMessageDispatcherAdapter(serviceRemotingMessageDispatcher);

            return new FabricTransportServiceRemotingListener(serviceContext, dispatcherAdapter, listenerSettings);
        });

        return dictionary;
    }

    /// <summary>
    /// Creates a V2 service remoting client factory for connecting to the service over remoted service interfaces.
    /// </summary>
    /// <param name="callbackMessageHandler"> The client implementation where the callbacks should be dispatched.</param>
    /// <returns>
    /// A Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Client.FabricTransportServiceRemotingClientFactory
    /// as Microsoft.ServiceFabric.Services.Remoting.V2.Client.IServiceRemotingClientFactory
    /// that can be used with Microsoft.ServiceFabric.Services.Remoting.Client.ServiceProxyFactory
    /// to generate service proxy to talk to a stateless or stateful service over remoted actor interface.
    /// </returns>
    public override IServiceRemotingClientFactory CreateServiceRemotingClientFactoryV2(IServiceRemotingCallbackMessageHandler? callbackMessageHandler)
    {
        FabricTransportRemotingSettings fabricTransportRemotingSettings = new FabricTransportRemotingSettings();
        fabricTransportRemotingSettings.MaxMessageSize = this.GetAndValidateMaxMessageSize(fabricTransportRemotingSettings.MaxMessageSize);
        fabricTransportRemotingSettings.OperationTimeout = this.GetAndValidateOperationTimeout(fabricTransportRemotingSettings.OperationTimeout);
        fabricTransportRemotingSettings.KeepAliveTimeout = this.GetAndValidateKeepAliveTimeout(fabricTransportRemotingSettings.KeepAliveTimeout);
        fabricTransportRemotingSettings.ConnectTimeout = this.GetConnectTimeout(fabricTransportRemotingSettings.ConnectTimeout);

        FabricTransportServiceRemotingClientFactory fabricTransportServiceRemotingClientFactory = new FabricTransportServiceRemotingClientFactory(
            fabricTransportRemotingSettings,
            callbackMessageHandler,
            servicePartitionResolver: null,
            exceptionHandlers: null,
            traceId: null);

        return new TraceContextEnrichedServiceRemotingClientFactoryAdapter(fabricTransportServiceRemotingClientFactory);
    }

    private FabricTransportRemotingListenerSettings GetListenerSettings(ServiceContext serviceContext)
    {
        FabricTransportRemotingListenerSettings listenerSettings = new FabricTransportRemotingListenerSettings();

        listenerSettings.MaxMessageSize = this.GetAndValidateMaxMessageSize(listenerSettings.MaxMessageSize);
        listenerSettings.OperationTimeout = this.GetAndValidateOperationTimeout(listenerSettings.OperationTimeout);
        listenerSettings.KeepAliveTimeout = this.GetAndValidateKeepAliveTimeout(listenerSettings.KeepAliveTimeout);

        return listenerSettings;
    }

    private long GetAndValidateMaxMessageSize(long maxMessageSizeDefault)
    {
        return (this.MaxMessageSize > 0) ? this.MaxMessageSize : maxMessageSizeDefault;
    }

    private TimeSpan GetAndValidateOperationTimeout(TimeSpan operationTimeoutDefault)
    {
        return (this.OperationTimeoutInSeconds > 0) ? TimeSpan.FromSeconds(this.OperationTimeoutInSeconds) : operationTimeoutDefault;
    }

    private TimeSpan GetAndValidateKeepAliveTimeout(TimeSpan keepAliveTimeoutDefault)
    {
        return (this.KeepAliveTimeoutInSeconds > 0) ? TimeSpan.FromSeconds(this.KeepAliveTimeoutInSeconds) : keepAliveTimeoutDefault;
    }

    private TimeSpan GetConnectTimeout(TimeSpan connectTimeoutDefault)
    {
        return (this.ConnectTimeoutInMilliseconds > 0) ? TimeSpan.FromMilliseconds(this.ConnectTimeoutInMilliseconds) : connectTimeoutDefault;
    }
}

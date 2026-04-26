// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Microsoft.ServiceFabric.Actors.Generator;
using Microsoft.ServiceFabric.Actors.Remoting.FabricTransport;
using Microsoft.ServiceFabric.Actors.Remoting.V2.FabricTransport.Client;
using Microsoft.ServiceFabric.Actors.Remoting.V2.FabricTransport.Runtime;
using Microsoft.ServiceFabric.Actors.Remoting.V2.Runtime;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.V2.Client;

namespace OpenTelemetry.Instrumentation.ServiceFabricRemoting;

/// <summary>
/// Sets fabric TCP transport as the default remoting provider for the actors.
/// </summary>
[AttributeUsage(AttributeTargets.Assembly)]
public sealed class TraceContextEnrichedActorRemotingProviderAttribute : FabricTransportActorRemotingProviderAttribute
{
    private const string DefaultV2listenerName = "V2Listener";

    /// <summary>
    /// Initializes a new instance of the <see cref="TraceContextEnrichedActorRemotingProviderAttribute"/> class.
    /// </summary>
    public TraceContextEnrichedActorRemotingProviderAttribute()
    {
        this.RemotingClientVersion = Microsoft.ServiceFabric.Services.Remoting.RemotingClientVersion.V2;
        this.RemotingListenerVersion = Microsoft.ServiceFabric.Services.Remoting.RemotingListenerVersion.V2;
    }

    /// <summary>
    ///     Creates a service remoting listener for remoting the actor interfaces.
    /// </summary>.
    /// <returns>
    ///     A <see cref="FabricTransportActorServiceRemotingListener"/> as <see cref="IServiceRemotingListener"/> for the specified actor service.
    /// </returns>
    public override Dictionary<string, Func<ActorService, IServiceRemotingListener>> CreateServiceRemotingListeners()
    {
        Dictionary<string, Func<ActorService, IServiceRemotingListener>> dictionary = new Dictionary<string, Func<ActorService, IServiceRemotingListener>>();

        dictionary.Add(DefaultV2listenerName, (actorService) =>
        {
            ActorServiceRemotingDispatcher actorServiceRemotingDispatcher = new ActorServiceRemotingDispatcher(actorService, serviceRemotingRequestMessageBodyFactory: null);
            ServiceRemotingMessageDispatcherAdapter dispatcherAdapter = new ServiceRemotingMessageDispatcherAdapter(actorServiceRemotingDispatcher);
            FabricTransportRemotingListenerSettings listenerSettings = this.InitializeListenerSettings(actorService);

            return new FabricTransportActorServiceRemotingListener(actorService, dispatcherAdapter, listenerSettings);
        });

        return dictionary;
    }

    /// <summary>
    ///  Creates a service remoting client factory that can be used by the Microsoft.ServiceFabric.Services.Remoting.V2.Client.ServiceProxyFactory
    ///  to create a proxy for the remoted interface of the service.
    /// </summary>
    /// <param name="callbackMessageHandler">Client implementation where the callbacks should be dispatched.</param>
    /// <returns> An <see cref="IServiceRemotingClientFactory"/>.</returns>
    public override IServiceRemotingClientFactory CreateServiceRemotingClientFactory(IServiceRemotingCallbackMessageHandler? callbackMessageHandler)
    {
        FabricTransportRemotingSettings settings = new FabricTransportRemotingSettings();
        settings.MaxMessageSize = this.GetAndValidateMaxMessageSize(settings.MaxMessageSize);
        settings.OperationTimeout = this.GetAndValidateOperationTimeout(settings.OperationTimeout);
        settings.KeepAliveTimeout = this.GetAndValidateKeepAliveTimeout(settings.KeepAliveTimeout);
        settings.ConnectTimeout = this.GetConnectTimeout(settings.ConnectTimeout);

        FabricTransportActorRemotingClientFactory fabricTransportActorRemotingClientFactory = new FabricTransportActorRemotingClientFactory(
            settings,
            callbackMessageHandler,
            servicePartitionResolver: null,
            exceptionHandlers: null,
            traceId: null);

        return new TraceContextEnrichedServiceRemotingClientFactoryAdapter(fabricTransportActorRemotingClientFactory);
    }

    private static FabricTransportRemotingListenerSettings GetActorListenerSettings(ActorService actorService)
    {
        string sectionName = ActorNameFormat.GetFabricServiceTransportSettingsSectionName(actorService.ActorTypeInformation.ImplementationType);

        bool isSucceded = FabricTransportRemotingListenerSettings.TryLoadFrom(sectionName, out FabricTransportRemotingListenerSettings listenerSettings);
        if (!isSucceded)
        {
            listenerSettings = new FabricTransportRemotingListenerSettings();
        }

        return listenerSettings;
    }

    private FabricTransportRemotingListenerSettings InitializeListenerSettings(ActorService actorService)
    {
        FabricTransportRemotingListenerSettings listenerSettings = GetActorListenerSettings(actorService);

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

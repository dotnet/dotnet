// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Fabric;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Remoting.V2.FabricTransport.Runtime;
using Microsoft.ServiceFabric.Actors.Remoting.V2.Runtime;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.V2.Runtime;

namespace OpenTelemetry.Instrumentation.ServiceFabricRemoting.Tests;

public class MyTestActorService : ActorService, IMyTestActorService
{
    private static readonly Guid ActorId = Guid.Parse("{1F263E8C-78D4-4D91-AAE6-C4B9CE03D6EB}");

    private readonly ServiceRemotingMessageDispatcherAdapter dispatcher;
    private MyTestActor actor;

    public MyTestActorService(StatefulServiceContext context, ActorTypeInformation actorTypeInfo, Func<ActorService, ActorId, ActorBase>? actorFactory = null, Func<ActorBase, IActorStateProvider, IActorStateManager>? stateManagerFactory = null, IActorStateProvider? stateProvider = null, ActorServiceSettings? settings = null)
        : base(context, actorTypeInfo, actorFactory, stateManagerFactory, stateProvider, settings)
    {
        ActorServiceRemotingDispatcher actorServiceRemotingDispatcher = new ActorServiceRemotingDispatcher(this, serviceRemotingRequestMessageBodyFactory: null);

        this.dispatcher = new ServiceRemotingMessageDispatcherAdapter(actorServiceRemotingDispatcher);

        ActorId id = new ActorId(ActorId);
        this.actor = new MyTestActor(this, id);
    }

    public IServiceRemotingMessageHandler Dispatcher
    {
        get { return this.dispatcher; }
    }

    public MyTestActor Actor
    {
        get { return this.actor; }
    }

    public Task<ServiceResponse> TestContextPropagation(string valueToReturn)
    {
        return this.actor.TestContextPropagation(valueToReturn);
    }

    protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
    {
        Func<IServiceRemotingListener> getListenerFunc = () =>
        {
            FabricTransportRemotingListenerSettings listenerSettings = new FabricTransportRemotingListenerSettings();

            return new FabricTransportActorServiceRemotingListener(this, this.dispatcher, listenerSettings);
        };

        ServiceReplicaListener serviceReplicaListener = new ServiceReplicaListener((StatefulServiceContext t) => getListenerFunc(), "V2Listener");

        return [serviceReplicaListener];
    }
}

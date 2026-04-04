// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.Fabric;
using System.Reflection;
using System.Text;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.V2;
using Microsoft.ServiceFabric.Services.Remoting.V2.Runtime;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using ServiceFabric.Mocks;
using ServiceFabric.Mocks.RemotingV2;
using Xunit;

namespace OpenTelemetry.Instrumentation.ServiceFabricRemoting.Tests;

public class ServiceFabricRemotingTests
{
    private const string ValueToSend = "SomeValue";
    private const string BaggageKey = "SomeBaggageKey";
    private const string BaggageValue = "SomeBaggageValue";
    private static readonly ActivitySource ActivitySource = new ActivitySource("ServiceFabricRemotingTests");

    [Fact]
    public async Task TestStatefulServiceContextPropagation_ShouldExtractActivityContextAndBaggage()
    {
        // Arrange
        using TracerProvider provider = Sdk.CreateTracerProviderBuilder()
            .AddServiceFabricRemotingInstrumentation()
            .Build();

        TextMapPropagator propagator = Propagators.DefaultTextMapPropagator;

        StatefulServiceContext serviceContext = MockStatefulServiceContextFactory.Default;
        MockReliableStateManager reliableStateManager = new MockReliableStateManager();

        // We need to create the service, then the dispatcher, and then set the dispatcher on the service, because the dispatcher needs the service as an argument, and the service needs the dispatcher.
        MyTestStatefulService myStatefulService = new MyTestStatefulService(serviceContext, reliableStateManager);
        ServiceRemotingMessageDispatcher serviceRemotingMessageDispatcher = new ServiceRemotingMessageDispatcher(serviceContext, myStatefulService);
        ServiceRemotingMessageDispatcherAdapter dispatcherAdapter = new ServiceRemotingMessageDispatcherAdapter(serviceRemotingMessageDispatcher);
        myStatefulService.SetDispatcher(dispatcherAdapter);

        // We create an ActivityContext and Baggage to inject into the request message, instead of starting a new Activity, because the dispatcher is in the same process as the test, and we don't want to set Activity.Current.
        ActivityContext activityContext = new ActivityContext(ActivityTraceId.CreateRandom(), ActivitySpanId.CreateRandom(), ActivityTraceFlags.Recorded);
        Baggage baggage = Baggage.Create(new Dictionary<string, string> { { BaggageKey, BaggageValue } });

        IServiceRemotingRequestMessageHeader remotingRequestMessageHeader = this.CreateServiceRemotingRequestMessageHeader(typeof(ITestMyStatefulService), nameof(ITestMyStatefulService.TestContextPropagation));

        propagator.Inject(new PropagationContext(activityContext, baggage), remotingRequestMessageHeader, ServiceFabricRemotingUtils.InjectTraceContextIntoServiceRemotingRequestMessageHeader);

        MockServiceRemotingRequestMessageBody messageBody = new MockServiceRemotingRequestMessageBody();
        messageBody.SetParameter(0, "valueToReturn", ValueToSend);

        ServiceRemotingRequestMessageMock requestMessage = new(remotingRequestMessageHeader, messageBody);
        FabricTransportServiceRemotingRequestContextMock remotingRequestContext = new FabricTransportServiceRemotingRequestContextMock();

        // Act
        IServiceRemotingResponseMessage response = await dispatcherAdapter.HandleRequestResponseAsync(remotingRequestContext, requestMessage);

        // Assert
        ServiceResponse serviceResponse = (ServiceResponse)response.GetBody().Get(typeof(ServiceResponse));

        Assert.Equal(ValueToSend, serviceResponse.ParameterValue);
        Assert.Equal(activityContext.TraceId, serviceResponse.ActivityContext.TraceId);
        Assert.Equal(BaggageValue, serviceResponse.Baggage.GetBaggage(BaggageKey));
    }

    [Fact]
    public async Task TestActorContextPropagation_ShouldExtractActivityContextAndBaggage()
    {
        // Arrange
        using TracerProvider provider = Sdk.CreateTracerProviderBuilder()
            .AddServiceFabricRemotingInstrumentation()
            .Build();

        TextMapPropagator propagator = Propagators.DefaultTextMapPropagator;

        // We have to include the method 'TestContextPropagation' in the interface IMyTestActorService a redirected it to the actor because the normal flow in the base classes is not unit-testable.
        // This still allows us to test what we want to test here, which is the method 'HandleRequestResponseAsync' in TraceContextEnrichedActorServiceV2RemotingDispatcher.
        Func<ActorService, ActorId, ActorBase> actorFactory = (service, actorId) => ((MyTestActorService)service).Actor;
        MyTestActorService actorService = MockActorServiceFactory.CreateCustomActorServiceForActor<MyTestActorService, MyTestActor>(actorFactory);

        // We create an ActivityContext and Baggage to inject into the request message, instead of starting a new Activity, because the dispatcher is in the same process as the test, and we don't want to set Activity.Current.
        ActivityContext activityContext = new ActivityContext(ActivityTraceId.CreateRandom(), ActivitySpanId.CreateRandom(), ActivityTraceFlags.Recorded);
        Baggage baggage = Baggage.Create(new Dictionary<string, string> { { BaggageKey, BaggageValue } });

        IServiceRemotingRequestMessageHeader actorRemotingMessageHeaders = this.CreateServiceRemotingRequestMessageHeader(typeof(IMyTestActorService), nameof(IMyTestActorService.TestContextPropagation));

        propagator.Inject(new PropagationContext(activityContext, baggage), actorRemotingMessageHeaders, ServiceFabricRemotingUtils.InjectTraceContextIntoServiceRemotingRequestMessageHeader);

        MockServiceRemotingRequestMessageBody messageBody = new MockServiceRemotingRequestMessageBody();
        messageBody.SetParameter(0, "valueToReturn", ValueToSend);

        ServiceRemotingRequestMessageMock requestMessage = new(actorRemotingMessageHeaders, messageBody);
        FabricTransportServiceRemotingRequestContextMock remotingRequestContext = new FabricTransportServiceRemotingRequestContextMock();

        // Act
        IServiceRemotingResponseMessage response = await actorService.Dispatcher.HandleRequestResponseAsync(remotingRequestContext, requestMessage);

        // Assert
        ServiceResponse serviceResponse = (ServiceResponse)response.GetBody().Get(typeof(ServiceResponse));

        Assert.Equal(ValueToSend, serviceResponse.ParameterValue);
        Assert.Equal(activityContext.TraceId, serviceResponse.ActivityContext.TraceId);
        Assert.Equal(BaggageValue, serviceResponse.Baggage.GetBaggage(BaggageKey));
    }

    [Fact]
    public async Task TestServiceRemotingClientContextPropagation_ShouldInjectActivityContextAndBaggage()
    {
        // Arrange
        using TracerProvider provider = Sdk.CreateTracerProviderBuilder()
            .AddServiceFabricRemotingInstrumentation()
            .AddSource(ActivitySource.Name)
            .Build();

        // The Baggage set here will be used automatically by TraceContextEnrichedServiceRemotingClientAdapter to inject the baggage into the request message.
        Baggage.SetBaggage(BaggageKey, BaggageValue);

        // The activity is created here will be used automatically by TraceContextEnrichedServiceRemotingClientAdapter to inject the context into the request message.
        using (Activity activity = ActivitySource.StartActivity("TestActivity")!)
        {
            ServiceRemotingRequestMessageHeaderMock header = new ServiceRemotingRequestMessageHeaderMock();
            MockServiceRemotingRequestMessageBody messageBody = new MockServiceRemotingRequestMessageBody();
            ServiceRemotingRequestMessageMock requestMessage = new(header, messageBody);

            // The ServiceRemotingClientMock reads the headers from the request and injects them into the response, using OpneTelemetry's TextMapPropagator.
            ServiceRemotingClientMock innerClient = new ServiceRemotingClientMock();
            TraceContextEnrichedServiceRemotingClientAdapter serviceRemotingClientAdapter = new TraceContextEnrichedServiceRemotingClientAdapter(innerClient);

            // Act
            IServiceRemotingResponseMessage response = await serviceRemotingClientAdapter.RequestResponseAsync(requestMessage);

            // Assert
            IServiceRemotingResponseMessageHeader responseMessageHeaders = response.GetHeader();
            PropagationContext propagationContext = Propagators.DefaultTextMapPropagator.Extract(default, responseMessageHeaders, this.ExtractTraceContextFromRequestMessageHeader);

            Assert.Equal(activity.TraceId, propagationContext.ActivityContext.TraceId);
            Assert.Equal(BaggageValue, propagationContext.Baggage.GetBaggage(BaggageKey));
        }
    }

    private ServiceRemotingRequestMessageHeaderMock CreateServiceRemotingRequestMessageHeader(Type interfaceType, string methodName)
    {
        int interfaceId = ServiceFabricUtils.GetInterfaceId(interfaceType);

        MethodInfo methodInfo = interfaceType.GetMethod(methodName)!;
        int methodId = ServiceFabricUtils.GetMethodId(methodInfo);

        ServiceRemotingRequestMessageHeaderMock serviceRemotingRequestMessageHeader = new ServiceRemotingRequestMessageHeaderMock
        {
            InterfaceId = interfaceId,
            MethodId = methodId,
        };

        return serviceRemotingRequestMessageHeader;
    }

    private IEnumerable<string> ExtractTraceContextFromRequestMessageHeader(IServiceRemotingResponseMessageHeader responseMessageHeaders, string headerKey)
    {
        if (responseMessageHeaders.TryGetHeaderValue(headerKey, out byte[] headerValueAsBytes))
        {
            string headerValue = Encoding.UTF8.GetString(headerValueAsBytes);

            return [headerValue];
        }

        return Enumerable.Empty<string>();
    }
}

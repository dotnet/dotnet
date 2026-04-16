# Guidance for instrumenting libraries with Diagnostic Source 

This document provides guidance for adding Diagnostic Source instrumentation to external libraries, which allows Application Insights and other monitoring solutions to collect meaningful and rich telemetry.

## Diagnostic Source and Activities

[Diagnostic Source][DiagnosticSourceGuide] is a simple module that allows code to be instrumented for production-time logging of rich data payloads for consumption within the process that was instrumented. At runtime, consumers can dynamically discover data sources and subscribe to the ones of interest.

[Activity][ActivityGuide] is a class that allows storing and accessing diagnostics context and consuming it with logging system.

Both Diagnostic Source and Activity have been used to instrument [System.Net.Http][SystemNetHttp] and [Microsoft.AspNetCore.Hosting][MicrosoftAspNetCoreHosting].

More recently two new libraries were instrumented and that work was the basis for this guidance. These libraries are client SDKs for [Azure Event Hubs][MicrosoftEventHubs] and [Azure Service Bus][MicrosoftServiceBus], both of which support high throughput scenarios.

[This document][DiagnosticSourceActivityHowto] goes into more details on how to efficiently use Diagnostic Source.

## What should be instrumented

The goal of instrumentation is to give the users the visibility to how particular operations are being performed inside the library. This information can be later used to diagnose performance or issues. It is up to the library authors to identify operations that the library performs and are worthy of monitoring. These operations can match the exposed API but also cover more specific internal logic (like outgoing service calls, retries, locking, cache utilization, awaiting system events, etc.). This way the users can get a good understanding of what's going on under the hood when they need it. 

## Instrumentation 

In the simplest case, the operation that is being monitored has to be wrapped by an activity. However in order to minimize performance impact the activity should be only created if there is any listener waiting for it.

```csharp
    static DiagnosticListener source = new DiagnosticListener("Example.MyLibrary");

    Activity activity = null;
    // create activity only when requested
    if (source.IsEnabled() && source.IsEnabled("Example.MyLibrary.MyOperation"))
    {
        activity = new Activity("Example.MyLibrary.MyOperation");
        source.StartActivity(activity, new { Input = input });
    }

    object output = null;
    try
    {
        // perform the actual operation
        output = RunOperation(input);  
    }
    finally
    {
        // stop activity if started
        if (activity != null)
             source.StopActivity(activity, new { Input = input, Output = output }); 
    }
```
> ### *__TODO__ - provide a pointer to a document with more advanced instrumentation examples (WIP)* 

### Payload

When starting and stopping an activity, it is possible to pass a payload object, which will be available for the listener. This way the listener can access raw, non-serialized data in real time. It can be useful for pulling out additional diagnostic information but also manipulating data as it is being processed (for example, inject diagnostic context before an outbound call is made). Good examples of such payload are ```HttpRequestMessage``` or messages that are being passed through queues.

Mind that payload is not preserved as part of Activity and is only available when activity is started/stopped. Therefore it is a good practice to specify all data that was passed to ```StartActivity()``` in ```StopActivity()``` as well.

#### Payload format

Diagnostic source event and activity start/stop API allows to specify only a single payload object. However, in order to pass more data and allow future additions the recommendation is to use dynamic objects. Since these are .NET objects the names of particular properties should follow [standard .NET naming convention][DotNetPropertyNamingConvention]. 

Here are some recommendations for typical payload property names:

| Property name | Description |
|:--------------|:-------------------|
| `Endpoint` | The ```Uri``` of an endpoint the activity is for (for example, target database, service) |
| `PartitionKey` | The key/ID of the partition the activity is for |
| `Status` | The ```TaskStatus``` of a completed asynchronous task |
| `Exception` | The captured exception object |

### Performance

In order to avoid unnecessary overhead, it is highly recommended to check if there is any listener for given activity: 

```csharp
    source.IsEnabled() && source.IsEnabled("Example.MyLibrary.MyOperation")
```

The parameterless ```source.IsEnabled()``` check should be put before any other as it is very efficient and can virtually prevent any overhead in absence of any listener for given diagnostic source.

It is also possible to specify additional context payload when making that call to allow the listener to make a more informed decision (for example, listeners may only be interested in activities for certain endpoint or partition). However, since this call is performed for every operation, it is NOT recommended to build a dynamic payload object as it was described earlier. Instead the raw input objects should be specified directly in the call - the Diagnostic Source API allows to specify up to two payload objects:

```csharp
    source.IsEnabled("Example.MyLibrary.MyOperation", input1, input2)
```

For more detailed performance considerations, refer to [Diagnostic Source][DiagnosticSourceGuide] and [Activity][ActivityGuide] guides.

### Tags

Activities can have additional tracing information in tags. Tags are meant to be easily, efficiently consumable and are expected to be logged with the activity without any processing. As such they should only contain essential information that should be made available to users to determine if the activity is of interest to them. All of the rich details should be made available in the payload.

Tags can be added to activity at any time of its existence until it is stopped. This way the activity can be enriched with information from operation input, output and/or exceptions. Note however that they should be specified as early as possible so that the listeners can have the most context. In particular, all available tags should be set before starting the activity. 

Tags are not propagated to child activities.

```csharp
    activity.AddTag("size", "small");
    activity.AddTag("color", "blue");
```

#### Tag naming

A single application can include numerous libraries instrumented with Diagnostic Source. In order to maintain a certain level of consistency of the whole diagnostic data, it is recommended to use a common tag naming convention. [OpenTracing][OpenTracingNamingConvention] published naming convention for such tags and can be used as a reference. If no suitable tag was defined there, then a new name can be used, ideally following the same convention.  



[DiagnosticSourceGuide]: https://github.com/dotnet/corefx/blob/master/src/System.Diagnostics.DiagnosticSource/src/DiagnosticSourceUsersGuide.md
[ActivityGuide]: https://github.com/dotnet/corefx/blob/master/src/System.Diagnostics.DiagnosticSource/src/ActivityUserGuide.md
[DiagnosticSourceActivityHowto]: https://github.com/lmolkova/correlation/wiki/How-to-instrument-library-with-Activity-and-DiagnosticSource
[OpenTracingNamingConvention]: https://github.com/opentracing/specification/blob/master/semantic_conventions.md#span-tags-table
[SystemNetHttp]: https://github.com/dotnet/corefx/blob/master/src/System.Net.Http/src/System/Net/Http/DiagnosticsHandler.cs
[MicrosoftAspNetCoreHosting]: https://github.com/aspnet/Hosting/blob/dev/src/Microsoft.AspNetCore.Hosting/Internal/HostingApplicationDiagnostics.cs
[MicrosoftEventHubs]: https://github.com/Azure/azure-event-hubs-dotnet/blob/dev/src/Microsoft.Azure.EventHubs/EventHubsDiagnosticSource.cs
[MicrosoftServiceBus]: https://github.com/Azure/azure-service-bus-dotnet/blob/dev/src/Microsoft.Azure.ServiceBus/ServiceBusDiagnosticsSource.cs
[DotNetPropertyNamingConvention]: https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/names-of-type-members#names-of-properties
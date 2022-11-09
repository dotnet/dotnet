_This document was last updated 11/25/2016 and is applicable to SDK version 2.5.0-beta2._

# Extending Heartbeat Properties in Application Insights #

The .NET Application Insights SDKs provide a new feature called Heartbeat. This feature
sends environment-specific information at pre-configured intervals. The feature will allow 
you to extend the properties that will be sent every interval, and will also allow you to 
set a flag denoting a healthy or unhealthy status for each property you add to the heartbeat 
payload.

## A General Code Example ##

In order to add the extended properties of your choice to the Heartbeat as a developer
of an ITelemetryModule, you can follow the following pattern. Note that you must first add the
properties you want to include in the payload, and you can update (via set) the vaules and health
status of those properties for the duration of the application life cycle.

To add the payload properties, aquire the IHeartbeatPropertyManager module using the internal
`TelemetryModules` singleton. One way in which you could do this is from inside your 
`Initialize(TelemetryConfiguration telemetryConfig)` implementation method, and then iterate 
through the available modules looking for an implementation of `IHeartbeatPropertyManager`:

    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    ...
    // heartbeat property manager to add/update my heartbeat-delivered properties into:
    private IHeartbeatPropertyManager heartbeatManager= null;
    ...
    public void Initialize(TelemetryConfiguration configuration)
    {
        ...
        var modules = TelemetryModules.Instance;
        foreach (var telemetryModule in modules.Modules)
        {
            if (telemetryModule is IHeartbeatPropertyManager)
            {
                this.heartbeatManager = telemetryModule as IHeartbeatPropertyManager;
                break;
            }
        }
        ...

...now you will have a heartbeat property manager that you can work with (or not, remember to
test!). From here (but likely still within your `Initialize` method, you can Add the health
property fields that you want to see in the heartbeat payload for the duration of the
application's lifecycle.

    ...
    this.heartbeatManager.AddHeartbeatProperty(propertyName: "myHeartbeatProperty", propertyValue: this.MyHeartbeatProperty, isHealthy: true);
    ...

Outside of your `Initialize` method, you can update the values you've added by using the 
`SetHeartbeatProperty` method very simply. For instance, you can add a property called 
'myHeartbeatProperty' in the initialize method as above, and then from within a property elsewhere
in your class, you can update the value in the heartbeat payload as follows:

    public string MyHeartbeatProperty
    {
        get => this.myHeartbeatPropertyValue;

        set
        {
            this.myHeartbeatPropertyValue = value;
            if (this.heartbeatManager != null)
            {
                bool myPropIsHealthy = this.SomeTestForHealthStatus(this.myHeartbeatPropertyValue);
                this.heartbeatManager.SetHeartbeatProperty(propertyName: "myHeartbeatProperty", propertyValue: value, isHealthy: myPropIsHealthy);
            }
        }
    }

Using the above example you can add and update properties in the Heartbeat for the
duration of your application's life.

> **Note:** You may also set values for the `HeartbeatInterval` value. This is discouraged, as
your override of this value may adversely affect the consumer's ApplicationInsights.config
configuration in doing so.

You can also set values into the `ExcludedHeartbeatProperties` list if you find it pertinent to
do so.  Setting values into the `ExcludedHeartbeatProperties` is fine, as your module may provide
more detailed information about one of the many SDK-supplied default fields, and in these cases it
is better  to remove the redundancy.

## A Working Example of Extending Properties ##

As of the writing of this document we have made the implementation of ITelemetryModule authors
with a way to extend the content of the heartbeat payload. An example of this has been
constructed in the Microsoft.ApplicationInsights.Web assembly, and can be reviewed here:

https://github.com/Microsoft/ApplicationInsights-dotnet-server/tree/dekeeler/sample-heartbeat-extension

...specifically, you can see how we've provided extra properties to the heartbeat in the
FileDiagnosticsTelemetryModule here:

https://github.com/Microsoft/ApplicationInsights-dotnet-server/blob/2089882ea10a32b88f8d4681eb4819f09a1471bd/Src/HostingStartup/HostingStartup.Net45/FileDiagnosticsTelemetryModule.cs#L134

> **NOTE:** This 'working example' requires that the ApplicationInsights.Web solution is updated to
the latest 'develop' nuget package for the base ApplicationInsights SDK.


# EventCounters Instrumentation for OpenTelemetry .NET - Examples

This is an all-in-one sample that shows how to publish EventCounters using
the OpenTelemetry Metrics Api.

## Expected Output

After running `dotnet run` from this directory

```text
Resource associated with Metric:
    service.name: unknown_service:Examples.EventCounters

Export EventCounters.MyEventSource.MyEventCounter, Meter: OpenTelemetry.Instrumentation.EventCounters/1.0.0.0
(2022-11-01T17:37:37.9046769Z, 2022-11-01T17:37:38.4014060Z] DoubleGauge
Value: 500

Export EventCounters.MyEventSource.MyPollingCounter, Meter: OpenTelemetry.Instrumentation.EventCounters/1.0.0.0
(2022-11-01T17:37:37.9076414Z, 2022-11-01T17:37:38.4014299Z] DoubleGauge
Value: 0.5233669819037192
```

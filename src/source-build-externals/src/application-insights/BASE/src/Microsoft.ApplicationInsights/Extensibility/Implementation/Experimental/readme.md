# Experimental Features

`TelemetryConfiguration.ExperimentalFeatures` is an `IList<string>` of feature names.

For simplicity, there is no data structure to parse or evaluate.
The presence of a string in this collection indicates that a feature is enabled.

## Developers: How to use 

There is no central storage of feature names.
There is no centralized caching of feature evaluations. You should evaluate and cache the result in your class.


```
public class MyClass
{
    private readonly bool isExampleFeatureEnabled;

    public MyClass()
    {
        this.isExampleFeatureEnabled = telemetryConfiguration.EvaluateExperimentalFeature("exampleFeature");
    }

    private void MyMethod()
    {
        if (this.isExampleFeatureEnabled)
        {
            // do stuff here
        }
    }
}
```


## Users: How to configure

### Via applicationinsights.config

```
<?xml version="1.0" encoding="utf-8"?>
<ApplicationInsights xmlns="http://schemas.microsoft.com/ApplicationInsights/2013/Settings">
   <InstrumentationKey>xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxx</InstrumentationKey>
   <TelemetryInitializers>...</TelemetryInitializers>
   <TelemetryModules>...</TelemetryModules>
   <ExperimentalFeatures>
      <Add>exampleFeature</Add>
      <Add>anotherFeature</Add>
   </ExperimentalFeatures>
</ApplicationInsights>
```


### Via code

```
var telemetryConfiguration = new TelemetryConfiguration();
telemetryConfiguration.ExperimentalFeatures.Add("exampleFeature");
telemetryConfiguration.ExperimentalFeatures.Add("anotherFeature");
```

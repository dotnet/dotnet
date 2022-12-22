# DataCollector Migration 
TestPlatform has introduced differences and enhancements to the data collection infrastructure. If you already have written data collectors for previous versions of TestPlatform, then you will need to migrate them.
This document will walk you through the changes that are required to migrate your TMI/MSTest.exe based DataCollector to work with TestPlatform.

## Referencing DataCollector Framework
Previously, `DataCollector` abstract class was present in `Microsoft.VisualStudio.QualityTools.ExecutionCommon.dll` under namespace `Microsoft.VisualStudio.TestTools.Execution`.

Now, `DataCollector`abstract class is present in Object Model. Add reference to [`Microsoft.TestPlatform.ObjectModel`](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/15.5.0-preview-20170810-02)  (preview) nuget package. DataCollector APIs are present under namespace `Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection`.
It is recommended to target your DataCollector to netstandard, so that it can also run cross-platform, i.e. on non-Windows operating systems.
For more info, refer to this [guide](./docs/extensions/datacollector.md).

## Deprecated Attributes
Currently `DataCollectorFriendlyNameAttribute` and `DataCollectorTypeUriAttribute` are the only attributes that are supported. 
Following attributes have been deprecated and should be removed if already being used.
* `DataCollectorEnabledByDefaultAttribute`
* `DataCollectorDescriptionAttribute`
* `DataCollectorConfigurationEditorTypeUriAttribute`
* `DataCollectorSupportsTailoredApplicationsAttribute`
* `DataCollectorVersionObsoleteAttribute` 
* `DataCollectorConfigurationEditorAttribute`

These attributes don't serve any purpose in TestPlatform and therefore, have been removed.

## Deprecated Events
Currently, four events are exposed to DataCollectors through `DataCollectionEvents`:
* Session Start.
* Test Case Start.
* Test Case End.
* Session End.

Following ten events that were exposed to DataCollectors through `DataCollectionEvents` have been deprecated and should be removed if already being used:
* Session Pause.
* Session Resume.
* Test Case Pause.
* Test Case Resume.
* Test Case Reset.
* Test Case Failed.
* Test Step Start.
* Test Step End.
* Data Request.
* Custom Notification.

These events are no longer supported in TestPlatform and hence, these have been removed from DataCollection infrastructure as well.

## DataCollector RunSettings
DataCollector RunSettings are highly compatible in all the versions of TestPlatform and old settings should continue to work with TestPlatform. For more info on runsettings, refer to [Configure DataCollectors](./docs/analyze.md#configure-datacollectors).

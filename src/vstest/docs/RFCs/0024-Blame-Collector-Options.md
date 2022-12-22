# 0024 Blame collector options

## Summary
This note outlines the proposed changes for enhancing test blame data collector options.

## Motivation
To enable a first-class option to support troubleshooting scenarios, including unattended execution in the pipeline, we need some additional options to be added to the existing test blame data collector.

## Proposed Changes
Here are the proposed changes for the test blame data collector.

Test blame data collector has an optional parameter called "CollectDump" which will collect a mini dump of the test host on an unexpected exit of the process.
Optionally one may choose to specify key value pairs to override this:

CollectAlways: If you choose to collect dump even when there is no process crash. It takes values false/true. By default, a dump will be created only on crash.
DumpType: If you choose to collect a full process dump. It takes values mini/full. By default, a mini dump will be created.

### Help text

Here is how the new command line help text will look like:

--Blame|/Blame:[CollectDump];[CollectAlways]=[Value];[DumpType]=[Value]
      Runs the test in blame mode. This option is helpful in isolating the problematic test causing test host crash.
      It creates an output file in the current directory as "Sequence.xml",
      that captures the order of execution of test before the crash.
      You may optionally choose to collect process dump for the test host.
      When you choose to collect dump, by default, a mini dump will be collected on a crash.
      You may also choose to override this default behaviour by some optional parameters:
      CollectAlways - To collect dump on exit even if there is no crash (true/false).
      DumpType - To specify dump type (mini/full).
      Example: /Blame
               /Blame:CollectDump
               /Blame:CollectDump;CollectAlways=true;DumpType=full

These may also be specified in the runsettings as below:

```xml
<RunSettings>
  <LoggerRunSettings>
    <Loggers>
      <Logger friendlyName="blame" enabled="True" />
    </Loggers>
  </LoggerRunSettings>
  <DataCollectionRunSettings>
    <DataCollectors>
      <DataCollector friendlyName="blame" enabled="True">
        <Configuration>
          <ResultsDirectory>C:\TestResults</ResultsDirectory>
          <CollectDump CollectAlways="true" DumpType="mini" />
        </Configuration>
      </DataCollector>
    </DataCollectors>
  </DataCollectionRunSettings>
</RunSettings>
```

### Errors and warnings
If the format of the specified options is wrong, the run will stop and throw an error:
The options specified with /blame is incorrect, please correct it and retry.

If an incorrect parameter is used with blame, it will be ignored with a warning such as:
The blame parameter specified, 'Parameter1' is not valid. Ignoring this parameter.

If an incorrect key is used with blame, it will be ignored with a warning such as:
The blame parameter key specified 'Key1' is not valid. Ignoring this key.

If an incorrect value is specified for a key, it will be ignored with a warning such as:
The blame parameter key 'Key1' can only support values Value1/Value2. Ignoring this key.

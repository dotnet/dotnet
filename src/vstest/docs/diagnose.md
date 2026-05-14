# Test platform diagnostics

This document outlines troubleshooting and diagnosis instructions for test platform.

## Collect traces using command line

The console test runner (`vstest.console`) provides a `diag` switch to collect
traces to a log file. Invoke the runner using below command line:

```shell
> vstest.console testApp.dll --diag:log.txt
```

Trace information will be available in the `log.txt` file. The testhost
execution logs will be in a `log.host.*.txt` file. Testhost logs will be most
interesting since that process actually loads the adapters and runs the tests. It
is also possible provide a path `/tmp/dir/log.txt`. `/tmp/dir` will be created if
it doesn't exist.

Trace level can be changed using below command line:

```shell
> vstest.console testApp.dll --diag:log.txt;tracelevel=verbose
```

Allowed values for tracelevel are: off, error, warning, info and verbose. The default value is verbose.

### Collect traces using VSTEST_DIAG

Since v17.2.0, environment variable `VSTEST_DIAG` can be used to specify the path to the log file. 

### Dotnet test

The `--diag` and `VSTEST_DIAG` option is supported on the `dotnet test` command as well. This will also produce same
set of log files: `log.txt` and `log.*.txt`.

```shell
> dotnet test --diag:log.txt
```

To get traces for VSTest build task, enable the following environment variable:

```shell
> set VSTEST_BUILD_TRACE=1
or
> $env:VSTEST_BUILD_TRACE=1    # powershell
> dotnet test
```

## Collect trace using config file

Tracing can also be enabled via a configuration file. Create
a `vstest.console.exe.config` configuration file in the same directory as
`vstest.console`. Similar configuration is also possible for `testhost.exe` or `testhost.x86.exe`.

This technique is useful to capture logs for IDE/Editor scenarios.

```shell
> where vstest.console
d:\tmp\vstest.console.exe

> notepad d:\tmp\vstest.console.exe.config
```

Add the following content to the config file (`vstest.console.exe.config` or `testhost.exe.config`).

```shell
<configuration>
  <system.diagnostics>
    <sources>
      <source name="TpTrace" 
        switchName="sourceSwitch" 
        switchType="System.Diagnostics.SourceSwitch">
        <listeners>
          <!-- This will spew out all traces to stdout. Add as appropriate -->
          <!-- <add name="console" 
            type="System.Diagnostics.ConsoleTraceListener">
            <filter type="System.Diagnostics.EventTypeFilter" 
              initializeData="Warning"/>
          </add> -->

          <add name="logfile"/>

          <remove name="Default"/>
        </listeners>
      </source>
    </sources>
    <switches>
      <add name="sourceSwitch" value="All"/>
    </switches>
    <sharedListeners>
      <!-- This will add all verbose logs to d:\tmp\log.txt -->
      <add name="logfile" 
        type="System.Diagnostics.TextWriterTraceListener" 
        initializeData="d:\tmp\log.txt">
        <filter type="System.Diagnostics.EventTypeFilter" 
          initializeData="Verbose"/>
      </add>
    </sharedListeners>
  </system.diagnostics>
</configuration>
```

## Collect trace in Azure DevOps CI (VSTest Task)

Add a variable to the build definition named `System.Debug` with value `True`. This can
be done while a new build is queued.

![image](https://github.com/user-attachments/assets/893eee2d-0e20-4b1f-999b-a1a4f8862d12)

This variable will ensure:

* Exact command line of `vstest.console.exe` is reported. It can be used to isolate if there
are errors in translating user inputs in the task to command line options of vstest.console.exe.
* The task invokes `vstest.console.exe` with `/diag` option. The log files can be uploaded
as artifacts for diagnosing errors.

Logs can be downloaded by clicking the right hand `...` menu: 

![image](https://github.com/user-attachments/assets/e8369817-f06a-41c1-b4ae-b042cdc4eaf9)

## Debug test platform components

The runner and test host processes support waiting for debugger attach. You can
enable the following environment variable to keep `vstest.console.exe` waiting for
debugger attach:

```shell
> set VSTEST_RUNNER_DEBUG=1
> vstest.console testApp.dll
Waiting for debugger attach...
Process Id: 51928, Name: vstest.console
```

For test host process (`testhost.exe`, or `dotnet exec testhost.dll` depending on
target framework) use the following environment variable.
Execution will halt until debugger is attached.

```shell
> set VSTEST_HOST_DEBUG=1
> vstest.console testApp.dll
Microsoft (R) Test Execution Command Line Tool Version 15.0.0.0
Copyright (c) Microsoft Corporation.  All rights reserved.

Starting test execution, please wait...
Host debugging is enabled. Please attach debugger to testhost process to continue.
```

## In Visual Studio

Set test logging to trace: 

![image](https://github.com/user-attachments/assets/0ee53d10-4f95-469a-91bd-8577d79b99bc)

Run your tests. 

Go to Output > Tests window: 

Please provide all files from the listed folder, and complete content of the Tests window.

![image](https://github.com/user-attachments/assets/c942642d-e10a-4bf3-9920-8ddd22d99ba8)



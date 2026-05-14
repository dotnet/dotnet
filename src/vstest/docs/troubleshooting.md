# Troubleshooting guide

The goal of this document is to help the test platform users to collect useful information for troubleshooting issues.

## Dotnet CLI

### Collect logs and crash dump

```shell
 dotnet test --diag:log.txt --blame-crash --blame-crash-dump-type full
```

At the end of the execution you'll find the list of artifacts generated with the link to the file dump:

```shell
Starting test execution, please wait...
Logging Vstest Diagnostics in file: C:\git\issue\bug\log.txt
A total of 1 test files matched the specified pattern.
...
   --- End of inner exception stack trace ---.
...
Attachments:
  C:\git\issues\bug\TestResults\620c075b-e035-41d2-b950-159f57abc604\Sequence_bfcc4d8558654413a3fb2f5164695bf6.xml
  C:\git\issues\bug\TestResults\620c075b-e035-41d2-b950-159f57abc604\dotnet.exe_11876_1660721586_crashdump.dmp
```

You'll find 3 files for logs(runner, datacollector, host), datacollector one can be missing.

```shell
-a----         8/17/2022   9:33 AM          30001 log.datacollector.22-08-17_09-32-54_50516_1.txt
-a----         8/17/2022   9:33 AM          37222 log.host.22-08-17_09-32-55_24965_7.txt
-a----         8/17/2022   9:33 AM         200345 log.txt
```
## vstest.console.exe  

### Collect logs with `/Diag:`

```shell
vstest.console.exe ...\TestProject1.dll /Collect:"Code Coverage" /Diag:log.txt
```
At the end of the execution you'll find the list of logs generated:
```shell
dir *.txt
 Volume in drive C has no label.
 Volume Serial Number is FA60-B142

 Directory of ...\net7.0

12/08/2022  12:00 PM           173,002 log.datacollector.22-12-08_12-00-14_68705_1.txt
12/08/2022  12:01 PM           173,000 log.datacollector.22-12-08_12-01-12_19459_1.txt
12/08/2022  12:00 PM            56,017 log.host.22-12-08_12-00-15_88174_7.txt
12/08/2022  12:01 PM            55,986 log.host.22-12-08_12-01-12_94082_7.txt
12/08/2022  12:01 PM           292,548 log.txt
               5 File(s)        750,553 bytes
               0 Dir(s)  263,236,558,848 bytes free
```

## Azure DevOps

### @VSTest2 task

#### Collect diagnostic logs

##### Using `otherConsoleOptions: /Diag:vstestlog.txt`

```yaml
  - task: VSTest@2
    inputs:
      displayName: "VsTest - testAssemblies"
      inputs:
      testAssemblyVer2: |
        ...
      otherConsoleOptions: '/Diag:vstestlog.diag'

  - task: CopyFiles@2
    displayName: Copy vstestlog logs to staging
    inputs:
      contents: '**/*vstestlog*.diag'
      targetFolder: $(Build.ArtifactStagingDirectory)/vstestlog
    condition: always()

  - task: PublishPipelineArtifact@1
    displayName: Publish vstestlog log
    inputs:
      targetPath: $(Build.ArtifactStagingDirectory)/vstestlog
      artifactName: vstestlog      
    condition: always()
```

You can now zip/download all logs from the published artifacts view under the `vstestlog` folder.  

##### Using `System.Debug` environment variable

For some scenarios, it is not possible use the `otherConsoleOptions` (e.g., parallel execution).

```yaml
jobs:
- job: ...

  variables:
    - name: System.Debug
      value: true

  steps:
  ...
  - task: VSTest@2
    inputs:
      displayName: "VsTest - testAssemblies"
      inputs:
      testAssemblyVer2: |
        ...
  
  - task: CopyFiles@2
    displayName: Copy vstestlog logs to staging
    inputs:
      sourceFolder: $(Agent.TempDirectory)
      contents: '**/*.diag'
      targetFolder: $(Build.ArtifactStagingDirectory)/vstestlog
    condition: always()

  - task: PublishPipelineArtifact@1
    displayName: Publish vstestlog log
    inputs:
      targetPath: $(Build.ArtifactStagingDirectory)/vstestlog
      artifactName: vstestlog
    condition: always()
```

You can now zip/download all logs from the published artifacts view under the `vstestlog` folder.  

#### Collect logs and crash dump/hang

##### Using `otherConsoleOptions: /Blame`  

```yaml
jobs:
- job: ...

  variables:
    - name: System.Debug
      value: true

  steps:
  ...
  - task: VSTest@2
    inputs:
      displayName: "VsTest - testAssemblies"
      inputs:
      testAssemblyVer2: |
        ...

      otherConsoleOptions: otherConsoleOptions: '/Blame:"CollectDump;DumpType=Full;CollectHangDump;TestTimeout=30min;HangDumpType=Full"'
    condition: always()
    continueOnError: true

  - task: CopyFiles@2
    displayName: Copy test logs to staging
    inputs:
      sourceFolder: $(Agent.TempDirectory)
      contents: '**/*.diag'
      targetFolder: $(Build.ArtifactStagingDirectory)/vstestlog
    continueOnError: true
    condition: always()

  - task: PublishPipelineArtifact@1
    displayName: Publish vstestlog log
    inputs:
      targetPath: $(Build.ArtifactStagingDirectory)/vstestlog
      artifactName: vstestlog
    condition: always()
    continueOnError: true
```

You can now zip/download all logs from the published artifacts view under the `vstestlog` folder and you can find the dump using the `Attachments` tab under `Tests` selecting the parent (first) test node.  

##### Using *.runsettings file

 For some scenarios, it is not possible use the `otherConsoleOptions` (e.g., parallel execution).

```yaml
jobs:
- job: ...

  variables:
    - name: System.Debug
      value: true

  steps:
  ...
  - task: VSTest@2
    inputs:
      displayName: "VsTest - testAssemblies"
      inputs:
      testAssemblyVer2: |
        ...
      runSettingsFile: ./config.runsettings
    continueOnError: true

  - task: CopyFiles@2
    displayName: Copy test logs to staging
    inputs:
      sourceFolder: $(Agent.TempDirectory)
      contents: '**/*.diag'
      targetFolder: $(Build.ArtifactStagingDirectory)/vstestlog
    condition: always()
    continueOnError: true

  - task: PublishPipelineArtifact@1
    displayName: Publish vstestlog log
    inputs:
      targetPath: $(Build.ArtifactStagingDirectory)/vstestlog
      artifactName: vstestlog
    condition: always()
    continueOnError: true
```

`config.runsettings` file

```xml
<RunSettings>
  <DataCollectionRunSettings>
    <DataCollectors>
      <DataCollector friendlyName="blame" enabled="True">
        <Configuration>
          <CollectDump DumpType="Full" />
          <CollectDumpOnTestSessionHang TestTimeout="30min" HangDumpType="Full" />
        </Configuration>
      </DataCollector>
    </DataCollectors>
  </DataCollectionRunSettings>
</RunSettings>
```

### DotNetCoreCLI@2 task  

#### Collect process dump using Procdump on Windows (i.e. OutOfMemory)

```yaml
  variables:
    - name: System.Debug
      value: true
    - name: PROCDUMP_PATH
      value: $(Agent.ToolsDirectory)\Procdump
    - name: VSTEST_DUMP_FORCEPROCDUMP
      value: 1
...
  - task: PowerShell@2
    displayName: 'Download ProcDump'
    inputs:
      targetType: inline
      script: |
        Invoke-WebRequest -Uri "https://download.sysinternals.com/files/Procdump.zip" -OutFile $(Agent.TempDirectory)\Procdump.zip
        Expand-Archive -LiteralPath $(Agent.TempDirectory)\Procdump.zip -DestinationPath $(Agent.ToolsDirectory)\Procdump -Force
...
  - task: DotNetCoreCLI@2
    displayName: Test project
    inputs:
      command: test
      arguments: --blame-crash --blame-crash-collect-always true --diag:log.txt
```

## Visual Studio

### Enable Diagnostic logs

* Go to Visual Studio options page (`Tools/Options`)
* Select `Test` and then `General`
* Under `Logging`, change `Logging level` to `Trace (Includes Platform logs)`
* Close the `Options` window
* Open the `Output` window and select `Tests` output
* Locate the entry similar to `C:\Users\<YOUR_USER>\AppData\Local\Temp\TestPlatformLogs\<DATE_TIME>` (e.g. `C:\Users\johndoe\AppData\Local\Temp\TestPlatformLogs\2022_07_14_15_24_06_19400`) and open the folder
* Run your tests
* Create a zip with all files available

Note: these logs could contain sensitive information (paths, project name...). Make sure to clean them or use the Visual Studio `Send Feedback` button. Don't put anything you want to keep private in the title or content of the initial report, which is public. Instead, say that you'll send details privately in a separate comment. Once the problem report is created, it's now possible to specify who can see your replies and attachments.

## Use procdump on Windows

Sometimes it's not possible to take the dump using test platform tool because the crash happen before we're able to attach to the process to take the dump self. In that situation we need a way to register for dump at process startup level.  
To achieve it we can use [procdump](https://docs.microsoft.com/sysinternals/downloads/procdump) that will install machine wide Just-in-time (AeDebug) debugger.

```shell
PS C:\tools\Procdump> .\procdump.exe -i C:\tools\Procdump\dumps

ProcDump v10.11 - Sysinternals process dump utility
Copyright (C) 2009-2021 Mark Russinovich and Andrew Richards
Sysinternals - www.sysinternals.com

Set to:
  HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\AeDebug
    (REG_SZ) Auto     = 1
    (REG_SZ) Debugger = "C:\tools\Procdump\procdump.exe" -accepteula -j "C:\tools\Procdump\dumps" %ld %ld %p

Set to:
  HKLM\SOFTWARE\Wow6432Node\Microsoft\Windows NT\CurrentVersion\AeDebug
    (REG_SZ) Auto     = 1
    (REG_SZ) Debugger = "C:\tools\Procdump\procdump.exe" -accepteula -j "C:\tools\Procdump\dumps" %ld %ld %p

ProcDump is now set as the Just-in-time (AeDebug) debugger.
```

After you can run your application and in case of crash a dump will be automatically taken inside the `C:\tools\Procdump\dumps` directory.

```
PS C:\tools\Procdump> ls C:\tools\Procdump\dumps


    Directory: C:\tools\Procdump\dumps


Mode                 LastWriteTime         Length Name
----                 -------------         ------ ----
-a----         8/17/2022   9:42 AM        6161605 dotnet.exe_220817_094234.dmp
```

You can unistall the automatic generation running at the end of the collection phase

```shell
.\procdump.exe -u
```

Keep in mind that this mode will collect machine wide crash so every process in the machine that will crash will collect a dump in the folder.

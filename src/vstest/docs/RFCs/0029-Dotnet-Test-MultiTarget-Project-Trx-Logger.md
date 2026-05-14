# 0029 Dotnet test on multi target project logging on multiple targets

## Summary
Dotnet test on a multi-target projects logs only the last target. In multi target project, the log file name provided is being overwritten and ending up in data loss. Issue: [https://github.com/microsoft/vstest/issues/1603](https://github.com/microsoft/vstest/issues/1603)

Example: `dotnet test "--logger:trx;LogFileName=results.trx"` on a multi-target project (e.g. `<TargetFrameworks>netcoreapp2.0;net45</TargetFrameworks>` in the .csproj) causes the logger to only log on the last target.

## Design

### Option 1: Separate folders for separate targets

There are 2 ways to specify the LogFileName.  
&nbsp;&nbsp;&nbsp;&nbsp;a. Relative filename: `dotnet test "--logger:trx;LogFileName=results.trx"`  
&nbsp;&nbsp;&nbsp;&nbsp;b. Absolute path: `dotnet test "--logger:trx;LogFileName=d:/results.trx"`  

In case of (a), the trx is generated in the test results directory of the multi target project.

Solution: Generate different target trx files in separate folders inside the test results directory.  
&nbsp;&nbsp;&nbsp;&nbsp;UnitTestProject/TestResults/net45/results.trx  
&nbsp;&nbsp;&nbsp;&nbsp;UnitTestProject/TestResults/netcoreapp2.0/results.trx  

For case (b), the trx file will still be overwritten with a warning.

### Option 2: Specify LogFilePrefix Parameter with timestamp appended

Introduce a LogFilePrefix parameter in trx logger. Example : `dotnet test "--logger:trx;LogFilePrefix=results"`  
With this, if we want all trx files in a multi target project, instead of specifying a logFileName, we provide a LogFilePrefix.  
The trx file name prefix is appended with time stamp to generate multi target trx files.  

For example : `dotnet test "--logger:trx;LogFilePrefix=results"` will generate  
&nbsp;&nbsp;&nbsp;&nbsp;UnitTestProject/TestResults/results_2018-12-24_14-01-07-176.trx  
&nbsp;&nbsp;&nbsp;&nbsp;UnitTestProject/TestResults/results_2018-12-24_14-01-08-111.trx  

The behavior of  `dotnet test "--logger:trx;LogFileName=results.trx"` will remain same with overwriting being done, and warning to switch to LogFilePrefix.

### Option 3: Specify LogFilePrefix Parameter with target framework appended

Introduce a LogFilePrefix parameter in trx logger. Example : `dotnet test "--logger:trx;LogFilePrefix=results"`  
With this, if we want all trx files in a multi target project, instead of specifying a logFileName, we provide a LogFilePrefix.  
The trx file name prefix is appended with target framework to generate multi target trx files.  

For example : `dotnet test "--logger:trx;LogFilePrefix=results"` will generate  
&nbsp;&nbsp;&nbsp;&nbsp;UnitTestProject/TestResults/results_net451.trx  
&nbsp;&nbsp;&nbsp;&nbsp;UnitTestProject/TestResults/results_netcoreapp20.trx  

The behavior of  `dotnet test "--logger:trx;LogFileName=results.trx"` will remain same with overwriting being done, and warning to switch to LogFilePrefix.  

When ResultsDirectory is specified, and the solution contains multiple projects with same target,  
For example,  `dotnet test "--logger:trx;LogFilePrefix=results"`  
ResultsDirectory: C:\temp, and the solution contains   
&nbsp;&nbsp;&nbsp;&nbsp; P1 (target net451)   
&nbsp;&nbsp;&nbsp;&nbsp; P2 (target net451)   

This case will overrride different project results as trx filename will be `C:\temp\results_net451.trx` in both cases.  

### Option 4: Specify LogFilePrefix Parameter with target framework and timestamp appended

Introduce a LogFilePrefix parameter in trx logger. Example : `dotnet test "--logger:trx;LogFilePrefix=results"`  
With this, if we want all trx files in a multi target project, instead of specifying a logFileName, we provide a LogFilePrefix.  
The trx file name prefix is appended with target framework to generate multi target trx files.  

For example : `dotnet test "--logger:trx;LogFilePrefix=results"` will generate  
&nbsp;&nbsp;&nbsp;&nbsp;UnitTestProject/TestResults/results_net451_2018_12-24_14-01-07-176.trx  
&nbsp;&nbsp;&nbsp;&nbsp;UnitTestProject/TestResults/results_netcoreapp20_2018-12-24_14-01-08-111.trx  

When ResultsDirectory is specified, and the solution contains multiple projects with same target,  
For example,  `dotnet test "--logger:trx;LogFilePrefix=results"`  
ResultsDirectory: C:\temp, and the solution contains
&nbsp;&nbsp;&nbsp;&nbsp; P1 (target net451)  
&nbsp;&nbsp;&nbsp;&nbsp; P2 (target net451)  

This case will not override results as along with framework, we will append the timestamp.  
&nbsp;&nbsp;&nbsp;&nbsp; C:\temp\results_net451_2018_12-24_14-01-07-176.trx  
&nbsp;&nbsp;&nbsp;&nbsp; C:\temp\results_net451_2018-12-24_14-01-08-111.trx  

## Approach Taken

Option 4 : This generates unique test results for different frameworks and different projects. 
Introduced a LogFilePrefix parameter in trx logger. 
Example : `dotnet test "--logger:trx;LogFilePrefix=results"`. 
 
For example : `dotnet test "--logger:trx;LogFilePrefix=results"` will generate  
&nbsp;&nbsp;&nbsp;&nbsp;UnitTestProject/TestResults/results_net451_2018_12-24_14-01-07-176.trx  
&nbsp;&nbsp;&nbsp;&nbsp;UnitTestProject/TestResults/results_netcoreapp20_2018-12-24_14-01-08-111.trx 

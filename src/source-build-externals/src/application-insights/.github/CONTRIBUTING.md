# How to Contribute

- Please read the general [contributor's guide](https://github.com/Microsoft/ApplicationInsights-Home/blob/master/CONTRIBUTING.md) located in the ApplicationInsights-Home repository 
- If making a large change we request that you open an [issue](https://github.com/Microsoft/ApplicationInsights-dotnet/issues) first. 
- We follow the [Git Flow](http://nvie.com/posts/a-successful-git-branching-model/) approach to branching. 
- This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.


## Solutions

- Everything.sln - this will build all projects and unit tests.
- ProjectsForSigning.sln - this builds all shipping projects.
- IntegrationTests.sln - this builds all Net Core Integration tests.
- BASE\Microsoft.ApplicationInsights.sln - this builds the Base SDK and ServerTelemetryChannel.
- WEB\Microsoft.ApplicationInsights.Web.sln - this builds the ASP.Net projects.
- WEB\dirs.proj - this builds the functional tests which rely on docker.
- NETCORE\ApplicationInsights.AspNetCore.sln - this builds the .Net Core projects.
- LOGGING\Logging.sln - this builds the logging adapters.



## Build

To successfully build the sources on your machine, make sure you've installed the following prerequisites:
- Visual Studio 2019 Community, Professional or Enterprise
- .NET SDKs (https://dotnet.microsoft.com/download)
    - .NET 4.8
	- .NET Core 3.1 SDK 



## Unit Tests

Unit tests can be run in either the Visual Studio Test Exploror or via command line `dotnet test`.

## Functional Tests
It is recommended to rely on unit tests to test functionalities wherever possible. For doing end-to-end validation, functional tests exists for all the modules. Unless doing significant changes,
it is not required to run func-tests locally. All the tests, including functional tests, are automatically run when a PR is submitted.

These tests works like described below:

Functional tests contain test apps which refers to the product dlls from the local build. Tests deploy the Test apps to IIS/Docker and http requests are fired against it to trigger various scenarios.
Tests apps are modified to send telemetry to a fake ingestion endpoint controlled by tests. Tests then validate the telemetry received by this endpoint.

### Pre-requisites:

To execute the functional tests, you need to install some additional prerequisites:

- For **Web** and **PerformanceCollector** tests IIS Express should be installed.
		
- For **Dependency Collector**, you need to install Docker for windows as these tests need several additional dependencies to be deployed like SQL Server and Azure Emulator. 
		Docker for Windows (https://docs.docker.com/docker-for-windows/install/). 		
		After installation switch Docker engine to Windows Containers.(https://blogs.msdn.microsoft.com/webdev/2017/09/07/getting-started-with-windows-containers/)
		And finally, make sure you can run ```docker run hello-world``` successfully to confirm that your machine is Docker ready.
				
### Running functional tests:

Before running the functional tests, the product code should be built following 'Build' instructions above.

After building, open the respective solutions from locations given below, build the solution. Tests will appear in Visual Studio Test Explorer and can be run from there.

The following solutions contains the functional tests for various features.

"\Test\Web\FunctionalTests.sln" -- Functional tests using apps onboarded with the nuget Microsoft.ApplicationInsights.Web
Helper script to build product and run all tests in this solution - ```runFunctionalTestsWeb```

"..\bin\Debug\Test\Web\FunctionalTests" -- Binary location for Test and Test apps.

"\Test\PerformanceCollector\FunctionalTests.sln" -- Functional tests using apps onboarded with the nuget Microsoft.ApplicationInsights.PerfCounterCollector
Helper script to build product and run all tests in this solution - ```runFunctionalTestsPerfCollectorAndQuickPulse```

"..\bin\Debug\Test\PerformanceCollector\FunctionalTests" -- Binary location for Test and Test apps.

"\Test\E2ETests\DependencyCollectionTests.sln" -- Functional tests using apps onboarded with the nuget Microsoft.ApplicationInsights.DependencyCollector
Helper script to build product and run all tests in this solution - ```runFunctionalTestsDependencyCollector```

"..bin\Debug\Test\E2ETests" -- Binary location for Test and Test apps.

### Special Notes regarding DependencyCollectionTests
1. All Docker images are downloaded from internet when ran for first time and this could take several minutes (depends on network speed as **around 20GB will be downloaded on first time on a machine**.). Tests may appear hung during this time. 
2. If using Visual Studio Test Explorer to run tests, group the tests by namespace and run each namespaces separately to avoid test conflicts. ```runFunctionalTestsDependencyCollector``` does this automatically.


Edit the helper scripts to change between 'Release' and 'Debug' as per your build.
```runAllFunctionalTests.cmd``` script builds the product and runs all the above functional tests.

Its is important to note that functional tests do not trigger product code build, so explicit build of product code is required before running functional tests.
A typical work flow would be make-produce-change followed by build-product followed by build-functest-solution and finally run-func-tests. (This helpers scripts does this.)

### Known issues/workarounds with running functional tests.

If any tests fail, please retry first to see if it helps. If not, try one of the known issues below. 

If these don't help, please open an [issue](https://github.com/Microsoft/ApplicationInsights-dotnet/issues) in Github describing the problem.

-  Tests fail with error like "It was not possible to find any compatible framework version The specified framework 'Microsoft.NETCore.App', version '1.0.4' was not found"

Workaround: Install .NET Core SDK 1.1.7.

-  Web and PerformanceCollector fails with error related to 'Port conflicts' - its possible that some prior tests has not released ports. 
	
Workaround: Kill all running IISExpress processes and re-run tests.
	
- All/many functional tests fail with error "Incorrect number of items. Expected: 1 Received: 0" when ran from Visual Studio IDE. 
Look for warnings in Visual Studio output window which contains errors like 'Unable to copy dll file due to file being locked..' etc. 

Workarounds: 
1. Close all instances of Visual Studio/cmd windows where scripts were run and retry.
2. One can use advanced tools like 'process explorer' to find out which process is locking files. Kill the process and retry.
3. Delete bin folder from repository root and rebuild. 
4. Restart machine if none of the above helps. 

- Dependency Collector functional tests fail with messages like "Assert.AreEqual failed. Expected:<1>. Actual<0>." or "All apps are not healthy", then its likely that Docker installation has some issues.
	
Workaround if you are trying first time - Make sure you can run ```docker run hello-world``` successfully to confirm that your machine is Docker ready. Also, the very first time DependencyCollector tests are run, all Docker images are downloaded from web and this could potentially take an hour or so. This is only one time per machine.	
Alternate workaround if you have previously run the tests successfully at least once - execute the ```dockercleanup.ps1``` from repository root to cleanup any containers from prior runs.

- All DependencyCollectionTests fail at initialization stage itself with error 'All apps are not healthy'. In the logs you'll find that Docker container has exited with some error codes. Eg: "Exited (2147943452) 53 seconds ago".

If this error occurs execute ```dockercleanup.ps1``` from repository root, and re-run the tests.

The test code intentionally does not clean up the containers it spun up. This is to enable fast re-runs of the tests. If the Test App code is changed, then Docker-Compose will detect it, and re-build the container.
If you want to do clean up all the containers created by the test, execute the ```dockercleanup.ps1``` from repository root. This is typically required if tests were aborted in the middle of a run for some reason.

After retrying, it tests still fail, please clear the binaries folder and rebuild the product solution and test solution and run the tests again.



## Debugging the functional tests.
It is important to note that since the test application is deployed as a separate process/container, debugging the tests itself will not help debug the application code. A debugger need to be attached
to the process hosting the Application, IISExpress or IIS, after deploying the application there.

The test apps refers to the Web SDK assemblies from your local build. After making the changes to product code, build locally (from Visual Studio or using ```buildDebug.cmd```). Then build and start the test application from its publish folder in either IISExpress or IIS, and attach debugger to it. Open the .cs file you want your breakpoint in and set it. Now triggering a request to the application will hit the breakpoint.
The exact request to be triggered depends on what you are doing. If investigating functional test failures locally, then the tests logs should contain the url it hit to trigger scenarios.

Dependency Collector tests deploy the test apps, along with dependencies (Fake Ingestion, SQL etc) to Docker containers inside same Docker virtual network, so that apps can access the dependencies with their names. However, if the test apps are deployed to IIS or IISExpress, then they are outside the Docker virtual network of dependencies, and so it won't be able to access dependencies without using their IP Address. This is a Docker for windows limitation, and could be fixed in future.
Until then, the test app need to address the dependencies using their IP Address. Instead of manually finding IP Addresses and replacing containers names with IP Address, its easy to just run the following script.
This uses Docker commands to determine the IP Addresses, and replaces them in the necessary configs.
"<repo-root>\bin\Debug\Test\E2ETests\E2ETests\replacecontainernamewithip.ps1"

Following pre-requisite is needed to deploy to IIS locally.
* IIS (Make sure Internet Information Services > World Wide Web Services > Application Development Features > ASP.NET 4.6 is enabled)


## Debugging the SDK in general (How to test Application Insights from local build in any Test App)

* Build the project using ```buildDebug.cmd```. If you build using the solution (*.sln) all required depenencies will also be built.
* If the build was successful, you'll find that it generated NuGet packages in "<repository root>\..\bin\Debug\NuGet". You can set this directory as a NuGet Repository to consume within your applications.
* Create a web application project to test the SDK on, and install the Microsoft.ApplicationInsights.Web NuGet package from the above directory.
* From your web application, open the .cs file you want your breakpoint in and set it
* Run your web application. Your breakpoints should be hit now when your web application triggers them.
